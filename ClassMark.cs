using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;

using Cognex.VisionPro;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.CalibFix;
using Cognex.VisionPro.Dimensioning;
namespace Align
{
    public class ClassMark
    {
        public CogPMAlignTool[] Mark = new CogPMAlignTool[6];
        public CogFindLineTool Line_W = new CogFindLineTool();
        public CogFindLineTool Line_H = new CogFindLineTool();
        public CogCalibCheckerboardTool Cal_Tool = new CogCalibCheckerboardTool();

        Main_Frame Frame;

        ClassEnum.Type gType;

        public int Exposuer = 35;
        public int Brightness = 0;
        public int Contrast = 0;
        public int Gain = 51;
        public double Gamma = 1;
        public int DataSheet = 0;

        string Recipe_Name;
        int Unit_Num;
        int Mark_Num;

        public double Master_X;
        public double Master_Y;
        public double resolution_X;
        public double resolution_Y;
        public double Rotate_Center_X;
        public double Rotate_Center_Y;

        public double Match_Rate = 0.7;
        int Find_Mark_Num = 0;
        public double inspection1 = 0, inspection2 = 0;


        public void Calibration_Data(PointF[] _Point, double _Angle)
        {
            double[] _Cal_X = new double[5];
            double[] _Cal_Y = new double[5];

            _Cal_X[0] = (Math.Abs(_Point[0].X - _Point[1].X) + Math.Abs(_Point[1].X - _Point[2].X) + Math.Abs(_Point[2].X - _Point[3].X)  +Math.Abs(_Point[3].X - _Point[4].X)) / 4;
            _Cal_X[1] = (Math.Abs(_Point[5].X - _Point[6].X) + Math.Abs(_Point[6].X - _Point[7].X) + Math.Abs(_Point[7].X - _Point[8].X) + Math.Abs(_Point[8].X - _Point[9].X)) / 4;
            _Cal_X[2] = (Math.Abs(_Point[10].X - _Point[11].X) + Math.Abs(_Point[11].X - _Point[12].X) + Math.Abs(_Point[12].X - _Point[13].X) + Math.Abs(_Point[13].X - _Point[14].X)) / 4;
            _Cal_X[3] = (Math.Abs(_Point[15].X - _Point[16].X) + Math.Abs(_Point[16].X - _Point[17].X) + Math.Abs(_Point[17].X - _Point[18].X) + Math.Abs(_Point[18].X - _Point[19].X)) / 4;
            _Cal_X[4] = (Math.Abs(_Point[20].X - _Point[21].X) + Math.Abs(_Point[21].X - _Point[22].X) + Math.Abs(_Point[22].X - _Point[23].X) + Math.Abs(_Point[23].X - _Point[24].X)) / 4;
            _Cal_Y[0] = (Math.Abs(_Point[0].Y - _Point[5].Y) + Math.Abs(_Point[5].Y - _Point[10].Y) + Math.Abs(_Point[10].Y - _Point[15].Y) + Math.Abs(_Point[15].Y - _Point[20].Y)) / 4;
            _Cal_Y[1] = (Math.Abs(_Point[1].Y - _Point[6].Y) + Math.Abs(_Point[6].Y - _Point[11].Y) + Math.Abs(_Point[11].Y - _Point[16].Y) + Math.Abs(_Point[16].Y - _Point[21].Y)) / 4;
            _Cal_Y[2] = (Math.Abs(_Point[2].Y - _Point[7].Y) + Math.Abs(_Point[7].Y - _Point[12].Y) + Math.Abs(_Point[12].Y - _Point[17].Y) + Math.Abs(_Point[17].Y - _Point[22].Y)) / 4;
            _Cal_Y[3] = (Math.Abs(_Point[3].Y - _Point[8].Y) + Math.Abs(_Point[8].Y - _Point[13].Y) + Math.Abs(_Point[13].Y - _Point[18].Y) + Math.Abs(_Point[18].Y - _Point[23].Y)) / 4;
            _Cal_Y[4] = (Math.Abs(_Point[4].Y - _Point[9].Y) + Math.Abs(_Point[9].Y - _Point[14].Y) + Math.Abs(_Point[14].Y - _Point[19].Y) + Math.Abs(_Point[19].Y - _Point[24].Y)) / 4;
            resolution_X = (Frame.Unit[Unit_Num].Type[(int)gType].Cal_Move_Pitch_X * 1000) / ((_Cal_X[0] + _Cal_X[1] + _Cal_X[2] + _Cal_X[3] + _Cal_X[4]) / 5);
            resolution_Y = (Frame.Unit[Unit_Num].Type[(int)gType].Cal_Move_Pitch_Y * 1000) / ((_Cal_Y[0] + _Cal_Y[1] + _Cal_Y[2] + _Cal_Y[3] + _Cal_Y[4]) / 5);
            ClassINI.Write_Calibration_Data("Resolution", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "_X", Convert.ToString(resolution_X));
            ClassINI.Write_Calibration_Data("Resolution", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "_Y", Convert.ToString(resolution_Y));
            Master_Position_Save(_Point[12].X, _Point[12].Y);
            Rotate_Center(_Point[25].X, _Point[25].Y, _Point[27].X, _Point[27].Y, _Angle, ref Rotate_Center_X, ref Rotate_Center_Y); //회전 중심 구하는 구간
            ClassINI.Write_Calibration_Data("Rotate Center", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "_X", Convert.ToString(Rotate_Center_X));
            ClassINI.Write_Calibration_Data("Rotate Center", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "_Y", Convert.ToString(Rotate_Center_Y));
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
            //Camera 별 회전 중심 구하기
            switch (Mark_Num)
            {
                case 0:
                    _Rotate_Center_X = PointX + Math.Abs(Y_Distance * Math.Sin(Atan * Math.PI / 180)); //좌
                    _Rotate_Center_Y = PointY + Math.Abs(Y_Distance * Math.Cos(Atan * Math.PI / 180)); //상
                    break;
                case 1:
                    _Rotate_Center_X = PointX - Math.Abs(Y_Distance * Math.Sin(Atan * Math.PI / 180)); //우
                    _Rotate_Center_Y = PointY + Math.Abs(Y_Distance * Math.Cos(Atan * Math.PI / 180)); //상
                    break;
                case 2:
                    _Rotate_Center_X = PointX + Math.Abs(Y_Distance * Math.Sin(Atan * Math.PI / 180)); //좌
                    _Rotate_Center_Y = PointY - Math.Abs(Y_Distance * Math.Cos(Atan * Math.PI / 180)); //하
                    break;
                case 3:
                    _Rotate_Center_X = PointX - Math.Abs(Y_Distance * Math.Sin(Atan * Math.PI / 180)); //우
                    _Rotate_Center_Y = PointY - Math.Abs(Y_Distance * Math.Cos(Atan * Math.PI / 180)); //하
                    break;
            }
        }

        private double deg_TO_Radian(double T)
        {
            return T * Math.PI / 180;
        }

        public void Master_Position_Load()
        {
            if ((ClassINI.Read_Calibration_Data("Master Position", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "_X")).Equals(""))
            {
                Master_X = 0;
                Master_Y = 0;
                Rotate_Center_X = 0;
                Rotate_Center_Y = 0;
                resolution_X = 4.73;
                resolution_Y = 4.73;
            }
            else
            {
                Master_X = Convert.ToDouble(ClassINI.Read_Calibration_Data("Master Position", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "_X"));
                Master_Y = Convert.ToDouble(ClassINI.Read_Calibration_Data("Master Position", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "_Y"));
                Rotate_Center_X = Convert.ToDouble(ClassINI.Read_Calibration_Data("Rotate Center", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "_X"));
                Rotate_Center_Y = Convert.ToDouble(ClassINI.Read_Calibration_Data("Rotate Center", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "_Y"));
                resolution_X = Convert.ToDouble(ClassINI.Read_Calibration_Data("Resolution", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "_X"));
                resolution_Y = Convert.ToDouble(ClassINI.Read_Calibration_Data("Resolution", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "_Y"));
            }
        }

        public void Master_Position_Save(double _X, double _Y)
        {
            Master_X = _X;
            Master_Y = _Y;
            ClassINI.Write_Calibration_Data("Master Position", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "_X", Convert.ToString(Master_X));
            ClassINI.Write_Calibration_Data("Master Position", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "_Y", Convert.ToString(Master_Y));
        }

        public void Recipe_Send(string _Recipe)
        {
            Recipe_Name = _Recipe;
        }

        public void Parameter_Save()
        {
            ClassINI.WriteConfig("ParaMeter", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "Exposuer", Convert.ToString(Exposuer));
            ClassINI.WriteConfig("ParaMeter", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "Brightness", Convert.ToString(Brightness));
            ClassINI.WriteConfig("ParaMeter", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "Contrast", Convert.ToString(Contrast));
            ClassINI.WriteConfig("ParaMeter", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "Gain", Convert.ToString(Gain));
            ClassINI.WriteConfig("ParaMeter", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "Gamma", Convert.ToString(Gamma));
            ClassINI.WriteConfig("ParaMeter", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "DataSheet", Convert.ToString(DataSheet));
        }

        public void Parameter_load()
        {
            if (ClassINI.ReadConfig("ParaMeter", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "Exposuer") != "")
                Exposuer = Convert.ToInt32(ClassINI.ReadConfig("ParaMeter", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "Exposuer"));
            if (ClassINI.ReadConfig("ParaMeter", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "Brightness") != "")
                Brightness = Convert.ToInt32(ClassINI.ReadConfig("ParaMeter", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "Brightness"));
            if (ClassINI.ReadConfig("ParaMeter", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "Contrast") != "")
                Contrast = Convert.ToInt32(ClassINI.ReadConfig("ParaMeter", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "Contrast"));
            if (ClassINI.ReadConfig("ParaMeter", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "Gain") != "")
                Gain = Convert.ToInt32(ClassINI.ReadConfig("ParaMeter", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "Gain"));
            if (ClassINI.ReadConfig("ParaMeter", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "Gamma") != "")
                Gamma = Convert.ToDouble(ClassINI.ReadConfig("ParaMeter", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "Gamma"));
            if (ClassINI.ReadConfig("ParaMeter", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "DataSheet") != "")
                DataSheet = Convert.ToInt32(ClassINI.ReadConfig("ParaMeter", "Unit" + Unit_Num.ToString() + "_" + gType + "_Mark" + Convert.ToString(Mark_Num) + "DataSheet"));
            try { Match_Rate = Convert.ToDouble(ClassINI.ReadConfig("Match Rate", "Unit" + Unit_Num.ToString() + "_" + gType.ToString() + "_Mark" + Mark_Num.ToString() + "_Match_Rate")); }
            catch { }
        }

        public void Parameter_Set(ICogAcqFifo _Cam, bool HIKE)
        {
            _Cam.FrameGrabber.OwnedGigEAccess.SetFeature("GammaEnable", "1");
            _Cam.OwnedExposureParams.Exposure = Exposuer;
            _Cam.OwnedBrightnessParams.Brightness = Brightness;
            _Cam.OwnedContrastParams.Contrast = Contrast;
            _Cam.FrameGrabber.OwnedGigEAccess.SetFeature("GammaSelector", "User");
            _Cam.FrameGrabber.OwnedGigEAccess.SetFeature("Gamma", Gamma.ToString());
            if (!HIKE)
            {
                _Cam.FrameGrabber.OwnedGigEAccess.SetIntegerFeature("GainRaw", Convert.ToUInt32(Gain));
                _Cam.FrameGrabber.OwnedGigEAccess.SetIntegerFeature("DigitalShift", (uint)DataSheet);
            }
            else
            {

            }
        }

        public void Mark_Set(ClassEnum.Type _Type, int _Unit_num, Main_Frame _Frame, int _Mark_Num)
        {
            Frame = _Frame;
            gType = _Type;
            Unit_Num = _Unit_num;
            Mark_Num = _Mark_Num;
        }

        public void Mark_Save(int _Mark_Index)
        {
            string sFilePath = "C:\\ALIGN\\Recipe\\" + Recipe_Name + "\\Mark\\Unit" + Unit_Num.ToString() + "\\" + gType + "_Mark" + Convert.ToString(Mark_Num) + _Mark_Index.ToString() + ".cog";
            IFormatter formatter = new BinaryFormatter();
            Stream stream = null;
            if (Mark[_Mark_Index] != null)
            {
                Mark[_Mark_Index].InputImage = null;
                Mark[_Mark_Index].Run();
                stream = new FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, Mark[_Mark_Index]);
            }
        }

        public void Line_Save(string Line_WH)
        {
            string sFilePath = "C:\\ALIGN\\Recipe\\" + Recipe_Name + "\\Mark\\Unit" + Unit_Num.ToString() + "\\" + gType + "_Line" + Mark_Num + "_" + Line_WH + ".cog";
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
                }
            }

        }

        public void Line_Load(string Line_WH)
        {
            string sFilePath = "C:\\ALIGN\\Recipe\\" + Recipe_Name + "\\Mark\\Unit" + Unit_Num.ToString() + "\\" + gType + "_Line" + Mark_Num + "_" + Line_WH + ".cog";
            if (File.Exists(sFilePath))
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(sFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                try
                {
                    if (Line_WH.Equals("W"))
                    {
                        Line_W = null;
                        Line_W = (CogFindLineTool)formatter.Deserialize(stream);
                    }
                    else if (Line_WH.Equals("H"))
                    {
                        Line_H = null;
                        Line_H = (CogFindLineTool)formatter.Deserialize(stream);
                    }
                }
                catch { }
            }
        }

        public void CalibChecker_Save(int _Mark_Index)
        {
            string sFilePath = "C:\\ALIGN\\Recipe\\" + Recipe_Name + "\\Calib\\Unit" + Unit_Num.ToString() + "\\" + gType + "_Cal" + Convert.ToString(Mark_Num) + ".cog";
            IFormatter formatter = new BinaryFormatter();
            Stream stream = null;
            if (Cal_Tool != null)
            {
                Cal_Tool.InputImage = null;
                Cal_Tool.Run();
                stream = new FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, Cal_Tool);
            }
        }

        public void CalibChecker_Load()
        {
            Cal_Tool = null;
            string sFilePath = "C:\\ALIGN\\Recipe\\" + Recipe_Name + "\\Calib\\Unit" + Unit_Num.ToString() + "\\" + gType + "_Cal" + Convert.ToString(Mark_Num) + ".cog";

            if (File.Exists(sFilePath))
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(sFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                try
                {
                    Cal_Tool = (CogCalibCheckerboardTool)formatter.Deserialize(stream);
                }
                catch { }
            }
        }

        public ICogImage CalibChecker_Run(ICogImage _Image)
        {
            try
            {
                Cal_Tool.InputImage = _Image;
                Cal_Tool.Run();
                return Cal_Tool.OutputImage;
            }
            catch { return _Image; }
        }

        public void Mark_Load()
        {
            for (int i = 0; i < 6; i++)
            {
                Mark[i] = null;
                string sFilePath = "C:\\ALIGN\\Recipe\\" + Recipe_Name + "\\Mark\\Unit" + Unit_Num.ToString() + "\\" + gType + "_Mark" + Convert.ToString(Mark_Num) + i.ToString() + ".cog";
                if (File.Exists(sFilePath))
                {
                    IFormatter formatter = new BinaryFormatter();
                    Stream stream = new FileStream(sFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    try
                    {
                        Mark[i] = (CogPMAlignTool)formatter.Deserialize(stream);
                    }
                    catch { }
                }
            }
            Master_Position_Load();
            CalibChecker_Load();
            Line_Load("W");
            Line_Load("H");
        }

        public void Mark_Delete(int _Mark_Index)
        {
            string sFilePath = "C:\\ALIGN\\Recipe\\" + Recipe_Name + "\\Mark\\Unit" + Unit_Num.ToString() + "\\" + gType + "_Mark" + Convert.ToString(Mark_Num) + _Mark_Index.ToString() + ".cog";
            FileInfo Delete = new FileInfo(sFilePath);
            if (Delete.Exists)
                Delete.Delete();
        }

        int cur_unit = 0;        

        public bool Enable_Mark_Find(ICogImage _Image, CogInteractiveGraphicsContainer _G, int _Mark_Num, ref double _X, ref double _Y, ref double _S)
        {
            try
            {   //21.01.13
                //21.01.27 재수정 Target / Object 구별 하도록 설정
                #region
                //if (Frame.Setup_Display.T_O_checkBox.Checked && Frame.Unit[1].Object_Use == true && gType == ClassEnum.Type.Target && cur_unit == 1
                // || Frame.Setup_Display.T_O_checkBox.Checked && Frame.Unit[2].Object_Use == true && gType == ClassEnum.Type.Target && cur_unit == 2)
                //if (Frame.Setup_Display.T_O_checkBox.Checked && Frame.Unit[1].Object_Use == true && Unit_Num == 1 && gType == ClassEnum.Type.Target
                // || Frame.Setup_Display.T_O_checkBox.Checked && Frame.Unit[2].Object_Use == true && Unit_Num == 2 && gType == ClassEnum.Type.Target)
                //{
                //    Parallel.For(0, 2, i =>
                //    {
                //        if (i == 0)
                //        {
                //            gType = ClassEnum.Type.Target;
                //            Mark[_Mark_Num].InputImage = _Image;
                //            Mark[_Mark_Num].Run();
                //        }
                //        else
                //        {
                //            gType = ClassEnum.Type.Object;
                //            Mark[_Mark_Num].InputImage = _Image;
                //            Mark[_Mark_Num].Run();
                //        }
                //    });
                //}

                /**
                //else if(Frame.Setup_Display.T_O_checkBox.Checked && Frame.Unit[1].Object_Use == true && gType == ClassEnum.Type.Object && cur_unit == 1
                //     || Frame.Setup_Display.T_O_checkBox.Checked && Frame.Unit[2].Object_Use == true && gType == ClassEnum.Type.Object && cur_unit == 2)
                //else if (Frame.Setup_Display.T_O_checkBox.Checked && Frame.Unit[1].Object_Use == true && cur_unit == 1
                //      || Frame.Setup_Display.T_O_checkBox.Checked && Frame.Unit[2].Object_Use == true && cur_unit == 2)
                //{
                //    Mark[_Mark_Num].InputImage = _Image;
                //    Mark[_Mark_Num].Run();
                //}
                **/
                #endregion
                //else
                //{
                    //기존
                    Mark[_Mark_Num].InputImage = _Image;
                    Mark[_Mark_Num].Run();
                //}                
            }
            catch
            {
                return true;
            }

            CogTransform2DLinear _Linear = new CogTransform2DLinear();
            if (Mark[_Mark_Num].Results != null && Mark[_Mark_Num].Results.Count > 0)
            {
                _Linear = Mark[_Mark_Num].Results[0].GetPose();
                _X = _Linear.TranslationX;
                _Y = _Linear.TranslationY;
                Find_Mark_Num = _Mark_Num;
                _S = Mark[_Mark_Num].Results[0].Score;

                CogLine LineX = new CogLine();
                CogLine LineY = new CogLine();
                LineX.SetFromStartXYEndXY(_X, 0, _X, _Image.Height);
                LineY.SetFromStartXYEndXY(0, _Y, _Image.Width, _Y);
                _G.Add(LineX, "1", true);
                _G.Add(LineY, "1", true);

                CogGraphicLabel Lable = DrawResultString(_X, _Y, _S, "S");
                Lable.Font = new System.Drawing.Font("a", 12);
                _G.Add(Lable, "Label", false);
            }

            return false;
        }

        public ICogImage Mark_Fixture(ICogImage _image)
        {
            try
            {
                if (_image.Equals(null))
                    return null;
            }
            catch { return null; }
            if (!_image.CoordinateSpaceTree.HasChanged)
            {
                CogFixtureTool FixtureTool = new CogFixtureTool();
                FixtureTool.InputImage = _image;
                FixtureTool.RunParams.UnfixturedFromFixturedTransform = Mark[0].Results[0].GetPose();
                FixtureTool.Run();
                return FixtureTool.OutputImage;
            }
            return _image;
        }

        public bool Enable_Line_Find(ICogImage _Image, CogInteractiveGraphicsContainer _G, ref double _X, ref double _Y)
        {
            CogIntersectLineLineTool Ling_Tool = new CogIntersectLineLineTool();
            try
            {
                Line_W.InputImage = (CogImage8Grey)_Image;
                Line_W.Run();
                Line_H.InputImage = (CogImage8Grey)_Image;
                Line_H.Run();
                Ling_Tool.LineA = Line_W.Results.GetLine();
                Ling_Tool.LineB = Line_H.Results.GetLine();
                if (Frame.Setup_Display.Line_Projection_checkBox.Checked)
                {
                    Ling_Tool.LineA.Rotation = 0;
                    double T = ((double)-90 / (double)180 * Math.PI);
                    Ling_Tool.LineB.Rotation = T;
                }
                Ling_Tool.InputImage = _Image;
                Ling_Tool.Run();
                if (Frame.Unit[Unit_Num].Type[(int)gType].Find_Type.Equals(ClassEnum.Mark_Type.MARK_TO_LINE))
                {
                    CogTransform2DLinear _Linear = Mark[0].Results[0].GetPose();
                    _X = Ling_Tool.X + _Linear.TranslationX;
                    _Y = Ling_Tool.Y + _Linear.TranslationY;
                }
                else
                {
                    _X = Ling_Tool.X;
                    _Y = Ling_Tool.Y;
                }
            }
            catch
            {
                return true;
            }
            if (Line_W.Results != null && Line_H.Results != null)
            {
                for (int i = 0; i < Line_W.Results.Count; i++)
                {
                    if (Line_W.Results[i].Used)
                        _G.Add(Line_W.Results[i].CreateResultGraphics(CogFindLineResultGraphicConstants.DataPoint), "D", true);
                }
                for (int i = 0; i < Line_H.Results.Count; i++)
                {
                    if (Line_H.Results[i].Used)
                        _G.Add(Line_H.Results[i].CreateResultGraphics(CogFindLineResultGraphicConstants.DataPoint), "D", true);
                }
                _G.Add(Ling_Tool.LineA, "A", true);
                _G.Add(Ling_Tool.LineB, "A", true);
                if (!Frame.Unit[Unit_Num].Inspection_Use)
                {
                    CogGraphicLabel Lable = DrawResultString(_X, _Y);
                    Lable.Font = new System.Drawing.Font("a", 12);
                    _G.Add(Lable, "Label", false);
                }
                else
                {
                    int point1, point2;
                    CogTransform2DLinear Liner = Frame.Unit[Unit_Num].Type[(int)ClassEnum.Type.Target].Model[Mark_Num].Mark[0].Results[0].GetPose();
                    Max_Min(Line_W, out point1, out point2);

                    double a = Line_W.Results[point1].Y - Line_W.Results[point2].Y;
                    double b = Line_W.Results[point2].X - Line_W.Results[point1].X;
                    double c = (Line_W.Results[point1].X * Line_W.Results[point2].Y) - (Line_W.Results[point2].X * Line_W.Results[point1].Y);
                    double dist = (double)Math.Abs((a * Liner.TranslationX) + (b * Liner.TranslationY) + c) / (double)Math.Sqrt((a * a) + (b * b)) * resolution_Y;
                    inspection1 = dist;

                    Max_Min(Line_H, out point1, out point2);
                    a = Line_H.Results[point1].Y - Line_H.Results[point2].Y;
                    b = Line_H.Results[point2].X - Line_H.Results[point1].X;
                    c = (Line_H.Results[point1].X * Line_H.Results[point2].Y) - (Line_H.Results[point2].X * Line_H.Results[point1].Y);
                    dist = (double)Math.Abs((a * Liner.TranslationX) + (b * Liner.TranslationY) + c) / (double)Math.Sqrt((a * a) + (b * b)) * resolution_Y;
                    inspection2 = dist;
                    _X = inspection1;
                    _Y = inspection2;



                    CogGraphicLabel Lable = DrawInspectionString(0, _Image.Height - 500, "Point1", inspection1);
                    Lable.Font = new System.Drawing.Font("a", 12);
                    _G.Add(Lable, "Label", true);
                    Lable = DrawInspectionString(0, _Image.Height - 250, "Point2", inspection2);
                    Lable.Font = new System.Drawing.Font("a", 12);
                    _G.Add(Lable, "Label", false);
                }
            }
            return false;
        }

        private void Max_Min(CogFindLineTool _tool, out int Min, out int Max)
        {
            Min = 0; Max = 0;
            for (int i = 0; i < _tool.Results.Count; i++)
                if (_tool.Results[i].Used)
                {
                    Min = i;
                    break;
                }
            for (int i = _tool.Results.Count - 1; i >= 0; i--)
                if (_tool.Results[i].Used)
                {
                    Max = i;
                    break;
                }
        }

        private static CogGraphicLabel DrawResultString(double _X, double _Y, double _dScoreOrAngle, string _strScoreOrAngle)
        {
            CogGraphicLabel label = new CogGraphicLabel();
            string strData = "X=" + _X.ToString("0.0") + "    " + "Y=" + _Y.ToString("0.0");
            if (_dScoreOrAngle != 0) strData += "    " + _strScoreOrAngle + "=" + _dScoreOrAngle.ToString("0.00");

            label.SetXYText(0, 0, strData);

            label.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
            label.BackgroundColor = CogColorConstants.White;
            label.Color = CogColorConstants.Blue;
            return label;
        }

        private static CogGraphicLabel DrawResultString(double _X, double _Y)
        {
            CogGraphicLabel label = new CogGraphicLabel();
            string strData = "X=" + _X.ToString("0.0") + "    " + "Y=" + _Y.ToString("0.0");

            label.SetXYText(0, 0, strData);

            label.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
            label.BackgroundColor = CogColorConstants.White;
            label.Color = CogColorConstants.Blue;
            return label;
        }

        private static CogGraphicLabel DrawInspectionString(double _X, double _Y, string Point, double Val)
        {
            CogGraphicLabel label = new CogGraphicLabel();
            string strData = Point + " : " + Val.ToString("0.0");

            label.SetXYText(_X, _Y, strData);

            label.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
            label.BackgroundColor = CogColorConstants.Black;
            label.Color = CogColorConstants.Yellow;
            return label;
        }

    }
}
