using AR.Drone.Infrastructure;
using System;
using System.Windows.Forms;

namespace DroneControl
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string ffmpegPath = string.Format(@"../../../AR.Drone/FFmpeg.AutoGen/FFmpeg/bin/windows/{0}", Environment.Is64BitProcess ? "x64" : "x86");
            InteropHelper.RegisterLibrariesSearchPath(ffmpegPath);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
