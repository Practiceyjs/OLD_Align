using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Align
{
    class ClassInterface
    {
        public static string[] PLC_Common = new string[] { "PLC ALIVE", "PLC AUTO MODE", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
        public static string[] PC_Common = new string[] { "PC ALIVE", "PC AUTO MODE", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
        public static string[] PLC_Align = new string[] { "READY", "ERROR", "RESET", "1St Ack", "Calibration Start", "Calibration End", "Align Start", "Align End", "ReAlign", "OCA_WINDOW", "", "2nd Ack", "Move Ack", "Move Done Req", "", "" };
        public static string[] PC_Align = new string[] { "READY", "ERROR", "RESET", "1St Req", "Calibration Start", "Calibration End", "Align Start", "Align End", "Align OK", "Align NG", "", "2nd Req", "Move Req", "Move Done Ack", "", "" };
        
        //기존 21.02.03
        //public static string[] PLC_Align_Offset = new string[] { "CURRENT X1", "", "CURRENT X2", "", "CURRENT Y1", "", "CURRENT Y2", "", "CURRENT CAM X", "", "CURRENT CAM Y", "", "", "", "", "" };
        //public static string[] PC_Align_Offset = new string[] { "TARGET X1", "", "TARGET X2", "", "TARGET Y1", "", "TARGET Y2", "", "", "", "", "", "", "", "", "" };

        public static string[] PLC_Align_Offset = new string[] { "CURRENT X1", "", "CURRENT X2", "", "CURRENT Y1", "", "CURRENT Y2", "", "SHUTTLE Y", "", "CURRENT CAM X", "", "CURRENT CAM Y", "", "", "" };
        public static string[] PC_Align_Offset = new string[] { "TARGET X1", "", "TARGET X2", "", "TARGET Y1", "", "TARGET Y2", "", "", "", "", "", "", "", "", "" };

        public static bool[] Bit_Data(int _data)
        {
            bool[] return_Bool = new bool[16];
            return_Bool[0] = (_data & 0x0001).Equals(0x0001);
            return_Bool[1] = (_data & 0x0002).Equals(0x0002);
            return_Bool[2] = (_data & 0x0004).Equals(0x0004);
            return_Bool[3] = (_data & 0x0008).Equals(0x0008);
            return_Bool[4] = (_data & 0x0010).Equals(0x0010);
            return_Bool[5] = (_data & 0x0020).Equals(0x0020);
            return_Bool[6] = (_data & 0x0040).Equals(0x0040);
            return_Bool[7] = (_data & 0x0080).Equals(0x0080);
            return_Bool[8] = (_data & 0x0100).Equals(0x0100);
            return_Bool[9] = (_data & 0x0200).Equals(0x0200);
            return_Bool[10] = (_data & 0x0400).Equals(0x0400);
            return_Bool[11] = (_data & 0x0800).Equals(0x0800);
            return_Bool[12] = (_data & 0x1000).Equals(0x1000);
            return_Bool[13] = (_data & 0x2000).Equals(0x2000);
            return_Bool[14] = (_data & 0x4000).Equals(0x4000);
            return_Bool[15] = (_data & 0x8000).Equals(0x8000);
            return return_Bool;
        }
    }
}
