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
using Cognex.VisionPro.Display;
using System.Threading;

namespace T_Align
{
    public partial class Calibration : UserControl
    {
        bool Center_Line_Use = true;
        bool Custom_Line_Use = true;
        bool Mesure_Use = true;
        bool Live_Use = false;
        bool Copy_Use = false;

        CogLineSegment CenterLine_X1 = new CogLineSegment();
        CogLineSegment CenterLine_Y1 = new CogLineSegment();
        CogLineSegment CenterLine_X2 = new CogLineSegment();
        CogLineSegment CenterLine_Y2 = new CogLineSegment();

        CogLine Custom_Line_X1 = new CogLine();
        CogLine Custom_Line_Y1 = new CogLine();
        CogLine Custom_Line_X2 = new CogLine();
        CogLine Custom_Line_Y2 = new CogLine();

        CogLine Masure_Line_X1 = new CogLine();
        CogLine Masure_Line_Y1 = new CogLine();
        CogLineSegment Masure_Line_X2 = new CogLineSegment();
        CogLineSegment Masure_Line_Y2 = new CogLineSegment();


        double Dis_Mesure_Start_X = 0;
        double Dis_Mesure_Start_Y = 0;
        double Dis_Mesure_End_X = 0;
        double Dis_Mesure_End_Y = 0;

        double Mouse_X = 0;
        double Mouse_Y = 0;
        bool Display_Click = false;
        bool IsStatic_Graphics = false;

        Enum.Type Current_Type = Enum.Type.Target;
        Enum.Unit Current_Unit = Enum.Unit.Unit1;
        Enum.Mark Current_Mark = Enum.Mark.MARK1;
        Enum.Mark_Type Current_Mark_Type = Enum.Mark_Type.MARK;
        int Current_Multi_Mark_Index = 5;
        Button[] Unit_Button;
        Button[] Mark_Button;
        public CogDisplay[] Cal_Displays;
        Thread Live_Thread;

        double X = 0;
        double Y = 0;

        double S = 0;
        double A = 0;
        double R = 0;
        double T = 0;

        public Calibration()
        {
            InitializeComponent();
            Unit_Button = new Button[] { Button_Unit1, Button_Unit2, Button_Unit3, Button_Unit4, Button_Unit5, Button_Unit6, Button_Unit7, Button_Unit8 };
            Mark_Button = new Button[] { Button_Mark1, Button_Mark2, Button_Mark3, Button_Mark4 };
            Cal_Displays = new CogDisplay[] { Cal_Display1, Cal_Display2 };
            for (int LoopCount = 0; LoopCount < 8; LoopCount++)
                Unit_Button[LoopCount].Text = COMMON.U_Setting[LoopCount + 1].Unit_Name;
            CenterLine_X1.Color = CogColorConstants.Green;
            CenterLine_Y1.Color = CogColorConstants.Green;
            CenterLine_X2.Color = CogColorConstants.Green;
            CenterLine_Y2.Color = CogColorConstants.Green;
            Custom_Line_X1.Color = CogColorConstants.Red;
            Custom_Line_Y1.Color = CogColorConstants.Red;
            Custom_Line_X2.Color = CogColorConstants.Red;
            Custom_Line_Y2.Color = CogColorConstants.Red;
            Masure_Line_X1.Color = CogColorConstants.Blue;
            Masure_Line_Y1.Color = CogColorConstants.Blue;
            Masure_Line_X2.Color = CogColorConstants.Blue;
            Masure_Line_Y2.Color = CogColorConstants.Blue;
            Live_Thread = new Thread(Live);
        }

        public void Loading_OK()
        {
            Live_Thread.Start();
            Type_Select("T");
            Unit_Select("1");
            Mark_Select("1");
            for (int LoopCount = 1; LoopCount < 9; LoopCount++)
            {
                if (COMMON.Frame.Unit[LoopCount] == null)
                    Unit_Button[LoopCount - 1].Visible = false;
            }
            Live_Thread.Suspend();
        }

        private void Live()
        {
            while (true)
            {
                if (!COMMON.Program_Run)
                    break;
                if (Live_Use)
                {
                    Cal_Display1.Image = COMMON.Grab_Image[COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Cam[0]];
                    Cal_Display2.Image = COMMON.Grab_Image[COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Cam[1]];
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
                    Button_Live.Image = global::T_Align.Properties.Resources.Live;
                    Live_Use = false;
                    Live_Thread.Suspend();
                }
                catch { }
            }
        }

        private void Calibration_Load(object sender, EventArgs e)
        {
            //Cal_Display1.Image = new Cognex.VisionPro.CogImage8Grey((Bitmap)Bitmap.FromFile("C:\\Users\\Lin\\Desktop\\Image\\210830.bmp"));
            //Cal_Display2.Image = new Cognex.VisionPro.CogImage8Grey((Bitmap)Bitmap.FromFile("C:\\Users\\Lin\\Desktop\\Image\\210830.bmp"));
        }

        private void cogDisplay1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Cal_Display1.Image != null)
            {
                Pixel_Data_Read(Cal_Display1, e);

                if (Display_Click && !Custom_Line_Use)
                {
                    Custom_Line_Draw(Cal_Display1, Cal_Display2);
                }

                if (Display_Click && !Mesure_Use)
                {
                    Dis_Mesure_End_X = Mouse_X;
                    Dis_Mesure_End_Y = Mouse_Y;
                    Mesuer_Line_Draw(Cal_Display1,Textbox_LX,Textbox_LY);
                }
            }
        }

        private void Unit_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Current_Unit = (Enum.Unit)Convert.ToInt32(button.Name.Substring(button.Name.Length - 1, 1));
            Unit_Select(button.Text.Substring(button.Text.Length - 1, 1));
        }

        private void Type_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Type_Select(button.Text.Substring(0, 1));
            Nud_Cal_Pitch_X0.Value = (decimal)COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Calibration_Pitch[0];
            Nud_Cal_Pitch_Y1.Value = (decimal)COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Calibration_Pitch[1];
            Nud_Cal_Pitch_T2.Value = (decimal)COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Calibration_Pitch[2];
        }

        private void Button_Grab_Click(object sender, EventArgs e)
        {
            Cal_Display1.Image = COMMON.Grab_Image[COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Cam[0]];
            Cal_Display2.Image = COMMON.Grab_Image[COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Cam[1]];
        }

        private void Button_Live_Click(object sender, EventArgs e)
        {
            Live_Use = !Live_Use;
            if (Live_Use)
            {
                Button_Live.Image = global::T_Align.Properties.Resources.Live_Select;
                if (Live_Thread.ThreadState == ThreadState.Suspended)
                {
                    Live_Thread = null;
                    Live_Thread = new Thread(Live);
                    Live_Thread.Start();
                }
            }
            else
            {
                Button_Live.Image = global::T_Align.Properties.Resources.Live;
                if (Live_Thread.ThreadState != ThreadState.Suspended)
                    Live_Thread.Suspend();
            }
        }

        private void Button_Clear_Click(object sender, EventArgs e)
        {
            Cal_Display1.InteractiveGraphics.Clear();
            Cal_Display2.InteractiveGraphics.Clear();
            Cal_Display1.StaticGraphics.Clear();
            Cal_Display2.StaticGraphics.Clear();
        }

        private void Button_Cal_Result_Click(object sender, EventArgs e)
        {
            Calibration_Result calibration_Result = new Calibration_Result(COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type]);
            calibration_Result.ShowDialog();
            calibration_Result.Dispose();
        }

        private void Mark_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Mark_Select(button.Text.Substring(button.Text.Length - 1, 1));
        }

        private void Find_Mark()
        {
            Current_Mark_Type = COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Mark_Type_Load();
            X = Y = S = R = A = 0;
            CogDisplay Current_Display;

            if(Current_Mark == Enum.Mark.MARK1 || Current_Mark == Enum.Mark.MARK3)
                Current_Display = Cal_Display1;
            else
                Current_Display = Cal_Display2;

            Current_Display.InteractiveGraphics.Clear();
            Current_Display.StaticGraphics.Clear();

            if (Current_Mark_Type == Enum.Mark_Type.MARK)
            {
                COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Mark_Find(Current_Display, (int)Current_Multi_Mark_Index, out X, out Y, out S);
            }
            else if (Current_Mark_Type == Enum.Mark_Type.LINE)
            {
                COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Line_Find(Current_Display, ref X, ref Y, out A);
            }
            else if (Current_Mark_Type == Enum.Mark_Type.CIRCLE)
            {
                COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Circle_Find(Current_Display, ref X, ref Y, out R);
            }
            else if (Current_Mark_Type == Enum.Mark_Type.PATTERN)
            {
                COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Pattern_Find(Current_Display, out X, out Y, out S);
            }
            else if (Current_Mark_Type == Enum.Mark_Type.MARK_TO_CIRCLE)
            {
                COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Mark_Find(Current_Display, (int)Current_Multi_Mark_Index, out X, out Y, out S);
                Current_Display.InteractiveGraphics.Clear();
                Current_Display.StaticGraphics.Clear();
                COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Circle_Find(Current_Display, ref X, ref Y, out R);
            }
            else if (Current_Mark_Type == Enum.Mark_Type.MARK_TO_LINE)
            {
                COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Mark_Find(Current_Display, (int)Current_Multi_Mark_Index, out X, out Y, out S);
                Current_Display.InteractiveGraphics.Clear();
                Current_Display.StaticGraphics.Clear();
                COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Line_Find(Current_Display, ref X, ref Y, out A);
            }
            if (COMMON.CTQ_Use)
                Log.CTQ_Triger_Write(Current_Unit, Current_Mark, new PointF((float)X, (float)Y));
        }

        private void Button_Find_Click(object sender, EventArgs e)
        {
            Find_Mark();
        }

        private void Button_Copy_Click(object sender, EventArgs e)
        {
            Copy_Use = !Copy_Use;
            if (Copy_Use)
            {
                Button_Copy.Image = global::T_Align.Properties.Resources.Button4_Select;
                Button_Copy.ForeColor = Color.Cyan;
            }
            else
            {
                Button_Copy.Image = global::T_Align.Properties.Resources.Button4;
                Button_Copy.ForeColor = Color.White;
            }
        }

        private void Button_Mesure_Click(object sender, EventArgs e)
        {
            if (Cal_Display1.Image != null || Cal_Display2.Image != null)
            {
                if (Mesure_Use)
                {
                    Mesure_Line_On();
                }
                else
                {
                    Mesure_Line_Off();
                }
            }
        }

        private void Button_Center_Line_Click(object sender, EventArgs e)
        {
            if(Cal_Display1.Image != null || Cal_Display2.Image != null)
            {
                if (Center_Line_Use)
                {
                    Center_Line_On();
                }
                else
                {
                    Center_Line_Off();
                }
            }
        }

        private void Button_Custom_Line_Click(object sender, EventArgs e)
        {
            if (Cal_Display1.Image != null || Cal_Display2.Image != null)
            {
                if (Custom_Line_Use)
                {
                    Custom_Line_On();
                }
                else
                {
                    Custom_Line_Off();
                }
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
            try
            {
                Mark_Display.Image = COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Multi_Mark[(int)Current_Multi_Mark_Index].Pattern.GetTrainedPatternImage();
            }
            catch { Mark_Display.Image = null; }
        }

        private void Unit_Select(string _Unit)
        {
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
            for (int LoopCount = 0; LoopCount < 8; LoopCount++)
            {
                Unit_Button[LoopCount].Image = global::T_Align.Properties.Resources.Unit;
                Unit_Button[LoopCount].ForeColor = Color.White;
            }
            Unit_Button[(int)Current_Unit - 1].Image = global::T_Align.Properties.Resources.Unit_Select;
            Unit_Button[(int)Current_Unit - 1].ForeColor = Color.Cyan;
            Nud_Cal_Pitch_X0.Value = (decimal)COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Calibration_Pitch[0];
            Nud_Cal_Pitch_Y1.Value = (decimal)COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Calibration_Pitch[1];
            Nud_Cal_Pitch_T2.Value = (decimal)COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Calibration_Pitch[2];
            try
            {
                Mark_Display.Image = COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Multi_Mark[(int)Current_Multi_Mark_Index].Pattern.GetTrainedPatternImage();
            }
            catch { Mark_Display.Image = null; }
        }

        private void Mark_Select(string _Mark)
        {
            Current_Mark = (Enum.Mark)(Convert.ToInt32(_Mark)-1);
            for (int LoopCount = 0; LoopCount < 4; LoopCount++)
            {
                Mark_Button[LoopCount].Image = global::T_Align.Properties.Resources.Button3;
                Mark_Button[LoopCount].ForeColor = Color.White;
            }
            Mark_Button[(int)Current_Mark].Image = global::T_Align.Properties.Resources.Button3_Select;
            Mark_Button[(int)Current_Mark].ForeColor = Color.Cyan;
            try
            {
                Mark_Display.Image = COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Multi_Mark[(int)Current_Multi_Mark_Index].Pattern.GetTrainedPatternImage();
            }
            catch { Mark_Display.Image = null; }
            
        }

        private void Pixel_Data_Read(CogDisplay _Current_DIsplay, MouseEventArgs e)
        {
            if (_Current_DIsplay.Image != null)
            {
                CogTransform2DLinear Pointer;
                Pointer = _Current_DIsplay.GetTransform("#", "*") as CogTransform2DLinear;
                Pointer.MapPoint(e.X, e.Y, out Mouse_X, out Mouse_Y);
                if (Mouse_X < 0)
                    Mouse_X = 0;
                if (Mouse_X > _Current_DIsplay.Image.Width)
                    Mouse_X = _Current_DIsplay.Image.Width - 1;
                if (Mouse_Y < 0)
                    Mouse_Y = 0;
                if (Mouse_Y > _Current_DIsplay.Image.Height)
                    Mouse_Y = _Current_DIsplay.Image.Height - 1;
                if (Textbox_Mouse_X.InvokeRequired)
                {
                    Textbox_Mouse_X.Invoke((MethodInvoker)delegate
                    {
                        Textbox_Mouse_X.Text = Mouse_X.ToString("0.0");
                    });
                }
                else
                {
                    Textbox_Mouse_X.Text = Mouse_X.ToString("0.0");
                }
                if (Textbox_Mouse_Y.InvokeRequired)
                {
                    Textbox_Mouse_Y.Invoke((MethodInvoker)delegate
                    {
                        Textbox_Mouse_Y.Text = Mouse_Y.ToString("0.0");
                    });
                }
                else
                {
                    Textbox_Mouse_Y.Text = Mouse_Y.ToString("0.0");
                }
                if (Textbox_Mouse_Bright.InvokeRequired)
                {
                    Textbox_Mouse_Bright.Invoke((MethodInvoker)delegate
                    {
                        Textbox_Mouse_Bright.Text = ((CogImage8Grey)_Current_DIsplay.Image).GetPixel((int)Mouse_X, (int)Mouse_Y).ToString();
                    });
                }
                else
                {
                    Textbox_Mouse_Bright.Text = ((CogImage8Grey)_Current_DIsplay.Image).GetPixel((int)Mouse_X, (int)Mouse_Y).ToString();
                }
            }

        }

        private void Cal_Display2_MouseMove(object sender, MouseEventArgs e)
        {
            if (Cal_Display1.Image != null)
            {
                Pixel_Data_Read(Cal_Display2, e);

                if (Display_Click && !Custom_Line_Use)
                {
                    Custom_Line_Draw(Cal_Display2, Cal_Display1);
                }

                if (Display_Click && !Mesure_Use)
                {
                    Dis_Mesure_End_X = Mouse_X;
                    Dis_Mesure_End_Y = Mouse_Y;
                    Mesuer_Line_Draw(Cal_Display2, Textbox_RX, Textbox_RY);
                }
            }
        }

        private void Center_Line_On()
        {
            Center_Line_Use = false;
            Button_Center_Line.Image = global::T_Align.Properties.Resources.Button1_Select;
            Button_Center_Line.ForeColor = Color.Cyan;
            if (Cal_Display1.Image != null)
            {
                CenterLine_X1.SetStartEnd(0, Cal_Display1.Image.Height / 2, Cal_Display1.Image.Width, Cal_Display1.Image.Height / 2);
                CenterLine_Y1.SetStartEnd(Cal_Display1.Image.Width / 2, 0, Cal_Display1.Image.Width / 2, Cal_Display1.Image.Height);
                Cal_Display1.StaticGraphics.Add(CenterLine_X1, "C");
                Cal_Display1.StaticGraphics.Add(CenterLine_Y1, "C");
            }
            if (Cal_Display2.Image != null)
            {
                CenterLine_X2.SetStartEnd(0, Cal_Display2.Image.Height / 2, Cal_Display2.Image.Width, Cal_Display2.Image.Height / 2);
                CenterLine_Y2.SetStartEnd(Cal_Display2.Image.Width / 2, 0, Cal_Display2.Image.Width / 2, Cal_Display2.Image.Height);
                Cal_Display2.StaticGraphics.Add(CenterLine_X2, "C");
                Cal_Display2.StaticGraphics.Add(CenterLine_Y2, "C");
            }
        }

        private void Center_Line_Off()
        {
            Center_Line_Use = true;
            Button_Center_Line.Image = global::T_Align.Properties.Resources.Button1;
            Button_Center_Line.ForeColor = Color.White;
            if (Cal_Display1.Image != null)
            {
                try
                {
                    Cal_Display1.StaticGraphics.Remove("C");
                }
                catch { }
            }
            if (Cal_Display2.Image != null)
            {
                try
                {
                    Cal_Display2.StaticGraphics.Remove("C");
                }
                catch { }
            }
        }

        private void Custom_Line_On()
        {
            Custom_Line_Use = false;
            Button_Custom_Line.Image = global::T_Align.Properties.Resources.Button1_Select;
            Button_Custom_Line.ForeColor = Color.Cyan;
        }

        private void Custom_Line_Off()
        {
            Custom_Line_Use = true;
            Button_Custom_Line.Image = global::T_Align.Properties.Resources.Button1;
            Button_Custom_Line.ForeColor = Color.White;
            if (Cal_Display1.Image != null)
            {
                try
                {
                    Cal_Display1.StaticGraphics.Remove("Custom");
                }
                catch { }
            }
            if (Cal_Display2.Image != null)
            {
                try
                {
                    Cal_Display2.StaticGraphics.Remove("Custom");
                }
                catch { }
            }
        }

        private void Mesure_Line_On()
        {
            Mesure_Use = false;
            Button_Mesure.Image = global::T_Align.Properties.Resources.Button2_Select;
            Button_Mesure.ForeColor = Color.Cyan;
        }

        private void Mesure_Line_Off()
        {
            Mesure_Use = true;
            Button_Mesure.Image = global::T_Align.Properties.Resources.Button2;
            Button_Mesure.ForeColor = Color.White;
            if (Cal_Display1.Image != null)
            {
                try
                {
                    Cal_Display1.StaticGraphics.Remove("Mesuer");
                    Cal_Display1.StaticGraphics.Remove("Mesuer_Standard");
                }
                catch { }
            }
            if (Cal_Display2.Image != null)
            {
                try
                {
                    Cal_Display2.StaticGraphics.Remove("Mesuer");
                    Cal_Display2.StaticGraphics.Remove("Mesuer_Standard");
                }
                catch { }
            }
        }

        private void Cal_Display1_MouseDown(object sender, MouseEventArgs e)
        {
            if (Cal_Display1.Image != null)
            {
                if (e.Button.Equals(MouseButtons.Left) && (!Custom_Line_Use))
                {
                    Display_Click = true;
                }
                if (e.Button.Equals(MouseButtons.Left) && (!Mesure_Use))
                {
                    Display_Click = true;
                    IsStatic_Graphics = false;
                    Dis_Mesure_Start_X = Mouse_X;
                    Dis_Mesure_Start_Y = Mouse_Y;
                    Dis_Mesure_End_X = Mouse_X;
                    Dis_Mesure_End_Y = Mouse_Y;
                    try
                    {
                        Cal_Display1.StaticGraphics.Remove("Mesuer_Standard");
                    }
                    catch { }
                    Masure_Line_X1.SetFromStartXYEndXY(0, Mouse_Y, Mouse_X, Mouse_Y);
                    Masure_Line_Y1.SetFromStartXYEndXY(Mouse_X, 0, Mouse_X, Mouse_Y);
                    Cal_Display1.StaticGraphics.Add(Masure_Line_X1, "Mesuer_Standard");
                    Cal_Display1.StaticGraphics.Add(Masure_Line_Y1, "Mesuer_Standard");
                    Mesuer_Line_Draw(Cal_Display1, Textbox_LX, Textbox_LY);
                }
            }
        }

        private void Cal_Display1_MouseUp(object sender, MouseEventArgs e)
        {
            if(Cal_Display1.Image != null)
            {
                if (e.Button.Equals(MouseButtons.Left) && (!Custom_Line_Use))
                    Display_Click = false;
                if (e.Button.Equals(MouseButtons.Left) && (!Mesure_Use))
                    Display_Click = false;
            }
        }

        private void Custom_Line_Draw(CogDisplay Display1, CogDisplay Display2)
        {
            try
            {
                Display1.StaticGraphics.Remove("Custom");
            }
            catch { }
            try
            {
                Display2.StaticGraphics.Remove("Custom");
            }
            catch { }
            try
            {
                            Custom_Line_X1.SetFromStartXYEndXY(0, Mouse_Y, Mouse_X, Mouse_Y);
            Custom_Line_Y1.SetFromStartXYEndXY(Mouse_X, 0, Mouse_X, Mouse_Y);
            Custom_Line_X2.SetFromStartXYEndXY(0, Mouse_Y, Mouse_X, Mouse_Y);
            Custom_Line_Y2.SetFromStartXYEndXY((Display1.Image.Width - Mouse_X), 0, (Display1.Image.Width - Mouse_X), Mouse_Y);
            Display1.StaticGraphics.Add(Custom_Line_X1, "Custom");
            Display1.StaticGraphics.Add(Custom_Line_Y1, "Custom");
            Display2.StaticGraphics.Add(Custom_Line_X2, "Custom");
            Display2.StaticGraphics.Add(Custom_Line_Y2, "Custom");
            }
            catch { }
        }

        private void Mesuer_Line_Draw(CogDisplay Display, TextBox X_Textbox, TextBox Y_Textbox)
        {
            try 
            {
                Display.StaticGraphics.Remove("Mesuer");
            }
            catch { }
            Masure_Line_X2.SetStartEnd(Dis_Mesure_Start_X, Dis_Mesure_End_Y, Dis_Mesure_End_X, Dis_Mesure_End_Y);
            Masure_Line_Y2.SetStartEnd(Dis_Mesure_End_X, Dis_Mesure_Start_Y, Dis_Mesure_End_X, Dis_Mesure_End_Y);
            Display.StaticGraphics.Add(Masure_Line_X2, "Mesuer");
            Display.StaticGraphics.Add(Masure_Line_Y2, "Mesuer");
            if (X_Textbox.InvokeRequired)
            {
                X_Textbox.Invoke((MethodInvoker)delegate
                {
                    X_Textbox.Text = (Math.Abs(Dis_Mesure_Start_X - Dis_Mesure_End_X) * COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Resolution_X).ToString("0.000");
                });
            }
            else
            {
                X_Textbox.Text = (Math.Abs(Dis_Mesure_Start_X - Dis_Mesure_End_X) * COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Resolution_X).ToString("0.000");
            }
            if (Y_Textbox.InvokeRequired)
            {
                Y_Textbox.Invoke((MethodInvoker)delegate
                {
                    Y_Textbox.Text = (Math.Abs(Dis_Mesure_Start_Y - Dis_Mesure_End_Y) * COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Resolution_Y).ToString("0.000");
                });
            }
            else
            {
                Y_Textbox.Text = (Math.Abs(Dis_Mesure_Start_Y - Dis_Mesure_End_Y) * COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Mark[(int)Current_Mark].Resolution_Y).ToString("0.000");
            }
        }

        private void Cal_Display2_MouseDown(object sender, MouseEventArgs e)
        {
            if (Cal_Display2.Image != null)
            {
                if (e.Button.Equals(MouseButtons.Left) && (!Custom_Line_Use))
                {
                    Display_Click = true;
                }
                if (e.Button.Equals(MouseButtons.Left) && (!Mesure_Use))
                {
                    Display_Click = true;
                    IsStatic_Graphics = false;
                    Dis_Mesure_Start_X = Mouse_X;
                    Dis_Mesure_Start_Y = Mouse_Y;
                    Dis_Mesure_End_X = Mouse_X;
                    Dis_Mesure_End_Y = Mouse_Y;
                    try
                    {
                        Cal_Display2.StaticGraphics.Remove("Mesuer_Standard");
                    }
                    catch { }
                    Masure_Line_X1.SetFromStartXYEndXY(0, Mouse_Y, Mouse_X, Mouse_Y);
                    Masure_Line_Y1.SetFromStartXYEndXY(Mouse_X, 0, Mouse_X, Mouse_Y);
                    Cal_Display2.StaticGraphics.Add(Masure_Line_X1, "Mesuer_Standard");
                    Cal_Display2.StaticGraphics.Add(Masure_Line_Y1, "Mesuer_Standard");
                    Mesuer_Line_Draw(Cal_Display2, Textbox_RX, Textbox_RY);
                }
            }
        }

        private void Cal_Display2_MouseUp(object sender, MouseEventArgs e)
        {
            if (Cal_Display2.Image != null)
            {
                if (e.Button.Equals(MouseButtons.Left) && (!Custom_Line_Use))
                    Display_Click = false;
                if (e.Button.Equals(MouseButtons.Left) && (!Mesure_Use))
                    Display_Click = false;
            }
        }

        private void Calibration_Pitch_Changed(object sender, EventArgs e)
        {
            NumericUpDown upDown = (NumericUpDown)sender;
            int Sel_Number = int.Parse(upDown.Name.Substring(upDown.Name.Length - 1, 1));
            COMMON.Frame.Unit[(int)Current_Unit].Type[(int)Current_Type].Calibration_Pitch[Sel_Number] = (double)upDown.Value;
        }

        private void Button_CTQ_Click(object sender, EventArgs e)
        {
            CTQ_MODE CTQ = new CTQ_MODE();
            CTQ.Triger += CTQ_Triger;
            CTQ.ShowDialog();
        }

        private void CTQ_Triger(CTQ_MODE Control)
        {
            Triger_panel.Visible = true;
            Thread Triger_Thread = new Thread(Triger);
            Triger_Thread.Start(Control);
        }

        private void Triger(object sender)
        {
            CTQ_MODE Count = (CTQ_MODE)sender;
            for (int LoopCount = 0; LoopCount < Count.Triger_Count; LoopCount++)
            {
                if (Current_Mark == Enum.Mark.MARK1 || Current_Mark == Enum.Mark.MARK3)
                    Cal_Display1.Image = COMMON.Grab_Image[COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Cam[0]];
                else
                    Cal_Display2.Image = COMMON.Grab_Image[COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Current_Type].Cam[1]];
                Find_Mark();
                if (Triger_Label.InvokeRequired)
                {
                    Triger_Label.Invoke((MethodInvoker)delegate
                    {
                        Triger_Label.Text = "TRIGER COUNT : " + LoopCount.ToString();
                    });
                }
                else
                {
                    Triger_Label.Text = "TRIGER COUNT : " + LoopCount.ToString();
                }
                Thread.Sleep(500);
            }
            if (Triger_panel.InvokeRequired)
            {
                Triger_panel.Invoke((MethodInvoker)delegate
                {
                    Triger_panel.Visible = false;
                });
            }
            else
            {
                Triger_panel.Visible = false;
            }
        }
    }
}
