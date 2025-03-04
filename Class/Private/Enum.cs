using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Align
{
    public class Enum
    {
        public enum Mode
        {
            Auto,
            Manual,
            Simul
        }

        public enum ERROR
        {
            Auto_Run,
            NOT_Login,
            Different_Password,
            Current_Recipe,
            Name_Null,
            Null_Recipe
        }

        public enum Type : int
        {
            Target = 0,
            Common = 1,
            Object = 2
        }
        public enum Align_Result : int
        {
            OK = 0,
            NG = 1
        }

        public enum Mark_Setting : int
        {
            COMMON,
            MARK,
            LINE_X,
            LINE_Y
        }

        public enum Graphics_Button : int
        {
            COMMON,
            MESURE,
            MARK,
            ROI,
            MASK
        }

        public enum Unit : int
        {
            Common = 0,
            Unit1 = 1,
            Unit2 = 2,
            Unit3 = 3,
            Unit4 = 4,
            Unit5 = 5,
            Unit6 = 6,
            Unit7 = 7,
            Unit8 = 8
        }

        public enum Mark : int
        {
            MARK1 = 0,
            MARK2 = 1,
            MARK3 = 2,
            MARK4 = 3,
            MARK5 = 4,
            MARK6 = 5
        }

        public enum Position : int
        {
            First = 0,
            COMMON = 1,
            Second = 2
        }

        public enum Mark_Type
        {
            MARK,
            LINE,
            MARK_TO_LINE,
            CIRCLE,
            MARK_TO_CIRCLE,
            PATTERN
        }

        public enum Data_Type
        {
            Picture,
            Chart
        }

        public enum Sequence
        {
            Align,
            Calibration,
            Inspection,
            Ready,
            Error
        }

        public enum Align_Sequence
        {
            Ready,
            Align_Start_Req,
            Align_Start,
            Target_Object_Req_1ST,
            Target_Object_Ack_1ST,
            Move_2nd_Req,
            Move_2nd_Ack,
            Target_Object_Req_2ND,
            Target_Object_Ack_2ND,
            Align,
            ReAlign_Check,
            ReAlign_Move_Ack,
            ReAlign_Move_Doen_Req,
            ReAlign_Move_Doen_Ack,
            ReAlign_1ST,
            Align_End_Req,
            Align_End_Ack,
            Align_End,
            Manual_Mark,
            Error,
        }

        public enum Calibration_Sequence
        {
            Ready,
            Calibration_Start_Req,
            Calibration_Start,
            UVW_Move_Req,
            UVW_Move_Ack,
            UVW_Move_Done_Req,
            UVW_Move_Done_Ack,
            Grab_OK,
            Move_2nd_Req,
            Move_2nd_Ack,
            Calibration_End_Req,
            Calibration_End_Ack,
            Calibration_End,
        }

        public enum Inspection_Sequence
        {
            Ready,
            Inspecion_Start_Req,
            Inspecion_Start,
            Move_2nd_Req,
            Move_2nd_Ack,
            Inspection,
            Inspection_End_Req,
            Inspection_End_Ack,
            Inspection_End,
            Manual_Mark,
            Error,
        }

        public enum Camera_Rotate : int
        {
            NON,
            _90,
            _180,
            _270,
            FLIP,
            FLIP90,
            FLIP180,
            FLIP270
        }

        public struct Cal_Data
        {
            public bool Result1;
            public bool Result2;
            public bool Result3;
            public bool Result4;

            public double Resolution_X1;
            public double Resolution_Y1;
            public double Resolution_X2;
            public double Resolution_Y2;
            public double Resolution_X3;
            public double Resolution_Y3;
            public double Resolution_X4;
            public double Resolution_Y4;

            public double Position_X1;
            public double Position_Y1;
            public double Position_X2;
            public double Position_Y2;
            public double Position_X3;
            public double Position_Y3;
            public double Position_X4;
            public double Position_Y4;
        }

        public enum Error_Code
        {
            NON,
            GRAB_NG,
            Mark_NG,
            NON2,
            NON3,
            NON4,
            NON5,
            NON6,
            NON7,
            NON8,
            NON9,
            L_CHECK_NG,
            LIMIT
        }

        public enum VC_COLOR
        {
            NON,
            R,
            G,
            B,
            W
        }

        public enum Unit_Type
        {
            NON,
            SG_SA,                  //타겟만
            SG_DA_UNIT,             //타켓만 보정값을 몰아주기
            SG_DA_TO,               //타겟 보정량 오브젝트 몰아주기
            DG_DA_TO_4POSITION,     //타겟 오브젝트 티칭 따로
            DG_DA,                  //타겟 오브젝트 티칭 동일
        }

        static public Unit_Type Unit_Type_Parse(string Unit_Type) { return (Unit_Type)System.Enum.Parse(typeof(Unit_Type), Unit_Type); }


        public enum Motor_Type
        {
            NON,
            UVW_4,
            UVW_4_PMMP,
            UVW_3,
            XYT,
            YXT
        }

        static public Motor_Type Motor_Type_Parse(string Motor_Type) { return (Motor_Type)System.Enum.Parse(typeof(Motor_Type), Motor_Type); }

        public enum Key_Type
        {
            ALL,
            Positive,
            Int_Positive,
            Int
        }

        public enum Calibration_Type
        {
            NON,
            _9T,
            _3XT,
            _3YT,
            _3X,
            _3Y,
            COPY
        }

        static public Calibration_Type Calibration_Type_Parse(string Calibration_Type) { return (Calibration_Type)System.Enum.Parse(typeof(Calibration_Type), Calibration_Type); }

        public enum Serial
        {
            NON,
            COM1,
            COM2,
            COM3,
            COM4,
            COM5,
            COM6,
            COM7,
            COM8,
            COM9,
            COM10,
            COM11,
            COM12,
            COM13,
            COM14,
            COM15,
            COM16,
        }

        public enum Log_Type
        {
            T_Log,
            TO_Log,
            CW_Log
        }

        static public Log_Type Log_Type_Parse(string Log_Type) { return (Log_Type)System.Enum.Parse(typeof(Log_Type), Log_Type); }
    }
}
