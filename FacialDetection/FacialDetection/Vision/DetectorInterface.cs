using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FacialDetection.Vision
{
    /// <summary>
    /// Object detector interface.
    /// </summary>
    interface DetectorInterface
    {
        /// <summary>
        /// Gets the detected objects.
        /// </summary>
        Rectangle[] DetectedObjects { get; }

        /// <summary>
        /// Processes an image and returns the found objects.
        /// </summary>
        Rectangle[] ProcessFrame(AForge.Imaging.UnmanagedImage image);
    }
}
