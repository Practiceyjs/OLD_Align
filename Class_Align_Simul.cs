using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Align
{
    public partial class Class_Align_Simul
    {
        const int PLC_READY = 0x0001;
        const int PLC_ERROR = 0x0002;
        const int PLC_RESET = 0x0004;
        const int PLC_1St_Ack = 0x0008;
        const int PLC_Cal_Start = 0x0010;
        const int PLC_Cal_End = 0x0020;
        const int PLC_Align_Start = 0x0040;
        const int PLC_Align_End = 0x0080;
        const int PLC_ReAlign = 0x0100;
        const int PLC_OCA_WINDOW = 0x0200;

        const int PLC_2nd_Ack = 0x0800;
        const int PLC_Move_Ack = 0x1000;
        const int PLC_Move_Done_Req = 0x2000;

        const int PC_OFF = 0x0000;
        const int PC_AUTO = 0x0002;

        const int PC_READY = 0x0001;
        const int PC_ERROR = 0x0002;
        const int PC_RESET = 0x0004;
        const int PC_1St_Req = 0x0008;
        const int PC_Cal_Start = 0x0010;
        const int PC_Cal_End = 0x0020;
        const int PC_Align_Start = 0x0040;
        const int PC_Align_End = 0x0080;
        const int PC_Align_OK = 0x0100;
        const int PC_Align_NG = 0x0200;

        const int PC_2nd_Req = 0x0800;
        const int PC_Move_Req = 0x1000;
        const int PC_Move_Done_Ack = 0x2000;

        Simul_Align_Sequence Sequence = Simul_Align_Sequence.Ready;
        double Delay = 0.5;

        public void Simul_Align(int Unit,ref int Receive_Data, int Send_Data)
        {
            switch (Sequence)
            {
                case Simul_Align_Sequence.Ready:
                    if ((Send_Data & PC_READY).Equals(PC_READY))
                    {
                        int tick = Environment.TickCount;
                        while (true)
                        {
                            if (Environment.TickCount - tick > (1.5 * 1000))
                                break;
                        }
                        Receive_Data = Receive_Data | PLC_Align_Start;
                        Sequence = Simul_Align_Sequence.Align_Start_Req;
                    }
                    break;
                case Simul_Align_Sequence.Align_Start_Req:
                    if ((Send_Data & PC_Align_Start).Equals(PC_Align_Start))
                    {
                        Sequence = Simul_Align_Sequence.Align_Start_Ack;
                    }
                    break;
                case Simul_Align_Sequence.Align_Start_Ack:
                    if ((Send_Data & PC_2nd_Req).Equals(PC_2nd_Req))
                    {
                        Sequence = Simul_Align_Sequence.Move_2nd_Req;
                    }
                    break;
                case Simul_Align_Sequence.Move_2nd_Req:
                    int tickCount = Environment.TickCount;
                    while (true)
                    {
                        if (Environment.TickCount - tickCount > (Delay * 1000))
                            break;
                    }
                    Receive_Data = Receive_Data | PLC_2nd_Ack;
                    Sequence = Simul_Align_Sequence.Move_2nd_Ack;
                    break;
                case Simul_Align_Sequence.Move_2nd_Ack:
                    if ((Send_Data & PC_2nd_Req).Equals(0))
                    {
                        Receive_Data = Receive_Data & (0xffff - PLC_2nd_Ack);
                        Sequence = Simul_Align_Sequence.Align_End_Req;
                    }
                    break;
                case Simul_Align_Sequence.Align_End_Req:
                    if ((Send_Data & PC_Align_End).Equals(PC_Align_End))
                    {
                        if ((Send_Data & PC_Align_OK).Equals(PC_Align_OK))
                        {
                            Receive_Data = Receive_Data | PLC_Align_End;
                            Receive_Data = Receive_Data & (0xffff - PLC_Align_Start);
                            Sequence = Simul_Align_Sequence.Align_End_Ack;
                        }
                        else if ((Send_Data & PC_Align_NG).Equals(PC_Align_NG))
                        {
                            Sequence = Simul_Align_Sequence.Manual_Mark;
                        }
                    }
                    break;
                case Simul_Align_Sequence.Manual_Mark:
                    if ((Send_Data & PC_Align_OK).Equals(PC_Align_OK))
                    {
                        Receive_Data = Receive_Data | PLC_Align_End;
                        Receive_Data = Receive_Data & (0xffff - PLC_Align_Start);
                        Sequence = Simul_Align_Sequence.Align_End_Ack;
                    }
                    break;
                case Simul_Align_Sequence.Align_End_Ack:
                    if ((Send_Data & PC_Align_End).Equals(0))
                    {
                        Receive_Data = Receive_Data & (0xffff - PLC_Align_End);
                        Sequence = Simul_Align_Sequence.Ready;
                    }
                    break;
            }
        }

        enum Simul_Align_Sequence
        {
            Ready,
            Align_Start_Req,
            Align_Start_Ack,
            Move_2nd_Req,
            Move_2nd_Ack,
            Align_End_Req,
            Align_End_Ack,
            Manual_Mark,
        }
    }
}
