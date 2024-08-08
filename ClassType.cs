using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognex.VisionPro.Display;
using Cognex.VisionPro;
using System.Drawing;
using System.Windows.Forms;

namespace Align
{
    public class ClassType
    {
        public double[] Align_X = new double[4];
        public double[] T_Align_X = new double[4];
        public double[] O_Align_X = new double[4];

        public double[] Align_Y = new double[4];
        public double[] T_Align_Y = new double[4];
        public double[] O_Align_Y = new double[4];

        public double[] Align_T = new double[4];
        public double[] T_Align_T = new double[4];
        public double[] O_Align_T = new double[4];

        public double[] X = new double[4];
        public double[] Y = new double[4];
        public double[] S = new double[4];
        public double[] Master_Theta = new double[4];
        public double[] Master_Width = new double[2];
        public double[] Master_Height = new double[2];

        //Test
        public double[] Inspection_X = new double[4];
        public double[] Inspection_Y = new double[4];        

        public double Cal_Move_Pitch_X = 0.7;
        public double Cal_Move_Pitch_Y = 0.7;
        public double Cal_Move_Pitch_T = 0.7;

        public ClassMark[] Model = new ClassMark[4];
        public ClassAlign Align = new ClassAlign();
        public ClassEnum.Mark_Type Find_Type = ClassEnum.Mark_Type.MARK;
        public ClassEnum.Align_Result Align_Result = ClassEnum.Align_Result.OK;
        public ClassEnum.Type gType = ClassEnum.Type.Common;

        public bool HIKE = false;

        public double[] L_Check = new double[4] { 0, 0, 0, 0 };
        public double[] T_L_Check = new double[4] { 0, 0, 0, 0 };
        public double[] O_L_Cehck = new double[4] { 0, 0, 0, 0 };
        public double[] Master_L_Check = new double[4] { 0, 0, 0, 0 };

        Cognex.VisionPro.ICogImage[] cogGrabImage;
        Cognex.VisionPro.ICogAcqFifo[] camera;

        public bool Second_Grab_Use = false;
        public bool One_Camera_2Point_Use = false;

        public CogDisplay[] Display;
        public CogDisplay[] Cal_Display;
        public ClassSequence Sequence;
        Light_Control[] Type_Light;
        Main_Frame Frame;

        public ClassEnum.Cal_Data Cal_Data = new ClassEnum.Cal_Data();
        public PointF[] Cal_Data_MARK1 = new PointF[28];
        public PointF[] Cal_Data_MARK2 = new PointF[28];
        public PointF[] Cal_Data_MARK3 = new PointF[28];
        public PointF[] Cal_Data_MARK4 = new PointF[28];

        int cur_Unit;
        int Light_Count = 0;

        public bool L_Check_Use = false;
        public double L_Check_Limit = 1;

        public double Cam_Current = 0;
        public double Mark_To_Mark = 0;

        public bool T_O_Align = false;

        private void Light_Controler()
        {
            int[] CH_num = new int[4] { 0, 0, 0, 0 };
            for (int l = 1; l <= 4; l++)
            {
                string temp = ClassINI.ReadProject("Unit", "Unit" + Convert.ToString(cur_Unit) + " " + gType.ToString() + "_Light" + Convert.ToString(l));
                if (!temp.Equals(""))
                {
                    CH_num[l - 1] = Convert.ToInt16(ClassINI.ReadProject("Unit", "Unit" + Convert.ToString(cur_Unit) + " " + gType.ToString() + "_Light" + Convert.ToString(l)));
                    Light_Count++;
                }
            } 
            Type_Light = new Light_Control[Light_Count];
            int Count = 0;
            for (int l = 0; l < 4; l++)
            {
                if (!CH_num[l].Equals(0))
                {
                    Type_Light[Count] = Frame.Light_Display.CH[CH_num[l] - 1];
                    Count++;
                }
            }
        }

        public void Light_ON()
        {
            for (int i = 0; i < Light_Count; i++)
            {
                Type_Light[i].Light_ON();
                while (Type_Light[i].Auto_Light_Flag) { }
            }
        }

        public void Light_OFF()
        {
            for (int i = 0; i < Light_Count; i++)
            {
                Type_Light[i].Light_OFF();
                while (Type_Light[i].Auto_Light_Flag) { }
            }
        }

        public void Type_Setting(int _cur_Unit, ClassSequence _Sequence, Cognex.VisionPro.ICogAcqFifo[] _camera, ClassEnum.Type _gType, Main_Frame _Frame)
        {
            Frame = _Frame;
            Align.Align_Set(this);
            Sequence = _Sequence;
            Master_Width[0] = 0;
            Master_Width[1] = 0;
            Master_Height[0] = 0;
            Master_Height[1] = 0;
            cur_Unit = _cur_Unit;
            gType = _gType;
            Master_Theta_Load();
            camera = _camera;
            cogGrabImage = new Cognex.VisionPro.ICogImage[camera.Length];
            Light_Controler();
            if (ClassINI.ReadProject("Unit", "Unit" + Convert.ToString(cur_Unit) + " " + gType.ToString() + "_2nd_Grab_Use").Equals("True"))
                Second_Grab_Use = true;
            else
                Second_Grab_Use = false;
            if (ClassINI.ReadProject("Unit", "Unit" + Convert.ToString(cur_Unit) + " " + gType.ToString() + "_One_Camera_2Point_Use").Equals("True"))
                One_Camera_2Point_Use = true;
            else
                One_Camera_2Point_Use = false;
            try
            {
                Mark_To_Mark = Convert.ToDouble(ClassINI.ReadProject("L Check", "Unit" + cur_Unit.ToString() + "_" + gType.ToString() + "_" + "Mark_To_Mark"));
                Cam_Current = Convert.ToDouble(ClassINI.ReadProject("L Check", "Unit" + cur_Unit.ToString() + "_" + gType.ToString() + "_" + "Cam_Current"));
            }
            catch { }
        }

        public void Master_Theta_Calculation()
        {
            Master_Theta[0] = Align.Cal_Theta_W(Model[0].Master_X, Model[0].Master_Y,
                                                Model[1].Master_X, Model[1].Master_Y,
                                                Model[0].Rotate_Center_X, Model[0].Rotate_Center_Y,
                                                Model[1].Rotate_Center_X, Model[1].Rotate_Center_Y,
                                                ref Master_Width[0]);
            if (Second_Grab_Use)
            {
                // 모든 Point 계산후 좌하 화면과 우하 화면의 Theta 기준값 취출
                Master_Theta[1] = Align.Cal_Theta_W(Model[2].Master_X, Model[2].Master_Y,
                                                    Model[3].Master_X, Model[3].Master_Y,
                                                    Model[2].Rotate_Center_X, Model[2].Rotate_Center_Y,
                                                    Model[3].Rotate_Center_X, Model[3].Rotate_Center_Y,
                                                    ref Master_Width[1]);
                // 모든 Point 계산후 좌상 화면과 좌하 화면의 Theta 기준값 취출
                Master_Theta[2] = Align.Cal_Theta_H(Model[0].Master_X, Model[0].Master_Y,
                                                    Model[2].Master_X, Model[2].Master_Y,
                                                    Model[0].Rotate_Center_X, Model[0].Rotate_Center_Y,
                                                    Model[2].Rotate_Center_X, Model[2].Rotate_Center_Y,
                                                    ref Master_Height[0]);
                // 모든 Point 계산후 우상 화면과 우하 화면의 Theta 기준값 취출
                Master_Theta[3] = Align.Cal_Theta_H(Model[1].Master_X, Model[1].Master_Y,
                                                    Model[3].Master_X, Model[3].Master_Y,
                                                    Model[1].Rotate_Center_X, Model[1].Rotate_Center_Y,
                                                    Model[3].Rotate_Center_X, Model[3].Rotate_Center_Y,
                                                    ref Master_Height[1]);
            }
        }

        public void Master_Theta_Save()
        {
            for (int i = 0; i < 4; i++)
            {
                ClassINI.WriteConfig("Unit" + Convert.ToString(cur_Unit), "Master Theta" + Convert.ToString(i), Convert.ToString(Master_Theta[i]));
                ClassINI.WriteConfig("Unit" + Convert.ToString(cur_Unit), "Master L Check" + Convert.ToString(i), Convert.ToString(Master_L_Check[i]));
            }
            for (int i = 0; i < 2; i++)
            {
                ClassINI.WriteConfig("Unit" + Convert.ToString(cur_Unit), "Width" + Convert.ToString(i), Convert.ToString(Master_Width[i]));
                ClassINI.WriteConfig("Unit" + Convert.ToString(cur_Unit), "Height" + Convert.ToString(i), Convert.ToString(Master_Height[i]));
            }
        }

        private void Master_Theta_Load()
        {
            for (int i = 0; i < 4; i++)
                if (ClassINI.ReadConfig("Unit" + Convert.ToString(cur_Unit), "Master Theta" + Convert.ToString(i)) != "")
                    Master_Theta[i] = Convert.ToDouble(ClassINI.ReadConfig("Unit" + Convert.ToString(cur_Unit), "Master Theta" + Convert.ToString(i)));
            for (int i = 0; i < 4; i++)
                if (ClassINI.ReadConfig("Unit" + Convert.ToString(cur_Unit), "Master L Check" + Convert.ToString(i)) != "")
                    Master_L_Check[i] = Convert.ToDouble(ClassINI.ReadConfig("Unit" + Convert.ToString(cur_Unit), "Master L Check" + Convert.ToString(i)));
            for (int i = 0; i < 2; i++)
            {
                if (ClassINI.ReadConfig("Unit" + Convert.ToString(cur_Unit), "Master Theta" + Convert.ToString(i)) != "")
                {
                    Master_Width[i] = Convert.ToDouble(ClassINI.ReadConfig("Unit" + Convert.ToString(cur_Unit), "Width" + Convert.ToString(i)));
                    Master_Height[i] = Convert.ToDouble(ClassINI.ReadConfig("Unit" + Convert.ToString(cur_Unit), "Height" + Convert.ToString(i)));
                }
            }
            if (!ClassINI.ReadConfig("Find Type", "Unit" + cur_Unit.ToString() + "_" + gType.ToString()).Equals(""))
                Find_Type_Set(ClassINI.ReadConfig("Find Type", "Unit" + cur_Unit.ToString() + "_" + gType.ToString()));
            else
                Find_Type = ClassEnum.Mark_Type.MARK;
        }

        public void Find_Type_Set(string Type)
        {
            switch (Type)
            {
                case "MARK":
                    Find_Type = ClassEnum.Mark_Type.MARK;
                    break;
                case "LINE":
                    Find_Type = ClassEnum.Mark_Type.LINE;
                    break;
                case "MARK_TO_LINE":
                    Find_Type = ClassEnum.Mark_Type.MARK_TO_LINE;
                    break;
            }
            ClassINI.WriteConfig("Find Type", "Unit" + cur_Unit.ToString() + "_" + gType.ToString(), Find_Type.ToString());
        }

        private static CogGraphicLabel Mark_NG_String()
        {
            CogGraphicLabel label = new CogGraphicLabel();
            string strData = "Mark Find NG";

            label.SetXYText(0, 0, strData);

            label.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
            label.BackgroundColor = CogColorConstants.Black;
            label.Color = CogColorConstants.Red;
            label.SelectedSpaceName = "*";
            return label;
        }

        public void Auto_Mark_FInd()
        {
            for (int i = 0; i < 4; i++)
            //Parallel.For(0, 4, i =>
            {
                Align_Result = ClassEnum.Align_Result.OK;
                Align_X[i] = 0;
                Align_Y[i] = 0;
                X[i] = 0;
                Y[i] = 0;
                S[i] = 0;
                if (Find_Type.Equals(ClassEnum.Mark_Type.MARK) || Find_Type.Equals(ClassEnum.Mark_Type.MARK_TO_LINE))
                {
                    for (int l = 0; l < 4; l++)
                    {
                        if (!One_Camera_2Point_Use)
                        {
                            Model[i].Enable_Mark_Find(Display[i].Image, Display[i].InteractiveGraphics, l, ref X[i], ref Y[i], ref S[i]);
                            if (S[i] >= Model[i].Match_Rate)
                                goto Label_OK;
                        }
                        //21.01.13
                        #region One Grab을 위한 부분
                        //else if(!One_Camera_2Point_Use && Frame.Setup_Display.T_O_checkBox.Checked && Frame.Unit[1].Object_Use == true
                        //     || !One_Camera_2Point_Use && Frame.Setup_Display.T_O_checkBox.Checked && Frame.Unit[2].Object_Use == true)
                        //{
                        //    Model[i].Enable_Mark_Find(Display[(i / 2) * 2].Image, Display[(i / 2) * 2].InteractiveGraphics, l, ref X[i], ref Y[i], ref S[i]);
                        //    if (S[i] >= Model[i].Match_Rate)
                        //        goto Label_OK;
                        //}
                        #endregion

                        //CCTV Lenz 용
                        else
                        {
                            Model[i].Enable_Mark_Find(Display[(i / 2) * 2].Image, Display[(i / 2) * 2].InteractiveGraphics, l, ref X[i], ref Y[i], ref S[i]);
                            if (S[i] >= Model[i].Match_Rate)
                                goto Label_OK;
                        }

                    }
                    CogGraphicLabel Lable = Mark_NG_String();
                    Lable.Font = new System.Drawing.Font("a", 12);
                    Display[i].InteractiveGraphics.Add(Lable, "Label", false);
                    Align_Result = ClassEnum.Align_Result.NG;
                }
                else if (Find_Type.Equals(ClassEnum.Mark_Type.LINE))
                {
                    if (!One_Camera_2Point_Use)
                    {
                        Model[i].Enable_Line_Find(Display[i].Image, Display[i].InteractiveGraphics, ref X[i], ref Y[i]);
                    }
                    else
                    {
                        Model[i].Enable_Line_Find(Display[i / 2].Image, Display[i / 2].InteractiveGraphics, ref X[i], ref Y[i]);
                    }
                }
            Label_OK:
                if (Find_Type.Equals(ClassEnum.Mark_Type.MARK_TO_LINE) && !Align_Result.Equals(ClassEnum.Align_Result.NG))
                {
                    if (!One_Camera_2Point_Use)
                    {
                        Display[i].InteractiveGraphics.Clear();
                        if (Model[i].Enable_Line_Find(Model[i].Mark_Fixture(Display[i].Image), Display[i].InteractiveGraphics, ref X[i], ref Y[i]))
                        {
                            Align_Result = ClassEnum.Align_Result.NG;
                        }
                    }
                    else
                    {
                        Display[i / 2].InteractiveGraphics.Clear();
                        if (Model[i].Enable_Line_Find(Model[i].Mark_Fixture(Display[i / 2].Image), Display[i / 2].InteractiveGraphics, ref X[i], ref Y[i]))
                        {
                            Align_Result = ClassEnum.Align_Result.NG;
                        }
                    }
                }
                if (!Align_Result.Equals(ClassEnum.Align_Result.NG))
                {
                    Align_X[i] = X[i];
                    Align_Y[i] = Y[i];
                }
                else
                {
                    Align_X[i] = 0;
                    Align_Y[i] = 0;
                }

            //});
            }
            if ((Sequence.Sequences).Equals(ClassEnum.Sequence.Align))
            {
                Sequence.Align_Sequence = ClassEnum.Align_Sequence.Align;
            }
        }

        public void Inspection_Mark_FInd(CogDisplay[] _DIsplay)
        {
            int count = 0;
            if (Frame.Unit[cur_Unit].Type[(int)ClassEnum.Type.Target].Second_Grab_Use)
                count = 4;
            else
                count = 2;
            for (int i = 0; i < count; i++)
            {
                Align_Result = ClassEnum.Align_Result.OK;
                Align_X[i] = 0;
                Align_Y[i] = 0;
                X[i] = 0;
                Y[i] = 0;
                S[i] = 0;
                if (Find_Type.Equals(ClassEnum.Mark_Type.MARK) || Find_Type.Equals(ClassEnum.Mark_Type.MARK_TO_LINE))
                {
                    for (int l = 0; l < 4; l++)
                    {
                        if (!One_Camera_2Point_Use)
                        {
                            Model[i].Enable_Mark_Find(_DIsplay[i].Image, _DIsplay[i].InteractiveGraphics, l, ref X[i], ref Y[i], ref S[i]);
                            if (S[i] >= Model[i].Match_Rate)
                                goto Label_OK;
                        }
                        else
                        {
                            Model[i].Enable_Mark_Find(_DIsplay[(i / 2) * 2].Image, _DIsplay[(i / 2) * 2].InteractiveGraphics, l, ref X[i], ref Y[i], ref S[i]);
                            if (S[i] >= Model[i].Match_Rate)
                                goto Label_OK;
                        }
                    }
                    CogGraphicLabel Lable = Mark_NG_String();
                    Lable.Font = new System.Drawing.Font("a", 12);
                    _DIsplay[i].InteractiveGraphics.Add(Lable, "Label", false);
                    Align_Result = ClassEnum.Align_Result.NG;
                }
                else if (Find_Type.Equals(ClassEnum.Mark_Type.LINE))
                {
                    if (!One_Camera_2Point_Use)
                    {
                        Model[i].Enable_Line_Find(_DIsplay[i].Image, _DIsplay[i].InteractiveGraphics, ref X[i], ref Y[i]);
                    }
                    else
                    {
                        Model[i].Enable_Line_Find(_DIsplay[i / 2].Image, _DIsplay[i / 2].InteractiveGraphics, ref X[i], ref Y[i]);
                    }
                }
            Label_OK:
                if (Find_Type.Equals(ClassEnum.Mark_Type.MARK_TO_LINE) && !Align_Result.Equals(ClassEnum.Align_Result.NG))
                {
                    if (!One_Camera_2Point_Use)
                    {
                        if (Model[i].Enable_Line_Find(Model[i].Mark_Fixture(_DIsplay[i].Image), _DIsplay[i].InteractiveGraphics, ref X[i], ref Y[i]))
                        {
                            Align_Result = ClassEnum.Align_Result.NG;
                        }
                    }
                    else
                    {
                        if (Model[i].Enable_Line_Find(Model[i].Mark_Fixture(_DIsplay[i / 2].Image), _DIsplay[i / 2].InteractiveGraphics, ref X[i], ref Y[i]))
                        {
                            Align_Result = ClassEnum.Align_Result.NG;
                        }
                    }
                }
                if (!Align_Result.Equals(ClassEnum.Align_Result.NG))
                {
                    Align_X[i] = X[i];
                    Align_Y[i] = Y[i];
                }
                else
                {
                    Align_X[i] = 0;
                    Align_Y[i] = 0;
                }

            }
            if ((Sequence.Sequences).Equals(ClassEnum.Sequence.Align))
            {
                Sequence.Align_Sequence = ClassEnum.Align_Sequence.Align;
            }
            else if ((Sequence.Sequences).Equals(ClassEnum.Sequence.Inspection))
            {
                Sequence.Inspection_Sequence = ClassEnum.Inspection_Sequence.Inspection;
            }
        }

        public bool Auto_Grab(int _Two)
        {
            //1차 위치에서 리얼라인이 아니거나 2차 위치에서 리얼라인 할 때
            if ((_Two == 0 && !Frame.Unit[cur_Unit].Sequence.Re_Align_2_1) || (_Two == 2 && Frame.Unit[cur_Unit].Sequence.Re_Align_2_1))
            {
                Display[0].Image = null;
                Display[1].Image = null;
                Display[2].Image = null;
                Display[3].Image = null;
                Display[0].InteractiveGraphics.Clear();
                Display[1].InteractiveGraphics.Clear();
                Display[2].InteractiveGraphics.Clear();
                Display[3].InteractiveGraphics.Clear();
            }

            int tickCount = Environment.TickCount;
            ///////////////////////////////////////////////////////0.2초 딜레이 시트라미 떨림 보정
            //while (true)
            //{
            //    if (Environment.TickCount - tickCount > 200)
            //    {
            //        break;
            //    }
            //}
            /////////////////////////////////////////////////////////
            if (!One_Camera_2Point_Use)
            {
                Parallel.For(0, 2, i =>
                {
                    if (Frame.Setup_Display.Cam_Simul)
                    {
                        Display[i + _Two].Image = Model[i + _Two].Mark[0].Pattern.TrainImage;
                    }
                    else
                    {
                        ICogImage _Image = Grab_Image(i, _Two, i);
                        if (Frame.Setup_Display.Auto_Checker_Borad.Checked)
                            Display[i + _Two].Image = Model[i + _Two].CalibChecker_Run(_Image);
                        else
                            //1차 위치(_Two = 0) 0,1 / 2차 위치(_Two = 1) 2,3
                            Display[i + _Two].Image = _Image;
                    }
                });
            }
            else
            {
                Display[0 + _Two].InteractiveGraphics.Clear();
                //Display[0 + _Two].Image = Grab_Image(0, _Two);
                //Display[0 + _Two].Image = Model[0].CalibChecker_Run(Grab_Image(0 + (int)gType, _Two));
                Display[0 + _Two].Image = Grab_Image(0 + (int)gType, _Two, 0);
            }
            if ((Sequence.Sequences).Equals(ClassEnum.Sequence.Align))
            {
                if (Sequence.Align_Sequence.Equals(ClassEnum.Align_Sequence.Align_Start_Ack))
                    Sequence.Align_Sequence = ClassEnum.Align_Sequence.Grab_1st_OK;
                else
                    Sequence.Align_Sequence = ClassEnum.Align_Sequence.Grab_2nd_OK;
            }
            else if ((Sequence.Sequences).Equals(ClassEnum.Sequence.Inspection))
            {
                if (Sequence.Inspection_Sequence.Equals(ClassEnum.Inspection_Sequence.Inspecion_Start_Ack))
                    Sequence.Inspection_Sequence = ClassEnum.Inspection_Sequence.Grab_1st_OK;
                else if (Sequence.Inspection_Sequence.Equals(ClassEnum.Inspection_Sequence.Move_2nd_Req))
                    Sequence.Inspection_Sequence = ClassEnum.Inspection_Sequence.Grab_2nd_OK;
            }
                return false;
        }

        //그랩 하는 부분
        //21.02.02 hike 여부 확인 후 grab
        public Cognex.VisionPro.ICogImage Grab_Image(int Camera_Num, int _Two, int _Mark)
        {
            Cognex.VisionPro.ICogImage _return_Image = null;
            if (Frame.Setup_Display.Unit2_HIKE.Checked == true)
            {
                try
                {
                    int trigger;
                    Model[Camera_Num + _Two].Parameter_Set(camera[Camera_Num], HIKE); //일단은 타겟 찰상따라간다
                    cogGrabImage[Camera_Num] = camera[Camera_Num].Acquire(out trigger);

                    _return_Image = cogGrabImage[Camera_Num];
                }
                catch { MessageBox.Show("그랩 실패"); }
            }
            else
            {
                try
                {
                    int trigger;
                    Model[Camera_Num + _Two].Parameter_Set(camera[Camera_Num], false); //일단은 타겟 찰상따라간다
                    cogGrabImage[Camera_Num] = camera[Camera_Num].Acquire(out trigger);

                    _return_Image = cogGrabImage[Camera_Num];
                }
                catch { MessageBox.Show("그랩 실패"); }
            }
            

            return _return_Image;
        }

        public bool Auto_Calibration(ref PointF _L, ref PointF _R, int _Two)
        {
            double _X = 0, _Y = 0, _S = 0;
            /////////////////////////////////////////////////////////////////////////////////////
            int tickCount = Environment.TickCount; // 0.2초 딜레이 시트라미 떨림 보정
            while (true)
            {
                if (Environment.TickCount - tickCount > 200)
                {
                    break;
                }
            }
            /////////////////////////////////////////////////////////////////////////////////////
            if (!One_Camera_2Point_Use)
            {
                Parallel.For(0, 2, i =>
                {
                    if (Frame.Setup_Display.Cam_Simul)
                    {
                        Display[i + _Two].Image = Model[i + _Two].Mark[0].Pattern.TrainImage;
                    }
                    else
                    {
                        ICogImage _Image = Grab_Image(i, _Two, i);
                        if (Frame.Setup_Display.Auto_Checker_Borad.Checked)
                            Cal_Display[i].Image = Model[i + _Two].CalibChecker_Run(_Image);
                        else
                            Cal_Display[i].Image = _Image;
                    }

                });
            }
            else
            {
                //Cal_Display[0].Image = Grab_Image(0, _Two);
                Cal_Display[0].Image = Model[0].CalibChecker_Run(Grab_Image(0, _Two, 0));
            }

            for (int i = 0; i < 2; i++)
            {
                if (!One_Camera_2Point_Use)
                {
                    Model[i + _Two].Enable_Mark_Find(Cal_Display[i].Image, Cal_Display[i].InteractiveGraphics, 5, ref _X, ref _Y, ref _S);
                    if ((i % 2).Equals(0))
                    {
                        _L.X = (float)_X; _L.Y = (float)_Y;
                    }
                    else
                    {
                        _R.X = (float)_X; _R.Y = (float)_Y;
                    }
                }
                else
                {
                    Model[i + _Two].Enable_Mark_Find(Cal_Display[i / 2].Image, Cal_Display[i / 2].InteractiveGraphics, 5, ref _X, ref _Y, ref _S);
                    if ((i % 2).Equals(0))
                    {
                        _L.X = (float)_X; _L.Y = (float)_Y;
                    }
                    else
                    {
                        _R.X = (float)_X; _R.Y = (float)_Y;
                    }
                }
            }
            if ((Sequence.Sequences).Equals(ClassEnum.Sequence.Calibration))
            {
                if (_Two.Equals(0))
                    //1차 촬상 위치
                    Sequence.Calibration_Sequence = ClassEnum.Calibration_Sequence.Mark1_Serch_OK;
                else
                    //2차 촬상 위치
                    Sequence.Calibration_Sequence = ClassEnum.Calibration_Sequence.Mark2_Serch_OK;
            }
            return false;
        }

        public int count = 0;
        public double R_T_X = 0;
        public double R_O_X = 0;
        public double R_T_Y = 0;
        public double R_O_Y = 0;

        //21.02.03 수정
        //21.02.09 수정 (Align_Height 및 CamTeaching 값 및 Shuttle Address 변경에 의해 값 변경)
        public bool Align_Start(ref double _X, ref double _Y, ref double _T)
        {
            bool T_Point_Use = false;
            //double Cam_Teaching = (((Frame.PLC.Receive_Data[(cur_Unit) * 10 + 28]) | (Frame.PLC.Receive_Data[(cur_Unit) * 10 + 29] << 16))) / 10;
            //ex) D60050 : Cam_Teaching x 일 때, 1 * 10 + 40 = 50 / D60134 : Shuttle Y 일 때, 1 * 10 + 124 = 134 (interface Map에 맞춰 작성할 수 있도록)
            //double Cam_Teaching = (((Frame.PLC.Receive_Data[(cur_Unit) * 10 + 40]) | (Frame.PLC.Receive_Data[(cur_Unit) * 10 + 41] << 16))) / 10;
            //A열
            double Cam_Teaching = (((Frame.PLC.Receive_Data[(cur_Unit) * 30 + 20]) | (Frame.PLC.Receive_Data[(cur_Unit) * 30 + 21] << 16))) / 10;
            int Shuttle_Y1 = (((Frame.PLC.Receive_Data[(cur_Unit) * 30 + 18]) | (Frame.PLC.Receive_Data[(cur_Unit) * 30 + 19] << 16))) / 10;
            //B열
            //double Cam_Teaching = (((Frame.PLC.Receive_Data[(cur_Unit) * 30 + 140]) | (Frame.PLC.Receive_Data[(cur_Unit) * 30 + 141] << 16))) / 10;
            //int Shuttle_Y1 = (((Frame.PLC.Receive_Data[(cur_Unit) * 30 + 138]) | (Frame.PLC.Receive_Data[(cur_Unit) * 30 + 139] << 16))) / 10;
            //int Shuttle_Y2 = (((Frame.PLC.Receive_Data[(cur_Unit) * 10 + 106]) | (Frame.PLC.Receive_Data[(cur_Unit) * 10 + 107] << 16))) / 10;
            //int Shuttle_Y1 = (((Frame.PLC.Receive_Data[(cur_Unit) * 10 + 124]) | (Frame.PLC.Receive_Data[(cur_Unit) * 10 + 125] << 16))) / 10;
            //int Shuttle_Y2 = (((Frame.PLC.Receive_Data[(cur_Unit) * 10 + 126]) | (Frame.PLC.Receive_Data[(cur_Unit) * 10 + 127] << 16))) / 10;
            //Mark Find 찾은 Count
            int count = 0;
            for (int i = 0; i < 4; i++)
                L_Check[i] = 0;
            if (Second_Grab_Use)
                count = 4;
            else
                count = 2;

            int count_X = 0;
            int X_Number = 0;
            for (int i = 0; i < count; i++)
                if (Align_X[i].Equals(0))
                {
                    count_X++;
                    X_Number = i;
                }
            //count 다 찾거나 1개 못찾거나 T_Point_Align = 3Point, count_X = 못 찾은 갯수
            if ((count - count_X).Equals(count) || (Frame.Setup_Display.T_Point_Align_CheckBox.Checked && count_X.Equals(1) && count.Equals(4)))
            {
                double _sum = 0;
                
                if (Second_Grab_Use)
                {   //21.01.12
                    if(Frame.Setup_Display.T_O_checkBox.Checked && Frame.Unit[cur_Unit].Object_Use == true && cur_Unit == 3)
                    {
                        //4개 다 찾았을 때
                        if (!Align_X[0].Equals(0) && !Align_X[1].Equals(0) && !Align_X[2].Equals(0) && !Align_X[3].Equals(0))
                        {
                            double T_T = 0;
                            double O_T = 0;

                            for (int i = 0; i < 2; i++)
                            {
                                if (i == 0)
                                {
                                    gType = ClassEnum.Type.Target;

                                    T_L_Check[0] = Align.Align_Width(this, 0, 1, Cam_Teaching);
                                    T_L_Check[1] = Align.Align_Width(this, 2, 3, Cam_Teaching);
                                    //Y축이 1개 일때
                                    T_L_Check[2] = Align.Align_Height(this, 0, 2, Shuttle_Y1);
                                    T_L_Check[3] = Align.Align_Height(this, 1, 3, Shuttle_Y1);
                                    //Y 축이 2개 일 때
                                    //T_L_Check[2] = Align.Align_Height_2(this, 0, 2, Shuttle_Y1, Shuttle_Y2);
                                    //T_L_Check[3] = Align.Align_Height_2(this, 1, 3, Shuttle_Y1, Shuttle_Y2);
                                    T_Align_T[0] = Align.Align_Theta_W(Model[0].Master_Y - Align_Y[0], Model[1].Master_Y - Align_Y[1], Master_Width[0]);
                                    T_Align_T[1] = Align.Align_Theta_W(Model[2].Master_Y - Align_Y[2], Model[3].Master_Y - Align_Y[3], Master_Width[1]);
                                    T_Align_T[2] = -(Align.Align_Theta_W(Model[0].Master_X - Align_X[0], Model[2].Master_X - Align_X[2], Master_Height[0]));
                                    T_Align_T[3] = -(Align.Align_Theta_W(Model[1].Master_X - Align_X[1], Model[3].Master_X - Align_X[3], Master_Height[1]));

                                    _sum = 0;
                                    for (int j = 0; j < 4; j++)
                                        _sum += T_Align_T[j];
                                    T_T = _sum / 4;
                                }
                                else
                                {
                                    gType = ClassEnum.Type.Object;

                                    O_L_Cehck[0] = Align.Align_Width(this, 4, 5, Cam_Teaching);
                                    O_L_Cehck[1] = Align.Align_Width(this, 6, 7, Cam_Teaching);
                                    //Y축이 1개 일때
                                    O_L_Cehck[2] = Align.Align_Height(this, 0, 2, Shuttle_Y1);
                                    O_L_Cehck[3] = Align.Align_Height(this, 1, 3, Shuttle_Y1);
                                    //Y축이 2개 일때
                                    //O_L_Cehck[2] = Align.Align_Height_2(this, 4, 6, Shuttle_Y1, Shuttle_Y2);
                                    //O_L_Cehck[3] = Align.Align_Height_2(this, 5, 7, Shuttle_Y1, Shuttle_Y2);
                                    O_Align_T[0] = Align.Align_Theta_W(Model[0].Master_Y - Align_Y[0], Model[1].Master_Y - Align_Y[1], Master_Width[0]);
                                    O_Align_T[1] = Align.Align_Theta_W(Model[2].Master_Y - Align_Y[2], Model[3].Master_Y - Align_Y[3], Master_Width[1]);
                                    O_Align_T[2] = -(Align.Align_Theta_W(Model[0].Master_X - Align_X[0], Model[2].Master_X - Align_X[2], Master_Height[0]));
                                    O_Align_T[3] = -(Align.Align_Theta_W(Model[1].Master_X - Align_X[1], Model[3].Master_X - Align_X[3], Master_Height[1]));

                                    _sum = 0;
                                    for (int j = 0; j < 4; j++)
                                        _sum += O_Align_T[j];
                                    O_T = _sum / 4;
                                }
                            }
                            /**
                            //if (gType == ClassEnum.Type.Target)
                            //{
                                
                            //    T_L_Check[0] = Align.Align_Width(this, 0, 1, Cam_Teaching);
                            //    T_L_Check[1] = Align.Align_Width(this, 2, 3, Cam_Teaching);
                            //    T_L_Check[2] = Align.Align_Height(this, 0, 2, Shuttle_Y1, Shuttle_Y2);
                            //    T_L_Check[3] = Align.Align_Height(this, 1, 3, Shuttle_Y1, Shuttle_Y2);
                            //    T_Align_T[0] = Align.Align_Theta_W(Model[0].Master_Y - Align_Y[0], Model[1].Master_Y - Align_Y[1], Master_Width[0]);
                            //    T_Align_T[1] = Align.Align_Theta_W(Model[2].Master_Y - Align_Y[2], Model[3].Master_Y - Align_Y[3], Master_Width[1]);
                            //    T_Align_T[2] = -(Align.Align_Theta_W(Model[0].Master_X - Align_X[0], Model[2].Master_X - Align_X[2], Master_Height[0]));
                            //    T_Align_T[3] = -(Align.Align_Theta_W(Model[1].Master_X - Align_X[1], Model[3].Master_X - Align_X[3], Master_Height[1]));

                            //    _sum = 0;
                            //    for (int i = 0; i < 4; i++)
                            //        _sum += T_Align_T[i];
                            //    T_T = _sum / 4;
                            //}
                            //else if(gType == ClassEnum.Type.Object)
                            //{

                            //    O_L_Cehck[0] = Align.Align_Width(this, 4, 5, Cam_Teaching);
                            //    O_L_Cehck[1] = Align.Align_Width(this, 6, 7, Cam_Teaching);
                            //    O_L_Cehck[2] = Align.Align_Height(this, 4, 6, Shuttle_Y1, Shuttle_Y2);
                            //    O_L_Cehck[3] = Align.Align_Height(this, 5, 7, Shuttle_Y1, Shuttle_Y2);
                            //    O_Align_T[0] = Align.Align_Theta_W(Model[0].Master_Y - Align_Y[0], Model[1].Master_Y - Align_Y[1], Master_Width[0]);
                            //    O_Align_T[1] = Align.Align_Theta_W(Model[2].Master_Y - Align_Y[2], Model[3].Master_Y - Align_Y[3], Master_Width[1]);
                            //    O_Align_T[2] = -(Align.Align_Theta_W(Model[0].Master_X - Align_X[0], Model[2].Master_X - Align_X[2], Master_Height[0]));
                            //    O_Align_T[3] = -(Align.Align_Theta_W(Model[1].Master_X - Align_X[1], Model[3].Master_X - Align_X[3], Master_Height[1]));

                            //    _sum = 0;
                            //    for (int i = 0; i < 4; i++)
                            //        _sum += O_Align_T[i];
                            //    O_T = _sum / 4;
                            //}
            **/

                            _T = (T_T + O_T) / 2;

                        }
                        // 3Point가 켜져있을 때, 3개 Point만 잡았을 때
                        else if (Frame.Setup_Display.T_Point_Align_CheckBox.Checked)
                        {
                            T_Point_Use = true;
                            switch (X_Number)
                            {
                                case 0:
                                    L_Check[1] = Align.Align_Width(this, 2, 3, Cam_Teaching);
                                    //Y축이 1개 일때
                                    L_Check[3] = Align.Align_Height(this, 1, 3, Shuttle_Y1);
                                    //Y축이 2개 일 때
                                    //L_Check[3] = Align.Align_Height_2(this, 1, 3, Shuttle_Y1, Shuttle_Y2);
                                    Align_T[0] = 0;
                                    Align_T[1] = Align.Align_Theta_W((Model[2].Master_Y - Align_Y[2]) * Model[2].resolution_Y, (Model[3].Master_Y - Align_Y[3]) * Model[3].resolution_Y, L_Check[1]);
                                    Align_T[2] = 0;
                                    Align_T[3] = -(Align.Align_Theta_H((Model[1].Master_X - Align_X[1]) * Model[1].resolution_X, (Model[3].Master_X - Align_X[3]) * Model[3].resolution_X, L_Check[3]));
                                    break;
                                case 1:
                                    L_Check[1] = Align.Align_Width(this, 2, 3, Cam_Teaching);
                                    //Y축이 1개 일때
                                    L_Check[2] = Align.Align_Height(this, 0, 2, Shuttle_Y1);
                                    //Y축이 2개 일때
                                    //L_Check[2] = Align.Align_Height_2(this, 0, 2, Shuttle_Y1, Shuttle_Y2);
                                    Align_T[0] = 0;
                                    Align_T[1] = Align.Align_Theta_W((Model[2].Master_Y - Align_Y[2]) * Model[2].resolution_Y, (Model[3].Master_Y - Align_Y[3]) * Model[3].resolution_Y, L_Check[1]);
                                    Align_T[2] = -(Align.Align_Theta_H((Model[0].Master_X - Align_X[0]) * Model[0].resolution_X, (Model[2].Master_X - Align_X[2]) * Model[2].resolution_X, L_Check[2]));
                                    Align_T[3] = 0;
                                    break;
                                case 2:
                                    L_Check[0] = Align.Align_Width(this, 0, 1, Cam_Teaching);
                                    //Y축이 1개 일때
                                    L_Check[3] = Align.Align_Height(this, 1, 3, Shuttle_Y1);
                                    //Y축이 2개 일 때
                                    //L_Check[3] = Align.Align_Height_2(this, 1, 3, Shuttle_Y1, Shuttle_Y2);
                                    Align_T[0] = Align.Align_Theta_W((Model[0].Master_Y - Align_Y[0]) * Model[0].resolution_Y, (Model[1].Master_Y - Align_Y[1]) * Model[1].resolution_Y, L_Check[0]);
                                    Align_T[1] = 0;
                                    Align_T[2] = 0;
                                    Align_T[3] = -(Align.Align_Theta_H((Model[1].Master_X - Align_X[1]) * Model[1].resolution_X, (Model[3].Master_X - Align_X[3]) * Model[3].resolution_X, L_Check[3]));
                                    break;
                                case 3:
                                    L_Check[0] = Align.Align_Width(this, 0, 1, Cam_Teaching);
                                    //Y축이 1개 일때
                                    L_Check[2] = Align.Align_Height(this, 0, 2, Shuttle_Y1);
                                    //Y축이 2개 일 때
                                    //L_Check[2] = Align.Align_Height_2(this, 0, 2, Shuttle_Y1, Shuttle_Y2);
                                    Align_T[0] = Align.Align_Theta_W((Model[0].Master_Y - Align_Y[0]) * Model[0].resolution_Y, (Model[1].Master_Y - Align_Y[1]) * Model[1].resolution_Y, L_Check[0]);
                                    Align_T[1] = 0;
                                    Align_T[2] = -(Align.Align_Theta_H((Model[0].Master_X - Align_X[0]) * Model[0].resolution_X, (Model[2].Master_X - Align_X[2]) * Model[2].resolution_X, L_Check[2]));
                                    Align_T[3] = 0;
                                    break;
                            }
                            _sum = 0;
                            for (int i = 0; i < 4; i++)
                                _sum += Align_T[i];
                            _T = _sum / 2;
                        }
                    }
                    
                    else
                    {   //4개 다 찾았을 때
                        if (!Align_X[0].Equals(0) && !Align_X[1].Equals(0) && !Align_X[2].Equals(0) && !Align_X[3].Equals(0))
                        {
                            L_Check[0] = Align.Align_Width(this, 0, 1, Cam_Teaching);
                            L_Check[1] = Align.Align_Width(this, 2, 3, Cam_Teaching);

                            //Y축이 1개 일 때
                            L_Check[2] = Align.Align_Height(this, 0, 2, Shuttle_Y1);
                            L_Check[3] = Align.Align_Height(this, 1, 3, Shuttle_Y1);

                            //Y축이 2개 일때
                            //L_Check[2] = Align.Align_Height_2(this, 0, 2, Shuttle_Y1, Shuttle_Y2);
                            //L_Check[3] = Align.Align_Height_2(this, 1, 3, Shuttle_Y1, Shuttle_Y2);
                            Align_T[0] = Align.Align_Theta_W(Model[0].Master_Y - Align_Y[0], Model[1].Master_Y - Align_Y[1], Master_Width[0]);
                            Align_T[1] = Align.Align_Theta_W(Model[2].Master_Y - Align_Y[2], Model[3].Master_Y - Align_Y[3], Master_Width[1]);
                            Align_T[2] = -(Align.Align_Theta_W(Model[0].Master_X - Align_X[0], Model[2].Master_X - Align_X[2], Master_Height[0]));
                            Align_T[3] = -(Align.Align_Theta_W(Model[1].Master_X - Align_X[1], Model[3].Master_X - Align_X[3], Master_Height[1]));

                            _sum = 0;
                            for (int i = 0; i < 4; i++)
                                _sum += Align_T[i];
                            _T = _sum / 4;
                        }
                        // 3Point가 켜져있을 때, 3개 Point만 잡았을 때
                        else if (Frame.Setup_Display.T_Point_Align_CheckBox.Checked)
                        {
                            T_Point_Use = true;
                            switch (X_Number)
                            {
                                case 0:
                                    L_Check[1] = Align.Align_Width(this, 2, 3, Cam_Teaching);
                                    //Y축이 1개 일 때
                                    L_Check[3] = Align.Align_Height(this, 1, 3, Shuttle_Y1);
                                    //Y축이 2개 일 때
                                    //L_Check[3] = Align.Align_Height_2(this, 1, 3, Shuttle_Y1, Shuttle_Y2);
                                    Align_T[0] = 0;
                                    Align_T[1] = Align.Align_Theta_W((Model[2].Master_Y - Align_Y[2]) * Model[2].resolution_Y, (Model[3].Master_Y - Align_Y[3]) * Model[3].resolution_Y, L_Check[1]);
                                    Align_T[2] = 0;
                                    Align_T[3] = -(Align.Align_Theta_H((Model[1].Master_X - Align_X[1]) * Model[1].resolution_X, (Model[3].Master_X - Align_X[3]) * Model[3].resolution_X, L_Check[3]));
                                    break;
                                case 1:
                                    L_Check[1] = Align.Align_Width(this, 2, 3, Cam_Teaching);
                                    //Y축이 1개 일 때
                                    L_Check[2] = Align.Align_Height(this, 0, 2, Shuttle_Y1);
                                    //Y축이 2개 일 때
                                    //L_Check[2] = Align.Align_Height_2(this, 0, 2, Shuttle_Y1, Shuttle_Y2);
                                    Align_T[0] = 0;
                                    Align_T[1] = Align.Align_Theta_W((Model[2].Master_Y - Align_Y[2]) * Model[2].resolution_Y, (Model[3].Master_Y - Align_Y[3]) * Model[3].resolution_Y, L_Check[1]);
                                    Align_T[2] = -(Align.Align_Theta_H((Model[0].Master_X - Align_X[0]) * Model[0].resolution_X, (Model[2].Master_X - Align_X[2]) * Model[2].resolution_X, L_Check[2]));
                                    Align_T[3] = 0;
                                    break;
                                case 2:
                                    L_Check[0] = Align.Align_Width(this, 0, 1, Cam_Teaching);
                                    //Y축이 1개 일 때
                                    L_Check[3] = Align.Align_Height(this, 1, 3, Shuttle_Y1);
                                    //Y축이 2개 일 때
                                    //L_Check[3] = Align.Align_Height_2(this, 1, 3, Shuttle_Y1, Shuttle_Y2);
                                    Align_T[0] = Align.Align_Theta_W((Model[0].Master_Y - Align_Y[0]) * Model[0].resolution_Y, (Model[1].Master_Y - Align_Y[1]) * Model[1].resolution_Y, L_Check[0]);
                                    Align_T[1] = 0;
                                    Align_T[2] = 0;
                                    Align_T[3] = -(Align.Align_Theta_H((Model[1].Master_X - Align_X[1]) * Model[1].resolution_X, (Model[3].Master_X - Align_X[3]) * Model[3].resolution_X, L_Check[3]));
                                    break;
                                case 3:
                                    L_Check[0] = Align.Align_Width(this, 0, 1, Cam_Teaching);
                                    //Y축이 1개 일 때
                                    L_Check[2] = Align.Align_Height(this, 0, 2, Shuttle_Y1);
                                    //Y축이 2개 일 때
                                    //L_Check[2] = Align.Align_Height_2(this, 0, 2, Shuttle_Y1, Shuttle_Y2);
                                    Align_T[0] = Align.Align_Theta_W((Model[0].Master_Y - Align_Y[0]) * Model[0].resolution_Y, (Model[1].Master_Y - Align_Y[1]) * Model[1].resolution_Y, L_Check[0]);
                                    Align_T[1] = 0;
                                    Align_T[2] = -(Align.Align_Theta_H((Model[0].Master_X - Align_X[0]) * Model[0].resolution_X, (Model[2].Master_X - Align_X[2]) * Model[2].resolution_X, L_Check[2]));
                                    Align_T[3] = 0;
                                    break;
                            }
                            _sum = 0;
                            for (int i = 0; i < 4; i++)
                                _sum += Align_T[i];
                            _T = _sum / 2;
                        }
                    }                    
                }
                
                //2Point만 찾았을 때
                else
                {
                    if(cur_Unit == 1)
                    {
                        L_Check[0] = Align.Align_Width(this, 0, 1, Cam_Teaching);

                        Align_T[0] = Align.Align_Theta_W((Model[0].Master_Y - Align_Y[0]) * Model[0].resolution_Y, (Model[1].Master_Y - Align_Y[1]) * Model[1].resolution_Y, L_Check[0]);
                        _T = Align_T[0];

                    }
                    else if(cur_Unit == 2 && Frame.Unit[cur_Unit].Object_Use == true && Frame.Setup_Display.T_O_checkBox.Checked)
                    {
                        double T_T = 0;
                        double O_T = 0;

                        for (int i = 0; i < 2; i++)
                        {
                            if (i == 0)
                            {
                                T_L_Check[0] = Align.Align_Width(this, 0, 1, Cam_Teaching);
                                T_Align_T[0] = Align.Align_Theta_W(Model[0].Master_Y - Align_Y[0], Model[1].Master_Y - Align_Y[1], Master_Width[0]);

                                T_T = T_Align_T[0];
                            }
                            else
                            {
                                O_L_Cehck[0] = Align.Align_Width(this, 0, 1, Cam_Teaching);
                                O_Align_T[0] = Align.Align_Theta_W(Model[0].Master_Y - Align_Y[0], Model[1].Master_Y - Align_Y[1], Master_Width[0]);

                                O_T = O_Align_T[0];
                            }
                        }

                        /**
                        //        if (gType == ClassEnum.Type.Target)
                        //{

                        //    T_L_Check[0] = Align.Align_Width(this, 0, 1, Cam_Teaching);                           
                        //    T_Align_T[0] = Align.Align_Theta_W(Model[0].Master_Y - Align_Y[0], Model[1].Master_Y - Align_Y[1], Master_Width[0]);                            

                        //    T_T = T_Align_T[0];
                        //}
                        //else if (gType == ClassEnum.Type.Object)
                        //{

                        //    O_L_Cehck[0] = Align.Align_Width(this, 0, 1, Cam_Teaching);                           
                        //    O_Align_T[0] = Align.Align_Theta_W(Model[0].Master_Y - Align_Y[0], Model[1].Master_Y - Align_Y[1], Master_Width[0]);                            

                        //    O_T = O_Align_T[0];
                        //}
                         **/
  
                        _T = (T_T + O_T) / 2;

                    }                    
                }
                
                if(Frame.Setup_Display.T_O_checkBox.Checked)
                {
                    Point S = new Point(0, 0);
                    Point C = new Point(0, 0);

                    double T_X = 0;
                    double T_Y = 0;
                    double O_X = 0;
                    double O_Y = 0;

                    //21.01.27 
                    for (int i = 0; i < 2; i++)
                    {
                        if (i == 0)
                        {
                            Point[] R_T = new Point[count];

                            for (int j = 0; j < count; j++)
                            {
                                if (!Align_X[j].Equals(0))
                                {
                                    #region
                                    //S.X = (int)Align_X[i];
                                    //S.Y = (int)Align_Y[i];
                                    //C.X = (int)Model[i].Rotate_Center_X;//Model (Camera 별 회전 중심)
                                    //C.Y = (int)Model[i].Rotate_Center_Y;
                                    //R_T[i] = Rotate(S, C, _T);
                                    //R_T_X += ((int)Align_X[j]) * Model[j].resolution_X;
                                    //R_T_Y += ((int)Align_Y[j]) * Model[j].resolution_Y;
                                    //R_T_X += (R_T[i].X - Model[i].Master_X) * Model[i].resolution_X;
                                    //R_T_Y += (R_T[i].Y - Model[i].Master_Y) * Model[i].resolution_Y;
                                    #endregion

                                    R_T_X += ((int)Align_X[j]) * Model[j].resolution_X;
                                    R_T_Y += ((int)Align_Y[j]) * Model[j].resolution_Y;
                                    
                                }
                            }
                            if (T_Point_Use)
                                count = 3;

                            T_X = -(R_T_X / count) / 1000;//X 평균
                            T_Y = (R_T_Y / count) / 1000; //Y 평균
                        }
                        else
                        {
                            Point[] R_O = new Point[count];

                            for (int j = 0; j < count; j++)
                            {
                                if (!Align_X[i].Equals(0))
                                {
                                    S.X = (int)Align_X[j];
                                    S.Y = (int)Align_Y[j];
                                    C.X = (int)Model[j].Rotate_Center_X;//Model (Camera 별 회전 중심)
                                    C.Y = (int)Model[j].Rotate_Center_Y;
                                    R_O[j] = Rotate(S, C, _T);
                                    R_O_X += (R_O[j].X - Model[j].Master_X) * Model[j].resolution_X;
                                    R_O_Y += (R_O[j].Y - Model[j].Master_Y) * Model[j].resolution_Y;
                                }
                            }
                            if (T_Point_Use)
                                count = 3;

                            O_X = -(R_O_X / count) / 1000;//X 평균
                            O_X = (R_O_Y / count) / 1000; //Y 평균
                        }
                    }

                    #region
                    //if (gType == ClassEnum.Type.Target)
                    //{
                    //    Point[] R_T = new Point[count];

                    //    for (int i = 0; i < count; i++)
                    //    {
                    //        if (!Align_X[i].Equals(0))
                    //        {
                    //            //S.X = (int)Align_X[i];
                    //            //S.Y = (int)Align_Y[i];
                    //            //C.X = (int)Model[i].Rotate_Center_X;//Model (Camera 별 회전 중심)
                    //            //C.Y = (int)Model[i].Rotate_Center_Y;
                    //            //R_T[i] = Rotate(S, C, _T);
                    //            R_T_X += ((int)Align_X[i]) * Model[i].resolution_X;
                    //            R_T_Y += ((int)Align_Y[i]) * Model[i].resolution_Y;
                    //            //R_T_X += (R_T[i].X - Model[i].Master_X) * Model[i].resolution_X;
                    //            //R_T_Y += (R_T[i].Y - Model[i].Master_Y) * Model[i].resolution_Y;
                    //        }
                    //    }
                    //    if (T_Point_Use)
                    //        count = 3;

                    //    T_X = -(R_T_X / count) / 1000;//X 평균
                    //    T_Y = (R_T_Y / count) / 1000; //Y 평균
                    //}
                    //else if(gType == ClassEnum.Type.Object)
                    //{
                    //    Point[] R_O = new Point[count];

                    //    for (int i = 0; i < count; i++)
                    //    {
                    //        if (!Align_X[i].Equals(0))
                    //        {
                    //            S.X = (int)Align_X[i];
                    //            S.Y = (int)Align_Y[i];
                    //            C.X = (int)Model[i].Rotate_Center_X;//Model (Camera 별 회전 중심)
                    //            C.Y = (int)Model[i].Rotate_Center_Y;
                    //            R_O[i] = Rotate(S, C, _T);
                    //            R_O_X += (R_O[i].X - Model[i].Master_X) * Model[i].resolution_X;
                    //            R_O_Y += (R_O[i].Y - Model[i].Master_Y) * Model[i].resolution_Y;
                    //        }
                    //    }
                    //    if (T_Point_Use)
                    //        count = 3;

                    //    O_X = -(R_O_X / count) / 1000;//X 평균
                    //    O_X = (R_O_Y / count) / 1000; //Y 평균
                    //}
                    #endregion

                    if (T_Point_Use)
                        count = 3;

                    _X = -((T_X - O_X) / count) / 1000;
                    _Y = (T_Y - O_Y) / 1000;
                   
                    Align_Result = ClassEnum.Align_Result.OK;
                }

                else
                {
                    Point S = new Point(0, 0);
                    Point C = new Point(0, 0);
                    Point[] R = new Point[count];
                    double R_X = 0, R_Y = 0;
                    for (int i = 0; i < count; i++)
                    {
                        if (!Align_X[i].Equals(0))
                        {
                            S.X = (int)Align_X[i];
                            S.Y = (int)Align_Y[i];
                            C.X = (int)Model[i].Rotate_Center_X;//Model (Camera 별 회전 중심)
                            C.Y = (int)Model[i].Rotate_Center_Y;
                            R[i] = Rotate(S, C, _T);
                            R_X += (R[i].X - Model[i].Master_X) * Model[i].resolution_X;
                            R_Y += (R[i].Y - Model[i].Master_Y) * Model[i].resolution_Y;
                        }
                    }

                    if (T_Point_Use)
                        count = 3;
                    _X = -(R_X / count) / 1000;//X 평균
                    _Y = (R_Y / count) / 1000; //Y 평균
                    Align_Result = ClassEnum.Align_Result.OK;
                }
                
                // Test용
                //Sequence.ClassLog.Test_Write(cur_Unit, (int)gType, DateTime.Now, R);
                //

            }
            else
            {
                Sequence.Align_Sequence = ClassEnum.Align_Sequence.Manual_Mark;
                Align_Result = ClassEnum.Align_Result.NG;
                return false;
            }
            return true;
        }
        
        #region Align_Start 기존
        /**
        public bool Align_Start(ref double _X, ref double _Y, ref double _T)
        {
            bool T_Point_Use = false;
            double Cam_Teaching = (((Frame.PLC.Receive_Data[(cur_Unit) * 10 + 28]) | (Frame.PLC.Receive_Data[(cur_Unit) * 10 + 29] << 16))) / 10;
            int Shuttle_Y1 = (((Frame.PLC.Receive_Data[(cur_Unit) * 10 + 124]) | (Frame.PLC.Receive_Data[(cur_Unit) * 10 + 125] << 16))) / 10; ;
            int Shuttle_Y2 = (((Frame.PLC.Receive_Data[(cur_Unit) * 10 + 126]) | (Frame.PLC.Receive_Data[(cur_Unit) * 10 + 127] << 16))) / 10; ;
            int count = 0;
            for (int i = 0; i < 4; i++)
                L_Check[i] = 0;
            if (Second_Grab_Use)
                count = 4;
            else
                count = 2;


            int count_X = 0;
            int X_Number = 0;
            for (int i = 0; i < count; i++)
                if (Align_X[i].Equals(0))
                {
                    count_X++;
                    X_Number = i;
                }
            if ((count - count_X).Equals(count) || (Frame.Setup_Display.T_Point_Align_CheckBox.Checked && count_X.Equals(1) && count.Equals(4)))
            {
                double _sum = 0;
                if (Second_Grab_Use)
                {
                    if (!Align_X[0].Equals(0) && !Align_X[1].Equals(0) && !Align_X[2].Equals(0) && !Align_X[3].Equals(0))
                    {
                        L_Check[0] = Align.Align_Width(this, 0, 1, Cam_Teaching);
                        L_Check[1] = Align.Align_Width(this, 2, 3, Cam_Teaching);
                        L_Check[2] = Align.Align_Height(this, 0, 2, Shuttle_Y1, Shuttle_Y2);
                        L_Check[3] = Align.Align_Height(this, 1, 3, Shuttle_Y1, Shuttle_Y2);
                        Align_T[0] = Align.Align_Theta_W(Model[0].Master_Y - Align_Y[0], Model[1].Master_Y - Align_Y[1], Master_Width[0]);
                        Align_T[1] = Align.Align_Theta_W(Model[2].Master_Y - Align_Y[2], Model[3].Master_Y - Align_Y[3], Master_Width[1]);
                        Align_T[2] = -(Align.Align_Theta_W(Model[0].Master_X - Align_X[0], Model[2].Master_X - Align_X[2], Master_Height[0]));
                        Align_T[3] = -(Align.Align_Theta_W(Model[1].Master_X - Align_X[1], Model[3].Master_X - Align_X[3], Master_Height[1]));

                        _sum = 0;
                        for (int i = 0; i < 4; i++)
                            _sum += Align_T[i];
                        _T = _sum / 4;
                    }
                    else if (Frame.Setup_Display.T_Point_Align_CheckBox.Checked)
                    {
                        T_Point_Use = true;
                        switch (X_Number)
                        {
                            case 0:
                                L_Check[1] = Align.Align_Width(this, 2, 3, Cam_Teaching);
                                L_Check[3] = Align.Align_Height(this, 1, 3, Shuttle_Y1, Shuttle_Y2);
                                Align_T[0] = 0;
                                Align_T[1] = Align.Align_Theta_W((Model[2].Master_Y - Align_Y[2]) * Model[2].resolution_Y, (Model[3].Master_Y - Align_Y[3]) * Model[3].resolution_Y, L_Check[1]);
                                Align_T[2] = 0;
                                Align_T[3] = -(Align.Align_Theta_H((Model[1].Master_X - Align_X[1]) * Model[1].resolution_X, (Model[3].Master_X - Align_X[3]) * Model[3].resolution_X, L_Check[3]));
                                break;
                            case 1:
                                L_Check[1] = Align.Align_Width(this, 2, 3, Cam_Teaching);
                                L_Check[2] = Align.Align_Height(this, 0, 2, Shuttle_Y1, Shuttle_Y2);
                                Align_T[0] = 0;
                                Align_T[1] = Align.Align_Theta_W((Model[2].Master_Y - Align_Y[2]) * Model[2].resolution_Y, (Model[3].Master_Y - Align_Y[3]) * Model[3].resolution_Y, L_Check[1]);
                                Align_T[2] = -(Align.Align_Theta_H((Model[0].Master_X - Align_X[0]) * Model[0].resolution_X, (Model[2].Master_X - Align_X[2]) * Model[2].resolution_X, L_Check[2]));
                                Align_T[3] = 0;
                                break;
                            case 2:
                                L_Check[0] = Align.Align_Width(this, 0, 1, Cam_Teaching);
                                L_Check[3] = Align.Align_Height(this, 1, 3, Shuttle_Y1, Shuttle_Y2);
                                Align_T[0] = Align.Align_Theta_W((Model[0].Master_Y - Align_Y[0]) * Model[0].resolution_Y, (Model[1].Master_Y - Align_Y[1]) * Model[1].resolution_Y, L_Check[0]);
                                Align_T[1] = 0;
                                Align_T[2] = 0;
                                Align_T[3] = -(Align.Align_Theta_H((Model[1].Master_X - Align_X[1]) * Model[1].resolution_X, (Model[3].Master_X - Align_X[3]) * Model[3].resolution_X, L_Check[3]));
                                break;
                            case 3:
                                L_Check[0] = Align.Align_Width(this, 0, 1, Cam_Teaching);
                                L_Check[2] = Align.Align_Height(this, 0, 2, Shuttle_Y1, Shuttle_Y2);
                                Align_T[0] = Align.Align_Theta_W((Model[0].Master_Y - Align_Y[0]) * Model[0].resolution_Y, (Model[1].Master_Y - Align_Y[1]) * Model[1].resolution_Y, L_Check[0]);
                                Align_T[1] = 0;
                                Align_T[2] = -(Align.Align_Theta_H((Model[0].Master_X - Align_X[0]) * Model[0].resolution_X, (Model[2].Master_X - Align_X[2]) * Model[2].resolution_X, L_Check[2]));
                                Align_T[3] = 0;
                                break;
                        }
                        _sum = 0;
                        for (int i = 0; i < 4; i++)
                            _sum += Align_T[i];
                        _T = _sum / 2;
                    }
                }
                else
                {
                    L_Check[0] = Align.Align_Width(this, 0, 1, Cam_Teaching);
                    Align_T[0] = Align.Align_Theta_W((Model[0].Master_Y - Align_Y[0]) * Model[0].resolution_Y, (Model[1].Master_Y - Align_Y[1]) * Model[1].resolution_Y, L_Check[0]);
                    _T = Align_T[0];
                }

                Point S = new Point(0, 0);
                Point C = new Point(0, 0);
                Point[] R = new Point[count];
                double R_X = 0, R_Y = 0;
                for (int i = 0; i < count; i++)
                {
                    if (!Align_X[i].Equals(0))
                    {
                        S.X = (int)Align_X[i];
                        S.Y = (int)Align_Y[i];
                        C.X = (int)Model[i].Rotate_Center_X;
                        C.Y = (int)Model[i].Rotate_Center_Y;
                        R[i] = Rotate(S, C, _T);
                        R_X += (R[i].X - Model[i].Master_X) * Model[i].resolution_X;
                        R_Y += (R[i].Y - Model[i].Master_Y) * Model[i].resolution_Y;
                    }
                }
                // Test용
                Sequence.ClassLog.Test_Write(1, 0, DateTime.Now, R);
                //
                if (T_Point_Use)
                    count = 3;
                _X = -(R_X / count) / 1000;
                _Y = (R_Y / count) / 1000;
                Align_Result = ClassEnum.Align_Result.OK;
            }
            else
            {
                Sequence.Align_Sequence = ClassEnum.Align_Sequence.Manual_Mark;
                Align_Result = ClassEnum.Align_Result.NG;
                return false;
            }
            return true;
        }
    **/
        #endregion

        //20.12.21 Inspection
        public double[] Insp_R_T_X;
        public double[] Insp_R_T_Y;
        public double[] Insp_R_O_X;
        public double[] Insp_R_O_Y;

        public bool Inspection_Start()
        {

            if (Frame.Unit[cur_Unit].Type[(int)ClassEnum.Type.Target].Second_Grab_Use)
                count = 4;
            else
                count = 2;

            double _sum = 0;

            double _T = 0;

            if (gType == ClassEnum.Type.Target)
            {
                //Point S = new Point(0, 0);
                Point S_T = new Point(0, 0);
                Point C = new Point(0, 0);
                Point[] R_T = new Point[count];    
                for (int i = 0; i < count; i++)
                {
                    T_Align_T[0] = Align.Align_Theta_W(Model[0].Master_Y - Align_Y[0], Model[1].Master_Y - Align_Y[1], Master_Width[0]);
                    T_Align_T[1] = Align.Align_Theta_W(Model[2].Master_Y - Align_Y[2], Model[3].Master_Y - Align_Y[3], Master_Width[1]);
                    T_Align_T[2] = -(Align.Align_Theta_W(Model[0].Master_X - Align_X[0], Model[2].Master_X - Align_X[2], Master_Height[0]));
                    T_Align_T[3] = -(Align.Align_Theta_W(Model[1].Master_X - Align_X[1], Model[3].Master_X - Align_X[3], Master_Height[1]));

                    if (!Align_X[i].Equals(0))
                    {
                        //S.X = (int)Align_X[i];
                        //S.Y = (int)Align_Y[i];
                        //C.X = (int)Model[i].Rotate_Center_X;//Model (Camera 별 회전 중심)
                        //C.Y = (int)Model[i].Rotate_Center_Y;
                        //R_T[i] = Rotate(S, C, Align_T);
                        //Insp_R_T_X[i] = R_T[i].X;
                        //Insp_R_T_Y[i] = R_T[i].Y;

                        Insp_R_T_X[i] = (int)Align_X[i];
                        Insp_R_T_Y[i] = (int)Align_Y[i];
                    }
                }
                _sum = 0;
                for (int i = 0; i < 4; i++)
                    _sum += T_Align_T[i];
                _T = _sum / 4;
            }
            if(gType == ClassEnum.Type.Object)
            {
                Point S = new Point(0, 0);
                Point C = new Point(0, 0);
                Point[] R_O = new Point[count];                
                for (int i = 0; i < count; i++)
                {
                    //Align_T = -(Align.Align_Theta_H(Align_X[0] * Model[0].resolution_X, (Align_X[2]) * Model[2].resolution_X, L_Check[2]));

                    if (!Align_X[i].Equals(0))
                    {
                        S.X = (int)Align_X[i];
                        S.Y = (int)Align_Y[i];
                        C.X = Convert.ToInt32(Insp_R_T_X);//Insp_R_T_X[2];//Model (Camera 별 회전 중심)
                        C.Y = Convert.ToInt32(Insp_R_T_Y);//Insp_R_T_Y[2];
                        R_O[i] = Rotate(S, C, _T);
                        Insp_R_O_X[i] = R_O[i].X;
                        Insp_R_O_Y[i] = R_O[i].Y;
                    }
                }
            }

            double[] Inspection_X = new double[count];
            double[] Inspection_Y = new double[count];

            for(int i = 0; i<count; i++)
            {
                Inspection_X[i] = (Math.Abs(Insp_R_T_X[i]) - Math.Abs(Insp_R_O_X[i])) * Model[i].resolution_X;
                Inspection_Y[i] = (Math.Abs(Insp_R_T_Y[i]) - Math.Abs(Insp_R_O_Y[i])) * Model[i].resolution_Y;
            }

            return true;
        }

        //21.02.09 Cal_Height 수정
        public void Calibration_L_Check()
        {
            double Cam_Teaching = (((Frame.PLC.Receive_Data[(cur_Unit) * 10 + 40]) | (Frame.PLC.Receive_Data[(cur_Unit) * 10 + 41] << 16)));
            int Shuttle_Y1 = (((Frame.PLC.Receive_Data[(cur_Unit) * 10 + 124]) | (Frame.PLC.Receive_Data[(cur_Unit) * 10 + 125] << 16))) / 10; ;
            int Shuttle_Y2 = (((Frame.PLC.Receive_Data[(cur_Unit) * 10 + 126]) | (Frame.PLC.Receive_Data[(cur_Unit) * 10 + 127] << 16))) / 10; ;
            Master_L_Check[0] = Align.Cal_Width(this, 0, 1, Cam_Teaching);
            Master_L_Check[1] = Align.Cal_Width(this, 2, 3, Cam_Teaching);

            //Y축 1축 일 때
            Master_L_Check[2] = Align.Cal_Height(this, 0, 2, Shuttle_Y1);
            Master_L_Check[3] = Align.Cal_Height(this, 1, 3, Shuttle_Y1);

            //Y가 2축 일 때
            //Master_L_Check[2] = Align.Cal_Height_2(this, 0, 2, Shuttle_Y1, Shuttle_Y2);
            //Master_L_Check[3] = Align.Cal_Height_2(this, 1, 3, Shuttle_Y1, Shuttle_Y2);
        }

        private Point Rotate(Point sourcePoint, Point centerPoint, double rotateAngle)
        {
            Point targetPoint = new Point();

            double radian = rotateAngle / 180 * Math.PI;

            targetPoint.X = (int)(Math.Cos(radian) * (sourcePoint.X - centerPoint.X) - Math.Sin(radian) * (sourcePoint.Y - centerPoint.Y) + centerPoint.X);
            targetPoint.Y = (int)(Math.Sin(radian) * (sourcePoint.X - centerPoint.X) + Math.Cos(radian) * (sourcePoint.Y - centerPoint.Y) + centerPoint.Y);

            return targetPoint;
        }
    }
}
