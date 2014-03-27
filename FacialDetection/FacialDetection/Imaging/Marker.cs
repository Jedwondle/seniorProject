using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacialDetection.Imaging
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using AForge.Imaging;
    using AForge.Imaging.Filters;

    /// <summary>
    ///   Filter to mark (highlight) rectangles in a image.
    /// </summary>
    /// 
    public class Marker : BaseInPlaceFilter
    {
        private Color markerColor = Color.White;
        private IEnumerable<Rectangle> rectangles;
        private Dictionary<PixelFormat, PixelFormat> formatTranslations = new Dictionary<PixelFormat, PixelFormat>();


        /// <summary>
        ///   Color used to mark pairs.
        /// </summary>
        /// 
        public Color MarkerColor
        {
            get { return markerColor; }
            set { markerColor = value; }
        }

        /// <summary>
        ///   The set of rectangles.
        /// </summary>
        /// 
        public IEnumerable<Rectangle> Rectangles
        {
            get { return rectangles; }
            set { rectangles = value; }
        }


        /// <summary>
        ///   Format translations dictionary.
        /// </summary>
        /// 
        public override Dictionary<PixelFormat, PixelFormat> FormatTranslations
        {
            get { return formatTranslations; }
        }


        /// <summary>
        ///   Initializes a new instance of the <see cref="Marker"/> class.
        /// </summary>
        /// 
        /// <param name="markerColor">The color to use to drawn the rectangles.</param>
        /// 
        public Marker(Color markerColor)
            : this(null, markerColor)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Marker"/> class.
        /// </summary>
        /// 
        /// <param name="rectangles">Set of rectangles to be drawn.</param>
        /// 
        public Marker(params Rectangle[] rectangles)
            : this(rectangles, Color.White)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Marker"/> class.
        /// </summary>
        /// 
        /// <param name="rectangles">Set of rectangles to be drawn.</param>
        /// 
        public Marker(IEnumerable<Rectangle> rectangles)
            : this(rectangles, Color.White)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Marker"/> class.
        /// </summary>
        /// 
        /// <param name="rectangles">Set of rectangles to be drawn.</param>
        /// <param name="markerColor">The color to use to drawn the rectangles.</param>
        /// 
        public Marker(IEnumerable<Rectangle> rectangles, Color markerColor)
        {
            this.rectangles = rectangles;
            this.markerColor = markerColor;

            formatTranslations[PixelFormat.Format8bppIndexed] = PixelFormat.Format8bppIndexed;
            formatTranslations[PixelFormat.Format24bppRgb] = PixelFormat.Format24bppRgb;
        }

        /// <summary>
        ///   Applies the filter to the image.
        /// </summary>
        protected override void ProcessFilter(UnmanagedImage image)
        {
            // mark all rectangular regions
            foreach (Rectangle rectangle in rectangles)
            {
                Drawing.Rectangle(image, rectangle, markerColor);
            }
        }
    }
}
