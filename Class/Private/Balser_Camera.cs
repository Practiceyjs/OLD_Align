using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basler.Pylon;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Cognex.VisionPro;
using Cognex.VisionPro.ImageProcessing;
using System.IO;

namespace T_Align
{
    class Balser_Camera
    {
        Camera Camera = null;
        int Camera_Number = 0;
        public bool Balser_Camera_Open(ICameraInfo Camera_Info, int _Camera_Number)
        {
            try
            {
                Camera_Number = _Camera_Number;
                Camera = new Camera(Camera_Info);
                Camera.CameraOpened += Configuration.AcquireContinuous;
                Camera.StreamGrabber.ImageGrabbed += Image_Reciveing;
                Camera.Open();
                IFloatParameter Exposure = Camera.Parameters[PLCamera.ExposureTimeAbs];
                IIntegerParameter Digtal_Shift = Camera.Parameters[PLCamera.DigitalShift];
                Exposure.SetValue(250000);
                Digtal_Shift.SetValue(0);
                Configuration.AcquireContinuous(Camera, null);
                Camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
            }
            catch { return false; }
            return true;
        }

        public void Close()
        {
            Camera.Close();
            Camera.Dispose();
        }

        private void Image_Reciveing(Object sender, ImageGrabbedEventArgs e)
        {
            try
            {
                IGrabResult grabResult = e.GrabResult;

                if (grabResult.IsValid)
                {
                    Bitmap bitmap = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format8bppIndexed);
                    BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                    int size = bmpData.Stride * bitmap.Height;
                    byte[] Data = new byte[size];
                    IntPtr ptrBmp = bmpData.Scan0;
                    Marshal.Copy(grabResult.PixelDataPointer, Data, 0, size);
                    Marshal.Copy(Data, 0, ptrBmp, size);
                    bitmap.UnlockBits(bmpData);
                    COMMON.Grab_Image[Camera_Number] = Flip_Rotate(new CogImage8Grey(bitmap), Rotate_Select(COMMON.CAMERA_Rotate[Camera_Number - 1].ToString()));
                }
            }
            catch (Exception exception)
            {
            }
            finally
            {
                e.DisposeGrabResultIfClone();
            }
        }

        private ICogImage Flip_Rotate(ICogImage _Image, CogIPOneImageFlipRotateOperationConstants F_R)
        {
            CogIPOneImageFlipRotate cogIPOneImageFlipRotate = new CogIPOneImageFlipRotate
            {
                OperationInPixelSpace = F_R
            };
            ICogImage _Output_Image = _Image;

            // 이미지 회전이 있을 경우에만 동작
            if (CogIPOneImageFlipRotateOperationConstants.None != cogIPOneImageFlipRotate.OperationInPixelSpace)
            {
                CogRectangle Rec = new CogRectangle();
                Rec.SetXYWidthHeight(0, 0, _Image.Width, _Image.Height);
                CogImage8Grey FlipImage = ((CogImage8Grey)cogIPOneImageFlipRotate.Execute(_Image, CogRegionModeConstants.PixelAlignedBoundingBox, Rec));

                FlipImage.PixelFromRootTransform = FlipImage.GetTransform("#", "#");
                _Output_Image = FlipImage;
            }
            return _Output_Image;
        }

        private CogIPOneImageFlipRotateOperationConstants Rotate_Select(string _Rotate)
        {
            switch (_Rotate)
            {
                case "90":
                    return CogIPOneImageFlipRotateOperationConstants.Rotate90Deg;
                case "180":
                    return CogIPOneImageFlipRotateOperationConstants.Rotate180Deg;
                case "270":
                    return CogIPOneImageFlipRotateOperationConstants.Rotate270Deg;
                case "FLIP":
                    return CogIPOneImageFlipRotateOperationConstants.Flip;
                case "FLIP90":
                    return CogIPOneImageFlipRotateOperationConstants.FlipAndRotate90Deg;
                case "FLIP180":
                    return CogIPOneImageFlipRotateOperationConstants.FlipAndRotate180Deg;
                case "FLIP270":
                    return CogIPOneImageFlipRotateOperationConstants.FlipAndRotate270Deg;
                default:
                    return CogIPOneImageFlipRotateOperationConstants.None;
            }
        }
    }
}
