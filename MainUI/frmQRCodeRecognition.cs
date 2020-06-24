using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AForge.Video;
using AForge.Video.DirectShow;
using Dynamsoft.Barcode;


namespace Eoba.Shipyard.ArrangementSimulator.MainUI
{
    public partial class frmQRCodeRecognition : Form
    {
        private FilterInfoCollection videoDevicesList;
        private IVideoSource videoSource;
        private Bitmap currentFrame;
        private bool stillImageMode = false;
        BarcodeReader reader = new BarcodeReader();
                
        public frmQRCodeRecognition()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            reader.LicenseKeys = "t0260NQAAACw9adkCzg3gpwCHWD3w+FzkDTxIWBKXfg8f1qkUI6JugElVAKwZIeOrnfNWo88wDK6MiuQO0On/EH9OO0P7IQj6gzbvbPNfCx0KgoDfei67LfDOfBYH3uq9HYQiq4uF+SA3WzCD81/gBUYTRgDWq7cFRUwjFjpFaML+kpHO8T6obf609fpX18Jjjh03CUcdOrZOwfw7aq5ed3JStz+/JxnGg8Gd3IcHqkOzKqIZZaLCwJ9WzARHTPk6W0YLxHVXmRqWOOaBXkRLd6gDcI3rrfbGWfUB80Vs+hNNITvUYXgptTb9+8cNeoIc+epAH9tGdNJ4NoQNeZmwXFKc60cqqGI=;";

            // get list of video devices
            videoDevicesList = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo videoDevice in videoDevicesList)
            {
                cmbVideoSource.Items.Add(videoDevice.Name);
            }
            if (cmbVideoSource.Items.Count > 0)
            {
                cmbVideoSource.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("No video sources found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // stop the camera on window close
            this.Closing += frmQRCodeRecognition_Closing;
        }

        private void frmQRCodeRecognition_Closing(object sender, CancelEventArgs e)
        {
            //signal to stop
            if (videoSource != null && videoSource.IsRunning) 
            {
                videoSource.SignalToStop();
            }
        }


        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // dispose previous image to avoid memory leak
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
            }

            if (currentFrame != null) 
            {
                currentFrame.Dispose();
            }

            // get new frame
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            currentFrame = (Bitmap)eventArgs.Frame.Clone();

            if (stillImageMode == false)
            {
                // here you can process the frame
                pictureBox1.Image = bitmap;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            videoSource = new VideoCaptureDevice(videoDevicesList[cmbVideoSource.SelectedIndex].MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
            videoSource.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            videoSource.SignalToStop();
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            stillImageMode = true;
            pictureBox1.Image = getCurrentFrame();
        }

        public Bitmap getCurrentFrame() 
        {
            return currentFrame;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string tick = Environment.TickCount.ToString();
                string strPath = "noname" + tick + ".bmp";
                
                currentFrame.Save(strPath);
                Bitmap temp = new Bitmap(strPath);
                
                BarcodeResult[] results = reader.DecodeFile(strPath);
                
                if (results == null)
                {
                    textBox1.Text = "No barcode found.";
                    return;
                }

                string strInfo = "Total barcode(s) found: " + results.Length.ToString() + ".\r\n";
                for (int i = 0; i < results.Length; ++i)
                {
                    BarcodeResult barcode = results[i];
                    strInfo += "Barcode " + (i + 1).ToString() + ":\r\n";
                    strInfo += barcode.BarcodeFormat.ToString() + "\r\n";
                    strInfo += barcode.BarcodeText + "\r\n\n";
                }
                textBox1.Text = strInfo;
            }
            catch (BarcodeReaderException exp)
            {
                MessageBox.Show("Error Code: " + exp.Code.ToString() + "\nError String: " + exp.Message);
            }
        }




    }
}
