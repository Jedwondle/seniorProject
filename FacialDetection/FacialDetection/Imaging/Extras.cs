using System;
using System.Drawing;
using System.Drawing.Imaging;

using FacialDetection.Vision;
using FacialDetection.CascadeNamespace;
using System.IO;

namespace FacialDetection.Imaging
{
    /// <summary>
    /// Extra funcitonality I thought I would need
    /// </summary>
    public static class Extras
    {
        /// <summary>
        /// Compares two rectangles for equality, considering an acceptance threshold.
        /// </summary>
        /// <param name="objA"></param>
        /// <param name="objB"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static bool IsEqual(this Rectangle objA, Rectangle objB, int threshold)
        {
            return (Math.Abs(objA.X - objB.X) < threshold) &&
                   (Math.Abs(objA.Y - objB.Y) < threshold) &&
                   (Math.Abs(objA.Width - objB.Width) < threshold) &&
                   (Math.Abs(objA.Height - objB.Height) < threshold);
        }

        /// <summary>
        /// Rotates one point around another
        /// </summary>
        /// <param name="pointToRotate">The point to rotate.</param>
        /// <param name="centerPoint">The centre point of rotation.</param>
        /// <param name="angleInDegrees">The rotation angle in degrees.</param>
        /// <returns>Rotated point</returns>
        public static Rectangle RotatePoint(Rectangle pointToRotate, Point centerPoint, double angleInDegrees, int difX, int difY)
        {
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);

            Point middle = new Point
            {
                X = pointToRotate.X + (pointToRotate.Width / 2),
                Y = pointToRotate.Y + (pointToRotate.Height / 2)
            };


            Point newPoint = new Point
            {
                X =
                    (int)
                    (cosTheta * (middle.X - centerPoint.X) -
                    sinTheta * (middle.Y - centerPoint.Y) + centerPoint.X) - difX,
                Y =
                    (int)
                    (sinTheta * (middle.X - centerPoint.X) +
                    cosTheta * (middle.Y - centerPoint.Y) + centerPoint.Y) - difY
            };


            Rectangle result = new Rectangle
            {
                X = newPoint.X - (pointToRotate.Width / 2),
                Y = newPoint.Y - (pointToRotate.Height / 2),
                Width = pointToRotate.Width,
                Height = pointToRotate.Height,
            };

            /// TODO:: Adjust this point by the width and height according to which quadrent it was found in
            /// fo fix the issues with rotation (currently the point is right in the middle
            //if (angleInDegrees > 90)
            //{
            //    result.X = result.X + 
            //}


            return result;
        }

        /// <summary>
        /// Rotate an image by the angle and return a new image to check for faces from.
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="angle"></param>
        /// <param name="bkColor"></param>
        /// <returns></returns>
        public static Bitmap RotateImg(Bitmap bmp, float angle, Color bkColor)
        {
            angle = angle % 360;
            if (angle > 180)
                angle -= 360;

            System.Drawing.Imaging.PixelFormat pf = default(System.Drawing.Imaging.PixelFormat);
            if (bkColor == Color.Transparent)
            {
                pf = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
            }
            else
            {
                pf = bmp.PixelFormat;
            }

            float sin = (float)Math.Abs(Math.Sin(angle * Math.PI / 180.0)); // this function takes radians
            float cos = (float)Math.Abs(Math.Cos(angle * Math.PI / 180.0)); // this one too
            float newImgWidth = sin * bmp.Height + cos * bmp.Width;
            float newImgHeight = sin * bmp.Width + cos * bmp.Height;
            float originX = 0f;
            float originY = 0f;

            if (angle > 0)
            {
                if (angle <= 90)
                    originX = sin * bmp.Height;
                else
                {
                    originX = newImgWidth;
                    originY = newImgHeight - sin * bmp.Width;
                }
            }
            else
            {
                if (angle >= -90)
                    originY = sin * bmp.Width;
                else
                {
                    originX = newImgWidth - sin * bmp.Height;
                    originY = newImgHeight;
                }
            }

            Bitmap newImg = new Bitmap((int)newImgWidth, (int)newImgHeight, pf);
            Graphics g = Graphics.FromImage(newImg);
            g.Clear(bkColor);
            g.TranslateTransform(originX, originY); // offset the origin to our calculated values
            g.RotateTransform(angle); // set up rotate
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(bmp, 0, 0); // draw the image at 0, 0
            g.Dispose();

            return newImg;
        }


        /// <summary>
        /// Completing requirement 3 (Creating the byte array to display)
        /// </summary>
        /// <param name="imageIn"></param>
        /// <returns></returns>
        public static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        /// <summary>
        /// Useful for getting an image from any other byte array made
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
    }
}