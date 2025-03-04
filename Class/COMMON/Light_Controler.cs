using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace T_Align
{
    public class Light_Controler
    {
        SerialPort serial;
        Enum.Serial Comport = Enum.Serial.NON;

        public int[] Light_Value = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public int[] Auto_Value = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public DateTime[] Life_Light_Time = new DateTime[17];
        public DateTime[] Start_Light_Time = new DateTime[17];

        object Lock_Object = new object();

        public bool COMPORT_OPEN(Enum.Serial _Comport)
        {
            Comport = _Comport;
            serial = new SerialPort(Comport.ToString(), 19200, Parity.None, 8, StopBits.One);
            serial.ReadTimeout = 100;
            serial.WriteTimeout = 100;
            serial.Open();
            if (!serial.IsOpen)
                return true;
            for(int LoopCount=1;LoopCount<17;LoopCount++)
            {
                try
                {
                    Life_Light_Time[LoopCount] = Life_Light_Time[LoopCount].AddSeconds(double.Parse(ClassINI.Read_Light_LifeLine(Comport.ToString(), "CH" + LoopCount.ToString())));
                }
                catch { }
            }
            return false;
        }

        public string Light_On(bool[] _Select)
        {
            string Messege = Comport.ToString();
            for (int LoopCount = 0; LoopCount < _Select.Length; LoopCount++)
                if (_Select[LoopCount])
                {
                    Light_Value[LoopCount] = Auto_Value[LoopCount];
                    Start_Light_Time[LoopCount] = DateTime.Now;
                    Messege += " CH" + LoopCount.ToString() + " : " + Light_Value[LoopCount].ToString();
                }
            if (Auto_Light())
                return Comport.ToString() + " (Light Controler Error)";
            else
                return Messege;
        }

        public string Light_Off(bool[] _Select)
        {
            string Messege = Comport.ToString();
            for (int LoopCount = 0; LoopCount < _Select.Length; LoopCount++)
                if (_Select[LoopCount])
                {
                    Light_Value[LoopCount] = 0;
                    LifeLine_Save(LoopCount);
                    Messege += " CH" + LoopCount.ToString() + " : " + Light_Value[LoopCount].ToString();
                }
            if (Auto_Light())
                return Comport.ToString() + " (Light Controler Error)";
            else
                return Messege;
        }

        public void All_Off()
        {
            for (int LoopCount = 0; LoopCount < 17; LoopCount++)
            {
                if (Light_Value[LoopCount] != 0)
                {
                    Light_Value[LoopCount] = 0;
                    LifeLine_Save(LoopCount);
                }
            }
            Auto_Light();
        }

        public void All_On()
        {
            for (int LoopCount = 0; LoopCount < 17; LoopCount++)
            {
                if (Light_Value[LoopCount] == 0)
                    Light_Value[LoopCount] = Auto_Value[LoopCount];
            }
            Auto_Light();
        }

        public void Value_Load()
        {
            for (int LoopCount = 1; LoopCount <= Auto_Value.Length; LoopCount++)
            {
                try
                {
                    Auto_Value[LoopCount] = int.Parse(ClassINI.Read_Light_Data(Comport.ToString(), "CH" + (LoopCount).ToString()));
                }
                catch { Auto_Value[LoopCount] = 0; }
            }
        }

        public void Value_Save(int _CH, int _Value)
        {
            Auto_Value[_CH] = _Value;
            ClassINI.Write_Light_Data(Comport.ToString(), "CH" + _CH.ToString(), Auto_Value[_CH].ToString());
        }

        public bool Manual_Light(int _CH, int _Value)
        {
            Light_Value[_CH] = _Value;
            lock (Lock_Object)
            {
                try
                {
                    if (!serial.IsOpen)
                        return true;
                    if (_Value > 255)
                        return true;
                    byte[] Data = new byte[8];
                    Data[0] = (byte)'N';
                    Data[1] = (byte)((_CH / 10) + 0x30);
                    Data[2] = (byte)((_CH % 10) + 0x30);
                    Data[3] = (byte)((Light_Value[_CH] / 100) + 0x30);
                    Data[4] = (byte)(((Light_Value[_CH] % 100) / 10) + 0x30);
                    Data[5] = (byte)((Light_Value[_CH] % 10) + 0x30);
                    Data[6] = (byte)0x0D;
                    Data[7] = (byte)0x0A;
                    serial.Write(Data, 0, Data.Length);
                    byte[] SendData = new byte[1];
                    serial.Read(SendData, 0, 1);
                    if (SendData[0] == 0x06)
                    {
                        if(Light_Value[_CH] != 0)
                        {
                            if(Start_Light_Time[_CH] == default(DateTime))
                                Start_Light_Time[_CH] = DateTime.Now;
                        }
                        else
                        {
                            LifeLine_Save(_CH);
                        }
                        return false;
                    }
                    else
                        return true;
                }
                catch { return true; }
            }
        }

        //private void Auto_Light()
        //{
        //    try
        //    {
        //        if (serial.IsOpen)
        //        {
        //            byte[] Data = new byte[ETC_Option.Light_Controler_Max_CH[(int)Comport] * 5 + 5];
        //            Data[0] = (byte)'P';
        //            Data[1] = (byte)'1';
        //            Data[2] = (byte)'6';
        //            for (int LoopCount = 0; LoopCount < ETC_Option.Light_Controler_Max_CH[(int)Comport]; LoopCount++)
        //            {
        //                Data[LoopCount * 5 + 3] = (byte)(((LoopCount + 1) / 10) + 0x30);
        //                Data[LoopCount * 5 + 4] = (byte)(((LoopCount + 1) % 10) + 0x30);
        //                Data[LoopCount * 5 + 5] = (byte)((30 / 100) + 0x30);
        //                Data[LoopCount * 5 + 6] = (byte)(((30 % 100) / 10) + 0x30);
        //                Data[LoopCount * 5 + 7] = (byte)((30 % 10) + 0x30);
        //            }
        //            //for (int LoopCount = 0; LoopCount < ETC_Option.Light_Controler_Max_CH[(int)Comport]; LoopCount++)
        //            //{
        //            //    Data[LoopCount * 5 + 3] = (byte)(((LoopCount + 1) / 10) + 0x30);
        //            //    Data[LoopCount * 5 + 4] = (byte)(((LoopCount + 1) % 10) + 0x30);
        //            //    Data[LoopCount * 5 + 5] = (byte)((Light_Value[LoopCount + 1] / 100) + 0x30);
        //            //    Data[LoopCount * 5 + 6] = (byte)(((Light_Value[LoopCount + 1] % 100) / 10) + 0x30);
        //            //    Data[LoopCount * 5 + 7] = (byte)((Light_Value[LoopCount + 1] % 10) + 0x30);
        //            //}
        //            Data[5 * ETC_Option.Light_Controler_Max_CH[(int)Comport] + 3] = (byte)0x0D;
        //            Data[5 * ETC_Option.Light_Controler_Max_CH[(int)Comport] + 4] = (byte)0x0A;
        //            serial.Write(Data, 0, Data.Length);
        //            byte[] SendData = new byte[1];
        //            serial.Read(SendData, 0, 1);
        //            if (SendData[0] == 0x06)
        //                return;
        //        }
        //    }
        //    catch { }
        //}

        /// //////////////////////////////////////////////////////////////////////////////////// 임시 조명컨트롤러 업데이트 전8,8로 16채널 제어 컨트롤러 펌웨어 12채널까지 한번에 가능

        private bool Auto_Light()
        {
            try
            {
                if (serial.IsOpen)
                {
                    byte[] Data = new byte[45];
                    Data[0] = (byte)'P';
                    Data[1] = (byte)'0';
                    Data[2] = (byte)'8';
                    for (int LoopCount = 0; LoopCount < 8; LoopCount++)
                    {
                        Data[LoopCount * 5 + 3] = (byte)(((LoopCount + 1) / 10) + 0x30);
                        Data[LoopCount * 5 + 4] = (byte)(((LoopCount + 1) % 10) + 0x30);
                        Data[LoopCount * 5 + 5] = (byte)((Light_Value[LoopCount + 1] / 100) + 0x30);
                        Data[LoopCount * 5 + 6] = (byte)(((Light_Value[LoopCount + 1] % 100) / 10) + 0x30);
                        Data[LoopCount * 5 + 7] = (byte)((Light_Value[LoopCount + 1] % 10) + 0x30);
                        if (Light_Value[LoopCount + 1] != 0)
                        {
                            if (Start_Light_Time[LoopCount + 1] == default(DateTime))
                                Start_Light_Time[LoopCount + 1] = DateTime.Now;
                        }
                    }
                    //for (int LoopCount = 0; LoopCount < ETC_Option.Light_Controler_Max_CH[(int)Comport]; LoopCount++)
                    //{
                    //    Data[LoopCount * 5 + 3] = (byte)(((LoopCount + 1) / 10) + 0x30);
                    //    Data[LoopCount * 5 + 4] = (byte)(((LoopCount + 1) % 10) + 0x30);
                    //    Data[LoopCount * 5 + 5] = (byte)((Light_Value[LoopCount + 1] / 100) + 0x30);
                    //    Data[LoopCount * 5 + 6] = (byte)(((Light_Value[LoopCount + 1] % 100) / 10) + 0x30);
                    //    Data[LoopCount * 5 + 7] = (byte)((Light_Value[LoopCount + 1] % 10) + 0x30);
                    //}
                    Data[40 + 3] = (byte)0x0D;
                    Data[40 + 4] = (byte)0x0A;
                    serial.Write(Data, 0, Data.Length);
                    byte[] SendData = new byte[1];
                    serial.Read(SendData, 0, 1);
                    if (SendData[0] == 0x06)
                    {
                        if (Auto_Light2())
                            return true;
                        return false;
                    }
                    return true;
                }
            }
            catch { return true; }
            return false;
        }
        private bool Auto_Light2()
        {
            try
            {
                if (serial.IsOpen)
                {
                    byte[] Data = new byte[45];
                    Data[0] = (byte)'P';
                    Data[1] = (byte)'0';
                    Data[2] = (byte)'8';
                    for (int LoopCount = 8; LoopCount < 16; LoopCount++)
                    {
                        Data[LoopCount * 5 + 3 - 40] = (byte)(((LoopCount + 1) / 10) + 0x30);
                        Data[LoopCount * 5 + 4 - 40] = (byte)(((LoopCount + 1) % 10) + 0x30);
                        Data[LoopCount * 5 + 5 - 40] = (byte)((Light_Value[LoopCount + 1] / 100) + 0x30);
                        Data[LoopCount * 5 + 6 - 40] = (byte)(((Light_Value[LoopCount + 1] % 100) / 10) + 0x30);
                        Data[LoopCount * 5 + 7 - 40] = (byte)((Light_Value[LoopCount + 1] % 10) + 0x30);
                        if (Light_Value[LoopCount + 1] != 0)
                        {
                            if (Start_Light_Time[LoopCount + 1] == default(DateTime))
                                Start_Light_Time[LoopCount + 1] = DateTime.Now;
                        }
                    }
                    //for (int LoopCount = 0; LoopCount < ETC_Option.Light_Controler_Max_CH[(int)Comport]; LoopCount++)
                    //{
                    //    Data[LoopCount * 5 + 3] = (byte)(((LoopCount + 1) / 10) + 0x30);
                    //    Data[LoopCount * 5 + 4] = (byte)(((LoopCount + 1) % 10) + 0x30);
                    //    Data[LoopCount * 5 + 5] = (byte)((Light_Value[LoopCount + 1] / 100) + 0x30);
                    //    Data[LoopCount * 5 + 6] = (byte)(((Light_Value[LoopCount + 1] % 100) / 10) + 0x30);
                    //    Data[LoopCount * 5 + 7] = (byte)((Light_Value[LoopCount + 1] % 10) + 0x30);
                    //}
                    Data[40 + 3] = (byte)0x0D;
                    Data[40 + 4] = (byte)0x0A;
                    serial.Write(Data, 0, Data.Length);
                    byte[] SendData = new byte[1];
                    serial.Read(SendData, 0, 1);
                    if (SendData[0] == 0x06)
                        return false;
                    return true;
                }
            }
            catch { return true; }
            return false;
        }
        /// ////////////////////////////////////////////////////////////////////////////////////


        private void LifeLine_Save(int CH)
        {
            try
            {
                Life_Light_Time[CH] = Life_Light_Time[CH].AddSeconds((DateTime.Now - Start_Light_Time[CH]).TotalSeconds);
                Start_Light_Time[CH] = default(DateTime);
                ClassINI.Write_Light_LifeLine(Comport.ToString(), "CH" + CH.ToString(), (Life_Light_Time[CH] - default(DateTime)).TotalSeconds.ToString());
            }
            catch { }
        }
    }
}
