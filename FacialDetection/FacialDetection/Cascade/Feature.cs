using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using FacialDetection.Imaging;
using FacialDetection.Vision;

namespace FacialDetection.CascadeNamespace
{
    /// <summary>
    /// Rectangle that contains a haar-feature
    /// </summary>
    [Serializable]
    public sealed class Feature : IXmlSerializable, ICloneable
    {
        // Useful member variables to manage features
        public bool Tilted { get; set; }
        public RectangleHelper[] Rectangles { get; set; }

        /// <summary>
        /// Default Constructor where Rectangles is set to an array of 2 and Tilted is set to false
        /// </summary>
        public Feature()
        {
            Tilted = false;
            this.Rectangles = new RectangleHelper[2];
        }

        /// <summary>
        /// Constructor given an array of Rectangles, tilted is set to false
        /// </summary>
        /// <param name="rectangles"></param>
        public Feature(params RectangleHelper[] rectangles)
        {
            Tilted = false;
            this.Rectangles = rectangles;
        }

        /// <summary>
        /// Constructor given a variable number of parameters including rectangles
        /// </summary>
        /// <param name="rectangles"></param>
        public Feature(params int[][] rectangles)
            : this(false, rectangles)
        {
        }

        /// <summary>
        /// Constructor given what tilted should be, and a variable number of parameters for rectangles
        /// </summary>
        /// <param name="tilted"></param>
        /// <param name="rectangles"></param>
        public Feature(bool tilted, params int[][] rectangles)
        {
            this.Tilted = tilted;
            this.Rectangles = new RectangleHelper[rectangles.Length];
            for (int i = 0; i < rectangles.Length; i++)
                this.Rectangles[i] = new RectangleHelper(rectangles[i]);
        }

        /// <summary>
        /// Gets the sum of the areas of the rectangular features in an integral image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public double GetSum(ImageHelper image, int x, int y)
        {
            double sum = 0.0;

            if (!Tilted)
            {
                // Compute the sum for a standard feature
                foreach (RectangleHelper rect in Rectangles)
                {
                    sum += image.GetSum(x + rect.scaledX, y + rect.scaledY,
                        rect.scaledWidth, rect.scaledHeight) * rect.scaledWeight;
                }
            }
            else
            {
                // Compute the sum for a rotated feature
                foreach (RectangleHelper rect in Rectangles)
                {
                    sum += image.GetSumT(x + rect.scaledX, y + rect.scaledY,
                        rect.scaledWidth, rect.scaledHeight) * rect.scaledWeight;
                }
            }

            return sum;
        }

        /// <summary>
        /// Sets the scale and weight of a the feature container
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="weight"></param>
        public void SetScaleAndWeight(float scale, float weight)
        {
            // manual loop unfolding

            if (Rectangles.Length == 2)
            {
                RectangleHelper a = Rectangles[0];
                RectangleHelper b = Rectangles[1];

                b.ScaleRectangle(scale);
                b.ScaleWeight(weight);

                a.ScaleRectangle(scale);
                a.scaledWeight = -(b.Area * b.scaledWeight) / a.Area;
            }
            else // rectangles.Length == 3
            {
                RectangleHelper a = Rectangles[0];
                RectangleHelper b = Rectangles[1];
                RectangleHelper c = Rectangles[2];

                c.ScaleRectangle(scale);
                c.ScaleWeight(weight);

                b.ScaleRectangle(scale);
                b.ScaleWeight(weight);

                a.ScaleRectangle(scale);
                a.scaledWeight = -(b.Area * b.scaledWeight
                    + c.Area * c.scaledWeight) / (a.Area);
            }
        }


        #region IXmlSerializable Members
        /// <summary>
        /// Required when extending IXmlSerializable
        /// </summary>
        /// <returns></returns>
        XmlSchema IXmlSerializable.GetSchema()
        {
            throw new NotSupportedException();
        }
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            reader.ReadStartElement("feature");

            reader.ReadToFollowing("rects");
            reader.ReadToFollowing("_");

            var rec = new List<RectangleHelper>();
            while (reader.Name == "_")
            {
                string str = reader.ReadElementContentAsString();
                rec.Add(RectangleHelper.Parse(str));

                while (reader.Name != "_" && reader.Name != "tilted" &&
                    reader.NodeType != XmlNodeType.EndElement)
                    reader.Read();
            }

            Rectangles = rec.ToArray();

            reader.ReadToFollowing("tilted", reader.BaseURI);
            Tilted = reader.ReadElementContentAsInt() == 1;

            reader.ReadEndElement();
        }


        #endregion


        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            RectangleHelper[] newRectangles = new RectangleHelper[Rectangles.Length];
            for (int i = 0; i < newRectangles.Length; i++)
            {
                RectangleHelper rect = Rectangles[i];
                newRectangles[i] = new RectangleHelper(rect.x, rect.y,
                    rect.width, rect.height, rect.weight);
            }

            Feature r = new Feature();
            r.Rectangles = newRectangles;
            r.Tilted = Tilted;

            return r;
        }

    }

}