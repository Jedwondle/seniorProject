using System;
using System.Globalization;

namespace FacialDetection.Imaging
{
    [Serializable]
    public class RectangleHelper
    {

        // Member variables needed for building a Rectangle
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public float weight { get; set; }

        // these variables are used in feature computations
        public int scaledX { get; set; }
        public int scaledY { get; set; }
        public int scaledWidth { get; set; }
        public int scaledHeight { get; set; }
        public float scaledWeight { get; set; }

        /// <summary>
        /// Constructor passing in an int array of values to fill the rectangle with
        /// </summary>
        /// <param name="pValues"></param>
        public RectangleHelper(int[] pValues)
        {
            this.x = pValues[0];
            this.y = pValues[1];
            this.width = pValues[2];
            this.height = pValues[3];
            this.weight = pValues[4];
        }

        /// <summary>
        /// Constructor passing in the x, y, width, height, and weight of a rectangle
        /// </summary>
        /// <param name="pX"></param>
        /// <param name="pY"></param>
        /// <param name="pWidth"></param>
        /// <param name="pHeight"></param>
        /// <param name="pWeight"></param>
        public RectangleHelper(int pX, int pY, int pWidth, int pHeight, float pWeight)
        {
            this.x = pX;
            this.y = pY;
            this.width = pWidth;
            this.height = pHeight;
            this.weight = pWeight;
        }

        /// <summary>
        /// Default Constructor (made private to discourage use)
        /// </summary>
        private RectangleHelper()
        {
        }

        /// <summary>
        /// Area (returns the scaled width X scaled height)
        /// </summary>
        public int Area
        {
            get { return scaledWidth * scaledHeight; }
        }

        /// <summary>
        /// ScaleRectangle scales the values of this rectangle by pValue
        /// </summary>
        /// <param name="pValue"></param>
        public void ScaleRectangle(float pValue)
        {
            scaledX = (int)(x * pValue);
            scaledY = (int)(y * pValue);
            scaledWidth = (int)(width * pValue);
            scaledHeight = (int)(height * pValue);
        }

        /// <summary>
        /// ScaleWeight scales the weight of the rectangle by pScale
        /// </summary>
        /// <param name="pScale"></param>
        public void ScaleWeight(float pScale)
        {
            scaledWeight = weight * pScale;
        }

        /// <summary>
        /// Creates a new rectangle by parsing the given string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static RectangleHelper Parse(string input)
        {
            string[] values = input.Trim().Split(' ');

            int x = int.Parse(values[0], CultureInfo.InvariantCulture);
            int y = int.Parse(values[1], CultureInfo.InvariantCulture);
            int w = int.Parse(values[2], CultureInfo.InvariantCulture);
            int h = int.Parse(values[3], CultureInfo.InvariantCulture);
            float weight = float.Parse(values[4], CultureInfo.InvariantCulture);

            return new RectangleHelper(x, y, w, h, weight);
        }
    }
}