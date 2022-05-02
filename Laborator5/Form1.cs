using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laborator5
{
    public partial class Form1 : Form
    {
        Image<Bgr, byte> My_Image = null;

        int TotalFrame, FrameNo;
        double Fps;
        bool IsReadingFrame;
        VideoCapture capture;

        private static VideoCapture cameraCapture;
        private Image<Bgr, Byte> newBackgroundImage;
        private static IBackgroundSubtractor fgDetector;


        public object Rotate_image { get; private set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog Openfile = new OpenFileDialog();
            if(Openfile.ShowDialog()== DialogResult.OK)
            {
                My_Image = new Image<Bgr, Byte>(Openfile.FileName);
                pictureBox1.Image = My_Image.ToBitmap();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog Openfile = new OpenFileDialog();
            if (Openfile.ShowDialog() == DialogResult.OK)
            {
                Image<Gray, byte> gray_image = My_Image.Convert<Gray, byte>();
                pictureBox2.Image = gray_image.AsBitmap();
                gray_image[0, 0] = new Gray(200);

                
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog Openfile = new OpenFileDialog();
            if (Openfile.ShowDialog() == DialogResult.OK)
            {
                Image<Gray, byte> gray_image = My_Image.Convert<Gray, byte>();
                pictureBox2.Image = gray_image.AsBitmap();
                gray_image[0, 0] = new Gray(200);

                var a = Convert.ToDouble(numericUpDown1.Value);
                int b = Convert.ToInt32(numericUpDown2.Value);

                //for (int i= 0; i<My_Image.Width/2 ;i++)
                //{
                //    for(int j=0; j<My_Image.Height/2 ;j++)
                //    {
                //        var res = gray_image[j, i].Intensity * a + b;
                //        gray_image[j, i] = new Gray(res);
                //    }
                //}
                gray_image = gray_image.Mul(a) + b;
              
                pictureBox3.Image = gray_image.AsBitmap();
            }
        }

        private void Gamma_Click(object sender, EventArgs e)
        {
            Image<Bgr, byte> gamma__Picture;
            var gamma = Convert.ToDouble(numericUpDown3.Value);
            gamma__Picture = My_Image;
            gamma__Picture._GammaCorrect(gamma);
            pictureBox4.Image = gamma__Picture.AsBitmap();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            HistogramViewer v = new HistogramViewer();
            v.HistogramCtrl.GenerateHistograms(My_Image, 255);
            v.Show();
        }

        private void resize(object sender, EventArgs e)
        {
            Image<Bgr, byte> resized_image;
            resized_image = My_Image.Resize(256, 128, Emgu.CV.CvEnum.Inter.Nearest);
            pictureBox5.Image = resized_image.AsBitmap();
        }

        private void rotate(object sender, EventArgs e)
        {
            Image<Bgr, byte> rotate_image;
            rotate_image = My_Image.Rotate(90, new Bgr(255, 255, 255));
            pictureBox6.Image = rotate_image.AsBitmap();

        }

        private void Video_capture(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                capture = new VideoCapture(ofd.FileName);
                Mat m = new Mat();
                capture.Read(m);
                pictureBox7.Image = m.ToBitmap();

                TotalFrame = (int)capture.Get(CapProp.FrameCount);
                Fps = capture.Get(CapProp.Fps);
                FrameNo = 1;
                numericUpDown1.Value = FrameNo;
                numericUpDown1.Minimum = 0;
                numericUpDown1.Maximum = TotalFrame;

                if (capture == null)
                {
                    return;
                }
                IsReadingFrame = true;
                ReadAllFrames();

            }
        }
        private async void ReadAllFrames()
        {

            Mat m = new Mat();
            while (IsReadingFrame == true && FrameNo < TotalFrame)
            {
                FrameNo += 1;
                var mat = capture.QueryFrame();
                pictureBox7.Image = mat.ToBitmap();
                await Task.Delay(1000 / Convert.ToInt16(Fps));
                label1.Text = FrameNo.ToString() + "/" + TotalFrame.ToString();
            }
        }

    }
}
