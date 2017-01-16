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
    class BarcodeScanning
    {
    
        MainForm mainForm;
        DroneController droneController;

        public BarcodeScanning(MainForm mf, DroneController dc)
        {
            mainForm = mf;
            droneController = dc;
        }

        /*function that scans for the barcode, and if the barcode is found updates it in the WMS
      * Called from Mainform frame update */
         public void scanForBarcode()
        {
            string barcode;

            // create a barcode reader instance
            IBarcodeReader reader = new BarcodeReader();
            // detect and decode the barcode inside the bitmap
            var result = reader.Decode(mainForm.getFrame());
            // return result or error
            if (result != null)
            {
                barcode = result.Text;
            }
            else { barcode = null; }

            if (barcode != null)
            {
                Console.WriteLine("Barcode gevoden: " + barcode);
                int id = int.MaxValue;
                int.TryParse(barcode, out id);
                mainForm.wmsForm.productScanned(id);
                mainForm.scanningForBarcode = false;

                if (mainForm.scanningForBarcode)
                {
                    mainForm.scanningForBarcode = false;
                     droneController.stopCurrentTasks();
                }
            }
        }
    }
}
