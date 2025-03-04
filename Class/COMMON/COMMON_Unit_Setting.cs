using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Align
{
    public class COMMON_Unit_Setting
    {
        public COMMON_Type_Setting[] T_Setting = new COMMON_Type_Setting[3];
        public string Unit_Name = "";
        public bool Inspection_Use = false;
        public string[] Bit_Recive_Name;
        public string[] Bit_Send_Name;
        public string[] Word_Recive_Name;
        public string[] Word_Send_Name;
    }
}
