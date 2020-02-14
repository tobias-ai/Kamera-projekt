using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

// Important: include the opencvsharp library in your code
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace Sandbox
{
    public partial class Form1 : Form
    {
        VideoCapture capture;
        Mat frame;
        Bitmap image;
        private Thread camera;
        bool isCameraRunning = false;
        private void CaptureCamera()
        {
            camera = new Thread(new ThreadStart(CaptureCameraCallback));
            camera.Start();
        }

        public void CaptureCameraCallback()
        {
            try
            {
                frame = new Mat();
                capture = new VideoCapture(0);
                capture.Open(0);
                if (capture.IsOpened())
                {
                    while (isCameraRunning)
                    {
                        capture.Read(frame);
                        image = BitmapConverter.ToBitmap(frame);
                        if (pictureBox1.Image != null)
                        {
                            pictureBox1.Image.Dispose();
                        }
                        pictureBox1.Image = image;
                    }
                }
            }
            catch
            {
                MessageBox.Show("stop pressing!");
            }
        }

        public Form1()
        {
            InitializeComponent();
            bool exists = Directory.Exists(@"C:\Users\Tobias.bejfalk\Desktop\Portfolio");
            if (exists == false)
            {
                System.IO.Directory.CreateDirectory(@"C:\Users\Tobias.bejfalk\Desktop\Portfolio");
                MessageBox.Show("Picture folder has been created on your Desktop");
            }
            else
            {
                this.Hide();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void Button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data source=CND825B8ZY\SQLEXPRESS;Initial Catalog=KnitDatabas; Integrated Security=True;");
            con.Open();
            try
            {
                if (textBox1.Text != "")
                {
                    SqlCommand cmd = new SqlCommand("select text from knittable", con);
                    SqlDataReader da = cmd.ExecuteReader();

                    while (da.Read())
                    {
                        textBox1.Text = da.GetValue(0).ToString();
                    }
                    con.Close();
                }
            }
            catch
            {
                textBox1.Text = "CAMGIRL2000";
            }
            try
            {
                if (button1.Text.Equals("Start Camera"))
                {
                    CaptureCamera();
                    button1.Text = "Stop Camera";
                    isCameraRunning = true;

                }
                else
                {
                    capture.Release();
                    button1.Text = "Start Camera";
                    isCameraRunning = false;
                }
            }
            catch
            {
                MessageBox.Show("stop pressing");
            }
            button1.Enabled = false;
            Thread.Sleep(500);
            button1.Enabled = true;

        }
        private void Button2_Click_1(object sender, EventArgs e)
        {
            try
            {

                if (isCameraRunning)
                {
                    System.IO.Directory.CreateDirectory(@"C:\Users\tobias.bejfalk\Desktop\Portfolio\Webcam");
                    Bitmap snapshot = new Bitmap(pictureBox1.Image);

                    snapshot.Save(string.Format(@"C:\Users\tobias.bejfalk\Desktop\Portfolio\Webcam\{0}.png", Guid.NewGuid()), ImageFormat.Png);

                }
                else
                {
                    MessageBox.Show("Image can't be taken if the Camera isn't active!");
                }
                {
                }
            }
            catch
            {
            }
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void PictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
    }