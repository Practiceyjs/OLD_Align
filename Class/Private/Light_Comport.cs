using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace T_Align
{
    public class Light_Comport
    {
        public SerialPort Serial;

        string COMPORT;
        public int Int_COMPORT;

        public int[] Ch_Val = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public int[] Ch_On = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public Light_Comport(string _COMPORT)
        {
            COMPORT = _COMPORT;
            Int_COMPORT = Convert.ToInt32(COMPORT.Substring(3, COMPORT.Length - 3));
            Serial = new SerialPort(COMPORT, 19200, Parity.None, 8, StopBits.One);
            try
            {
                Serial.Open();
            }
            catch { }
        }

        public void Val_Load()
        {
            for (int i = 1; i <= 16; i++)
            {
                try
                {
                    Ch_Val[i] = Convert.ToInt32(ClassINI.ReadConfig("Light Control", COMPORT + "_CH" + Convert.ToString(i)));
                }
                catch { }
            }
        }

        public void Val_Save(int _Ch, int _Val)
        {
            Ch_Val[_Ch] = _Val;
            ClassINI.WriteConfig("Light Control", COMPORT + "_CH" + Convert.ToString(_Ch), Convert.ToString(Ch_Val[_Ch]));
        }

        public void Auto_Unit_On(bool[] Select, bool On)
        {
            if (On)
            {
                for (int i = 0; i < 16; i++)
                    if (Select[i])
                        Ch_On[i] = Ch_Val[i];
            }
            else
            {
                for (int i = 0; i < 16; i++)
                    if (Select[i])
                        Ch_On[i] = 0;
            }
            //ON OFF 추가 예정
        }

        private Object lockObject = new object();

        public int RS232_TX(int CH_Num, int Bright_Value)
        {
            try
            {
                lock (lockObject)
                {
                    if (ETC_Option.Light_Controler[Int_COMPORT] == "VC")
                    {
                        byte[] Send_data = new byte[8];
                        Send_data[0] = (byte)0x4E;
                        Send_data[1] = (byte)(0x30 + (CH_Num / 10));
                        Send_data[2] = (byte)(0x30 + (CH_Num % 10));
                        Send_data[3] = (byte)(0x30 + (Bright_Value / 100));
                        Send_data[4] = (byte)(0x30 + ((Bright_Value % 100) / 10));
                        Send_data[5] = (byte)(0x30 + (Bright_Value % 10));
                        Send_data[6] = (byte)0x0D;
                        Send_data[7] = (byte)0x0A;
                        Serial.Write(Send_data, 0, 8);
                        int tickCount = Environment.TickCount;
                        byte[] Read_Byte = new byte[100];
                        while (true)
                        {
                            Serial.Read(Read_Byte, 0, 1);
                            if (Read_Byte[0] == 0x06)
                                break;
                            if (Environment.TickCount - tickCount > 1000)
                            {
                                return 1;
                            }
                        }
                    }
                    else if(ETC_Option.Light_Controler[Int_COMPORT] == "TWIM")
                    {
                        byte[] Send_data = new byte[5];
                        Send_data[0] = (byte)0x05;
                        Send_data[1] = (byte)Convert.ToByte((byte)CH_Num - 1);  //CH
                        Send_data[2] = (byte)Convert.ToByte(Bright_Value >> 8);
                        Send_data[3] = (byte)Convert.ToByte(Bright_Value & 0x00ff);
                        Send_data[4] = (byte)0x03;
                        Serial.Write(Send_data, 0, 5);
                    }
                }
            }
            catch
            {
                return 1;
            }
            return 0;
        }
    }
}
