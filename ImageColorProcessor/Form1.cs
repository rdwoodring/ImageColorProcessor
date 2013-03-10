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

        //TODO: ugh this is cumbersome.  Let's create a color class later
        public double redl, reda, redb, greenl, greena, greenb, yellowl, yellowa, yellowb, bluel, bluea, blueb, orangel, orangea, orangeb, pinkl, pinka, pinkb, teall, teala, tealb, purplel, purplea, purpleb, brownl, browna, brownb, grayl, graya, grayb, blackl, blacka, blackb, whitel, whitea, whiteb;
        
        //the converted LAB values for a pixel
        public double L, a, b;

        public Form1()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
            InitializeColorLibrary();
        }

        //TODO: ugh this is cumbersome.  Let's create a color class later
        private void InitializeColorLibrary()
        {
            redl = 53.2328817894157;
            reda = 80.1093095249555;
            redb = 67.2200683094151;

            greenl = 46.228817846502;
            greena = -51.6996473266658;
            greenb = 49.8979523093553;

            yellowl = 97.1382469815799;
            yellowa = -21.5559083327789;
            yellowb = 94.4824854400185;

            bluel = 32.3025866714813;
            bluea = 79.1966617869831;
            blueb = -107.863681038521;

            orangel = 74.9321948475493;
            orangea = 23.9360490682307;
            orangeb = 78.9563071717174;

            pinkl = 83.5847988592782;
            pinka = 24.1496610104581;
            pinkb = 3.31538715087307;

            teall = 48.2560738171712;
            teala = -28.8415594623173;
            tealb = -8.48105008590958;

            purplel = 29.7821000963544;
            purplea = 58.939837317824;
            purpleb = -36.4979299618299;

            brownl = 37.5218297481742;
            browna = 49.6997828773777;
            brownb = 30.5402671893619;

            grayl = 53.5850134557251;
            graya = 0.00315562034780559;
            grayb = -0.00624356603593501;

            blackl = 0;
            blacka = 0;
            blackb = 0;

            whitel = 100;
            whitea = 0.0052604999577488;
            whiteb = -0.0104081845242465;
        }

        //private void EvaluateColorsEuclidean(BackgroundWorker worker, DoWorkEventArgs e)
        //{
        //    long redCount, greenCount, blueCount, orangeCount, yellowCount, tealCount, purpleCount, pinkCount, whiteCount, grayCount, blackCount, transparentCount; //these keep track of how many of each color pixel there is
        //    redCount = 0;
        //    greenCount = 0;
        //    blueCount = 0;
        //    orangeCount = 0;
        //    yellowCount = 0;
        //    tealCount = 0;
        //    purpleCount = 0;
        //    pinkCount = 0;
        //    whiteCount = 0;
        //    grayCount = 0;
        //    blackCount = 0;
        //    transparentCount = 0;

        //    int progress = 0;

        //    //starting loop to itierate through each x value
        //    for (int x = 0; x < this.myImage.Width; x++)
        //    {
        //        //starting loop to iterate through each y value
        //        for (int y = 0; y < this.myImage.Height; y++)
        //        {
        //            if (worker.CancellationPending == true)
        //            {
        //                e.Cancel = true;
        //                break;
        //            }
        //            else
        //            {
        //                //grab the pixel.  might need to change this later to use LockBits.  This is quite a heavy task as is, espeically for large images
        //                Color pixelColor = this.myImage.GetPixel(x, y);

        //                //if a pixel is completely transparent, we are going to basically ignore it, 'cause we can't see it
        //                if (pixelColor.A <= 0)
        //                {
        //                    transparentCount++;
        //                }
        //                //otherwise, let's do a little more evaluation, shall we?
        //                else
        //                {
        //                    double distanceFromRed, distanceFromGreen, distanceFromYellow, distanceFromBlue, distanceFromOrange, distanceFromPink, distanceFromPurple;
        //                    distanceFromRed = Math.Sqrt(Math.Pow((redr - pixelColor.R),2)  + Math.Pow((redg - pixelColor.G), 2) + Math.Pow((redb - pixelColor.B), 2));
        //                    distanceFromGreen = Math.Sqrt(Math.Pow((greenr - pixelColor.R), 2) + Math.Pow((greeng - pixelColor.G), 2) + Math.Pow((greenb - pixelColor.B), 2));
        //                    distanceFromYellow = Math.Sqrt(Math.Pow((yellowr - pixelColor.R), 2) + Math.Pow((yellowg - pixelColor.G), 2) + Math.Pow((yellowb - pixelColor.B), 2));
        //                    distanceFromBlue = Math.Sqrt(Math.Pow((bluer - pixelColor.R), 2) + Math.Pow((blueg - pixelColor.G), 2) + Math.Pow((blueb - pixelColor.B), 2));
        //                    distanceFromOrange = Math.Sqrt(Math.Pow((oranger - pixelColor.R), 2) + Math.Pow((orangeg - pixelColor.G), 2) + Math.Pow((orangeb - pixelColor.B), 2));
        //                    distanceFromPink = Math.Sqrt(Math.Pow((pinkr - pixelColor.R), 2) + Math.Pow((pinkg - pixelColor.G), 2) + Math.Pow((pinkb - pixelColor.B), 2));
        //                    distanceFromPurple = Math.Sqrt(Math.Pow((purpler - pixelColor.R), 2) + Math.Pow((purpleg - pixelColor.G), 2) + Math.Pow((purpleb - pixelColor.B), 2));

        //                    if ((distanceFromRed < distanceFromGreen) && (distanceFromRed>distanceFromYellow) && (distanceFromRed<distanceFromBlue) && (distanceFromRed < distanceFromOrange) && (distanceFromRed<distanceFromPink) && (distanceFromRed<distanceFromPurple))
        //                    {
        //                        redCount++;
        //                    }
        //                    else if ((distanceFromGreen < distanceFromRed) && (distanceFromGreen < distanceFromYellow) && (distanceFromGreen < distanceFromBlue) && (distanceFromGreen < distanceFromOrange) && (distanceFromGreen < distanceFromPink) && (distanceFromGreen < distanceFromPurple))
        //                    {
        //                        greenCount++;
        //                    }
        //                    else if ((distanceFromYellow < distanceFromRed) && (distanceFromYellow < distanceFromGreen) && (distanceFromYellow < distanceFromBlue) && (distanceFromYellow < distanceFromOrange) && (distanceFromYellow < distanceFromPink) && (distanceFromYellow < distanceFromPurple))
        //                    {
        //                        yellowCount++;
        //                    }
        //                    else if ((distanceFromBlue < distanceFromRed) && (distanceFromBlue < distanceFromGreen) && (distanceFromBlue < distanceFromYellow) && (distanceFromBlue < distanceFromOrange) && (distanceFromBlue < distanceFromPink) && (distanceFromBlue < distanceFromPurple))
        //                    {
        //                        blueCount++;
        //                    }
        //                    else if ((distanceFromOrange < distanceFromRed) && (distanceFromOrange < distanceFromGreen) && (distanceFromOrange < distanceFromYellow) && (distanceFromOrange < distanceFromBlue) && (distanceFromOrange < distanceFromPink) && (distanceFromOrange < distanceFromPurple))
        //                    {
        //                        orangeCount++;
        //                    }
        //                    else if ((distanceFromPink < distanceFromRed) && (distanceFromPink < distanceFromGreen) && (distanceFromPink < distanceFromYellow) && (distanceFromPink < distanceFromBlue) && (distanceFromPink < distanceFromOrange) && (distanceFromPink < distanceFromPurple))
        //                    {
        //                        pinkCount++;
        //                    }
        //                    else if ((distanceFromPurple < distanceFromRed) && (distanceFromPurple < distanceFromGreen) && (distanceFromPurple < distanceFromYellow) && (distanceFromPurple < distanceFromBlue) && (distanceFromPurple < distanceFromOrange) && (distanceFromPurple < distanceFromPink))
        //                    {
        //                        purpleCount++;
        //                    }
        //                }
        //            }

        //            //lets calculate our progress
        //            progress++;
        //        }
        //        //now lets report our progress back to the GUI so we can update our progress bar
        //        worker.ReportProgress(progress);
        //    }
        //    e.Result = "The image is composed as follows:\r\n" +
        //                   "Red: " + (((double)redCount / totalArea) * 100).ToString() + "%\r\nOrange: " + (((double)orangeCount / totalArea) * 100).ToString() +
        //                   "%\r\nYellow: " + (((double)yellowCount / totalArea) * 100).ToString() + "%\r\nGreen: " + (((double)greenCount / totalArea) * 100).ToString() +
        //                   "%\r\nTeal: " + (((double)tealCount / totalArea) * 100).ToString() + "%\r\nBlue: " + (((double)blueCount / totalArea) * 100).ToString() +
        //                   "%\r\nPurple: " + (((double)purpleCount / totalArea) * 100).ToString() + "%\r\nPink: " + (((double)pinkCount / totalArea) * 100).ToString() + "%\r\nWhite: " +
        //                   (((double)whiteCount / totalArea) * 100).ToString() + "%\r\nGray: " + (((double)grayCount / totalArea) * 100).ToString() +
        //                   "%\r\nBlack: " + (((double)blackCount / totalArea) * 100).ToString() + "%\r\nTransparent: " + (((double)transparentCount / totalArea) * 100).ToString() + "%";

        //}

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
                myImage = new Bitmap(@"C:\Users\rswoody\Desktop\flower.jpg");

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

            //EvaluateImage(worker, e);            
            //EvaluateColorsEuclidean(worker, e);
            EvaluateColorsDelta(worker, e);
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

        private void ConvertRGBToLAB(double red, double green, double blue)
        {
            red /= 255;
            green /= 255;
            blue /= 255;

            //making the RGB values linear and in the nominal range b/t 0.0 and 1.0
            if (red > 0.04045)
                red = Math.Pow(((red + 0.055) / 1.055), 2.4);
            else
                red = red / 12.92;

            if (green > 0.04045)
                green = Math.Pow(((green + 0.055) / 1.055), 2.4);
            else
                green = green / 12.92;

            if (blue > 0.04045)
                blue = Math.Pow(((blue + 0.055) / 1.055), 2.4);
            else
                blue = blue / 12.92;

            red *= 100;
            green *= 100;
            blue *= 100;

            //converting to XYZ color space
            double x, y, z;
            x = red * 0.4124 + green * 0.3576 + blue * 0.1805;
            y = red * 0.2126 + green * 0.7152 + blue * 0.0722;
            z = red * 0.0193 + green * 0.1192 + blue * 0.9505;

            //finally, converting XYZ color space to CIE-L*ab color space
            x /= 95.047;
            y /= 100;
            z /= 108.883;

            if (x > 0.008856)
                x = Math.Pow(x, (.3333333333));
            else
                x = (7.787 * x) + (16 / 116);

            if (y > 0.008856)
                y = Math.Pow(y, (.3333333333));
            else
                y = (7.787 * y) + (16 / 116);

            if (z > 0.008856)
                z = Math.Pow(z, (.3333333333));
            else
                z = (7.787 * z) + (16 / 116);

            //last step
            //double L, a, b;
            L = (116 * y) - 16;
            a = 500 * (x - y);
            b = 200 * (y - z);
        }

        private void EvaluateColorsDelta(BackgroundWorker worker, DoWorkEventArgs e)
        {
            long redCount, greenCount, blueCount, orangeCount, yellowCount, tealCount, purpleCount, pinkCount, whiteCount, grayCount, blackCount, brownCount, transparentCount; //these keep track of how many of each color pixel there is
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
            brownCount = 0;
            transparentCount = 0;

            int progress = 0;

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

                        //pass the pixel to our conversion method.
                        //this method will convert RGB colorspace to XYZ color space
                        //then XYZ colorspace to L*ab color space
                        //the resulting LAB values are in public variables L, a, b
                        ConvertRGBToLAB(pixelColor.R, pixelColor.G, pixelColor.B);

                        //let's set up some variables to hold our distances
                        double distfromred=0, distfromgreen=0, distfromblue=0, distfromorange=0, distfromyellow=0, distfromteal=0, distfrompurple=0, 
                            distfrompink=0, distfromwhite=0, distfromgray=0, distfromblack=0, distfrombrown=0;

                        //now let's call our comparison method
                        distfromred = CalculateDeltaE(redl, reda, redb, L, a, b);
                        distfromgreen = CalculateDeltaE(greenl, greena, greenb, L, a, b);
                        distfromblue = CalculateDeltaE(bluel, bluea, blueb, L, a, b);
                        distfromorange = CalculateDeltaE(orangel, orangea, orangeb, L, a, b);
                        distfromyellow = CalculateDeltaE(yellowl, yellowa, yellowb, L, a, b);
                        distfromteal = CalculateDeltaE(teall, teala, tealb, L, a, b);
                        distfrompurple = CalculateDeltaE(purplel, purplea, purpleb, L, a, b);
                        distfrompink = CalculateDeltaE(pinkl, pinka, pinkb, L, a, b);
                        distfromwhite = CalculateDeltaE(whitel, whitea, whiteb, L, a, b);
                        distfromgray = CalculateDeltaE(grayl, graya, grayb, L, a, b);
                        distfromblack = CalculateDeltaE(blackl, blacka, blackb, L, a, b);
                        distfrombrown = CalculateDeltaE(brownl, browna, brownb, L, a, b);
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
                           "%\r\nBlack: " + (((double)blackCount / totalArea) * 100).ToString() + "%\r\nBrown: " + (((double)brownCount/totalArea)*100).ToString() + "%\r\nTransparent: " + (((double)transparentCount / totalArea) * 100).ToString() + "%";

        }

        private double CalculateDeltaE(double basel, double basea, double baseb, double pixell, double pixela, double pixelb)
        {
            double distancebetween=0;

            //delta-3 1976
            distancebetween = Math.Sqrt(Math.Pow(basel - pixell, 2) + Math.Pow(basea - pixela, 2) + Math.Pow(baseb - pixelb, 2));

            //delta-e94 (easyrgb.com)
            //double whitel = 1, whitec = 1, whiteh = 1, xc1, xc2, xdl, xdc, xde, xdh, xsc, xsh;

            //xc1 = Math.Sqrt((Math.Pow(basea, 2)) + (Math.Pow(baseb, 2)));
            //xc2 = Math.Sqrt((Math.Pow(pixela, 2)) + (Math.Pow(pixelb, 2)));
            //xdl = pixell - basel;
            //xdc = xc2 - xc1;
            //xde = Math.Sqrt(((basel - pixell) * (basel - pixell)) + ((basea - pixela) * (basea * pixela)) + ((baseb - pixelb) * (baseb - pixelb)));

            //if (Math.Sqrt(xde) > Math.Sqrt(Math.Abs(xdl)) + Math.Sqrt(Math.Abs(xdc)))
            //    xdh = Math.Sqrt((xde * xde) - (xdl * xdl) - (xdc * xdc));
            //else
            //    xdh = 0;

            //xsc = 1 + (.045 * xc1);
            //xsh = 1 + (.015 * xc1);
            //xdl /= whitel;
            //xdc /= whitec * xsc;
            //xdh /= whiteh * xsh;

            //distancebetween = Math.Sqrt(Math.Pow(xdl, 2) + Math.Pow(xdc, 2) + Math.Pow(xdh, 2));

            
            //delta-e94 (bruce lindbloom)
            //double deltal, kl, sl, deltac, c1, c2, kc, sc, deltah, deltaa, deltab, kh, sh;

            //deltal = basel - pixell;
            //kl = 1;
            //sl = 1;
            //c1 = (Math.Sqrt(Math.Pow(basea, 2) + Math.Pow(baseb, 2)));
            //c2 = (Math.Sqrt(Math.Pow(pixela, 2) + Math.Pow(pixelb, 2)));
            //deltac = c1 - c2;
            //kc = 1;
            //sc = 1 + (.045 * c1);
            //deltaa = Math.Sqrt((Math.Pow(basea, 2) + Math.Pow(pixela, 2)));
            //deltab = Math.Sqrt((Math.Pow(baseb, 2) + Math.Pow(pixelb, 2)));
            //deltah = Math.Sqrt((Math.Pow(deltaa, 2) + Math.Pow(deltab, 2) - Math.Pow(deltac, 2)));
            //kh = 1;
            //sh = 1 + (.015 * c1);

            //distancebetween = Math.Sqrt(((Math.Pow((deltal / kl * sl), 2)) + (Math.Pow((deltac / kc * sc), 2)) + (Math.Pow((deltah / kh * sh), 2))));

            return distancebetween;
        }
    }
}
