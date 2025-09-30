namespace FernandezDigitalImagerProcessingProgram
{
    partial class MainForm
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chooseImageBtn = new System.Windows.Forms.Button();
            this.chooseBackgroundBtn = new System.Windows.Forms.Button();
            this.choiceDrpbox = new System.Windows.Forms.ComboBox();
            this.saveBtn = new System.Windows.Forms.Button();
            this.copyBtn = new System.Windows.Forms.Button();
            this.subChoiceDrpbox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(38, 165);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(300, 300);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(382, 165);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(300, 300);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Location = new System.Drawing.Point(728, 165);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(300, 300);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 3;
            this.pictureBox3.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Baskerville Old Face", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(441, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(183, 32);
            this.label1.TabIndex = 4;
            this.label1.Text = "Original Image";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Baskerville Old Face", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(778, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(203, 32);
            this.label2.TabIndex = 5;
            this.label2.Text = "Processed Image";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Baskerville Old Face", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(73, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(227, 32);
            this.label3.TabIndex = 6;
            this.label3.Text = "Background Image";
            // 
            // chooseImageBtn
            // 
            this.chooseImageBtn.Location = new System.Drawing.Point(464, 488);
            this.chooseImageBtn.Name = "chooseImageBtn";
            this.chooseImageBtn.Size = new System.Drawing.Size(130, 30);
            this.chooseImageBtn.TabIndex = 7;
            this.chooseImageBtn.Text = "Choose Image";
            this.chooseImageBtn.UseVisualStyleBackColor = true;
            // 
            // chooseBackgroundBtn
            // 
            this.chooseBackgroundBtn.Location = new System.Drawing.Point(111, 488);
            this.chooseBackgroundBtn.Name = "chooseBackgroundBtn";
            this.chooseBackgroundBtn.Size = new System.Drawing.Size(152, 30);
            this.chooseBackgroundBtn.TabIndex = 8;
            this.chooseBackgroundBtn.Text = "Choose Background";
            this.chooseBackgroundBtn.UseVisualStyleBackColor = true;
            this.chooseBackgroundBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // choiceDrpbox
            // 
            this.choiceDrpbox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.choiceDrpbox.FormattingEnabled = true;
            this.choiceDrpbox.Items.AddRange(new object[] {
            "Basic Copy",
            "Greyscale",
            "Color Inversion",
            "Sepia",
            "Histogram",
            "Subtract",
            "Smoothing",
            "Gaussian Blur",
            "Sharpen",
            "Mean Removal",
            "Emboss"});
            this.choiceDrpbox.Location = new System.Drawing.Point(793, 96);
            this.choiceDrpbox.Name = "choiceDrpbox";
            this.choiceDrpbox.Size = new System.Drawing.Size(177, 24);
            this.choiceDrpbox.TabIndex = 9;
            this.choiceDrpbox.SelectedIndexChanged += new System.EventHandler(this.choiceDrpbox_SelectedIndexChanged);
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(809, 537);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(141, 28);
            this.saveBtn.TabIndex = 10;
            this.saveBtn.Text = "Save Image";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.button3_Click);
            // 
            // copyBtn
            // 
            this.copyBtn.Location = new System.Drawing.Point(809, 488);
            this.copyBtn.Name = "copyBtn";
            this.copyBtn.Size = new System.Drawing.Size(141, 28);
            this.copyBtn.TabIndex = 11;
            this.copyBtn.Text = "Copy Image";
            this.copyBtn.UseVisualStyleBackColor = true;
            this.copyBtn.Click += new System.EventHandler(this.copyBtn_Click);
            // 
            // subChoiceDrpbox
            // 
            this.subChoiceDrpbox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.subChoiceDrpbox.FormattingEnabled = true;
            this.subChoiceDrpbox.Location = new System.Drawing.Point(793, 135);
            this.subChoiceDrpbox.Name = "subChoiceDrpbox";
            this.subChoiceDrpbox.Size = new System.Drawing.Size(177, 24);
            this.subChoiceDrpbox.TabIndex = 12;
            this.subChoiceDrpbox.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1059, 596);
            this.Controls.Add(this.subChoiceDrpbox);
            this.Controls.Add(this.copyBtn);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.choiceDrpbox);
            this.Controls.Add(this.chooseBackgroundBtn);
            this.Controls.Add(this.chooseImageBtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "MainForm";
            this.Text = "Image Processor";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button chooseImageBtn;
        private System.Windows.Forms.Button chooseBackgroundBtn;
        private System.Windows.Forms.ComboBox choiceDrpbox;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Button copyBtn;
        private System.Windows.Forms.ComboBox subChoiceDrpbox;
    }
}

