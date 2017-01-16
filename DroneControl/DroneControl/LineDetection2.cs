using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using AForge.Imaging.Filters;
using AForge;
using AForge.Imaging;
using AForge.Math.Geometry;

namespace DroneControl
{
    public class LineDetection
    {
        MainForm mainForm;
        DroneController droneController;

        public LineDetection(MainForm mf, DroneController dc)
        {
            mainForm = mf;
            droneController = dc;
        }

        //function that calculates the angle from the drone using the line. Called from mainform frame update.
        public void detectLine()
        {
            Bitmap myBitmap = mainForm.getFrame();

            // lock image
            BitmapData bitmapData = myBitmap.LockBits(
                new Rectangle(0, 0, myBitmap.Width, myBitmap.Height),
                ImageLockMode.ReadWrite,
                myBitmap.PixelFormat);

            // step 1 - turn background to black
            IntRange colorRange = new IntRange(150, 255);
            ColorFiltering colorFilter = new ColorFiltering(colorRange, colorRange, colorRange);
            colorFilter.FillOutsideRange = true; //All colors outside colorRange range are turned black
            colorFilter.ApplyInPlace(bitmapData); //Apply to current bitmap

            // step 2 - locating objects
            BlobCounter blobCounter = new BlobCounter()
            {
                FilterBlobs = true,
                MinHeight = 5,
                MinWidth = 5,
            };
            blobCounter.ProcessImage(bitmapData);

            Blob[] blobs = blobCounter.GetObjectsInformation();

            myBitmap.UnlockBits(bitmapData);

            // step 3 - check objects' type and highlight
            // Check if there's a line on the ground with a minimum surface area of 200 square pixels and get it in the middle of the screen
            for (int i = 0; i < blobs.Length; i++)
            {
                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blobs[i]);
                List<IntPoint> corners;

                if (isQuadrilateralAndBigEnough(edgePoints, out corners))
                {
                    //Find upperleft corner
                    IntPoint upperLeftCorner = corners.Aggregate((curMin, c) => (c.X + c.Y) < (curMin.X + curMin.Y) ? c : curMin);
                    corners.Remove(upperLeftCorner);

                    IntPoint lowerRightCorner = corners.Aggregate((curMax, c) => (c.X + c.Y) > (curMax.X + curMax.Y) ? c : curMax);

                    //Get IntPoint making shortest line
                    IntPoint lowerLeftCorner = findPointMakingLongestOrShortestLine(upperLeftCorner, corners, false);
                    corners.Remove(lowerLeftCorner);

                    IntPoint remainingCorner;
                    if (upperLeftCorner.Y < lowerRightCorner.Y)
                    {
                        //Get IntPoint making shortest line
                        remainingCorner = findPointMakingLongestOrShortestLine(upperLeftCorner, corners, false);
                    }
                    else
                    {
                        //Get IntPoint making longest line
                        remainingCorner = findPointMakingLongestOrShortestLine(upperLeftCorner, corners, true);
                    }

                    /*
                     * We now have two points. 
                     * Enough to calculate the angle of the line compared to the relative horizon of the drone camera feed.
                     */
                    droneController.turnDegrees = calculateAngle(upperLeftCorner, remainingCorner);

                    if(droneController.turnDegrees != 0)
                    {
                        droneController.scanningForAngle = false;
                        droneController.stopCurrentTasks();
                    }
                }
            }
        }

        private bool isQuadrilateralAndBigEnough(List<IntPoint> edgePoints, out List<IntPoint> corners)
        {
            double threshold = 200d;
            SimpleShapeChecker shapeChecker = new SimpleShapeChecker();
            bool isQuadrilateral = shapeChecker.IsQuadrilateral(edgePoints, out corners);
            bool isBigEnough = corners[0].DistanceTo(corners[1]) * corners[1].DistanceTo(corners[2]) > threshold;
            return isQuadrilateral && isBigEnough;
        }

        private int calculateAngle(IntPoint pointA, IntPoint pointB)
        {
            if (pointB.X - pointA.X == 0)
            {
                return 0;
            }
            double angleRadians = Math.Atan(((double)pointB.Y - pointA.Y) / (pointB.X - pointA.X));
            int degrees = (int)Math.Ceiling(angleRadians * (180.0 / Math.PI));
            return degrees;
        }

        private IntPoint findPointMakingLongestOrShortestLine(IntPoint startPoint, List<IntPoint> points, bool getLongest)
        {
            /*
             * Returns the point making the longest or shortest line between itself and startPoint.
             * This depends on the getLongest bool. If true then get the longest, else get shortest line.
             */
            double knownDistance;
            if (getLongest)
            {
                knownDistance = 0d;
            }
            else
            {
                knownDistance = double.MaxValue;
            }

            IntPoint longestOrShortestPoint = startPoint; //Must be initialized
            foreach (IntPoint point in points)
            {
                double currentDistance = startPoint.DistanceTo(point);
                if (getLongest && currentDistance > knownDistance)
                {
                    longestOrShortestPoint = point;
                    knownDistance = currentDistance;
                }
                else if (!getLongest && currentDistance < knownDistance)
                {
                    longestOrShortestPoint = point;
                    knownDistance = currentDistance;
                }
            }
            return longestOrShortestPoint;
        }

    }
}
