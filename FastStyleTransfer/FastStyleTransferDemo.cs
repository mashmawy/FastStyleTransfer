using AlbiruniML;
using alb = AlbiruniML.Ops;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace FastStyleTransfer
{
    public partial class FastStyleTransferDemo : Form
    {
        public FastStyleTransferDemo()
        {
            InitializeComponent();
        }
        TransformNet tnet;
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        Tensor imageTensor;
        private Tensor LoadImage(OpenFileDialog ofd)
        {
            Image image = ResizeImage(Bitmap.FromFile(ofd.FileName), 500, 500);
            SrcPictureBox.Image = image;
            var x = alb.buffer(new int[] { image.Height, image.Width, 3 });
            Bitmap bmp = new Bitmap(image);
            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color clr = bmp.GetPixel(j, i);
                    float red = clr.R / 255.0f;
                    float green = clr.G / 255.0f;
                    float blue = clr.B / 255.0f;

                    x.Set(red, i, j, 0);
                    x.Set(green, i, j, 1);
                    x.Set(blue, i, j, 2);
                }
            }
            return x.toTensor();
        }

        private void LoadTensor(object dataobject)
        {
            Tensor data = dataobject as Tensor;
            Bitmap bmp = new Bitmap(data.Shape[0], data.Shape[1]);
            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    var col = Color.FromArgb((int)(data.Get(i, j, 0) * 255.0f),
                        (int)(data.Get(i, j, 1) * 255.0f),
                        (int)(data.Get(i, j, 2) * 255.0f));
                    bmp.SetPixel(j, i, col);

                }
            }
            DisPictureBox.Image = bmp;
            this.label1.Visible = false;

            this.progressBar1.Visible = false;
            this.SaveButton.Enabled = true;
            this.TransferButton.Enabled = true;
            this.StyleComboBox.Enabled = true;
            this.BrowseButton.Enabled = true;
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            var resp = ofd.ShowDialog();
            if (resp == System.Windows.Forms.DialogResult.OK)
            {
                this.imageTensor = LoadImage(ofd);

            }
            this.TransferButton.Enabled = true;
        }
        delegate void FormInvok();
        delegate void FormInvokWithParam(object param);
        Thread t;
        private void TransferButton_Click(object sender, EventArgs e)
        {
            this.TransferButton.Enabled = false;
            this.label1.Visible = true;

            this.progressBar1.Visible = true;
            this.progressBar1.Value = 0;
            this.progressBar1.Maximum = 120;
            this.StyleComboBox.Enabled = false;
            this.BrowseButton.Enabled = false;
            t = new Thread(new ThreadStart(Transfer));
            t.Start();
            
        }

        private void Transfer()
        {
            this.imageTensor.keep();

            var p = tnet.Predict(this.imageTensor);
            this.Invoke(new FormInvokWithParam(LoadTensor), p); 
        }

        private void FastStyleTransferDemo_Load(object sender, EventArgs e)
        {
            tnet = new TransformNet();
            tnet.ReportProgress += tnet_ReportProgress;
            foreach (var item in tnet.variableDictionary)
            {
                this.StyleComboBox.Items.Add(item.Key);
            }

            this.StyleComboBox.SelectedItem = this.StyleComboBox.Items[0];
            this.StylePictureBox.Image = new Bitmap("styles/" + this.StyleComboBox.Items[0].ToString() + ".jpg");
        }

        void tnet_ReportProgress(int progress)
        {
            this.Invoke(new FormInvokWithParam(UpdateProgress), progress);
        }
        void UpdateProgress(object value)
        {
            this.progressBar1.Value = Convert.ToInt32(value);
        }
        private void StyleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.StylePictureBox.Image = new Bitmap("styles/" + this.StyleComboBox.SelectedItem.ToString() + ".jpg");
            this.tnet.ChangeVariable(this.StyleComboBox.SelectedItem.ToString());
           
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            var sd = sfd.ShowDialog();
            if (sd == System.Windows.Forms.DialogResult.OK)
            {

                DisPictureBox.Image.Save(sfd.FileName);
            }
        }

        private void FastStyleTransferDemo_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (t != null)
            {
                if (t.IsAlive)
                {
                    t.Abort();
                }
            }
        }
   
    }
}
