using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToptecMCP;
using System.Threading;

namespace T_Align
{
    public class ClassPLC
    {
        public ClassMCP MCP = new ClassMCP();
        LC_MelsecNet LC_Net = new LC_MelsecNet();
        Thread Enet;
        Thread LC;

        public bool Program_run = true;

        public void New_MCP_Cnt()
        {
            Program_run = false;
            MCP = null;
            MCP = new ClassMCP();
        }

        private void Enet_Thead()
        {
            while (true)
            {
                if (!COMMON.Program_Run)
                    break;
                while (Program_run)
                {
                    try
                    {
                        if (MCP.IsConnect)
                        {
                            try
                            {
                                int[] Temp_Data = new int[500];
                                Temp_Data = MCP.ReadDeviceBlock(ClassInterface.Recive_Address, 500);      //PLC Data 영역 읽기
                                if (Temp_Data.Length.Equals(500))
                                    Array.Copy(Temp_Data, COMMON.Receive_Data, Temp_Data.Length);
                            }
                            catch { }
                            try { Recipe_Cheek(COMMON.Receive_Data[9]); }
                            catch { }
                            if (COMMON.Write_Bit)       //데이터 쓰기요청 있을시
                            {
                                COMMON.Write_Bit = !COMMON.Write_Bit;
                                Write_Data();
                            }
                        }
                    }
                    catch { }
                    Thread.Sleep(50);
                }
            }
        }

        private void LC_Thead()
        {
            while (true)
            {
                if (!COMMON.Program_Run)
                    break;
                while (Program_run)
                {
                    try
                    {
                        if (true)
                        {
                            ushort[] Temp_Data = new ushort[500];
                            LC_Net.Melsec_Recive_R(out Temp_Data);      //PLC Data 영역 읽기
                            if (!Temp_Data.Length.Equals(0))
                                COMMON.Receive_Data = Array.ConvertAll(Temp_Data, new Converter<ushort, int>(ConvertToInt));
                            //Array.Copy(Temp_Data, COMMON.Receive_Data, Temp_Data.Length);
                        }
                    }
                    catch { }
                    try { Recipe_Cheek(COMMON.Receive_Data[9]); }
                    catch { }
                    if (COMMON.Write_Bit)       //데이터 쓰기요청 있을시
                    {
                        COMMON.Write_Bit = !COMMON.Write_Bit;
                        Write_Data_LC();
                    }
                    Thread.Sleep(50);
                }
            }
        }

        private void Recipe_Cheek(int _PLC_Recipe)
        {
            //if (Common.Auto_Mode.Equals(ClassEnum.Auto_Mode.AUTO)) //레시피가 다르고 AUTO일때
            //{
            //    try
            //    {
            //        if (_PLC_Recipe != Convert.ToInt32(Main_Frame.Recipe.Substring(0, 2)))  //"" 들어오면 Catch 잡힘
            //        {
            //            Frame.Setup_Display.Recipe_Change(_PLC_Recipe.ToString("00")); //존재하지 않는 레시피일시 무시해야함
            //        }
            //    }
            //    catch
            //    {
            //        Frame.Setup_Display.Recipe_Change("00"); //존재하지 않는 레시피일시 무시해야함
            //    }
            //}
        }

        private bool Write_Data()
        {
            int[] Send_Array = new int[COMMON.Send_Data.Length];
            Array.Copy(COMMON.Send_Data, Send_Array, COMMON.Send_Data.Length);
            int i = MCP.WriteDeviceBlock(ClassInterface.Send_Address, Send_Array);

            if (i.Equals(0))
                return false;
            return true;
        }

        private bool Write_Data_LC()
        {
            ushort[] Send_Array = Array.ConvertAll(COMMON.Send_Data, new Converter<int, ushort>(ConvertToUshort));
            //Array.Copy(COMMON.Send_Data, Send_Array, COMMON.Send_Data.Length);
            int Error = LC_Net.Melsec_Send_R(Send_Array);
            if (Error != 0)
                return false;
            return true;
        }

        private ushort ConvertToUshort(int Data)
        {
            return (ushort)Data;
        }

        private int ConvertToInt(ushort Data)
        {
            return (ushort)Data;
        }

        public int OPEN()
        {
            if (ETC_Option.Enet)
            {
                if (Enet == null)
                {
                    Enet = new Thread(Enet_Thead);
                    Enet.Start();
                }
                if (!MCP_OPEN().Equals(0))
                    return 1;
            }
            else
            {
                LC_Net.Melsec_Set(152, 0, 255, ClassInterface.Send_Address, ClassInterface.Recive_Address);
                int Error = LC_Net.Melsec_Open();
                if (Error != 0)
                    return Error;
                LC = new Thread(LC_Thead);
                LC.Start();
            }
            Program_run = true;
            return 0;
        }

        public int MCP_OPEN()
        {
            MCP.PLC_Setting(COMMON.IP, COMMON.PORT, COMMON.NETWORK, COMMON.PLC_STATION, COMMON.PC_STATION);
            MCP.IsConnect = false;
            MCP.Connect();
            int tickCount = Environment.TickCount;
            try
            {
                while (!MCP.IsConnect)
                {
                    if (Environment.TickCount - tickCount > 3000)
                        return 1;
                }
            }
            catch { return 1; }
            return 0;
        }

        public void CLOSE()
        {
            Program_run = false;
            try
            {
                Enet.Abort();
                Enet.Join();
            }
            catch { }
            try
            {
                LC.Abort();
                LC.Join();
            }
            catch { }
            MCP.DisConnect();
        }

        public void Thread_Stop()
        {
            try
            {
                Enet.Abort();
                Enet.Join();
            }
            catch { }
            try
            {
                LC.Abort();
                LC.Join();
            }
            catch { }
        }
    }
}
