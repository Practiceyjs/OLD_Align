using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Align
{
     public class COMMON_Type_Setting
    {
        public Cognex.VisionPro.Display.CogDisplay[] Displays = new Cognex.VisionPro.Display.CogDisplay[4];

        Enum.Type Current_Type = Enum.Type.Common;

        public bool[] Position = { false, false, false, false };
        public string[] ComPort = { "", "", "", "", "", "" };
        public int[] Ch = { 0, 0, 0, 0, 0, 0 };
        public string Type_Name = "";
        public int[] Cam = { 0, 0};
        public string[] Light_Name;
        public int[] Direction = { 1, 1, 1 };
        public int DIsplay_Page = 0;
        public bool CCTV = false;
        public double[] Calibration_Pitch = { 0.5, 0.5, 0.5 };
        public bool[] Useing_Comport = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
    }
}
