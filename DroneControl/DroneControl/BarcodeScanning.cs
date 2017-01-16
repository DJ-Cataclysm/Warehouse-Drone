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
    public class BarcodeScanning
    {
    
        MainForm mainForm;
        DroneController droneController;
        IBarcodeReader reader;

        public BarcodeScanning(MainForm mf, DroneController dc)
        {
            mainForm = mf;
            droneController = dc;
            reader = new BarcodeReader();
        }

        /*
         * function that scans for the barcode, and if the barcode is found updates it in the WMS
         * Called from Mainform frame update 
         */
         public void scanForBarcode()
        {
            // detect and decode the barcode inside the bitmap
            var result = reader.Decode(mainForm.getFrame());

            if (result != null)
            {
                int id = int.MaxValue;
                int.TryParse(result.Text, out id);
                mainForm.wmsForm.productScanned(id);
                droneController.scanningForBarcode = false;
                droneController.stopCurrentTasks();
            }
        }
    }
}
