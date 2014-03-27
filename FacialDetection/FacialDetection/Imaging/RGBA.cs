using System;
using System.Drawing;

namespace FacialDetection.Imaging
{
    /// <summary>
    /// Used to manage RGBA Data
    /// </summary>
    public class RGBA
    {
        public const short R = 2;
        public const short G = 1;
        public const short B = 0;
        public const short A = 3;

        // member vairables used to keep track of the RGBA data
        public byte Red;
        public byte Green;
        public byte Blue;
        public byte Alpha;

        /// <summary>
        /// Create a Color object
        /// </summary>
        public System.Drawing.Color Color
        {
            get { return Color.FromArgb( Alpha, Red, Green, Blue ); }
            set
            {
                Red   = value.R;
                Green = value.G;
                Blue  = value.B;
                Alpha = value.A;
            }
        }

        /// <summary>
        /// Default Constructor for RGBA
        /// </summary>
        public RGBA( )
        {
            Red   = 0;
            Green = 0;
            Blue  = 0;
            Alpha = 255;
        }

        /// <summary>
        /// Constructor for RGBA passing in red green and blue (sets Alpha to 255)
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        public RGBA( byte red, byte green, byte blue )
        {
            this.Red   = red;
            this.Green = green;
            this.Blue  = blue;
            this.Alpha = 255;
        }

        /// <summary>
        /// Constructor for RGBA passing in red green blue and alpha
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <param name="alpha"></param>
        public RGBA( byte red, byte green, byte blue, byte alpha )
        {
            this.Red   = red;
            this.Green = green;
            this.Blue  = blue;
            this.Alpha = alpha;
        }

        /// <summary>
        /// Constructor for RGBA passing in a Color object
        /// </summary>
        /// <param name="color"></param>
        public RGBA( System.Drawing.Color color )
        {
            this.Red   = color.R;
            this.Green = color.G;
            this.Blue  = color.B;
            this.Alpha = color.A;
        }
    }
}