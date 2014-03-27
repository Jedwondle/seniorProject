using System.Drawing;
using System;

namespace FacialDetection.Imaging
{
    /// <summary>
    /// Useful point class in order to handle rectangle points
    /// </summary>
    public struct NewPoint
    {

        private float mX, mY, mW;

        /// <summary>
        /// Get/Set mX
        /// </summary>
        public float X
        {
            get { return mX; }
            set { mX = value; }
        }

        /// <summary>
        /// Get/Set mY
        /// </summary>
        public float Y
        {
            get { return mY; }
            set { mY = value; }
        }

        /// <summary>
        /// Get/Set mW (the inverse scaling factor for X and Y) 
        /// </summary>
        public float W
        {
            get { return mW; }
            set { mW = value; }
        }

        /// <summary>
        /// Constructor taking two floats (mW is set to 1)
        /// </summary>
        /// <param name="pX"></param>
        /// <param name="pY"></param>
        public NewPoint(float pX, float pY)
        {
            mX = pX;
            mY = pY;
            mW = 1;
        }

        /// <summary>
        /// Constructor taking three floats to set mX, mY, mW
        /// </summary>
        /// <param name="pX"></param>
        /// <param name="pY"></param>
        /// <param name="pW"></param>
        public NewPoint(float pX, float pY, float pW)
        {
            mX = pX;
            mY = pY;
            mW = pW;
        }

        /// <summary>
        /// Returns a new empty point (0, 0) with a scaling factor of 1
        /// </summary>
        public static readonly NewPoint Empty = new NewPoint(0, 0, 1);
    }
}