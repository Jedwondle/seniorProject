using System;
using System.Drawing;

using FacialDetection.Imaging;
using FacialDetection.Vision;

namespace FacialDetection.CascadeNamespace
{

    /// <summary>
    /// This is what makes haarCascades work.  We come up with Strong Classifiers based on weaker ones based on the haar-feature rectangles (See Viola–Jones object detection framework)
    /// </summary>
    [Serializable]
    public class Classifier
    {
        // Important member variables
        private Cascade cascade;
        private float invArea;
        private float scale;

        /// <summary>
        /// Constructor for a Classifier taking a cascade
        /// </summary>
        /// <param name="pCascade"></param>
        public Classifier(Cascade pCascade)
        {
            this.cascade = pCascade;
        }

        /// <summary>
        /// Constructor for a Classifier taking a width height and stages (to create a cascade with)
        /// </summary>
        /// <param name="pWidth"></param>
        /// <param name="pHeight"></param>
        /// <param name="pStages"></param>
        public Classifier(int pWidth, int pHeight, Stage[] pStages)
            : this(new Cascade(pWidth, pHeight, pStages))
        {
        }

        /// <summary>
        /// Get the cascade from the classifier
        /// </summary>
        public Cascade Cascade
        {
            get { return cascade; }
        }

        /// <summary>
        /// Get/Set Scale of the window being used to analize the image
        /// </summary>
        public float Scale
        {
            get { return this.scale; }
            set
            {
                if (this.scale == value)
                    return;

                this.scale = value;
                this.invArea = 1f / (cascade.width * cascade.height * scale * scale);

                foreach (Stage stage in cascade.stages)
                {
                    foreach (FeatureNode[] node in stage.nodes)
                    {
                        foreach (FeatureNode current in node)
                        {
                            // Set the scale and weight for the current feature
                            current.mFeature.SetScaleAndWeight(value, invArea);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// This is where we do the detection for an image passing in the interest window
        /// </summary>
        /// <param name="pImage"></param>
        /// <param name="pWindow"></param>
        /// <returns></returns>
        public bool Compute(ImageHelper pImage, Rectangle pWindow)
        {
            // grab the dimensions
            int x = pWindow.X;
            int y = pWindow.Y;
            int w = pWindow.Width;
            int h = pWindow.Height;

            // figure out the details of the window in the image
            double mean = pImage.GetSum(x, y, w, h) * invArea;
            double factor = pImage.GetSum2(x, y, w, h) * invArea - (mean * mean);

            // do the math to adjust the factor
            factor = (factor >= 0) ? Math.Sqrt(factor) : 1;


            // For each classification stage in the cascade
            foreach (Stage stage in cascade.stages)
            {
                // Check to see if the image was rejected
                if (stage.Classify(pImage, x, y, factor) == false)
                {
                    return false; // If it was, we tell the caller that we were rejected.
                }
            }

            // If we've gone through all the stages and were not rejected, the object was detected inside the window
            return true;
        }


    }
}