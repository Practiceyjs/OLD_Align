using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Align
{
    class ClassInterface
    {
        static public int Recive_Address = 0;
        static public int Send_Address = 0;
        static public int Channel = 0;
        public static void Word_Write(int _Data_Address, int Value)
        {
            COMMON.Send_Data[_Data_Address] = Value;
            COMMON.Write_Bit = true;
        }

        static private object Lock_Object = new object();

        public static void Bit_On_Off(int _Data_Address, int _Bit_Address, bool _On_Off)
        {
            lock (Lock_Object)
            {
                int Address = (int)Math.Pow(2, _Bit_Address);
                if (_On_Off)
                    COMMON.Send_Data[_Data_Address] = COMMON.Send_Data[_Data_Address] | Address;
                else
                    COMMON.Send_Data[_Data_Address] = COMMON.Send_Data[_Data_Address] & (0xffff - Address);
                COMMON.Write_Bit = true;
            }
        }

        public static bool Bit_Check(int _Data_Address, int _Bit_Address)
        {
            int Address = (int)Math.Pow(2, _Bit_Address);
            if ((COMMON.Receive_Data[_Data_Address] & Address) == Address)
                return true;
            else
                return false;
        }

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
