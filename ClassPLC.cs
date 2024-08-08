using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToptecMCP;
using System.Threading;

namespace Align
{
    //PLC 구성 부분
    public class ClassPLC
    {
        string IP = "100.100.100.20";
        int PORT = 7002;
        int NETWORK = 2;
        int PLC_STATION = 1;
        int PC_STATION = 4;

        //int Send_Address = 60500;//PC
        //int Receive_Address = 60000;//PLC
        int Send_Address = 61500;
        //int Send_Address = 62500;//PC
        int Receive_Address = 61000;//PLC A  열
        //int Receive_Address = 62000;//PLC B 열
        int RW_Point = 500;//ReadWord

        Main_Frame Frame;
        public ClassMCP MCP = new ClassMCP();
        Thread Enet;

        public int[] Send_Data = new int[500];
        public int[] Temp_Data = new int[500];
        public int[] Receive_Data = new int[500];
        public int[] Write_Command = new int[500];

        bool Program_run = true;

        public bool Write_Bit = false;

        private void Enet_Thead()
        {
            while(Program_run)
            {
                if (MCP.IsConnect && !Frame.Setup_Display.PLC_Simul)
                {
                    Temp_Data = MCP.ReadDeviceBlock(Receive_Address, RW_Point);      //PLC Data 영역 읽기
                    if (!Temp_Data.Length.Equals(0))
                        Receive_Data = Temp_Data;
                    Temp_Data = MCP.ReadDeviceBlock(Send_Address, RW_Point);            //PC Data 영역 읽기
                    if (!Temp_Data.Length.Equals(0))
                        Send_Data = Temp_Data;
                    try { Recipe_Cheek(Receive_Data[5]); }      //가끔 레시피가 0으로 들어온다???
                    catch { }
                    if (Write_Bit)       //데이터 쓰기요청 있을시
                    {
                        Write_Bit = !Write_Bit;
                        if (Frame.Setup_Display.Loop_Test.Checked)
                            Write_Command[0] = Write_Command[0] | 0x0200; //9 512
                        else
                            Write_Command[0] = Write_Command[0] & (0xffff - 0x0200); //9
                        if (Frame.Setup_Display.Unit1_Loop.Checked)
                            Write_Command[0] = Write_Command[0] | 0x0800;
                        else
                            Write_Command[0] = Write_Command[0] & (0xffff - 0x0800);// B
                        if (Frame.Setup_Display.Unit2_Loop.Checked)
                            Write_Command[0] = Write_Command[0] | 0x1000;
                        else
                            Write_Command[0] = Write_Command[0] & (0xffff - 0x1000); //C
                        if (Frame.Setup_Display.Unit3_Loop.Checked)
                            Write_Command[0] = Write_Command[0] | 0x2000;
                        else
                            Write_Command[0] = Write_Command[0] & (0xffff - 0x2000); //D
                        if (Frame.Setup_Display.Unit4_Loop.Checked)
                            Write_Command[0] = Write_Command[0] | 0x4000;
                        else
                            Write_Command[0] = Write_Command[0] & (0xffff - 0x4000); //F
                        if (Frame.Setup_Display.Unit5_Loop.Checked)
                            Write_Command[0] = Write_Command[0] | 0x0400;
                        else
                            Write_Command[0] = Write_Command[0] & (0xffff - 0x0400); //A
                        Write_Data();
                    }
                }
                //else if (!MCP.IsConnect)
                //    MCP_OPEN();
                //Thread.Sleep(5);
            }
        }

        private void Recipe_Cheek(int _PLC_Recipe)
        {
            ClassEnum.Auto_Mode _Auto_Mode = Frame.Auto_Mode;
            if (_Auto_Mode.Equals(ClassEnum.Auto_Mode.AUTO)) //레시피가 다르고 AUTO일때
            {
                try
                {
                    if (_PLC_Recipe != Convert.ToInt32(Main_Frame.Recipe.Substring(0, 2)))  //"" 들어오면 Catch 잡힘
                    {
                        Frame.Setup_Display.Recipe_Change(_PLC_Recipe.ToString("00")); //존재하지 않는 레시피일시 무시해야함
                    }
                }
                catch
                {
                    Frame.Setup_Display.Recipe_Change("00"); //존재하지 않는 레시피일시 무시해야함
                }
            }
        }

        private bool Write_Data()
        {
            int i = MCP.WriteDeviceBlock(Send_Address, Write_Command);

            if(i.Equals(0))
                return false;
            return true;
        }

        public int OPEN(Main_Frame _Frame)
        {
            Frame = _Frame;
            IP = ClassINI.ReadProject("PLC", "IP Address");
            PORT = Convert.ToInt32(ClassINI.ReadProject("PLC", "Port"));
            NETWORK = Convert.ToInt32(ClassINI.ReadProject("PLC", "Network"));
            PLC_STATION = Convert.ToInt32(ClassINI.ReadProject("PLC", "PLC Station"));
            PC_STATION = Convert.ToInt32(ClassINI.ReadProject("PLC", "PC Station"));
            Send_Address = Convert.ToInt32(ClassINI.ReadProject("PLC", "PC Data Address"));
            Receive_Address = Convert.ToInt32(ClassINI.ReadProject("PLC", "PLC Data Address"));
            RW_Point = Convert.ToInt32(ClassINI.ReadProject("PLC", "Read Point"));

            if (!MCP_OPEN().Equals(0))
                return 1;
            Enet = new Thread(Enet_Thead);
            Enet.Start();
            return 0;
        }

        public int MCP_OPEN()
        {
            MCP.PLC_Setting(IP, PORT, NETWORK, PLC_STATION, PC_STATION);
            MCP.Connect();
            int tickCount = Environment.TickCount;
            while (!MCP.IsConnect)
            {
                if (Environment.TickCount - tickCount > 3000)
                    return 1;
            }
            return 0;
        }

        public void CLOSE()
        {
            Program_run = false;
            Enet.Join();
            MCP.DisConnect();
        }
    }
}
