using System.Windows.Forms;
namespace FacialDetection
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.displayImageMatrix = new System.Windows.Forms.Button();
            this.displayResultsArray = new System.Windows.Forms.Button();
            this.findFaces = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.showButton = new System.Windows.Forms.Button();
            this.reset = new System.Windows.Forms.Button();
            this.line = new System.Windows.Forms.Label();
            this.line2 = new System.Windows.Forms.Label();
            this.dropdown2 = new System.Windows.Forms.ComboBox();
            this.dropdown1 = new System.Windows.Forms.ComboBox();
            this.textbox1 = new System.Windows.Forms.TextBox();
            this.textbox2 = new System.Windows.Forms.TextBox();
            this.textbox3 = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 83.42246F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.57754F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(534, 561);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanel1.SetColumnSpan(this.pictureBox1, 2);
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(528, 462);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.displayImageMatrix);
            this.flowLayoutPanel1.Controls.Add(this.displayResultsArray);
            this.flowLayoutPanel1.Controls.Add(this.findFaces);
            this.flowLayoutPanel1.Controls.Add(this.clearButton);
            this.flowLayoutPanel1.Controls.Add(this.showButton);
            this.flowLayoutPanel1.Controls.Add(this.line);
            this.flowLayoutPanel1.Controls.Add(this.dropdown2);
            this.flowLayoutPanel1.Controls.Add(this.dropdown1);
            this.flowLayoutPanel1.Controls.Add(this.textbox1);
            this.flowLayoutPanel1.Controls.Add(this.textbox2);
            this.flowLayoutPanel1.Controls.Add(this.textbox3);
            this.flowLayoutPanel1.Controls.Add(this.line2);
            this.flowLayoutPanel1.Controls.Add(this.reset);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(83, 471);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(448, 87);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // displayImageMatrix
            // 
            this.displayImageMatrix.AutoSize = true;
            this.displayImageMatrix.Location = new System.Drawing.Point(370, 3);
            this.displayImageMatrix.Name = "displayImageMatrix";
            this.displayImageMatrix.Size = new System.Drawing.Size(75, 23);
            this.displayImageMatrix.TabIndex = 0;
            this.displayImageMatrix.Text = "ImageMatrix";
            this.displayImageMatrix.UseVisualStyleBackColor = true;
            this.displayImageMatrix.Click += new System.EventHandler(this.displayImageMatrix_Click);
            // 
            // displayResultsArray
            // 
            this.displayResultsArray.AutoSize = true;
            this.displayResultsArray.Location = new System.Drawing.Point(288, 3);
            this.displayResultsArray.Name = "displayResultsArray";
            this.displayResultsArray.Size = new System.Drawing.Size(76, 23);
            this.displayResultsArray.TabIndex = 0;
            this.displayResultsArray.Text = "ResultsArray";
            this.displayResultsArray.UseVisualStyleBackColor = true;
            this.displayResultsArray.Click += new System.EventHandler(this.displayResultsArray_Click);
            // 
            // findFaces
            // 
            this.findFaces.AutoSize = true;
            this.findFaces.Location = new System.Drawing.Point(207, 3);
            this.findFaces.Name = "findFaces";
            this.findFaces.Size = new System.Drawing.Size(75, 23);
            this.findFaces.TabIndex = 1;
            this.findFaces.Text = "Find Faces!";
            this.findFaces.UseVisualStyleBackColor = true;
            this.findFaces.Click += new System.EventHandler(this.findFaces_Click);
            // 
            // clearButton
            // 
            this.clearButton.AutoSize = true;
            this.clearButton.Location = new System.Drawing.Point(107, 3);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(94, 23);
            this.clearButton.TabIndex = 2;
            this.clearButton.Text = "Clear the picture";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // showButton
            // 
            this.showButton.AutoSize = true;
            this.showButton.Location = new System.Drawing.Point(10, 3);
            this.showButton.Name = "showButton";
            this.showButton.Size = new System.Drawing.Size(91, 23);
            this.showButton.TabIndex = 3;
            this.showButton.Text = "Chose a picture";
            this.showButton.UseVisualStyleBackColor = true;
            this.showButton.Click += new System.EventHandler(this.showButton_Click);
            // 
            // reset
            // 
            this.reset.AutoSize = true;
            this.reset.Location = new System.Drawing.Point(10, 3);
            this.reset.Name = "reset";
            this.reset.Size = new System.Drawing.Size(91, 23);
            this.reset.TabIndex = 3;
            this.reset.Text = "Reset Picture";
            this.reset.UseVisualStyleBackColor = true;
            this.reset.Click += new System.EventHandler(this.reset_Click);
            // 
            // line
            // 
            this.line.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.line.Location = new System.Drawing.Point(-921, 29);
            this.line.Name = "line";
            this.line.Size = new System.Drawing.Size(1366, 1);
            this.line.TabIndex = 4;
            // 
            // line2
            // 
            this.line2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.line2.Location = new System.Drawing.Point(-921, 29);
            this.line2.Name = "line2";
            this.line2.Size = new System.Drawing.Size(1366, 1);
            this.line2.TabIndex = 4;
            // 
            // dropdown2
            // 
            this.dropdown2.DataSource = new string[] {
        "SmallerToGreater",
        "GreaterToSmaller"};
            this.dropdown2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropdown2.Location = new System.Drawing.Point(324, 33);
            this.dropdown2.Name = "dropdown2";
            this.dropdown2.Size = new System.Drawing.Size(121, 21);
            this.dropdown2.TabIndex = 6;
            this.dropdown2.SelectedIndexChanged += new System.EventHandler(this.dropdown2_SelectedIndexChanged);
            // 
            // dropdown1
            // 
            this.dropdown1.DataSource = new string[] {
        "Default",
        "No Overlap",
        "Single"};
            this.dropdown1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropdown1.Location = new System.Drawing.Point(197, 33);
            this.dropdown1.Name = "dropdown1";
            this.dropdown1.Size = new System.Drawing.Size(121, 21);
            this.dropdown1.TabIndex = 5;
            this.dropdown1.SelectedIndexChanged += new System.EventHandler(this.dropdown1_SelectedIndexChanged);
            // 
            // textbox1
            // 
            this.textbox1.AcceptsReturn = true;
            this.textbox1.Location = new System.Drawing.Point(162, 33);
            this.textbox1.Name = "textbox1";
            this.textbox1.Size = new System.Drawing.Size(29, 20);
            this.textbox1.TabIndex = 7;
            this.textbox1.Text = "15";
            this.textbox1.TextChanged += new System.EventHandler(this.textbox1_TextChanged);
            // 
            // textbox2
            // 
            this.textbox2.Location = new System.Drawing.Point(107, 33);
            this.textbox2.Name = "textbox2";
            this.textbox2.Size = new System.Drawing.Size(49, 20);
            this.textbox2.TabIndex = 8;
            this.textbox2.Text = "500";
            this.textbox2.TextChanged += new System.EventHandler(this.textbox2_TextChanged);
            // 
            // textbox3
            // 
            this.textbox3.Location = new System.Drawing.Point(12, 33);
            this.textbox3.Name = "textbox3";
            this.textbox3.Size = new System.Drawing.Size(89, 20);
            this.textbox3.TabIndex = 9;
            this.textbox3.Text = "1.1";
            this.textbox3.TextChanged += new System.EventHandler(this.textbox3_TextChanged);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.checkBox1);
            this.flowLayoutPanel2.Controls.Add(this.checkBox2);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 471);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(74, 87);
            this.flowLayoutPanel2.TabIndex = 1;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(3, 3);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(66, 17);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "Autosize";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(3, 26);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(58, 17);
            this.checkBox2.TabIndex = 2;
            this.checkBox2.Text = "Rotate";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "JPEG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|BMP Files (*.bmp)|*.bmp|All file" +
    "s (*.*)|*.*";
            this.openFileDialog1.Title = "Select a picture file";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.checkBox1_CheckedChanged);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(534, 561);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Picture Viewer";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private PictureBox pictureBox1;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private FlowLayoutPanel flowLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel2;
        private Button showButton;
        private Button clearButton;
        private Button findFaces;
        private Button displayImageMatrix;
        private Button displayResultsArray;
        private Button reset;
        private OpenFileDialog openFileDialog1;
        private ColorDialog colorDialog1;
        private ComboBox dropdown1;
        private ComboBox dropdown2;
        private TextBox textbox1;
        private TextBox textbox2;
        private TextBox textbox3;
        private Label line;
        private Label line2;
    }
}

