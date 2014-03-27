using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FacialDetection.Imaging;
using FacialDetection.Vision;
using FacialDetection.CascadeNamespace;
using System.IO;

namespace FacialDetection
{
    /// <summary>
    /// Form that controls the program (The application)
    /// </summary>
    public partial class Form1 : Form
    {
        // useful member variables
        private Cascade cascade; 
        private Detector detector;
        private Bitmap map;
        private Bitmap original;
        private byte[] byteArray;
        private List<Rectangle> detectedFeatures = null;

        /// <summary>
        /// Default Constructor that handles initializations
        /// </summary>
        public Form1()
        {
            // Create the casscade using the cascade information from one of the OpenCV files.
            
            // BEST SO FAR
             cascade = Cascade.FromXml("C:\\cygwin\\home\\Jonathan\\seniorProject\\FacialDetection\\FacialDetection\\Resources\\frontalface_default.xml");
            
            // Second best so far
            // cascade = HaarCascade.FromXml("C:\\cygwin\\home\\Jonathan\\seniorProject\\FacialDetection\\FacialDetection\\Resources\\frontalface_alt_tree_cascades.xml");
            
            // Third best so far
            // cascade = HaarCascade.FromXml("C:\\cygwin\\home\\Jonathan\\seniorProject\\FacialDetection\\FacialDetection\\Resources\\frontalface_alt.xml");
            
            // TEST FOR HANDS
            // cascade = HaarCascade.FromXml("C:\\cygwin\\home\\Jonathan\\seniorProject\\FacialDetection\\FacialDetection\\Resources\\hand1.xml");


            // Create the detector out of the cascade, and some other configuration details
            // Best config is (Cascade 1, 15, Default, 1.1F, GreaterToSmaller, 2000)
            detector = new Detector(this.cascade, 15, searchMode.Default, 1.1F, scalingMode.SmallerToGreater, 500);
            
            // Create a console to wrtie to
            AllocConsole();

            // Initialize the Form
            InitializeComponent();
        }

        /// <summary>
        /// This is the allocation of a console used in this program
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        /// <summary>
        /// Used to set up the image by creating a bitmap
        /// </summary>
        private void setupImage()
        {
            this.map = new Bitmap(pictureBox1.Image);
        }

        /// <summary>
        /// This is where we handle the file grabbing (Show the dialogue). Give the user a dialogue to select a picture with
        /// and then make sure its a .jpg, .jpeg, .png, or .gif and that it is within the pixel size.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showButton_Click(object sender, EventArgs e)
        {
            // Show the Open File dialog. If the user chooses OK, load the 
            // picture that the user choose.
            openFileDialog1.Filter = "Image Files (JPG,JPEG,PNG,GIF)|*.JPG;*.JPEG;*.PNG;*.GIF";
            ArrayList extensions = new ArrayList(new string[] {"jpg", "JPG", "jpeg", "JPEG", "png", "PNG", "gif", "GIF" });
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Parse the file name
                string extension = openFileDialog1.FileName.Split('.').Last();

                // Decide on if the extension is acceptable
                if (!extensions.Contains(extension))
                {
                    Console.WriteLine("You chose an image that was in the wrong format!\r\nAccepted images include the file format of 'jpg', 'jpeg', 'png', and 'gif'");
                }

                // And check its dimensions if it is an acceptable extension
                else
                {
                    Image image1 = Image.FromFile(openFileDialog1.FileName);
                    this.original = new Bitmap(image1);
                    if (image1.Height <= 2048 && image1.Width <= 2048)
                    {
                        Console.WriteLine("Height: " + image1.Height + "\tWidth: " + image1.Width);
                        pictureBox1.Load(openFileDialog1.FileName);
                    }
                    else
                    {
                        Console.WriteLine("You chose an image that exceeded the resolution parameters of 2000x2000 pixles");
                    }
                }
                //Console.WriteLine("What type am I?: " + pictureBox1.Image.RawFormat);
            }

        }

        /// <summary>
        /// Clear the picture from the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearButton_Click(object sender, EventArgs e)
        {
            // Clear the picture.
            pictureBox1.Image = null;
        }

        /// <summary>
        /// Do the hard work for finding faces
        /// </summary>
        private void searchForFaces()
        {
            if (detectedFeatures == null)
            {
                detectedFeatures = new List<Rectangle>();
            }
            else
            {
                detectedFeatures.Clear();
            }

            Console.WriteLine("\r\nWe're now searching for faces");

            // Here we're inspecting the detector's information
            //Console.WriteLine();
            //Console.WriteLine(detector.ToString());

            // This is a hack to get EVERY face detected between the given scalefactor and 3f (NOT NEEDED)
            // for (int k = (int)(1.1f * 100); k < 300; k += 15)
            // {
            //    detector.ScalingFactor = (float)((float)k / 100.0f);

            // If we want to check for rotated images, we do this
            if (checkBox2.Checked)
            {
                // raise the scaling factor in order to speed things up. Comment this out if you want to keep the initial scaling factor
                //detector.ScalingFactor = 2f;

                // make a holder bitmap;
                Bitmap map2 = this.map;

                // make a temp holder for the Rectangles we get back
                Rectangle[] temp;

                // find the center of the new bitmap;
                Point center = new Point(this.map.Height / 2, this.map.Width / 2);

                for (int i = 05; i < 360; i += 30)
                {
                    map2 = Extras.RotateImg(this.map, i, Color.White);
                    int difX = (map2.Width - this.map.Width) / 2;
                    int difY = (map2.Height - this.map.Height) / 2;
                    center = new Point(map2.Width / 2, map2.Height / 2);

                    temp = detector.ProcessFrame(map2);
                    if (temp.Length > 0)
                    {
                        double angle = -i;
                        for (int j = 0; j < temp.Length; j++)
                        {
                            detectedFeatures.Add(Extras.RotatePoint(temp[j], center, angle, difX, difY));
                        }
                    }
                }

            }
            else
            {
                detectedFeatures = detector.ProcessFrame(this.map).ToList<Rectangle>();
            }
            //}
        }


        /// <summary>
        /// This funciton calls the necessary functions to handle the face detection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void findFaces_Click(object sender, EventArgs e)
        {
            // Create the bitmap needed for the detector's process frame function
            this.setupImage(); // Apparently, black and white is not needed to normalize the image

            // do the search for faces!
            searchForFaces();

            // If we didn't detect any faces, let the user know.
            if (this.detectedFeatures.Count <= 0)
            {
                Console.WriteLine("No faces were detected :(");
            }

            // Otherwise, draw squares around each face.
            else
            {
                Pen pen = new Pen(Color.Red, 2f);
                Graphics g = Graphics.FromImage(this.map);
                foreach (var faceRect in this.detectedFeatures)
                {
                    g.DrawRectangle(pen, faceRect);
                }
                pictureBox1.Image = (Image)this.map;
                pen.Dispose();
                g.Dispose();
                Console.WriteLine("There are your faces :)");
            }

        }

        /// <summary>
        /// Display the results of faces in the N-3 matrix needed to fulfill the requirement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void displayResultsArray_Click(object sender, EventArgs e)
        {
            // Here we grab our detector, and do what we need to to get it to 
            // detect features
            this.setupImage();

            // search for faces
            searchForFaces();

            if (detectedFeatures.Count <= 0)
            {
                Console.WriteLine("No faces were detected :(");
            }
            else
            {
                Console.WriteLine("The N-3 Matrix is as follows:");
                Console.WriteLine("|   X  |   Y  | Length |");
                Console.WriteLine("------------------------");
                foreach (var faceRect in this.detectedFeatures)
                {
                    Console.WriteLine("| {0, 4} | {1, 4} |  {2, 3}   |", faceRect.X, faceRect.Y, faceRect.Width);
                    Console.WriteLine("------------------------");
                }
            }
        }


        /// <summary>
        /// Completing OLD requirement 3 turn the image to black and white and -- NO LONGER NEEDED
        /// display the byte array information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void displayImageMatrix_Click(object sender, EventArgs e)
        {
            // Show the ByteArray made of the image 
            this.setupImage();
            
            // give them the option to not display image array
            this.byteArray = Extras.imageToByteArray((Image)map);
            
            Console.WriteLine("Write the image byte array (Press Enter to display or no/n to skip)");
            string input = Console.ReadLine();
            input = input.ToLower();
            
            if (!(input.CompareTo("n") == 0 || input.CompareTo("no") == 0))
            {
                for (int i = 0; i < byteArray.Length; i++)
                {
                    Console.Write("{0, 4} ", byteArray[i]);
                }    
            }
            Console.Write("\r\n");
        }

        /// <summary>
        /// Close the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, EventArgs e)
        {
            // Close the form. 
            this.Close();
        }

        /// <summary>
        /// Reset the image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reset_Click(object sender, EventArgs e)
        {
            // Close the form. 
            pictureBox1.Image = (Image)this.original;
        }

        /// <summary>
        /// Set the picture to be normal (actual pixel ratio) or zoomed (adjusted to fit the picture box)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // If the user selects the Stretch check box,  
            // change the PictureBox's SizeMode property to "Stretch". 
            // If the user clears the check box, change it to "Normal".
            // Choosing Stretch shows the entire image in the available space.          
            if (checkBox1.Checked)
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            else
                pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
        }

        /// <summary>
        /// Select the searchMode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dropdown1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((string)dropdown1.SelectedItem == "Default")
            {
                Console.WriteLine("Default was selected and set");
                this.detector.SearchMode = searchMode.Default;
                Console.WriteLine(detector.ToString());
            }
            else if ((string)dropdown1.SelectedItem == "No Overlap")
            {
                Console.WriteLine("No Overlap was selected and set");
                this.detector.SearchMode = searchMode.NoOverlap;
                Console.WriteLine(detector.ToString());
            }
            else if ((string)dropdown1.SelectedItem == "Single")
            {
                Console.WriteLine("Single was selected and set");
                this.detector.SearchMode = searchMode.Single;
                Console.WriteLine(detector.ToString());
            }
        }

        /// <summary>
        /// Select the scalingMode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dropdown2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((string)dropdown2.SelectedItem == "SmallerToGreater")
            {
                Console.WriteLine("SmallerToGreater was selected and set");
                this.detector.ScalingMode = scalingMode.SmallerToGreater;
                Console.WriteLine(detector.ToString());
            }
            else if ((string)dropdown2.SelectedItem == "GreaterToSmaller")
            {
                Console.WriteLine("GreaterToSmaller was selected and set");
                this.detector.ScalingMode = scalingMode.GreaterToSmaller;
                Console.WriteLine(detector.ToString());
            }
        }


        /// <summary>
        /// Set the MaxSize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textbox2_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine("MaxSize was set");
            int val;
            bool result = int.TryParse(textbox2.Text, out val);
            if (result)
            {
                this.detector.MaxSize = new Size(val, val);
                Console.WriteLine(detector.ToString());
            }
        }

        /// <summary>
        /// Set the MinSize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textbox1_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine("MinSize was set");
            int val;
            bool result = int.TryParse(textbox1.Text, out val);
            if (result)
            {
                this.detector.MinSize = new Size(val, val);
                Console.WriteLine(detector.ToString());
            }
        }

        /// <summary>
        /// Set the scale factor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textbox3_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine("ScaleFactor was set");
            double val;
            bool result = double.TryParse(textbox3.Text, out val);
            if (result)
            {
                this.detector.ScalingFactor = (float)val;
                Console.WriteLine(detector.ToString());
            }
        }

    }
}
