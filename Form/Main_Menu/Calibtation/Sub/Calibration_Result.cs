using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Cognex.VisionPro;
using Cognex.VisionPro.Display;

namespace T_Align
{
    public partial class Calibration_Result : Form
    {
        Enum.Mark Current_Mark = Enum.Mark.MARK1;
        Button[] Mark_Button;
        Label[] Resolution_X;
        Label[] Resolution_Y;
        Label[] Rotate_X;
        Label[] Rotate_Y;
        Label[] Length;
        ClassType Type;
        public Calibration_Result(ClassType _Type)
        {
            Type = _Type;
            InitializeComponent();
            Mark_Button = new Button[] { Button_Mark1, Button_Mark2, Button_Mark3, Button_Mark4 };
            Resolution_X = new Label[] { label21, label22, label23, label24 };
            Resolution_Y = new Label[] { label28, label27, label26, label25 };
            Rotate_X = new Label[] { label32, label31, label30, label29 };
            Rotate_Y = new Label[] { label36, label35, label34, label33 };
            Length = new Label[] { label40, label39, label38, label37 };
            Mark_Select("1");
            Mark_Sel();
            for (int LoopCount=0;LoopCount<4;LoopCount++)
            {
                Resolution_X[LoopCount].Text = Type.Mark[LoopCount].Resolution_X.ToString("0.000");
                Resolution_Y[LoopCount].Text = Type.Mark[LoopCount].Resolution_Y.ToString("0.000");
                Rotate_X[LoopCount].Text = Type.Mark[LoopCount].Rotate_Center_X.ToString("0.000");
                Rotate_Y[LoopCount].Text = Type.Mark[LoopCount].Rotate_Center_Y.ToString("0.000");
                Length[LoopCount].Text = Type.L_Check_Spec[LoopCount].ToString("0.000");
            }
        }

        private void Point_Graphics()
        {
            Point_Display.InteractiveGraphics.Clear();
            double[] Data_X = new double[9];
            double[] Data_Y = new double[9];
            double Max_X, Min_X, Max_Y, Min_Y;
            int Width, Height;
            double Offset_X, Offset_Y;
            Array.Copy(Type.Mark[(int)Current_Mark - 1].Calibration_Point_X, Data_X, 9);
            Array.Copy(Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y, Data_Y, 9);
            Min_X = Data_X.Min();
            Min_Y = Data_Y.Min();
            Max_X = Data_X.Max();
            Max_Y = Data_Y.Max();
            Width = (int)((Max_X - Min_X) * 1.5);
            Height = (int)((Max_Y - Min_Y) * 1.5);
            Offset_X = (Max_X - Min_X) * 0.25;
            Offset_Y = (Max_Y - Min_Y) * 0.25;
            try
            {
                Bitmap bitmap = new Bitmap(Width, Height);
                Point_Display.Image = new CogImage8Grey(bitmap);
                double[] Source = new double[4] { Offset_X, Offset_Y, Min_X, Min_Y };
                int Num1, Num2;
                Num1 = 0; Num2 = 1;
                Add_Graphics(Point_Display, Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Num1], Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Num1], Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Num2], Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Num2], Source);
                Num1 = 1; Num2 = 2;
                Add_Graphics(Point_Display, Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Num1], Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Num1], Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Num2], Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Num2], Source);
                Num1 = 3; Num2 = 4;
                Add_Graphics(Point_Display, Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Num1], Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Num1], Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Num2], Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Num2], Source);
                Num1 = 4; Num2 = 5;
                Add_Graphics(Point_Display, Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Num1], Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Num1], Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Num2], Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Num2], Source);
                Num1 = 6; Num2 = 7;
                Add_Graphics(Point_Display, Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Num1], Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Num1], Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Num2], Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Num2], Source);
                Num1 = 7; Num2 = 8;
                Add_Graphics(Point_Display, Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Num1], Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Num1], Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Num2], Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Num2], Source);
                Num1 = 0; Num2 = 3;
                Add_Graphics(Point_Display, Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Num1], Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Num1], Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Num2], Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Num2], Source);
                Num1 = 3; Num2 = 6;
                Add_Graphics(Point_Display, Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Num1], Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Num1], Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Num2], Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Num2], Source);
                Num1 = 1; Num2 = 4;
                Add_Graphics(Point_Display, Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Num1], Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Num1], Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Num2], Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Num2], Source);
                Num1 = 4; Num2 = 7;
                Add_Graphics(Point_Display, Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Num1], Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Num1], Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Num2], Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Num2], Source);
                Num1 = 2; Num2 = 5;
                Add_Graphics(Point_Display, Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Num1], Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Num1], Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Num2], Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Num2], Source);
                Num1 = 5; Num2 = 8;
                Add_Graphics(Point_Display, Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Num1], Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Num1], Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Num2], Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Num2], Source);
                for (int LoopCount = 0; LoopCount < Data_X.Length; LoopCount++)
                {
                    Dot_Graphics(Point_Display, Data_X[LoopCount], Data_Y[LoopCount], Source);
                }
            }
            catch
            {

            }
            
        }

        private void Rotate_Graphics()
        {
            Rotate_Display.InteractiveGraphics.Clear();
            double[] Data_X = new double[7];
            double[] Data_Y = new double[7];
            double Max_X, Min_X, Max_Y, Min_Y;
            int Width, Height;
            double Offset_X, Offset_Y;
            Array.Copy(Type.Mark[(int)Current_Mark - 1].Calibration_Point_X, 9, Data_X, 0, 7);
            Array.Copy(Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y, 9, Data_Y, 0, 7);
            Min_X = Data_X.Min();
            Min_Y = Data_Y.Min();
            Max_X = Data_X.Max();
            Max_Y = Data_Y.Max();
            Width = (int)((Max_X - Min_X) * 1.5);
            Height = (int)((Max_Y - Min_Y) * 1.5);
            Offset_X = (Max_X - Min_X) * 0.25;
            Offset_Y = (Max_Y - Min_Y) * 0.25;
            try
            {
                Bitmap bitmap = new Bitmap(Width, Height);
                Rotate_Display.Image = new CogImage8Grey(bitmap);
                double[] Source = new double[4] { Offset_X, Offset_Y, Min_X, Min_Y };

                for (int LoopCount = 0; LoopCount < Data_X.Length - 1; LoopCount++)
                {
                    Add_Graphics(Rotate_Display, Data_X[LoopCount], Data_Y[LoopCount], Data_X[LoopCount + 1], Data_Y[LoopCount + 1], Source);
                }
                for (int LoopCount = 0; LoopCount < Data_X.Length; LoopCount++)
                {
                    Dot_Graphics(Rotate_Display, Data_X[LoopCount], Data_Y[LoopCount], Source);
                }
            }
            catch
            {

            }
        }

        private void Dot_Graphics(CogDisplay display, double P1_X, double P1_Y,double[] Source)
        {
            CogPointMarker cogPointMarker = new CogPointMarker();
            cogPointMarker.GraphicDOFEnable = CogPointMarkerDOFConstants.Position;
            cogPointMarker.GraphicDOFEnableBase = CogGraphicDOFConstants.Position;
            cogPointMarker.GraphicType = CogPointMarkerGraphicTypeConstants.Circle;
            cogPointMarker.Color = CogColorConstants.Red;
            double Start_X = P1_X - Source[2] + Source[0];
            double Start_Y = P1_Y - Source[3] + Source[1];
            cogPointMarker.X = Start_X;
            cogPointMarker.Y = Start_Y;
            display.InteractiveGraphics.Add(cogPointMarker, "D", false);
        }

        private void Add_Graphics(CogDisplay display, double P1_X, double P1_Y, double P2_X, double P2_Y,double[] Source)
        {
            CogLineSegment line = new CogLineSegment();
            double Start_X = P1_X - Source[2] + Source[0];
            double Start_Y = P1_Y - Source[3] + Source[1];
            double End_X = P2_X - Source[2] + Source[0];
            double End_Y = P2_Y - Source[3] + Source[1];
            line.SetStartEnd(Start_X, Start_Y, End_X, End_Y);
            display.InteractiveGraphics.Add(line, "L", false);
        }

        private void Mark_Sel ()
        {
            Point_dataGridView.Rows.Clear();
            for (int Loopcount = 0; Loopcount < 9; Loopcount++)
            {
                object[] Row_Data = { Loopcount, Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Loopcount].ToString("0.000"), Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Loopcount].ToString("0.000") };
                Point_dataGridView.Rows.Add(Row_Data);
            }
            object[] Null_Data = {  };
            Point_dataGridView.Rows.Add(Null_Data);
            for (int Loopcount = 9; Loopcount < 16; Loopcount++)
            {
                object[] Row_Data = { Loopcount, Type.Mark[(int)Current_Mark - 1].Calibration_Point_X[Loopcount].ToString("0.000"), Type.Mark[(int)Current_Mark - 1].Calibration_Point_Y[Loopcount].ToString("0.000") };
                Point_dataGridView.Rows.Add(Row_Data);
            }
            Point_Graphics();
            Rotate_Graphics();
        }

        private void Button_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Mark_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Mark_Select(button.Text.Substring(button.Text.Length - 1, 1));
            Mark_Sel();
        }

        private void Mark_Select(string _Mark)
        {
            Current_Mark = (Enum.Mark)Convert.ToInt32(_Mark);
            for (int LoopCount = 0; LoopCount < 4; LoopCount++)
            {
                Mark_Button[LoopCount].Image = global::T_Align.Properties.Resources.Button3;
                Mark_Button[LoopCount].ForeColor = Color.White;
            }
            Mark_Button[(int)Current_Mark - 1].Image = global::T_Align.Properties.Resources.Button3_Select;
            Mark_Button[(int)Current_Mark - 1].ForeColor = Color.Cyan;
        }
    }
}
