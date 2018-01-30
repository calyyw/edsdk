using EDSDKLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CanonSDKTutorial
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           Application.Run(new MainForm());

            /*
            SDKHandler CameraHandler = new SDKHandler();//.TakePhoto();
            List<Camera> cams = CameraHandler.GetCameraList();
            Camera c = null;
            if (cams.Count == 1)
            {

            foreach (Camera item in cams)
            {
                c = item;
            }
            }
            if (null != c)
            {
                CameraHandler.OpenSession(c);
                Console.WriteLine("打开会话");
                CameraHandler.TakePhoto();

            }
            */

        }

      
    }
}
