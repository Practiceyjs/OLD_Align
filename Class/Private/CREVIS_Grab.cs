using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crevis.VirtualFG40Library;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace T_Align
{
    class CREVIS_Grab
    {
        VirtualFG40Library VirtualFG40Library = new VirtualFG40Library();
        uint Max_Cam = 0;
        int[] H_Device = { 0,0,0,0,0,0,0,0};

        CREVIS_CAMERA[] CREVIS_CAMERAs;

        public bool[] Crevis_Open(string[] _IP)
        {
            bool[] Connect = { false, false, false, false, false, false, false, false };
            //System Module 생성 및 Library를 초기화
            VirtualFG40Library.InitSystem();

            //연결된 Device의 정보를 Update
            VirtualFG40Library.UpdateDevice();

            //연결 가능한 Device의 개수를 가져온다
            VirtualFG40Library.GetAvailableCameraNum(ref Max_Cam);
            CREVIS_CAMERAs = new CREVIS_CAMERA[Max_Cam];
            for (uint LoopCount = 0; LoopCount < Max_Cam; LoopCount++)
            {
                uint Size = 30;
                byte[] Recive_Ip = new byte[Size];
                int a = VirtualFG40Library.GetEnumDeviceInfo(LoopCount, VirtualFG40Library.MCAM_DEVICEINFO_IP_ADDRESS, Recive_Ip, ref Size);
                Array.Resize(ref Recive_Ip, (int)Size);
                string string_IP = Encoding.Default.GetString(Recive_Ip);
                for (int LoopCount2 = 0; LoopCount2 < COMMON.CAMERA_IP.Length; LoopCount2++)
                {
                    try
                    {
                        if (COMMON.CAMERA_IP[LoopCount2].Substring(0, 10) == string_IP.Substring(0, 10))
                        {
                            int H_Device = 0;
                            VirtualFG40Library.OpenDevice(LoopCount, ref H_Device, false);
                            if (H_Device == -1)
                                continue;
                            CREVIS_CAMERAs[LoopCount] = new CREVIS_CAMERA();
                            Connect[LoopCount] = CREVIS_CAMERAs[LoopCount].Camera_Open(VirtualFG40Library, (uint)LoopCount2 + 1, H_Device);
                        }
                    }
                    catch { }
                }
            }
            return Connect;
        }

        public void Live_Start()
        {
            for (uint LoopCount = 0; LoopCount < Max_Cam; LoopCount++)
            {
                CREVIS_CAMERAs[LoopCount].Live_Start();
            }
        }

        public void Thread_Stop()
        {
            for (uint LoopCount = 0; LoopCount < Max_Cam; LoopCount++)
            {
                CREVIS_CAMERAs[LoopCount].Live_Stop();
            }
        }
    }
}
