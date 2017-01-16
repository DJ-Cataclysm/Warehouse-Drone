using AR.Drone.Client;
using AR.Drone.Client.Configuration;
using AR.Drone.Data;
using AR.Drone.Data.Navigation;
using RoutePlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using WMS;
using System.Threading.Tasks;
using ZXing;
using System.Drawing;
using System.Drawing.Imaging;
using AForge.Imaging.Filters;
using AForge;
using AForge.Imaging;
using AForge.Math.Geometry; 

namespace DroneControl
{
    class LineDetection
    {
        MainForm mainForm;
        DroneController droneController;
        public LineDetection(MainForm mf, DroneController dc)
        {
            mainForm = mf;
            droneController = dc;
        }

        public async Task zoekLijn()
        {
            Bitmap videoFrame = mainForm.getFrame();
            BitmapData bitmapData = createFilteredBitMap(videoFrame);
            SimpleShapeChecker shapeChecker = new SimpleShapeChecker();
            Graphics g = Graphics.FromImage(videoFrame);
            BlobCounter blobCounter = new BlobCounter();
            Blob[] blobs = findBlobs(blobCounter, bitmapData);
            videoFrame.UnlockBits(bitmapData);

            // Check if there's a line on the ground and get it in the middle of the screen
            for (int i = 0, n = blobs.Length; i < n; i++)
            {
                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blobs[i]);
                List<IntPoint> corners;
                if (shapeChecker.IsQuadrilateral(edgePoints, out corners))
                {


                    if (mainForm.scanningForLine)
                    {
                        mainForm.scanningForLine = false;
                        await Task.Delay(350);
                        droneController.stopCurrentTasks();
                    }
                   
                }
            }
            mainForm.pbVideo.Image = videoFrame;
            g.Dispose();
        }

        private BitmapData createFilteredBitMap(Bitmap frame)
        {
            // Lock image to prevent other sources from interfering
            BitmapData bitmapData = frame.LockBits(
                new Rectangle(0, 0, frame.Width, frame.Height),
                ImageLockMode.ReadWrite, frame.PixelFormat);

            // Turn anything that isn't white, into black
            ColorFiltering colorFilter = new ColorFiltering();
            colorFilter.Red = new IntRange(160, 255);
            colorFilter.Green = new IntRange(160, 255);
            colorFilter.Blue = new IntRange(160, 255);
            colorFilter.FillOutsideRange = true;
            colorFilter.ApplyInPlace(bitmapData);

            return bitmapData;
        }

        private Blob[] findBlobs(BlobCounter counter, BitmapData bmpData)
        {
            // Find the corners in the frame and identify them
            counter.FilterBlobs = true;
            counter.MinHeight = 5;
            counter.MinWidth = 5;
            counter.ProcessImage(bmpData);
            Blob[] blobs = counter.GetObjectsInformation();

            return blobs;
        }
    }
}
