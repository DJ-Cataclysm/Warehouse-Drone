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
    class AngleDetection
    {
        MainForm mainForm;
        DroneController droneController;

        public AngleDetection(MainForm mf, DroneController dc)
        {
            mainForm = mf;
            droneController = dc;
        }

        //function that calculates the angle from the drone using the line. Called from mainform frame update.
        public void calculateAngle()
        {
            Bitmap myBitmap = mainForm.getFrame();

            // lock image
            BitmapData bitmapData = myBitmap.LockBits(
            new Rectangle(0, 0, myBitmap.Width, myBitmap.Height),
            ImageLockMode.ReadWrite, myBitmap.PixelFormat);

            // step 1 - turn background to black
            ColorFiltering colorFilter = new ColorFiltering();
            colorFilter.Red = new IntRange(150, 255);
            colorFilter.Green = new IntRange(150, 255);
            colorFilter.Blue = new IntRange(150, 255);
            colorFilter.FillOutsideRange = true;
            colorFilter.ApplyInPlace(bitmapData);

            // step 2 - locating objects
            BlobCounter blobCounter = new BlobCounter();
            blobCounter.FilterBlobs = true;
            blobCounter.MinHeight = 5;
            blobCounter.MinWidth = 5;

            blobCounter.ProcessImage(bitmapData);

            Blob[] blobs = blobCounter.GetObjectsInformation();

            myBitmap.UnlockBits(bitmapData);

            // step 3 - check objects' type and highlight
            SimpleShapeChecker shapeChecker = new SimpleShapeChecker();
            Graphics g = Graphics.FromImage(myBitmap);

            // Check if there's a line on the ground with a minimum surface area of 200 pixels and get it in the middle of the screen
            for (int i = 0, n = blobs.Length; i < n; i++)
            {
                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blobs[i]);
                List<IntPoint> corners;

                if (shapeChecker.IsQuadrilateral(edgePoints, out corners) && corners[0].DistanceTo(corners[1]) * corners[1].DistanceTo(corners[2]) > 200)
                {
                    //Find upperleft corner
                    IntPoint upperLeftCorner = corners.Aggregate((curMin, c) => (c.X + c.Y) < (curMin.X + curMin.Y) ? c : curMin);
                    corners.Remove(upperLeftCorner);

                    IntPoint lowerRightCorner = corners.Aggregate((curMax, c) => (c.X + c.Y) > (curMax.X + curMax.Y) ? c : curMax);

                    IntPoint lowerLeftCorner = findPointMakingShortestLine(upperLeftCorner, corners);
                    corners.Remove(lowerLeftCorner);

                    //Als upperLeft corner hoger zit dan lowerRight, neem kortste lijn, anders de langste lijn.
                    IntPoint remainingCorner;
                    if (upperLeftCorner.Y < lowerRightCorner.Y)
                    {
                        remainingCorner = findPointMakingShortestLine(upperLeftCorner, corners);
                    }
                    else
                    {
                        remainingCorner = findPointMakingLongestLine(upperLeftCorner, corners);
                    }

                    /*
                     * We now have two points. 
                     * Enough to calculate the angle of the line compared to the relative horizon of the drone camera feed.
                     */
                    if (remainingCorner.X - upperLeftCorner.X == 0)
                    {
                        return;
                    }
                    double angleRadians = Math.Atan(((double)remainingCorner.Y - upperLeftCorner.Y) / (remainingCorner.X - upperLeftCorner.X));
                    droneController.turnDegrees = (int)Math.Ceiling(angleRadians * (180.0 / Math.PI));

                    Pen redPen = new Pen(Color.Red, 2);
                    g.DrawLine(redPen, upperLeftCorner.X, upperLeftCorner.Y, remainingCorner.X, remainingCorner.Y);
                }
            }
        }

        private IntPoint findPointMakingLongestLine(IntPoint startPoint, List<IntPoint> points)
        {
            //Returns the point making the longest line between itself and startPoint.
            double longestDistance = 0;
            IntPoint longestPoint = startPoint; //longestPoint must be initialized
            foreach (IntPoint point in points)
            {
                double currentDistance = startPoint.DistanceTo(point);
                if (currentDistance > longestDistance)
                {
                    longestPoint = point;
                    longestDistance = currentDistance;
                }
            }
            return longestPoint;
        }

        private IntPoint findPointMakingShortestLine(IntPoint startPoint, List<IntPoint> points)
        {
            //Returns the point making the shortest line between itself and startPoint.
            double shortestDistance = double.MaxValue;
            IntPoint shortestPoint = startPoint; //shortestPoint must be initialized
            foreach (IntPoint point in points)
            {
                double currentDistance = startPoint.DistanceTo(point);
                if (currentDistance < shortestDistance)
                {
                    shortestPoint = point;
                    shortestDistance = currentDistance;
                }
            }
            return shortestPoint;
        }

    }
}
