using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.Dimensioning;
using Cognex.VisionPro.CNLSearch;

namespace T_Align
{
    public class ClassMark
    {
        public ICogImage Last_Grab_Image;

        public CogPMAlignTool[] Multi_Mark = new CogPMAlignTool[6];
        public double Circle_R_Spec = 0;
        public double[] Mark_Match_Rate = { 0.7, 0.7, 0.7, 0.7, 0.7, 0.7 };
        public double Pattern_Match_Rate = 0.7;
        public double Circle_Radius_Limit = 0;
        public double Line_Angle_Limit = 0;
        public CogFindLineTool Line_W = new CogFindLineTool();
        public CogFindLineTool Line_H = new CogFindLineTool();
        public CogFindCircleTool Circle = new CogFindCircleTool();
        public CogCNLSearchTool CNL = new CogCNLSearchTool();
        CogFitCircleTool CogFitCircle = new CogFitCircleTool();
        Enum.Unit Current_Unit = Enum.Unit.Common;
        Enum.Type Current_Type = Enum.Type.Common;
        Enum.Mark Current_Mark = Enum.Mark.MARK1;
        Enum.Mark_Type Current_Mark_Type = Enum.Mark_Type.MARK;

        public bool Mark_Error = false;

        public double Resolution_X = 0.001;
        public double Resolution_Y = 0.001;
        public double Master_X = 0;
        public double Master_Y = 0;
        public double Rotate_Center_X = 0;
        public double Rotate_Center_Y = 0;
        public double[] Calibration_Point_X = new double[17];
        public double[] Calibration_Point_Y = new double[17];
        public int Camera_Width = 0;
        public int Camera_Height = 0;

        public double X = 0;
        public double Y = 0;
        public double S = 0;
        public double AR = 0;

        public CogDisplay Display;

        public int Last_Mark_Index = 0;

        private object Line_Lock = new object();
        private object Circle_Lock = new object();

        public void Calibration_Data_Set(int Point_Number)
        {
            Calibration_Point_X[Point_Number] = X;
            Calibration_Point_Y[Point_Number] = Y;
        }

        public void Calibration_Calculation(double Pitch_X, double Pitch_Y)
        {
            if (ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._9T)
            {
                double[] Resol_X = new double[3];
                double[] Resol_Y = new double[3];
                Resol_X[0] = (Pitch_X * 2000) / (Calibration_Point_X[0] - Calibration_Point_X[2]);
                Resol_X[1] = (Pitch_X * 2000) / (Calibration_Point_X[3] - Calibration_Point_X[5]);
                Resol_X[2] = (Pitch_X * 2000) / (Calibration_Point_X[6] - Calibration_Point_X[8]);
                Resol_Y[0] = (Pitch_Y * 2000) / (Calibration_Point_Y[0] - Calibration_Point_Y[6]);
                Resol_Y[1] = (Pitch_Y * 2000) / (Calibration_Point_Y[1] - Calibration_Point_Y[7]);
                Resol_Y[2] = (Pitch_Y * 2000) / (Calibration_Point_Y[2] - Calibration_Point_Y[8]);
                Resolution_X = Math.Abs(Resol_X.Average());
                Resolution_Y = Math.Abs(Resol_Y.Average());
                Master_X = Calibration_Point_X[4];
                Master_Y = Calibration_Point_Y[4];
            }
            else if (ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._3X || ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._3XT)
            {
                Resolution_X = (Pitch_X * 2000) / (Calibration_Point_X[3] - Calibration_Point_X[5]);
                Resolution_Y = (Pitch_X * 2000) / (Calibration_Point_X[3] - Calibration_Point_X[5]);
                Master_X = Calibration_Point_X[4];
                Master_Y = Calibration_Point_Y[4];
            }
            else if (ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._3Y || ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._3YT)
            {
                Resolution_X = (Pitch_Y * 2000) / (Calibration_Point_Y[0] - Calibration_Point_Y[6]);
                Resolution_Y = (Pitch_Y * 2000) / (Calibration_Point_Y[0] - Calibration_Point_Y[6]);
                Master_X = Calibration_Point_X[3];
                Master_Y = Calibration_Point_Y[3];
            }
            Camera_Width = Last_Grab_Image.Width;
            Camera_Height = Last_Grab_Image.Height;
        }

        public void Calibration_Theta(double Pitch_T)
        {
            if (ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._9T || ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._3XT || ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._3YT)
            {
                double[] R_X = new double[21];
                double[] R_Y = new double[21];
                int Count = 0;
                for (int LoopCount = 9; LoopCount < 16; LoopCount++)
                {
                    double Theta_Count = 1;
                    for (int LoopCount2 = LoopCount + 1; LoopCount2 < 16; LoopCount2++)
                    {
                        Rotate_Center(Calibration_Point_X[LoopCount], Calibration_Point_Y[LoopCount], Calibration_Point_X[LoopCount2], Calibration_Point_Y[LoopCount2], Theta_Count * Pitch_T, ref R_X[Count], ref R_Y[Count]);
                        Count++;
                        Theta_Count++;
                    }
                }
                Rotate_Center_X = R_X.Average();
                Rotate_Center_Y = R_Y.Average();
            }
            else if (ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._3X || ETC_Option.Calibration_Type[(int)Current_Type, (int)Current_Unit] == Enum.Calibration_Type._3Y)
            {
                Rotate_Center_X = 0;
                Rotate_Center_Y = 0;
            }
        }

        private void Load_Limit()
        {
            for(int LoopCount=0;LoopCount<6;LoopCount++)
            {
                try
                {
                    Mark_Match_Rate[LoopCount] = double.Parse(ClassINI.ReadConfig("Mark Limit", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_" + LoopCount.ToString()));
                }
                catch { }
            }
            try
            {
                Line_Angle_Limit = double.Parse(ClassINI.ReadConfig("Mark Limit", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "Line Angle"));
            }
            catch { }
            try
            {
                Circle_Radius_Limit = double.Parse(ClassINI.ReadConfig("Mark Limit", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "Circle Radius"));
            }
            catch { }
            try
            {
                Circle_R_Spec = double.Parse(ClassINI.ReadConfig("Mark Limit", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "Circle Radius Spec"));
            }
            catch { }
            try
            {
                Pattern_Match_Rate = double.Parse(ClassINI.ReadConfig("Mark Limit", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "Pattern Match"));
            }
            catch { }
        }

        public void Mark_Limit_Save(int Multimark_Number, double Rate)
        {
            Mark_Match_Rate[Multimark_Number] = Rate;
            ClassINI.WriteConfig("Mark Limit", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_" + Multimark_Number.ToString(), Mark_Match_Rate[Multimark_Number].ToString());
        }

        public void Line_Limit_Save(double Angle)
        {
            Line_Angle_Limit = Angle;
            ClassINI.WriteConfig("Mark Limit", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "Line Angle", Line_Angle_Limit.ToString());
        }

        public void Circle_Limit_Save(double Radius)
        {
            Circle_Radius_Limit = Radius;
            ClassINI.WriteConfig("Mark Limit", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "Circle Radius", Circle_Radius_Limit.ToString());
        }

        public void Circle_R_Save(double Radius)
        {
            Circle_R_Spec = Radius;
            ClassINI.WriteConfig("Mark Limit", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "Circle Radius Spec", Circle_Radius_Limit.ToString());
        }

        public void Pattern_Limit_Save(double Rate)
        {
            Pattern_Match_Rate = Rate;
            ClassINI.WriteConfig("Mark Limit", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "Pattern Match", Pattern_Match_Rate.ToString());
        }

        public void Master_Position_Save(double _X, double _Y)
        {
            Master_X = _X;
            Master_Y = _Y;
            ClassINI.Write_Calibration_Data("Master", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_X", Master_X.ToString());
            ClassINI.Write_Calibration_Data("Master", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_Y", Master_Y.ToString());
        }

        public void Calibration_Data_Save()
        {
            ClassINI.Write_Calibration_Data("Resolution", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_X", Resolution_X.ToString());
            ClassINI.Write_Calibration_Data("Resolution", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_Y", Resolution_Y.ToString());
            ClassINI.Write_Calibration_Data("Master", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_X", Master_X.ToString());
            ClassINI.Write_Calibration_Data("Master", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_Y", Master_Y.ToString());
            ClassINI.Write_Calibration_Data("Rotate_Center", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_X", Rotate_Center_X.ToString());
            ClassINI.Write_Calibration_Data("Rotate_Center", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_Y", Rotate_Center_Y.ToString());
            ClassINI.Write_Calibration_Data("Size", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_X", Camera_Width.ToString());
            ClassINI.Write_Calibration_Data("Size", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_Y", Camera_Height.ToString());
            for (int LoopCount = 0; LoopCount < 16; LoopCount++)
            {
                ClassINI.Write_Calibration_Data(((int)Current_Unit * 10 + ((int)Current_Type * 2) + (int)Current_Mark).ToString(),
                    "X_" + LoopCount.ToString(), Calibration_Point_X[LoopCount].ToString());
                ClassINI.Write_Calibration_Data(((int)Current_Unit * 10 + ((int)Current_Type * 2) + (int)Current_Mark).ToString(),
                    "Y_" + LoopCount.ToString(), Calibration_Point_Y[LoopCount].ToString());
            }
        }

        private void Calibration_Data_Load()
        {
            try
            {
                Resolution_X = double.Parse(ClassINI.Read_Calibration_Data("Resolution", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_X"));
                Resolution_Y = double.Parse(ClassINI.Read_Calibration_Data("Resolution", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_Y"));
                Master_X = double.Parse(ClassINI.Read_Calibration_Data("Master", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_X"));
                Master_Y = double.Parse(ClassINI.Read_Calibration_Data("Master", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_Y"));
                Rotate_Center_X = double.Parse(ClassINI.Read_Calibration_Data("Rotate_Center", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_X"));
                Rotate_Center_Y = double.Parse(ClassINI.Read_Calibration_Data("Rotate_Center", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_Y"));
                Camera_Width = int.Parse(ClassINI.Read_Calibration_Data("Size", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_X"));
                Camera_Height = int.Parse(ClassINI.Read_Calibration_Data("Size", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_Y"));
                for (int LoopCount = 0; LoopCount < 16; LoopCount++)
                {
                    Calibration_Point_X[LoopCount] = double.Parse(ClassINI.Read_Calibration_Data(((int)Current_Unit * 10 + ((int)Current_Type * 2) + (int)Current_Mark).ToString(),
                        "X_" + LoopCount.ToString()));
                    Calibration_Point_Y[LoopCount] = double.Parse(ClassINI.Read_Calibration_Data(((int)Current_Unit * 10 + ((int)Current_Type * 2) + (int)Current_Mark).ToString(),
                        "Y_" + LoopCount.ToString()));
                }
            }
            catch { }
        }

        private void Rotate_Center(double _X, double _Y, double _X1, double _Y1, double _Angle, ref double _Rotate_Center_X, ref double _Rotate_Center_Y)
        {
            double PointX = (_X + _X1) / 2;
            double PointY = (_Y + _Y1) / 2;

            double X_Distance = Math.Sqrt(Math.Pow(_X - _X1, 2) + Math.Pow(_Y - _Y1, 2)) / 2;
            double T = (_Y - _Y1) / (_X - _X1);
            double T2 = -1 / T;
            double Atan = 90 + Math.Atan(T2) * 180 / Math.PI;
            double Y_Distance = X_Distance / Math.Tan(deg_TO_Radian(_Angle));
            if (_X1 - _X > 0)
            {
                if (_Y1 - _Y > 0)
                {
                    _Rotate_Center_X = PointX - Math.Abs(Y_Distance * Math.Sin(Atan * Math.PI / 180)); //우
                    _Rotate_Center_Y = PointY + Math.Abs(Y_Distance * Math.Cos(Atan * Math.PI / 180)); //상
                }
                else
                {
                    _Rotate_Center_X = PointX + Math.Abs(Y_Distance * Math.Sin(Atan * Math.PI / 180)); //좌
                    _Rotate_Center_Y = PointY + Math.Abs(Y_Distance * Math.Cos(Atan * Math.PI / 180)); //상
                }
            }
            else
            {
                if (_Y1 - _Y > 0)
                {
                    _Rotate_Center_X = PointX - Math.Abs(Y_Distance * Math.Sin(Atan * Math.PI / 180)); //우
                    _Rotate_Center_Y = PointY - Math.Abs(Y_Distance * Math.Cos(Atan * Math.PI / 180)); //하
                }
                else
                {
                    _Rotate_Center_X = PointX + Math.Abs(Y_Distance * Math.Sin(Atan * Math.PI / 180)); //좌
                    _Rotate_Center_Y = PointY - Math.Abs(Y_Distance * Math.Cos(Atan * Math.PI / 180)); //하
                }
            }
        }

        private double deg_TO_Radian(double T)
        {
            return T * Math.PI / 180;
        }

        public void DIsplay_Set(CogDisplay _Display)
        {
            Display = _Display;
        }

        private void Find_NG()
        {
            if (COMMON.Current_Mode == Enum.Mode.Auto || COMMON.Current_Mode == Enum.Mode.Simul)
            {
                try
                {
                    X = 0;
                    Y = 0;
                    Mark_Error = true;
                    CogRectangle Error_Rect = new CogRectangle();
                    Error_Rect.SetXYWidthHeight(0, 0, Display.Image.Width, Display.Image.Height);
                    Error_Rect.LineWidthInScreenPixels = 20;
                    Error_Rect.Color = CogColorConstants.Red;
                    Display.InteractiveGraphics.Add(Error_Rect, "E", false);
                }
                catch { }
            }
        }

        public void Auto_Mark_Find()
        {
            X = Y = S = AR = 0;
            Mark_Error = false;
            if (ETC_Option.Mark_Test_Mode)
                Last_Grab_Image = Multi_Mark[0].Pattern.TrainImage;
            if(COMMON.Frame.Unit[(int)Current_Unit].Sequence.Sequences == Enum.Sequence.Inspection)
            {
                COMMON.Inspection_Display[(int)Current_Mark].Image = Last_Grab_Image;
                if (Current_Mark_Type == Enum.Mark_Type.MARK || Current_Mark_Type == Enum.Mark_Type.MARK_TO_LINE || Current_Mark_Type == Enum.Mark_Type.MARK_TO_CIRCLE)
                {
                    COMMON.Inspection_Display[(int)Current_Mark].InteractiveGraphics.Clear();
                    for (int LoopCount = 0; LoopCount < 5; LoopCount++)
                    {
                        Mark_Find(COMMON.Inspection_Display[(int)Current_Mark], LoopCount, out X, out Y, out S);
                        if (S > Mark_Match_Rate[LoopCount])
                        {
                            Last_Mark_Index = LoopCount;
                            goto Mark_Find_OK;
                        }
                    }
                    Find_NG();
                    return;
                }
                Mark_Find_OK:
                if (Current_Mark_Type == Enum.Mark_Type.LINE || Current_Mark_Type == Enum.Mark_Type.MARK_TO_LINE)
                {
                    lock (Line_Lock)
                    {
                        COMMON.Inspection_Display[(int)Current_Mark].InteractiveGraphics.Clear();
                        if (Line_Find(COMMON.Inspection_Display[(int)Current_Mark], ref X, ref Y, out AR))
                            Find_NG();
                    }
                }
                if (Current_Mark_Type == Enum.Mark_Type.CIRCLE || Current_Mark_Type == Enum.Mark_Type.MARK_TO_CIRCLE)
                {
                    lock (Circle_Lock)
                    {
                        COMMON.Inspection_Display[(int)Current_Mark].InteractiveGraphics.Clear();
                        if (Circle_Find(COMMON.Inspection_Display[(int)Current_Mark], ref X, ref Y, out AR))
                            Find_NG();
                    }
                }
                if (Current_Mark_Type == Enum.Mark_Type.PATTERN)
                {
                    COMMON.Inspection_Display[(int)Current_Mark].InteractiveGraphics.Clear();
                    if (Pattern_Find(COMMON.Inspection_Display[(int)Current_Mark], out X, out Y, out S))
                        Find_NG();
                }
            }
            else
            {
                Display.Image = Last_Grab_Image;
                if (Current_Mark_Type == Enum.Mark_Type.MARK || Current_Mark_Type == Enum.Mark_Type.MARK_TO_LINE || Current_Mark_Type == Enum.Mark_Type.MARK_TO_CIRCLE)
                {
                    Display.InteractiveGraphics.Clear();
                    for (int LoopCount = 0; LoopCount < 5; LoopCount++)
                    {
                        Mark_Find(Display, LoopCount, out X, out Y, out S);
                        if (S > Mark_Match_Rate[LoopCount])
                        {
                            Last_Mark_Index = LoopCount;
                            goto Mark_Find_OK;
                        }
                    }
                    Find_NG();
                    return;
                }
                Mark_Find_OK:
                if (Current_Mark_Type == Enum.Mark_Type.LINE || Current_Mark_Type == Enum.Mark_Type.MARK_TO_LINE)
                {
                    lock (Line_Lock)
                    {
                        Display.InteractiveGraphics.Clear();
                        if (Line_Find(Display, ref X, ref Y, out AR))
                            Find_NG();
                    }
                }
                if (Current_Mark_Type == Enum.Mark_Type.CIRCLE || Current_Mark_Type == Enum.Mark_Type.MARK_TO_CIRCLE)
                {
                    lock (Circle_Lock)
                    {
                        Display.InteractiveGraphics.Clear();
                        if (Circle_Find(Display, ref X, ref Y, out AR))
                            Find_NG();
                    }
                }
                if (Current_Mark_Type == Enum.Mark_Type.PATTERN)
                {
                    Display.InteractiveGraphics.Clear();
                    if (Pattern_Find(Display, out X, out Y, out S))
                        Find_NG();
                }
            }
        }

        public void Calibration_Mark_Find(CogDisplay _Display)
        {
            X = Y = S = AR = 0;
            _Display.Image = Last_Grab_Image;
            if (Current_Mark_Type == Enum.Mark_Type.MARK || Current_Mark_Type == Enum.Mark_Type.MARK_TO_LINE || Current_Mark_Type == Enum.Mark_Type.MARK_TO_CIRCLE)
            {
                _Display.InteractiveGraphics.Clear();
                Mark_Find(_Display, 5, out X, out Y, out S);
                if (S > Mark_Match_Rate[5])
                    goto Mark_Find_OK;
                Mark_Error = true;
                return;
            }
        Mark_Find_OK:
            if (Current_Mark_Type == Enum.Mark_Type.LINE || Current_Mark_Type == Enum.Mark_Type.MARK_TO_LINE)
            {
                _Display.InteractiveGraphics.Clear();
                Line_Find(_Display, ref X, ref Y, out AR);
            }
            if (Current_Mark_Type == Enum.Mark_Type.CIRCLE || Current_Mark_Type == Enum.Mark_Type.MARK_TO_CIRCLE)
            {
                _Display.InteractiveGraphics.Clear();
                Circle_Find(_Display, ref X, ref Y, out AR);
            }
            if (Current_Mark_Type == Enum.Mark_Type.PATTERN)
            {
                Display.InteractiveGraphics.Clear();
                Pattern_Find(Display, out X, out Y, out S);
            }
            Mark_Error = false;
        }

        public void Mark_Set(Enum.Unit _Unit, Enum.Type _Type, Enum.Mark _Mark)
        {
            Current_Unit = _Unit;
            Current_Type = _Type;
            Current_Mark = _Mark;
            Mark_Load();
            Calibration_Data_Load();
            Load_Limit();
        }

        public void Mark_Delete(int _Mark_Index)
        {
            string sFilePath = "C:\\ALIGN PARAM\\Recipe\\" + COMMON.RECIPE + "\\Mark\\" + Current_Unit.ToString() + "\\" + Current_Type.ToString() + "_" + Current_Mark.ToString() + _Mark_Index.ToString() + ".cog";
            FileInfo Delete = new FileInfo(sFilePath);
            if (Delete.Exists)
                Delete.Delete();
        }

        public void Mark_Save(int _Mark_Index)
        {
            string sFolderPath = "C:\\ALIGN PARAM\\Recipe\\" + COMMON.RECIPE + "\\Mark\\" + Current_Unit.ToString();
            DirectoryInfo di = new DirectoryInfo(sFolderPath);
            if (di.Exists.Equals(false))
            {
                di.Create();
            }
            string sFilePath = "C:\\ALIGN PARAM\\Recipe\\" + COMMON.RECIPE + "\\Mark\\" + Current_Unit.ToString() + "\\" + Current_Type.ToString() + "_" + Current_Mark.ToString() + _Mark_Index.ToString() + ".cog";
            IFormatter formatter = new BinaryFormatter();
            Stream stream = null;
            if (Multi_Mark[_Mark_Index] != null)
            {
                Multi_Mark[_Mark_Index].InputImage = null;
                Multi_Mark[_Mark_Index].Run();
                stream = new FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, Multi_Mark[_Mark_Index]);
                stream.Close();
            }
        }

        public void Line_Save(string Line_WH)
        {
            string sFolderPath = "C:\\ALIGN PARAM\\Recipe\\" + COMMON.RECIPE + "\\Mark\\" + Current_Unit.ToString();
            DirectoryInfo di = new DirectoryInfo(sFolderPath);
            if (di.Exists.Equals(false))
            {
                di.Create();
            }
            string sFilePath = "C:\\ALIGN PARAM\\Recipe\\" + COMMON.RECIPE + "\\Mark\\" + Current_Unit.ToString() + "\\" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_Line" + "_" + Line_WH + ".cog";
            if (Current_Mark_Type == Enum.Mark_Type.MARK_TO_LINE)
                sFilePath = "C:\\ALIGN PARAM\\Recipe\\" + COMMON.RECIPE + "\\Mark\\" + Current_Unit.ToString() + "\\" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_Mark_to_Line" + "_" + Line_WH + ".cog";
            IFormatter formatter = new BinaryFormatter();
            Stream stream = null;
            if (Line_WH.Equals("W"))
            {
                if (Line_W != null)
                {
                    Line_W.InputImage = null;
                    Line_W.Run();
                    stream = new FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
                    formatter.Serialize(stream, Line_W);
                    stream.Close();
                }
            }
            else if (Line_WH.Equals("H"))
            {
                if (Line_H != null)
                {
                    Line_H.InputImage = null;
                    Line_H.Run();
                    stream = new FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
                    formatter.Serialize(stream, Line_H);
                    stream.Close();
                }
            }
        }

        public void Circle_Save()
        {
            string sFolderPath = "C:\\ALIGN PARAM\\Recipe\\" + COMMON.RECIPE + "\\Mark\\" + Current_Unit.ToString();
            DirectoryInfo di = new DirectoryInfo(sFolderPath);
            if (di.Exists.Equals(false))
            {
                di.Create();
            }
            string sFilePath = "C:\\ALIGN PARAM\\Recipe\\" + COMMON.RECIPE + "\\Mark\\" + Current_Unit.ToString() + "\\" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_Circle" + ".cog";
            if (Current_Mark_Type == Enum.Mark_Type.MARK_TO_CIRCLE)
                sFilePath = "C:\\ALIGN PARAM\\Recipe\\" + COMMON.RECIPE + "\\Mark\\" + Current_Unit.ToString() + "\\" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_Mark_to_Circle" + ".cog";
            IFormatter formatter = new BinaryFormatter();
            Stream stream = null;
            if (Circle != null)
            {
                Circle.InputImage = null;
                Circle.Run();
                stream = new FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, Circle);
                stream.Close();
            }
        }

        public void Pattern_Save()
        {
            string sFolderPath = "C:\\ALIGN PARAM\\Recipe\\" + COMMON.RECIPE + "\\Mark\\" + Current_Unit.ToString();
            DirectoryInfo di = new DirectoryInfo(sFolderPath);
            if (di.Exists.Equals(false))
            {
                di.Create();
            }
            string sFilePath = "C:\\ALIGN PARAM\\Recipe\\" + COMMON.RECIPE + "\\Mark\\" + Current_Unit.ToString() + "\\" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_CNL" + ".cog";
            IFormatter formatter = new BinaryFormatter();
            Stream stream = null;
            if (CNL != null)
            {
                CNL.InputImage = null;
                CNL.Run();
                stream = new FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, CNL);
                stream.Close();
            }
        }

        public void Mark_Type_Save(Enum.Mark_Type _Current_Mark_Type)
        {
            Current_Mark_Type = _Current_Mark_Type;
            ClassINI.WriteConfig("Mark Type", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "Type", Current_Mark_Type);
        }

        public Enum.Mark_Type Mark_Type_Load()
        {
            return Current_Mark_Type;
        }

        public void Mark_Load()
        {
            try
            {
                Current_Mark_Type = (Enum.Mark_Type)System.Enum.Parse(typeof(Enum.Mark_Type), ClassINI.ReadConfig("Mark Type", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "Type"));
            }
            catch { }
            for (int LoopCount = 0; LoopCount < 6; LoopCount++)
            {
                Multi_Mark[LoopCount] = null;
                string sFilePath = "C:\\ALIGN PARAM\\Recipe\\" + COMMON.RECIPE + "\\Mark\\" + Current_Unit.ToString() + "\\" + Current_Type.ToString() + "_" + Current_Mark.ToString() + LoopCount.ToString() + ".cog";
                if (File.Exists(sFilePath))
                {
                    IFormatter formatter = new BinaryFormatter();
                    Stream stream = new FileStream(sFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    try
                    {
                        Multi_Mark[LoopCount] = (CogPMAlignTool)formatter.Deserialize(stream);
                    }
                    catch { }
                }
            }
            Type_Change_Load();
            Pattern_Load();
        }

        public void Type_Change_Load()
        {
            try
            {
                Current_Mark_Type = (Enum.Mark_Type)System.Enum.Parse(typeof(Enum.Mark_Type), ClassINI.ReadConfig("Mark Type", Current_Unit.ToString() + "_" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "Type"));
            }
            catch { }
            Line_Load("W");
            Line_Load("H");
            Circle_Load();
        }

        public void Line_Load(string Line_WH)
        {
            CogFindLineTool Load_Tool = new CogFindLineTool();
            string sFilePath = "";
            if (Current_Mark_Type == Enum.Mark_Type.LINE)
                sFilePath = "C:\\ALIGN PARAM\\Recipe\\" + COMMON.RECIPE + "\\Mark\\" + Current_Unit.ToString() + "\\" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_Line" + "_" + Line_WH + ".cog";
            if (Current_Mark_Type == Enum.Mark_Type.MARK_TO_LINE)
                sFilePath = "C:\\ALIGN PARAM\\Recipe\\" + COMMON.RECIPE + "\\Mark\\" + Current_Unit.ToString() + "\\" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_Mark_to_Line" + "_" + Line_WH + ".cog";
            if (File.Exists(sFilePath))
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(sFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                try
                {
                    Load_Tool = null;
                    Load_Tool = (CogFindLineTool)formatter.Deserialize(stream);
                    if (Line_WH.Equals("W"))
                        Line_W = Load_Tool;
                    else if (Line_WH.Equals("H"))
                        Line_H = Load_Tool;
                }
                catch { }
            }
            else
            {
                Load_Tool.RunParams.NumCalipers = 50;
                Load_Tool.RunParams.CaliperSearchLength = 200;
                Load_Tool.RunParams.CaliperProjectionLength = 20;
                Load_Tool.RunParams.NumToIgnore = 15;
                Load_Tool.RunParams.CaliperRunParams.FilterHalfSizeInPixels = 5;
                Load_Tool.RunParams.CaliperRunParams.ContrastThreshold = 10;
            }
        }

        public void Circle_Load()
        {
            string sFilePath = "";
            if (Current_Mark_Type == Enum.Mark_Type.CIRCLE)
                sFilePath = "C:\\ALIGN PARAM\\Recipe\\" + COMMON.RECIPE + "\\Mark\\" + Current_Unit.ToString() + "\\" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_Circle" + ".cog";
            if (Current_Mark_Type == Enum.Mark_Type.MARK_TO_CIRCLE)
                sFilePath = "C:\\ALIGN PARAM\\Recipe\\" + COMMON.RECIPE + "\\Mark\\" + Current_Unit.ToString() + "\\" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_Mark_to_Circle" + ".cog";
            if (File.Exists(sFilePath))
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(sFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                try
                {
                    Circle = null;
                    Circle = (CogFindCircleTool)formatter.Deserialize(stream);
                }
                catch { }
            }
        }

        public void Pattern_Load()
        {
                CNL = null;
                string sFilePath = "C:\\ALIGN PARAM\\Recipe\\" + COMMON.RECIPE + "\\Mark\\" + Current_Unit.ToString() + "\\" + Current_Type.ToString() + "_" + Current_Mark.ToString() + "_CNL" + ".cog";
                if (File.Exists(sFilePath))
                {
                    IFormatter formatter = new BinaryFormatter();
                    Stream stream = new FileStream(sFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    try
                    {
                        CNL = (CogCNLSearchTool)formatter.Deserialize(stream);
                    }
                    catch { }
                }
        }

        public void Mark_Find(CogDisplay _DIsplay, int Mark_Index, out double X, out double Y, out double S)
        {
            X = Y = S = 0;
            if (_DIsplay.Image == null)
                return;
            CogInteractiveGraphicsContainer G = _DIsplay.InteractiveGraphics;
            try
            {
                Multi_Mark[Mark_Index].InputImage = _DIsplay.Image;
                Multi_Mark[Mark_Index].Run();
                if (Multi_Mark[Mark_Index].Results.Count == 0 || Multi_Mark[Mark_Index].Results[0].Score < Mark_Match_Rate[Mark_Index])
                {
                    DrawErrorString(G, "Mark Find NG", _DIsplay.Image.Width / 2, _DIsplay.Image.Height / 2);
                    return;
                }
            }
            catch
            {
                DrawErrorString(G, "Mark Find NG", _DIsplay.Image.Width / 2, _DIsplay.Image.Height / 2);
                return;
            }
            CogLineSegment GX = new CogLineSegment();
            CogLineSegment GY = new CogLineSegment();
            if (Multi_Mark[Mark_Index].Results[0].Score < 0.70)
            {
                GX.Color = CogColorConstants.Red;
                GY.Color = CogColorConstants.Red;
            }
            else
            {
                GX.Color = CogColorConstants.Green;
                GY.Color = CogColorConstants.Green;
            }
            if (Multi_Mark[Mark_Index].LastRunRecordDiagEnable == CogPMAlignLastRunRecordDiagConstants.SearchRegion)
            {
                CogRectangleAffine Search_Region = Multi_Mark[Mark_Index].SearchRegion as CogRectangleAffine;
                Search_Region.Interactive = false;
                Search_Region.GraphicDOFEnable =  CogRectangleAffineDOFConstants.None;
                G.Add(Search_Region, "R", false);
            }
            CogTransform2DLinear _Point = new CogTransform2DLinear(Multi_Mark[Mark_Index].Results[0].GetPose());
            GX.LineWidthInScreenPixels = 3;
            GY.LineWidthInScreenPixels = 3;
            GX.SetStartEnd(_Point.TranslationX - 100, _Point.TranslationY, _Point.TranslationX + 100, _Point.TranslationY);
            GY.SetStartEnd(_Point.TranslationX, _Point.TranslationY - 100, _Point.TranslationX, _Point.TranslationY + 100);
            G.Add(GX, "P", false);
            G.Add(GY, "P", false);
            X = _Point.TranslationX;
            Y = _Point.TranslationY;
            S = Multi_Mark[Mark_Index].Results[0].Score;
            DrawResultString(G, X, Y, S, Mark_Index.ToString());
        }

        public bool Line_Find(CogDisplay _DIsplay, ref double X, ref double Y, out double A)
        {
            double Offset_X = X;
            double Offset_Y = Y;
            double Origin_WSX = Line_W.RunParams.ExpectedLineSegment.StartX;
            double Origin_WSY = Line_W.RunParams.ExpectedLineSegment.StartY;
            double Origin_WEX = Line_W.RunParams.ExpectedLineSegment.EndX;
            double Origin_WEY = Line_W.RunParams.ExpectedLineSegment.EndY;

            double Origin_HSX = Line_H.RunParams.ExpectedLineSegment.StartX;
            double Origin_HSY = Line_H.RunParams.ExpectedLineSegment.StartY;
            double Origin_HEX = Line_H.RunParams.ExpectedLineSegment.EndX;
            double Origin_HEY = Line_H.RunParams.ExpectedLineSegment.EndY;
            A = 0;
            //CogFindLineTool Run_Tool_X = new CogFindLineTool(Line_W);
            Line_W.RunParams.ExpectedLineSegment.StartX += Offset_X;
            Line_W.RunParams.ExpectedLineSegment.StartY += Offset_Y;
            Line_W.RunParams.ExpectedLineSegment.EndX += Offset_X;
            Line_W.RunParams.ExpectedLineSegment.EndY += Offset_Y;
            //CogFindLineTool Run_Tool_Y = new CogFindLineTool(Line_H);
            Line_H.RunParams.ExpectedLineSegment.StartX += Offset_X;
            Line_H.RunParams.ExpectedLineSegment.StartY += Offset_Y;
            Line_H.RunParams.ExpectedLineSegment.EndX += Offset_X;
            Line_H.RunParams.ExpectedLineSegment.EndY += Offset_Y;
            CogInteractiveGraphicsContainer G = _DIsplay.InteractiveGraphics;
            Line_W.InputImage = (CogImage8Grey)_DIsplay.Image;
            Line_H.InputImage = (CogImage8Grey)_DIsplay.Image;
            CogIntersectLineLineTool Cross_Tool = new CogIntersectLineLineTool();
            try
            {
                Line_W.Run();
                Line_H.Run();

                Cross_Tool.InputImage = _DIsplay.Image;
                Cross_Tool.LineA = Line_W.Results.GetLine();
                Cross_Tool.LineB = Line_H.Results.GetLine();
                Cross_Tool.Run();

                Line_W.RunParams.ExpectedLineSegment.StartX = Origin_WSX;
                Line_W.RunParams.ExpectedLineSegment.StartY = Origin_WSY;
                Line_W.RunParams.ExpectedLineSegment.EndX = Origin_WEX;
                Line_W.RunParams.ExpectedLineSegment.EndY = Origin_WEY;

                Line_H.RunParams.ExpectedLineSegment.StartX = Origin_HSX;
                Line_H.RunParams.ExpectedLineSegment.StartY = Origin_HSY;
                Line_H.RunParams.ExpectedLineSegment.EndX = Origin_HEX;
                Line_H.RunParams.ExpectedLineSegment.EndY = Origin_HEY;
                if ((Math.Abs(Cross_Tool.Angle) * 180 / Math.PI < (90 - Line_Angle_Limit)) || (Math.Abs(Cross_Tool.Angle) * 180 / Math.PI > (90 + Line_Angle_Limit)))
                {
                    DrawErrorString(G, "Line Find NG", _DIsplay.Image.Width / 2, _DIsplay.Image.Height / 2);
                    return true;
                }
            }
            catch
            {
                DrawErrorString(G, "Line Find NG", _DIsplay.Image.Width / 2, _DIsplay.Image.Height / 2);
                Line_W.RunParams.ExpectedLineSegment.StartX = Origin_WSX;
                Line_W.RunParams.ExpectedLineSegment.StartY = Origin_WSY;
                Line_W.RunParams.ExpectedLineSegment.EndX = Origin_WEX;
                Line_W.RunParams.ExpectedLineSegment.EndY = Origin_WEY;

                Line_H.RunParams.ExpectedLineSegment.StartX = Origin_HSX;
                Line_H.RunParams.ExpectedLineSegment.StartY = Origin_HSY;
                Line_H.RunParams.ExpectedLineSegment.EndX = Origin_HEX;
                Line_H.RunParams.ExpectedLineSegment.EndY = Origin_HEY;
                return true;
            }

            CogLine GX = new CogLine();
            CogLine GY = new CogLine();
            GX.Color = CogColorConstants.Green;
            GY.Color = CogColorConstants.Green;
            for (int Loopcount = 0; Loopcount < Line_W.Results.Count; Loopcount++)
                if (Line_W.Results[Loopcount].Used)
                    G.Add(Line_W.Results[Loopcount].CreateResultGraphics(CogFindLineResultGraphicConstants.DataPoint), "L", false);
            for (int Loopcount = 0; Loopcount < Line_H.Results.Count; Loopcount++)
                if (Line_H.Results[Loopcount].Used)
                    G.Add(Line_H.Results[Loopcount].CreateResultGraphics(CogFindLineResultGraphicConstants.DataPoint), "L", false);
            GX = Cross_Tool.LineA;
            GY = Cross_Tool.LineB;
            G.Add(GX, "L", false);
            G.Add(GY, "L", false);
            X = Cross_Tool.X;
            Y = Cross_Tool.Y;
            A = Cross_Tool.Angle * 180 / Math.PI;
            DrawResultString(G, X, Y, A, "A");
            return false;
        }

        public bool Circle_Find(CogDisplay _DIsplay, ref double X, ref double Y, out double R)
        {
            //X = Y = 0
            double Offset_X = X;
            double Offset_Y = Y;
            double Origin_X = Circle.RunParams.ExpectedCircularArc.CenterX;
            double Origin_Y = Circle.RunParams.ExpectedCircularArc.CenterY;
            R = 0;
            //CogFindCircleTool Run_Tool = new CogFindCircleTool(Circle);
            Circle.RunParams.ExpectedCircularArc.CenterX += Offset_X;
            Circle.RunParams.ExpectedCircularArc.CenterY += Offset_Y;
            CogInteractiveGraphicsContainer G = _DIsplay.InteractiveGraphics;
            Circle.InputImage = (CogImage8Grey)_DIsplay.Image;
            try
            {
                Circle.Run();
                Circle.RunParams.ExpectedCircularArc.CenterX = Origin_X;
                Circle.RunParams.ExpectedCircularArc.CenterY = Origin_Y;
                if (Circle.Results.GetCircle().Radius - Circle.Results.GetCircle().Radius > Circle_Radius_Limit || Circle.Results.GetCircle().Radius - Circle.Results.GetCircle().Radius < -Circle_Radius_Limit)
                {
                    DrawErrorString(G, "Circle Find NG", _DIsplay.Image.Width / 2, _DIsplay.Image.Height / 2);
                    return true;
                }
            }
            catch
            {
                DrawErrorString(G, "Circle Find NG", _DIsplay.Image.Width / 2, _DIsplay.Image.Height / 2);
                Circle.RunParams.ExpectedCircularArc.CenterX = Origin_X;
                Circle.RunParams.ExpectedCircularArc.CenterY = Origin_Y;
                return true;
            }

            CogLine GX = new CogLine();
            CogLine GY = new CogLine();
            GX.Color = CogColorConstants.Green;
            GY.Color = CogColorConstants.Green;
            for (int Loopcount = 0; Loopcount < Circle.Results.Count; Loopcount++)
                if (Circle.Results[Loopcount].Used)
                    G.Add(Circle.Results[Loopcount].CreateResultGraphics(CogFindCircleResultGraphicConstants.DataPoint), "C", false);
            G.Add(Circle.Results.GetCircle(), "C", false);
            GX.SetFromStartXYEndXY(0, Circle.Results.GetCircle().CenterY, _DIsplay.Image.Width, Circle.Results.GetCircle().CenterY);
            GY.SetFromStartXYEndXY(Circle.Results.GetCircle().CenterX, 0, Circle.Results.GetCircle().CenterX, _DIsplay.Image.Height);
            G.Add(GX, "C", false);
            G.Add(GY, "C", false);
            X = Circle.Results.GetCircle().CenterX;
            Y = Circle.Results.GetCircle().CenterY;
            R = Circle.Results.GetCircle().Radius;
            DrawResultString(G, X, Y, R, "R");
            return false;
        }

        public bool Pattern_Find(CogDisplay _DIsplay, out double X, out double Y, out double S)
        {
            X = Y = S = 0;
            if (_DIsplay.Image == null)
                return true;
            CogInteractiveGraphicsContainer G = _DIsplay.InteractiveGraphics;
            try
            {
                CNL.InputImage = _DIsplay.Image as CogImage8Grey;
                CNL.Run();
                if (CNL.Results.Count == 0 || CNL.Results[0].Score < Pattern_Match_Rate)
                {
                    DrawErrorString(G, "Mark Find NG", _DIsplay.Image.Width / 2, _DIsplay.Image.Height / 2);
                    return true;
                }
            }
            catch
            {
                DrawErrorString(G, "Mark Find NG", _DIsplay.Image.Width / 2, _DIsplay.Image.Height / 2);
                return true;
            }
            CogLineSegment GX = new CogLineSegment();
            CogLineSegment GY = new CogLineSegment();
            if (CNL.Results[0].Score < 0.70)
            {
                GX.Color = CogColorConstants.Red;
                GY.Color = CogColorConstants.Red;
            }
            else
            {
                GX.Color = CogColorConstants.Green;
                GY.Color = CogColorConstants.Green;
            }
            if (CNL.LastRunRecordDiagEnable == CogCNLSearchLastRunRecordDiagConstants.SearchRegion)
            {
                CogRectangleAffine Search_Region = CNL.SearchRegion as CogRectangleAffine;
                Search_Region.Interactive = false;
                Search_Region.GraphicDOFEnable = CogRectangleAffineDOFConstants.None;
                G.Add(Search_Region, "R", false);
            }
            CogTransform2DLinear _Point = new CogTransform2DLinear();
            GX.LineWidthInScreenPixels = 3;
            GY.LineWidthInScreenPixels = 3;
            GX.SetStartEnd(CNL.Results[0].LocationX - 100, CNL.Results[0].LocationY, CNL.Results[0].LocationX + 100, CNL.Results[0].LocationY);
            GY.SetStartEnd(CNL.Results[0].LocationX, CNL.Results[0].LocationY - 100, CNL.Results[0].LocationX, CNL.Results[0].LocationY + 100);
            G.Add(GX, "P", false);
            G.Add(GY, "P", false);
            X = CNL.Results[0].LocationX;
            Y = CNL.Results[0].LocationY;
            S = CNL.Results[0].Score;
            DrawResultString(G, X, Y, S, "S");
            return false;
        }

        public void Calibration_Copy(Enum.Type Target_Type)
        {
            Resolution_X = COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Target_Type].Mark[(int)Current_Mark].Resolution_X;
            Resolution_Y = COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Target_Type].Mark[(int)Current_Mark].Resolution_Y;
            Master_X = COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Target_Type].Mark[(int)Current_Mark].Master_X;
            Master_Y = COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Target_Type].Mark[(int)Current_Mark].Master_Y;
            Rotate_Center_X = COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Target_Type].Mark[(int)Current_Mark].Rotate_Center_X;
            Rotate_Center_Y = COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Target_Type].Mark[(int)Current_Mark].Rotate_Center_Y;
            Calibration_Point_X = COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Target_Type].Mark[(int)Current_Mark].Calibration_Point_X;
            Calibration_Point_Y = COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Target_Type].Mark[(int)Current_Mark].Calibration_Point_Y;
            Camera_Width = COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Target_Type].Mark[(int)Current_Mark].Camera_Width;
            Camera_Height = COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Target_Type].Mark[(int)Current_Mark].Camera_Height;
        }

        private void DrawResultString(CogInteractiveGraphicsContainer G, double _X, double _Y, double SAR, string Type_SAR)
        {
            try
            {
                G.Remove("Label");
            }
            catch { }
            CogGraphicLabel label = new CogGraphicLabel();
            string strData = "X=" + _X.ToString("0.0") + "    " + "Y=" + _Y.ToString("0.0") + "    ";
            switch (Type_SAR)
            {
                case "A":
                    strData += "A=" + SAR.ToString("0.0");
                    break;
                case "R":
                    strData += "R=" + SAR.ToString("0.0");
                    break;
                case "S":
                    strData += "S=" + SAR.ToString("0.0");
                    break;
                default:
                    strData += "S" + Type_SAR.ToString() + "=" + SAR.ToString("0.0");
                    break;
            }

            label.SetXYText(0, 0, strData);

            label.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
            label.BackgroundColor = CogColorConstants.White;
            label.Color = CogColorConstants.Blue;
            G.Add(label, "Label", false);
        }

        private void DrawErrorString(CogInteractiveGraphicsContainer G, string Messege, double _X, double _Y)
        {
            try
            {
                G.Remove("Label");
            }
            catch { }
            CogGraphicLabel label = new CogGraphicLabel();
            string strData = Messege;

            label.SetXYText(_X, _Y, strData);
            label.Alignment = CogGraphicLabelAlignmentConstants.BaselineCenter;
            label.BackgroundColor = CogColorConstants.Black;
            label.Color = CogColorConstants.Red;
            G.Add(label, "Label", false);
        }
    }
}
