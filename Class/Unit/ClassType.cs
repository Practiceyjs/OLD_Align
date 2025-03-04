using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognex.VisionPro.Display;
using System.Windows.Forms;
using Cognex.VisionPro;
using System.Drawing;

namespace T_Align
{
    public class ClassType
    {
        public ClassMark[] Mark = new ClassMark[6];
        Enum.Unit Current_Unit = Enum.Unit.Common;
        Enum.Type Current_Type = Enum.Type.Common;
        Type_Display Display;
        CogDisplay[] Mark_Display;

        public double[] Shuttle_Y = { 0, 0, 0 };
        public double[] CAM_Y = { 0, 0, 0 };

        public Label Title_Label;

        public double Ratio = 0;
        public double Camera_Length = 0;
        public double Shuttle_Length = 0;

        public double Result_X = 0;
        public double Result_Y = 0;
        public double Result_T = 0;

        public double P_Result_X = 0;
        public double P_Result_Y = 0;
        public double P_Result_T = 0;

        public double L_Result_X = 0;
        public double L_Result_Y = 0;
        public double L_Result_T = 0;

        public double[] L_Check = { 0, 0, 0, 0 };
        public double[] L_Check_Spec = { 0, 0, 0, 0 };
        public double[] L_Check_Offset = { 0, 0, 0, 0 };

        public double[] Calibration_Pitch = { 0, 0, 0 };

        public double L_Check_Limit = 0.1;

        public void Type_Set(Enum.Unit _Unit,Enum.Type _Type, Type_Display _Display)
        {
            Current_Unit = _Unit;
            Current_Type = _Type;
            Display = _Display;
            for (int LoopCount = 0; LoopCount < 6; LoopCount++)
            {
                Mark[LoopCount] = new ClassMark();
            }
            Mark_Display = Display.DIsplay_Set();
            Title_Label = Display.Title_Set();
            for (int LoopCount = 0; LoopCount < 4; LoopCount++)
            {
                Mark[LoopCount].DIsplay_Set(Mark_Display[LoopCount]);
                Mark_Display[LoopCount].DoubleClick += Double_Click_Event;
            }

            Recipe_Load();
        }

        private void Double_Click_Event(object sender, EventArgs e)
        {
            CogDisplay Click_Display = (CogDisplay)sender;
            int Sel_Dislay = int.Parse(Click_Display.Name.Substring(Click_Display.Name.Length - 1, 1)) - 1;
            if (Mark[Sel_Dislay].Mark_Error)
            {
                Manual_Mark manual_Mark = new Manual_Mark(Click_Display.Image, Mark[Sel_Dislay]);
                if(manual_Mark.ShowDialog() == DialogResult.OK)
                {
                    Mark[Sel_Dislay].X = manual_Mark.Point_X;
                    Mark[Sel_Dislay].Y = manual_Mark.Point_Y;
                    Mark[Sel_Dislay].Mark_Error = false;
                    Click_Display.InteractiveGraphics.Clear();
                    Click_Display.InteractiveGraphics.Add(manual_Mark.X_Line, "ML", false);
                    Click_Display.InteractiveGraphics.Add(manual_Mark.Y_Line, "ML", false);
                    manual_Mark.Dispose();
                }
                for (int LoopCount=0;LoopCount<4;LoopCount++)
                {
                    if (Mark[LoopCount].Mark_Error)
                        return;
                }
                if(COMMON.Frame.Unit[(int)Current_Unit].Sequence.Sequences == Enum.Sequence.Align)
                    COMMON.Frame.Unit[(int)Current_Unit].Sequence.Align_Sequence = Enum.Align_Sequence.Align;
                else if (COMMON.Frame.Unit[(int)Current_Unit].Sequence.Sequences == Enum.Sequence.Inspection)
                    COMMON.Frame.Unit[(int)Current_Unit].Sequence.Inspection_Sequence = Enum.Inspection_Sequence.Inspection;
            }
        }

        public void Recipe_Load()
        {
            Calibration_Data_Load();
            for (int LoopCount = 0; LoopCount < 6; LoopCount++)
            {
                Mark[LoopCount].Mark_Set(Current_Unit, Current_Type, (Enum.Mark)LoopCount);
            }
        }

        public void Calibration_Data_Save()
        {
            ClassINI.Write_Calibration_Data("Ratio", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + "Ratio", Ratio.ToString());
            ClassINI.Write_Calibration_Data("Camera Length", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + "Camera Length", Camera_Length.ToString());
            ClassINI.Write_Calibration_Data("Shuttle Length", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + "Shuttle Length", Shuttle_Length.ToString());
            for (int LoopCount = 0; LoopCount < 4; LoopCount++)
                ClassINI.Write_Calibration_Data("L Check", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + "L Check" + LoopCount.ToString(), L_Check_Spec[LoopCount].ToString());
            for (int LoopCount = 0; LoopCount < 3; LoopCount++)
                ClassINI.Write_Calibration_Data("Pitch", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + LoopCount.ToString(), Calibration_Pitch[LoopCount].ToString());
        }

        public void Calibration_Data_Load()
        {
            try
            {
                Ratio = double.Parse(ClassINI.Read_Calibration_Data("Ratio", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + "Ratio"));
                Camera_Length = double.Parse(ClassINI.Read_Calibration_Data("Camera Length", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + "Camera Length"));
                for (int LoopCount = 0; LoopCount < 4; LoopCount++)
                    L_Check_Spec[LoopCount] = double.Parse(ClassINI.Read_Calibration_Data("L Check", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + "L Check" + LoopCount.ToString()));
                for (int LoopCount = 0; LoopCount < 4; LoopCount++)
                    L_Check_Offset[LoopCount] = double.Parse(ClassINI.Read_Calibration_Data("L Check", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + "L Offset" + LoopCount.ToString()));
                L_Check_Limit = double.Parse(ClassINI.Read_Calibration_Data("L Check", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + "L Limit"));
            }
            catch { }
            try
            {
                for (int LoopCount = 0; LoopCount < 3; LoopCount++)
                    Calibration_Pitch[LoopCount] = double.Parse(ClassINI.Read_Calibration_Data("Pitch", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + LoopCount.ToString()));
            }
            catch { }
            try
            {
                Shuttle_Length = double.Parse(ClassINI.Read_Calibration_Data("Shuttle Length", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + "Shuttle Length"));
            }
            catch { }
        }

        public void Auto_Grab_Image(Enum.Position _Position)
        {
            for (int LoopCount = 0; LoopCount < 1; LoopCount++)
            {
                Parallel.For(0, 2, i =>
                {
#if CW_SAM
                    int Retry_Count = 0;
                    if(Recipe.Hole_Align_Use)
                    {
                        if (Recipe.Hole_Align_Position[(int)_Position + i])
                        {
                            Mark[i + (int)_Position].Last_Grab_Image = COMMON.Grab_Image[COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Cam[i]];
                            Mark[i + (int)_Position].Auto_Mark_Find();
                        }
                    }
                    else
                    {
                        //if (!(Current_Unit == Enum.Unit.Unit3 && Current_Type == Enum.Type.Target && _Position == Enum.Position.First && i == 0))
                        if (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[i+(int)_Position])
                        {
                            Retry:
                            Mark[i + (int)_Position].Last_Grab_Image = COMMON.Grab_Image[COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Cam[i]];
                            Mark[i + (int)_Position].Auto_Mark_Find();
                            if (Mark[i + (int)_Position].Mark_Error && Retry_Count<ETC_Option.Retry_Limit)
                            {
                                Retry_Count++;
                                goto Retry;
                            }
                        }
                        else
                        {
                            Mark[i + (int)_Position].Display.Image = null;
                            Mark[i + (int)_Position].X = 0;
                            Mark[i + (int)_Position].Y = 0;
                            //Mark[i + (int)_Position].Mark_Error = true;
                        }
                    }
#else
                    Mark[i + (int)_Position].Last_Grab_Image = COMMON.Grab_Image[COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Cam[i]];
                    Mark[i + (int)_Position].Auto_Mark_Find();
#endif
                });
            }
        }

        public int Calibration_Grab_Image(Enum.Position _Position)
        {
            DateTime Tic = DateTime.Now;
            while (true)
                if ((DateTime.Now - Tic).TotalMilliseconds > 800)
                    break;
            for (int LoopCount = 0; LoopCount < 1; LoopCount++)
            {
                Parallel.For(0, 2, i =>
                {
                    if (Recipe.Hole_Align_Use)
                    {
                        if (Recipe.Hole_Align_Position[(int)_Position + i])
                        {
                            Mark[i + (int)_Position].Last_Grab_Image = COMMON.Grab_Image[COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Cam[i]];
                            Mark[i + (int)_Position].Calibration_Mark_Find(COMMON.Frame.Calibration.Cal_Displays[i]);
                        }
                    }
                    else
                    {
                        if(COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[i+(int)_Position])
                        {
                            Mark[i + (int)_Position].Last_Grab_Image = COMMON.Grab_Image[COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Cam[i]];
                            Mark[i + (int)_Position].Calibration_Mark_Find(COMMON.Frame.Calibration.Cal_Displays[i]);
                        }
                    }
                });
                if (!Mark[0 + (int)_Position].Mark_Error && !Mark[1 + (int)_Position].Mark_Error)
                    return 0;
            }
            return 1;
        }

        public int Calibration_Grab_Image(Enum.Position _Position,int Number)
        {
            DateTime Tic = DateTime.Now;
            while(true)
            {
                if ((DateTime.Now - Tic).TotalMilliseconds > 500)
                    break;
            }
            Parallel.For(0, 2, i =>
            {
                if (Recipe.Hole_Align_Use)
                {
                    if (Recipe.Hole_Align_Position[(int)_Position])
                    {
                        Mark[i + (int)_Position].Last_Grab_Image = COMMON.Grab_Image[COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Cam[i]];
                        Mark[i + (int)_Position].Calibration_Mark_Find(COMMON.Frame.Calibration.Cal_Displays[i]);
                    }
                }
                else
                {
                    Mark[i + (int)_Position].Last_Grab_Image = COMMON.Grab_Image[COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Cam[i]];
                    Mark[i + (int)_Position].Calibration_Mark_Find(COMMON.Frame.Calibration.Cal_Displays[i]);
                }
            });
            if (!Mark[0 + (int)_Position].Mark_Error && !Mark[1 + (int)_Position].Mark_Error)
                return 0;
            return 1;
        }


        private int Hole_Align(ref double X, ref double Y, ref double T)
        {
            L_Check[0] = 0;
            L_Check[1] = 0;
            L_Check[2] = 0;
            L_Check[3] = 0;
            double Calculat_T = 0;
            if (Recipe.Hole_Align_Number<2)
            {
                L_Check[1] = Calculation_Length(Mark[2], Mark[3], "W");
                Calculat_T = Calculation_T(Mark[2], Mark[3], "W", L_Check[1]);
            }
            else
            {
                L_Check[0] = Calculation_Length(Mark[0], Mark[1], "W");
                Calculat_T = Calculation_T(Mark[0], Mark[1], "W", L_Check[0]);
            }
            if (Length_Check(L_Check)) // L체크 NG
                return 11;
            Result_T = Calculat_T;

            if (Mark[Recipe.Hole_Align_Number].X != 0)
            {
                PointF P = new PointF(0, 0);
                PointF C = new PointF(0, 0);
                PointF R = new PointF(0, 0);
                P.X = (float)Mark[Recipe.Hole_Align_Number].X;
                P.Y = (float)Mark[Recipe.Hole_Align_Number].Y;
                C.X = (float)Mark[Recipe.Hole_Align_Number].Rotate_Center_X;
                C.Y = (float)Mark[Recipe.Hole_Align_Number].Rotate_Center_Y;
                R = Rotate(P, C, Result_T);
                Result_X = (Mark[Recipe.Hole_Align_Number].Master_X - R.X) * Mark[Recipe.Hole_Align_Number].Resolution_X;
                Result_Y = (Mark[Recipe.Hole_Align_Number].Master_Y - R.Y) * Mark[Recipe.Hole_Align_Number].Resolution_Y;
            }
            else
            {
                Result_X = 0;
                Result_Y = 0;
            }
            if (Recipe.Ratio_Use)
                Result_T = Result_T * Ratio;
            X = Result_X / 1000;
            Y = Result_Y / 1000;
            T = Result_T;
            return 0;
        }

        public void Inspection_L_Check()
        {
            L_Check[0] = Calculation_Length(Mark[0], Mark[1], "W");
            L_Check[1] = Calculation_Length(Mark[2], Mark[3], "W");
            L_Check[2] = Calculation_Length(Mark[0], Mark[2], "H");
            L_Check[3] = Calculation_Length(Mark[1], Mark[3], "H");
        }

        public int Align_Start(ref double X, ref double Y, ref double T)
        {
            int Align_Point_Check = 0;
            int Mark_NG_Check = 0;
            if (Recipe.OK_PASS || Recipe.NG_PASS)
            {
                X = 0.0001; Y = 0.0001; T = 0.0001;
                return 0;
            }
            for (int LoopCount = 0; LoopCount < 4; LoopCount++)
                if (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[LoopCount])
                    Align_Point_Check++;
            for (int LoopCount = 0; LoopCount < 4; LoopCount++)
                if (Mark[LoopCount].Mark_Error)
                    Mark_NG_Check++;
#if CW_SAM
            if (Recipe.Hole_Align_Use && Current_Unit == Enum.Unit.Unit1)
            {
                if (Mark_NG_Check > 0)
                    return 2;
                 return Hole_Align(ref X, ref Y, ref T);
            }
            //if (Current_Unit == Enum.Unit.Unit3 && Current_Type == Enum.Type.Target)
            //    if (Mark_NG_Check > 1)
            //        return 2;
            //    else
            //        goto OK;
#endif
            if ((Recipe.N3_Point_Align && Align_Point_Check == 4 && Mark_NG_Check > 1) || (!Recipe.N3_Point_Align && Align_Point_Check == 4 && Mark_NG_Check > 0) || (Align_Point_Check != 4 && Mark_NG_Check > 0))
                return 2; // 마크NG
            OK:
            if (Align_Point_Check == 4 && Mark_NG_Check == 0)
            {//4Point
                L_Check[0] = Calculation_Length(Mark[0], Mark[1], "W");
                L_Check[1] = Calculation_Length(Mark[2], Mark[3], "W");
                L_Check[2] = Calculation_Length(Mark[0], Mark[2], "H");
                L_Check[3] = Calculation_Length(Mark[1], Mark[3], "H");
                if (Length_Check(L_Check)) // L체크 NG
                    return 11;
                double[] Calculat_T = { 0, 0, 0, 0 };
                switch (ETC_Option.Mode)
                {
                    case 1:
                        ////////////////////////////////////////////////////////////////////////////
                        // 쓸위치만 주석해제
                        //Result_T = Calculation_T(Mark[0], Mark[1], "W", L_Check[0]);
                        //Result_T = Calculation_T(Mark[2], Mark[3], "W", L_Check[1]);
                        //Result_T = Calculation_T(Mark[0], Mark[2], "H", L_Check[2]);
                        Result_T = Calculation_T(Mark[1], Mark[3], "H", L_Check[3]);
                        ////////////////////////////////////////////////////////////////////////////
                        break;
                    case 2:
                        Array.Resize(ref Calculat_T, 2);
                        if(L_Check[0]>L_Check[2])
                        {
                            Calculat_T[0] = Calculation_T(Mark[0], Mark[1], "W", L_Check[0]);
                            Calculat_T[1] = Calculation_T(Mark[2], Mark[3], "W", L_Check[1]);
                        }
                        else
                        {
                            Calculat_T[0] = Calculation_T(Mark[2], Mark[0], "H", L_Check[2]);
                            Calculat_T[1] = Calculation_T(Mark[3], Mark[1], "H", L_Check[3]);
                        }
                        Result_T = Calculat_T.Average();
                        break;
                    default:
                        Calculat_T[0] = Calculation_T(Mark[0], Mark[1], "W", L_Check[0]);
                        Calculat_T[1] = Calculation_T(Mark[2], Mark[3], "W", L_Check[1]);
                        Calculat_T[2] = Calculation_T(Mark[2], Mark[0], "H", L_Check[2]);
                        Calculat_T[3] = Calculation_T(Mark[3], Mark[1], "H", L_Check[3]);
                        Result_T = Calculat_T.Average();
                        break;
                }
                Calculation_XY(ref Result_X, ref Result_Y, 4);
                if (Recipe.Ratio_Use)
                    Result_T = Result_T * Ratio;
                X = Result_X / 1000; Y = Result_Y / 1000; T = Result_T;
#if CW_SAM
                if (Current_Unit == Enum.Unit.Unit2 || Current_Unit == Enum.Unit.Unit3)
                {
                    P_Result_T = T;
                    L_Result_T = T;
                    P_Result_X /= 1000;
                    P_Result_Y /= 1000;
                    L_Result_X /= 1000;
                    L_Result_Y /= 1000;
                }
#endif
                return 0;
            }
            else if (Align_Point_Check == 3 || (Align_Point_Check == 4 && Mark_NG_Check == 1))
            {//3Point
                L_Check[0] = 0;
                L_Check[1] = 0;
                L_Check[2] = 0;
                L_Check[3] = 0;
                if (Mark[0].X == 0) //우하
                {
                    L_Check[1] = Calculation_Length(Mark[2], Mark[3], "W");
                    L_Check[3] = Calculation_Length(Mark[1], Mark[3], "H");
                    if (L_Check[1] > L_Check[3])
                        Result_T = Calculation_T(Mark[2], Mark[3], "W", L_Check[1]);
                    else
                        Result_T = Calculation_T(Mark[3], Mark[1], "H", L_Check[3]);
                }
                else if (Mark[1].X == 0)//좌하
                {
                    L_Check[1] = Calculation_Length(Mark[2], Mark[3], "W");
                    L_Check[2] = Calculation_Length(Mark[0], Mark[2], "H");
                    if (L_Check[1] > L_Check[2])
                        Result_T = Calculation_T(Mark[2], Mark[3], "W", L_Check[1]);
                    else
                        Result_T = Calculation_T(Mark[2], Mark[0], "H", L_Check[2]);
                }
                else if (Mark[2].X == 0)//우상
                {
                    L_Check[0] = Calculation_Length(Mark[0], Mark[1], "W");
                    L_Check[3] = Calculation_Length(Mark[1], Mark[3], "H");
                    if (L_Check[0] > L_Check[3])
                        Result_T = Calculation_T(Mark[0], Mark[1], "W", L_Check[0]);
                    else
                        Result_T = Calculation_T(Mark[3], Mark[1], "H", L_Check[3]);
                }
                else if (Mark[3].X == 0)//좌상
                {
                    L_Check[0] = Calculation_Length(Mark[0], Mark[1], "W");
                    L_Check[2] = Calculation_Length(Mark[0], Mark[2], "H");
                    if (L_Check[0] > L_Check[2])
                        Result_T = Calculation_T(Mark[0], Mark[1], "W", L_Check[0]);
                    else
                        Result_T = Calculation_T(Mark[2], Mark[0], "H", L_Check[2]);
                }
                if (Length_Check(L_Check))
                    return 11;
                Calculation_XY(ref Result_X, ref Result_Y, 3);
                if (Recipe.Ratio_Use)
                    Result_T = Result_T * Ratio;
                X = Result_X / 1000; Y = Result_Y / 1000; T = Result_T;
#if CW_SAM
                if (Current_Unit == Enum.Unit.Unit2 || Current_Unit == Enum.Unit.Unit3)
                {
                    P_Result_T = T;
                    L_Result_T = T;
                    P_Result_X /= 1000;
                    P_Result_Y /= 1000;
                    L_Result_X /= 1000;
                    L_Result_Y /= 1000;
                }
#endif
                return 0;
            }
            else if (Align_Point_Check == 2)
            {//2Point
                L_Check[0] = 0;
                L_Check[1] = 0;
                L_Check[2] = 0;
                L_Check[3] = 0;
                if (Mark[0].X == 0 && Mark[1].X == 0) // 하
                {
                    L_Check[1] = Calculation_Length(Mark[2], Mark[3], "W");
                    Result_T = Calculation_T(Mark[2], Mark[3], "W", L_Check[1]);
                }
                else if (Mark[2].X == 0 && Mark[3].X == 0) // 상
                {
                    L_Check[0] = Calculation_Length(Mark[0], Mark[1], "W");
                    Result_T = Calculation_T(Mark[0], Mark[1], "W", L_Check[0]);
                }
                else if (Mark[0].X == 0 && Mark[2].X == 0) // 우
                {
                    L_Check[3] = Calculation_Length(Mark[1], Mark[3], "H");
                    Result_T = Calculation_T(Mark[3], Mark[1], "H", L_Check[3]);
                }
                else if (Mark[1].X == 0 && Mark[3].X == 0) // 좌
                {
                    L_Check[2] = Calculation_Length(Mark[0], Mark[2], "H");
                    Result_T = Calculation_T(Mark[2], Mark[0], "H", L_Check[2]);
                }
                if (Length_Check(L_Check))
                    return 11;
                Calculation_XY(ref Result_X, ref Result_Y, 2);
                if (Recipe.Ratio_Use)
                    Result_T = Result_T * Ratio;
                X = Result_X / 1000; Y = Result_Y / 1000; T = Result_T;
#if CW_SAM
                if (Current_Unit == Enum.Unit.Unit2 || Current_Unit == Enum.Unit.Unit3)
                {
                    P_Result_T = T;
                    L_Result_T = T;
                    P_Result_X /= 1000;
                    P_Result_Y /= 1000;
                    L_Result_X /= 1000;
                    L_Result_Y /= 1000;
                }
#endif
                return 0;
            }
            return -1; // 알수 없는 경우
        }

        private PointF Rotate(PointF sourcePoint, PointF centerPoint, double rotateAngle)
        {
            Point targetPoint = new Point();

            double radian = rotateAngle / 180 * Math.PI;

            targetPoint.X = (int)(Math.Cos(radian) * (sourcePoint.X - centerPoint.X) - Math.Sin(radian) * (sourcePoint.Y - centerPoint.Y) + centerPoint.X);
            targetPoint.Y = (int)(Math.Sin(radian) * (sourcePoint.X - centerPoint.X) + Math.Cos(radian) * (sourcePoint.Y - centerPoint.Y) + centerPoint.Y);

            return targetPoint;
        }

        private bool Length_Check(double[] Length)
        {
            if (Recipe.L_Check_Use)
                for (int LoopCount = 0; LoopCount < 4; LoopCount++)
                {
                    if (Length[LoopCount] != 0)
                    {
                        if (Math.Abs(Length[LoopCount] / 1000 - L_Check_Spec[LoopCount]) > L_Check_Limit)
                            return true;
                    }
                }
            return false;
        }

        private void Calculation_XY(ref double X, ref double Y,int count)
        {
            PointF P = new PointF(0, 0);
            PointF C = new PointF(0, 0);
            PointF[] R = new PointF[4];
            double[] Calculat_X = new double[4];
            double[] Calculat_Y = new double[4];
            for (int LoopCount = 0; LoopCount < 4; LoopCount++)
            {
                if (Mark[LoopCount].X != 0)
                {
                    P.X = (float)Mark[LoopCount].X;
                    P.Y = (float)Mark[LoopCount].Y;
                    C.X = (float)Mark[LoopCount].Rotate_Center_X;
                    C.Y = (float)Mark[LoopCount].Rotate_Center_Y;
                    R[LoopCount] = Rotate(P, C, Result_T);
                    Calculat_X[LoopCount] = (Mark[LoopCount].Master_X - R[LoopCount].X) * Mark[LoopCount].Resolution_X;
                    Calculat_Y[LoopCount] = (Mark[LoopCount].Master_Y - R[LoopCount].Y) * Mark[LoopCount].Resolution_Y;
                }
                else
                {
                    Calculat_X[LoopCount] = 0;
                    Calculat_Y[LoopCount] = 0;
                }
            }
            X = Calculat_X.Sum() / count;
            Y = Calculat_Y.Sum() / count;
#if CW_SAM
            if (Current_Unit == Enum.Unit.Unit2 || Current_Unit == Enum.Unit.Unit3)
            {
                PointF[] M = new PointF[4];
                for (int LoopCount = 0; LoopCount < 4; LoopCount++)
                {
                    if (Mark[LoopCount].X != 0)
                    {
                        P.X = (float)Mark[LoopCount].X;
                        P.Y = (float)Mark[LoopCount].Y;
                        C.X = (float)Mark[LoopCount].Rotate_Center_X;
                        C.Y = (float)Mark[LoopCount].Rotate_Center_Y;
                        double Teaching_Rotate;
                        if (Current_Unit == Enum.Unit.Unit3)
                            Teaching_Rotate = 90 + ((double)(COMMON.Receive_Data[((int)Current_Unit - 1) * 30 + 22] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 23] << 16)) / 10000);
                        else
                            Teaching_Rotate = (double)(COMMON.Receive_Data[((int)Current_Unit - 1) * 30 + 22] | (COMMON.Receive_Data[(((int)Current_Unit - 1) * 30) + 23] << 16)) / 10000;
                        M[LoopCount] = Rotate(new PointF((float)Mark[LoopCount].Master_X, (float)Mark[LoopCount].Master_Y), C, Teaching_Rotate);
                        R[LoopCount] = Rotate(P, C, Result_T + Teaching_Rotate);
                        Calculat_X[LoopCount] = (M[LoopCount].X - R[LoopCount].X) * Mark[LoopCount].Resolution_X;
                        Calculat_Y[LoopCount] = (M[LoopCount].Y - R[LoopCount].Y) * Mark[LoopCount].Resolution_Y;


                    }
                    else
                    {
                        Calculat_X[LoopCount] = 0;
                        Calculat_Y[LoopCount] = 0;
                    }
                }
                P_Result_X = Calculat_X.Sum() / count;
                P_Result_Y = Calculat_Y.Sum() / count;

                for (int LoopCount = 0; LoopCount < 4; LoopCount++)
                {
                    if (Mark[LoopCount].X != 0)
                    {
                        P.X = (float)Mark[LoopCount].X;
                        P.Y = (float)Mark[LoopCount].Y;
                        C.X = (float)Mark[LoopCount].Rotate_Center_X;
                        C.Y = (float)Mark[LoopCount].Rotate_Center_Y;
                        double Teaching_Rotate = 90;
                        M[LoopCount] = Rotate(new PointF((float)Mark[LoopCount].Master_X, (float)Mark[LoopCount].Master_Y), C, Teaching_Rotate);
                        R[LoopCount] = Rotate(P, C, Result_T + Teaching_Rotate);
                        Calculat_X[LoopCount] = (M[LoopCount].X - R[LoopCount].X) * Mark[LoopCount].Resolution_X;
                        Calculat_Y[LoopCount] = (M[LoopCount].Y - R[LoopCount].Y) * Mark[LoopCount].Resolution_Y;


                    }
                    else
                    {
                        Calculat_X[LoopCount] = 0;
                        Calculat_Y[LoopCount] = 0;
                    }
                }
                L_Result_X = Calculat_X.Sum() / count;
                L_Result_Y = Calculat_Y.Sum() / count;
            }
#endif
        }

        private double Calculation_T(ClassMark Mark1, ClassMark Mark2, string Angle, double Length)
        {
            double T = 0;
            if (Angle == "W")
            {
                double H = ((Mark1.Master_Y - Mark1.Y) * Mark1.Resolution_Y) - ((Mark2.Master_Y - Mark2.Y) * Mark2.Resolution_Y);
                T = Math.Asin(H / Length) * 180 / Math.PI;
            }
            else if (Angle == "H")
            {
                double W = ((Mark1.Master_X - Mark1.X) * Mark1.Resolution_X) - ((Mark2.Master_X - Mark2.X) * Mark2.Resolution_X);
                T = Math.Acos(W / Length) * 180 / Math.PI - 90;
            }
            return T;
        }

        private double Calculation_Length(ClassMark Mark1, ClassMark Mark2,string Angle)
        {
            double Length = 0;
            if (Angle == "W")
            {
                double W = ((Mark1.Rotate_Center_X - Mark1.X) * Mark1.Resolution_X) + ((Math.Abs(Mark2.Rotate_Center_X) + Mark2.X) * Mark2.Resolution_X);
                double H = (Mark1.Y - Mark2.Y) * ((Mark1.Resolution_Y + Mark2.Resolution_Y) / 2);
                Length = Math.Sqrt(Math.Pow(W, 2) + Math.Pow(H, 2));
            }
            else if(Angle == "H")
            {
                if(ETC_Option.Shuttle_Position_Use)
                {
                    double W = (Mark1.X - Mark2.X) * ((Mark1.Resolution_X + Mark2.Resolution_X) / 2);
                    double H = ((Mark1.Last_Grab_Image.Height - Mark1.Y) * Mark1.Resolution_Y) + (Mark2.Y * Mark2.Resolution_Y) + Math.Abs(((Shuttle_Y[0] + CAM_Y[0]) - (Shuttle_Y[2] + CAM_Y[2])) * 1000) - (Mark1.Last_Grab_Image.Height * Mark1.Resolution_Y);
                    Length = Math.Sqrt(Math.Pow(W, 2) + Math.Pow(H, 2));
                }
                else
                {
                    double W = (Mark1.X - Mark2.X) * ((Mark1.Resolution_X + Mark2.Resolution_X) / 2);
                    double H = ((Mark1.Rotate_Center_Y - Mark1.Y) * Mark1.Resolution_Y) - ((Mark2.Rotate_Center_Y - Mark2.Y) * Mark2.Resolution_Y);
                    Length = Math.Sqrt(Math.Pow(W, 2) + Math.Pow(H, 2));
                }

            }
            return Length;
        }

        public void Calibration_Calculation()
        {
            int number = 0;
            L_Check_Spec[0] = 0;
            L_Check_Spec[1] = 0;
            L_Check_Spec[2] = 0;
            L_Check_Spec[3] = 0;
            double Calculation_X = 0;
            double Calculation_Y = 0;
            if (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[0] && COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[1])
            {
                Calculation_X = ((Mark[number].Rotate_Center_X - Mark[number].Calibration_Point_X[4]) * Mark[number].Resolution_X) +
                    ((Mark[number + 1].Calibration_Point_X[4] + Math.Abs(Mark[number + 1].Rotate_Center_X)) * Mark[number + 1].Resolution_X);
                Calculation_Y = (Mark[number].Calibration_Point_Y[4] - Mark[number + 1].Calibration_Point_Y[4]) * ((Mark[number].Resolution_Y + Mark[number + 1].Resolution_Y) / 2);
                L_Check_Spec[0] = (Math.Sqrt(Math.Pow(Calculation_X, 2) + Math.Pow(Calculation_Y, 2)) / 1000);
            }
            if (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[2] && COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[3])
            {
                number = 2;
                Calculation_X = ((Mark[number].Rotate_Center_X - Mark[number].Calibration_Point_X[4]) * Mark[number].Resolution_X) +
                        ((Mark[number + 1].Calibration_Point_X[4] + Math.Abs(Mark[number + 1].Rotate_Center_X)) * Mark[number + 1].Resolution_X);
                Calculation_Y = (Mark[number].Calibration_Point_Y[4] - Mark[number + 1].Calibration_Point_Y[4]) * ((Mark[number].Resolution_Y + Mark[number + 1].Resolution_Y) / 2);
                L_Check_Spec[1] = (Math.Sqrt(Math.Pow(Calculation_X, 2) + Math.Pow(Calculation_Y, 2)) / 1000);
            }


            if(ETC_Option.Shuttle_Position_Use)
            {
                if (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[0] && COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[2])
                {
                    number = 0;
                    Calculation_Y = ((Mark[number].Last_Grab_Image.Height - Mark[number].Calibration_Point_Y[4]) * Mark[number].Resolution_Y) +
                                    ((Mark[number + 2].Last_Grab_Image.Height - Mark[number + 2].Calibration_Point_Y[4]) * Mark[number + 2].Resolution_Y) +
                                    Math.Abs(((Shuttle_Y[0] + CAM_Y[0]) - (Shuttle_Y[2] + CAM_Y[2])) * 1000) - (Mark[number].Last_Grab_Image.Height * Mark[number].Resolution_Y);
                    Calculation_X = (Mark[number].Calibration_Point_X[4] - Mark[number + 2].Calibration_Point_X[4]) * ((Mark[number].Resolution_X + Mark[number + 2].Resolution_X) / 2);
                    L_Check_Spec[2] = (Math.Sqrt(Math.Pow(Calculation_X, 2) + Math.Pow(Calculation_Y, 2)) / 1000);
                }
                if (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[1] && COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[3])
                {
                    number = 1;
                    Calculation_Y = ((Mark[number].Last_Grab_Image.Height - Mark[number].Calibration_Point_Y[4]) * Mark[number].Resolution_Y) +
                                    ((Mark[number + 2].Last_Grab_Image.Height - Mark[number + 2].Calibration_Point_Y[4]) * Mark[number + 2].Resolution_Y) +
                                    Math.Abs(((Shuttle_Y[0] + CAM_Y[0]) - (Shuttle_Y[2] + CAM_Y[2])) * 1000) - (Mark[number].Last_Grab_Image.Height * Mark[number].Resolution_Y);
                    Calculation_X = (Mark[number].Calibration_Point_X[4] - Mark[number + 2].Calibration_Point_X[4]) * ((Mark[number].Resolution_X + Mark[number + 2].Resolution_X) / 2);
                    L_Check_Spec[3] = (Math.Sqrt(Math.Pow(Calculation_X, 2) + Math.Pow(Calculation_Y, 2)) / 1000);
                }
            }
            else
            {
                if (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[0] && COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[2])
                {
                    number = 0;
                    Calculation_Y = ((Mark[number].Rotate_Center_Y - Mark[number].Calibration_Point_Y[4]) * Mark[number].Resolution_Y) -
                                       ((Mark[number + 2].Rotate_Center_Y - Mark[number + 2].Calibration_Point_Y[4]) * Mark[number + 2].Resolution_Y);
                    Calculation_X = (Mark[number].Calibration_Point_X[4] - Mark[number + 2].Calibration_Point_X[4]) * ((Mark[number].Resolution_X + Mark[number + 2].Resolution_X) / 2);
                    L_Check_Spec[2] = (Math.Sqrt(Math.Pow(Calculation_X, 2) + Math.Pow(Calculation_Y, 2)) / 1000);
                }
                if (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[1] && COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position[3])
                {
                    number = 1;
                    Calculation_Y = ((Mark[number].Rotate_Center_Y - Mark[number].Calibration_Point_Y[4]) * Mark[number].Resolution_Y) -
                                        ((Mark[number + 2].Rotate_Center_Y - Mark[number + 2].Calibration_Point_Y[4]) * Mark[number + 2].Resolution_Y);
                    Calculation_X = (Mark[number].Calibration_Point_X[4] - Mark[number + 2].Calibration_Point_X[4]) * ((Mark[number].Resolution_X + Mark[number + 2].Resolution_X) / 2);
                    L_Check_Spec[3] = (Math.Sqrt(Math.Pow(Calculation_X, 2) + Math.Pow(Calculation_Y, 2)) / 1000);
                }
            }
        }

        public void Calibration_Copy(Enum.Type Target_Type)
        {
            Ratio = COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Target_Type].Ratio;
            Camera_Length = COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Target_Type].Camera_Length;
            L_Check_Spec = COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Target_Type].L_Check_Spec;
        }

        public void Master_Position_Save(double[] _L_Check_Spec, double[] _L_Check_Offset, double[] _X, double[] _Y,double _L_Check_Limit)
        {
            L_Check_Spec = _L_Check_Spec;
            L_Check_Offset = _L_Check_Offset;
            L_Check_Limit = _L_Check_Limit;
            for (int LoopCount = 0; LoopCount < 4; LoopCount++)
            {
                if(L_Check_Spec[LoopCount] != 0)
                {
                    ClassINI.Write_Calibration_Data("L Check", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + "L Check" + LoopCount.ToString(), L_Check_Spec[LoopCount].ToString());
                    ClassINI.Write_Calibration_Data("L Check", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + "L Offset" + LoopCount.ToString(), L_Check_Offset[LoopCount].ToString());
                }
                Mark[LoopCount].Master_Position_Save(_X[LoopCount], _Y[LoopCount]);
            }
            ClassINI.Write_Calibration_Data("L Check", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + "L Limit", L_Check_Limit.ToString());
        }
    }
}
