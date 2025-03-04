using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cognex.VisionPro;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.CNLSearch;
using System.Threading;


using System.IO;
using System.Drawing.Imaging;

namespace T_Align
{
    public partial class Model : UserControl
    {
        Enum.Mark Current_Mark = Enum.Mark.MARK1;
        Enum.Mark_Type Current_Mark_Type = Enum.Mark_Type.MARK;
        Enum.Type Current_Type = Enum.Type.Target;
        Enum.Unit Current_Unit = Enum.Unit.Unit1;
        Button[] Unit_Button;
        NumericUpDown[] Line_Param;
        NumericUpDown[] Circle_Param;

        bool E_R_Use = false;
        int Current_Multi_Mark_Index = 0;

        bool Copy_Use = false;
        CogPMAlignTool Copy_PMAlign_Tool = new CogPMAlignTool();
        CogFindLineTool Copy_Line_Tool = new CogFindLineTool();
        CogFindLineTool Copy_Line_SubTool = new CogFindLineTool();
        CogFindCircleTool Copy_Circle_Tool = new CogFindCircleTool();

        CogPMAlignTool Current_PMAlign_Tool = new CogPMAlignTool();
        CogFindLineTool Current_Line_Tool = new CogFindLineTool();
        CogFindLineTool Current_Line_SubTool = new CogFindLineTool();
        CogFindCircleTool Current_Circle_Tool = new CogFindCircleTool();
        CogCNLSearchTool Current_CNL_Tool = new CogCNLSearchTool();

        CogCoordinateAxes Mark_Origin;
        CogRectangleAffine Mark_Rectangle;
        CogRectangleAffine ROI_Rectangle;

        CogCoordinateAxes Pattern_Origin;
        CogRectangleAffine Pattern_Rectangle;
        CogRectangleAffine Pattern_ROI_Rectangle;

        Cog_Panel[] Multimark_Panel;

        Thread Live_Thread;
        bool Live_On = false;
        bool ROI_Use = false;

        double Pattern_Origin_Angle = 0;
        string Current_Tool_Type = "";

        double Mouse_X = 0;
        double Mouse_Y = 0;
        bool Display_Click = false;
        bool Mesure_Use = false;
        bool IsStatic_Graphics = false;

        double Dis_Mesure_Start_X = 0;
        double Dis_Mesure_Start_Y = 0;
        double Dis_Mesure_End_X = 0;
        double Dis_Mesure_End_Y = 0;

        int Mask_Brush = 10;
        bool Brush = false;
        SolidBrush brush;
        Bitmap Mask_Bitmap;
        float pre_X, pre_Y;
        float cul_X, cul_Y;
        CogMaskGraphic MaskGraphic = new CogMaskGraphic();
        CogRectangle Mask_Rectangle = new CogRectangle();
        CogEllipse Mask_Circle = new CogEllipse();
        Check_Button[] Brush_Button;

        double X = 0;
        double Y = 0;

        double S = 0;
        double A = 0;
        double R = 0;
        double T = 0;

        public void Loading_OK()
        {
            Live_Thread.Start();
            Label_Current_Recipe.Text = COMMON.RECIPE;
            Mark_Select();
            Display_Mark_Type(Current_Mark_Type);
            Unit_Select("1");
            Type_Select("T");            
            Live_Thread.Suspend();
            for (int LoopCount = 1; LoopCount < 9; LoopCount++)
            {
                if (COMMON.Frame.Unit[LoopCount] == null)
                    Unit_Button[LoopCount - 1].Visible = false;
            }
        }

        public Model()
        {
            InitializeComponent();
            Unit_Button = new Button[] { Button_Unit1, Button_Unit2, Button_Unit3, Button_Unit4, Button_Unit5, Button_Unit6, Button_Unit7, Button_Unit8 };
            Brush_Button = new Check_Button[] { Button_Brush_1, Button_Brush_5, Button_Brush_10, Button_Brush_20, Button_Brush_30, Button_Brush_50 };
            Line_Param = new NumericUpDown[] { Line_Updown1, Line_Updown2, Line_Updown3, Line_Updown4, Line_Updown5, Line_Updown6 };
            Circle_Param = new NumericUpDown[] { Circle_UpDown1, Circle_UpDown2, Circle_UpDown3, Circle_UpDown4, Circle_UpDown5, Circle_UpDown6, Circle_UpDown7 };
            Multimark_Panel = new Cog_Panel[] { cog_Panel2, cog_Panel3, cog_Panel4, cog_Panel5, cog_Panel6 };
            for (int LoopCount = 0; LoopCount < 8; LoopCount++)
                Unit_Button[LoopCount].Text = COMMON.U_Setting[LoopCount + 1].Unit_Name;
            for (int LoopCount = 0; LoopCount < 5; LoopCount++)
            {
                Multimark_Panel[LoopCount].Re_Size();
            }
            Panel_Pattern_Image.Re_Size();
            Display_Mark_Type(Current_Mark_Type);
            Live_Thread = new Thread(Live);
        }

        public void Thread_Stop()
        {
            Live_Thread.Join();
        }

        private void Live()
        {
            while (true)
            {
                if (!COMMON.Program_Run)
                    break;
                if (Live_On)
                {
                    Model_Display.Image = COMMON.Grab_Image[COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Cam[(int)Current_Mark % 2]];
                }
                Thread.Sleep(100);
            }
        }

        public void Live_Stop()
        {
            if (Live_Thread.ThreadState != ThreadState.Suspended)
            {
                try
                {
                    Button_Live_Camera.Image = global::T_Align.Properties.Resources.Live;
                    Live_On = false;
                    Live_Thread.Suspend();
                }
                catch { }
            }
        }

        private void Model_Load(object sender, EventArgs e)
        {
            //Model_Display.Image = new Cognex.VisionPro.CogImage8Grey((Bitmap)Bitmap.FromFile("C:\\Users\\Lin\\Desktop\\Image\\210830.bmp"));
        }

        private void Label_Current_Recipe_DoubleClick(object sender, EventArgs e)
        {
            COMMON.Recipe_Select.ShowDialog();
            Label_Current_Recipe.Text = COMMON.RECIPE;
        }

        private void Button_Mark_Index_Click(object sender, EventArgs e)
        {
            Mark_Index mark_Index = new Mark_Index(false, COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Position);
            mark_Index.ShowDialog();
            Current_Mark = mark_Index.Mark;
            Button_Mark_Index.Text = Current_Mark.ToString();
            mark_Index.Dispose();
            Mark_Select();
        }

        private void Button_Mark_Type_Click(object sender, EventArgs e)
        {
            Type_Index type_Index = new Type_Index();
            type_Index.ShowDialog();
            Current_Mark_Type = type_Index.E_Type;
            Button_Mark_Type.Text = type_Index.S_Type;
            type_Index.Dispose();
            Display_Mark_Type(Current_Mark_Type);
            COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Mark_Type_Save(Current_Mark_Type);
            COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Type_Change_Load();
            Mark_Type_Cancel();
        }

        private void Display_Mark_Type(Enum.Mark_Type _Type)
        {
            Button_Mark.Visible = false;
            Button_Line_H.Visible = false;
            Button_Line_W.Visible = false;
            Button_Circle.Visible = false;
            Button_Pattern_Mode.Visible = false;
            switch (_Type)
            {
                case Enum.Mark_Type.MARK:
                    Button_Mark.Visible = true;
                    Button_Find_Mark.Text = "Find\nMark";
                    Button_Find_Mark_To.Visible = false;
                    break;
                case Enum.Mark_Type.LINE:
                    Button_Line_H.Visible = true;
                    Button_Line_W.Visible = true;
                    Button_Find_Mark.Text = "Find\nLine";
                    Button_Find_Mark_To.Visible = false;
                    break;
                case Enum.Mark_Type.MARK_TO_LINE:
                    Button_Mark.Visible = true;
                    Button_Line_H.Visible = true;
                    Button_Line_W.Visible = true;
                    Button_Find_Mark.Text = "Find\nMark";
                    Button_Find_Mark_To.Visible = true;
                    Button_Find_Mark_To.Text = "Find\nMark\nLine";
                    break;
                case Enum.Mark_Type.CIRCLE:
                    Button_Circle.Visible = true;
                    Button_Find_Mark.Text = "Find\nCircle";
                    Button_Find_Mark_To.Visible = false;
                    break;
                case Enum.Mark_Type.MARK_TO_CIRCLE:
                    Button_Mark.Visible = true;
                    Button_Circle.Visible = true;
                    Button_Find_Mark.Text = "Find\nMark";
                    Button_Find_Mark_To.Visible = true;
                    Button_Find_Mark_To.Text = "Find\nMark\nCircle";
                    break;
                case Enum.Mark_Type.PATTERN:
                    Button_Pattern_Mode.Visible = true;
                    Button_Find_Mark.Text = "Find\nPattern";
                    Button_Find_Mark_To.Visible = false;
                    break;
            }
        }

        private void Unit_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Unit_Select(button.Name.Substring(button.Name.Length - 1, 1));
            
            Mark_Select();
        }

        private void Unit_Select(string _Unit)
        {
            Current_Unit = (Enum.Unit)Convert.ToInt32(_Unit);
            for (int LoopCount = 0; LoopCount < 8; LoopCount++)
            {
                Unit_Button[LoopCount].Image = global::T_Align.Properties.Resources.Unit;
                Unit_Button[LoopCount].ForeColor = Color.White;
            }
            Unit_Button[(int)Current_Unit - 1].Image = global::T_Align.Properties.Resources.Unit_Select;
            Unit_Button[(int)Current_Unit - 1].ForeColor = Color.Cyan;

            Button_Target.Text = COMMON.U_Setting[(int)Current_Unit].T_Setting[0].Type_Name;
            Button_Object.Text = COMMON.U_Setting[(int)Current_Unit].T_Setting[2].Type_Name;
            if (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Object].Position[0] == false &&
                COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Object].Position[1] == false &&
                COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Object].Position[2] == false &&
                COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Object].Position[3] == false)
            {
                Current_Type = Enum.Type.Target;
                Button_Object.Visible = false;
                Button_Target.Image = global::T_Align.Properties.Resources.Button1_Select;
                Button_Object.Image = global::T_Align.Properties.Resources.Button1;
                Button_Target.ForeColor = Color.Cyan;
                Button_Object.ForeColor = Color.White;
            }
            else
            {
                Button_Object.Visible = true;
            }
        }

        private void Type_Select(string _Type)
        {
            switch (_Type)
            {
                case "T":
                    Current_Type = Enum.Type.Target;
                    break;
                case "O":
                    Current_Type = Enum.Type.Object;
                    break;
            }
            if (Current_Type == Enum.Type.Target)
            {
                Button_Target.Image = global::T_Align.Properties.Resources.Button1_Select;
                Button_Object.Image = global::T_Align.Properties.Resources.Button1;
                Button_Target.ForeColor = Color.Cyan;
                Button_Object.ForeColor = Color.White;
            }
            else
            {
                Button_Target.Image = global::T_Align.Properties.Resources.Button1;
                Button_Object.Image = global::T_Align.Properties.Resources.Button1_Select;
                Button_Target.ForeColor = Color.White;
                Button_Object.ForeColor = Color.Cyan;
            }
            Mark_Select();
        }

        private void Type_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Type_Select(button.Name.Substring(7, 1));
        }

        private void Button_Grab_Home_Click(object sender, EventArgs e)
        {
            Model_Display.Image = COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Last_Grab_Image;
        }

        private void Button_Grab_Open_Click(object sender, EventArgs e)
        {
            string FIlePath = string.Empty;

            using (OpenFileDialog Dialog = new OpenFileDialog())
            {
                Dialog.InitialDirectory = @"D:\\AlignLog";
                Dialog.Filter = "이미지파일(*.jpg,*.bmp,*.png)|*.jpeg;*.jpg;*.bmp;*.png|JPG이미지파일(*.jpg)|*.jpg;*.jpeg|BMP이미지파일(*.bmp)|*.bmp|PNG이미지파일(*.png)|*.png";
                DialogResult result = Dialog.ShowDialog();
                if (result.Equals(DialogResult.OK))
                {
                    FIlePath = Dialog.FileName;
                    Model_Display.Image = new CogImage8Grey((Bitmap)Bitmap.FromFile(FIlePath));
                }
            }
        }

        private void Button_Grab_Camera_Click(object sender, EventArgs e)
        {
            Model_Display.Image = COMMON.Grab_Image[COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Cam[(int)Current_Mark % 2]];
        }

        static private void Image_Save_JPG(Image _image, string Path, long _Quality)
        {
            long jpegQuality = _Quality;
            Image image = _image;

            EncoderParameters ep = new EncoderParameters(1);
            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, jpegQuality);
            ImageCodecInfo ici = GetEncoderInfo("image/jpeg");
            image.Save(Path, ici, ep);
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < encoders.Length; ++i)
            {
                if (encoders[i].MimeType == mimeType)
                    return encoders[i];
            }
            return null;
        }

        private void Button_Live_Camera_Click(object sender, EventArgs e)
        {
            Live_On = !Live_On;
            if (Live_On)
            {
                Button_Live_Camera.Image = global::T_Align.Properties.Resources.Live_Select;
                if (Live_Thread.ThreadState == ThreadState.Suspended)
                    Live_Thread.Resume();
            }
            else
            {
                Button_Live_Camera.Image = global::T_Align.Properties.Resources.Live;
                if (Live_Thread.ThreadState != ThreadState.Suspended)
                    Live_Thread.Suspend();
            }
        }

        private void Button_Clear_Click(object sender, EventArgs e)
        {
            Model_Display.InteractiveGraphics.Clear();
            Model_Display.StaticGraphics.Clear();
        }

        private void Button_Mark_Click(object sender, EventArgs e)
        {
            Mark_Type_Select("M");
            Mark_Select();
        }

        private void Mark_Select()
        {
            E_R_Use = false;
            Graphic_Select(E_R_Use);
            for(int LoopCount=0;LoopCount<5;LoopCount++)
            {
                try
                {
                    Multimark_Panel[LoopCount].Pattern_Display.Image = COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Multi_Mark[LoopCount].Pattern.GetTrainedPatternImage();
                }
                catch { Multimark_Panel[LoopCount].Pattern_Display.Image = null; }
            }
            try
            {
                Current_Mark_Type = COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Mark_Type_Load();
                Button_Mark_Type.Text = Current_Mark_Type.ToString().Replace('_', ' ');
            }
            catch { }
            if (Model_Display.Image != null && Button_Mark.ForeColor == Color.Cyan)
            {
                try
                {
                    Mesure_Use = false;
                    Button_Masure.Image = global::T_Align.Properties.Resources.M_Masure;
                    Button_Masure.ForeColor = Color.White;
                    Model_Display.StaticGraphics.Clear();
                    Model_Display.InteractiveGraphics.Clear();
                    if (COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Multi_Mark[Current_Multi_Mark_Index] == null)
                    {
                        Current_PMAlign_Tool = new CogPMAlignTool();
                        Current_PMAlign_Tool.RunParams.RunAlgorithm = CogPMAlignRunAlgorithmConstants.PatMax;
                        Current_PMAlign_Tool.RunParams.ZoneAngle.Configuration = CogPMAlignZoneConstants.LowHigh;
                        Current_PMAlign_Tool.RunParams.ZoneAngle.High = Custom_Math.DegreeToRadian(1);
                        Current_PMAlign_Tool.RunParams.ZoneAngle.Low = Custom_Math.DegreeToRadian(-1);
                        Current_PMAlign_Tool.RunParams.ZoneScale.Configuration = CogPMAlignZoneConstants.LowHigh;
                        Current_PMAlign_Tool.RunParams.ZoneScale.High = 1.2;
                        Current_PMAlign_Tool.RunParams.ZoneScale.Low = 0.8;
                    }
                    else
                        Current_PMAlign_Tool = COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Multi_Mark[Current_Multi_Mark_Index];
                    PM_Tool_Load();
                }
                catch (Exception Ex) { string E = Ex.ToString(); }
            }
            Display_Mark_Type(Current_Mark_Type);
        }

        private void Line_Click(object sender, EventArgs e)
        {
            if (Model_Display.Image != null)
            {
                Model_Display.InteractiveGraphics.Clear();
                Model_Display.StaticGraphics.Clear();
                Button button = (Button)sender;
                Mark_Type_Select(button.Text.Substring(button.Text.Length - 1, 1));
                switch (button.Text.Substring(button.Text.Length - 1, 1))
                {
                    case "W":
                        Current_Line_Tool = new CogFindLineTool(COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Line_W);
                        Current_Line_SubTool = new CogFindLineTool(COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Line_H);
                        break;
                    case "H":
                        Current_Line_Tool = new CogFindLineTool(COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Line_H);
                        Current_Line_SubTool = new CogFindLineTool(COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Line_W);
                        break;
                }
                if (Current_Mark_Type == Enum.Mark_Type.MARK_TO_LINE)
                {
                    Current_Line_Tool.RunParams.ExpectedLineSegment.StartX += X;
                    Current_Line_Tool.RunParams.ExpectedLineSegment.StartY += Y;
                    Current_Line_Tool.RunParams.ExpectedLineSegment.EndX += X;
                    Current_Line_Tool.RunParams.ExpectedLineSegment.EndY += Y;
                    Current_Line_SubTool.RunParams.ExpectedLineSegment.StartX += X;
                    Current_Line_SubTool.RunParams.ExpectedLineSegment.StartY += Y;
                    Current_Line_SubTool.RunParams.ExpectedLineSegment.EndX += X;
                    Current_Line_SubTool.RunParams.ExpectedLineSegment.EndY += Y;
                }
                Current_Line_Tool.InputImage = (CogImage8Grey)Model_Display.Image;
                Current_Line_SubTool.InputImage = (CogImage8Grey)Model_Display.Image;
                Line_Tool_Load();
            }
        }

        private void Button_Circle_Click(object sender, EventArgs e)
        {
            if (Model_Display.Image != null)
            {
                Model_Display.InteractiveGraphics.Clear();
                Model_Display.StaticGraphics.Clear();
                Mark_Type_Select("C");
                Current_Circle_Tool = new CogFindCircleTool(COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Circle);
                Circle_Tool_Load();
            }
        }

        private void Mark_Type_Select(string _Type)
        {
            if (Model_Display.Image != null)
            {
                Mark_Type_Cancel();
                Current_Tool_Type = _Type;
                switch (_Type)
                {
                    case "M":
                        Button_Mark.Image = global::T_Align.Properties.Resources.Type_Select1;
                        Button_Mark.ForeColor = Color.Cyan;
                        Panel_Mark.Visible = true;
                        Button_Train.Visible = true;
                        break;
                    case "C":
                        Button_Circle.Image = global::T_Align.Properties.Resources.Type_Select1;
                        Button_Circle.ForeColor = Color.Cyan;
                        Panel_CIrcle.Visible = true;
                        Button_Edit_G.Visible = true;
                        Button_Run_G.Visible = true;
                        break;
                    case "P":
                        Button_Pattern_Mode.Image = global::T_Align.Properties.Resources.Type_Select1;
                        Button_Pattern_Mode.ForeColor = Color.Cyan;
                        Panel_Pattern_Mode.Visible = true;
                        Button_Train.Visible = true;
                        break;
                    default:
                        Panel_Line.Visible = true;
                        if (_Type == "W")
                        {
                            Button_Line_W.Image = global::T_Align.Properties.Resources.Type_Select1;
                            Button_Line_W.ForeColor = Color.Cyan;
                        }
                        else if (_Type == "H")
                        {
                            Button_Line_H.Image = global::T_Align.Properties.Resources.Type_Select1;
                            Button_Line_H.ForeColor = Color.Cyan;
                        }
                        Button_Edit_G.Visible = true;
                        Button_Run_G.Visible = true;
                        break;
                }
                Button_Detail.Visible = true;
                Button_Type_Save.Visible = true;
                Graphic_Select(E_R_Use);
            }
        }

        private void Mark_Type_Cancel()
        {
            Model_Display.InteractiveGraphics.Clear();
            Model_Display.StaticGraphics.Clear();
            Current_Tool_Type = "";
            Button_Train.Visible = false;
            Button_Edit_G.Visible = false;
            Button_Run_G.Visible = false;
            Button_Detail.Visible = false;
            Button_Type_Save.Visible = false;
            Default_Type_Button(Button_Mark);
            Default_Type_Button(Button_Line_H);
            Default_Type_Button(Button_Line_W);
            Default_Type_Button(Button_Circle);
            Default_Type_Button(Button_Pattern_Mode);
            //Button_Pattern.Image = global::T_Align.Properties.Resources.Button6;
            //Button_Pattern.ForeColor = Color.White;
            //Button_ROI.Image = global::T_Align.Properties.Resources.Button6;
            //Button_ROI.ForeColor = Color.White;
            //Button_Masking.Image = global::T_Align.Properties.Resources.Button6;
            //Button_Masking.ForeColor = Color.White;
            Panel_Mark.Visible = false;
            Panel_Line.Visible = false;
            Panel_CIrcle.Visible = false;
            Panel_Pattern_Mode.Visible = false;
            E_R_Use = false;
        }


        private void Default_Type_Button(Button Control)
        {
            Control.Image = global::T_Align.Properties.Resources.Type;
            Control.ForeColor = Color.White;
        }

        private void Button_Type_Save_Click(object sender, EventArgs e)
        {
            switch (Current_Mark_Type)
            {
                case Enum.Mark_Type.PATTERN:
                    try
                    {
                        if (!Button_Pattern_ROI_Use.Check)
                            Pattern_ROI_Rectangle = null;
                        Current_CNL_Tool.SearchRegion = Pattern_ROI_Rectangle;
                        if (Button_Pattern_ROI_Display.Check)
                            Current_CNL_Tool.LastRunRecordDiagEnable = CogCNLSearchLastRunRecordDiagConstants.SearchRegion;
                        else
                            Current_CNL_Tool.LastRunRecordDiagEnable = CogCNLSearchLastRunRecordDiagConstants.None;
                        COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].CNL = new CogCNLSearchTool(Current_CNL_Tool);
                        COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Mark_Match_Rate[Current_Multi_Mark_Index] = (double)Mark_Mathing_Rate.Value;
                        Model_Display.InteractiveGraphics.Clear();
                        COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Pattern_Save();
                        COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Pattern_Limit_Save((double)Nmr_Ptn_Match_Rate.Value);
                    }
                    catch (Exception _e)
                    { MessageBox.Show("트레인 에러 : " + _e.ToString()); }
                    break;
                case Enum.Mark_Type.MARK:
                    _Mark:
                    try
                    {
                        if (!ROI_Use)
                            ROI_Rectangle = null;
                        Current_PMAlign_Tool.SearchRegion = ROI_Rectangle;
                        if (check_Button_ROI.Check)
                            Current_PMAlign_Tool.LastRunRecordDiagEnable = CogPMAlignLastRunRecordDiagConstants.SearchRegion;
                        else
                            Current_PMAlign_Tool.LastRunRecordDiagEnable = CogPMAlignLastRunRecordDiagConstants.None;
                        COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Multi_Mark[Current_Multi_Mark_Index] = new CogPMAlignTool(Current_PMAlign_Tool);
                        COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Mark_Match_Rate[Current_Multi_Mark_Index] = (double)Mark_Mathing_Rate.Value;
                        Model_Display.InteractiveGraphics.Clear();
                        Button_Brush_Use.Image = global::T_Align.Properties.Resources.Button1;
                        Button_Brush_Use.ForeColor = Color.White;
                        Brush = false;
                        COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Mark_Save(Current_Multi_Mark_Index);
                        COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Mark_Limit_Save(Current_Multi_Mark_Index, (double)Mark_Mathing_Rate.Value);
                    }
                    catch (Exception _e)
                    { MessageBox.Show("트레인 에러 : " + _e.ToString()); }
                    break;
                case Enum.Mark_Type.LINE:
                _Line:
                    try
                    {
                        if (Current_Mark_Type == Enum.Mark_Type.MARK_TO_LINE)
                        {
                            Current_Line_Tool.RunParams.ExpectedLineSegment.StartX -= X;
                            Current_Line_Tool.RunParams.ExpectedLineSegment.StartY -= Y;
                            Current_Line_Tool.RunParams.ExpectedLineSegment.EndX -= X;
                            Current_Line_Tool.RunParams.ExpectedLineSegment.EndY -= Y;
                        }
                        Current_Line_Tool.Run();
                        if (Button_Line_W.ForeColor == Color.Cyan)
                        {
                            COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Line_W = new CogFindLineTool(Current_Line_Tool);
                            COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Line_Save("W");
                            COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Line_Limit_Save((double)Line_Angle_Limit.Value);
                        }
                        if (Button_Line_H.ForeColor == Color.Cyan)
                        {
                            COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Line_H = new CogFindLineTool(Current_Line_Tool);
                            COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Line_Save("H");
                            COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Line_Limit_Save((double)Line_Angle_Limit.Value);
                        }
                        Model_Display.InteractiveGraphics.Clear();
                        Model_Display.StaticGraphics.Clear();
                    }
                    catch { MessageBox.Show("세이브 에러"); }
                    break;
                case Enum.Mark_Type.CIRCLE:
                _Circle:
                    try
                    {
                        Current_Circle_Tool.RunParams.RadiusConstraintEnabled = false;
                        Current_Circle_Tool.Run();
                        if (Current_Mark_Type == Enum.Mark_Type.MARK_TO_CIRCLE)
                        {
                            Current_Circle_Tool.RunParams.ExpectedCircularArc.CenterX -= X;
                            Current_Circle_Tool.RunParams.ExpectedCircularArc.CenterY -= Y;
                        }

                        COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Circle_R_Save((double)Current_Circle_Tool.Results.GetCircle().Radius);
                        COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Circle = new CogFindCircleTool(Current_Circle_Tool);
                        COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Circle_Save();
                        COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Circle_Limit_Save((double)Radius_Limit.Value);
                        Model_Display.InteractiveGraphics.Clear();
                        Model_Display.StaticGraphics.Clear();
                    }
                    catch(Exception Ex)  { MessageBox.Show("세이브 에러" + Ex.ToString()); }
                    break;
                case Enum.Mark_Type.MARK_TO_CIRCLE:
                    if (Button_Mark.ForeColor == Color.Cyan)
                        goto _Mark;
                    else
                        goto _Circle;
                case Enum.Mark_Type.MARK_TO_LINE:
                    if (Button_Mark.ForeColor == Color.Cyan)
                        goto _Mark;
                    else
                        goto _Line;
            }
            Mark_Type_Cancel();
            Mark_Select();
        }

        private void Graphic_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Text.Substring(0, 1))
            {
                case "E":
                    E_R_Use = false;
                    break;
                case "R":
                    E_R_Use = true;
                    break;
            }
            Graphic_Select(E_R_Use);
            if (Button_Circle.ForeColor == Color.Cyan)
            {
                if (!E_R_Use)
                    Circle_Drawing(Current_Circle_Tool);
                else
                    Run_Circle_Drawing(Current_Circle_Tool);
            }
            if (Button_Line_H.ForeColor == Color.Cyan || Button_Line_W.ForeColor == Color.Cyan)
            {
                if (!E_R_Use)
                    Line_Drawing(Current_Line_Tool);
                else
                    Run_Line_Drawing(Current_Line_Tool);
            }
        }

        private void Graphic_Select(bool _Type)
        {
            Button_Edit_G.Image = global::T_Align.Properties.Resources.Button5;
            Button_Run_G.Image = global::T_Align.Properties.Resources.Button5;
            Button_Edit_G.ForeColor = Color.White;
            Button_Run_G.ForeColor = Color.White;
            switch (_Type)
            {
                case false:
                    Button_Edit_G.Image = global::T_Align.Properties.Resources.Button5_Select;
                    Button_Edit_G.ForeColor = Color.Cyan;
                    E_R_Use = false;
                    break;
                case true:
                    Button_Run_G.Image = global::T_Align.Properties.Resources.Button5_Select;
                    Button_Run_G.ForeColor = Color.Cyan;
                    E_R_Use = true;
                    break;
            }
        }

        private void Mark_Click(object sender, EventArgs e)
        {
            Button_Pattern.Image = global::T_Align.Properties.Resources.Button6;
            Button_Pattern.ForeColor = Color.White;
            Button_ROI.Image = global::T_Align.Properties.Resources.Button6;
            Button_ROI.ForeColor = Color.White;
            Button_Masking.Image = global::T_Align.Properties.Resources.Button6;
            Button_Masking.ForeColor = Color.White;
            Panel_Pattern.Visible = false;
            Panel_ROI.Visible = false;
            Panel_Masking.Visible = false;
            Model_Display.InteractiveGraphics.Clear();
            Button_Brush_Use.Image = global::T_Align.Properties.Resources.Button1;
            Button_Brush_Use.ForeColor = Color.White;
            Brush = false;
            Button button = (Button)sender;
            Select_Mark_Graphic(button.Text.Substring(0, 1));
        }

        private void Select_Mark_Graphic(string _Command)
        {
            switch (_Command)
            {
                case "P":
                    Button_Pattern.Image = global::T_Align.Properties.Resources.Button6_Select;
                    Button_Pattern.ForeColor = Color.Cyan;
                    Panel_Pattern.Visible = true;
                    Model_Display.InteractiveGraphics.Add(Mark_Origin, "O", false);
                    Model_Display.InteractiveGraphics.Add(Mark_Rectangle, "R", false);
                    break;
                case "R":
                    Button_ROI.Image = global::T_Align.Properties.Resources.Button6_Select;
                    Button_ROI.ForeColor = Color.Cyan;
                    Panel_ROI.Visible = true;
                    ROI_Rectangle.Color = CogColorConstants.Orange;
                    ROI_Rectangle.Interactive = true;
                    ROI_Rectangle.GraphicDOFEnable = CogRectangleAffineDOFConstants.All;
                    if (ROI_Use)
                        Model_Display.InteractiveGraphics.Add(ROI_Rectangle, "R2", false);
                    break;
                case "M":
                    Button_Masking.Image = global::T_Align.Properties.Resources.Button6_Select;
                    Button_Masking.ForeColor = Color.Cyan;
                    Panel_Masking.Visible = true;
                    MaskGraphic.Image = new CogImage8Grey(Mask_Bitmap);
                    Model_Display.InteractiveGraphics.Add((ICogGraphicInteractive)MaskGraphic, "", false);
                    Mask_Rectangle.Interactive = true;
                    Mask_Rectangle.GraphicDOFEnable = CogRectangleDOFConstants.All;
                    try { Model_Display.InteractiveGraphics.Add(Mask_Rectangle as ICogGraphicInteractive, "R", true); }
                    catch { }
                    Mask_Circle.Interactive = true;
                    Mask_Circle.GraphicDOFEnable = (CogEllipseDOFConstants.Position | CogEllipseDOFConstants.Size | CogEllipseDOFConstants.Scale);
                    try { Model_Display.InteractiveGraphics.Add(Mask_Circle as ICogGraphicInteractive, "C", true); }
                    catch { }
                    break;
            }
        }

        private void Button_Return_Click(object sender, EventArgs e)
        {
            Pattern_Origin_Angle -= 90;
            if (Pattern_Origin_Angle == -90)
                Pattern_Origin_Angle = 270;
            Mark_Origin.Rotation = Custom_Math.DegreeToRadian(Pattern_Origin_Angle);
        }

        private void Button_turn_Click(object sender, EventArgs e)
        {
            Pattern_Origin_Angle += 90;
            if (Pattern_Origin_Angle == 360)
                Pattern_Origin_Angle = 0;
            Mark_Origin.Rotation = Custom_Math.DegreeToRadian(Pattern_Origin_Angle);
        }

        private void Origin_Moveing_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Name.Substring(7, button.Name.Length - 7))
            {
                case "U":
                    Mark_Origin.OriginY -= 1;
                    break;
                case "UU":
                    Mark_Origin.OriginY -= 10;
                    break;
                case "D":
                    Mark_Origin.OriginY += 1;
                    break;
                case "DD":
                    Mark_Origin.OriginY += 10;
                    break;
                case "R":
                    Mark_Origin.OriginX += 1;
                    break;
                case "RR":
                    Mark_Origin.OriginX += 10;
                    break;
                case "L":
                    Mark_Origin.OriginX -= 1;
                    break;
                case "LL":
                    Mark_Origin.OriginX -= 10;
                    break;
            }
        }

        private void Numeric_Scale_Min_ValueChanged(object sender, EventArgs e)
        {
            Current_PMAlign_Tool.RunParams.ZoneScale.Low = (double)Numeric_Scale_Min.Value;
        }

        private void Numeric_Scale_Max_ValueChanged(object sender, EventArgs e)
        {
            Current_PMAlign_Tool.RunParams.ZoneScale.High = (double)Numeric_Scale_Max.Value;
        }

        private void Numeric_Angle_Min_ValueChanged(object sender, EventArgs e)
        {
            Current_PMAlign_Tool.RunParams.ZoneAngle.Low = Custom_Math.DegreeToRadian((double)Numeric_Angle_Min.Value);
        }

        private void Numeric_Angle_Max_ValueChanged(object sender, EventArgs e)
        {
            Current_PMAlign_Tool.RunParams.ZoneAngle.High = Custom_Math.DegreeToRadian((double)Numeric_Angle_Max.Value);
        }

        private void comboBox_MultiMark_Index_SelectedIndexChanged(object sender, EventArgs e)
        {
            Current_Multi_Mark_Index = int.Parse(comboBox_MultiMark_Index.Text);
            Mark_Select();
        }

        private void Button_ROI_Use_Click(object sender, EventArgs e)
        {
            ROI_Use = !ROI_Use;
            if (ROI_Use)
            {
                Button_ROI_Use.Image = global::T_Align.Properties.Resources.Button5_Select;
                Button_ROI_Use.ForeColor = Color.Cyan;
                Button_ROI_Use.Text = "Un Use ROI";
            }
            else
            {
                Button_ROI_Use.Image = global::T_Align.Properties.Resources.Button5;
                Button_ROI_Use.ForeColor = Color.White;
                Button_ROI_Use.Text = "Use ROI";
            }
            Model_Display.InteractiveGraphics.Clear();
            Select_Mark_Graphic("R");
        }

        private string Type_Select()
        {
            if (Button_Pattern_Mode.ForeColor == Color.Cyan)
                return "Pattern";
            else if (Button_Mark.ForeColor == Color.Cyan)
                return "Mark";
            else if (Button_Circle.ForeColor == Color.Cyan)
                return "Circle";
            else
                return "Line";
        }

        private void Button_Detail_Click(object sender, EventArgs e)
        {
            Detail detail = null;
            switch (Type_Select())
            {
                case "Mark":
                    Current_PMAlign_Tool.InputImage = Model_Display.Image;
                    detail = new Detail(Current_PMAlign_Tool);
                    if (detail.ShowDialog() == DialogResult.OK)
                    {
                        Current_PMAlign_Tool = new CogPMAlignTool(detail.PM_Tool);
                        PM_Tool_Load();
                    }
                    break;
                case "Line":
                    Current_Line_Tool.InputImage = Model_Display.Image as CogImage8Grey;
                    detail = new Detail(Current_Line_Tool);
                    if (detail.ShowDialog() == DialogResult.OK)
                    {
                        Current_Line_Tool = new CogFindLineTool(detail.Line_Tool);
                        Line_Tool_Load();
                    }
                    break;
                case "Circle":
                    Current_Circle_Tool.InputImage = Model_Display.Image as CogImage8Grey;
                    detail = new Detail(Current_Circle_Tool);
                    if (detail.ShowDialog() == DialogResult.OK)
                    {
                        Current_Circle_Tool = new CogFindCircleTool(detail.Circle_Tool);
                        Circle_Tool_Load();
                    }
                    break;
                case "Pattern":
                    Current_CNL_Tool.InputImage = Model_Display.Image as CogImage8Grey;
                    detail = new Detail(Current_CNL_Tool);
                    if (detail.ShowDialog() == DialogResult.OK)
                    {
                        Current_CNL_Tool = new CogCNLSearchTool(detail.CNL_Tool);
                        Pattern_Tool_Load(); ;
                    }
                    break;
            }
            detail.Dispose();
        }

        private void Button_Center_Click(object sender, EventArgs e)
        {
            Mark_Origin.OriginX = Mark_Rectangle.CenterX;
            Mark_Origin.OriginY = Mark_Rectangle.CenterY;
        }

        private void Model_Display_MouseMove(object sender, MouseEventArgs e)
        {
            if (Model_Display.Image != null)
            {
                CogTransform2DLinear Pointer;
                Pointer = Model_Display.GetTransform("#", "*") as CogTransform2DLinear;
                Pointer.MapPoint(e.X, e.Y, out Mouse_X, out Mouse_Y);
                Pixel_Data_Read();

                if (Display_Click && Mesure_Use)
                {
                    Dis_Mesure_End_X = Mouse_X;
                    Dis_Mesure_End_Y = Mouse_Y;
                    Mesure_Drawing(CogColorConstants.Blue);
                    Mesure_Data_Display();
                }
                if (Display_Click && Brush)
                {
                    using (Graphics g = Graphics.FromImage(Mask_Bitmap))
                    {
                        cul_X = (float)Mouse_X;
                        cul_Y = (float)Mouse_Y;
                        g.DrawLine(new Pen(brush, Mask_Brush), pre_X, pre_Y, cul_X, cul_Y);
                        g.FillEllipse(brush, cul_X - (Mask_Brush / 2), cul_Y - (Mask_Brush / 2), Mask_Brush, Mask_Brush);
                        pre_X = cul_X;
                        pre_Y = cul_Y;
                    }

                    MaskGraphic.Image = new CogImage8Grey(Mask_Bitmap);
                }
            }
        }

        private void Pixel_Data_Read()
        {
            if (Mouse_X < 0)
                Mouse_X = 0;
            if (Mouse_X > Model_Display.Image.Width)
                Mouse_X = Model_Display.Image.Width - 1;
            if (Mouse_Y < 0)
                Mouse_Y = 0;
            if (Mouse_Y > Model_Display.Image.Height)
                Mouse_Y = Model_Display.Image.Height - 1;
            if (Label_Mouse_X.InvokeRequired)
            {
                Label_Mouse_X.Invoke((MethodInvoker)delegate
                {
                    Label_Mouse_X.Text = Mouse_X.ToString("0.0");
                });
            }
            else
            {
                Label_Mouse_X.Text = Mouse_X.ToString("0.0");
            }
            if (Label_Mouse_Y.InvokeRequired)
            {
                Label_Mouse_Y.Invoke((MethodInvoker)delegate
                {
                    Label_Mouse_Y.Text = Mouse_Y.ToString("0.0");
                });
            }
            else
            {
                Label_Mouse_Y.Text = Mouse_Y.ToString("0.0");
            }
            try
            {
                if (Label_Mouse_B.InvokeRequired)
                {
                    Label_Mouse_B.Invoke((MethodInvoker)delegate
                    {
                        Label_Mouse_B.Text = ((CogImage8Grey)Model_Display.Image).GetPixel((int)Mouse_X, (int)Mouse_Y).ToString();
                    });
                }
                else
                {
                    Label_Mouse_B.Text = ((CogImage8Grey)Model_Display.Image).GetPixel((int)Mouse_X, (int)Mouse_Y).ToString();
                }
            }
            catch
            { }
        }

        private void Model_Display_MouseDown(object sender, MouseEventArgs e)
        {
            Cognex.VisionPro.Display.CogDisplay display = (Cognex.VisionPro.Display.CogDisplay)sender;
            if (e.Button.Equals(MouseButtons.Left) && (Mesure_Use))
            {
                Display_Click = true;
                IsStatic_Graphics = false;
                Model_Display.StaticGraphics.Clear();
                Dis_Mesure_Start_X = 0;
                Dis_Mesure_Start_Y = 0;
                Dis_Mesure_End_X = 0;
                Dis_Mesure_End_Y = 0;
                double _X, _Y;
                CogTransform2DLinear Pointer;
                Pointer = Model_Display.GetTransform("#", "*") as CogTransform2DLinear;
                Pointer.MapPoint(e.X, e.Y, out _X, out _Y);
                Dis_Mesure_Start_X = _X;
                Dis_Mesure_Start_Y = _Y;
                Dis_Mesure_End_X = _X;
                Dis_Mesure_End_Y = _Y;
                Mesure_Drawing(CogColorConstants.Blue);
            }
            if (e.Button.Equals(MouseButtons.Left) && (Brush) && !Mask_Rectangle.Selected && display.MouseMode == Cognex.VisionPro.Display.CogDisplayMouseModeConstants.Pointer)
            {
                //수정중 마우스 클릭이동시 점 찍히는거
                Display_Click = true;
                double _X, _Y;
                CogTransform2DLinear Pointer;
                Pointer = Model_Display.GetTransform("#", "*") as CogTransform2DLinear;
                Pointer.MapPoint(e.X, e.Y, out _X, out _Y);
                pre_X = (float)_X;
                pre_Y = (float)_Y;
            }
        }

        private void Model_Display_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Left) && (Mesure_Use))
                Display_Click = false;
            if (e.Button.Equals(MouseButtons.Left) && (Brush))
                Display_Click = false;
            if (e.Button.Equals(MouseButtons.Left) && (Current_Line_Tool.RunParams.ExpectedLineSegment.Selected))
            {
                Line_Point_Display();
            }
            if (e.Button.Equals(MouseButtons.Left) && (Current_Circle_Tool.RunParams.ExpectedCircularArc.Selected))
            {
                Circle_Point_Display();
            }
        }

        private void Button_Masure_Click(object sender, EventArgs e)
        {
            Mesure_Use = !Mesure_Use;
            if (Mesure_Use)
            {
                Button_Masure.Image = global::T_Align.Properties.Resources.M_Masure_Select;
                Button_Masure.ForeColor = Color.Cyan;
                Model_Display.InteractiveGraphics.Clear();
                Mark_Type_Cancel();
            }
            else
            {
                Button_Masure.Image = global::T_Align.Properties.Resources.M_Masure;
                Button_Masure.ForeColor = Color.White;
            }
        }

        private void Brush_Click(object sender, EventArgs e)
        {
            for (int LoopCount = 0; LoopCount < 6; LoopCount++)
                Brush_Button[LoopCount].Check = false;
            Check_Button button = (Check_Button)sender;
            Mask_Brush = int.Parse(button.Name.Substring(13, button.Name.Length - 13));
        }

        private void Button_Brush_Use_Click(object sender, EventArgs e)
        {
            Brush = !Brush;
            if (Brush)
            {
                Button_Brush_Use.Image = global::T_Align.Properties.Resources.Button1_Select;
                Button_Brush_Use.ForeColor = Color.Cyan;
            }
            else
            {
                Button_Brush_Use.Image = global::T_Align.Properties.Resources.Button1;
                Button_Brush_Use.ForeColor = Color.White;
            }
        }

        private void Button_Mask_Click(object sender, EventArgs e)
        {
            brush.Color = Color.Black;
            Button_Mask.Image = global::T_Align.Properties.Resources.Button6_Select;
            Button_Mask.ForeColor = Color.Cyan;
            Button_UnMask.Image = global::T_Align.Properties.Resources.Button6;
            Button_UnMask.ForeColor = Color.White;
        }

        private void Button_UnMask_Click(object sender, EventArgs e)
        {
            brush.Color = Color.White;
            Button_Mask.Image = global::T_Align.Properties.Resources.Button6;
            Button_Mask.ForeColor = Color.White;
            Button_UnMask.Image = global::T_Align.Properties.Resources.Button6_Select;
            Button_UnMask.ForeColor = Color.Cyan;
        }

        private void Button_Fill_ALL_Click(object sender, EventArgs e)
        {
            Rectangle rectangle = new Rectangle(0, 0, Mask_Bitmap.Width, Mask_Bitmap.Height);
            using (Graphics g = Graphics.FromImage(Mask_Bitmap))
                g.FillRectangle(brush, rectangle);
            MaskGraphic.Image = new CogImage8Grey(Mask_Bitmap);
        }

        private void Button_Fill_Rectangle_Click(object sender, EventArgs e)
        {
            Rectangle rectangle = new Rectangle((int)Mask_Rectangle.X, (int)Mask_Rectangle.Y, (int)Mask_Rectangle.Width, (int)Mask_Rectangle.Height);
            using (Graphics g = Graphics.FromImage(Mask_Bitmap))
                g.FillRectangle(brush, rectangle);
            MaskGraphic.Image = new CogImage8Grey(Mask_Bitmap);
        }

        private void Button_Find_Mark_Click(object sender, EventArgs e)
        {
            X = Y = S = A = R = T = 0;
            DateTime Start = DateTime.Now;
            Model_Display.InteractiveGraphics.Clear();
            Model_Display.StaticGraphics.Clear();

            switch (Current_Mark_Type)
            {
                case Enum.Mark_Type.MARK:
                    COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Mark_Find(Model_Display, (int)Current_Multi_Mark_Index, out X, out Y, out S);
                    break;
                case Enum.Mark_Type.LINE:
                    COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Line_Find(Model_Display, ref X, ref Y, out A);
                    break;
                case Enum.Mark_Type.CIRCLE:
                    COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Circle_Find(Model_Display, ref X, ref Y, out R);
                    break;
                case Enum.Mark_Type.PATTERN:
                    COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Pattern_Find(Model_Display, out X, out Y, out S);
                    break;
                default:
                    COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Mark_Find(Model_Display, (int)Current_Multi_Mark_Index, out X, out Y, out S);
                    break;
            }
            T = ((DateTime.Now - Start).TotalMilliseconds) / 1000;
            Find_Data_Refresh();
        }

        private void Line_Updown1_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown numeric = (NumericUpDown)sender;
            switch (numeric.Name.Substring(numeric.Name.Length - 1, 1))
            {
                case "1":
                    Current_Line_Tool.RunParams.NumCalipers = (int)numeric.Value;
                    break;
                case "2":
                    Current_Line_Tool.RunParams.CaliperSearchLength = (int)numeric.Value;
                    break;
                case "3":
                    Current_Line_Tool.RunParams.CaliperProjectionLength = (int)numeric.Value;
                    break;
                case "4":
                    Current_Line_Tool.RunParams.NumToIgnore = (int)numeric.Value;
                    break;
                case "5":
                    Current_Line_Tool.RunParams.CaliperRunParams.ContrastThreshold = (int)numeric.Value;
                    break;
                case "6":
                    Current_Line_Tool.RunParams.CaliperRunParams.FilterHalfSizeInPixels = (int)numeric.Value;
                    break;
            }
            Model_Display.InteractiveGraphics.Clear();
            if(!E_R_Use)
                Line_Drawing(Current_Line_Tool);
            else
                Run_Line_Drawing(Current_Line_Tool);
        }

        private void Mesure_Drawing(CogColorConstants Color)
        {
            Model_Display.InteractiveGraphics.Clear();
            CogLine Display1_1_X = new CogLine();
            CogLine Display1_1_Y = new CogLine();
            CogLineSegment Display1_2_X = new CogLineSegment();
            CogLineSegment Display1_2_Y = new CogLineSegment();

            Display1_1_X.Color = Color;
            Display1_1_Y.Color = Color;
            Display1_2_X.Color = Color;
            Display1_2_Y.Color = Color;

            try
            {
                Display1_1_X.SetFromStartXYEndXY(Dis_Mesure_Start_X, 0, Dis_Mesure_Start_X, Model_Display.Height);
                Display1_1_Y.SetFromStartXYEndXY(0, Dis_Mesure_Start_Y, Model_Display.Width, Dis_Mesure_Start_Y);
            }
            catch { }
            try
            {
                Display1_2_X.SetStartEnd(Dis_Mesure_Start_X, Dis_Mesure_End_Y, Dis_Mesure_End_X, Dis_Mesure_End_Y);
                Display1_2_Y.SetStartEnd(Dis_Mesure_End_X, Dis_Mesure_Start_Y, Dis_Mesure_End_X, Dis_Mesure_End_Y);
            }
            catch { }

            if (!IsStatic_Graphics)
            {
                Model_Display.StaticGraphics.Add(Display1_1_X, "X");
                Model_Display.StaticGraphics.Add(Display1_1_Y, "Y");
                IsStatic_Graphics = true;
            }
            Model_Display.InteractiveGraphics.Add(Display1_2_X, "X", true);
            Model_Display.InteractiveGraphics.Add(Display1_2_Y, "Y", true);
        }

        private void Mesure_Data_Display()
        {
            if (Label_Masure_X.InvokeRequired)
            {
                Label_Masure_X.Invoke((MethodInvoker)delegate
                {
                    Label_Masure_X.Text = ((Math.Abs(Dis_Mesure_Start_X - Dis_Mesure_End_X) * COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Resolution_X) / 1000).ToString("0.000");
                });
            }
            else
            {
                Label_Masure_X.Text = ((Math.Abs(Dis_Mesure_Start_X - Dis_Mesure_End_X) * COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Resolution_X) / 1000).ToString("0.000");
            }
            if (Label_Masure_Y.InvokeRequired)
            {
                Label_Masure_Y.Invoke((MethodInvoker)delegate
                {
                    Label_Masure_Y.Text = ((Math.Abs(Dis_Mesure_Start_Y - Dis_Mesure_End_Y) * COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Resolution_Y) / 1000).ToString("0.000");
                });
            }
            else
            {
                Label_Masure_Y.Text = ((Math.Abs(Dis_Mesure_Start_Y - Dis_Mesure_End_Y) * COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Resolution_Y) / 1000).ToString("0.000");
            }
        }

        private void Button_Line_Direction_R_Click(object sender, EventArgs e)
        {
            Current_Line_Tool.RunParams.CaliperSearchDirection = -Current_Line_Tool.RunParams.CaliperSearchDirection;
            if (!E_R_Use)
                Line_Drawing(Current_Line_Tool);
            else
                Run_Line_Drawing(Current_Line_Tool);
        }

        private void Contrast_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Contrast_Select(button.Name.Substring(button.Name.Length - 1, 1));
        }

        private void Contrast_Select(string _Select)
        {
            Button_Line_Contrast_C.Image = global::T_Align.Properties.Resources.Button5;
            Button_Line_Position_P.Image = global::T_Align.Properties.Resources.Button5;
            Button_Line_PositionNeg_N.Image = global::T_Align.Properties.Resources.Button5;
            Button_Line_Contrast_C.ForeColor = Color.White;
            Button_Line_Position_P.ForeColor = Color.White;
            Button_Line_PositionNeg_N.ForeColor = Color.White;
            switch (_Select)
            {
                case "C":
                    CogCaliperScorerContrast Contrast = new CogCaliperScorerContrast();
                    Current_Line_Tool.RunParams.CaliperRunParams.SingleEdgeScorers.RemoveAt(0);
                    Current_Line_Tool.RunParams.CaliperRunParams.SingleEdgeScorers.Add(Contrast);
                    Button_Line_Contrast_C.Image = global::T_Align.Properties.Resources.Button5_Select;
                    Button_Line_Contrast_C.ForeColor = Color.Cyan;
                    break;
                case "P":
                    CogCaliperScorerPosition Position = new CogCaliperScorerPosition();
                    Current_Line_Tool.RunParams.CaliperRunParams.SingleEdgeScorers.RemoveAt(0);
                    Current_Line_Tool.RunParams.CaliperRunParams.SingleEdgeScorers.Add(Position);
                    Button_Line_Position_P.Image = global::T_Align.Properties.Resources.Button5_Select;
                    Button_Line_Position_P.ForeColor = Color.Cyan;
                    break;
                case "N":
                    CogCaliperScorerPositionNeg Position_Neg = new CogCaliperScorerPositionNeg();
                    Current_Line_Tool.RunParams.CaliperRunParams.SingleEdgeScorers.RemoveAt(0);
                    Current_Line_Tool.RunParams.CaliperRunParams.SingleEdgeScorers.Add(Position_Neg);
                    Button_Line_PositionNeg_N.Image = global::T_Align.Properties.Resources.Button5_Select;
                    Button_Line_PositionNeg_N.ForeColor = Color.Cyan;
                    break;
            }
            if (!E_R_Use)
                Line_Drawing(Current_Line_Tool);
            else
                Run_Line_Drawing(Current_Line_Tool);
        }

        private void Circle_Contrast_Select(string _Select)
        {
            Button_Circle_Contrast_C.Image = global::T_Align.Properties.Resources.Button5;
            Button_Circle_Position_P.Image = global::T_Align.Properties.Resources.Button5;
            Button_Circle_PositionNeg_N.Image = global::T_Align.Properties.Resources.Button5;
            Button_Circle_Contrast_C.ForeColor = Color.White;
            Button_Circle_Position_P.ForeColor = Color.White;
            Button_Circle_PositionNeg_N.ForeColor = Color.White;
            switch (_Select)
            {
                case "C":
                    CogCaliperScorerContrast Contrast = new CogCaliperScorerContrast();
                    Current_Circle_Tool.RunParams.CaliperRunParams.SingleEdgeScorers.RemoveAt(0);
                    Current_Circle_Tool.RunParams.CaliperRunParams.SingleEdgeScorers.Add(Contrast);
                    Button_Circle_Contrast_C.Image = global::T_Align.Properties.Resources.Button5_Select;
                    Button_Circle_Contrast_C.ForeColor = Color.Cyan;
                    break;
                case "P":
                    CogCaliperScorerPosition Position = new CogCaliperScorerPosition();
                    Current_Circle_Tool.RunParams.CaliperRunParams.SingleEdgeScorers.RemoveAt(0);
                    Current_Circle_Tool.RunParams.CaliperRunParams.SingleEdgeScorers.Add(Position);
                    Button_Circle_Position_P.Image = global::T_Align.Properties.Resources.Button5_Select;
                    Button_Circle_Position_P.ForeColor = Color.Cyan;
                    break;
                case "N":
                    CogCaliperScorerPositionNeg Position_Neg = new CogCaliperScorerPositionNeg();
                    Current_Circle_Tool.RunParams.CaliperRunParams.SingleEdgeScorers.RemoveAt(0);
                    Current_Circle_Tool.RunParams.CaliperRunParams.SingleEdgeScorers.Add(Position_Neg);
                    Button_Circle_PositionNeg_N.Image = global::T_Align.Properties.Resources.Button5_Select;
                    Button_Circle_PositionNeg_N.ForeColor = Color.Cyan;
                    break;
            }
            if (!E_R_Use)
                Circle_Drawing(Current_Circle_Tool);
            else
                Run_Circle_Drawing(Current_Circle_Tool);
        }

        private string Scoring_name_Return(ICogCaliperScorer _Scorer)
        {
            switch (_Scorer.GetType().Name.Substring(16, _Scorer.GetType().Name.Length - 16))
            {
                case "Contrast":
                    return "C";
                case "Position":
                    return "P";
                default:
                    return "N";
            }
        }

        private void Line_Color_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Line_Color_Select(button.Name.Substring(button.Name.Length - 1, 1));
        }

        private void Circle_Color_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Circle_Color_Select(button.Name.Substring(button.Name.Length - 1, 1));
        }

        private void Line_Color_Select(string _Sel)
        {
            Button_Line_L_To_D.Image = global::T_Align.Properties.Resources.Button5;
            Button_Line_D_To_L.Image = global::T_Align.Properties.Resources.Button5;
            Button_Line_L_To_D.ForeColor = Color.White;
            Button_Line_D_To_L.ForeColor = Color.White;
            switch (_Sel)
            {
                case "D":
                    Current_Line_Tool.RunParams.CaliperRunParams.Edge0Polarity = CogCaliperPolarityConstants.LightToDark;
                    Button_Line_L_To_D.Image = global::T_Align.Properties.Resources.Button5_Select;
                    Button_Line_L_To_D.ForeColor = Color.Cyan;
                    break;
                case "L":
                    Current_Line_Tool.RunParams.CaliperRunParams.Edge0Polarity = CogCaliperPolarityConstants.DarkToLight;
                    Button_Line_D_To_L.Image = global::T_Align.Properties.Resources.Button5_Select;
                    Button_Line_D_To_L.ForeColor = Color.Cyan;
                    break;
            }
            if (!E_R_Use)
                Line_Drawing(Current_Line_Tool);
            else
                Run_Line_Drawing(Current_Line_Tool);
        }

        private void Circle_Color_Select(string _Sel)
        {
            Button_Circle_L_To_D.Image = global::T_Align.Properties.Resources.Button5;
            Button_Circle_D_To_L.Image = global::T_Align.Properties.Resources.Button5;
            Button_Circle_L_To_D.ForeColor = Color.White;
            Button_Circle_D_To_L.ForeColor = Color.White;
            switch (_Sel)
            {
                case "D":
                    Current_Circle_Tool.RunParams.CaliperRunParams.Edge0Polarity = CogCaliperPolarityConstants.LightToDark;
                    Button_Circle_L_To_D.Image = global::T_Align.Properties.Resources.Button5_Select;
                    Button_Circle_L_To_D.ForeColor = Color.Cyan;
                    break;
                case "L":
                    Current_Circle_Tool.RunParams.CaliperRunParams.Edge0Polarity = CogCaliperPolarityConstants.DarkToLight;
                    Button_Circle_D_To_L.Image = global::T_Align.Properties.Resources.Button5_Select;
                    Button_Circle_D_To_L.ForeColor = Color.Cyan;
                    break;
            }
            if (!E_R_Use)
                Circle_Drawing(Current_Circle_Tool);
            else
                Run_Circle_Drawing(Current_Circle_Tool);
        }

        private void Line_Drawing(CogFindLineTool _Tool)
        {
            Model_Display.InteractiveGraphics.Clear();
            ICogRecord record = _Tool.CreateCurrentRecord();
            CogGraphicCollection Caliper = (CogGraphicCollection)record.SubRecords["InputImage"].SubRecords["CaliperRegions"].Content;
            _Tool.RunParams.ExpectedLineSegment.Color = CogColorConstants.Cyan;
            _Tool.RunParams.ExpectedLineSegment.SelectedColor = CogColorConstants.Cyan;
            Caliper.Add(_Tool.RunParams.ExpectedLineSegment as ICogGraphicInteractive);
            for (int iLoopCount = 0; iLoopCount < Caliper.Count; iLoopCount++)
            {
                Caliper[iLoopCount].Color = CogColorConstants.Blue;
                Model_Display.InteractiveGraphics.Add((ICogGraphicInteractive)Caliper[iLoopCount], "", false);
            }
        }

        private void Circle_Drawing(CogFindCircleTool _Tool)
        {
            Model_Display.InteractiveGraphics.Clear();
            ICogRecord record = _Tool.CreateCurrentRecord();
            CogGraphicCollection Caliper = (CogGraphicCollection)record.SubRecords["InputImage"].SubRecords["CaliperRegions"].Content;
            _Tool.RunParams.ExpectedCircularArc.Color = CogColorConstants.Cyan;
            Caliper.Add(_Tool.RunParams.ExpectedCircularArc);
            for (int iLoopCount = 0; iLoopCount < Caliper.Count; iLoopCount++)
            {
                Caliper[iLoopCount].Color = CogColorConstants.Blue;
                Model_Display.InteractiveGraphics.Add((ICogGraphicInteractive)Caliper[iLoopCount], "", false);
            }
        }

        private void Run_Line_Drawing(CogFindLineTool _Tool)
        {
            try
            {
                _Tool.InputImage = Model_Display.Image as CogImage8Grey;
                _Tool.Run();
            }
            catch
            {
                return;
            }
            Model_Display.InteractiveGraphics.Clear();
            try
            {
                for (int Loopcount = 0; Loopcount < _Tool.Results.Count; Loopcount++)
                    Model_Display.InteractiveGraphics.Add(_Tool.Results[Loopcount].CreateResultGraphics(CogFindLineResultGraphicConstants.DataPoint), "L", false);
            }
            catch { }
            try
            {
                Model_Display.InteractiveGraphics.Add(_Tool.Results.GetLine(), "L", false);
            }
            catch { }
        }

        private void Run_Circle_Drawing(CogFindCircleTool _Tool)
        {
            try
            {
                _Tool.InputImage = Model_Display.Image as CogImage8Grey;
                _Tool.Run();
            }
            catch
            {
                return;
            }
            Model_Display.InteractiveGraphics.Clear();
            try
            {
                for (int Loopcount = 0; Loopcount < _Tool.Results.Count; Loopcount++)
                    Model_Display.InteractiveGraphics.Add(_Tool.Results[Loopcount].CreateResultGraphics(CogFindCircleResultGraphicConstants.DataPoint), "C", false);
                Model_Display.InteractiveGraphics.Add(_Tool.Results.GetCircle(), "C", false);
            }
            catch { }
        }

        private void Circle_UpDown7_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown numeric = (NumericUpDown)sender;
            switch (numeric.Name.Substring(numeric.Name.Length - 1, 1))
            {
                case "1":
                    Current_Circle_Tool.RunParams.NumCalipers = (int)numeric.Value;
                    break;
                case "2":
                    Current_Circle_Tool.RunParams.CaliperSearchLength = (int)numeric.Value;
                    break;
                case "3":
                    Current_Circle_Tool.RunParams.CaliperProjectionLength = (int)numeric.Value;
                    break;
                case "4":
                    Current_Circle_Tool.RunParams.NumToIgnore = (int)numeric.Value;
                    break;
                case "5":
                    Current_Circle_Tool.RunParams.CaliperRunParams.ContrastThreshold = (int)numeric.Value;
                    break;
                case "6":
                    Current_Circle_Tool.RunParams.CaliperRunParams.FilterHalfSizeInPixels = (int)numeric.Value;
                    break;
                case "7":
                    if (numeric.Value == 999)
                    {
                        Current_Circle_Tool.RunParams.RadiusConstraint = (int)numeric.Value;
                        Current_Circle_Tool.RunParams.RadiusConstraintEnabled = false;
                    }
                    else
                    {
                        Current_Circle_Tool.RunParams.RadiusConstraint = (int)numeric.Value;
                        Current_Circle_Tool.RunParams.RadiusConstraintEnabled = true;
                    }
                    break;
            }
            Model_Display.InteractiveGraphics.Clear();
            if (!E_R_Use)
                Circle_Drawing(Current_Circle_Tool);
            else
                Run_Circle_Drawing(Current_Circle_Tool);
        }

        private void Circle_Contrast_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Circle_Contrast_Select(button.Name.Substring(button.Name.Length - 1, 1));
        }

        private void Button_Circle_Direction_R_Click(object sender, EventArgs e)
        {
            if (Current_Circle_Tool.RunParams.CaliperSearchDirection == CogFindCircleSearchDirectionConstants.Outward)
                Current_Circle_Tool.RunParams.CaliperSearchDirection = CogFindCircleSearchDirectionConstants.Inward;
            else
                Current_Circle_Tool.RunParams.CaliperSearchDirection = CogFindCircleSearchDirectionConstants.Outward;
            if (!E_R_Use)
                Circle_Drawing(Current_Circle_Tool);
            else
                Run_Circle_Drawing(Current_Circle_Tool);
        }

        private void Line_Static_Drawing(CogFindLineTool _Tool)
        {
            ICogRecord record = _Tool.CreateCurrentRecord();
            CogGraphicCollection Caliper = (CogGraphicCollection)record.SubRecords["InputImage"].SubRecords["CaliperRegions"].Content;
            _Tool.RunParams.ExpectedLineSegment.Color = CogColorConstants.Green;
            _Tool.RunParams.ExpectedLineSegment.SelectedColor = CogColorConstants.Green;
            Caliper.Add(_Tool.RunParams.ExpectedLineSegment as ICogGraphicInteractive);
            for (int iLoopCount = 0; iLoopCount < Caliper.Count; iLoopCount++)
            {
                Caliper[iLoopCount].Color = CogColorConstants.Green;
                Model_Display.StaticGraphics.Add((ICogGraphicInteractive)Caliper[iLoopCount], "");
            }
        }

        private void Button_Find_Mark_To_Click(object sender, EventArgs e)
        {
            DateTime Start = DateTime.Now;
            if (Current_Mark_Type == Enum.Mark_Type.MARK_TO_CIRCLE)
            {
                COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Mark_Find(Model_Display, (int)Current_Multi_Mark_Index, out X, out Y, out S);
                Model_Display.InteractiveGraphics.Clear();
                Model_Display.StaticGraphics.Clear();
                COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Circle_Find(Model_Display, ref X, ref Y, out R);
            }
            else if (Current_Mark_Type == Enum.Mark_Type.MARK_TO_LINE)
            {
                COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Mark_Find(Model_Display, (int)Current_Multi_Mark_Index, out X, out Y, out S);
                Model_Display.InteractiveGraphics.Clear();
                Model_Display.StaticGraphics.Clear();
                COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Line_Find(Model_Display, ref X, ref Y, out A);
            }
            T = ((DateTime.Now - Start).TotalMilliseconds) / 1000;
            Find_Data_Refresh();
        }

        private void Line_Point_Display()
        {
            Nmr_Line_St_X.Value = (decimal)Current_Line_Tool.RunParams.ExpectedLineSegment.StartX;
            Nmr_Line_St_Y.Value = (decimal)Current_Line_Tool.RunParams.ExpectedLineSegment.StartY;
            Nmr_Line_Ed_X.Value = (decimal)Current_Line_Tool.RunParams.ExpectedLineSegment.EndX;
            Nmr_Line_Ed_Y.Value = (decimal)Current_Line_Tool.RunParams.ExpectedLineSegment.EndY;
        }

        private void Button_Copy_Click(object sender, EventArgs e)
        {
            if (!Copy_Use && COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Multi_Mark[Current_Multi_Mark_Index] == null && Mark_Copy_CheckButton.Check)
                return;
            Copy_Use = !Copy_Use;
            if (Copy_Use)
            {
                Button_Copy.Image = global::T_Align.Properties.Resources.Unit_Select;
                Button_Copy.Text = "PASTE";
                Button_Copy.ForeColor = Color.Cyan;
                if (Mark_Copy_CheckButton.Check)
                    Copy_PMAlign_Tool = new CogPMAlignTool(COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Multi_Mark[Current_Multi_Mark_Index]);
                if (Line_Copy_CheckButton.Check)
                {
                    Copy_Line_Tool = new CogFindLineTool(COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Line_W);
                    Copy_Line_SubTool = new CogFindLineTool(COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Line_H);
                }
                if (Circle_Copy_CheckButton.Check)
                    Copy_Circle_Tool = new CogFindCircleTool(COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Circle);
            }
            else
            {
                Button_Copy.Image = global::T_Align.Properties.Resources.Unit;
                Button_Copy.Text = "COPY";
                Button_Copy.ForeColor = Color.White;
                if (Mark_Copy_CheckButton.Check)
                {
                    COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Multi_Mark[Current_Multi_Mark_Index] = new CogPMAlignTool(Copy_PMAlign_Tool);
                    COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Mark_Save(Current_Multi_Mark_Index);
                }

                if (Line_Copy_CheckButton.Check)
                {
                    COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Line_W = new CogFindLineTool(Copy_Line_Tool);
                    COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Line_H = new CogFindLineTool(Copy_Line_SubTool);
                    COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Line_Save("W");
                    COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Line_Save("H");
                }
                if (Circle_Copy_CheckButton.Check)
                {
                    COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Circle = new CogFindCircleTool(Copy_Circle_Tool);
                    COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Circle_Save();
                }
                Mark_Select();
            }
            Copy_Panel.Enabled = !Copy_Use;
        }

        private void Button_Delete_Click(object sender, EventArgs e)
        {
            if (Mark_Copy_CheckButton.Check)
            {
                COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Mark_Delete(Current_Multi_Mark_Index);
                COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Multi_Mark[Current_Multi_Mark_Index] = null;
                Mark_Select();
            }
        }

        private void Button_MasterPosition_Click(object sender, EventArgs e)
        {
            Master_Position master_Position = new Master_Position(COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type], Current_Unit, Current_Type);
            if (master_Position.ShowDialog() == DialogResult.OK)
            {
                COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Master_Position_Save(master_Position.L_Check_Spec, master_Position.L_Check_Offset, master_Position.Position_X, master_Position.Position_Y, master_Position.L_Check_Limit);
                master_Position.Dispose();
            }
        }

        private void Button_Train_Click(object sender, EventArgs e)
        {
            if(Current_Mark_Type == Enum.Mark_Type.PATTERN)
            {
                try
                {
                    Model_Display.InteractiveGraphics.Remove("F");
                }
                catch { }
                try
                {
                    Panel_Pattern_Image.Pattern_Display.InteractiveGraphics.Remove("F");
                }
                catch { }
                Current_CNL_Tool.Pattern.TrainImage = Model_Display.Image as CogImage8Grey;
                Current_CNL_Tool.Pattern.OriginX = Pattern_Origin.OriginX;
                Current_CNL_Tool.Pattern.OriginY = Pattern_Origin.OriginY;

                Current_CNL_Tool.Pattern.TrainRegion = Pattern_Rectangle;
                Current_CNL_Tool.CurrentRecordEnable = CogCNLSearchCurrentRecordConstants.All;
                if(Linear_CheckButton.Check)
                {
                    Current_CNL_Tool.RunParams.Algorithm = CogCNLSearchAlgorithmConstants.LinearCNLPAS;
                    Current_CNL_Tool.Pattern.Algorithms = CogCNLSearchAlgorithmConstants.LinearCNLPAS;
                }
                else if(NONLinear_CheckButton.Check)
                {
                    Current_CNL_Tool.Pattern.Algorithms = CogCNLSearchAlgorithmConstants.NonLinearCNLPAS;
                    Current_CNL_Tool.RunParams.Algorithm = CogCNLSearchAlgorithmConstants.NonLinearCNLPAS;
                    Current_CNL_Tool.Pattern.EdgeThresholdLow = (int)Nmr_Thres_Low.Value;
                    Current_CNL_Tool.Pattern.EdgeThresholdHigh = (int)Nmr_Thres_High.Value;
                }
                Current_CNL_Tool.Pattern.Train();
                Current_CNL_Tool.InputImage = Model_Display.Image as CogImage8Grey;
                Current_CNL_Tool.Run();
                Panel_Pattern_Image.Pattern_Display.Image = Current_CNL_Tool.Pattern.GetTrainedPatternImage();

                Panel_Pattern_Image.Pattern_Display.StaticGraphics.Clear();

                ICogRecord CurrentRecord = Current_CNL_Tool.CreateCurrentRecord();
                if (NONLinear_CheckButton.Check)
                {
                    CogMaskGraphic cogMask = new CogMaskGraphic((CogMaskGraphic)CurrentRecord.SubRecords["TrainImage"].SubRecords["TrainedEdges"].Content);
                    CogMaskGraphic Model_Mask = new CogMaskGraphic((CogMaskGraphic)CurrentRecord.SubRecords["TrainImage"].SubRecords["TrainedEdges"].Content);
                    Model_Display.InteractiveGraphics.Add(Model_Mask, "F", false);
                    cogMask.OffsetX = 0;
                    cogMask.OffsetY = 0;
                    Panel_Pattern_Image.Pattern_Display.InteractiveGraphics.Add(cogMask, "F", false);
                }
            }
            else
            {
                try
                {
                    Current_PMAlign_Tool.RunParams.SaveMatchInfo = true;
                    Current_PMAlign_Tool.Pattern.TrainImage = Model_Display.Image;
                    Current_PMAlign_Tool.Pattern.Origin.TranslationX = Mark_Origin.OriginX;
                    Current_PMAlign_Tool.Pattern.Origin.TranslationY = Mark_Origin.OriginY;
                    Current_PMAlign_Tool.Pattern.TrainImageMask = new CogImage8Grey(Mask_Bitmap);
                    Current_PMAlign_Tool.Pattern.TrainRegion = Mark_Rectangle;
                    Current_PMAlign_Tool.CurrentRecordEnable = CogPMAlignCurrentRecordConstants.All;
                    Current_PMAlign_Tool.Pattern.Train();
                    Current_PMAlign_Tool.InputImage = Model_Display.Image;
                    Current_PMAlign_Tool.Run();
                    cog_Panel1.Pattern_Display.Image = Current_PMAlign_Tool.Pattern.GetTrainedPatternImage();
                    cog_Panel1.Pattern_Display.StaticGraphics.Clear();
                    cog_Panel1.Pattern_Display.StaticGraphics.AddList(Current_PMAlign_Tool.Pattern.CreateGraphicsFine(CogColorConstants.Yellow), "F");
                    cog_Panel1.Pattern_Display.StaticGraphics.AddList(Current_PMAlign_Tool.Pattern.CreateGraphicsCoarse(CogColorConstants.Green), "C");
                    Model_Display.StaticGraphics.Clear();
                    Model_Display.StaticGraphics.AddList(Current_PMAlign_Tool.Pattern.CreateGraphicsFine(CogColorConstants.Yellow), "F");
                    Model_Display.StaticGraphics.AddList(Current_PMAlign_Tool.Pattern.CreateGraphicsCoarse(CogColorConstants.Green), "C");
                    //ICogRecord CurrentRecord = Current_PMAlign_Tool.CreateCurrentRecord();
                    //CogGraphicCollection Coarse = (CogGraphicCollection)CurrentRecord.SubRecords["TrainImage"].SubRecords["FeaturesCoarse"].Content;
                    //CogGraphicCollection Fine = (CogGraphicCollection)CurrentRecord.SubRecords["TrainImage"].SubRecords["FeaturesFine"].Content;
                    //Model_Display.StaticGraphics.AddList(Coarse, "X");
                    //Model_Display.StaticGraphics.AddList(Fine, "X");
                }
                catch (Exception _e)
                {
                    MessageBox.Show("트레인 에러 : " + _e.ToString());
                    Current_PMAlign_Tool.Pattern.TrainImage = null;
                }
            }
        }

        private void Button_Fill_Circle_Click(object sender, EventArgs e)
        {
            Rectangle rectangle = new Rectangle((int)(Mask_Circle.CenterX - Mask_Circle.RadiusX), (int)(Mask_Circle.CenterY - Mask_Circle.RadiusY), (int)(Mask_Circle.RadiusX * 2), (int)(Mask_Circle.RadiusY * 2));
            using (Graphics g = Graphics.FromImage(Mask_Bitmap))
                g.FillEllipse(brush, rectangle);
            MaskGraphic.Image = new CogImage8Grey(Mask_Bitmap);
        }

        private void comboBox_MultiMark_Index_MouseLeave(object sender, EventArgs e)
        {
            if (MultiMark_Panel.Visible)
                MultiMark_Panel.Visible = false;
        }

        private void comboBox_MultiMark_Index_MouseMove(object sender, MouseEventArgs e)
        {
            if (!MultiMark_Panel.Visible)
                MultiMark_Panel.Visible = true;
        }

        private void Circle_Data_Changed(object sender, EventArgs e)
        {
            TextBox upDown = (TextBox)sender;
            if (upDown.Text != "")
            {
                switch (upDown.Name)
                {
                    case "Line_START_X":
                        Current_Line_Tool.RunParams.ExpectedLineSegment.StartX = int.Parse(upDown.Text);
                        break;
                    case "Line_START_Y":
                        Current_Line_Tool.RunParams.ExpectedLineSegment.StartY = int.Parse(upDown.Text);
                        break;
                    case "Line_END_X":
                        Current_Line_Tool.RunParams.ExpectedLineSegment.EndX = int.Parse(upDown.Text);
                        break;
                    case "Line_END_Y":
                        Current_Line_Tool.RunParams.ExpectedLineSegment.EndY = int.Parse(upDown.Text);
                        break;
                }
            }
        }

        private void Number_Only(object sender, KeyPressEventArgs e)
        {
            int Key = Convert.ToInt16(e.KeyChar);
            e.Handled = true;
            if ((char.IsDigit(e.KeyChar)) || Key.Equals(8) || Key.Equals(13) || Key.Equals(46))
            {
                e.Handled = false;
            }
        }

        private void Circle_Value_Changed(object sender, EventArgs e)
        {
            NumericUpDown upDown = (NumericUpDown)sender;
            switch(upDown.Name)
            {
                case "Nmr_Circle_X":
                    Current_Circle_Tool.RunParams.ExpectedCircularArc.CenterX = (double)upDown.Value;
                    break;
                case "Nmr_Circle_Y":
                    Current_Circle_Tool.RunParams.ExpectedCircularArc.CenterY = (double)upDown.Value;
                    break;
                case "Nmr_Circle_R":
                    Current_Circle_Tool.RunParams.ExpectedCircularArc.Radius = (double)upDown.Value;
                    break;
            }
            Model_Display.InteractiveGraphics.Clear();
            if (!E_R_Use)
                Circle_Drawing(Current_Circle_Tool);
            else
                Run_Circle_Drawing(Current_Circle_Tool);
        }

        private void Line_Value_Changed(object sender, EventArgs e)
        {
            NumericUpDown upDown = (NumericUpDown)sender;
            if (upDown.Text != "")
            {
                switch (upDown.Name)
                {
                    case "Nmr_Line_St_X":
                        Current_Line_Tool.RunParams.ExpectedLineSegment.StartX = (double)upDown.Value;
                        break;
                    case "Nmr_Line_St_Y":
                        Current_Line_Tool.RunParams.ExpectedLineSegment.StartY = (double)upDown.Value;
                        break;
                    case "Nmr_Line_Ed_X":
                        Current_Line_Tool.RunParams.ExpectedLineSegment.EndX = (double)upDown.Value;
                        break;
                    case "Nmr_Line_Ed_Y":
                        Current_Line_Tool.RunParams.ExpectedLineSegment.EndY = (double)upDown.Value;
                        break;
                }
                Model_Display.InteractiveGraphics.Clear();
                if (!E_R_Use)
                    Line_Drawing(Current_Line_Tool);
                else
                    Run_Line_Drawing(Current_Line_Tool);
            }
        }

        private void Button_Pattern_Mode_Click(object sender, EventArgs e)
        {
            if (Model_Display.Image == null)
                return;
            Mark_Type_Select("P");
            Pattern_Tool_Load();
        }

        private void Linear_CheckButton_Click(object sender, EventArgs e)
        {
            Check_Button check_Button = (Check_Button)sender;
            Linear_Check();
            if (!check_Button.Check && check_Button.Name == "NONLinear_CheckButton")
                Panel_NonLinear.Visible = true;
            else
                Panel_NonLinear.Visible = false;
        }

        private void Linear_Check()
        {
            NONLinear_CheckButton.Check = false;
            Linear_CheckButton.Check = false;
        }

        private void Button_Pattern_ROI_Use_Click(object sender, EventArgs e)
        {
            Pattern_ROI_Use(Button_Pattern_ROI_Use.Check);
        }

        private void Pattern_ROI_Use(bool UnSelect)
        {
            if(UnSelect)
            {
                try
                {
                    Model_Display.InteractiveGraphics.Remove("ROI");
                }
                catch { }
            }
            else
            {
                Model_Display.InteractiveGraphics.Add(Pattern_ROI_Rectangle, "ROI", false);
            }
        }

        private void Pattern_Origin_Move_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Name.Substring(11, button.Name.Length - 11))
            {
                case "U":
                    Pattern_Origin.OriginY -= 1;
                    break;
                case "UU":
                    Pattern_Origin.OriginY -= 10;
                    break;
                case "D":
                    Pattern_Origin.OriginY += 1;
                    break;
                case "DD":
                    Pattern_Origin.OriginY += 10;
                    break;
                case "R":
                    Pattern_Origin.OriginX += 1;
                    break;
                case "RR":
                    Pattern_Origin.OriginX += 10;
                    break;
                case "L":
                    Pattern_Origin.OriginX -= 1;
                    break;
                case "LL":
                    Pattern_Origin.OriginX -= 10;
                    break;
            }
        }

        private void Button_Ptn_Center_Click(object sender, EventArgs e)
        {
            Pattern_Origin.OriginX = Pattern_Rectangle.CenterX;
            Pattern_Origin.OriginY = Pattern_Rectangle.CenterY;
        }

        private void Button_Ptn_turn_Click(object sender, EventArgs e)
        {
            double Angle = Custom_Math.RadianToDegree(Pattern_Origin.Rotation) + 90;
            if (Angle == 360)
                Angle = 0;
            Pattern_Origin.Rotation = Custom_Math.DegreeToRadian(Angle);
        }

        private void Button_Ptn_Return_Click(object sender, EventArgs e)
        {
            double Angle = Custom_Math.RadianToDegree(Pattern_Origin.Rotation) - 90;
            if (Angle == -90)
                Angle = 270;
            Pattern_Origin.Rotation = Custom_Math.DegreeToRadian(Angle);
        }


        private void Circle_Point_Display()
        {
            try
            {
                Nmr_Circle_X.Value = (decimal)Current_Circle_Tool.RunParams.ExpectedCircularArc.CenterX;
                Nmr_Circle_Y.Value = (decimal)Current_Circle_Tool.RunParams.ExpectedCircularArc.CenterY;
                Nmr_Circle_R.Value = (decimal)Current_Circle_Tool.RunParams.ExpectedCircularArc.Radius;
            }
            catch {}
            
        }

        private void Find_Data_Refresh()
        {
            Label_Find_X.Text = X.ToString("0.000");
            Label_Find_Y.Text = Y.ToString("0.000");
            Label_Find_S.Text = S.ToString("0.000");
            if (Current_Mark_Type == Enum.Mark_Type.MARK_TO_LINE || Current_Mark_Type == Enum.Mark_Type.LINE)
                Label_Find_AR.Text = A.ToString("0.000");
            else if (Current_Mark_Type == Enum.Mark_Type.MARK_TO_CIRCLE || Current_Mark_Type == Enum.Mark_Type.CIRCLE)
                Label_Find_AR.Text = R.ToString("0.000");
            else
                Label_Find_AR.Text = "0.000";
            Label_Find_Tact.Text = T.ToString("0.000");
        }

        private void PM_Tool_Load()
        {
            Model_Display.InteractiveGraphics.Clear();
            Model_Display.StaticGraphics.Clear();
            ICogRegion cogRegion;
            cogRegion = Current_PMAlign_Tool.Pattern.TrainRegion;
            Mark_Rectangle = new CogRectangleAffine((CogRectangleAffine)cogRegion);
            Mark_Rectangle.SelectedColor = CogColorConstants.Cyan;
            if (Current_PMAlign_Tool.LastRunRecordDiagEnable == CogPMAlignLastRunRecordDiagConstants.SearchRegion)
                check_Button_ROI.Check = true;
            else
                check_Button_ROI.Check = false;
            cogRegion = Current_PMAlign_Tool.SearchRegion;
            try
            {
                ROI_Rectangle = new CogRectangleAffine((CogRectangleAffine)cogRegion);
                ROI_Rectangle.Color = CogColorConstants.Orange;
                ROI_Rectangle.SelectedColor = CogColorConstants.Orange;
                ROI_Use = true;
            }
            catch
            {
                ROI_Rectangle = new CogRectangleAffine();
                ROI_Use = false;
            }
            if (ROI_Use)
            {
                Button_ROI_Use.Image = global::T_Align.Properties.Resources.Button5_Select;
                Button_ROI_Use.ForeColor = Color.Cyan;
                Button_ROI_Use.Text = "Un Use ROI";
            }
            else
            {
                Button_ROI_Use.Image = global::T_Align.Properties.Resources.Button5;
                Button_ROI_Use.ForeColor = Color.White;
                Button_ROI_Use.Text = "Use ROI";
            }
            Pattern_Origin_Angle = 0;
            Mark_Origin = new CogCoordinateAxes();
            Mark_Origin.Interactive = true;
            Mark_Origin.Color = CogColorConstants.Red;
            Mark_Origin.SelectedColor = CogColorConstants.Red;
            Mark_Origin.OriginX = Current_PMAlign_Tool.Pattern.Origin.TranslationX;
            Mark_Origin.OriginY = Current_PMAlign_Tool.Pattern.Origin.TranslationY;
            Mark_Origin.GraphicDOFEnable = CogCoordinateAxesDOFConstants.Position;
            Mark_Origin.DisplayedXAxisLength = 200000;
            Numeric_Angle_Max.Value = (decimal)Custom_Math.RadianToDegree(Current_PMAlign_Tool.RunParams.ZoneAngle.High);
            Numeric_Angle_Min.Value = (decimal)Custom_Math.RadianToDegree(Current_PMAlign_Tool.RunParams.ZoneAngle.Low);
            Numeric_Scale_Max.Value = (decimal)Current_PMAlign_Tool.RunParams.ZoneScale.High;
            Numeric_Scale_Min.Value = (decimal)Current_PMAlign_Tool.RunParams.ZoneScale.Low;
            try
            {
                cog_Panel1.Pattern_Display.Image = Current_PMAlign_Tool.Pattern.GetTrainedPatternImage();
                cog_Panel1.Pattern_Display.StaticGraphics.Clear();
                cog_Panel1.Pattern_Display.StaticGraphics.AddList(Current_PMAlign_Tool.Pattern.CreateGraphicsFine(CogColorConstants.Yellow), "F");
                cog_Panel1.Pattern_Display.StaticGraphics.AddList(Current_PMAlign_Tool.Pattern.CreateGraphicsCoarse(CogColorConstants.Green), "C");
            }
            catch
            {
                cog_Panel1.Pattern_Display.Image = null;
            }
            if (Button_Pattern.ForeColor == Color.Cyan)
                Select_Mark_Graphic("P");
            if (Button_ROI.ForeColor == Color.Cyan)
                Select_Mark_Graphic("R");
            if (Button_Masking.ForeColor == Color.Cyan)
                Select_Mark_Graphic("M");
            Mark_Mathing_Rate.Value = (decimal)COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Mark_Match_Rate[Current_Multi_Mark_Index];

            brush = new SolidBrush(Color.White);
            try { Mask_Bitmap = Current_PMAlign_Tool.Pattern.TrainImageMask.ToBitmap(); }
            catch
            {
                Mask_Bitmap = new Bitmap(Model_Display.Image.Width, Model_Display.Image.Height);
                Rectangle rectangle = new Rectangle(0, 0, Model_Display.Image.Width, Model_Display.Image.Height);
                using (Graphics g = Graphics.FromImage(Mask_Bitmap))
                    g.FillRectangle(brush, rectangle);
                MaskGraphic.Image = new CogImage8Grey(Mask_Bitmap);
                Model_Display.InteractiveGraphics.Add(MaskGraphic, "", false);
            }
            brush.Color = Color.Black;
            Button_Mask.Image = global::T_Align.Properties.Resources.Button6_Select;
            Button_Mask.ForeColor = Color.Cyan;
            Button_UnMask.Image = global::T_Align.Properties.Resources.Button6;
            Button_UnMask.ForeColor = Color.White;
            Mask_Brush = 10;
            for (int LoopCount = 0; LoopCount < 6; LoopCount++)
                Brush_Button[LoopCount].Check = false;

            Brush_Button[2].Check = true;
            Button_Brush_Use.Image = global::T_Align.Properties.Resources.Button1;
            Button_Brush_Use.ForeColor = Color.White;
            Brush = false;
        }

        private void Line_Tool_Load()
        {
            Model_Display.InteractiveGraphics.Clear();
            Model_Display.StaticGraphics.Clear();
            Line_Static_Drawing(Current_Line_SubTool);
            Line_Param[0].Value = Current_Line_Tool.RunParams.NumCalipers;
            Line_Param[1].Value = (decimal)Current_Line_Tool.RunParams.CaliperSearchLength;
            Line_Param[2].Value = (decimal)Current_Line_Tool.RunParams.CaliperProjectionLength;
            Line_Param[3].Value = Current_Line_Tool.RunParams.NumToIgnore;
            Line_Param[4].Value = (decimal)Current_Line_Tool.RunParams.CaliperRunParams.ContrastThreshold;
            Line_Param[5].Value = Current_Line_Tool.RunParams.CaliperRunParams.FilterHalfSizeInPixels;
            if (Current_Line_Tool.RunParams.CaliperRunParams.Edge0Polarity == CogCaliperPolarityConstants.DarkToLight)
                Line_Color_Select("L");
            else
                Line_Color_Select("D");
            Contrast_Select(Scoring_name_Return(Current_Line_Tool.RunParams.CaliperRunParams.SingleEdgeScorers[0]));
            Line_Point_Display();
            if (!E_R_Use)
                Line_Drawing(Current_Line_Tool);
            else
                Run_Line_Drawing(Current_Line_Tool);
        }

        private void Circle_Tool_Load()
        {
            Model_Display.InteractiveGraphics.Clear();
            Model_Display.StaticGraphics.Clear();
            if (Current_Mark_Type == Enum.Mark_Type.MARK_TO_CIRCLE)
            {
                Current_Circle_Tool.RunParams.ExpectedCircularArc.CenterX += X;
                Current_Circle_Tool.RunParams.ExpectedCircularArc.CenterY += Y;
            }

            Current_Circle_Tool.InputImage = (CogImage8Grey)Model_Display.Image;
            Current_Circle_Tool.InputImage = (CogImage8Grey)Model_Display.Image;

            Circle_Param[0].Value = Current_Circle_Tool.RunParams.NumCalipers;
            Circle_Param[1].Value = (decimal)Current_Circle_Tool.RunParams.CaliperSearchLength;
            Circle_Param[2].Value = (decimal)Current_Circle_Tool.RunParams.CaliperProjectionLength;
            Circle_Param[3].Value = Current_Circle_Tool.RunParams.NumToIgnore;
            Circle_Param[4].Value = (decimal)Current_Circle_Tool.RunParams.CaliperRunParams.ContrastThreshold;
            Circle_Param[5].Value = Current_Circle_Tool.RunParams.CaliperRunParams.FilterHalfSizeInPixels;
            Circle_Param[6].Value = (decimal)Math.Round(Current_Circle_Tool.RunParams.RadiusConstraint, 0);
            if (Current_Circle_Tool.RunParams.CaliperRunParams.Edge0Polarity == CogCaliperPolarityConstants.DarkToLight)
                Circle_Color_Select("L");
            else
                Circle_Color_Select("D");
            Circle_Contrast_Select(Scoring_name_Return(Current_Circle_Tool.RunParams.CaliperRunParams.SingleEdgeScorers[0]));
            Circle_Point_Display();
            if (!E_R_Use)
                Circle_Drawing(Current_Circle_Tool);
            else
                Run_Circle_Drawing(Current_Circle_Tool);
        }

        private void Pattern_Tool_Load()
        {
            Model_Display.InteractiveGraphics.Clear();
            Model_Display.StaticGraphics.Clear();
            try
            {
                Panel_Pattern_Image.Pattern_Display.InteractiveGraphics.Clear();
                Current_CNL_Tool = new CogCNLSearchTool(COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].CNL);
                Panel_Pattern_Image.Pattern_Display.Image = Current_CNL_Tool.Pattern.GetTrainedPatternImage();
                if (Current_CNL_Tool.RunParams.Algorithm == CogCNLSearchAlgorithmConstants.LinearCNLPAS)
                {
                    Linear_Check();
                    Linear_CheckButton.Check = true;
                    Panel_NonLinear.Visible = false;
                }
                else if (Current_CNL_Tool.RunParams.Algorithm == CogCNLSearchAlgorithmConstants.NonLinearCNLPAS)
                {
                    Linear_Check();
                    NONLinear_CheckButton.Check = true;
                    Panel_NonLinear.Visible = true;
                }
            }
            catch
            {
                Current_CNL_Tool = new CogCNLSearchTool();
                Panel_Pattern_Image.Pattern_Display.Image = null;
                Current_CNL_Tool.RunParams.Algorithm = CogCNLSearchAlgorithmConstants.LinearCNLPAS;
                Current_CNL_Tool.Pattern.Algorithms = CogCNLSearchAlgorithmConstants.LinearCNLPAS;
                Linear_Check();
                Linear_CheckButton.Check = true;
                Current_CNL_Tool.Pattern.EdgeThresholdLow = 3;
                Current_CNL_Tool.Pattern.EdgeThresholdHigh = 9;
                Panel_NonLinear.Visible = false;
            }
            if (Current_CNL_Tool.LastRunRecordDiagEnable == CogCNLSearchLastRunRecordDiagConstants.SearchRegion)
                Button_Pattern_ROI_Display.Check = true;
            else
                Button_Pattern_ROI_Display.Check = false;
            Nmr_Thres_High.Value = Current_CNL_Tool.Pattern.EdgeThresholdHigh;
            Nmr_Thres_Low.Value = Current_CNL_Tool.Pattern.EdgeThresholdLow;
            Pattern_Origin = new CogCoordinateAxes();
            Pattern_Origin.DisplayedXAxisLength = 200000;
            Pattern_Origin.GraphicDOFEnable = CogCoordinateAxesDOFConstants.Position;
            Pattern_Origin.OriginX = Current_CNL_Tool.Pattern.OriginX;
            Pattern_Origin.OriginY = Current_CNL_Tool.Pattern.OriginY;
            Pattern_Origin.Color = CogColorConstants.Red;
            Pattern_Origin.SelectedColor = CogColorConstants.Red;
            Pattern_Origin.Interactive = true;
            Model_Display.InteractiveGraphics.Clear();
            Model_Display.StaticGraphics.Clear();
            try
            {
                Pattern_Rectangle = new CogRectangleAffine(Current_CNL_Tool.Pattern.TrainRegion as CogRectangleAffine);
            }
            catch
            {
                Pattern_Rectangle = new CogRectangleAffine();
            }
            try
            {
                Pattern_ROI_Rectangle = new CogRectangleAffine(Current_CNL_Tool.SearchRegion as CogRectangleAffine);
                Pattern_ROI_Use(false);
                Button_Pattern_ROI_Use.Check = true;
            }
            catch
            {
                Pattern_ROI_Rectangle = new CogRectangleAffine();
                Pattern_ROI_Use(true);
            }
            Pattern_Rectangle.Color = CogColorConstants.Cyan;
            Pattern_Rectangle.SelectedColor = CogColorConstants.Cyan;
            Pattern_ROI_Rectangle.Color = CogColorConstants.Orange;
            Pattern_ROI_Rectangle.SelectedColor = CogColorConstants.Orange;
            Pattern_Rectangle.GraphicDOFEnable = CogRectangleAffineDOFConstants.All;
            Pattern_ROI_Rectangle.GraphicDOFEnable = CogRectangleAffineDOFConstants.All;
            Pattern_Rectangle.Interactive = true;
            Pattern_ROI_Rectangle.Interactive = true;
            Model_Display.InteractiveGraphics.Add(Pattern_Origin, "O", false);
            Model_Display.InteractiveGraphics.Add(Pattern_Rectangle, "R", false);
            ICogRecord CurrentRecord = Current_CNL_Tool.CreateCurrentRecord();
            if (Current_CNL_Tool.RunParams.Algorithm == CogCNLSearchAlgorithmConstants.NonLinearCNLPAS)
            {
                CogMaskGraphic cogMask = new CogMaskGraphic((CogMaskGraphic)CurrentRecord.SubRecords["TrainImage"].SubRecords["TrainedEdges"].Content);
                CogMaskGraphic Model_Mask = new CogMaskGraphic((CogMaskGraphic)CurrentRecord.SubRecords["TrainImage"].SubRecords["TrainedEdges"].Content);
                Model_Display.InteractiveGraphics.Add(Model_Mask, "F", false);
                cogMask.OffsetX = 0;
                cogMask.OffsetY = 0;
                Panel_Pattern_Image.Pattern_Display.InteractiveGraphics.Add(cogMask, "F", false);
            }
        }

        private void Button_Pattern_Mode_Train_Image_Click(object sender, EventArgs e)
        {
            if (Current_CNL_Tool.Pattern.TrainImage != null)
                Model_Display.Image = Current_CNL_Tool.Pattern.TrainImage;
        }

        private void Button_Mark_Train_Image_Click(object sender, EventArgs e)
        {
            if (Current_PMAlign_Tool.Pattern.TrainImage != null)
                Model_Display.Image = Current_PMAlign_Tool.Pattern.TrainImage;
        }

        private void Button_Line_Sort_Click(object sender, EventArgs e)
        {
            if(Current_Tool_Type == "W")
            {
                Current_Line_Tool.RunParams.ExpectedLineSegment.EndY = Current_Line_Tool.RunParams.ExpectedLineSegment.StartY;
                Current_Line_Tool.RunParams.ExpectedLineSegment.EndX = Current_Line_Tool.RunParams.ExpectedLineSegment.StartX + (Current_Line_Tool.RunParams.NumCalipers * Current_Line_Tool.RunParams.CaliperProjectionLength);
            }
            else if (Current_Tool_Type == "H")
            {
                Current_Line_Tool.RunParams.ExpectedLineSegment.EndX = Current_Line_Tool.RunParams.ExpectedLineSegment.StartX;
                Current_Line_Tool.RunParams.ExpectedLineSegment.EndY = Current_Line_Tool.RunParams.ExpectedLineSegment.StartY + (Current_Line_Tool.RunParams.NumCalipers * Current_Line_Tool.RunParams.CaliperProjectionLength);
            }
            Line_Point_Display();
            if (!E_R_Use)
                Line_Drawing(Current_Line_Tool);
            else
                Run_Line_Drawing(Current_Line_Tool);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Keys Key = keyData & ~(Keys.Shift | Keys.Control | Keys.Alt);
            switch (Key)
            {
                case Keys.Right:
                    if ((keyData & Keys.Shift) != 0)
                        Position_Move(10, "W", Current_Tool_Type);
                    else
                        Position_Move(1, "W", Current_Tool_Type);
                    break;
                case Keys.Left:
                    if ((keyData & Keys.Shift) != 0)
                        Position_Move(-10, "W", Current_Tool_Type);
                    else
                        Position_Move(-1, "W", Current_Tool_Type);
                    break;
                case Keys.Up:
                    if ((keyData & Keys.Shift) != 0)
                        Position_Move(-10, "H", Current_Tool_Type);
                    else
                        Position_Move(-1, "H", Current_Tool_Type);
                    break;
                case Keys.Down:
                    if ((keyData & Keys.Shift) != 0)
                        Position_Move(10, "H", Current_Tool_Type);
                    else
                        Position_Move(1, "H", Current_Tool_Type);
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Position_Move(int Pixel, string WH, string Tool_Type)
        {
            switch (Tool_Type)
            {
                case "M":
                    if (WH == "W")
                        Mark_Origin.OriginX += Pixel;
                    else
                        Mark_Origin.OriginY += Pixel;
                    break;
                case "C":
                    if (WH == "W")
                        Current_Circle_Tool.RunParams.ExpectedCircularArc.CenterX += Pixel;
                    else
                        Current_Circle_Tool.RunParams.ExpectedCircularArc.CenterY += Pixel;
                    Circle_Point_Display();
                    if (!E_R_Use)
                        Circle_Drawing(Current_Circle_Tool);
                    else
                        Run_Circle_Drawing(Current_Circle_Tool);
                    break;
                case "P":
                    if (WH == "W")
                        Pattern_Origin.OriginX += Pixel;
                    else
                        Pattern_Origin.OriginY += Pixel;
                    break;
                case "W":
                case "H":
                    if (WH == "W")
                    {
                        Current_Line_Tool.RunParams.ExpectedLineSegment.StartX += Pixel;
                        Current_Line_Tool.RunParams.ExpectedLineSegment.EndX += Pixel;
                    }
                    else
                    {
                        Current_Line_Tool.RunParams.ExpectedLineSegment.StartY += Pixel;
                        Current_Line_Tool.RunParams.ExpectedLineSegment.EndY += Pixel;
                    }
                    Line_Point_Display();
                    if (!E_R_Use)
                        Line_Drawing(Current_Line_Tool);
                    else
                        Run_Line_Drawing(Current_Line_Tool);
                    break;
            }
        }
    }
}
