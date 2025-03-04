using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognex.VisionPro;
using System.IO;
using Cognex.VisionPro.ImageProcessing;

namespace T_Align
{
    class Cognex_Camera
    {
        int Camera_Number = 0;
        int Comp_Titck = 0;
        int Triger = 0;
        ICogAcqFifo Fifo;

        public void Camera_Set(ICogAcqFifo _Fifo, int _Camera_Number)
        {
            Fifo = _Fifo;
            Camera_Number = _Camera_Number+1;
            Fifo.OwnedExposureParams.Exposure=250;
            Fifo.OwnedTriggerParams.TriggerModel = CogAcqTriggerModelConstants.FreeRun;
            Fifo.Complete += Image_Send;
        }

        private void Image_Send(object Sender, Cognex.VisionPro.CogCompleteEventArgs e)
        {
            try
            {
                COMMON.Grab_Image[Camera_Number] = Flip_Rotate(Fifo.CompleteAcquire(0, out Comp_Titck, out Triger), Rotate_Select(COMMON.CAMERA_Rotate[Camera_Number - 1].ToString()));
                if (ETC_Option.Live_Use)
                    COMMON.Live_Display[Camera_Number].Image = COMMON.Grab_Image[Camera_Number];
            }
            catch(Exception Ex)
            {
                string ExS = Ex.ToString();
                //COMMON.Cam_Labels[Camera_Number].ForeColor = System.Drawing.Color.Red;
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
