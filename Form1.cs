using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImageColorProcessor
{
    public partial class Form1 : Form
    {
        //lets set up some variables that we'll need
        public int totalArea; //will hold the total area of the image
        public Bitmap myImage; //the image        

        public Form1()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
        }

        private void InitializeBackgroundWorker()
        {
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;

            backgroundWorker1.DoWork +=
                new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(
            backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.ProgressChanged +=
                new ProgressChangedEventHandler(
            backgroundWorker1_ProgressChanged);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //let's make sure that the textbox, progress bar, and progress label are all set to starting values
            //just in case we ran a job before
            textBox1.Text = "";
            progressBar1.Value = 0;
            lblProgressPercentage.Text = "0%";

            try
            {
                //first things first... let's load up the image we're going to evaluate            
                myImage = new Bitmap(@"C:\Users\rswoody\Documents\Wedding\Ms Mary.jpg");

                //next, let's calculate the image's total area.  we'll need it for several things, now and later
                //so let's just get it out of the way
                totalArea = myImage.Height * myImage.Width;

                //let's set the progress bar up
                progressBar1.Maximum = totalArea;

                //we'll make sure there isn't already a job running
                if (backgroundWorker1.IsBusy != true)
                {
                    //and if there isn't, let's let the user know we're starting
                    textBox1.Text += "Commencing Job\r\n";

                    //enable the cancel button to let users stop the processing
                    button2.Enabled = true;

                    //and disable the start job button for now
                    button1.Enabled = false;

                    //and let's actually start
                    backgroundWorker1.RunWorkerAsync();
                }
                //if there is a job running
                else
                {
                    //let's tell the user so.  they'll have to hit cancel to stop it and start a new job
                    MessageBox.Show("There is already a job in progress.  If you wish to start another, you will have to cancel the current job first.", "Job in Progress", MessageBoxButtons.OK);
                }
            }
            catch (ArgumentException exception)
            {
                textBox1.Text += "Error encountered.  File may not be in specified location.  Please try again.";
            }
        }

        public void EvaluateImage(BackgroundWorker worker, DoWorkEventArgs e)
        {
            long redCount, greenCount, blueCount, orangeCount, yellowCount, tealCount, purpleCount, pinkCount, whiteCount, grayCount, blackCount, transparentCount; //these keep track of how many of each color pixel there is
            redCount = 0;
            greenCount = 0;
            blueCount = 0;
            orangeCount = 0;
            yellowCount = 0;
            tealCount = 0;
            purpleCount = 0;
            pinkCount = 0;
            whiteCount = 0;
            grayCount = 0;
            blackCount = 0;
            transparentCount = 0;

            int progress =0;

            //holds the hue value for some work later
            double hue = 0;

            //starting loop to itierate through each x value
            for (int x = 0; x < this.myImage.Width; x++)
            {
                //starting loop to iterate through each y value
                for (int y = 0; y < this.myImage.Height; y++)
                {
                    if (worker.CancellationPending == true)
                    {
                        e.Cancel = true;
                        break;
                    }
                    else
                    {
                        //grab the pixel.  might need to change this later to use LockBits.  This is quite a heavy task as is, espeically for large images
                        Color pixelColor = this.myImage.GetPixel(x, y);

                        //if a pixel is completely transparent, we are going to basically ignore it, 'cause we can't see it
                        if (pixelColor.A <= 0)
                        {
                            transparentCount++;
                        }
                        //otherwise, let's do a little more evaluation, shall we?
                        else
                        {
                            //if the pixel's saturation is 0, then we're dealing with gray scale.  technically it has to be 0, but it's pretty toug
                            //to tell a difference between 0 and 7, so let's just... you know, expand the grayscale a bit, yes?
                            //but we'll still need to do more evaluation, 'cause white and black are technically grayscale, but we want them to show up as their
                            //own color.
                            if (pixelColor.GetSaturation() * 100 <= 7)
                            {
                                //if the saturation is 0 and the brightness is 100, the pixel is white
                                if (pixelColor.GetBrightness() * 100 == 100)
                                {
                                    whiteCount++;
                                }
                                //otherwise it's gray.  We'll deal with black later b/c saturation doesn't need to be 0 for a pixel to be black
                                else
                                {
                                    grayCount++;
                                }
                            }
                            else
                            {
                                //ok, we're almost done with the special cases.  let's just check to see if the pixel is black real quick
                                //technically a brightness (value) of 0 is black, but values less than 5 look pretty black.
                                //besides, this might let us kick out more pixels pretty quickly
                                if (pixelColor.GetBrightness() * 100 <= 5)
                                {
                                    blackCount++;
                                }
                                else
                                {
                                    //evaluating each pixel's hue
                                    //we'll use this to get a general idea of which color family the image should be placed in
                                    hue = pixelColor.GetHue();
                                    if ((hue >= 350 && hue <= 360) || (hue >= 0 && hue <= 9))
                                        redCount++;
                                    else if (hue >= 10 && hue <= 33)
                                        orangeCount++;
                                    else if (hue >= 34 && hue <= 70)
                                        yellowCount++;
                                    else if (hue >= 71 && hue <= 140)
                                        greenCount++;
                                    else if (hue >= 141 && hue <= 161)
                                        tealCount++;
                                    else if (hue >= 162 && hue <= 248)
                                        blueCount++;
                                    else if (hue >= 249 && hue <= 337)
                                        purpleCount++;
                                    else if (hue >= 338 && hue <= 349)
                                        pinkCount++;
                                }
                            }
                        }
                    }

                    //lets calculate our progress
                    progress++;                                        
                }
                //now lets report our progress back to the GUI so we can update our progress bar
                worker.ReportProgress(progress);
            }
            e.Result = "The image is composed as follows:\r\n" +
                           "Red: " + (((double)redCount / totalArea) * 100).ToString() + "%\r\nOrange: " + (((double)orangeCount / totalArea) * 100).ToString() +
                           "%\r\nYellow: " + (((double)yellowCount / totalArea) * 100).ToString() + "%\r\nGreen: " + (((double)greenCount / totalArea) * 100).ToString() +
                           "%\r\nTeal: " + (((double)tealCount / totalArea) * 100).ToString() + "%\r\nBlue: " + (((double)blueCount / totalArea) * 100).ToString() +
                           "%\r\nPurple: " + (((double)purpleCount / totalArea) * 100).ToString() + "%\r\nPink: " + (((double)pinkCount / totalArea) * 100).ToString() + "%\r\nWhite: " +
                           (((double)whiteCount / totalArea) * 100).ToString() + "%\r\nGray: " + (((double)grayCount / totalArea) * 100).ToString() +
                           "%\r\nBlack: " + (((double)blackCount / totalArea) * 100).ToString() + "%\r\nTransparent: " + (((double)transparentCount/totalArea)*100).ToString() + "%";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.WorkerSupportsCancellation == true)
            {
                if (backgroundWorker1.IsBusy == true)
                {
                    MessageBox.Show("Cancelling the current job", "Cancelling", MessageBoxButtons.OK);

                    // Cancel the asynchronous operation.
                    backgroundWorker1.CancelAsync();
                }
                else
                {
                    MessageBox.Show("There is no job currently in progress", "No Job in Progress", MessageBoxButtons.OK);
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            EvaluateImage(worker, e);            
        }

        // This event handler updates the progress. 
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (progressBar1.Value < progressBar1.Maximum)
            {
                this.progressBar1.Value = e.ProgressPercentage;
            }
            lblProgressPercentage.Text = Math.Round((decimal)((progressBar1.Value / progressBar1.Maximum)*100),0) + "%";
        }

        // This event handler deals with the results of the background operation. 
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                MessageBox.Show("Job cancelled!", "Job Cancelled", MessageBoxButtons.OK);
            }
            else if (e.Error != null)
            {
                MessageBox.Show("Error: " + e.Error.Message.ToString(), "Error", MessageBoxButtons.OK);
            }
            else
            {
                //printing our final results out to the "status" text box.  eventually, we might want to write resutls to a file
                textBox1.Text += e.Result.ToString();

                MessageBox.Show("Job complete.  Results are shown in status box", "Job Complete", MessageBoxButtons.OK);
            }

            //and let's reactivate the start button, then deactivate the cancel button
            button1.Enabled = true;
            button2.Enabled = false;
        }

    }
}
