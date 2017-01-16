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
        DroneController _droneController;
        BlobCounter _blobCounter;
        const int LOWERBOUND_COLOR_RANGE = 150; //Minimum = 0, must be lower than upperbound
        const int UPPERBOUNDBOUND_COLOR_RANGE = 255; //Maximum = 255, must be higher than lowerbound
        const int MINIMUM_SURFACE_AREA_QUADRILATERAL = 200;
        const int BLOB_MINHEIGHT = 5;
        const int BLOB_MINWIDTH = 5;

        public LineDetection(DroneController droneController)
        {
            _droneController = droneController;

            _blobCounter = new BlobCounter()
            {
                FilterBlobs = true,
                MinHeight = BLOB_MINHEIGHT,
                MinWidth = BLOB_MINWIDTH,
            };
        }

        //Function that calculates the angle from the drone using the line. Called from mainform frame update.
        public int detectLine(Bitmap frame)
        {
            //Lock image
            BitmapData bitmapData = frame.LockBits(
                new Rectangle(0, 0, frame.Width, frame.Height),
                ImageLockMode.ReadWrite,
                frame.PixelFormat);

            //Filter image, make all unimportant pixels completely black
            IntRange colorRange = new IntRange(LOWERBOUND_COLOR_RANGE, UPPERBOUNDBOUND_COLOR_RANGE);
            ColorFiltering colorFilter = new ColorFiltering(colorRange, colorRange, colorRange);
            colorFilter.FillOutsideRange = true; //All colors outside colorRange range are turned black
            colorFilter.ApplyInPlace(bitmapData); //Apply to current bitmap

            //Locating objects using a BlobCounter
            _blobCounter.ProcessImage(bitmapData);

            Blob[] blobs = _blobCounter.GetObjectsInformation();

            frame.UnlockBits(bitmapData);

            //Check if there's a line on the ground with a specified minimum surface area
            for (int i = 0; i < blobs.Length; i++)
            {
                List<IntPoint> edgePoints = _blobCounter.GetBlobsEdgePoints(blobs[i]);
                List<IntPoint> corners;

                if (isQuadrilateralAndBigEnough(edgePoints, out corners))
                {
                    //Find upperleft corner
                    IntPoint upperLeftCorner = getUpperLeftCorner(corners);
                    corners.Remove(upperLeftCorner);

                    IntPoint lowerRightCorner = getLowerRightCorner(corners);

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
                     * We now have two points. Enough to calculate the angle of the line compared to 
                     * the relative horizon of the drone camera feed.
                     */
                    int angle = calculateAngle(upperLeftCorner, remainingCorner);
                    return angle;
                }
            }

            return 0;
        }

        private IntPoint getUpperLeftCorner(List<IntPoint> corners)
        {
            IntPoint upperLeftCorner = corners.Aggregate((curMin, c) => (c.X + c.Y) < (curMin.X + curMin.Y) ? c : curMin);
            return upperLeftCorner;
        }

        private IntPoint getLowerRightCorner(List<IntPoint> corners)
        {
            IntPoint lowerRightCorner = corners.Aggregate((curMax, c) => (c.X + c.Y) > (curMax.X + curMax.Y) ? c : curMax);
            return lowerRightCorner;
        }


        private bool isQuadrilateralAndBigEnough(List<IntPoint> edgePoints, out List<IntPoint> corners)
        {
            SimpleShapeChecker shapeChecker = new SimpleShapeChecker();
            bool isQuadrilateral = shapeChecker.IsQuadrilateral(edgePoints, out corners);
            bool isBigEnough = corners[0].DistanceTo(corners[1]) * corners[1].DistanceTo(corners[2]) > MINIMUM_SURFACE_AREA_QUADRILATERAL;
            return isQuadrilateral && isBigEnough;
        }

        private int calculateAngle(IntPoint pointA, IntPoint pointB)
        {
            if (pointB.X - pointA.X == 0)
            {
                return 0;
            }
            double angleRadians = Math.Atan(((double)pointB.Y - pointA.Y) / (pointB.X - pointA.X));
            //Using literal because converting radians to degrees will never change
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
