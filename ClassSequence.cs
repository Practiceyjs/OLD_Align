using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace Align
{
    public class ClassSequence
    {
        double Re_Align_Limit = 0.011;

        #region U_Lami
        //Hex
        //2Byte / WORD
        const int PLC_READY = 0x0001; //1 -> 0
        const int PLC_ERROR = 0x0002; //2 -> 1
        const int PLC_RESET = 0x0004; //4 -> 2
        const int PLC_1St_Ack = 0x0008; //8 -> 3
        const int PLC_Cal_Start = 0x0010; //16 -> 4
        const int PLC_Cal_End = 0x0020; //32 -> 5
        const int PLC_Align_Start = 0x0040; //64 -> 6
        const int PLC_Align_End = 0x0080; //128 -> 7
        const int PLC_ReAlign = 0x0100; //256 -> 8
        const int PLC_OCA_WINDOW = 0x0200; //512 -> 9
        //const int = 0x0400; //1024 -> A
        const int PLC_2nd_Ack = 0x0800; //2048 -> B
        const int PLC_Move_Ack = 0x1000; //4096 -> C
        const int PLC_Move_Done_Req = 0x2000; //8192 -> D
        const int PLC_Inspection_Start = 0x4000; //E
        
        const int PC_OFF = 0x0000;
        const int PC_AUTO = 0x0002;

        const int PC_READY = 0x0001; //1-> 0
        const int PC_ERROR = 0x0002; //2 -> 1
        const int PC_RESET = 0x0004; //4 -> 2
        const int PC_1St_Req = 0x0008; //8 -> 3
        const int PC_Cal_Start = 0x0010; //16 -> 4
        const int PC_Cal_End = 0x0020; // 32 -> 5
        const int PC_Align_Start = 0x0040; //64 -> 6
        const int PC_Align_End = 0x0080; //128 -> 7
        const int PC_Align_OK = 0x0100; //256 -> 8
        const int PC_Align_NG = 0x0200; //512 -> 9
        //const int = 0x0400; //1024 -> A
        const int PC_2nd_Req = 0x0800; // 20248 -> B
        const int PC_Move_Req = 0x1000; // 4096 -> C
        const int PC_Move_Done_Ack = 0x2000; //8192 -> D
        const int PC_Inspection_OK = 0x4000; //16384 -> E
        const int PC_Inspection_NG = 0x8000; //32768 -> F

        #endregion

        #region Bending
        //Hex
        //2Byte / WORD
        //const int PLC_READY = 0x0001; //1 -> 0
        //const int PLC_ERROR = 0x0002; //2 -> 1
        //const int PLC_RESET = 0x0004; //4 -> 2
        //const int PLC_1St_Ack = 0x0008; //8 -> 3
        //const int PLC_Cal_Start = 0x0010; //16 -> 4
        //const int PLC_Cal_End = 0x0020; //32 -> 5
        //const int PLC_Align_Start = 0x0040; //64 -> 6
        //const int PLC_Align_End = 0x0080; //128 -> 7
        //const int PLC_ReAlign = 0x0100; //256 -> 8
        //const int PLC_OCA_WINDOW = 0x0200; //512 -> 9
        //const int Offset_Change_Ack= 0x0400; //1024 -> A
        //const int PLC_2nd_Ack = 0x0800; //2048 -> B
        //const int PLC_Move_Ack = 0x1000; //4096 -> C
        //const int PLC_Move_Done_Req = 0x2000; //8192 -> D
        //const int PLC_Inspection_Start = 0x4000; //E

        //const int PC_OFF = 0x0000;
        //const int PC_AUTO = 0x0002;

        //const int PC_READY = 0x0001;
        //const int PC_ERROR = 0x0002;
        //const int PC_RESET = 0x0004;
        //const int PC_1St_Req = 0x0008;
        //const int PC_Cal_Start = 0x0010;
        //const int PC_Cal_End = 0x0020;
        //const int PC_Align_Start = 0x0040;
        //const int PC_Align_End = 0x0080;
        //const int PC_Align_OK = 0x0100;
        //const int PC_Align_NG = 0x0200;
        //const int Mark_NG_Out = 0x0400; //1024 -> A
        //const int PC_2nd_Req = 0x0800;
        //const int PC_Move_Req = 0x1000;
        //const int PC_Move_Done_Ack = 0x2000;
        //const int PC_Inspection_OK = 0x4000;
        //const int PC_Inspection_NG = 0x8000;

        #endregion

        #region Cu_Foam
        //Hex
        //2Byte / WORD
        //const int PLC_READY = 0x0001; //1 -> 0
        //const int PLC_ERROR = 0x0002; //2 -> 1
        //const int PLC_RESET = 0x0004; //4 -> 2
        //const int PLC_1St_Ack = 0x0008; //8 -> 3
        //const int PLC_Cal_Start = 0x0010; //16 -> 4
        //const int PLC_Cal_End = 0x0020; //32 -> 5
        //const int PLC_Align_Start = 0x0040; //64 -> 6
        //const int PLC_Align_End = 0x0080; //128 -> 7
        //const int PLC_ReAlign = 0x0100; //256 -> 8
        //const int PLC_OCA_WINDOW = 0x0200; //512 -> 9
        //const int Offset_Change_Ack= 0x0400; //1024 -> A
        //const int PLC_2nd_Ack = 0x0800; //2048 -> B
        //const int PLC_Move_Ack = 0x1000; //4096 -> C
        //const int PLC_Move_Done_Req = 0x2000; //8192 -> D
        //const int PLC_Inspection_Start = 0x4000; //E

        //const int PC_OFF = 0x0000;
        //const int PC_AUTO = 0x0002;

        //const int PC_READY = 0x0001;
        //const int PC_ERROR = 0x0002;
        //const int PC_RESET = 0x0004;
        //const int PC_1St_Req = 0x0008;
        //const int PC_Cal_Start = 0x0010;
        //const int PC_Cal_End = 0x0020;
        //const int PC_Align_Start = 0x0040;
        //const int PC_Align_End = 0x0080;
        //const int PC_Align_OK = 0x0100;
        //const int PC_Align_NG = 0x0200;
        //const int Mark_NG_Out = 0x0400; //1024 -> A
        //const int PC_2nd_Req = 0x0800;
        //const int PC_Move_Req = 0x1000;
        //const int PC_Move_Done_Ack = 0x2000;
        //const int PC_Inspection_OK = 0x4000;
        //const int PC_Inspection_NG = 0x8000;

        #endregion

        Main_Frame Frame;
        ClassUnit[] Unit;

        public ClassLog ClassLog = new ClassLog();
        public ClassEnum.Sequence Sequences;
        public ClassEnum.Align_Sequence Align_Sequence;
        public ClassEnum.Calibration_Sequence Calibration_Sequence;
        public ClassEnum.Inspection_Sequence Inspection_Sequence;
        public ClassEnum.Auto_Mode Auto_Mode = ClassEnum.Auto_Mode.MANUAL;

        ClassEnum.Type Cul_Type = ClassEnum.Type.Target;
        ClassType[] Type;

        int[] Send_Data;

        int Cal_Position = 0;

        double Align_Position_X;
        double Align_Position_Y;
        double Align_Position_T;
        double Offset_Position_X;
        double Offset_Position_Y;
        double Offset_Position_T;

        int Recipe_No = 0;

        string Cell_ID = "";

        double[] Cal_Position_Data_X;
        double[] Cal_Position_Data_Y;
        double[] Cal_Position_Data_T;

        public bool Re_Align_2_1 = false;
        bool gError;

        DateTime ST;

        int Unit_Num;

        //28 Point Cal_Position
        public void Pitch_Set()
        {
            Cal_Position_Data_X = new double[28] {  -Type[(int)Cul_Type].Cal_Move_Pitch_X * 2, -Type[(int)Cul_Type].Cal_Move_Pitch_X, 0, Type[(int)Cul_Type].Cal_Move_Pitch_X, Type[(int)Cul_Type].Cal_Move_Pitch_X * 2,
                                                    -Type[(int)Cul_Type].Cal_Move_Pitch_X * 2, -Type[(int)Cul_Type].Cal_Move_Pitch_X, 0, Type[(int)Cul_Type].Cal_Move_Pitch_X, Type[(int)Cul_Type].Cal_Move_Pitch_X * 2,
                                                    -Type[(int)Cul_Type].Cal_Move_Pitch_X * 2, -Type[(int)Cul_Type].Cal_Move_Pitch_X, 0, Type[(int)Cul_Type].Cal_Move_Pitch_X, Type[(int)Cul_Type].Cal_Move_Pitch_X * 2,
                                                    -Type[(int)Cul_Type].Cal_Move_Pitch_X * 2, -Type[(int)Cul_Type].Cal_Move_Pitch_X, 0, Type[(int)Cul_Type].Cal_Move_Pitch_X, Type[(int)Cul_Type].Cal_Move_Pitch_X * 2,
                                                    -Type[(int)Cul_Type].Cal_Move_Pitch_X * 2, -Type[(int)Cul_Type].Cal_Move_Pitch_X, 0, Type[(int)Cul_Type].Cal_Move_Pitch_X, Type[(int)Cul_Type].Cal_Move_Pitch_X * 2,
                                                    0, 0, 0 };
            Cal_Position_Data_Y = new double[28] {  -Type[(int)Cul_Type].Cal_Move_Pitch_Y * 2, -Type[(int)Cul_Type].Cal_Move_Pitch_Y * 2, -Type[(int)Cul_Type].Cal_Move_Pitch_Y * 2, -Type[(int)Cul_Type].Cal_Move_Pitch_Y * 2, -Type[(int)Cul_Type].Cal_Move_Pitch_Y * 2,
                                                    -Type[(int)Cul_Type].Cal_Move_Pitch_Y, -Type[(int)Cul_Type].Cal_Move_Pitch_Y, -Type[(int)Cul_Type].Cal_Move_Pitch_Y, -Type[(int)Cul_Type].Cal_Move_Pitch_Y, -Type[(int)Cul_Type].Cal_Move_Pitch_Y,
                                                    0, 0, 0, 0, 0,
                                                    Type[(int)Cul_Type].Cal_Move_Pitch_Y, Type[(int)Cul_Type].Cal_Move_Pitch_Y, Type[(int)Cul_Type].Cal_Move_Pitch_Y, Type[(int)Cul_Type].Cal_Move_Pitch_Y, Type[(int)Cul_Type].Cal_Move_Pitch_Y,
                                                    Type[(int)Cul_Type].Cal_Move_Pitch_Y * 2, Type[(int)Cul_Type].Cal_Move_Pitch_Y * 2, Type[(int)Cul_Type].Cal_Move_Pitch_Y * 2, Type[(int)Cul_Type].Cal_Move_Pitch_Y * 2, Type[(int)Cul_Type].Cal_Move_Pitch_Y * 2,
                                                    0, 0, 0 };
            Cal_Position_Data_T = new double[28] {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                    -Type[(int)Cul_Type].Cal_Move_Pitch_T, 0, Type[(int)Cul_Type].Cal_Move_Pitch_T };
        }

        public void Sequence_Set(Main_Frame _Frame, int _Unit_Num)
        {
            Frame = _Frame;
            Unit_Num = _Unit_Num;
            Align_Position_X = 0;
            Align_Position_Y = 0;
            Align_Position_T = 0;
            Offset_Position_X = 0;
            Offset_Position_Y = 0;
            Offset_Position_T = 0;
            Send_Data = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            gError = false;
            Sequences = ClassEnum.Sequence.Ready;
            Align_Sequence = ClassEnum.Align_Sequence.Ready;
            Calibration_Sequence = ClassEnum.Calibration_Sequence.Ready;
            Inspection_Sequence = ClassEnum.Inspection_Sequence.Ready;
            ClassLog.Log_Set(Frame);
        }
        public void Loading()
        {
            Unit = Frame.Unit;
            Type = Unit[Unit_Num].Type;
            Pitch_Set();
            ClassINI.Read_Calibration_Data(Unit_Num, out Type[(int)Cul_Type].Cal_Data_MARK1, out Type[(int)Cul_Type].Cal_Data_MARK2, ClassEnum.Position.First);
            ClassINI.Read_Calibration_Data(Unit_Num, out Type[(int)Cul_Type].Cal_Data_MARK3, out Type[(int)Cul_Type].Cal_Data_MARK4, ClassEnum.Position.Second);
        }

        public void Auto_Condition(ClassEnum.Auto_Mode _Auto_Mode)
        {
            Auto_Mode = _Auto_Mode;
            if (Auto_Mode.Equals(ClassEnum.Auto_Mode.AUTO))
            {
                Write_On_Bit(0, PC_AUTO);
                for (int i = 1; i <= Frame.gMax_Unit; i++)
                    Write_On_Bit(i, PC_READY);
            }
            else
            {
                Write_On_Bit(0, PC_OFF);
                for (int i = 1; i <= Frame.gMax_Unit; i++)
                    Write_On_Bit(i, PC_OFF);
            }
        }

        public void Unit_Sequence(int[] _Read_Data, int[] _Send_Data)
        {
            if (Auto_Mode.Equals(ClassEnum.Auto_Mode.AUTO))
            {
                int Read_Data = _Read_Data[Unit_Num];
                if ((Read_Data & PLC_RESET).Equals(PLC_RESET))                          // 해당 유닛 리셋
                {
                    Sequences = ClassEnum.Sequence.Ready;
                    Align_Sequence = ClassEnum.Align_Sequence.Ready;
                    Unit_Reset(Unit_Num);
                }
                if ((Read_Data & PLC_Align_Start).Equals(PLC_Align_Start) && Align_Sequence.Equals(ClassEnum.Align_Sequence.Ready))                  // 얼라인 신호시
                {                    
                    ST = DateTime.Now;
                    //타입 나뉘는 곳
                    if ((Read_Data & PLC_OCA_WINDOW).Equals(PLC_OCA_WINDOW))
                        Cul_Type = ClassEnum.Type.Object;
                    else
                        Cul_Type = ClassEnum.Type.Target;
                    //Unit[Unit_Num].Type[(int)Cul_Type].Light_ON();
                    Sequences = ClassEnum.Sequence.Align;
                    Align_Sequence = ClassEnum.Align_Sequence.Align_Start_Req;
                    ClassLog.Sequence_Log_Write(Unit_Num, Align_Sequence);
                    Frame.Label_Color(Color.Blue, Unit_Num, Cul_Type);
                }
                else if ((Read_Data & PLC_Cal_Start).Equals(PLC_Cal_Start) && Calibration_Sequence.Equals(ClassEnum.Calibration_Sequence.Ready))     // 캘리브레이션 신호시
                {
                    if ((Read_Data & PLC_OCA_WINDOW).Equals(PLC_OCA_WINDOW))
                        Cul_Type = ClassEnum.Type.Object;
                    else
                        Cul_Type = ClassEnum.Type.Target;
                    Sequences = ClassEnum.Sequence.Calibration;
                    Calibration_Sequence = ClassEnum.Calibration_Sequence.Calibration_Start_Req;
                }
                if ((Read_Data & PLC_Inspection_Start).Equals(PLC_Inspection_Start) && Inspection_Sequence.Equals(ClassEnum.Inspection_Sequence.Ready))                  // 인스펙션 신호시
                {
                    ClassLog.PLC_Log_Write(Unit_Num, Read_Data);
                    ST = DateTime.Now;
                    //Unit[Unit_Num].Type[(int)Cul_Type].Light_ON();
                    Sequences = ClassEnum.Sequence.Inspection;
                    Inspection_Sequence = ClassEnum.Inspection_Sequence.Inspecion_Start_Req;
                    ClassLog.Inspection_Log_Write(Unit_Num, Inspection_Sequence);
                    Frame.Label_Color(Color.Blue, Unit_Num, Cul_Type);
                }
                if (Sequences.Equals(ClassEnum.Sequence.Align))                      // 얼라인 시퀀스
                    gError = Is_Algin_Sequence(Read_Data);
                else if (Sequences.Equals(ClassEnum.Sequence.Calibration))           // 캘리브레이션 시퀀스
                    gError = Is_Calibration_Sequence(Read_Data);
                else if (Sequences.Equals(ClassEnum.Sequence.Inspection))            //인스펙션 시퀀스
                    gError = Is_Inspection_Sequence(Read_Data);
                if (gError)
                {
                    Write_On_Bit(Unit_Num, PC_ERROR);
                    Sequences = ClassEnum.Sequence.Error;
                }

            }
        }

        private bool Is_Algin_Sequence(int _Read_Data)
        {
            bool mError = false;
            switch (Align_Sequence)
            {
                case ClassEnum.Align_Sequence.Align_Start_Req:
                    Write_On_Bit(Unit_Num, PC_Align_Start);
                    Align_Sequence = ClassEnum.Align_Sequence.Align_Start_Ack;
                    ClassLog.Sequence_Log_Write(Unit_Num, Align_Sequence);
                    break;


                case ClassEnum.Align_Sequence.Align_Start_Ack:
                    //Auto_Grab = 0
                    //21.01.27 Target과 Object Grab 하는 구간으로 변경

                    #region 재검토 필요
                    //if (Frame.Setup_Display.T_O_checkBox.Checked && Frame.Unit[1].Object_Use == true && Unit_Num == 1 && Cul_Type == ClassEnum.Type.Target
                    // || Frame.Setup_Display.T_O_checkBox.Checked && Frame.Unit[2].Object_Use == true && Unit_Num == 2 && Cul_Type == ClassEnum.Type.Target)
                    if(Frame.Setup_Display.T_O_checkBox.Checked && Frame.Unit[2].Object_Use == true && Unit_Num == 2 && Cul_Type == ClassEnum.Type.Target)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            if (i == 0)
                            {
                                Cul_Type = ClassEnum.Type.Target;
                                mError = Type[(int)Cul_Type].Auto_Grab((int)ClassEnum.Position.First);
                            }
                            else
                            {
                                Cul_Type = ClassEnum.Type.Object;
                                mError = Type[(int)Cul_Type].Auto_Grab((int)ClassEnum.Position.First);
                            }
                        }
                    }
                    #endregion
                    //기존
                    //else
                    //{
                    mError = Type[(int)Cul_Type].Auto_Grab((int)ClassEnum.Position.First);
                    //}
                    
                    if (Re_Align_2_1)                   // 리얼라인 2차먼저 할시 시퀀스 맞추기용
                        Align_Sequence = ClassEnum.Align_Sequence.Grab_2nd_OK;
                    break;


                case ClassEnum.Align_Sequence.Grab_1st_OK:
                    ClassLog.Sequence_Log_Write(Unit_Num, Align_Sequence);
                    if (Type[(int)Cul_Type].Second_Grab_Use)
                    {
                        Write_On_Bit(Unit_Num, PC_2nd_Req);
                        Align_Sequence = ClassEnum.Align_Sequence.Move_2nd_Req;
                        ClassLog.Sequence_Log_Write(Unit_Num, Align_Sequence);
                    }
                    else
                        Align_Sequence = ClassEnum.Align_Sequence.Grab_2nd_OK;
                    break;


                case ClassEnum.Align_Sequence.Move_2nd_Req:
                    if ((_Read_Data & PLC_2nd_Ack).Equals(PLC_2nd_Ack))
                    {
                        ClassLog.Sequence_Log_Write(Unit_Num, ClassEnum.Align_Sequence.Move_2nd_Ack);
                        Write_Off_Bit(Unit_Num, PC_2nd_Req);
                        mError = Type[(int)Cul_Type].Auto_Grab((int)ClassEnum.Position.Second);
                    }
                    break;


                case ClassEnum.Align_Sequence.Grab_2nd_OK:
                    ClassLog.Sequence_Log_Write(Unit_Num, Align_Sequence);
                    // 마크 찾기
                    //Unit[Unit_Num].Type[(int)Cul_Type].Light_OFF();

                    //21.01.13
                    //21.01.27 재 수정 실제로 Mark Find에서 Target / Object 구별하도록 도전
                    #region
                    if ((Unit_Num == 1 || Unit_Num == 2) && Frame.Setup_Display.T_O_checkBox.Checked
                        && Frame.Unit[Unit_Num].Object_Use == true && Cul_Type == ClassEnum.Type.Target)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            if (i == 0)
                            {
                                Cul_Type = ClassEnum.Type.Target;
                                Type[(int)Cul_Type].Auto_Mark_FInd();
                            }
                            else
                            {
                                Cul_Type = ClassEnum.Type.Object;
                                Type[(int)Cul_Type].Auto_Mark_FInd();
                            }
                        }
                    }
                    else
                    {
                        //기존
                        Type[(int)Cul_Type].Auto_Mark_FInd();
                    }
                    #endregion

                    //Type[(int)Cul_Type].Auto_Mark_FInd();

                    break;


                case ClassEnum.Align_Sequence.Align:
                    // CAL시 DATA와 비교해 보정 수치 추출
                    if (!Frame.Setup_Display.No_UVW_TEST.Checked)
                    {
                        ClassLog.Sequence_Log_Write(Unit_Num, Align_Sequence);
                        if (Type[(int)Cul_Type].Align_Start(ref Align_Position_X, ref Align_Position_Y, ref Align_Position_T))
                        {
                            ClassLog.Sequence_Log_Write(Unit_Num, Align_Sequence);
                            if (Frame.Setup_Display.Cam_Simul)
                            {
                                Align_Position_X = 0;
                                Align_Position_Y = 0;
                                Align_Position_T = 0;
                            }

                            // UVW 에 추출한 보정치와 Offset값을 합산하여 이동량 전송
                            if (UVW_Moveing(Align_Position_X + Offset_Position_X, Align_Position_Y + Offset_Position_Y, Align_Position_T + Offset_Position_T))
                            {
                                //리밋오버 에러 추가예정
                                Frame.PLC.Write_Command[Unit_Num + 39] = 2;
                                Write_On_Bit(Unit_Num, PC_Align_NG);
                            }
                            else
                            {
                                if (Frame.Setup_Display.L_Check_Use_CheckBox.Checked && L_Check_Start())
                                {
                                    Frame.PLC.Write_Command[Unit_Num + 39] = 3;
                                    Write_On_Bit(Unit_Num, PC_Align_NG);
                                }
                                else
                                {
                                    Align_Sequence = ClassEnum.Align_Sequence.Mark_Serch_OK;
                                    Frame.Dis_Unit_Label[Unit_Num - 1].Label_Color = Color.Lime;
                                    Frame.Label_Color(Color.Lime, Unit_Num, Cul_Type);
                                    if (Frame.Setup_Display.ReAlign_checkBox.Checked)                       // Program 자체 REALIGN (PLC 얼라인이 END를 안쳐요..... 무한루프라서 만듬)
                                                                                                            //if ((Read_Data & PLC_ReAlign).Equals(PLC_ReAlign))                    // PLC REALGIN 사용보고
                                        Re_Align_Sq(Align_Position_X, Align_Position_Y, Align_Position_T);
                                    
                                }
                            }
                        }
                        else
                        {
                            Frame.Label_Color(Color.Red, Unit_Num, Cul_Type);
                            Frame.Dis_Unit_Label[Unit_Num - 1].Label_Color = Color.Red;
                            Frame.PLC.Write_Command[Unit_Num + 39] = 1;
                            Write_On_Bit(Unit_Num, PC_Align_NG);
                        }
                    }
                    else
                    {
                        UVW_Moveing(0.5, 0.5, 0.5);
                        Align_Sequence = ClassEnum.Align_Sequence.Mark_Serch_OK;
                        Frame.Dis_Unit_Label[Unit_Num - 1].Label_Color = Color.Lime;
                        Frame.Label_Color(Color.Lime, Unit_Num, Cul_Type);
                    }
                    break;

                case ClassEnum.Align_Sequence.Mark_Serch_OK:
                    ClassLog.Sequence_Log_Write(Unit_Num, Align_Sequence);
                    Write_Off_Bit(Unit_Num, PC_Align_Start);
                    Write_On_Bit(Unit_Num, PC_Align_End);
                    if (Type[(int)Cul_Type].Align_Result.Equals(ClassEnum.Align_Result.OK))
                        Write_On_Bit(Unit_Num, PC_Align_OK);
                    else
                        Write_On_Bit(Unit_Num, PC_Align_NG);
                    Align_Sequence = ClassEnum.Align_Sequence.Align_End_Req;
                    ClassLog.Sequence_Log_Write(Unit_Num, Align_Sequence);
                    break;


                case ClassEnum.Align_Sequence.ReAlign_Move_Req:
                    if ((_Read_Data & PLC_Move_Ack).Equals(PLC_Move_Ack))
                    {
                        Write_Off_Bit(Unit_Num, PC_Move_Req);
                        //Write_On_Bit(Unit_Num, PC_Move_Req);
                        Align_Sequence = ClassEnum.Align_Sequence.ReAlign_Move_Doen_Req;
                    }
                    break;


                case ClassEnum.Align_Sequence.ReAlign_Move_Doen_Req:
                    if ((_Read_Data & PLC_Move_Done_Req).Equals(PLC_Move_Done_Req))
                    {
                        Write_On_Bit(Unit_Num, PC_Move_Done_Ack);
                        Align_Sequence = ClassEnum.Align_Sequence.ReAlign_Move_Doen_Ack;
                    }
                    break;


                case ClassEnum.Align_Sequence.ReAlign_Move_Doen_Ack:
                    if ((_Read_Data & PLC_Move_Done_Req).Equals(0))
                    {
                        Write_Off_Bit(Unit_Num, PC_Move_Done_Ack);
                        Write_On_Bit(Unit_Num, PC_1St_Req);
                        Align_Sequence = ClassEnum.Align_Sequence.ReAlign_1st;
                    }
                    break;


                case ClassEnum.Align_Sequence.ReAlign_1st:
                    if ((_Read_Data & PLC_1St_Ack).Equals(PLC_1St_Ack))
                    {
                        //Write_Off_Bit(Unit_Num, PC_1St_Req);
                        Write_On_Bit(Unit_Num, PC_1St_Req);
                        Align_Sequence = ClassEnum.Align_Sequence.Align_Start_Ack;
                    }
                    break;


                case ClassEnum.Align_Sequence.Align_End_Req:
                    //if ((_Read_Data & PLC_Align_End).Equals(PLC_Align_End))
                    if ((_Read_Data & PLC_Align_Start).Equals(0))
                    {
                        ClassLog.Sequence_Log_Write(Unit_Num, ClassEnum.Align_Sequence.Align_End_Ack);
                        Write_Off_Bit(Unit_Num, PC_Align_End);
                        Write_Off_Bit(Unit_Num, PC_Align_OK);
                        Write_Off_Bit(Unit_Num, PC_Align_NG);
                        Re_Align_2_1 = false;
                        Align_Sequence = ClassEnum.Align_Sequence.Align_End;
                        ClassLog.Sequence_Log_Write(Unit_Num, Align_Sequence);
                    }
                    break;


                case ClassEnum.Align_Sequence.Align_End:
                    if ((_Read_Data & PLC_Align_End).Equals(0))
                    {
                        //로그 및 이미지 저장 시작
                        DateTime Save_Time = DateTime.Now;
                        ClassLog.Tact_Write(Unit_Num, (Save_Time - ST).TotalMilliseconds);
                        Frame.Main_Display.Log_Data(Unit_Num, Save_Time, (Save_Time - ST).TotalMilliseconds, Cell_ID, Type[(int)Cul_Type].L_Check[0] / 1000,
                                            Align_Position_X, Align_Position_Y, Align_Position_T,
                                            Offset_Position_X, Offset_Position_Y, Offset_Position_T);
                        ClassLog.Sequence_Log_Write(Unit_Num, Align_Sequence);
                        ClassLog.Log_Write(Unit_Num, (int)Cul_Type, Save_Time, Cell_ID, Recipe_No,
                                            Align_Position_X, Align_Position_Y, Align_Position_T,
                                            Offset_Position_X, Offset_Position_Y, Offset_Position_T);
                        ClassLog.Sequence_Log_Write(Unit_Num, Align_Sequence);
                        ClassLog.Picture_Save(Unit_Num, (int)Cul_Type, Save_Time, Cell_ID);
                        Align_Sequence = ClassEnum.Align_Sequence.Ready;
                        Sequences = ClassEnum.Sequence.Ready;
                        ClassLog.Sequence_Log_Write(Unit_Num, Align_Sequence);
                        //MessageBox.Show(Convert.ToString((DateTime.Now - ST).Milliseconds));
                    }
                    break;
            }
            return false;
        }

        private bool Is_Calibration_Sequence(int _Read_Data)
        {
            bool mError = false;
            if(Frame.Setup_Display.CTQ_CheckBox.Checked)
            {
                CTQ(_Read_Data);
            }
            else
            {
                switch (Calibration_Sequence)
                {
                    case ClassEnum.Calibration_Sequence.Calibration_Start_Req:
                        Write_On_Bit(Unit_Num, PC_Cal_Start);
                        //Cal_Position = 0;
                        //Unit[Unit_Num].Sim_Cal = 0;
                        Type[(int)Cul_Type].Cal_Display[0].InteractiveGraphics.Clear();
                        Type[(int)Cul_Type].Cal_Display[1].InteractiveGraphics.Clear();
                        Calibration_Sequence = ClassEnum.Calibration_Sequence.Calibrationn_Start_Ack;
                        break;


                    case ClassEnum.Calibration_Sequence.Calibrationn_Start_Ack:
                        UVW_Moveing(Cal_Position_Data_X[Cal_Position], Cal_Position_Data_Y[Cal_Position], Cal_Position_Data_T[Cal_Position]);
                        Write_On_Bit(Unit_Num, PC_Move_Req);
                        Calibration_Sequence = ClassEnum.Calibration_Sequence.Move1_Req;
                        break;


                    case ClassEnum.Calibration_Sequence.Move1_Req:
                        if ((_Read_Data & PLC_Move_Ack).Equals(PLC_Move_Ack))
                        {
                            Write_Off_Bit(Unit_Num, PC_Move_Req);
                            Calibration_Sequence = ClassEnum.Calibration_Sequence.Move1_Done_Req;
                        }
                        break;


                    case ClassEnum.Calibration_Sequence.Move1_Done_Req:
                        if ((_Read_Data & PLC_Move_Done_Req).Equals(PLC_Move_Done_Req))
                        {
                            Write_On_Bit(Unit_Num, PC_Move_Done_Ack);
                            Calibration_Sequence = ClassEnum.Calibration_Sequence.Move1_Done_Ack;
                        }
                        break;


                    case ClassEnum.Calibration_Sequence.Move1_Done_Ack:
                        if ((_Read_Data & PLC_Move_Done_Req).Equals(0))
                        {
                            Write_Off_Bit(Unit_Num, PC_Move_Done_Ack);
                            // 캘리브레이션시 좌측화면 우측화면 X Y 읽어오기 (Cal_Position_Y * 3 + Cal_Position_X + Cal_Position_T)
                            // 괄호 식은 배열 POINT 넘버링용
                            mError = Type[(int)Cul_Type].Auto_Calibration(ref Type[(int)Cul_Type].Cal_Data_MARK1[Cal_Position], ref Type[(int)Cul_Type].Cal_Data_MARK2[Cal_Position], (int)ClassEnum.Position.First);
                            UVW_Moveing(0, 0, 0);
                            Write_On_Bit(Unit_Num, PC_Move_Req);
                        }
                        break;

                        //1차 촬상 요구
                    case ClassEnum.Calibration_Sequence.Mark1_Serch_OK:
                        if ((_Read_Data & PLC_Move_Ack).Equals(PLC_Move_Ack))
                        {
                            Write_Off_Bit(Unit_Num, PC_Move_Req);
                            Calibration_Sequence = ClassEnum.Calibration_Sequence.Home1_Ack;
                        }
                        break;

                    case ClassEnum.Calibration_Sequence.Home1_Ack:
                        if ((_Read_Data & PLC_Move_Done_Req).Equals(PLC_Move_Done_Req))
                        {
                            Write_On_Bit(Unit_Num, PC_Move_Done_Ack);
                            Calibration_Sequence = ClassEnum.Calibration_Sequence.Home1_Req;
                        }
                        break;
                    case ClassEnum.Calibration_Sequence.Home1_Req:
                        if ((_Read_Data & PLC_Move_Done_Req).Equals(0))
                        {
                            Write_Off_Bit(Unit_Num, PC_Move_Done_Ack);
                            int tickCount = Environment.TickCount;
                            while (true)
                            {
                                if (Environment.TickCount - tickCount > 500)
                                    break;
                            }
                            if (Cal_Position.Equals(27))                                           // THETA CAL 완료 후
                            {
                                Cal_Position = 0;
                                Type[(int)Cul_Type].Cal_Display[0].InteractiveGraphics.Clear();
                                Type[(int)Cul_Type].Cal_Display[1].InteractiveGraphics.Clear();
                                if (Type[(int)Cul_Type].Second_Grab_Use)
                                {
                                    Write_On_Bit(Unit_Num, PC_2nd_Req);
                                    Calibration_Sequence = ClassEnum.Calibration_Sequence.Move_2nd_Req;
                                }
                                else
                                {
                                    Write_Off_Bit(Unit_Num, PC_Cal_Start);
                                    Write_On_Bit(Unit_Num, PC_Cal_End);
                                    Calibration_Sequence = ClassEnum.Calibration_Sequence.Calibration_End_Req;
                                }
                            }
                            else
                            {
                                Cal_Position++;
                                Calibration_Sequence = ClassEnum.Calibration_Sequence.Calibrationn_Start_Ack;
                            }
                        }
                        break;


                    case ClassEnum.Calibration_Sequence.Move_2nd_Req:                           //이후 2차 POSITION 동일
                        if ((_Read_Data & PLC_2nd_Ack).Equals(PLC_2nd_Ack))
                        {
                            Write_Off_Bit(Unit_Num, PC_2nd_Req);
                            Calibration_Sequence = ClassEnum.Calibration_Sequence.Move_2nd_Ack;
                        }
                        break;


                    case ClassEnum.Calibration_Sequence.Move_2nd_Ack:
                        UVW_Moveing(Cal_Position_Data_X[Cal_Position], Cal_Position_Data_Y[Cal_Position], Cal_Position_Data_T[Cal_Position]);
                        Write_On_Bit(Unit_Num, PC_Move_Req);
                        Calibration_Sequence = ClassEnum.Calibration_Sequence.Move2_Req;
                        break;


                    case ClassEnum.Calibration_Sequence.Move2_Req:
                        if ((_Read_Data & PLC_Move_Ack).Equals(PLC_Move_Ack))
                        {
                            Write_Off_Bit(Unit_Num, PC_Move_Req);
                            Calibration_Sequence = ClassEnum.Calibration_Sequence.Move2_Done_Req;
                        }
                        break;


                    case ClassEnum.Calibration_Sequence.Move2_Done_Req:
                        if ((_Read_Data & PLC_Move_Done_Req).Equals(PLC_Move_Done_Req))
                        {
                            Write_On_Bit(Unit_Num, PC_Move_Done_Ack);
                            Calibration_Sequence = ClassEnum.Calibration_Sequence.Move2_Done_Ack;
                        }
                        break;


                    case ClassEnum.Calibration_Sequence.Move2_Done_Ack:
                        if ((_Read_Data & PLC_Move_Done_Req).Equals(0))
                        {
                            Write_Off_Bit(Unit_Num, PC_Move_Done_Ack);
                            mError = Type[(int)Cul_Type].Auto_Calibration(ref Type[(int)Cul_Type].Cal_Data_MARK3[Cal_Position], ref Type[(int)Cul_Type].Cal_Data_MARK4[Cal_Position], (int)ClassEnum.Position.Second);
                            UVW_Moveing(0, 0, 0);
                            Write_On_Bit(Unit_Num, PC_Move_Req);
                        }
                        break;


                    case ClassEnum.Calibration_Sequence.Mark2_Serch_OK:
                        if ((_Read_Data & PLC_Move_Ack).Equals(PLC_Move_Ack))
                        {
                            Write_Off_Bit(Unit_Num, PC_Move_Req);
                            Calibration_Sequence = ClassEnum.Calibration_Sequence.Home2_Ack;
                        }
                        break;

                    case ClassEnum.Calibration_Sequence.Home2_Ack:
                        if ((_Read_Data & PLC_Move_Done_Req).Equals(PLC_Move_Done_Req))
                        {
                            Write_On_Bit(Unit_Num, PC_Move_Done_Ack);
                            Calibration_Sequence = ClassEnum.Calibration_Sequence.Home2_Req;
                        }
                        break;
                    case ClassEnum.Calibration_Sequence.Home2_Req:
                        if ((_Read_Data & PLC_Move_Done_Req).Equals(0))
                        {
                            Write_Off_Bit(Unit_Num, PC_Move_Done_Ack);
                            int tickCount = Environment.TickCount;
                            while (true)
                            {
                                if (Environment.TickCount - tickCount > 500)
                                    break;
                            }
                            if (Cal_Position.Equals(27))
                            {
                                Cal_Position = 0;
                                Write_Off_Bit(Unit_Num, PC_Cal_Start);
                                Write_On_Bit(Unit_Num, PC_Cal_End);
                                Type[(int)Cul_Type].Model[0].Calibration_Data(Type[(int)Cul_Type].Cal_Data_MARK1, Type[(int)Cul_Type].Cal_Move_Pitch_T);
                                Type[(int)Cul_Type].Model[1].Calibration_Data(Type[(int)Cul_Type].Cal_Data_MARK2, Type[(int)Cul_Type].Cal_Move_Pitch_T);
                                Type[(int)Cul_Type].Model[2].Calibration_Data(Type[(int)Cul_Type].Cal_Data_MARK3, Type[(int)Cul_Type].Cal_Move_Pitch_T);
                                Type[(int)Cul_Type].Model[3].Calibration_Data(Type[(int)Cul_Type].Cal_Data_MARK4, Type[(int)Cul_Type].Cal_Move_Pitch_T);
                                Type[(int)Cul_Type].Calibration_L_Check();
                                ClassINI.Write_Calibration_Data(Unit_Num, Type[(int)Cul_Type].Cal_Data_MARK1, Type[(int)Cul_Type].Cal_Data_MARK2, ClassEnum.Position.First);
                                ClassINI.Write_Calibration_Data(Unit_Num, Type[(int)Cul_Type].Cal_Data_MARK3, Type[(int)Cul_Type].Cal_Data_MARK4, ClassEnum.Position.Second);
                                Calibration_Sequence = ClassEnum.Calibration_Sequence.Calibration_End_Req;
                            }
                            else
                            {
                                Cal_Position++;
                                Calibration_Sequence = ClassEnum.Calibration_Sequence.Move_2nd_Ack;
                            }
                        }
                        break;
                    case ClassEnum.Calibration_Sequence.Calibration_End_Req:
                        //if ((Read_Data & PLC_Cal_End).Equals(0))    // PLC CLA END 신호
                        if ((_Read_Data & PLC_Cal_Start).Equals(0))   // PLC CAL START OFF 신호        PLC가 CLA END를 안주고 스타트만 꺼요........
                        {
                            Write_Off_Bit(Unit_Num, PC_Cal_End);
                            Calibration_Sequence = ClassEnum.Calibration_Sequence.Calibration_End;
                        }
                        break;


                    case ClassEnum.Calibration_Sequence.Calibration_End:
                        if ((_Read_Data & PLC_Cal_End).Equals(0))
                        {
                            Thread Result_Thread = new Thread(Result);
                            Result_Thread.SetApartmentState(ApartmentState.STA);
                            Result_Thread.Start();
                            Result_Thread.Join();
                            Calibration_Sequence = ClassEnum.Calibration_Sequence.Ready;
                            Sequences = ClassEnum.Sequence.Ready;
                        }
                        break;
                }
            }
            return mError;
        }

        private bool Is_Inspection_Sequence(int _Read_Data)
        {
            bool mError = false;
            switch (Inspection_Sequence)
            {
                case ClassEnum.Inspection_Sequence.Inspecion_Start_Req:
                    ClassLog.PLC_Log_Write(Unit_Num, _Read_Data);
                    Write_On_Bit(Unit_Num, PC_Align_Start);
                    Inspection_Sequence = ClassEnum.Inspection_Sequence.Inspecion_Start_Ack;
                    ClassLog.Inspection_Log_Write(Unit_Num, Inspection_Sequence);
                    break;


                case ClassEnum.Inspection_Sequence.Inspecion_Start_Ack:
                    ClassLog.PLC_Log_Write(Unit_Num, _Read_Data);
                    mError = Unit[Unit_Num].Type[(int)ClassEnum.Type.Target].Auto_Grab((int)ClassEnum.Position.First);
                    break;


                case ClassEnum.Inspection_Sequence.Grab_1st_OK:
                    ClassLog.PLC_Log_Write(Unit_Num, _Read_Data);
                    ClassLog.Inspection_Log_Write(Unit_Num, Inspection_Sequence);
                    if (Type[(int)ClassEnum.Type.Target].Second_Grab_Use)
                    {
                        Write_On_Bit(Unit_Num, PC_2nd_Req);
                        Inspection_Sequence = ClassEnum.Inspection_Sequence.Move_2nd_Req;
                        ClassLog.Inspection_Log_Write(Unit_Num, Inspection_Sequence);
                    }
                    else
                        Inspection_Sequence = ClassEnum.Inspection_Sequence.Grab_2nd_OK;
                    break;


                case ClassEnum.Inspection_Sequence.Move_2nd_Req:
                    if ((_Read_Data & PLC_2nd_Ack).Equals(PLC_2nd_Ack))
                    {
                        ClassLog.PLC_Log_Write(Unit_Num, _Read_Data);
                        ClassLog.Inspection_Log_Write(Unit_Num, ClassEnum.Inspection_Sequence.Move_2nd_Ack);
                        Write_Off_Bit(Unit_Num, PC_2nd_Req);
                        mError = Type[(int)ClassEnum.Type.Target].Auto_Grab((int)ClassEnum.Position.Second);
                    }
                    break;


                case ClassEnum.Inspection_Sequence.Grab_2nd_OK:
                    ClassLog.PLC_Log_Write(Unit_Num, _Read_Data);
                    ClassLog.Inspection_Log_Write(Unit_Num, Inspection_Sequence);
                    Type[(int)ClassEnum.Type.Target].Auto_Mark_FInd();
                    Type[(int)ClassEnum.Type.Object].Inspection_Mark_FInd(Type[(int)ClassEnum.Type.Target].Display);
                    break;


                case ClassEnum.Inspection_Sequence.Inspection:
                    if (Type[(int)ClassEnum.Type.Target].Inspection_Start())     //인스펙션 함수 만들어야함
                    {
                        ClassLog.PLC_Log_Write(Unit_Num, _Read_Data);
                        Inspection_Sequence = ClassEnum.Inspection_Sequence.Mark_Serch_OK;
                        Frame.Dis_Unit_Label[Unit_Num - 1].Label_Color = Color.Lime;
                        Frame.Label_Color(Color.Lime, Unit_Num, Cul_Type);
                    }
                    else
                    {
                        ClassLog.PLC_Log_Write(Unit_Num, _Read_Data);
                        Frame.Label_Color(Color.Red, Unit_Num, Cul_Type);
                        Frame.Dis_Unit_Label[Unit_Num - 1].Label_Color = Color.Red;
                        Frame.PLC.Write_Command[Unit_Num + 39] = 1;
                        Write_On_Bit(Unit_Num, PC_Align_NG);
                    }
                    break;



                case ClassEnum.Inspection_Sequence.Mark_Serch_OK:
                    ClassLog.PLC_Log_Write(Unit_Num, _Read_Data);
                    ClassLog.Inspection_Log_Write(Unit_Num, Inspection_Sequence);
                    Write_Off_Bit(Unit_Num, PC_Align_Start);
                    if (Type[(int)ClassEnum.Type.Target].Align_Result.Equals(ClassEnum.Align_Result.OK))
                        Write_On_Bit(Unit_Num, PC_Inspection_OK);
                    else
                        Write_On_Bit(Unit_Num, PC_Inspection_NG);
                    Inspection_Sequence = ClassEnum.Inspection_Sequence.Inspecion_End_Req;
                    ClassLog.Inspection_Log_Write(Unit_Num, Inspection_Sequence);
                    break;

                case ClassEnum.Inspection_Sequence.Inspecion_End_Req:
                    //if ((_Read_Data & PLC_Align_End).Equals(PLC_Align_End))
                    if ((_Read_Data & PLC_Inspection_Start).Equals(0))
                    {
                        ClassLog.PLC_Log_Write(Unit_Num, _Read_Data);
                        ClassLog.Inspection_Log_Write(Unit_Num, ClassEnum.Inspection_Sequence.Inspecion_End_Ack);
                        Write_Off_Bit(Unit_Num, PC_Inspection_OK);
                        Write_Off_Bit(Unit_Num, PC_Inspection_NG);
                        Inspection_Sequence = ClassEnum.Inspection_Sequence.Inspecion_End;
                        ClassLog.Inspection_Log_Write(Unit_Num, Inspection_Sequence);
                    }
                    break;


                case ClassEnum.Inspection_Sequence.Inspecion_End:
                    //로그 및 이미지 저장 시작
                    DateTime Save_Time = DateTime.Now;
                    ClassLog.PLC_Log_Write(Unit_Num, _Read_Data);
                    Frame.Main_Display.Inspection_Log_Data(Unit_Num, Save_Time, (Save_Time - ST).TotalMilliseconds, Cell_ID, Type[(int)Cul_Type].L_Check[0] / 1000,     //인스펙션 로그 만들어야함
                                                           Type[(int)ClassEnum.Type.Object].Model[0].inspection1, Type[(int)ClassEnum.Type.Object].Model[0].inspection2,
                                                           Type[(int)ClassEnum.Type.Object].Model[1].inspection1, Type[(int)ClassEnum.Type.Object].Model[1].inspection2,
                                                           Type[(int)ClassEnum.Type.Object].Model[2].inspection1, Type[(int)ClassEnum.Type.Object].Model[2].inspection2,
                                                           Type[(int)ClassEnum.Type.Object].Model[3].inspection1, Type[(int)ClassEnum.Type.Object].Model[3].inspection2, 0);
                    ClassLog.Picture_Save(Unit_Num, (int)ClassEnum.Type.Target, Save_Time, Cell_ID);
                    Inspection_Sequence = ClassEnum.Inspection_Sequence.Ready;
                    Sequences = ClassEnum.Sequence.Ready;
                    ClassLog.Inspection_Log_Write(Unit_Num, Inspection_Sequence);
                    break;
            }
            return false;
        }
        int CTQ_SEQ_COUNT = 0;
        int CTQ_GRAB_COUNT = 0;
        double[] CTQ_Position_Data_X = new double[17] { 0, 0.5, -0.5, 1, -1, 1.5, -1.5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, };
        double[] CTQ_Position_Data_Y = new double[17] { 0, 0, 0, 0, 0, 0, 0, 0.5, -0.5, 1, -1, 1.5, -1.5, 0, 0, 0, 0, };
        double[] CTQ_Position_Data_T = new double[17] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.5, -0.5, 1, -1 };
        private void CTQ(int _Read_Data)
        {
            bool mError = false;
            switch (Calibration_Sequence)
            {
                case ClassEnum.Calibration_Sequence.Calibration_Start_Req:
                    Write_On_Bit(Unit_Num, PC_Cal_Start);
                    Calibration_Sequence = ClassEnum.Calibration_Sequence.Calibrationn_Start_Ack;
                    break;


                case ClassEnum.Calibration_Sequence.Calibrationn_Start_Ack:
                    UVW_Moveing(CTQ_Position_Data_X[CTQ_SEQ_COUNT], CTQ_Position_Data_Y[CTQ_SEQ_COUNT], CTQ_Position_Data_T[CTQ_SEQ_COUNT]);
                    Write_On_Bit(Unit_Num, PC_Move_Req);
                    Calibration_Sequence = ClassEnum.Calibration_Sequence.Move1_Req;
                    break;


                case ClassEnum.Calibration_Sequence.Move1_Req:
                    if ((_Read_Data & PLC_Move_Ack).Equals(PLC_Move_Ack))
                    {
                        Write_Off_Bit(Unit_Num, PC_Move_Req);
                        Calibration_Sequence = ClassEnum.Calibration_Sequence.Move1_Done_Req;
                    }
                    break;


                case ClassEnum.Calibration_Sequence.Move1_Done_Req:
                    if ((_Read_Data & PLC_Move_Done_Req).Equals(PLC_Move_Done_Req))
                    {
                        Write_On_Bit(Unit_Num, PC_Move_Done_Ack);
                        Calibration_Sequence = ClassEnum.Calibration_Sequence.Move1_Done_Ack;
                    }
                    break;


                case ClassEnum.Calibration_Sequence.Move1_Done_Ack:
                    if ((_Read_Data & PLC_Move_Done_Req).Equals(0))
                    {
                        Write_Off_Bit(Unit_Num, PC_Move_Done_Ack);
                        mError = Type[(int)Cul_Type].Auto_Grab((int)ClassEnum.Position.First);
                        Write_On_Bit(Unit_Num, PC_2nd_Req);
                        Calibration_Sequence = ClassEnum.Calibration_Sequence.Move_2nd_Req;
                    }
                    break;

                case ClassEnum.Calibration_Sequence.Move_2nd_Req:                           //이후 2차 POSITION 동일
                    if ((_Read_Data & PLC_2nd_Ack).Equals(PLC_2nd_Ack))
                    {
                        Write_Off_Bit(Unit_Num, PC_2nd_Req);
                        mError = Type[(int)Cul_Type].Auto_Grab((int)ClassEnum.Position.Second);
                        UVW_Moveing(0, 0, 0);
                        Write_On_Bit(Unit_Num, PC_Move_Req);
                        Calibration_Sequence = ClassEnum.Calibration_Sequence.Mark1_Serch_OK;
                    }
                    break;

                case ClassEnum.Calibration_Sequence.Mark1_Serch_OK:
                    if ((_Read_Data & PLC_Move_Ack).Equals(PLC_Move_Ack))
                    {
                        Write_Off_Bit(Unit_Num, PC_Move_Req);
                        Calibration_Sequence = ClassEnum.Calibration_Sequence.Home1_Ack;
                    }
                    break;

                case ClassEnum.Calibration_Sequence.Home1_Ack:
                    if ((_Read_Data & PLC_Move_Done_Req).Equals(PLC_Move_Done_Req))
                    {
                        Write_On_Bit(Unit_Num, PC_Move_Done_Ack);
                        Calibration_Sequence = ClassEnum.Calibration_Sequence.Home1_Req;
                    }
                    break;
                case ClassEnum.Calibration_Sequence.Home1_Req:
                    if ((_Read_Data & PLC_Move_Done_Req).Equals(0))
                    {
                        Write_Off_Bit(Unit_Num, PC_Move_Done_Ack);
                        Type[(int)Cul_Type].Auto_Mark_FInd();
                        Calibration_Sequence = ClassEnum.Calibration_Sequence.Grab1_OK;
                    }
                    break;
                case ClassEnum.Calibration_Sequence.Grab1_OK:
                    Type[(int)Cul_Type].Align_Start(ref Align_Position_X, ref Align_Position_Y, ref Align_Position_T);
                    DateTime Save_Time = DateTime.Now;
                    Frame.Main_Display.Log_Data(Unit_Num, Save_Time, (Save_Time - ST).TotalMilliseconds, Cell_ID, Type[(int)Cul_Type].L_Check[0] / 1000,
                    Align_Position_X, Align_Position_Y, Align_Position_T,
                    Offset_Position_X, Offset_Position_Y, Offset_Position_T);
                    ClassLog.Log_Write(Unit_Num, (int)Cul_Type, Save_Time, Cell_ID, Recipe_No,
                                        Align_Position_X, Align_Position_Y, Align_Position_T,
                                        Offset_Position_X, Offset_Position_Y, Offset_Position_T);
                    ClassLog.Picture_Save(Unit_Num, (int)Cul_Type, Save_Time, Cell_ID);
                    Calibration_Sequence = ClassEnum.Calibration_Sequence.Grab2_OK;
                    break;

                case ClassEnum.Calibration_Sequence.Grab2_OK:
                    Calibration_Sequence = ClassEnum.Calibration_Sequence.Home2_Req;
                    CTQ_GRAB_COUNT++;
                    ST = DateTime.Now;
                    if (CTQ_GRAB_COUNT.Equals(10))
                    {
                        CTQ_GRAB_COUNT = 0;
                        CTQ_SEQ_COUNT++;
                        if (CTQ_SEQ_COUNT.Equals(17))
                        {
                            Write_Off_Bit(Unit_Num, PC_Cal_Start);
                            Write_On_Bit(Unit_Num, PC_Cal_End);
                            Calibration_Sequence = ClassEnum.Calibration_Sequence.Calibration_End_Req;
                        }
                    }
                    break;

                case ClassEnum.Calibration_Sequence.Home2_Req:
                    Write_On_Bit(Unit_Num, PC_1St_Req);
                    Calibration_Sequence = ClassEnum.Calibration_Sequence.Home2_Ack;
                    break;
                case ClassEnum.Calibration_Sequence.Home2_Ack:
                    if (((Int32)Frame.PLC.Receive_Data[120 + (Unit_Num * 10)] | (Int32)(Frame.PLC.Receive_Data[121 + (Unit_Num * 10)] << 16)).Equals(((Int32)Frame.PLC.Receive_Data[124 + (Unit_Num * 10)] | (Int32)(Frame.PLC.Receive_Data[125 + (Unit_Num * 10)] << 16))))
                    {
                        Write_Off_Bit(Unit_Num, PC_1St_Req);
                        Calibration_Sequence = ClassEnum.Calibration_Sequence.Calibrationn_Start_Ack;
                    }
                    break;

                case ClassEnum.Calibration_Sequence.Calibration_End_Req:
                    //if ((Read_Data & PLC_Cal_End).Equals(0))    // PLC CLA END 신호
                    if ((_Read_Data & PLC_Cal_Start).Equals(0))   // PLC CAL START OFF 신호        PLC가 CLA END를 안주고 스타트만 꺼요........
                    {
                        Write_Off_Bit(Unit_Num, PC_Cal_End);
                        Calibration_Sequence = ClassEnum.Calibration_Sequence.Calibration_End;
                    }
                    break;


                case ClassEnum.Calibration_Sequence.Calibration_End:
                    if ((_Read_Data & PLC_Cal_End).Equals(0))
                    {
                        Calibration_Sequence = ClassEnum.Calibration_Sequence.Ready;
                        Sequences = ClassEnum.Sequence.Ready;
                    }
                    break;
            }
        }


        private void Re_Align_Sq(double _X, double _Y, double _T)
        {
            if (_X > Re_Align_Limit || _Y > Re_Align_Limit || _T > Re_Align_Limit || _X < -Re_Align_Limit || _Y < -Re_Align_Limit || _T < -Re_Align_Limit) // 리얼라인 정도
            {
                // 리얼라인시 직전 로그 및 이미지 저장
                DateTime Save_Time = DateTime.Now;
                Frame.Main_Display.Log_Data(Unit_Num, Save_Time, (Save_Time - ST).TotalMilliseconds, Cell_ID, Type[(int)Cul_Type].L_Check[0] / 1000,
                    Align_Position_X, Align_Position_Y, Align_Position_T,
                    Offset_Position_X, Offset_Position_Y, Offset_Position_T);

                ClassLog.Log_Write(Unit_Num, (int)Cul_Type, Save_Time, Cell_ID, Recipe_No,
                                    Align_Position_X, Align_Position_Y, Align_Position_T,
                                    Offset_Position_X, Offset_Position_Y, Offset_Position_T);

                ClassLog.Picture_Save(Unit_Num, (int)Cul_Type, Save_Time, Cell_ID);

                Write_On_Bit(Unit_Num, PC_Move_Req);
                Align_Sequence = ClassEnum.Align_Sequence.ReAlign_Move_Req;
                Frame.Label_Color(Color.Blue, Unit_Num, Cul_Type);
            }
        }

        private void Write_On_Bit(int unit, int _Bit)
        {
            Send_Data[unit] = Send_Data[unit] | _Bit;
            Frame.PLC.Write_Command[unit] = Send_Data[unit];
            Frame.PLC.Write_Bit = true;
        }

        private void Write_Off_Bit(int unit, int _Bit)
        {
            Send_Data[unit] = Send_Data[unit] & (0xffff - _Bit);
            Frame.PLC.Write_Command[unit] = Send_Data[unit];
            Frame.PLC.Write_Bit = true;
        }

        public ClassAlign motion = null;

        private bool UVW_Moveing(double _X, double _Y, double _T)
        {
            if (_X < Unit[Unit_Num].Limit_X && _Y < Unit[Unit_Num].Limit_Y && _T < Unit[Unit_Num].Limit_T)
            {
                #region 기존
                //double X = _X * 10000;                                          // mm를 um로 바꾸기 위하여 *10000
                //double Y = _Y * 10000;
                //double T = Type[(int)Cul_Type].Align.UVW_Theta(_T) * 10000;        //Theta 값을 회전 중심부터 R에 맞게 값 읽어오기
                //Frame.PLC.Write_Command[Unit_Num * 10] = (int)(X + T);
                //Frame.PLC.Write_Command[Unit_Num * 10 + 1] = (int)(X + T) >> 16; // 보정 Data를 16비트씩 보내기 위해 상위 Data Shift 
                //Frame.PLC.Write_Command[Unit_Num * 10 + 2] = (int)(X - T);
                //Frame.PLC.Write_Command[Unit_Num * 10 + 3] = (int)(X - T) >> 16;
                //Frame.PLC.Write_Command[Unit_Num * 10 + 4] = (int)(Y + T);
                //Frame.PLC.Write_Command[Unit_Num * 10 + 5] = (int)(Y + T) >> 16;
                //Frame.PLC.Write_Command[Unit_Num * 10 + 6] = (int)(Y - T);
                //Frame.PLC.Write_Command[Unit_Num * 10 + 7] = (int)(Y - T) >> 16;
                //return false;


                //double X = _X * 10000;                                          // mm를 um로 바꾸기 위하여 *10000
                //double Y = _Y * 10000;
                //double T = Type[(int)Cul_Type].Align.UVW_Theta(_T) * 10000;        //Theta 값을 회전 중심부터 R에 맞게 값 읽어오기
                //Frame.PLC.Write_Command[Unit_Num * 10 + (Unit_Num - 1) * 20 ] = (int)(X + T);
                //Frame.PLC.Write_Command[Unit_Num * 10 + 1 + (Unit_Num - 1) * 20] = (int)(X + T) >> 16; // 보정 Data를 16비트씩 보내기 위해 상위 Data Shift 
                //Frame.PLC.Write_Command[Unit_Num * 10 + 2 + (Unit_Num - 1) * 20] = (int)(X - T);
                //Frame.PLC.Write_Command[Unit_Num * 10 + 3 + (Unit_Num - 1) * 20] = (int)(X - T) >> 16;
                //Frame.PLC.Write_Command[Unit_Num * 10 + 4 + (Unit_Num - 1) * 20] = (int)(Y + T);
                //Frame.PLC.Write_Command[Unit_Num * 10 + 5 + (Unit_Num - 1) * 20] = (int)(Y + T) >> 16;
                //Frame.PLC.Write_Command[Unit_Num * 10 + 6 + (Unit_Num - 1) * 20] = (int)(Y - T);
                //Frame.PLC.Write_Command[Unit_Num * 10 + 7 + (Unit_Num - 1) * 20] = (int)(Y - T) >> 16;
                //return false;

                #endregion

                double x = _X * 10000;
                double y = _Y * 10000;
                double t = _T * 10000;

                double X = 0;
                double Y1 = 0;
                double Y2 = 0;

                motion.UVW_Theta_XYT(x, _Y, _T, ref X, ref Y1, ref Y2);
                
                //double X = _X * 10000;                                          // mm를 um로 바꾸기 위하여 *10000
                //double Y = _Y * 10000;
                //double T = Type[(int)Cul_Type].Align.UVW_Theta_XYT(_X, _Y, _T, X,Y1,Y2) * 10000;        //Theta 값을 회전 중심부터 R에 맞게 값 읽어오기
                Frame.PLC.Write_Command[Unit_Num * 10 + (Unit_Num - 1) * 20 ] = (int)X;
                Frame.PLC.Write_Command[Unit_Num * 10 + 1 + (Unit_Num - 1) * 20] = (int)X >> 16; // 보정 Data를 16비트씩 보내기 위해 상위 Data Shift 
                //Frame.PLC.Write_Command[Unit_Num * 30 + 2 + (Unit_Num - 1) * 20] = (int)(X - T);
                //Frame.PLC.Write_Command[Unit_Num * 10 + 3 + (Unit_Num - 1) * 20] = (int)(X - T) >> 16;
                Frame.PLC.Write_Command[Unit_Num * 10 + 4 + (Unit_Num - 1) * 20] = (int)Y1;
                Frame.PLC.Write_Command[Unit_Num * 10 + 5 + (Unit_Num - 1) * 20] = (int)Y1 >> 16;
                Frame.PLC.Write_Command[Unit_Num * 10 + 6 + (Unit_Num - 1) * 20] = (int)Y2;
                Frame.PLC.Write_Command[Unit_Num * 10 + 7 + (Unit_Num - 1) * 20] = (int)Y2 >> 16;
                return false;
            }
            return true;
        }

        private bool L_Check_Start()
        {
            if (Type[(int)Cul_Type].L_Check[0] - Type[(int)Cul_Type].Master_L_Check[0] <= (double)Frame.Setup_Display.L_CHeck_Limit_numericUpDown.Value &&
                Type[(int)Cul_Type].L_Check[1] - Type[(int)Cul_Type].Master_L_Check[1] <= (double)Frame.Setup_Display.L_CHeck_Limit_numericUpDown.Value &&
                Type[(int)Cul_Type].L_Check[2] - Type[(int)Cul_Type].Master_L_Check[2] <= (double)Frame.Setup_Display.L_CHeck_Limit_numericUpDown.Value &&
                Type[(int)Cul_Type].L_Check[3] - Type[(int)Cul_Type].Master_L_Check[3] <= (double)Frame.Setup_Display.L_CHeck_Limit_numericUpDown.Value)
            { return false; }
            return true;
        }

        private void Result()
        {
            // 모든 Point 계산후 좌상 화면과 우상 화면의 Theta 기준값 취출
            Type[(int)Cul_Type].Master_Theta_Calculation();
            Type[(int)Cul_Type].Master_Theta_Save();
            if (Type[(int)Cul_Type].Second_Grab_Use)
            {
                Type[(int)Cul_Type].Cal_Data.Result1 = Result_Mark(Type[(int)Cul_Type].Cal_Data_MARK1);
                Type[(int)Cul_Type].Cal_Data.Result2 = Result_Mark(Type[(int)Cul_Type].Cal_Data_MARK2);
                Type[(int)Cul_Type].Cal_Data.Result3 = Result_Mark(Type[(int)Cul_Type].Cal_Data_MARK3);
                Type[(int)Cul_Type].Cal_Data.Result4 = Result_Mark(Type[(int)Cul_Type].Cal_Data_MARK4);

                Type[(int)Cul_Type].Cal_Data.Resolution_X1 = Type[(int)Cul_Type].Model[0].resolution_X;
                Type[(int)Cul_Type].Cal_Data.Resolution_X2 = Type[(int)Cul_Type].Model[1].resolution_X;
                Type[(int)Cul_Type].Cal_Data.Resolution_X3 = Type[(int)Cul_Type].Model[2].resolution_X;
                Type[(int)Cul_Type].Cal_Data.Resolution_X4 = Type[(int)Cul_Type].Model[3].resolution_X;
                Type[(int)Cul_Type].Cal_Data.Resolution_Y1 = Type[(int)Cul_Type].Model[0].resolution_Y;
                Type[(int)Cul_Type].Cal_Data.Resolution_Y2 = Type[(int)Cul_Type].Model[1].resolution_Y;
                Type[(int)Cul_Type].Cal_Data.Resolution_Y3 = Type[(int)Cul_Type].Model[2].resolution_Y;
                Type[(int)Cul_Type].Cal_Data.Resolution_Y4 = Type[(int)Cul_Type].Model[3].resolution_Y;

                Type[(int)Cul_Type].Cal_Data.Position_X1 = Type[(int)Cul_Type].Model[0].Master_X;
                Type[(int)Cul_Type].Cal_Data.Position_X2 = Type[(int)Cul_Type].Model[1].Master_X;
                Type[(int)Cul_Type].Cal_Data.Position_X3 = Type[(int)Cul_Type].Model[2].Master_X;
                Type[(int)Cul_Type].Cal_Data.Position_X4 = Type[(int)Cul_Type].Model[3].Master_X;
                Type[(int)Cul_Type].Cal_Data.Position_Y1 = Type[(int)Cul_Type].Model[0].Master_Y;
                Type[(int)Cul_Type].Cal_Data.Position_Y2 = Type[(int)Cul_Type].Model[1].Master_Y;
                Type[(int)Cul_Type].Cal_Data.Position_Y3 = Type[(int)Cul_Type].Model[2].Master_Y;
                Type[(int)Cul_Type].Cal_Data.Position_Y4 = Type[(int)Cul_Type].Model[3].Master_Y;
                Calibration_Result cal_Result = new Calibration_Result(Type[(int)Cul_Type].Cal_Data, Type[(int)Cul_Type].Cal_Data_MARK1, Type[(int)Cul_Type].Cal_Data_MARK2, Type[(int)Cul_Type].Cal_Data_MARK3, Type[(int)Cul_Type].Cal_Data_MARK4);
                cal_Result.ShowDialog();
            }
            else
            {
                Type[(int)Cul_Type].Cal_Data.Result1 = Result_Mark(Type[(int)Cul_Type].Cal_Data_MARK1);
                Type[(int)Cul_Type].Cal_Data.Result2 = Result_Mark(Type[(int)Cul_Type].Cal_Data_MARK2);

                Type[(int)Cul_Type].Cal_Data.Resolution_X1 = Type[(int)Cul_Type].Model[0].resolution_X;
                Type[(int)Cul_Type].Cal_Data.Resolution_X2 = Type[(int)Cul_Type].Model[1].resolution_X;
                Type[(int)Cul_Type].Cal_Data.Resolution_Y1 = Type[(int)Cul_Type].Model[0].resolution_Y;
                Type[(int)Cul_Type].Cal_Data.Resolution_Y2 = Type[(int)Cul_Type].Model[1].resolution_Y;

                Type[(int)Cul_Type].Cal_Data.Position_X1 = Type[(int)Cul_Type].Model[0].Master_X;
                Type[(int)Cul_Type].Cal_Data.Position_X2 = Type[(int)Cul_Type].Model[1].Master_X;
                Type[(int)Cul_Type].Cal_Data.Position_Y1 = Type[(int)Cul_Type].Model[0].Master_Y;
                Type[(int)Cul_Type].Cal_Data.Position_Y2 = Type[(int)Cul_Type].Model[1].Master_Y;
                Calibration_Result cal_Result = new Calibration_Result(Type[(int)Cul_Type].Cal_Data, Type[(int)Cul_Type].Cal_Data_MARK1, Type[(int)Cul_Type].Cal_Data_MARK2);
                cal_Result.ShowDialog();
            }
        }

        private bool Result_Mark(PointF[] _Point)
        {
            if (Math.Abs(_Point[2].X - _Point[22].X) < (Type[(int)Cul_Type].Cal_Move_Pitch_X * 10) && Math.Abs(_Point[10].Y - _Point[14].Y) < (Type[(int)Cul_Type].Cal_Move_Pitch_Y * 10))
                return true;
            return false;
        }

        public void Reset()
        {
            Re_Align_2_1 = false;
            Unit_Reset(Unit_Num);
        }

        private void Unit_Reset(int Unit)
        {
            Sequences = ClassEnum.Sequence.Ready;
            Align_Sequence = ClassEnum.Align_Sequence.Ready;
            Calibration_Sequence = ClassEnum.Calibration_Sequence.Ready;
            Send_Data[Unit] = PC_READY;
            Frame.PLC.Write_Command[Unit] = Send_Data[Unit];
            Frame.PLC.Write_Bit = true;
            Cal_Position = 0;
            Sequences = ClassEnum.Sequence.Ready;
            Align_Sequence = ClassEnum.Align_Sequence.Ready;
            Calibration_Sequence = ClassEnum.Calibration_Sequence.Ready;
            Frame.Dis_Unit_Label[Unit_Num - 1].Label_Color = Color.Lime;
        }
    }
}
