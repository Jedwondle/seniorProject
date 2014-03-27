using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Concurrent;
using System.IO;

using FacialDetection.Imaging;
using FacialDetection.CascadeNamespace;


namespace FacialDetection.Vision
{
    /// <summary>
    /// Enumeration of object search modes
    /// </summary>
    public enum searchMode
    {
        // Default = 0 means the whole image will be scanned.
        Default = 0,

        // Single means only one object will be scanned
        Single,

        // NoOverlap means that if an object has been detected, the space won't be searched twice for overlapping objects.
        NoOverlap,

        // TODO: Add some options for maximum number of faces before early quit
    }

    /// <summary>
    /// Enumeration of object scaling modes
    /// </summary>
    public enum scalingMode
    {
        // Starts with a large search window and moves to smaller ones.
        GreaterToSmaller,

        // Starts with a small search window and moves to larger ones.
        SmallerToGreater,
    }

    /// <summary>
    /// Detector class that utilizes haar-feature cascading
    /// </summary>
    public class Detector : DetectorInterface
    {
        // list of detected objects
        private List<Rectangle> detectedObjects;

        // The haar classifier object used to match patterns against
        private Classifier classifier;

        // configurations needed for searching
        private searchMode searchMode = searchMode.NoOverlap;
        private scalingMode scalingMode = scalingMode.GreaterToSmaller;

        // used to configure search some more
        private Size minSize = new Size(20, 20);      // we're saying a face can't get smaller than this
        private Size maxSize = new Size(500, 500);    // we're saying a face can't get bigger than this
        private float factor = 1.2f;                  // used for the classifier
        private int channel = RGBA.R;                 // looking at red

        private Rectangle[] lastFound;             
        private int steadyThreshold = 2;             // threshold used for face detection

        private int mWidth;                       // set up as initial face width
        private int mHeight;                      // set up as initial face height

        private int lastWidth;                       // the final face width
        private int lastHeight;                      // the final face height
        private float[] steps;                       // the number of steps to find a face

        #region Getters/Setters
        // Get or Set to use threads
        // not implemented yet...
        public bool useThreads { get; set; }

        // Get or Set the minSize
        public Size MinSize
        {
            get { return minSize; }
            set { minSize = value; }
        }

        // Get or Set the maxSize
        public Size MaxSize
        {
            get { return maxSize; }
            set { maxSize = value; }
        }

        // Get or Set the color channel 
        public int Channel
        {
            get { return channel; }
            set { channel = value; }
        }

        // Get or Set the scaling factor
        public float ScalingFactor
        {
            get { return factor; }
            set
            {
                if (value != factor)
                {
                    factor = value;
                    steps = null;
                }
            }
        }

        // Get or Set the searchMode
        public searchMode SearchMode
        {
            get { return searchMode; }
            set { searchMode = value; }
        }

        // Get or Set the scalingMode
        public scalingMode ScalingMode
        {
            get { return scalingMode; }
            set
            {
                if (value != scalingMode)
                {
                    scalingMode = value;
                    steps = null;
                }
            }
        }

        // Gets detectedObjects.
        public Rectangle[] DetectedObjects
        {
            get { return detectedObjects.ToArray(); }
        }

        // Get the Cascade Classifier
        public Classifier Classifier
        {
            get { return classifier; }
        }

        // Get how many frames the object has been detected in a steady position
        public int Steady { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor taking a Cascade
        /// </summary>
        /// <param name="cascade"></param>
        public Detector(Cascade cascade)
            : this(cascade, 20) { }

        /// <summary>
        /// Constructor taking a cascade object and a minimum size
        /// </summary>
        /// <param name="cascade"></param>
        /// <param name="minSize"></param>
        public Detector(Cascade cascade, int minSize)
            : this(cascade, minSize, searchMode.NoOverlap) { }

        /// <summary>
        /// Constructor taking a cascade, minimum size, and searchMode
        /// </summary>
        /// <param name="cascade"></param>
        /// <param name="minSize"></param>
        /// <param name="searchMode"></param>
        public Detector(Cascade cascade, int minSize, searchMode searchMode)
            : this(cascade, minSize, searchMode, 1.2f) { }

        /// <summary>
        /// Construcctor taking a cascade, minSize, searchMode, and scaleFactor
        /// </summary>
        /// <param name="cascade"></param>
        /// <param name="minSize"></param>
        /// <param name="searchMode"></param>
        /// <param name="scaleFactor"></param>
        public Detector(Cascade cascade, int minSize, searchMode searchMode, float scaleFactor)
            : this(cascade, minSize, searchMode, scaleFactor, scalingMode.SmallerToGreater) { }

        /// <summary>
        /// Constructor taking a cascade, minSize, searchMode, scaleFactor, and scalingMode
        /// </summary>
        /// <param name="cascade"></param>
        /// <param name="minSize"></param>
        /// <param name="searchMode"></param>
        /// <param name="scaleFactor"></param>
        /// <param name="scalingMode"></param>
        public Detector(Cascade cascade, int minSize, searchMode searchMode, float scaleFactor,
            scalingMode scalingMode)
        {
            this.classifier = new Classifier(cascade);
            this.minSize = new Size(minSize, minSize);
            this.searchMode = searchMode;
            this.ScalingMode = scalingMode;
            this.factor = scaleFactor;
            this.detectedObjects = new List<Rectangle>();

            this.mWidth = cascade.width;
            this.mHeight = cascade.height;
        }

        /// <summary>
        /// Constructor taking a cascade, minSize, searchMode, scaleFactor, scalingMode, and maxSize
        /// </summary>
        /// <param name="cascade"></param>
        /// <param name="minSize"></param>
        /// <param name="searchMode"></param>
        /// <param name="scaleFactor"></param>
        /// <param name="scalingMode"></param>
        /// <param name="maxSize"></param>
        public Detector(Cascade cascade, int minSize, searchMode searchMode, float scaleFactor,
            scalingMode scalingMode, int maxSize)
        {
            this.classifier = new Classifier(cascade);
            this.minSize = new Size(minSize, minSize);
            this.maxSize = new Size(maxSize, maxSize);
            this.searchMode = searchMode;
            this.ScalingMode = scalingMode;
            this.factor = scaleFactor;
            this.detectedObjects = new List<Rectangle>();

            this.mWidth = cascade.width;
            this.mHeight = cascade.height;
        }
        #endregion

        /// <summary>
        /// Do object detection on the given frame
        /// </summary>
        /// <param name="pFrame"></param>
        /// <returns></returns>
        public Rectangle[] ProcessFrame(Bitmap pFrame)
        {
            return ProcessFrame(AForge.Imaging.UnmanagedImage.FromManagedImage(pFrame));
        }

        /// <summary>
        /// Do object detection on the given frame
        /// </summary>
        /// <param name="pImage"></param>
        /// <returns></returns>
        public Rectangle[] ProcessFrame(AForge.Imaging.UnmanagedImage pImage)
        {
            // Console.WriteLine("Started processing the frame");

            // Creates an integral image representation of the frame
            ImageHelper imHelper = ImageHelper.FromBitmap(
                pImage, channel, classifier.Cascade.hasTiltedFeatures);

            // Creates a new list of detected objects.
            this.detectedObjects.Clear();

            int pWidth = imHelper.Width;
            int pHeight = imHelper.Height;

            // Update parameters only if different size
            if (steps == null || pWidth != lastWidth || pHeight != lastHeight)
                update(pWidth, pHeight);


            Rectangle pWindow = Rectangle.Empty;

            // For each scaling step
            for (int i = 0; i < steps.Length; i++)
            {
                // Console.WriteLine("Started a new scaling step: " + steps[i]);
                float scaling = steps[i];

                // Set the classifier window scale
                classifier.Scale = scaling;

                // Get the scaled window size
                pWindow.Width = (int)(mWidth * scaling);
                pWindow.Height = (int)(mHeight * scaling);

                // Check if the window is lesser than the minimum size
                if (pWindow.Width < minSize.Width && pWindow.Height < minSize.Height &&
                    pWindow.Width > maxSize.Width && pWindow.Height > maxSize.Height)
                {
                    // If we are searching in greater to smaller mode,
                    if (scalingMode == scalingMode.GreaterToSmaller)
                    {
                        break;
                    }
                    else
                    {
                        continue; // continue until it gets greater.
                    }
                }


                // Grab some scan loop parameters
                int xStep = pWindow.Width >> 3;
                int yStep = pWindow.Height >> 3;

                int xEnd = pWidth - pWindow.Width;
                int yEnd = pHeight - pWindow.Height;

                // TODO: Check if we should run in parallel or sequential
                
                
                // For now... Sequential mode --- Scan the integral image one step at a time

                // For every pixel in the window column
                for (int y = 0; y < yEnd; y += yStep)
                {
                    pWindow.Y = y;
                    
                    // For every pixel in the window row
                    for (int x = 0; x < xEnd; x += xStep)
                    {
                        pWindow.X = x;

                        // We have already detected something here, moving along.
                        if (searchMode == searchMode.NoOverlap && overlaps(pWindow))
                            continue; 

                        // Try to detect and object inside the window
                        if (classifier.Compute(imHelper, pWindow))
                        {
                            //Console.WriteLine("Face detected: {0}, {1} - {2}, {3}", pWindow.X, pWindow.Y, pWindow.X + pWindow.Width, pWindow.Y + pWindow.Height);
                            
                            // an object has been detected
                            detectedObjects.Add(pWindow);

                            // Stop on first object found
                            if (searchMode == searchMode.Single)
                                break; 
                        }
                    }
                }

                
            }
            

            // Create the return object
            Rectangle[] objects = detectedObjects.ToArray();

            // Do the check for steadiness
            checkSteadiness(objects);

            // Save the last found objects
            lastFound = objects;

            // return what we found
            return objects; 
        }

        /// <summary>
        /// Update the detector's parameters passingin a width and height
        /// </summary>
        /// <param name="pWidth"></param>
        /// <param name="pHeight"></param>
        private void update(int pWidth, int pHeight)
        {
            List<float> listSteps = new List<float>();

            // Set initial parameters according to scaling mode
            if (scalingMode == scalingMode.SmallerToGreater)
            {
                float start = 1f;
                float stop = Math.Min(pWidth / (float)mWidth, pHeight / (float)mHeight);
                float step = factor;

                for (float f = start; f < stop; f *= step)
                    listSteps.Add(f);
            }
            // otherwise, update the detector with the new scaled information
            else
            {
                float start = Math.Min(pWidth / (float)mWidth, pHeight / (float)mHeight);
                float stop = 1f;
                float step = 1f / factor;

                for (float f = start; f > stop; f *= step)
                    listSteps.Add(f);
            }

            steps = listSteps.ToArray();

            lastWidth = pWidth;
            lastHeight = pHeight;
        }

        /// <summary>
        /// Check for steadiness of the rectangles, saving the result in the Steady variable
        /// </summary>
        /// <param name="rectangles"></param>
        private void checkSteadiness(Rectangle[] rectangles)
        {
            // if we're missing anything, return without trying
            if (lastFound == null ||
                rectangles == null ||
                rectangles.Length == 0)
            {
                Steady = 0;
                return;
            }

            // otherwise, start here by seting foundSteady to true
            bool foundSteady = true;

            // for each rectangle
            foreach (Rectangle current in rectangles)
            {
                // start by seting found to false
                bool found = false;
                
                // for each rectangle in the history
                foreach (Rectangle last in lastFound)
                {
                    // if they are the same as the current rectangle, and it is past the steady threshold
                    // set found to true, and break from the loop
                    if (current.IsEqual(last, steadyThreshold))
                    {
                        found = true;
                        continue;
                    }
                }

                // if we didn't find a stable region set the foundSteady to false
                if (!found)
                {
                    foundSteady = false;
                    break;
                }
            }

            // if we did find a steady reagion, increment the steady count
            if (foundSteady)
                Steady++;

            else
                Steady = 0;
        }

        /// <summary>
        /// Check to see if a rectangle overlaps with another within the detected objects passing in a rectangle
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        private bool overlaps(Rectangle rect)
        {
            // for each rectangle in our detected objects, if the one we were given interescts with one of them, return true
            foreach (Rectangle r in detectedObjects)
            {
                if (rect.IntersectsWith(r))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Convert the Detector to a displayable string and return this to the caller
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            String result = String.Format("\r\nMinSize: {0}\r\nMaxSize: {1}\r\nScalingFactor: {2}\r\nSteady: {3}\r\nClassifier-height: {4}\r\nClasifier-width: {5}\r\nChannel: {6}\r\n",
                this.MinSize.ToString(), this.MaxSize.ToString(), this.ScalingFactor.ToString(),
                this.Steady.ToString(), this.Classifier.Cascade.height.ToString(), this.Classifier.Cascade.width.ToString(),
                this.Channel.ToString());
            if (this.ScalingMode == scalingMode.SmallerToGreater)
            {
                result += "Scaling Mode: SmallerToGreater\r\n";
            }
            else
            {
                result += "Scaling Mode: GreaterToSmaller\r\n";
            }

            if (this.SearchMode == searchMode.Default)
            {
                result += "Search Mode: Default\r\n";
            }
            else if (this.SearchMode == searchMode.NoOverlap)
            {
                result += "Search Mode: No Overlap\r\n";
            }
            else
            {
                result += "Search Mode: Single\r\n";
            }
            return result;
        }
    }

}
