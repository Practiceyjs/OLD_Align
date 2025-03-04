using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Align
{
    class LC_MelsecNet
    {

        [DllImport("MDFUNC32.dll")]
        private static extern short mdSendEx(Int32 Path, Int32 Netno, Int32 Stno, Int32 Devtyp, Int32 devno, ref Int32 size, ref short buf);

        #region DLL Import
        [DllImport("MDFUNC32.dll")]
        private static extern short mdOpen(short Chan, short Mode, ref Int32 Path);
        [DllImport("MDFUNC32.dll")]
        private static extern short mdClose(Int32 Path);
        [DllImport("MDFUNC32.dll")]
        private static extern short mdSend(Int32 Path, short Stno, short Devtyp, short Devno, ref short Size, ref short Buf);
        [DllImport("MDFUNC32.dll")]
        private static extern short mdReceive(Int32 Path, short Stno, short Devtyp, short Devno, ref short Size, ref short Buf);
        [DllImport("MDFUNC32.dll")]
        private static extern short mdRandW(Int32 Path, short Stno, short[] Dev, short[] Buf, short bufsiz);
        [DllImport("MDFUNC32.dll")]
        private static extern short mdRandR(Int32 Path, short Stno, short Dev, short Buf, short bufsiz);
        [DllImport("MDFUNC32.dll")]
        private static extern short mdBdLedRead(Int32 Path, ref short buf);
        [DllImport("MDFUNC32.dll")]
        private static extern short mdBdModRead(Int32 Path, short Mode);
        [DllImport("MDFUNC32.dll")]
        private static extern short mdBdModSet(Int32 Path, short Mode);
        [DllImport("MDFUNC32.dll")]
        private static extern short mdBdRst(Int32 Path);
        [DllImport("MDFUNC32.dll")]
        private static extern short mdBdSwRead(Int32 path, ref short buf);
        [DllImport("MDFUNC32.dll")]
        private static extern short mdBdVerRead(Int32 Path, ref short buf);
        [DllImport("MDFUNC32.dll")]
        private static extern short mdWaitBdEvent(Int32 Path, ref short eventno, Int32 timeout, ref short signaledno, ref short details);
        [DllImport("MDFUNC32.dll")]
        private static extern short mdSendEx(Int32 Path, Int32 Netno, Int32 Stno, Int32 Devtyp, Int32 devno, ref Int32 size, short[] buf);
        [DllImport("MDFUNC32.dll")]
        private static extern Int32 mdReceiveEx(Int32 path, Int32 Netno, Int32 Stno, Int32 Devtyp, Int32 devno, ref Int32 size, ref short buf);
        [DllImport("MDFUNC32.dll")]
        private static extern short mdDevSetEx(Int32 path, Int32 Netno, Int32 Stno, Int32 Devtyp, Int32 devno);
        [DllImport("MDFUNC32.dll")]
        private static extern Int32 mdDevRstEx(Int32 Path, Int32 Netno, Int32 Stno, Int32 Devtyp, Int32 devno);
        [DllImport("MDFUNC32.dll")]
        private static extern Int32 mdRandWEx(Int32 path, Int32 Netno, Int32 Stno, Int32[] dev, UInt16[] buf, Int32 bufsiz);
        [DllImport("MDFUNC32.dll")]
        private static extern Int32 mdRandREx(Int32 path, Int32 Netno, Int32 Stno, Int32[] dev, UInt16[] buf, Int32 bufsiz);
        #endregion


        public enum DeviceTypeList
        {
            DevRBM = -32768,
            DevRAB = -32736,
            DevRX = -32735,
            DevRY = -32734,
            DevRW = -32732,
            DevARB = -32704,
            DevSB = -32669,
            DevSW = -32668,
            DevX = 1,
            DevY = 2,
            DevL = 3,
            DevM = 4,
            DevSM = 5,
            DevF = 6,
            DevTT = 7,
            DevTC = 8,
            DevCT = 9,
            DevCC = 10,
            DevTN = 11,
            DevCN = 12,
            DevD = 13,
            DevSD = 14,
            DevTM = 15,
            DevTS = 16,
            DevCM = 17,
            DevCS = 18,
            DevA = 19,
            DevZ = 20,
            DevV = 21,
            DevR = 22,
            DevB = 23,
            DevW = 24,
            DevSTT = 26,
            DevSTC = 27,
            DevQSW = 28,
            DevQV = 30,
            DevMRB = 33,
            DevMAB = 34,
            DevSTN = 35,
            DevWw = 36,
            DevWr = 37,
            DevFS = 40,
            DevSPB = 50,
            DevSPX = 51,
            DevSPY = 52,
            DevUSER = 100,
            DevMAIL = 101,
            DevMAILNC = 102,
            DevSPB1 = 501,
            DevSPB2 = 502,
            DevSPB3 = 503,
            DevSPB4 = 504,
            DevTS2 = 16002,
            DevTS3 = 16003,
            DevCS2 = 18002,
            DevCS3 = 18003
        }

        short Channel = 0;
        short Mode = -1;
        public int Path = 0;

        int Network = 0;
        int Station = 0;
        int Device_Type = (int)DeviceTypeList.DevW;
        int Device_Start_No = 0;
        int Size = 2;

        int Send_Address = 0;
        int Receive_Address = 0;

        public void Melsec_Set(short _Channel, int _Network, int _Station, int _Send_Address, int _Receive_Address)
        {
            Channel = _Channel;
            Network = _Network;
            Station = _Station;
            Send_Address = _Send_Address;
            Receive_Address = _Receive_Address;
        }

        public int Melsec_Open()
        {
            try
            {
                return mdOpen(Channel, Mode, ref Path);
            }
            catch { return 1; }
        }

        public int Melsec_Send_R(UInt16[] _Data)
        {
            int Result = 0;
            Int32[] Dev = new Int32[4];
            Dev[0] = 1;
            Dev[1] = (Int32)DeviceTypeList.DevW;
            Dev[2] = Send_Address;
            Dev[3] = _Data.Length;

            Result = mdRandWEx(Path, Network, Station, Dev, _Data, _Data.Length);
            return Result;
        }

        public int Melsec_Recive_R(out UInt16[] _Data)
        {
            _Data = new UInt16[500];
            int Result = 0;
            Int32[] Dev = new Int32[4];
            Dev[0] = 1;
            Dev[1] = (Int32)DeviceTypeList.DevW;
            Dev[2] = Receive_Address;
            Dev[3] = _Data.Length;

            Result = mdRandREx(Path, Network, Station, Dev, _Data, _Data.Length);
            return Result;
        }
    }
}
