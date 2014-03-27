using System;
using System.Xml;
using System.Xml.Serialization;

using FacialDetection.Imaging;
using FacialDetection.Vision;

namespace FacialDetection.CascadeNamespace
{
    /// <summary>
    /// Class to handle the Stages for each cascade
    /// </summary>
    [Serializable]
    [XmlRoot("_")]
    public class Stage
    {
        // Useful stage features utilizing XML formats
        [XmlArray("trees")]
        [XmlArrayItem("_")]
        [XmlArrayItem("_", NestingLevel = 1)]
        public FeatureNode[][] nodes { get; set; }
       
        [XmlElement("stage_threshold")]
        public double threshold { get; set; }

        [XmlElement("parent")]
        public int parent { get; set; }

        [XmlElement("next")]
        public int next { get; set; }

        /// <summary>
        /// Constructs a new stage
        /// </summary>
        public Stage()
        {
        }

        /// <summary>
        /// Constructs a new stage
        /// </summary>
        public Stage(double pThreshold)
        {
            this.threshold = pThreshold;
        }

        /// <summary>
        /// Constructs a new stage
        /// </summary>
        public Stage(double pThreshold, int pParent, int pNext)
        {
            this.threshold = pThreshold;
            this.parent = pParent;
            this.next = pNext;
        }

        /// <summary>
        ///   Classifies an image as having the searched object or not.
        /// </summary>
        public bool Classify(ImageHelper pImage, int row, int column, double pFactor)
        {
            // Keep track of how close to the threshold we are.
            double achievedThreshold = 0;

            // For each feature in the feature tree of the current stage,
            foreach (FeatureNode[] node in nodes)
            {
                int current = 0;
                do
                {
                    // Get the feature node from the current branch in the node tree
                    FeatureNode feature = node[current];

                    // Evaluate the node's feature
                    double sum = feature.mFeature.GetSum(pImage, row, column);

                    // And increase the value accumulator
                    if (sum < feature.mThreshold * pFactor)
                    {
                        achievedThreshold += feature.mLeftValue;
                        current = feature.mLeft;
                    }
                    else
                    {
                        achievedThreshold += feature.mRightValue;
                        current = feature.mRight;
                    }

                } while (current > 0);
            }

            // If the achieved threshold is greater than the desired threshold we have found a face!!
            if (achievedThreshold <= this.threshold)
            {
                // Otherwise our image doesn't contain what we're looking for.
                return false;
            }
            else
            {
                // We had a match!
                return true;
            }
        }

    }

    /// <summary>
    /// Required or loading from an XML file
    /// </summary>
    [Serializable]
    [XmlRoot(Namespace = "", IsNullable = false, ElementName = "stages")]
    public class CascadeSerializer
    {
        /// <summary>
        /// Each stage is retrieved after deserialization
        /// </summary>
        [XmlElement("_")]
        public Stage[] Stages { get; set; }
    }
}
