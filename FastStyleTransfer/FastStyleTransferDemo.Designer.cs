namespace FastStyleTransfer
{
    partial class FastStyleTransferDemo
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
            this.SaveButton = new System.Windows.Forms.Button();
            this.TransferButton = new System.Windows.Forms.Button();
            this.StyleLabel = new System.Windows.Forms.Label();
            this.StyleComboBox = new System.Windows.Forms.ComboBox();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.StylePictureBox = new System.Windows.Forms.PictureBox();
            this.DisPictureBox = new System.Windows.Forms.PictureBox();
            this.SrcPictureBox = new System.Windows.Forms.PictureBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.StylePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DisPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SrcPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // SaveButton
            // 
            this.SaveButton.Enabled = false;
            this.SaveButton.Location = new System.Drawing.Point(672, 295);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 17;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // TransferButton
            // 
            this.TransferButton.Enabled = false;
            this.TransferButton.Location = new System.Drawing.Point(554, 295);
            this.TransferButton.Name = "TransferButton";
            this.TransferButton.Size = new System.Drawing.Size(75, 23);
            this.TransferButton.TabIndex = 16;
            this.TransferButton.Text = "Transfer";
            this.TransferButton.UseVisualStyleBackColor = true;
            this.TransferButton.Click += new System.EventHandler(this.TransferButton_Click);
            // 
            // StyleLabel
            // 
            this.StyleLabel.AutoSize = true;
            this.StyleLabel.Location = new System.Drawing.Point(57, 295);
            this.StyleLabel.Name = "StyleLabel";
            this.StyleLabel.Size = new System.Drawing.Size(63, 13);
            this.StyleLabel.TabIndex = 15;
            this.StyleLabel.Text = "Select Style";
            // 
            // StyleComboBox
            // 
            this.StyleComboBox.FormattingEnabled = true;
            this.StyleComboBox.Location = new System.Drawing.Point(140, 290);
            this.StyleComboBox.Name = "StyleComboBox";
            this.StyleComboBox.Size = new System.Drawing.Size(121, 21);
            this.StyleComboBox.TabIndex = 14;
            this.StyleComboBox.SelectedIndexChanged += new System.EventHandler(this.StyleComboBox_SelectedIndexChanged);
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(303, 290);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(75, 23);
            this.BrowseButton.TabIndex = 13;
            this.BrowseButton.Text = "Browse..";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // StylePictureBox
            // 
            this.StylePictureBox.Location = new System.Drawing.Point(50, 36);
            this.StylePictureBox.Name = "StylePictureBox";
            this.StylePictureBox.Size = new System.Drawing.Size(223, 248);
            this.StylePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.StylePictureBox.TabIndex = 12;
            this.StylePictureBox.TabStop = false;
            // 
            // DisPictureBox
            // 
            this.DisPictureBox.Location = new System.Drawing.Point(554, 36);
            this.DisPictureBox.Name = "DisPictureBox";
            this.DisPictureBox.Size = new System.Drawing.Size(223, 248);
            this.DisPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.DisPictureBox.TabIndex = 11;
            this.DisPictureBox.TabStop = false;
            // 
            // SrcPictureBox
            // 
            this.SrcPictureBox.Location = new System.Drawing.Point(303, 36);
            this.SrcPictureBox.Name = "SrcPictureBox";
            this.SrcPictureBox.Size = new System.Drawing.Size(223, 248);
            this.SrcPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.SrcPictureBox.TabIndex = 10;
            this.SrcPictureBox.TabStop = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(170, 319);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(295, 23);
            this.progressBar1.TabIndex = 18;
            this.progressBar1.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(98, 321);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Transfering";
            this.label1.Visible = false;
            // 
            // FastStyleTransferDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 354);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.TransferButton);
            this.Controls.Add(this.StyleLabel);
            this.Controls.Add(this.StyleComboBox);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.StylePictureBox);
            this.Controls.Add(this.DisPictureBox);
            this.Controls.Add(this.SrcPictureBox);
            this.MaximizeBox = false;
            this.Name = "FastStyleTransferDemo";
            this.Text = "Fast Style Transfer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FastStyleTransferDemo_FormClosing);
            this.Load += new System.EventHandler(this.FastStyleTransferDemo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.StylePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DisPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SrcPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button TransferButton;
        private System.Windows.Forms.Label StyleLabel;
        private System.Windows.Forms.ComboBox StyleComboBox;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.PictureBox StylePictureBox;
        private System.Windows.Forms.PictureBox DisPictureBox;
        private System.Windows.Forms.PictureBox SrcPictureBox;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
    }
}

