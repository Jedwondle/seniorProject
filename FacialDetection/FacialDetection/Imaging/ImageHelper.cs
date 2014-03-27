using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace FacialDetection.Imaging
{
    /// <summary>
    /// Joint representation of both Integral Image and Squared Integral Image,
    /// which are important for the image processing. These are also known as
    /// Summed area tables. Which are useful for quickly and efficiently generating
    /// sums of values in a rectangle.
    /// 
    /// This is where we also implement a bit of a tilted feature operation. Doing the math
    /// here makes things simpler than doing them when we're checking each classifier...
    /// 
    /// Inspired by AForge IntegralImage2
    /// </summary>
    public unsafe class ImageHelper : IDisposable
    {

        private int[,] integralImage; // normal  integral image
        private int[,] squaredIntImage; // squared integral image
        private int[,] tiltedIntImage; // tilted  integral image

        private int* intImage; // normal  integral image
        private int* squaredImage; // squared integral image
        private int* tiltedImage; // tilted  integral image

        // Required in order to manage unmanaged resources
        private GCHandle intPointer;
        private GCHandle squaredPointer;
        private GCHandle tiltedPointer;


        // image dimensions
        private int iWidth;
        private int iHeight;

        private int nWidth;
        private int nHeight;

        private int tWidth;
        private int tHeight;


        // Getters and Setters for member variables
        public int Width
        {
            get { return iWidth; }
        }
        public int Height
        {
            get { return iHeight; }
        }
        public int[,] Image
        {
            get { return integralImage; }
        }
        public int[,] Squared
        {
            get { return squaredIntImage; }
        }
        public int[,] Rotated
        {
            get { return tiltedIntImage; }
        }

        /// <summary>
        /// imageHelper Constructor passing in width and height and whether we want to tilt the feature
        /// </summary>
        /// <param name="pWidth"></param>
        /// <param name="pHeight"></param>
        /// <param name="pComputeTilted"></param>
        protected ImageHelper(int pWidth, int pHeight, bool pComputeTilted)
        {
            // figure out the width and height for each
            this.iWidth = pWidth;
            this.iHeight = pHeight;

            this.nWidth = pWidth + 1;
            this.nHeight = pHeight + 1;

            this.tWidth = pWidth + 2;
            this.tHeight = pHeight + 2;


            // grab the image and create a GCHandle pointer to the memory for that image
            this.integralImage = new int[nHeight, nWidth];
            this.intPointer = GCHandle.Alloc(integralImage, GCHandleType.Pinned);
            this.intImage = (int*)intPointer.AddrOfPinnedObject().ToPointer();

            this.squaredIntImage = new int[nHeight, nWidth];
            this.squaredPointer = GCHandle.Alloc(squaredIntImage, GCHandleType.Pinned);
            this.squaredImage = (int*)squaredPointer.AddrOfPinnedObject().ToPointer();

            if (pComputeTilted)
            {
                this.tiltedIntImage = new int[tHeight, tWidth];
                this.tiltedPointer = GCHandle.Alloc(tiltedIntImage, GCHandleType.Pinned);
                this.tiltedImage = (int*)tiltedPointer.AddrOfPinnedObject().ToPointer();
            }
        }

        /// <summary>
        /// Constructor from a bitmap and the channel
        /// </summary>
        /// <param name="image"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public static ImageHelper FromBitmap(Bitmap image, int channel)
        {
            return FromBitmap(image, channel, false);
        }

        /// <summary>
        /// Constructor passing in a bitmap, channel, and whether we're computing the tilted image
        /// </summary>
        /// <param name="pImage"></param>
        /// <param name="pChannel"></param>
        /// <param name="pComputeTilted"></param>
        /// <returns></returns>
        public static ImageHelper FromBitmap(Bitmap pImage, int pChannel, bool pComputeTilted)
        {
            // check image format, if it isn't grayscale or 24 bpp RGB we will have issues
            if (!(pImage.PixelFormat == PixelFormat.Format8bppIndexed ||
                pImage.PixelFormat == PixelFormat.Format24bppRgb ||
                pImage.PixelFormat == PixelFormat.Format32bppArgb))
            {
                throw new AForge.Imaging.UnsupportedImageFormatException("Only grayscale and 24 bpp RGB images are supported.");
            }

            // lock source image
            BitmapData imageData = pImage.LockBits(
                new Rectangle(0, 0, pImage.Width, pImage.Height),
                ImageLockMode.ReadOnly, pImage.PixelFormat);

            // process the image
            ImageHelper im = FromBitmap(imageData, pChannel, pComputeTilted);

            // unlock image
            pImage.UnlockBits(imageData);

            return im;
        }

        /// <summary>
        /// Constructor passing in BitmapData and a channel
        /// </summary>
        /// <param name="pImageData"></param>
        /// <param name="pChannel"></param>
        /// <returns></returns>
        public static ImageHelper FromBitmap(BitmapData pImageData, int pChannel)
        {
            return FromBitmap(new AForge.Imaging.UnmanagedImage(pImageData), pChannel);
        }

        /// <summary>
        /// Constructor passing in BitmapData a channel, and the flag to compute a tilted image
        /// </summary>
        /// <param name="pImageData"></param>
        /// <param name="pChannel"></param>
        /// <param name="pComputeTilted"></param>
        /// <returns></returns>
        public static ImageHelper FromBitmap(BitmapData pImageData, int pChannel, bool pComputeTilted)
        {
            return FromBitmap(new AForge.Imaging.UnmanagedImage(pImageData), pChannel, pComputeTilted);
        }

        /// <summary>
        /// Constructor passing in an unmanaged image, and the channel
        /// </summary>
        /// <param name="pImage"></param>
        /// <param name="pChannel"></param>
        /// <returns></returns>
        public static ImageHelper FromBitmap(AForge.Imaging.UnmanagedImage pImage, int pChannel)
        {
            return FromBitmap(pImage, pChannel, false);
        }

        /// <summary>
        /// Constructor passing in an unmanaged image, a channel, and the flag to compute a tilted image
        /// </summary>
        /// <param name="pImage"></param>
        /// <param name="pChannel"></param>
        /// <param name="pComputeTilted"></param>
        /// <returns></returns>
        public static ImageHelper FromBitmap(AForge.Imaging.UnmanagedImage pImage, int pChannel, bool pComputeTilted)
        {
            // check image format, we'll have the same issue with bitmaps here as we did before
            if (!(pImage.PixelFormat == PixelFormat.Format8bppIndexed ||
                pImage.PixelFormat == PixelFormat.Format24bppRgb ||
                pImage.PixelFormat == PixelFormat.Format32bppArgb))
            {
                throw new AForge.Imaging.UnsupportedImageFormatException("Only grayscale and 24 bpp RGB images are supported.");
            }

            // find the pixel size
            int pixelSize = System.Drawing.Image.GetPixelFormatSize(pImage.PixelFormat) / 8;

            // get source image size
            int pWidth = pImage.Width;
            int pHeight = pImage.Height;
            int pStride = pImage.Stride;
            int pOffset = pStride - pWidth * pixelSize;

            // create integral image
            ImageHelper im = new ImageHelper(pWidth, pHeight, pComputeTilted);
            int* nSum = im.intImage, sSum = im.squaredImage, tSum = im.tiltedImage;

            // compute the adjusted widths and heights for the other two operations
            int nWidth = im.nWidth, nHeight = im.nHeight;
            int tWidth = im.tWidth, tHeight = im.tHeight;


            // We're going to have an issue if the channel doesn't support 8 bpp
            if (pImage.PixelFormat == PixelFormat.Format8bppIndexed && pChannel != 0)
                throw new ArgumentException("Only the first channel is available for 8 bpp images.", "channel");

            // grab the image pointer src
            byte* srcStart = (byte*)pImage.ImageData.ToPointer() + pChannel;

            // do the job
            byte* src = srcStart;

            // for each line
            for (int y = 1; y <= pHeight; y++)
            {
                int yy = nWidth * (y);
                int y1 = nWidth * (y - 1);

                // for each pixel
                for (int x = 1; x <= pWidth; x++, src += pixelSize)
                {
                    // set p1 to the src
                    // and square it for p2
                    int p1 = *src;
                    int p2 = p1 * p1;

                    // find the indexes for each sum
                    int r = yy + (x);
                    int a = yy + (x - 1);
                    int b = y1 + (x);
                    int c = y1 + (x - 1);

                    // and sum them up!
                    nSum[r] = p1 + nSum[a] + nSum[b] - nSum[c];
                    sSum[r] = p2 + sSum[a] + sSum[b] - sSum[c];
                }
                src += pOffset;
            }

            // if we're computing the tilted image as well, there is a lot more going on
            if (pComputeTilted)
            {
                // grab the source
                src = srcStart;

                // Left-to-right, top-to-bottom pass
                for (int y = 1; y <= pHeight; y++, src += pOffset)
                {
                    // compute the new yy and y1
                    int yy = tWidth * (y);
                    int y1 = tWidth * (y - 1);

                    for (int x = 2; x < pWidth + 2; x++, src += pixelSize)
                    {
                        // find the indexes
                        int a = y1 + (x - 1);
                        int b = yy + (x - 1);
                        int c = y1 + (x - 2);
                        int r = yy + (x);

                        // and sum them up
                        tSum[r] = *src + tSum[a] + tSum[b] - tSum[c];
                    }
                }

                {
                    int yy = tWidth * (pHeight);
                    int y1 = tWidth * (pHeight + 1);

                    for (int x = 2; x < pWidth + 2; x++, src += pixelSize)
                    {
                        int a = yy + (x - 1);
                        int c = yy + (x - 2);
                        int b = y1 + (x - 1);
                        int r = y1 + (x);

                        tSum[r] = tSum[a] + tSum[b] - tSum[c];
                    }
                }


                // Now do it Right-to-left, and bottom-to-top
                for (int y = pHeight; y >= 0; y--)
                {
                    // compute the new values
                    int yy = tWidth * (y);
                    int y1 = tWidth * (y + 1);

                    for (int x = pWidth + 1; x >= 1; x--)
                    {
                        // grab the indexes
                        int r = yy + (x);
                        int b = y1 + (x - 1);

                        // and add them up
                        tSum[r] += tSum[b];
                    }
                }

                for (int y = pHeight + 1; y >= 0; y--)
                {
                    // grab the new value
                    int yy = tWidth * (y);

                    for (int x = pWidth + 1; x >= 2; x--)
                    {
                        // compute the indexes
                        int r = yy + (x);
                        int b = yy + (x - 2);

                        // and add them up
                        tSum[r] -= tSum[b];
                    }
                }
            }

            // finally return the restult... All the math should be done by this point
            return im;
        }

        /// <summary>
        /// Get the integral image sum from coordinates x, y, and a width and height provided
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public int GetSum(int x, int y, int width, int height)
        {
            int a = nWidth * (y) + (x);
            int b = nWidth * (y + height) + (x + width);
            int c = nWidth * (y + height) + (x);
            int d = nWidth * (y) + (x + width);

            return intImage[a] + intImage[b] - intImage[c] - intImage[d];
        }

        /// <summary>
        /// Get the squared sum from coordinates x, y, and a width and height provided
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public int GetSum2(int x, int y, int width, int height)
        {
            int a = nWidth * (y) + (x);
            int b = nWidth * (y + height) + (x + width);
            int c = nWidth * (y + height) + (x);
            int d = nWidth * (y) + (x + width);

            return squaredImage[a] + squaredImage[b] - squaredImage[c] - squaredImage[d];
        }

        /// <summary>
        /// Get the tilted image sum from coordinates x, y, and a width and height provided
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public int GetSumT(int x, int y, int width, int height)
        {
            int a = tWidth * (y + width) + (x + width + 1);
            int b = tWidth * (y + height) + (x - height + 1);
            int c = tWidth * (y) + (x + 1);
            int d = tWidth * (y + width + height) + (x + width - height + 1);

            return tiltedImage[a] + tiltedImage[b] - tiltedImage[c] - tiltedImage[d];
        }



        #region IDisposable Members

        /// <summary>
        /// Required for iDisposable objects in order to manage resetting or releasing unmanaged resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Release the unmanaged resource
        /// </summary>
        ~ImageHelper()
        {
            Dispose(false);
        }

        /// <summary>
        /// Release unmanaged (or managed) resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed 
        /// and unmanaged resources; <c>false</c> to release only unmanaged
        /// resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                // (i.e. IDisposable objects)
            }

            // free native resources
            if (intPointer.IsAllocated)
            {
                intPointer.Free();
                intImage = null;
            }
            if (squaredPointer.IsAllocated)
            {
                squaredPointer.Free();
                squaredImage = null;
            }
            if (tiltedPointer.IsAllocated)
            {
                tiltedPointer.Free();
                tiltedImage = null;
            }
        }

        #endregion

    }
}