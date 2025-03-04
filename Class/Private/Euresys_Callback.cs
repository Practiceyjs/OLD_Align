using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Euresys;
using System.Drawing;
using System.Runtime.InteropServices;
using Cognex.VisionPro;
using Cognex.VisionPro.ImageProcessing;

namespace T_Align
{
    class Euresys_Callback
    {
        Euresys.EGrabberCallbackSingleThread grabber;
        System.Drawing.Bitmap bitmap;
        UInt64 imgWidth;
        UInt64 imgHeight;
        String imgFormat;
        int Device_Num;

        public bool Euresys_Set(EGenTL eGenTL, int _Open_Num, int _Device_Num)
        {
            try
            {
                Device_Num = _Device_Num;
                grabber = new EGrabberCallbackSingleThread(eGenTL, 0, _Open_Num);
                grabber.runScript("config.js");
                imgWidth = grabber.getWidth();
                imgHeight = grabber.getHeight();
                imgFormat = grabber.getPixelFormat();

                //Format8bppIndexed : Bitmap 객체를 8bit Gray Array로 담을 때 사용
                bitmap = new System.Drawing.Bitmap((int)imgWidth, (int)imgHeight, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                grabber.onNewBufferEvent = delegate (Euresys.EGrabberCallbackSingleThread g, Euresys.NewBufferData data)
                {
                    //생성자는 출력 대기열의 앞쪽에서 버퍼를 사용
                    ScopedBuffer scopedBuffer = new ScopedBuffer(grabber, data);
                    IntPtr imgPtr;

                    //버퍼 정보 취득
                    scopedBuffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_BASE, out imgPtr);
                    System.Drawing.Imaging.BitmapData bmpData = null;
                    try
                    {
                        bmpData = bitmap.LockBits(new Rectangle(0, 0, (int)imgWidth, (int)imgHeight),
                            System.Drawing.Imaging.ImageLockMode.WriteOnly,
                            System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                        IntPtr ptr = bmpData.Scan0;
                        int size = bmpData.Stride * bitmap.Height;
                        byte[] array = new byte[size];
                        Marshal.Copy(imgPtr, array, 0, size);
                        Marshal.Copy(array, 0, ptr, size);
                    }
                    finally
                    {
                        if (bmpData != null)
                        {
                            bitmap.UnlockBits(bmpData);
                            COMMON.Grab_Image[Device_Num] = Flip_Rotate(new CogImage8Grey(bitmap), Rotate_Select(COMMON.CAMERA_Rotate[Device_Num - 1].ToString()));
                            if (ETC_Option.Live_Use)
                                COMMON.Live_Display[Device_Num].Image = COMMON.Grab_Image[Device_Num];
                        }
                    }
                    scopedBuffer = null;
                    grabber.push(data);
                };

                //Enable onCicEvent Callbacks
                grabber.enableCicDataEvent();

                //Grabbber Buffer를 할당한다
                grabber.reallocBuffers(50);
            }
            catch 
            {
                COMMON.Cam_Labels[Device_Num].ForeColor = System.Drawing.Color.Red;
                return false;
            }
            return true;
        }

        public void Grab_Start()
        {
            grabber.start();
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
