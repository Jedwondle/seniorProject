using System;
using System.Xml;
using System.Xml.Serialization;

using FacialDetection.Vision;

namespace FacialDetection.CascadeNamespace
{
    /// <summary>
    /// Feature Node that handles tree of classifiers in the cascade
    /// </summary>
    [Serializable]
    public class FeatureNode : ICloneable
    {
        // member variables helpful for navigating the tree
        private int rightNode = -1;
        private int leftNode = -1;

        // Getters and Setters
        [XmlElement("threshold")]
        public double mThreshold { get; set; }
        [XmlElement("left_val")]
        public double mLeftValue { get; set; }
        [XmlElement("right_val")]
        public double mRightValue { get; set; }
        [XmlElement("left_node")]
        public int mLeft
        {
            get { return leftNode; }
            set { leftNode = value; }
        }
        [XmlElement("right_node")]
        public int mRight
        {
            get { return rightNode; }
            set { rightNode = value; }
        }
        [XmlElement("feature", IsNullable = false)]
        public Feature mFeature { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public FeatureNode()
        {
        }

        /// <summary>
        /// Constructor given a threshold, left value, right value, and rectangles
        /// </summary>
        /// <param name="pThreshold"></param>
        /// <param name="pLeft"></param>
        /// <param name="pRight"></param>
        /// <param name="pRectangles"></param>
        public FeatureNode(double pThreshold, double pLeft, double pRight, params int[][] pRectangles)
            : this(pThreshold, pLeft, pRight, false, pRectangles)
        {
        }

        /// <summary>
        /// Constructor given a threshold, left value, right value, tilted flag, and rectangles
        /// </summary>
        /// <param name="pThreshold"></param>
        /// <param name="pLeft"></param>
        /// <param name="pRight"></param>
        /// <param name="pTilted"></param>
        /// <param name="pRectangles"></param>
        public FeatureNode(double pThreshold, double pLeft, double pRight, bool pTilted, params int[][] pRectangles)
        {
            this.mFeature = new Feature(pTilted, pRectangles);
            this.mThreshold = pThreshold;
            this.mLeftValue = pLeft;
            this.mRightValue = pRight;
        }
        
        /// <summary>
        /// Allows for a copy of this object to be made
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            FeatureNode r = new FeatureNode();

            r.mFeature = (Feature)mFeature.Clone();
            r.mThreshold = mThreshold;

            r.mRightValue = mRightValue;
            r.mLeftValue = mLeftValue;

            r.mLeft = leftNode;
            r.mRight = rightNode;

            return r;
        }

    }
}