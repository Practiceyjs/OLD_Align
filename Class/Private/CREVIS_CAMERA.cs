using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crevis.VirtualFG40Library;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Cognex.VisionPro;

namespace T_Align
{
    class CREVIS_CAMERA
    {
        VirtualFG40Library Cam;
        VirtualFG40Library.CallbackFunc vfgCallback;
        int H_Device = 0;
        uint Cam_Number = 0;

        int width = 0;
        int height = 0;
        Int32 Buffsize = 0;
        byte[] Pointer_Image;

        public bool Camera_Open(VirtualFG40Library _Cam, uint _Cam_Number, int _H_Device)
        {
            IntPtr UserData = new IntPtr();
            try
            {
                Cam = _Cam;
                Cam_Number = _Cam_Number;
                H_Device = _H_Device;
                int Error = 0;
                //Intergerinterger 타입 Feature의 값을 가져온다
                Error = Cam.GetIntReg(H_Device, VirtualFG40Library.MCAM_WIDTH, ref width);
                Error = Cam.GetIntReg(H_Device, VirtualFG40Library.MCAM_HEIGHT, ref height);
                Buffsize = width * height;
                Pointer_Image = new byte[Buffsize];

                //Enumeration 타입 Feature의 값을 설정합니다
                Error = Cam.SetEnumReg(H_Device, VirtualFG40Library.MCAM_ACQUISITION_MODE, VirtualFG40Library.ACQUISITION_MODE_CONTINUOUS);
                vfgCallback = new VirtualFG40Library.CallbackFunc(GrabFunction);

                //Callback 함수를 등록 한다
                Error = Cam.SetCallbackFunction(H_Device, VirtualFG40Library.EVENT_NEW_IMAGE, vfgCallback, UserData);
                Error = Cam.SetEnumReg(H_Device, VirtualFG40Library.MCAM_ACQUISITION_FRAMERATE_ENABLE, VirtualFG40Library.ACQUISITION_FRAMERATE_ENABLE_ON);
                Error = Cam.SetEnumReg(H_Device, VirtualFG40Library.MCAM_PIXEL_FORMAT, VirtualFG40Library.PIXEL_FORMAT_MONO8);

                //Float 타입 Feature에 값을 설정합니다
                Error = Cam.SetFloatReg(H_Device, VirtualFG40Library.MCAM_ACQUISITIONF_RAMERATE, 8);

                //Enumeration 타입 Feature의 값을 설정합니다
                Error = Cam.SetEnumReg(H_Device, VirtualFG40Library.MCAM_DEVICE_FILTER_DRIVER_MODE, VirtualFG40Library.DEVICE_FILTER_DRIVER_MODE_OFF);
            }
            catch { return false; }
            return true;
        }

        public void Live_Start()
        {
            //Acquisition(취득)을 시작
            Cam.AcqStart(H_Device);
            
            //Asynchronous(비동기) 모드로 영상을 획득 할 때 사용
            //Max Delay : 영상 획득 최대 유효 시간
            Cam.GrabStartAsync(H_Device, 0xFFFFFFFF);
        }

        public void Live_Stop()
        {
            Cam.CloseDevice(H_Device);
        }

        private Int32 GrabFunction(Int32 EventID, IntPtr pImage, IntPtr userData)
        {
            if (EventID != VirtualFG40Library.EVENT_NEW_IMAGE)
                return -1;
            uint Image_Enable = 0;
            //현재 새로운 영상이 획득 되었는지를 확일 할 때 사용
            Cam.GetImageAvailable(H_Device, ref Image_Enable);

            try
            {
                if (Image_Enable == 1)
                {
                    //Format8bppIndexed : Bitmap 객체를 8bit Gray Array로 담을 때 사용
                    using (Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed))
                    {
                        Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                        BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
                        IntPtr ptr = bmpData.Scan0;
                        //이미지의 전체 크기 : Stride * Height
                        //Stride : Width * byteperpixel + Padding
                        //BytePerPixel : 픽셀당 Byte수
                        int size = bmpData.Stride * bmp.Height;
                        Marshal.Copy(pImage, Pointer_Image, 0, size);
                        Marshal.Copy(Pointer_Image, 0, ptr, size);
                        bmp.UnlockBits(bmpData);
                        COMMON.Grab_Image[Cam_Number] = new CogImage8Grey(bmp);
                        if (ETC_Option.Live_Use)
                            COMMON.Live_Display[Cam_Number].Image = COMMON.Grab_Image[Cam_Number];
                    }
                }
                return 0;
            }
            catch(Exception Ex)
            {
                COMMON.Cam_Labels[Cam_Number].ForeColor = System.Drawing.Color.Red;
                return 1;
            }
        }
    }
}
