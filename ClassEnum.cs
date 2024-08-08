using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Align
{
    public class ClassEnum
    {
        public enum Type : int
        {
            Target = 0,
            Common = 1,
            Object = 2
        }

        public enum PLC_Simul_Mode : int
        {
            ALIGN = 0,
            CALIBRATION = 1,
            INSPECTION = 2
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
            Mark0 = 0,
            Mark1 = 1,
            Mark2 = 2,
            Mark3 = 3
        }

        public enum Authority_Type
        {
            Every,
            Engineer,
            Administrator,
            Maker
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
            MARK_TO_LINE
        }

        public enum Auto_Mode
        {
            AUTO,
            MANUAL,
            SIMUL
        }

        public enum Menu
        {
            Main,
            Calibration,
            Model,
            Data,
            Setup,
            Interface
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
            Align_Start_Ack,
            Move_1st_Req,
            Move_1st_Ack,
            Grab_1st_OK,
            Move_2nd_Req,
            Move_2nd_Ack,
            Grab_2nd_OK,
            Mark_Serch_OK,
            Align,
            ReAlign_Move_Req,
            ReAlign_Move_Ack,
            ReAlign_Move_Doen_Req,
            ReAlign_Move_Doen_Ack,
            ReAlign_1st,
            Align_End_Req,
            Align_End_Ack,
            Align_End,
            Manual_Mark,
            Error,
            TEST_Picture1,
            TEST_Picture2,
            TEST_Picture3,
            TEST_Picture4,
            TEST_Picture5,
            TEST_Picture6
        }

        public enum Calibration_Sequence
        {
            Ready,
            Calibration_Start_Req,
            Calibrationn_Start_Ack,
            Move_1st_Req,
            Move_1st_Ack,
            Grab1_OK,
            Mark1_Serch_OK,
            Move1_Req,
            Move1_Ack,
            Move1_Done_Req,
            Move1_Done_Ack,
            Move_2nd_Req,
            Move_2nd_Ack,
            Grab2_OK,
            Mark2_Serch_OK,
            Move2_Req,
            Move2_Ack,
            Move2_Done_Req,
            Move2_Done_Ack,
            Calibration_End_Req,
            Calibration_End_Ack,
            Calibration_End,
            Home1_Req,
            Home1_Ack,
            Home2_Req,
            Home2_Ack
        }

        public enum Inspection_Sequence
        {
            Ready,
            Inspecion_Start_Req,
            Inspecion_Start_Ack,
            Move_1st_Req,
            Move_1st_Ack,
            Grab_1st_OK,
            Move_2nd_Req,
            Move_2nd_Ack,
            Grab_2nd_OK,
            Mark_Serch_OK,
            Inspection,
            Inspecion_End_Req,
            Inspecion_End_Ack,
            Inspecion_End,
            Manual_Mark,
            Error
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
    }
}
