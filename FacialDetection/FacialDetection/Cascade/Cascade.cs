using System;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace FacialDetection.CascadeNamespace
{
    /// <summary>
    /// Class to handle Cascades
    /// </summary>
    [Serializable]
    public class Cascade
    {
        // Important member variables of a cascade
        public int width { get; protected set; }
        public int height { get; protected set; }
        public Stage[] stages { get; protected set; }
        public bool hasTiltedFeatures { get; protected set; }

        /// <summary>
        /// Constructor for Cascade taking a width, height and stages
        /// </summary>
        /// <param name="pWidth"></param>
        /// <param name="pHeight"></param>
        /// <param name="pStages"></param>
        public Cascade(int pWidth, int pHeight, Stage[] pStages)
        {
            width = pWidth;
            height = pHeight;
            stages = pStages;

            hasTiltedFeatures = findTiltedFeatures(pStages);
        }

        /// <summary>
        /// Constructor for Cascade passing in the height and width
        /// </summary>
        /// <param name="pWidth"></param>
        /// <param name="pHeight"></param>
        protected Cascade(int pWidth, int pHeight)
        {
            width = pWidth;
            height = pHeight;
        }

        /// <summary>
        /// Looks for the tilted features flag on the cascade in order to set the hasTiltedFeatures flag
        /// </summary>
        /// <param name="pStages"></param>
        /// <returns></returns>
        private static bool findTiltedFeatures(Stage[] pStages)
        {
            foreach (var stage in pStages)
                foreach (var tree in stage.nodes)
                    foreach (var node in tree)
                        if (node.mFeature.Tilted == true)
                            return true;
            return false;
        }

        /// <summary>
        /// Constructor for Cascade passing a stream from an xml file
        /// </summary>
        /// <param name="pStream"></param>
        /// <returns></returns>
        public static Cascade FromXml(Stream pStream)
        {
            return FromXml(new StreamReader(pStream));
        }

        /// <summary>
        /// Constructor for Cascade passing a path to an xml file
        /// </summary>
        /// <param name="pPath"></param>
        /// <returns></returns>
        public static Cascade FromXml(string pPath)
        {
            return FromXml(new StreamReader(pPath));
        }

        /// <summary>
        /// The Heavy lifter that creates a Cascade from an xml file using a TextReader
        /// </summary>
        /// <param name="pStringReader"></param>
        /// <returns></returns>
        public static Cascade FromXml(TextReader pStringReader)
        {
            XmlTextReader xmlReader = new XmlTextReader(pStringReader);

            // Grab the base window size
            xmlReader.ReadToFollowing("size");
            string size = xmlReader.ReadElementContentAsString();

            // Load the cascade stages
            xmlReader.ReadToFollowing("stages");
            XmlSerializer serializer = new XmlSerializer(typeof(CascadeSerializer));
            var pStages = (CascadeSerializer)serializer.Deserialize(xmlReader);

            // Parse the size into width and height
            string[] s = size.Trim().Split(' ');
            int pWidth = int.Parse(s[0], CultureInfo.InvariantCulture);
            int pHeight = int.Parse(s[1], CultureInfo.InvariantCulture);

            // Create and return the new cascade
            return new Cascade(pWidth, pHeight, pStages.Stages);
        }

    }
}