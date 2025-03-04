using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basler.Pylon;

namespace T_Align
{
    class Balser_Grabber
    {
        string[] IP_Address;
        Balser_Camera[] balser_Cameras;
        public bool[] Balser_Grabber_Connect(string[] _IP_Address)
        {
            bool[] Connect = new bool[8];
            IP_Address = _IP_Address;
            try
            {
                // Ask the camera finder for a list of camera devices.
                List<ICameraInfo> allCameras = CameraFinder.Enumerate();
                balser_Cameras = new Balser_Camera[_IP_Address.Length];
                for (int LoopCount = 0; LoopCount < _IP_Address.Length; LoopCount++)
                {
                    foreach (ICameraInfo cameraInfo in allCameras)
                    {
                        string IP_Info = cameraInfo[CameraInfoKey.DeviceIpAddress];
                        if (IP_Cutting(IP_Address[LoopCount]) == IP_Cutting(IP_Info))
                        {
                            balser_Cameras[LoopCount] = new Balser_Camera();
                            Connect[LoopCount] = balser_Cameras[LoopCount].Balser_Camera_Open(cameraInfo, LoopCount + 1);
                        }
                    }
                }
                //ICameraInfo selectedCamera = null;
                //foreach (ICameraInfo cameraInfo in allCameras)
                //{
                //    if (cameraInfo[CameraInfoKey.FriendlyName] == camera_Selecter.Select_Camera)
                //    {
                //        selectedCamera = cameraInfo;
                //        break;
                //    }
                //}
                //if (selectedCamera == null)
                //    return;
                //camera = new Camera(selectedCamera);
                //camera.CameraOpened += Configuration.AcquireContinuous;
                //camera.StreamGrabber.ImageGrabbed += Image_Reciveing;
                //camera.Open();
                //Configuration.AcquireContinuous(camera, null);
                //camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
            }
            catch (Exception exception)
            {
            }
            return Connect;
        }

        public void Balser_Close()
        {
            for (int LoopCount = 0; LoopCount < IP_Address.Length; LoopCount++)
            {
                if (balser_Cameras[LoopCount] != null)
                    balser_Cameras[LoopCount].Close();
            }
        }

        private string IP_Cutting(string IP_Address)
        {
            int a = 0;
            a = IP_Address.IndexOf('.', a);
            a = IP_Address.IndexOf('.', a + 1);
            a = IP_Address.IndexOf('.', a + 1);

            return IP_Address.Substring(0,a);
        }
    }
}
