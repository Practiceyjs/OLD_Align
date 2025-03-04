using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace T_Align
{
    public class Sequence
    {
        private enum PLC_COMMAND
        {
            READY,
            ERROR,
            RESET,
            N1St_Ack,
            Cal_Start,
            Cal_End,
            Align_Start,
            AlignEnd,
            ReAlign,
            Spare,
            Target_Object,
            N2nd_Ack,
            Move_Ack,
            Move_Done_Req,
            Inspection_Start,
            Inspection_End
        }

        private enum PC_COMMAND
        {
            READY,
            ERROR,
            RESET,
            N1St_Req,
            Cal_Start,
            Cal_End,
            Align_Start,
            AlignEnd,
            OK,
            NG,
            Target_Object,
            N2nd_Req,
            Move_Req,
            Move_Done_Ack,
            Inspection_OK,
            Inspection_NG
        }

        Enum.Type Current_Type = Enum.Type.Common;
        Enum.Unit Current_Unit = Enum.Unit.Unit1;

        Enum.Position Current_Position = Enum.Position.COMMON;

        public Enum.Sequence Sequences = Enum.Sequence.Ready;
        public Enum.Align_Sequence Align_Sequence = Enum.Align_Sequence.Ready;
        public Enum.Calibration_Sequence Calibration_Sequence = Enum.Calibration_Sequence.Ready;
        public Enum.Inspection_Sequence Inspection_Sequence = Enum.Inspection_Sequence.Ready;

        ClassUnit Unit;
        Unit_Log Unit_Log = new Unit_Log();

        double Calculation_Theta_Pitch = 0;
        double Calibration_Length = 0;
        int Calibration_Count = 0;
        static double Pitch_X = 1;
        static double Pitch_Y = 1;
        static double Pitch_T = 0.3;
        double[] Calibration_X = { -Pitch_X, 0, Pitch_X, -Pitch_X, 0, Pitch_X, -Pitch_X, 0, Pitch_X, 0, 0, 0, 0, 0, 0, 0 };
        double[] Calibration_Y = { -Pitch_Y, -Pitch_Y, -Pitch_Y, 0, 0, 0, Pitch_Y, Pitch_Y, Pitch_Y, 0, 0, 0, 0, 0, 0, 0 };
        double[] Calibration_T = { 0, 0, 0, 0, 0, 0, 0, 0, 0, -Pitch_T * 3, -Pitch_T * 2, -Pitch_T, 0, Pitch_T, Pitch_T * 2, Pitch_T * 3 };
        string[] CTQ_COMMAND;
        double CTQ_X = 0;
        double CTQ_Y = 0;
        double CTQ_T = 0;
        PointF[,] CTQ_DATA;

        double Align_Position_X = 0;
        double Align_Position_Y = 0;
        double Align_Position_T = 0;

        double Offset_Position_X = 0;
        double Offset_Position_Y = 0;
        double Offset_Position_T = 0;
        double P_Offset_Position_X = 0;
        double P_Offset_Position_Y = 0;
        double P_Offset_Position_T = 0;
        double L_Offset_Position_X = 0;
        double L_Offset_Position_Y = 0;
        double L_Offset_Position_T = 0;

        double[] Calibration_Orizin = { 0, 0, 0, 0 };

        string Cell_ID = "";

        double Keep_Data_X = 0;
        double Keep_Data_Y = 0;
        double Keep_Data_T = 0;

        DateTime Start;
        string Tact = "";

        double Re_Align_Spec = 0.01;
        int Re_Align_Count = 0;

        public void Sequence_Set(Enum.Unit _Unit_Num, ClassUnit _Unit)
        {
            Unit = _Unit;
            Current_Unit = _Unit_Num;
        }

        public void Unit_Sequence()
        {
            if (COMMON.Current_Mode.Equals(Enum.Mode.Auto) || COMMON.Current_Mode.Equals(Enum.Mode.Simul))
            {
                if (ClassInterface.Bit_Check((int)Current_Unit, (int)PLC_COMMAND.RESET))                          // 해당 유닛 리셋
                {
                    Unit_Reset();
                }
                if (Sequences.Equals(Enum.Sequence.Align))                         // 얼라인 시퀀스
                {
                    if (AlignSequence() == Enum.Align_Sequence.Ready)
                    {
                        Sequences = Enum.Sequence.Ready;
                        Unit.Thread_Stop();
                    }
                }
                else if (Sequences.Equals(Enum.Sequence.Inspection))                         // 얼라인 시퀀스
                {
                    if (InspectionSequence() == Enum.Inspection_Sequence.Ready)
                    {
                        Sequences = Enum.Sequence.Ready;
                        Unit.Thread_Stop();
                    }
                }
                else if (Sequences.Equals(Enum.Sequence.Calibration))                         // 얼라인 시퀀스
                {
                    if (CalibrationSequence() == Enum.Calibration_Sequence.Ready)
                    {
                        Sequences = Enum.Sequence.Ready;
                        Unit.Thread_Stop();
                    }
                }
            }
        }

        public void Unit_Reset()
        {
            while (ClassInterface.Bit_Check((int)Current_Unit, (int)PLC_COMMAND.RESET))
            { }
            Sequences = Enum.Sequence.Ready;
            Align_Sequence = Enum.Align_Sequence.Ready;
            Calibration_Sequence = Enum.Calibration_Sequence.Ready;
            Unit.Type[(int)Enum.Type.Target].Title_Label.BackColor = Color.LightGreen;
            Unit.Type[(int)Enum.Type.Object].Title_Label.BackColor = Color.LightGreen;
            ClassInterface.Word_Write((int)Current_Unit,1);
            ClassInterface.Word_Write((int)Current_Unit + 259, (int)0);
            for (int LoopCount = 0; LoopCount < 30; LoopCount++)
            {
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 10 + ETC_Option.Interface_Offset + LoopCount] = 0;
            }
            Unit.Thread_Stop();
        }

        private Enum.Inspection_Sequence InspectionSequence()
        {
            switch (Inspection_Sequence)
            {
                case Enum.Inspection_Sequence.Inspecion_Start_Req:
                    Start = DateTime.Now;
                    if (ETC_Option.Second_Grab_First_Use[(int)Current_Unit])
                        Current_Position = Enum.Position.Second;
                    else
                        Current_Position = Enum.Position.First;
                    Current_Type = Enum.Type.Target;
                    Inspection_Sequence = Enum.Inspection_Sequence.Inspecion_Start;
                    Unit_Log.Sequence_Write(Current_Unit, Inspection_Sequence.ToString());
                    break;
                case Enum.Inspection_Sequence.Inspecion_Start:
                    COMMON.Inspection_Tiltle.BackColor = Color.SkyBlue;
                    Mark_Index_Reset(Enum.Type.Target);
                    Mark_Index_Reset(Enum.Type.Object);

                    Unit_Light(true, Current_Type);
                    Inspection_Grab(Current_Position);
                    break;
                case Enum.Inspection_Sequence.Move_2nd_Req:
                    Unit_Log.Sequence_Write(Current_Unit, Inspection_Sequence.ToString());
                    ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.N2nd_Req, true);
                    Inspection_Sequence = Enum.Inspection_Sequence.Move_2nd_Ack;
                    break;
                case Enum.Inspection_Sequence.Move_2nd_Ack:
                    if (ClassInterface.Bit_Check((int)Current_Unit, (int)PLC_COMMAND.N2nd_Ack))
                    {
                        Unit_Log.Sequence_Write(Current_Unit, Inspection_Sequence.ToString());
                        ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.N2nd_Req, false);
                        if (ETC_Option.Second_Grab_First_Use[(int)Current_Unit])
                            Current_Position = Enum.Position.First;
                        else
                            Current_Position = Enum.Position.Second;
                        Inspection_Grab(Current_Position);
                    }
                    break;
                case Enum.Inspection_Sequence.Inspection:
                    Unit_Log.Sequence_Write(Current_Unit, Inspection_Sequence.ToString());
                    int Error = Unit.Inspection_Start();
                    if (Error == 0)
                    {

                    }
                    else
                    {
                        Inspection_Error(Error);
                    }
                    Unit_Log.Tact_Write(Current_Unit, "Inspection Calculat : " + ((DateTime.Now - Start).TotalMilliseconds / 1000).ToString("0.000"));
                    if (Inspection_Sequence == Enum.Inspection_Sequence.Inspection)
                        Inspection_Sequence = Enum.Inspection_Sequence.Inspection_End_Req;
                    break;
                case Enum.Inspection_Sequence.Inspection_End_Req:
                    Unit_Log.Sequence_Write(Current_Unit, Inspection_Sequence.ToString());
                    ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.Inspection_NG, false);
                    ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.Inspection_OK, true);
                    COMMON.Inspection_Tiltle.BackColor = Color.LightGreen;
                    Tact = ((DateTime.Now - Start).TotalMilliseconds / 1000).ToString("0.000");
                    Unit_Log.Tact_Write(Current_Unit, "Total Tact : " + Tact);
                    Inspection_Sequence = Enum.Inspection_Sequence.Inspection_End_Ack;
                    break;
                case Enum.Inspection_Sequence.Inspection_End_Ack:
                    if (ClassInterface.Bit_Check((int)Current_Unit, (int)PLC_COMMAND.Inspection_End))
                    {
                        Unit_Log.Sequence_Write(Current_Unit, Inspection_Sequence.ToString());
                        ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.Inspection_OK, false);
                        Inspection_Sequence = Enum.Inspection_Sequence.Inspection_End;
                    }
                    break;
                case Enum.Inspection_Sequence.Inspection_End:
                    if (!ClassInterface.Bit_Check((int)Current_Unit, (int)PLC_COMMAND.AlignEnd))
                    {
                        Unit_Log.Sequence_Write(Current_Unit, Inspection_Sequence.ToString());
                        //Main화면 Log
                        Inspection_Data_Save();
                        Inspection_Sequence = Enum.Inspection_Sequence.Ready;
                        Unit_Log.Sequence_Write(Current_Unit, Inspection_Sequence.ToString());
                        Unit_Log.Sequence_Write(Current_Unit, "--------------------------------------------------------");
                        Unit_Log.Tact_Write(Current_Unit, "PLC End Off : " + ((DateTime.Now - Start).TotalMilliseconds / 1000).ToString("0.000"));
                    }
                    break;
            }
            return Inspection_Sequence;
        }

        private Enum.Align_Sequence AlignSequence()
        {
            switch (Align_Sequence)
            {
                case Enum.Align_Sequence.Align_Start_Req:
                    Calibration_Orizin[0] = 0;
                    Calibration_Orizin[1] = 0;
                    Calibration_Orizin[2] = 0;
                    Calibration_Orizin[3] = 0;
                    Start = DateTime.Now;
#if CW_SAM
                    if (Current_Unit == Enum.Unit.Unit2)
                    {
                        if(COMMON.Light_Controlers[1].Light_Value[13] != 20)
                        {
                            COMMON.Light_Controlers[1].Manual_Light(13, 20);
                            COMMON.Light_Controlers[1].Manual_Light(14, 20);
                        }
                    }
                    if (Current_Unit == Enum.Unit.Unit3)
                    {
                        if (COMMON.Light_Controlers[1].Light_Value[13] != 43)
                        {
                            COMMON.Light_Controlers[1].Manual_Light(13, 43);
                            COMMON.Light_Controlers[1].Manual_Light(14, 43);
                        }
                    }
#endif
                    if (ClassInterface.Bit_Check((int)Current_Unit, (int)PLC_COMMAND.Target_Object))
                        Current_Type = Enum.Type.Object;
                    else
                        Current_Type = Enum.Type.Target;
                    if (ETC_Option.Second_Grab_First_Use[(int)Current_Unit])
                        Current_Position = Enum.Position.Second;
                    else
                        Current_Position = Enum.Position.First;
                    Re_Align_Count = 0;
                    Align_Sequence = Enum.Align_Sequence.Align_Start;
                    Unit_Log.Sequence_Write(Current_Unit, Align_Sequence.ToString());
                    break;
                case Enum.Align_Sequence.Align_Start:
                    if(ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.DG_DA_TO_4POSITION || ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.DG_DA)
                    {
                        Unit.Type[(int)Enum.Type.Target].Title_Label.BackColor = Color.SkyBlue;
                        Unit.Type[(int)Enum.Type.Object].Title_Label.BackColor = Color.SkyBlue;
                        Mark_Index_Reset(Enum.Type.Target);
                        Mark_Index_Reset(Enum.Type.Object);
                    }
                    else
                    {
                        Mark_Index_Reset(Current_Type);
                        Unit.Type[(int)Current_Type].Title_Label.BackColor = Color.SkyBlue;
                    }
                    Unit_Light(true, Current_Type);
                    ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.Align_Start, true);
                    Unit_Grab(Current_Position);
                    break;
                case Enum.Align_Sequence.Target_Object_Req_1ST:
                    Unit_Log.Sequence_Write(Current_Unit, Align_Sequence.ToString());
                    ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.Target_Object, true);
                    Align_Sequence = Enum.Align_Sequence.Target_Object_Ack_1ST;
                    break;
                case Enum.Align_Sequence.Target_Object_Ack_1ST:
                    if(ClassInterface.Bit_Check((int)Current_Unit, (int)PLC_COMMAND.Target_Object))
                    {
                        Unit_Log.Sequence_Write(Current_Unit, Align_Sequence.ToString());
                        ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.Target_Object, false);
                        Current_Type = Enum.Type.Object;
                        Unit_Grab(Current_Position);
                    }
                    break;
                case Enum.Align_Sequence.Move_2nd_Req:
                    Unit_Log.Sequence_Write(Current_Unit, Align_Sequence.ToString());
                    ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.N2nd_Req, true);
                    Align_Sequence = Enum.Align_Sequence.Move_2nd_Ack;
                    break;
                case Enum.Align_Sequence.Move_2nd_Ack:
                    if (ClassInterface.Bit_Check((int)Current_Unit, (int)PLC_COMMAND.N2nd_Ack))
                    {
                        Unit_Log.Sequence_Write(Current_Unit, Align_Sequence.ToString());
                        ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.N2nd_Req, false);
                        if (ETC_Option.Second_Grab_First_Use[(int)Current_Unit])
                            Current_Position = Enum.Position.First;
                        else
                            Current_Position = Enum.Position.Second;
                        Unit_Grab(Current_Position);
                    }
                    break;
                case Enum.Align_Sequence.Target_Object_Req_2ND:
                    Unit_Log.Sequence_Write(Current_Unit, Align_Sequence.ToString());
                    ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.Target_Object, true);
                    Align_Sequence = Enum.Align_Sequence.Target_Object_Ack_2ND;
                    break;
                case Enum.Align_Sequence.Target_Object_Ack_2ND:
                    if (ClassInterface.Bit_Check((int)Current_Unit, (int)PLC_COMMAND.Target_Object))
                    {
                        Unit_Log.Sequence_Write(Current_Unit, Align_Sequence.ToString());
                        ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.Target_Object, false);
                        Current_Type = Enum.Type.Object;
                        Unit_Grab(Current_Position);
                    }
                    break;
                case Enum.Align_Sequence.Align:
                    Unit_Log.Sequence_Write(Current_Unit, Align_Sequence.ToString());
                    Unit_Align();
                    switch (ETC_Option.Log_Type[(int)Current_Unit])
                    {
                        case Enum.Log_Type.CW_Log:
                            P_Offset_Position_X = (double)(COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 28] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 29] << 16)) / 10000;
                            P_Offset_Position_Y = (double)(COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 30] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 31] << 16)) / 10000;
                            P_Offset_Position_T = (double)(COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 32] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 33] << 16)) / 10000;
                            L_Offset_Position_X = (double)(COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 34] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 35] << 16)) / 10000;
                            L_Offset_Position_Y = (double)(COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 36] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 37] << 16)) / 10000;
                            L_Offset_Position_T = (double)(COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 38] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 39] << 16)) / 10000;
                            break;
                        default:
                            Offset_Position_X = (double)(COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 34] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 35] << 16)) / 10000;
                            Offset_Position_Y = (double)(COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 36] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 37] << 16)) / 10000;
                            Offset_Position_T = (double)(COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 38] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 39] << 16)) / 10000;
                            break;
                    }
                    Unit_Log.Tact_Write(Current_Unit, "Align Calculat : " + ((DateTime.Now - Start).TotalMilliseconds / 1000).ToString("0.000"));
                    if (Align_Sequence == Enum.Align_Sequence.Align)
                        Align_Sequence = Enum.Align_Sequence.ReAlign_Check;
                    break;
                case Enum.Align_Sequence.ReAlign_Check:
                    Unit_Log.Sequence_Write(Current_Unit, Align_Sequence.ToString());
                    if(COMMON.CTQ_Use && COMMON.CTQ_Zero_Align)
                        UVW_Moveing(0.0001, 0.0001, 0.0000);
                    else if (COMMON.CTQ_Use && COMMON.CTQ_Random_Align && Re_Align_Count == 0)
                    {
                        Random random = new Random();
                        double Rand_X;
                        double Rand_Y;
                        double Rand_T;
                        if (random.Next(0, 1) == 1)
                            Rand_X = random.NextDouble();
                        else
                            Rand_X = -random.NextDouble();
                        if (random.Next(0, 1) == 1)
                            Rand_Y = random.NextDouble();
                        else
                            Rand_Y = -random.NextDouble();
                        if (random.Next(0, 1) == 1)
                            Rand_T = random.NextDouble();
                        else
                            Rand_T = -random.NextDouble();
                        UVW_Moveing(Rand_X, Rand_Y, Rand_T);
                    }
                    else
                        UVW_Moveing(Align_Position_X + Offset_Position_X, Align_Position_Y + Offset_Position_Y, Align_Position_T + Offset_Position_T);
                    if (Re_Align_Check())
                    {
                        ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.Move_Req, true);
                        Align_Sequence = Enum.Align_Sequence.ReAlign_Move_Ack;
                    }
                    else
                    {
                        Unit_Light(false, Current_Type);
                        Align_Sequence = Enum.Align_Sequence.Align_End_Req;
                    }
                    break;
                case Enum.Align_Sequence.ReAlign_Move_Ack:
                    if (ClassInterface.Bit_Check((int)Current_Unit, (int)PLC_COMMAND.Move_Ack))
                    {
                        Unit_Log.Sequence_Write(Current_Unit, Align_Sequence.ToString());
                        Align_Sequence = Enum.Align_Sequence.ReAlign_Move_Doen_Req;
                        ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.Move_Req, false);
                    }
                    break;
                case Enum.Align_Sequence.ReAlign_Move_Doen_Req:
                    if (ClassInterface.Bit_Check((int)Current_Unit, (int)PLC_COMMAND.Move_Done_Req))
                    {
                        Unit_Log.Sequence_Write(Current_Unit, Align_Sequence.ToString());
                        Align_Sequence = Enum.Align_Sequence.ReAlign_Move_Doen_Ack;
                        ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.Move_Done_Ack, true);
                    }
                    break;
                case Enum.Align_Sequence.ReAlign_Move_Doen_Ack:
                    if (!ClassInterface.Bit_Check((int)Current_Unit, (int)PLC_COMMAND.Move_Done_Req))
                    {
                        Unit_Log.Sequence_Write(Current_Unit, Align_Sequence.ToString());
                        Align_Sequence = Enum.Align_Sequence.ReAlign_1ST;
                        ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.Move_Done_Ack, false);
                        ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.N1St_Req, true);
                    }
                    break;
                case Enum.Align_Sequence.ReAlign_1ST:
                    if (ClassInterface.Bit_Check((int)Current_Unit, (int)PLC_COMMAND.N1St_Ack))
                    {
                        Unit_Log.Sequence_Write(Current_Unit, Align_Sequence.ToString());
                        Align_Sequence = Enum.Align_Sequence.Align_Start;
                        if (ETC_Option.Second_Grab_First_Use[(int)Current_Unit])
                            Current_Position = Enum.Position.Second;
                        else
                            Current_Position = Enum.Position.First;
                        ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.N1St_Req, false);
                    }
                    break;
                case Enum.Align_Sequence.Align_End_Req:
                    Unit_Log.Sequence_Write(Current_Unit, Align_Sequence.ToString());
                    ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.Align_Start, false);
                    ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.AlignEnd, true);
                    ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.NG, false);
                    ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.OK, true);
                    if (ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.DG_DA_TO_4POSITION || ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.DG_DA)
                    {
                        Unit.Type[(int)Enum.Type.Target].Title_Label.BackColor = Color.LightGreen;
                        Unit.Type[(int)Enum.Type.Object].Title_Label.BackColor = Color.LightGreen;
                    }
                    else
                        Unit.Type[(int)Current_Type].Title_Label.BackColor = Color.LightGreen;
                    Tact = ((DateTime.Now - Start).TotalMilliseconds / 1000).ToString("0.000");
                    Unit_Log.Tact_Write(Current_Unit, "Total Tact : " + Tact);
                    Align_Sequence = Enum.Align_Sequence.Align_End_Ack;
                    break;
                case Enum.Align_Sequence.Align_End_Ack :
                    if (ClassInterface.Bit_Check((int)Current_Unit, (int)PLC_COMMAND.AlignEnd))
                    {
                        Unit_Log.Sequence_Write(Current_Unit, Align_Sequence.ToString());
                        ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.AlignEnd, false);
                        Align_Sequence = Enum.Align_Sequence.Align_End;
                    }
                    break;
                case Enum.Align_Sequence.Align_End:
                    if (!ClassInterface.Bit_Check((int)Current_Unit, (int)PLC_COMMAND.AlignEnd))
                    {
                        ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.OK, false);
                        ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.NG, false);
                        Unit_Log.Sequence_Write(Current_Unit, Align_Sequence.ToString());
                        //Main화면 Log
                        Data_Save();
#if CW_SAM
                        if (Current_Unit == Enum.Unit.Unit2)
                        {
                            COMMON.Light_Controlers[1].Manual_Light(13, 43);
                            COMMON.Light_Controlers[1].Manual_Light(14, 43);
                        }
                        if (Current_Unit == Enum.Unit.Unit3)
                        {
                            COMMON.Light_Controlers[1].Manual_Light(13, 20);
                            COMMON.Light_Controlers[1].Manual_Light(14, 20);
                        }
#endif
                        Align_Sequence = Enum.Align_Sequence.Ready;
                        Unit_Log.Sequence_Write(Current_Unit, Align_Sequence.ToString());
                        Unit_Log.Sequence_Write(Current_Unit, "--------------------------------------------------------");
                        Unit_Log.Tact_Write(Current_Unit, "PLC End Off : " + ((DateTime.Now - Start).TotalMilliseconds / 1000).ToString("0.000"));
                    }
                    break;
            }
            return Align_Sequence;
        }

        private Enum.Calibration_Sequence CalibrationSequence()
        {
            switch (Calibration_Sequence)
            {
                case Enum.Calibration_Sequence.Calibration_Start_Req:
                    if (ClassInterface.Bit_Check((int)Current_Unit, (int)PLC_COMMAND.Target_Object))
                        Current_Type = Enum.Type.Object;
                    else
                        Current_Type = Enum.Type.Target;
                    if (ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.DG_DA || ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.DG_DA_TO_4POSITION)
                    {
                        Current_Type = Enum.Type.Target;
                    }
#if CW_SAM
                    if (Current_Unit == Enum.Unit.Unit1)
                        Current_Type = Enum.Type.Object;
#endif
                    Pitch_X = Unit.Type[(int)Current_Type].Calibration_Pitch[0];
                    Pitch_Y = Unit.Type[(int)Current_Type].Calibration_Pitch[1];
                    Pitch_T = Unit.Type[(int)Current_Type].Calibration_Pitch[2];
                    if (ETC_Option.Second_Grab_First_Use[(int)Current_Unit])
                        Current_Position = Enum.Position.Second;
                    else
                        Current_Position = Enum.Position.First;
                    if (COMMON.CTQ_Use)
                    {
                        CTQ_COMMAND = COMMON.CTQ_COMMAND.Split(' ');
                        CTQ_DATA = new PointF[4, CTQ_COMMAND.Length];
                        CTQ_X = 0; CTQ_Y = 0; CTQ_T = 0;
                    }
                    Sequences = Enum.Sequence.Calibration;
                    Calibration_Sequence = Enum.Calibration_Sequence.Calibration_Start;
                    break;
                case Enum.Calibration_Sequence.Calibration_Start:
                    ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.Cal_Start, true);
                    if (!COMMON.CTQ_Use)
                    {
                        Object_Length object_Length = new Object_Length();
                        object_Length.ShowDialog();
                        Calibration_Length = object_Length.Length;
                        object_Length.Dispose();
                    }
                    if (ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._9T || ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._3Y || ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._3YT)
                        Calibration_Count = 0;
                    else if (ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._3X || ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._3XT )
                        Calibration_Count = 6;
                    else
                        Calibration_Count = 15;
                    Unit.Type[(int)Current_Type].Shuttle_Y[(int)Current_Position] = COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 18] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 19] << 16) / 1000;
                    Unit.Type[(int)Current_Type].CAM_Y[(int)Current_Position] = COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 22] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 23] << 16) / 1000;
                    Calibration_Orizin[0] = COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 10] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 11] << 16);
                    Calibration_Orizin[1] = COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 12] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 13] << 16);
                    Calibration_Orizin[2] = COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 14] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 15] << 16);
                    Calibration_Orizin[3] = COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 16] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 17] << 16);
                    Calibration_Sequence = Enum.Calibration_Sequence.UVW_Move_Req;
                    break;
                case Enum.Calibration_Sequence.UVW_Move_Req:
                    if(!COMMON.CTQ_Use)
                    {
                        if (ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._9T || Calibration_Count >= 9)
                            UVW_Moveing(Calibration_X[Calibration_Count] * COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Direction[0], Calibration_Y[Calibration_Count] * COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Direction[1], Calibration_T[Calibration_Count] * COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Direction[2]);
                        else if (ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._3X || ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._3XT)
                            UVW_Moveing(Calibration_X[Calibration_Count - 3] * COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Direction[0], Calibration_Y[Calibration_Count - 3] * COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Direction[1], Calibration_T[Calibration_Count - 3] * COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Direction[2]);
                        else if (ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._3Y || ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._3YT)
                            UVW_Moveing(Calibration_X[Calibration_Count + 1] * COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Direction[0], Calibration_Y[Calibration_Count + 1] * COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Direction[1], Calibration_T[Calibration_Count + 1] * COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Direction[2]);
                    }
                    else
                    {
                        switch (CTQ_COMMAND[Calibration_Count])
                        {
                            case "←":
                                CTQ_X -= Pitch_X;
                                break;
                            case "→":
                                CTQ_X += Pitch_X;
                                break;
                            case "↑":
                                CTQ_Y -= Pitch_Y;
                                break;
                            case "↓":
                                CTQ_Y += Pitch_Y;
                                break;
                            case "↘":
                                CTQ_T += Pitch_T;
                                break;
                            case "↙":
                                CTQ_T -= Pitch_T;
                                break;
                            case "H":
                                CTQ_X = 0; CTQ_Y = 0; CTQ_T = 0;
                                break;
                        }
                        UVW_Moveing(CTQ_X, CTQ_Y, CTQ_T);
                    }
                    DateTime Tic = DateTime.Now;
                    while (true)
                        if ((DateTime.Now - Tic).TotalMilliseconds > 100)
                            break;
                    ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.Move_Req, true);
                    Calibration_Sequence = Enum.Calibration_Sequence.UVW_Move_Ack;
                    break;
                case Enum.Calibration_Sequence.UVW_Move_Ack:
                    if (ClassInterface.Bit_Check((int)Current_Unit, (int)PLC_COMMAND.Move_Ack))
                    {
                        ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.Move_Req, false);
                        Calibration_Sequence = Enum.Calibration_Sequence.UVW_Move_Done_Req;
                    }
                    break;
                case Enum.Calibration_Sequence.UVW_Move_Done_Req:
                    if (ClassInterface.Bit_Check((int)Current_Unit, (int)PLC_COMMAND.Move_Done_Req))
                    {
                        ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.Move_Done_Ack, true);
                        Calibration_Sequence = Enum.Calibration_Sequence.UVW_Move_Done_Ack;
                    }
                    break;
                case Enum.Calibration_Sequence.UVW_Move_Done_Ack:
                    if (!ClassInterface.Bit_Check((int)Current_Unit, (int)PLC_COMMAND.Move_Done_Req))
                    {
                        ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.Move_Done_Ack, false);
                        Calibration_Sequence = Enum.Calibration_Sequence.Grab_OK;
                    }
                    break;
                case Enum.Calibration_Sequence.Grab_OK:
                    if (ETC_Option.Simul_Calibration)
                        Unit.Type[(int)Current_Type].Calibration_Grab_Image(Current_Position, Calibration_Count);
                    else
                        Unit.Type[(int)Current_Type].Calibration_Grab_Image(Current_Position);
                    if (!COMMON.CTQ_Use)
                    {
                        if (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[0 + (int)Current_Position])
                            Unit.Type[(int)Current_Type].Mark[0 + (int)Current_Position].Calibration_Data_Set(Calibration_Count);
                        if (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[1 + (int)Current_Position])
                            Unit.Type[(int)Current_Type].Mark[1 + (int)Current_Position].Calibration_Data_Set(Calibration_Count);
                        if (Calibration_Count == 15)
                        {
                            //캘데이터 저장
                            if (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[0 + (int)Current_Position])
                                Unit.Type[(int)Current_Type].Mark[0 + (int)Current_Position].Calibration_Calculation(Pitch_X, Pitch_Y);
                            if (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[1 + (int)Current_Position])
                                Unit.Type[(int)Current_Type].Mark[1 + (int)Current_Position].Calibration_Calculation(Pitch_X, Pitch_Y);

                            if (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[0 + (int)Current_Position] &&
                                COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[1 + (int)Current_Position])
                            {
                                Unit.Type[(int)Current_Type].Camera_Length = Calibration_Cam_Pitch(Calibration_Length, Current_Position);
                                Calculation_Theta_Pitch = Calibration_Calculation_Theta(Unit.Type[(int)Current_Type].Camera_Length, Current_Position);
                                Unit.Type[(int)Current_Type].Ratio = Pitch_T / (Calculation_Theta_Pitch * 2);
                                Unit.Type[(int)Current_Type].Mark[0 + (int)Current_Position].Calibration_Theta(Calculation_Theta_Pitch);
                                Unit.Type[(int)Current_Type].Mark[1 + (int)Current_Position].Calibration_Theta(Calculation_Theta_Pitch);
                            }
                            if ((!ETC_Option.Second_Grab_First_Use[(int)Current_Unit] && Current_Position == Enum.Position.First && (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[2] || COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[3])) || (Current_Position == Enum.Position.Second && ETC_Option.Second_Grab_First_Use[(int)Current_Unit]))
                            {
                                if (ETC_Option.Second_Grab_First_Use[(int)Current_Unit])
                                    Current_Position = Enum.Position.First;
                                else
                                    Current_Position = Enum.Position.Second;
                                Calibration_Sequence = Enum.Calibration_Sequence.Move_2nd_Req;
                                if (ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._9T || ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._3Y || ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._3YT)
                                    Calibration_Count = 0;
                                else if (ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._3X || ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._3XT)
                                    Calibration_Count = 6;
                                else
                                    Calibration_Count = 15;
                            }
                            else
                            {
                                Current_Position = Enum.Position.First;
                                Calibration_Sequence = Enum.Calibration_Sequence.Calibration_End_Req;
                            }
                        }
                        else
                        {
                            if (ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._9T || ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._3X || ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._3XT || Calibration_Count >= 9)
                                Calibration_Count++;
                            else if (ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._3Y || ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._3YT)
                                Calibration_Count = Calibration_Count + 3;
                            Calibration_Sequence = Enum.Calibration_Sequence.UVW_Move_Req;
                        }
                    }
                    else
                    {
                        CTQ_DATA[0 + (int)Current_Position, Calibration_Count] = new PointF((float)Unit.Type[(int)Current_Type].Mark[0 + (int)Current_Position].X, (float)Unit.Type[(int)Current_Type].Mark[0 + (int)Current_Position].Y);
                        CTQ_DATA[1 + (int)Current_Position, Calibration_Count] = new PointF((float)Unit.Type[(int)Current_Type].Mark[1 + (int)Current_Position].X, (float)Unit.Type[(int)Current_Type].Mark[1 + (int)Current_Position].Y);
                        if (Calibration_Count == CTQ_COMMAND.Length - 1)
                        {
                            if ((!ETC_Option.Second_Grab_First_Use[(int)Current_Unit] && Current_Position == Enum.Position.First && (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[2] || COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[3])) || (Current_Position == Enum.Position.Second && ETC_Option.Second_Grab_First_Use[(int)Current_Unit]))
                            {
                                if (ETC_Option.Second_Grab_First_Use[(int)Current_Unit])
                                    Current_Position = Enum.Position.First;
                                else
                                    Current_Position = Enum.Position.Second;
                                Calibration_Sequence = Enum.Calibration_Sequence.Move_2nd_Req;
                                Calibration_Count = 0;
                            }
                            else
                            {
                                Current_Position = Enum.Position.First;
                                Calibration_Sequence = Enum.Calibration_Sequence.Calibration_End_Req;
                            }
                        }
                        else
                        {
                            Calibration_Count++;
                            Calibration_Sequence = Enum.Calibration_Sequence.UVW_Move_Req;
                        }
                    }

                    break;
                case Enum.Calibration_Sequence.Move_2nd_Req:
                    ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.N2nd_Req, true);
                    Calibration_Sequence = Enum.Calibration_Sequence.Move_2nd_Ack;
                    break;
                case Enum.Calibration_Sequence.Move_2nd_Ack:
                    if (ClassInterface.Bit_Check((int)Current_Unit, (int)PLC_COMMAND.N2nd_Ack))
                    {
                        ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.N2nd_Req, false);
                        Unit.Type[(int)Current_Type].Shuttle_Y[(int)Current_Position] = COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 18] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 19] << 16) / 1000;
                        Unit.Type[(int)Current_Type].CAM_Y[(int)Current_Position] = COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 22] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 23] << 16) / 1000;
                        //Calibration_Orizin[0] = COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 10] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 11] << 16);
                        //Calibration_Orizin[1] = COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 12] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 13] << 16);
                        //Calibration_Orizin[2] = COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 14] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 15] << 16);
                        //Calibration_Orizin[3] = COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 16] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 17] << 16);
                        Calibration_Sequence = Enum.Calibration_Sequence.UVW_Move_Req;
                    }
                    break;
                case Enum.Calibration_Sequence.Calibration_End_Req:
                    ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.Cal_End, true);
                    ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.Cal_Start, false);
                    if (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[0] && !COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[1] &&
                         COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[2] && !COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[3])
                    {//좌측만 사용
                        Unit.Type[(int)Current_Type].Shuttle_Length = Shuttle_Pitch(Unit.Type[(int)Current_Type].Mark[0].Resolution_Y, Unit.Type[(int)Current_Type].Mark[2].Resolution_Y, Unit.Type[(int)Current_Type].Mark[0].Camera_Height);
                        Calibration_Calculation_Theta(Unit.Type[(int)Current_Type].Shuttle_Length, 0);
                        Calculation_Theta_Pitch = Calculation_Theta_Pitch = Calibration_Calculation_Theta(Unit.Type[(int)Current_Type].Camera_Length, Current_Position);
                        Unit.Type[(int)Current_Type].Ratio = Pitch_T / (Calculation_Theta_Pitch * 2);
                        Unit.Type[(int)Current_Type].Mark[0].Calibration_Theta(Calculation_Theta_Pitch);
                        Unit.Type[(int)Current_Type].Mark[2].Calibration_Theta(Calculation_Theta_Pitch);
                    }
                    else if (!COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[0] && COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[1] &&
                         !COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[2] && COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[3])
                    {//우측만 사용
                        Unit.Type[(int)Current_Type].Shuttle_Length = Shuttle_Pitch(Unit.Type[(int)Current_Type].Mark[1].Resolution_Y, Unit.Type[(int)Current_Type].Mark[3].Resolution_Y, Unit.Type[(int)Current_Type].Mark[1].Camera_Height);
                        Calibration_Calculation_Theta(Unit.Type[(int)Current_Type].Shuttle_Length, 1);
                        Calculation_Theta_Pitch = Calculation_Theta_Pitch = Calibration_Calculation_Theta(Unit.Type[(int)Current_Type].Camera_Length, Current_Position);
                        Unit.Type[(int)Current_Type].Ratio = Pitch_T / (Calculation_Theta_Pitch * 2);
                        Unit.Type[(int)Current_Type].Mark[1].Calibration_Theta(Calculation_Theta_Pitch);
                        Unit.Type[(int)Current_Type].Mark[3].Calibration_Theta(Calculation_Theta_Pitch);
                    }
                    Calibration_Sequence = Enum.Calibration_Sequence.Calibration_End_Ack;
                    break;
                case Enum.Calibration_Sequence.Calibration_End_Ack:
                    if (ClassInterface.Bit_Check((int)Current_Unit, (int)PLC_COMMAND.Cal_End))
                    {
                        ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.Cal_End, false);
                        Calibration_Sequence = Enum.Calibration_Sequence.Calibration_End;
                    }
                    break;
                case Enum.Calibration_Sequence.Calibration_End:
                    if (!ClassInterface.Bit_Check((int)Current_Unit, (int)PLC_COMMAND.Cal_End))
                    {
                        if (!COMMON.CTQ_Use)
                        {
                            if (ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.DG_DA || ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.DG_DA_TO_4POSITION)
                            {
                                Unit.Type[(int)Current_Type].Calibration_Calculation();
                                Unit.Type[(int)Current_Type].Calibration_Data_Save();
                                for (int LoopCount = 0; LoopCount < 4; LoopCount++)
                                {
                                    if (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[LoopCount])
                                        Unit.Type[(int)Current_Type].Mark[LoopCount].Calibration_Data_Save();
                                }
#if CW_SAM
                                Current_Type = Enum.Type.Target;
                                Unit.Type[(int)Current_Type].Calibration_Copy(Enum.Type.Object);
                                Unit.Type[(int)Current_Type].Calibration_Data_Save();
                                for (int LoopCount = 0; LoopCount < 4; LoopCount++)
                                {
                                    if (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[LoopCount])
                                    {
                                        Unit.Type[(int)Current_Type].Mark[LoopCount].Calibration_Copy(Enum.Type.Object);
                                        Unit.Type[(int)Current_Type].Mark[LoopCount].Calibration_Data_Save();
                                    }
                                }
#else
                                Current_Type = Enum.Type.Object;
                                Unit.Type[(int)Current_Type].Calibration_Copy(Enum.Type.Target);
                                Unit.Type[(int)Current_Type].Calibration_Data_Save();
                                for (int LoopCount = 0; LoopCount < 4; LoopCount++)
                                {
                                    if (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[LoopCount])
                                    {
                                        Unit.Type[(int)Current_Type].Mark[LoopCount].Calibration_Copy(Enum.Type.Target);
                                        Unit.Type[(int)Current_Type].Mark[LoopCount].Calibration_Data_Save();
                                    }
                                }
#endif
                            }
                            else
                            {
                                Unit.Type[(int)Current_Type].Calibration_Calculation();
                                Unit.Type[(int)Current_Type].Calibration_Data_Save();
                                for (int LoopCount = 0; LoopCount < 4; LoopCount++)
                                {
                                    if (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[LoopCount])
                                        Unit.Type[(int)Current_Type].Mark[LoopCount].Calibration_Data_Save();
                                }
                            }
                            COMMON.Frame.Invoke((MethodInvoker)(() =>
                            {
                                Calibration_Result calibration_Result = new Calibration_Result(COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type]);
                                calibration_Result.ShowDialog();
                                calibration_Result.Dispose();
                            }));
                        }
                        else
                            Unit_Log.CTQ_Accuracy_Write(Current_Unit,CTQ_DATA);
                        Calibration_Sequence = Enum.Calibration_Sequence.Ready;
                    }
                    break;
            }
            return Calibration_Sequence;
        }

        private void Unit_Grab(Enum.Position _Position)
        {
#if CW_SAM
            if(!(Current_Unit == Enum.Unit.Unit1 && Current_Type == Enum.Type.Object))
            {
                if (Current_Unit == Enum.Unit.Unit3)
                {
                    DateTime Tic = DateTime.Now;
                    while (true)
                        if ((DateTime.Now - Tic).TotalMilliseconds > 300)
                            break;
                }
                else
                {
                    DateTime Tic = DateTime.Now;
                    while (true)
                        if ((DateTime.Now - Tic).TotalMilliseconds > 500)
                            break;
                }
            }
#elif LARGE_AREA_LAMI
            DateTime Tic = DateTime.Now;
            while (true)
                if ((DateTime.Now - Tic).TotalMilliseconds > 500)
                    break;
#endif
            Unit.Type[(int)Current_Type].Shuttle_Y[(int)Current_Position] = COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 18] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 19] << 16) / 1000;
            Unit.Type[(int)Current_Type].CAM_Y[(int)Current_Position] = COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 22] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 23] << 16) / 1000;
            if (Align_Sequence == Enum.Align_Sequence.Align_Start || Align_Sequence == Enum.Align_Sequence.Target_Object_Ack_1ST)
            {
                if ((!COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[0] && !COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[1]) ||
                    (!COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[2] && !COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[3]))
                {
                    Align_Sequence = Enum.Align_Sequence.Align;
                    for (int LoopCount = 0; LoopCount < 4; LoopCount++)
                        if (!COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[LoopCount])
                        {
                            Unit.Type[(int)Current_Type].Mark[LoopCount].Display.Image = null;
                            Unit.Type[(int)Current_Type].Mark[LoopCount].X = 0;
                            Unit.Type[(int)Current_Type].Mark[LoopCount].Y = 0;
                        }

                }
                else
                {
                    Align_Sequence = Enum.Align_Sequence.Move_2nd_Req;
                }
            }
            else
                Align_Sequence = Enum.Align_Sequence.Align;

            switch (ETC_Option.Unit_Type[(int)Current_Unit])
            {
                case Enum.Unit_Type.DG_DA:
                    Unit.Type[(int)Enum.Type.Target].Auto_Grab_Image(_Position);
                    Unit_Log.Tact_Write(Current_Unit, "Target" + _Position.ToString() + "Grab : " + ((DateTime.Now - Start).TotalMilliseconds / 1000).ToString("0.000"));
                    Unit.Type[(int)Enum.Type.Object].Auto_Grab_Image(_Position);
                    Unit_Log.Tact_Write(Current_Unit, "Object" + _Position.ToString() + "Grab : " + ((DateTime.Now - Start).TotalMilliseconds / 1000).ToString("0.000"));
                    break;
                case Enum.Unit_Type.DG_DA_TO_4POSITION:
                    if (Current_Type == Enum.Type.Target)
                    {
                        Unit.Type[(int)Current_Type].Auto_Grab_Image(_Position);
                        Unit_Log.Tact_Write(Current_Unit, Current_Type.ToString() + _Position.ToString() + "Grab : " + ((DateTime.Now - Start).TotalMilliseconds / 1000).ToString("0.000"));
                        if (Align_Sequence == Enum.Align_Sequence.Move_2nd_Req)
                            Align_Sequence = Enum.Align_Sequence.Target_Object_Req_1ST;
                        else
                            Align_Sequence = Enum.Align_Sequence.Target_Object_Req_2ND;
                    }
                    else
                    {
                        Unit.Type[(int)Current_Type].Auto_Grab_Image(_Position);
                        Unit_Log.Tact_Write(Current_Unit, Current_Type.ToString() + _Position.ToString() + "Grab : " + ((DateTime.Now - Start).TotalMilliseconds / 1000).ToString("0.000"));
                        Current_Type = Enum.Type.Target;
                    }
                    break;
                default:
                    Unit.Type[(int)Current_Type].Auto_Grab_Image(_Position);
                    Unit_Log.Tact_Write(Current_Unit, Current_Type.ToString() + _Position.ToString() + "Grab : " + ((DateTime.Now - Start).TotalMilliseconds / 1000).ToString("0.000"));
                    break;
            }
        }

        private void Inspection_Grab(Enum.Position _Position)
        {
#if LARGE_AREA_LAMI
            DateTime Tic = DateTime.Now;
            while (true)
                if ((DateTime.Now - Tic).TotalMilliseconds > 500)
                    break;
#endif
            Unit.Type[(int)Current_Type].Shuttle_Y[(int)Current_Position] = COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 18] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 19] << 16) / 1000;
            Unit.Type[(int)Current_Type].CAM_Y[(int)Current_Position] = COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 22] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 23] << 16) / 1000;
            if (Inspection_Sequence == Enum.Inspection_Sequence.Inspecion_Start)
                Inspection_Sequence = Enum.Inspection_Sequence.Move_2nd_Req;
            else
                Inspection_Sequence = Enum.Inspection_Sequence.Inspection;

            Unit.Type[(int)Enum.Type.Target].Auto_Grab_Image(_Position);
            Unit_Log.Tact_Write(Current_Unit, "Target" + _Position.ToString() + "Grab : " + ((DateTime.Now - Start).TotalMilliseconds / 1000).ToString("0.000"));
            Unit.Type[(int)Enum.Type.Object].Auto_Grab_Image(_Position);
            Unit_Log.Tact_Write(Current_Unit, "Object" + _Position.ToString() + "Grab : " + ((DateTime.Now - Start).TotalMilliseconds / 1000).ToString("0.000"));
        }

        private bool Light_Check(bool[] Select)
        {
            for (int LoopCount = 0; LoopCount < Select.Length; LoopCount++)
                if (Select[LoopCount])
                    return true;
            return false;
        }

        private void Unit_Light(bool On_Off, Enum.Type _Type)
        {
            if (Recipe.Auto_Light)
            {
                switch (ETC_Option.Unit_Type[(int)Current_Unit])
                {
                    case Enum.Unit_Type.DG_DA:
                        for (int LoopCount = 0; LoopCount < 16; LoopCount++)
                        {
                            if (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Target].Useing_Comport[LoopCount])
                            {
                                bool[] Select = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
                                for (int LoopCount2 = 0; LoopCount2 < 6; LoopCount2++)
                                {
                                    try
                                    {
                                        if (int.Parse(COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Target].ComPort[LoopCount2].Substring(3, COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Target].ComPort[LoopCount2].Length - 3)) == LoopCount)
                                            Select[COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Target].Ch[LoopCount2]] = true;
                                    }
                                    catch { }
                                }
                                if (Light_Check(Select))
                                {
                                    string retult;
                                    if (On_Off)
                                        retult = COMMON.Light_Controlers[LoopCount].Light_On(Select);
                                    else
                                        retult = COMMON.Light_Controlers[LoopCount].Light_Off(Select);
                                    Unit_Log.Sequence_Write(Current_Unit, retult);
                                }
                            }
                        }
                        for (int LoopCount = 0; LoopCount < 16; LoopCount++)
                        {
                            if (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Object].Useing_Comport[LoopCount])
                            {
                                bool[] Select = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
                                for (int LoopCount2 = 0; LoopCount2 < 6; LoopCount2++)
                                {
                                    try
                                    {
                                        if (int.Parse(COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Object].ComPort[LoopCount2].Substring(3, COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Object].ComPort[LoopCount2].Length - 3)) == LoopCount)
                                            Select[COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Object].Ch[LoopCount2]] = true;
                                    }
                                    catch { }
                                }
                                if (Light_Check(Select))
                                {
                                    string retult;
                                    if (On_Off)
                                        retult = COMMON.Light_Controlers[LoopCount].Light_On(Select);
                                    else
                                        retult = COMMON.Light_Controlers[LoopCount].Light_Off(Select);
                                    Unit_Log.Sequence_Write(Current_Unit, retult);
                                }
                            }
                        }
                        break;
                    case Enum.Unit_Type.DG_DA_TO_4POSITION:
                        for (int LoopCount = 0; LoopCount < 16; LoopCount++)
                        {
                            if (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Target].Useing_Comport[LoopCount])
                            {
                                bool[] Select = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
                                for (int LoopCount2 = 0; LoopCount2 < 6; LoopCount2++)
                                {
                                    try
                                    {
                                        if (int.Parse(COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Target].ComPort[LoopCount2].Substring(3, COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Target].ComPort[LoopCount2].Length - 3)) == LoopCount)
                                            Select[COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Target].Ch[LoopCount2]] = true;
                                    }
                                    catch { }
                                }
                                if (Light_Check(Select))
                                {
                                    string retult;
                                    if (On_Off)
                                        retult = COMMON.Light_Controlers[LoopCount].Light_On(Select);
                                    else
                                        retult = COMMON.Light_Controlers[LoopCount].Light_Off(Select);
                                    Unit_Log.Sequence_Write(Current_Unit, retult);
                                }
                            }
                        }
                        for (int LoopCount = 0; LoopCount < 16; LoopCount++)
                        {
                            if (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Object].Useing_Comport[LoopCount])
                            {
                                bool[] Select = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
                                for (int LoopCount2 = 0; LoopCount2 < 6; LoopCount2++)
                                {
                                    try
                                    {
                                        if (int.Parse(COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Object].ComPort[LoopCount2].Substring(3, COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Object].ComPort[LoopCount2].Length - 3)) == LoopCount)
                                            Select[COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Object].Ch[LoopCount2]] = true;
                                    }
                                    catch { }
                                }
                                if (Light_Check(Select))
                                {
                                    string retult;
                                    if (On_Off)
                                        retult = COMMON.Light_Controlers[LoopCount].Light_On(Select);
                                    else
                                        retult = COMMON.Light_Controlers[LoopCount].Light_Off(Select);
                                    Unit_Log.Sequence_Write(Current_Unit, retult);
                                }
                            }
                        }
                        break;
                    default:
                        for(int LoopCount=0;LoopCount<16;LoopCount++)
                        {
                            if(COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Useing_Comport[LoopCount])
                            {
                                bool[] Select = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
                                for (int LoopCount2=0;LoopCount2<6;LoopCount2++)
                                {
                                    try
                                    {
                                        if (int.Parse(COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].ComPort[LoopCount2].Substring(3, COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].ComPort[LoopCount2].Length - 3)) == LoopCount)
                                            Select[COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Ch[LoopCount2]] = true;
                                    }
                                    catch { }
                                }
                                if (Light_Check(Select))
                                {
                                    string retult;
                                    if (On_Off)
                                        retult = COMMON.Light_Controlers[LoopCount].Light_On(Select);
                                    else
                                        retult = COMMON.Light_Controlers[LoopCount].Light_Off(Select);
                                    Unit_Log.Sequence_Write(Current_Unit, retult);
                                }
                            }
                        }
                        break;
                }
            }
        }

        private bool Re_Align_Check()
        {
            if (((Math.Abs(Align_Position_X - Offset_Position_X) < Re_Align_Spec && 
                Math.Abs(Align_Position_Y - Offset_Position_Y) < Re_Align_Spec && 
                Math.Abs(Align_Position_T - Offset_Position_T) < Re_Align_Spec) || !(Recipe.Re_Align_Use && ClassInterface.Bit_Check((int)Current_Unit, (int)PLC_COMMAND.ReAlign))) )
                return false;
            else
            {
                Tact = ((DateTime.Now - Start).TotalMilliseconds / 1000).ToString("0.000");
                Data_Save();

                return true;
            }
        }


        ///////////////////////////////////////////
        /// 얼라인 스타트 리턴값
        /// 0 : 정상
        /// 2 : 마크NG
        /// 11 : L CHECK NG
        /// -1 : 원인불명 디버깅 필요
        ///////////////////////////////////////////
        private void Unit_Align()
        {
            if (ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.DG_DA || ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.DG_DA_TO_4POSITION)
            {
                int Error = Unit.Type[(int)Enum.Type.Target].Align_Start(ref Align_Position_X, ref Align_Position_Y, ref Align_Position_T);
                if (Error == 0)
                {
                    Keep_Data_X = Align_Position_X * (double)COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Target].Direction[0];
                    Keep_Data_Y = Align_Position_Y * COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Target].Direction[1];
                    Keep_Data_T = Align_Position_T * COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Target].Direction[2];
                    Align_Position_X = 0; Align_Position_Y = 0; Align_Position_T = 0;
                    Error = Unit.Type[(int)Enum.Type.Object].Align_Start(ref Align_Position_X, ref Align_Position_Y, ref Align_Position_T);
                    if (Error == 0)
                    {
                        Align_Position_X = Align_Position_X * COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Object].Direction[0];
                        Align_Position_Y = Align_Position_Y * COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Object].Direction[1];
                        Align_Position_T = Align_Position_T * COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Object].Direction[2];
                        Align_Position_X += Keep_Data_X;
                        Align_Position_Y += Keep_Data_Y;
                        Align_Position_T += Keep_Data_T;
                        Error_Code_Set(Current_Unit, Enum.Error_Code.NON);
                        return;
                    }
                    else
                    {
                        Align_Error(Error);
                        return;
                    }
                }
                else
                {
                    Align_Error(Error);
                    return;
                }
            }
            else if (ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.SG_DA_UNIT || ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.SG_SA || ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.SG_DA_TO)
            {
                int Error = Unit.Type[(int)Current_Type].Align_Start(ref Align_Position_X, ref Align_Position_Y, ref Align_Position_T);
                if (Error == 0)
                {
                    Align_Position_X = Align_Position_X * COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Direction[0];
                    Align_Position_Y = Align_Position_Y * COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Direction[1];
                    Align_Position_T = Align_Position_T * COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Direction[2];

                    if (ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.SG_DA_UNIT && ETC_Option.DA_UNIT[(int)Current_Unit] != 0)
                    {
                        Align_Position_X += COMMON.Frame.Unit[ETC_Option.DA_UNIT[(int)Current_Unit]].Sequence.Align_Position_X;
                        Align_Position_Y += COMMON.Frame.Unit[ETC_Option.DA_UNIT[(int)Current_Unit]].Sequence.Align_Position_Y;
                        Align_Position_T += COMMON.Frame.Unit[ETC_Option.DA_UNIT[(int)Current_Unit]].Sequence.Align_Position_T;
                    }
                    if (ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.SG_DA_TO && Current_Type == Enum.Type.Target)
                    {
                        Keep_Data_X = Align_Position_X;
                        Keep_Data_Y = Align_Position_Y;
                        Keep_Data_T = Align_Position_T;
                    }
                    if (ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.SG_DA_TO && Current_Type == Enum.Type.Object)
                    {
                        Align_Position_X += Keep_Data_X;
                        Align_Position_Y += Keep_Data_Y;
                        Align_Position_T += Keep_Data_T;
                        Keep_Data_X = 0;
                        Keep_Data_Y = 0;
                        Keep_Data_T = 0;
                    }
                    Error_Code_Set(Current_Unit, Enum.Error_Code.NON);
                    return;
                }
                else
                {
                    Align_Error(Error);
                    return;
                }
            }
        }

        private void Align_Error(int Code)
        {
            if (ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.DG_DA_TO_4POSITION || ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.DG_DA)
            {
                Unit.Type[(int)Enum.Type.Target].Title_Label.BackColor = Color.Red;
                Unit.Type[(int)Enum.Type.Object].Title_Label.BackColor = Color.Red;
            }
            else
                Unit.Type[(int)Current_Type].Title_Label.BackColor = Color.Red;
            switch (Code)
            {
                case 1:
                    Unit_Log.Sequence_Write(Current_Unit, "Grab NG");
                    Align_Sequence = Enum.Align_Sequence.Error;
                    break;
                case 2:
                    Unit_Log.Sequence_Write(Current_Unit, "Manual Mark");
                    for (int LoopCount = 0; LoopCount < 4; LoopCount++)
                    {
                        if (ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.DG_DA_TO_4POSITION || ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.DG_DA)
                        {
                            if (Unit.Type[(int)Enum.Type.Target].Mark[LoopCount].Mark_Error)
                                Unit_Log.NG_Picture_Save((int)Current_Unit, (int)Enum.Type.Target, DateTime.Now, Cell_ID, LoopCount);
                            if (Unit.Type[(int)Enum.Type.Object].Mark[LoopCount].Mark_Error)
                                Unit_Log.NG_Picture_Save((int)Current_Unit, (int)Enum.Type.Object, DateTime.Now, Cell_ID, LoopCount);
                        }
                        else
                        {
                            if (Unit.Type[(int)Current_Type].Mark[LoopCount].Mark_Error)
                                Unit_Log.NG_Picture_Save((int)Current_Unit, (int)Current_Type, DateTime.Now, Cell_ID, LoopCount);
                        }
                    }
                    Align_Sequence = Enum.Align_Sequence.Manual_Mark;
                    break;
                case 11:
                    Unit_Log.Sequence_Write(Current_Unit, "L Check Error");
                    Align_Sequence = Enum.Align_Sequence.Error;
                    break;
                case 12:
                    Unit_Log.Sequence_Write(Current_Unit, "Limit Error");
                    Align_Sequence = Enum.Align_Sequence.Error;
                    break;
                default:
                    Unit_Log.Sequence_Write(Current_Unit, "unKnow Error");
                    Align_Sequence = Enum.Align_Sequence.Error;
                    break;
            }
            Error_Code_Set(Current_Unit, (Enum.Error_Code)Code);
            ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.NG, true);
        }

        private void Inspection_Error(int Code)
        {
            COMMON.Inspection_Tiltle.BackColor = Color.Red;
            switch (Code)
            {
                case 1:
                    Unit_Log.Sequence_Write(Current_Unit, "Grab NG");
                    Inspection_Sequence = Enum.Inspection_Sequence.Error;
                    break;
                case 2:
                    Unit_Log.Sequence_Write(Current_Unit, "Manual Mark");
                    for (int LoopCount = 0; LoopCount < 4; LoopCount++)
                    {
                        if (ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.DG_DA_TO_4POSITION || ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.DG_DA)
                        {
                            if (Unit.Type[(int)Enum.Type.Target].Mark[LoopCount].Mark_Error)
                                Unit_Log.NG_Picture_Save((int)Current_Unit, (int)Enum.Type.Target, DateTime.Now, Cell_ID, LoopCount);
                            if (Unit.Type[(int)Enum.Type.Object].Mark[LoopCount].Mark_Error)
                                Unit_Log.NG_Picture_Save((int)Current_Unit, (int)Enum.Type.Object, DateTime.Now, Cell_ID, LoopCount);
                        }
                        else
                        {
                            if (Unit.Type[(int)Current_Type].Mark[LoopCount].Mark_Error)
                                Unit_Log.NG_Picture_Save((int)Current_Unit, (int)Current_Type, DateTime.Now, Cell_ID, LoopCount);
                        }
                    }
                    Inspection_Sequence = Enum.Inspection_Sequence.Manual_Mark;
                    break;
                case 11:
                    Unit_Log.Sequence_Write(Current_Unit, "L Check Error");
                    Inspection_Sequence = Enum.Inspection_Sequence.Error;
                    break;
                case 12:
                    Unit_Log.Sequence_Write(Current_Unit, "Limit Error");
                    Inspection_Sequence = Enum.Inspection_Sequence.Error;
                    break;
                default:
                    Unit_Log.Sequence_Write(Current_Unit, "unKnow Error");
                    Inspection_Sequence = Enum.Inspection_Sequence.Error;
                    break;
            }
            Error_Code_Set(Current_Unit, (Enum.Error_Code)Code);
            ClassInterface.Bit_On_Off((int)Current_Unit, (int)PC_COMMAND.NG, true);
        }

        private void Error_Code_Set(Enum.Unit _Unit, Enum.Error_Code _Code)
        {
            ClassInterface.Word_Write((int)_Unit + 259, (int)_Code);
        }

        private void Data_Save()  //데이터 세이브 항목들 수정해야함
        {
            DateTime Now = DateTime.Now;
            object[] Data1;
            switch (ETC_Option.Log_Type[(int)Current_Unit])
            {
                case Enum.Log_Type.CW_Log:
                    Data1 = new object[] { Now.ToString("HH:mm:ss") ,Cell_ID, Tact ,Re_Align_Count.ToString(),
                        (Unit.Type[(int)Current_Type].P_Result_Y).ToString("0.000") , (-Unit.Type[(int)Current_Type].P_Result_X).ToString("0.000") , (Unit.Type[(int)Current_Type].P_Result_T).ToString("0.000") ,
                        (P_Offset_Position_X).ToString("0.000") , (P_Offset_Position_Y).ToString("0.000") , (P_Offset_Position_T).ToString("0.000") ,
                        (Unit.Type[(int)Current_Type].L_Result_Y).ToString("0.000") , (-Unit.Type[(int)Current_Type].L_Result_X).ToString("0.000") , (Unit.Type[(int)Current_Type].L_Result_T).ToString("0.000") ,
                        (L_Offset_Position_X).ToString("0.000") , (L_Offset_Position_Y).ToString("0.000") , (L_Offset_Position_T).ToString("0.000") ,
                        Align_Position_X.ToString("0.000") , Align_Position_Y.ToString("0.000") , Align_Position_T.ToString("0.000") , Offset_Position_X.ToString("0.000") , Offset_Position_Y.ToString("0.000") , Offset_Position_T.ToString("0.000") };
                    break;
                default:
                    Data1 = new object[] { Now.ToString("HH:mm:ss") ,Cell_ID, Tact ,Re_Align_Count.ToString(), (Align_Position_X + Offset_Position_X).ToString("0.000") , (Align_Position_Y + Offset_Position_Y).ToString("0.000") , (Align_Position_T + Offset_Position_T).ToString("0.000") ,
                        Align_Position_X.ToString("0.000") , Align_Position_Y.ToString("0.000") , Align_Position_T.ToString("0.000") , Offset_Position_X.ToString("0.000") , Offset_Position_Y.ToString("0.000") , Offset_Position_T.ToString("0.000") };
                    break;
            }
            object[] Data2 = new object[48];
            for (int LoopCount = 0; LoopCount < 20; LoopCount = LoopCount + 5)
            {
                Data2[LoopCount] = Unit.Type[0].Mark[LoopCount / 5].Last_Mark_Index.ToString();
                Data2[LoopCount + 1] = Unit.Type[0].Mark[LoopCount / 5].X.ToString("0.000");
                Data2[LoopCount + 2] = Unit.Type[0].Mark[LoopCount / 5].Y.ToString("0.000");
                Data2[LoopCount + 3] = Unit.Type[0].Mark[LoopCount / 5].S.ToString("0.000");
                Data2[LoopCount + 4] = Unit.Type[0].Mark[LoopCount / 5].AR.ToString("0.000");
            }
            for (int LoopCount = 20; LoopCount < 40; LoopCount = LoopCount + 5)
            {
                Data2[LoopCount] = Unit.Type[2].Mark[LoopCount / 5 - 4].Last_Mark_Index.ToString();
                Data2[LoopCount + 1] = Unit.Type[2].Mark[LoopCount / 5 - 4].X.ToString("0.000");
                Data2[LoopCount + 2] = Unit.Type[2].Mark[LoopCount / 5 - 4].Y.ToString("0.000");
                Data2[LoopCount + 3] = Unit.Type[2].Mark[LoopCount / 5 - 4].S.ToString("0.000");
                Data2[LoopCount + 4] = Unit.Type[2].Mark[LoopCount / 5 - 4].AR.ToString("0.000");
            }
            for (int LoopCount = 40; LoopCount < 44; LoopCount++)
            {
                Data2[LoopCount] = ((Unit.Type[0].L_Check[LoopCount % 4] / 1000) + Unit.Type[0].L_Check_Offset[LoopCount % 4]).ToString("0.000");
            }
            for (int LoopCount = 44; LoopCount < 48; LoopCount++)
            {
                Data2[LoopCount] = ((Unit.Type[2].L_Check[LoopCount % 4] / 1000) + Unit.Type[2].L_Check_Offset[LoopCount % 4]).ToString("0.000");
            }


            var List = new List<object>();
            List.AddRange(Data1);
            List.AddRange(Data2);
            object[] Data = List.ToArray();
            if (COMMON.CTQ_Use && COMMON.CTQ_Zero_Align)
                Unit_Log.Align_Data_Write(Current_Unit, Data, "Moveing");
            else if (COMMON.CTQ_Use && COMMON.CTQ_Random_Align)
                Unit_Log.Align_Data_Write(Current_Unit, Data, "REALIGN");
            else
                Unit_Log.Align_Data_Write(Current_Unit, Data);

            switch (ETC_Option.Log_Type[(int)Current_Unit])
            {
                case Enum.Log_Type.TO_Log:
                    Data = new object[] { Now.ToString("HH:mm:ss") , Tact , (Align_Position_X + Offset_Position_X).ToString("0.000") , (Align_Position_Y + Offset_Position_Y).ToString("0.000") , (Align_Position_T + Offset_Position_T).ToString("0.000") ,
                        Keep_Data_X.ToString("0.000"),Keep_Data_Y.ToString("0.000"),Keep_Data_T.ToString("0.000"),(Align_Position_X - Keep_Data_X).ToString("0.000"),(Align_Position_Y - Keep_Data_Y).ToString("0.000"),(Align_Position_T - Keep_Data_T).ToString("0.000"),
                        Align_Position_X.ToString("0.000") , Align_Position_Y.ToString("0.000") , Align_Position_T.ToString("0.000") , Offset_Position_X.ToString("0.000") , Offset_Position_Y.ToString("0.000") , Offset_Position_T.ToString("0.000"),
                              ((Unit.Type[0].L_Check[0] / 1000) + Unit.Type[0].L_Check_Offset[0]).ToString("0.000") ,((Unit.Type[0].L_Check[1] / 1000) + Unit.Type[0].L_Check_Offset[1]).ToString("0.000") ,((Unit.Type[0].L_Check[2] / 1000) + Unit.Type[0].L_Check_Offset[2]).ToString("0.000") ,((Unit.Type[0].L_Check[3] / 1000) + Unit.Type[0].L_Check_Offset[3]).ToString("0.000"),
                        ((Unit.Type[2].L_Check[0] / 1000) + Unit.Type[2].L_Check_Offset[0]).ToString("0.000") ,((Unit.Type[2].L_Check[1] / 1000) + Unit.Type[2].L_Check_Offset[1]).ToString("0.000") ,((Unit.Type[2].L_Check[2] / 1000) + Unit.Type[2].L_Check_Offset[2]).ToString("0.000") ,((Unit.Type[2].L_Check[3] / 1000) + Unit.Type[2].L_Check_Offset[3]).ToString("0.000")};
                    break;
                case Enum.Log_Type.T_Log:
                    Data = new object[] { Now.ToString("HH:mm:ss") , Tact , (Align_Position_X + Offset_Position_X).ToString("0.000") , (Align_Position_Y + Offset_Position_Y).ToString("0.000") , (Align_Position_T + Offset_Position_T).ToString("0.000") ,
                        Align_Position_X.ToString("0.000") , Align_Position_Y.ToString("0.000") , Align_Position_T.ToString("0.000") , Offset_Position_X.ToString("0.000") , Offset_Position_Y.ToString("0.000") , Offset_Position_T.ToString("0.000"),
                              ((Unit.Type[0].L_Check[0] / 1000) + Unit.Type[0].L_Check_Offset[0]).ToString("0.000") ,((Unit.Type[0].L_Check[1] / 1000) + Unit.Type[0].L_Check_Offset[1]).ToString("0.000") ,((Unit.Type[0].L_Check[2] / 1000) + Unit.Type[0].L_Check_Offset[2]).ToString("0.000") ,((Unit.Type[0].L_Check[3] / 1000) + Unit.Type[0].L_Check_Offset[3]).ToString("0.000")};
                    break;
                case Enum.Log_Type.CW_Log:
                    Data = new object[] { Now.ToString("HH:mm:ss") , Tact ,
                        (Unit.Type[(int)Current_Type].P_Result_Y).ToString("0.000") , (-Unit.Type[(int)Current_Type].P_Result_X).ToString("0.000") , (Unit.Type[(int)Current_Type].P_Result_T).ToString("0.000") ,
                        (Unit.Type[(int)Current_Type].L_Result_Y).ToString("0.000") , (-Unit.Type[(int)Current_Type].L_Result_X).ToString("0.000") , (Unit.Type[(int)Current_Type].L_Result_T).ToString("0.000") ,
                        (P_Offset_Position_X).ToString("0.000") , (P_Offset_Position_Y).ToString("0.000") , (P_Offset_Position_T).ToString("0.000") ,
                        (L_Offset_Position_X).ToString("0.000") , (L_Offset_Position_Y).ToString("0.000") , (L_Offset_Position_T).ToString("0.000") ,
                        ((Unit.Type[0].L_Check[0] / 1000) + Unit.Type[0].L_Check_Offset[0]).ToString("0.000") ,((Unit.Type[0].L_Check[1] / 1000) + Unit.Type[0].L_Check_Offset[1]).ToString("0.000") ,((Unit.Type[0].L_Check[2] / 1000) + Unit.Type[0].L_Check_Offset[2]).ToString("0.000") ,((Unit.Type[0].L_Check[3] / 1000) + Unit.Type[0].L_Check_Offset[3]).ToString("0.000")};
                    break;
            }
            Unit.Data_Display(Data);
            if (ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.DG_DA || ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.DG_DA_TO_4POSITION)
            {
                Unit_Log.Picture_Save((int)Current_Unit, (int)Enum.Type.Target, Now, Cell_ID);
                Unit_Log.Picture_Save((int)Current_Unit, (int)Enum.Type.Object, Now, Cell_ID);
            }
            else
                Unit_Log.Picture_Save((int)Current_Unit, (int)Current_Type, Now, Cell_ID);
            Unit_Log.UI_Capture((int)Current_Unit, (int)Current_Type, Now, Cell_ID);
            Re_Align_Count++;
        }


        private void Inspection_Data_Save()  //데이터 세이브 항목들 수정해야함
        {
            DateTime Now = DateTime.Now;
            object[] Data1;

            Data1 = new object[] { Now.ToString("HH:mm:ss") ,Cell_ID, Tact ,
            Unit.Distance_X[0].ToString("0.000"),Unit.Distance_Y[0].ToString("0.000"),
            Unit.Distance_X[1].ToString("0.000"),Unit.Distance_Y[1].ToString("0.000"),
            Unit.Distance_X[2].ToString("0.000"),Unit.Distance_Y[2].ToString("0.000"),
            Unit.Distance_X[3].ToString("0.000"),Unit.Distance_Y[3].ToString("0.000")};
            object[] Data2 = new object[48];
            for (int LoopCount = 0; LoopCount < 20; LoopCount = LoopCount + 5)
            {
                Data2[LoopCount] = Unit.Type[0].Mark[LoopCount / 5].Last_Mark_Index.ToString();
                Data2[LoopCount + 1] = Unit.Type[0].Mark[LoopCount / 5].X.ToString("0.000");
                Data2[LoopCount + 2] = Unit.Type[0].Mark[LoopCount / 5].Y.ToString("0.000");
                Data2[LoopCount + 3] = Unit.Type[0].Mark[LoopCount / 5].S.ToString("0.000");
                Data2[LoopCount + 4] = Unit.Type[0].Mark[LoopCount / 5].AR.ToString("0.000");
            }
            for (int LoopCount = 20; LoopCount < 40; LoopCount = LoopCount + 5)
            {
                Data2[LoopCount] = Unit.Type[2].Mark[LoopCount / 5 - 4].Last_Mark_Index.ToString();
                Data2[LoopCount + 1] = Unit.Type[2].Mark[LoopCount / 5 - 4].X.ToString("0.000");
                Data2[LoopCount + 2] = Unit.Type[2].Mark[LoopCount / 5 - 4].Y.ToString("0.000");
                Data2[LoopCount + 3] = Unit.Type[2].Mark[LoopCount / 5 - 4].S.ToString("0.000");
                Data2[LoopCount + 4] = Unit.Type[2].Mark[LoopCount / 5 - 4].AR.ToString("0.000");
            }
            for (int LoopCount = 40; LoopCount < 44; LoopCount++)
            {
                Data2[LoopCount] = ((Unit.Type[0].L_Check[LoopCount % 4] / 1000) + Unit.Type[0].L_Check_Offset[LoopCount % 4]).ToString("0.000");
            }
            for (int LoopCount = 44; LoopCount < 48; LoopCount++)
            {
                Data2[LoopCount] = ((Unit.Type[2].L_Check[LoopCount % 4] / 1000) + Unit.Type[2].L_Check_Offset[LoopCount % 4]).ToString("0.000");
            }


            var List = new List<object>();
            List.AddRange(Data1);
            List.AddRange(Data2);
            object[] Data = List.ToArray();
            Unit_Log.Inspection_Data_Write(Current_Unit, Data);
            Data = new object[] { Now.ToString("HH:mm:ss") , Tact ,
            Unit.Distance_X[0].ToString("0.000") ,Unit.Distance_Y[0].ToString("0.000") ,
            Unit.Distance_X[1].ToString("0.000") ,Unit.Distance_Y[1].ToString("0.000") ,
            Unit.Distance_X[2].ToString("0.000") ,Unit.Distance_Y[2].ToString("0.000") ,
            Unit.Distance_X[3].ToString("0.000") ,Unit.Distance_Y[3].ToString("0.000") ,
            ((Unit.Type[0].L_Check[0] / 1000) + Unit.Type[0].L_Check_Offset[0]).ToString("0.000") ,((Unit.Type[0].L_Check[1] / 1000) + Unit.Type[0].L_Check_Offset[1]).ToString("0.000") ,((Unit.Type[0].L_Check[2] / 1000) + Unit.Type[0].L_Check_Offset[2]).ToString("0.000") ,((Unit.Type[0].L_Check[3] / 1000) + Unit.Type[0].L_Check_Offset[3]).ToString("0.000"),
            ((Unit.Type[2].L_Check[0] / 1000) + Unit.Type[2].L_Check_Offset[0]).ToString("0.000") ,((Unit.Type[2].L_Check[1] / 1000) + Unit.Type[2].L_Check_Offset[1]).ToString("0.000") ,((Unit.Type[2].L_Check[2] / 1000) + Unit.Type[2].L_Check_Offset[2]).ToString("0.000") ,((Unit.Type[2].L_Check[3] / 1000) + Unit.Type[2].L_Check_Offset[3]).ToString("0.000")};
            Unit.Inspection_Data_Display(Data);
            Unit_Log.Picture_Save((int)Current_Unit, (int)Enum.Type.Target, Now, Cell_ID);
            Unit_Log.Picture_Save((int)Current_Unit, (int)Enum.Type.Object, Now, Cell_ID);
            Unit_Log.UI_Capture((int)Current_Unit, (int)Current_Type, Now, Cell_ID);
            
        }

        private void UVW_Moveing(double _X, double _Y, double _T)           //일반 무빙
        {
            if ((ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.SG_DA_UNIT && ETC_Option.DA_UNIT[(int)Current_Unit] == 0) ||
                ETC_Option.Unit_Type[(int)Current_Unit] == Enum.Unit_Type.SG_DA_TO && Current_Type == Enum.Type.Target)
            {
                _X = 0; _Y = 0; _T = 0;
            }
            double X = _X * 10000;                                          // mm를 um로 바꾸기 위하여 *10000
            double Y = _Y * 10000;
            double T = UVW_Theta(_T) * 10000;        //Theta 값을 회전 중심부터 R에 맞게 값 읽어오기
            double _Motor_T = _T * 10000;
            if (ETC_Option.Motor_Type[(int)Current_Unit] == Enum.Motor_Type.UVW_4_PMMP)
            {
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 10 + ETC_Option.Interface_Offset] = (int)(X + T) + (int)Calibration_Orizin[0];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 11 + ETC_Option.Interface_Offset] = ((int)(X + T) >> 16) + ((int)Calibration_Orizin[0] >> 16); // 보정 Data를 16비트씩 보내기 위해 상위 Data Shift 
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 12 + ETC_Option.Interface_Offset] = (int)(X - T) + (int)Calibration_Orizin[1];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 13 + ETC_Option.Interface_Offset] = ((int)(X - T) >> 16) + ((int)Calibration_Orizin[1] >> 16);
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 14 + ETC_Option.Interface_Offset] = (int)(Y - T) + (int)Calibration_Orizin[2];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 15 + ETC_Option.Interface_Offset] = ((int)(Y - T) >> 16) + ((int)Calibration_Orizin[2] >> 16);
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 16 + ETC_Option.Interface_Offset] = (int)(Y + T) + (int)Calibration_Orizin[3];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 17 + ETC_Option.Interface_Offset] = ((int)(Y + T) >> 16) + ((int)Calibration_Orizin[3] >> 16);

                //().ToString("0.000"),.ToString("0.000"),.ToString("0.000")
#if CW_SAM
                double Unloading_X = (Align_Position_X - Keep_Data_X) * 10000;
                double Unloading_Y = (Align_Position_Y - Keep_Data_Y) * 10000;
                double Unloading_T = UVW_Theta(Align_Position_T - Keep_Data_T) * 10000;
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 32 + ETC_Option.Interface_Offset] = (int)(Unloading_X + Unloading_T) + (int)Calibration_Orizin[0];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 33 + ETC_Option.Interface_Offset] = ((int)(Unloading_X + Unloading_T) >> 16) + ((int)Calibration_Orizin[0] >> 16); // 보정 Data를 16비트씩 보내기 위해 상위 Data Shift 
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 34 + ETC_Option.Interface_Offset] = (int)(Unloading_X - Unloading_T) + (int)Calibration_Orizin[1];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 35 + ETC_Option.Interface_Offset] = ((int)(Unloading_X - Unloading_T) >> 16) + ((int)Calibration_Orizin[1] >> 16);
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 36 + ETC_Option.Interface_Offset] = (int)(Unloading_Y - Unloading_T) + (int)Calibration_Orizin[2];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 37 + ETC_Option.Interface_Offset] = ((int)(Unloading_Y - Unloading_T) >> 16) + ((int)Calibration_Orizin[2] >> 16);
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 38 + ETC_Option.Interface_Offset] = (int)(Unloading_Y + Unloading_T) + (int)Calibration_Orizin[3];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 39 + ETC_Option.Interface_Offset] = ((int)(Unloading_Y + Unloading_T) >> 16) + ((int)Calibration_Orizin[3] >> 16);
#endif
            }
            else if (ETC_Option.Motor_Type[(int)Current_Unit] == Enum.Motor_Type.UVW_4)
            {
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 10 + ETC_Option.Interface_Offset] = (int)(X + T) + (int)Calibration_Orizin[0];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 11 + ETC_Option.Interface_Offset] = ((int)(X + T) >> 16) + ((int)Calibration_Orizin[0] >> 16); // 보정 Data를 16비트씩 보내기 위해 상위 Data Shift 
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 12 + ETC_Option.Interface_Offset] = (int)(X - T) + (int)Calibration_Orizin[1];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 13 + ETC_Option.Interface_Offset] = ((int)(X - T) >> 16) + ((int)Calibration_Orizin[1] >> 16);
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 14 + ETC_Option.Interface_Offset] = (int)(Y + T) + (int)Calibration_Orizin[2];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 15 + ETC_Option.Interface_Offset] = ((int)(Y + T) >> 16) + ((int)Calibration_Orizin[2] >> 16);
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 16 + ETC_Option.Interface_Offset] = (int)(Y - T) + (int)Calibration_Orizin[3];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 17 + ETC_Option.Interface_Offset] = ((int)(Y - T) >> 16) + ((int)Calibration_Orizin[3] >> 16);
            }
            else if (ETC_Option.Motor_Type[(int)Current_Unit] == Enum.Motor_Type.UVW_3)
            {
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 10 + ETC_Option.Interface_Offset] = (int)(X + T) + (int)Calibration_Orizin[0];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 11 + ETC_Option.Interface_Offset] = ((int)(X + T) >> 16) + ((int)Calibration_Orizin[0] >> 16); // 보정 Data를 16비트씩 보내기 위해 상위 Data Shift 
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 14 + ETC_Option.Interface_Offset] = (int)(Y - T) + (int)Calibration_Orizin[2];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 15 + ETC_Option.Interface_Offset] = ((int)(Y - T) >> 16) + ((int)Calibration_Orizin[2] >> 16);
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 16 + ETC_Option.Interface_Offset] = (int)(Y + T) + (int)Calibration_Orizin[3];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 17 + ETC_Option.Interface_Offset] = ((int)(Y + T) >> 16) + ((int)Calibration_Orizin[3] >> 16);
            }
            else if (ETC_Option.Motor_Type[(int)Current_Unit] == Enum.Motor_Type.XYT)
            {
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 10 + ETC_Option.Interface_Offset] = (int)(X) + (int)Calibration_Orizin[0];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 11 + ETC_Option.Interface_Offset] = ((int)(X) >> 16) + ((int)Calibration_Orizin[0] >> 16); // 보정 Data를 16비트씩 보내기 위해 상위 Data Shift 
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 12 + ETC_Option.Interface_Offset] = (int)(Y) + (int)Calibration_Orizin[2];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 13 + ETC_Option.Interface_Offset] = ((int)(Y) >> 16) + ((int)Calibration_Orizin[2] >> 16);
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 14 + ETC_Option.Interface_Offset] = (int)(_Motor_T) + (int)Calibration_Orizin[3];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 15 + ETC_Option.Interface_Offset] = ((int)(_Motor_T) >> 16) + ((int)Calibration_Orizin[3] >> 16);
            }
            else if (ETC_Option.Motor_Type[(int)Current_Unit] == Enum.Motor_Type.YXT)
            {
                double calue_Y = (Y) + (int)Calibration_Orizin[0];
                double calue_T = (_Motor_T) + (int)Calibration_Orizin[2];

                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 12 + ETC_Option.Interface_Offset] = (int)(X) + (int)Calibration_Orizin[1];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 13 + ETC_Option.Interface_Offset] = ((int)(X) >> 16) + ((int)Calibration_Orizin[1] >> 16); // 보정 Data를 16비트씩 보내기 위해 상위 Data Shift 
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 10 + ETC_Option.Interface_Offset] = (int)calue_Y;
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 11 + ETC_Option.Interface_Offset] = ((int)calue_Y >> 16);
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 14 + ETC_Option.Interface_Offset] = (int)calue_T;
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 15 + ETC_Option.Interface_Offset] = ((int)calue_T >> 16);
            }
#if CW_SAM
            if ((Current_Unit == Enum.Unit.Unit2 || Current_Unit == Enum.Unit.Unit3) && Sequences == Enum.Sequence.Align)
            {
                double PX = (Unit.Type[(int)Current_Type].P_Result_X + P_Offset_Position_Y) * -10000;
                double PY = (Unit.Type[(int)Current_Type].P_Result_Y + P_Offset_Position_X) * 10000;
                double P_Motor_T = (Unit.Type[(int)Current_Type].P_Result_T + P_Offset_Position_T) * 10000;

                double LX = (Unit.Type[(int)Current_Type].L_Result_X + L_Offset_Position_Y) * -10000;
                double LY = (Unit.Type[(int)Current_Type].L_Result_Y + L_Offset_Position_X) * 10000;
                double L_Motor_T = (Unit.Type[(int)Current_Type].L_Result_T + L_Offset_Position_T) * 10000;

                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 12 + ETC_Option.Interface_Offset] = (int)(PX) + (int)Calibration_Orizin[1];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 13 + ETC_Option.Interface_Offset] = ((int)(PX) >> 16) + ((int)Calibration_Orizin[1] >> 16); // 보정 Data를 16비트씩 보내기 위해 상위 Data Shift 
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 10 + ETC_Option.Interface_Offset] = (int)(PY) + (int)Calibration_Orizin[0];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 11 + ETC_Option.Interface_Offset] = ((int)(PY) >> 16) + ((int)Calibration_Orizin[0] >> 16);
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 14 + ETC_Option.Interface_Offset] = (int)(P_Motor_T) + (int)Calibration_Orizin[2];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 15 + ETC_Option.Interface_Offset] = ((int)(P_Motor_T) >> 16) + ((int)Calibration_Orizin[2] >> 16);

                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 20 + ETC_Option.Interface_Offset] = (int)(X) + (int)Calibration_Orizin[1];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 21 + ETC_Option.Interface_Offset] = ((int)(X) >> 16) + ((int)Calibration_Orizin[1] >> 16); // 보정 Data를 16비트씩 보내기 위해 상위 Data Shift 
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 18 + ETC_Option.Interface_Offset] = (int)(Y) + (int)Calibration_Orizin[0];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 19 + ETC_Option.Interface_Offset] = ((int)(Y) >> 16) + ((int)Calibration_Orizin[0] >> 16);
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 22 + ETC_Option.Interface_Offset] = (int)(_Motor_T) + (int)Calibration_Orizin[2];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 23 + ETC_Option.Interface_Offset] = ((int)(_Motor_T) >> 16) + ((int)Calibration_Orizin[2] >> 16);

                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 26 + ETC_Option.Interface_Offset] = (int)(LX) + (int)Calibration_Orizin[1];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 27 + ETC_Option.Interface_Offset] = ((int)(LX) >> 16) + ((int)Calibration_Orizin[1] >> 16); // 보정 Data를 16비트씩 보내기 위해 상위 Data Shift 
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 24 + ETC_Option.Interface_Offset] = (int)(LY) + (int)Calibration_Orizin[0];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 25 + ETC_Option.Interface_Offset] = ((int)(LY) >> 16) + ((int)Calibration_Orizin[0] >> 16);
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 28 + ETC_Option.Interface_Offset] = (int)(L_Motor_T) + (int)Calibration_Orizin[2];
                COMMON.Send_Data[((int)Current_Unit - 1) * 30 + 29 + ETC_Option.Interface_Offset] = ((int)(L_Motor_T) >> 16) + ((int)Calibration_Orizin[2] >> 16);
            }
#endif
            COMMON.Write_Bit = true;
        }

        private double UVW_Theta(double _theta)
        {
            return (ETC_Option.UVW_R[(int)Current_Unit] * Math.Cos(Deg_To_Radians(_theta + 270 + 0))) - (ETC_Option.UVW_R[(int)Current_Unit] * Math.Cos(Deg_To_Radians(270 + 0)));
        }

        private double Deg_To_Radians(double Deg)
        {
            return Deg * Math.PI / 180;
        }

        private double Calibration_Cam_Pitch(double object_Length, Enum.Position _Position)
        {
            double X1, X2, Y1, Y2;
            double distance1, distance2, distance_Y;
            double Theta;
            object_Length = object_Length * 1000;
            X1 = Unit.Type[(int)Current_Type].Mark[0 + (int)_Position].Master_X;
            Y1 = Unit.Type[(int)Current_Type].Mark[0 + (int)_Position].Master_Y;
            X2 = Unit.Type[(int)Current_Type].Mark[1 + (int)_Position].Master_X;
            Y2 = Unit.Type[(int)Current_Type].Mark[1 + (int)_Position].Master_Y;
            distance1 = (Unit.Type[(int)Current_Type].Mark[0 + (int)_Position].Last_Grab_Image.Width - X1) * Unit.Type[(int)Current_Type].Mark[0 + (int)_Position].Resolution_X;
            distance2 = X2 * Unit.Type[(int)Current_Type].Mark[1 + (int)_Position].Resolution_X;
            distance_Y = (Y1 - Y2) * ((Unit.Type[(int)Current_Type].Mark[0 + (int)_Position].Resolution_Y + Unit.Type[(int)Current_Type].Mark[1 + (int)_Position].Resolution_Y) / 2);
            Theta = Math.Asin(distance_Y / object_Length);
            double t = object_Length * Math.Cos(Theta);
            return object_Length * Math.Cos(Theta) - distance1 - distance2;
        }

        private double Shuttle_Pitch(double Resolution1, double Resolution2,int Cam_Height)
        {
            double Resolution = (Resolution1 + Resolution2) / 2;
            double Distance = Math.Abs(Math.Abs(Unit.Type[(int)Current_Type].Shuttle_Y[0]) - Math.Abs(Unit.Type[(int)Current_Type].Shuttle_Y[2])) - (Cam_Height * Resolution);
            return Distance;
        }

        private double Calibration_Calculation_Theta(double Cam_Distance, Enum.Position _Position)
        {
            double[] Theta = new double[7];
            double[] Pitch_Theta = new double[6];
            for (int LoopCount = 0; LoopCount < 7; LoopCount++)
            {
                double X1, X2, Y;
                X1 = (Unit.Type[(int)Current_Type].Mark[0 + (int)_Position].Last_Grab_Image.Width - Unit.Type[(int)Current_Type].Mark[0 + (int)_Position].Calibration_Point_X[9 + LoopCount]) * Unit.Type[(int)Current_Type].Mark[0 + (int)_Position].Resolution_X;
                X2 = Unit.Type[(int)Current_Type].Mark[1 + (int)_Position].Calibration_Point_X[9 + LoopCount] * Unit.Type[(int)Current_Type].Mark[1 + (int)_Position].Resolution_X;
                Y = (Unit.Type[(int)Current_Type].Mark[0 + (int)_Position].Calibration_Point_Y[9 + LoopCount] - Unit.Type[(int)Current_Type].Mark[1 + (int)_Position].Calibration_Point_Y[9 + LoopCount]) * ((Unit.Type[(int)Current_Type].Mark[0 + (int)_Position].Resolution_Y + Unit.Type[(int)Current_Type].Mark[1 + (int)_Position].Resolution_Y) / 2);
                Theta[LoopCount] = Math.Atan(Y / (X1 + X2 + Cam_Distance)) * 180 / Math.PI;
            }
            for (int LoopCount = 0; LoopCount < 6; LoopCount++)
            {
                Pitch_Theta[LoopCount] = Math.Abs(Theta[LoopCount] - Theta[LoopCount + 1]);
            }
            return Pitch_Theta.Average() / 2;
        }

        private double Calibration_Calculation_Theta(double Shuttle_DIstance, int LR)
        {
            double[] Theta = new double[7];
            double[] Pitch_Theta = new double[6];
            for (int LoopCount = 0; LoopCount < 7; LoopCount++)
            {
                double X, Y1, Y2;
                Y1 = (Unit.Type[(int)Current_Type].Mark[0 + LR].Last_Grab_Image.Height - Unit.Type[(int)Current_Type].Mark[0 + LR].Calibration_Point_Y[9 + LoopCount]) * Unit.Type[(int)Current_Type].Mark[0 + LR].Resolution_Y;
                Y2 = Unit.Type[(int)Current_Type].Mark[2 + LR].Calibration_Point_Y[9 + LoopCount] * Unit.Type[(int)Current_Type].Mark[2+ LR].Resolution_Y;
                X = (Unit.Type[(int)Current_Type].Mark[0 + LR].Calibration_Point_X[9 + LoopCount] - Unit.Type[(int)Current_Type].Mark[2 + LR].Calibration_Point_X[9 + LoopCount]) * ((Unit.Type[(int)Current_Type].Mark[0 + LR].Resolution_X + Unit.Type[(int)Current_Type].Mark[1 + LR].Resolution_X) / 2);
                Theta[LoopCount] = Math.Atan((Y1+Y2+ Shuttle_DIstance) / X) * 180 / Math.PI;
            }
            for (int LoopCount = 0; LoopCount < 6; LoopCount++)
            {
                Pitch_Theta[LoopCount] = Math.Abs(Theta[LoopCount] - Theta[LoopCount + 1]);
            }
            return Pitch_Theta.Average() / 2;
        }

        private void Mark_Index_Reset(Enum.Type _Type)
        {
            for(int LoopCount=0;LoopCount<6;LoopCount++)
            {
                if (COMMON.Frame.Unit[(int)Current_Unit].Type[(int)_Type].Mark[LoopCount] != null)
                    COMMON.Frame.Unit[(int)Current_Unit].Type[(int)_Type].Mark[LoopCount].Last_Mark_Index = 0;
            }
        }
    }
}
