using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognex.VisionPro.Caliper;

namespace T_Align
{
    static public class Recipe
    {

        /////////////////////////////////////////////////////////////////////
        ///OPTION영역
        static public int Data_Save_Day = 90;
        static public int Data_Storage = 90;
        static public bool L_Check_Use = false;
        static public bool Re_Align_Use = false;
        static public bool N3_Point_Align = false;
        static public bool Line_Projection = false;
        static public bool Orizinal_Image_Save = false;
        static public bool Ratio_Use = false;
        static public bool OK_PASS = false;
        static public bool NG_PASS = false;
        static public bool Auto_Light = false;
        static public bool Hole_Align_Use = false;
        static public int Hole_Align_Number = 0;
        static public bool[] Hole_Align_Position = { false, false, false, false };
        /////////////////////////////////////////////////////////////////////
    }
}
