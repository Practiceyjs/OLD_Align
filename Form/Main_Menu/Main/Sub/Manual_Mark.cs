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

namespace T_Align
{
    public partial class Manual_Mark : Form
    {
        public double Point_X = 0;
        public double Point_Y = 0;
        public CogLine X_Line = new CogLine();
        public CogLine Y_Line = new CogLine();

        bool Click = false;

        int Move_Pitch = 10;
        ClassMark Mark;

        public Manual_Mark(ICogImage _Image, ClassMark _Mark)
        {
            Mark = _Mark;
            InitializeComponent();
            cogDisplay1.Image = _Image;
            for(int LoopCount=0;LoopCount<5;LoopCount++)
            {
                try
                {
                    ICogImage Image = Mark.Multi_Mark[LoopCount].Pattern.TrainImage;
                    if (Image != null)
                    {
                        cogDisplay2.Image = Image;
                        CogLine GX = new CogLine();
                        CogLine GY = new CogLine();
                        CogTransform2DLinear _Point = new CogTransform2DLinear();
                        _Point.TranslationX = Mark.Multi_Mark[LoopCount].Pattern.Origin.TranslationX;
                        _Point.TranslationY = Mark.Multi_Mark[LoopCount].Pattern.Origin.TranslationY;
                        GX.LineWidthInScreenPixels = 3;
                        GY.LineWidthInScreenPixels = 3;
                        GX.SetFromStartXYEndXY(_Point.TranslationX - 100, _Point.TranslationY, _Point.TranslationX + 100, _Point.TranslationY);
                        GY.SetFromStartXYEndXY(_Point.TranslationX, _Point.TranslationY - 100, _Point.TranslationX, _Point.TranslationY + 100);
                        cogDisplay2.InteractiveGraphics.Add(GX, "P", false);
                        cogDisplay2.InteractiveGraphics.Add(GY, "P", false);
                        break;
                    }
                }
                catch { }
            }
        }

        private void Pixel_Click(object sender, EventArgs e)
        {
            RadioButton Sel_Button = (RadioButton)sender;
            Move_Pitch = int.Parse(Sel_Button.Name.Substring(12, Sel_Button.Name.Length - 12));
        }

        private void Button_OK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Button_NG_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Move_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Name.Substring(7, button.Name.Length - 7))
            {
                case "U":
                    Point_Y -= 1;
                    break;
                case "UU":
                    Point_Y -= Move_Pitch;
                    break;
                case "D":
                    Point_Y += 1;
                    break;
                case "DD":
                    Point_Y += Move_Pitch;
                    break;
                case "R":
                    Point_X += 1;
                    break;
                case "RR":
                    Point_X += Move_Pitch;
                    break;
                case "L":
                    Point_X -= 1;
                    break;
                case "LL":
                    Point_X -= Move_Pitch;
                    break;
            }
            Line_Draw();
        }

        private void Line_Draw()
        {
            try
            {
                X_Line = new CogLine();
                Y_Line = new CogLine();
                X_Line.Color = CogColorConstants.Red;
                Y_Line.Color = CogColorConstants.Red;
                X_Line.SetFromStartXYEndXY(0, Point_Y, Point_X, Point_Y);
                Y_Line.SetFromStartXYEndXY(Point_X, 0, Point_X, Point_Y);
                cogDisplay1.InteractiveGraphics.Clear();
                cogDisplay1.InteractiveGraphics.Add(X_Line, "L", false);
                cogDisplay1.InteractiveGraphics.Add(Y_Line, "L", false);
            }
            catch { }
        }

        private void cogDisplay1_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left && cogDisplay1.MouseMode == Cognex.VisionPro.Display.CogDisplayMouseModeConstants.Pointer)
            {
                Click = true;
                Pixel_Data_Read(cogDisplay1, e);
            }
        }

        private void cogDisplay1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Click && cogDisplay1.MouseMode == Cognex.VisionPro.Display.CogDisplayMouseModeConstants.Pointer)
            {
                Pixel_Data_Read(cogDisplay1, e);
            }
        }

        private void cogDisplay1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && cogDisplay1.MouseMode == Cognex.VisionPro.Display.CogDisplayMouseModeConstants.Pointer)
            {
                Pixel_Data_Read(cogDisplay1, e);
                Click = false;
            }
        }

        private void Pixel_Data_Read(CogDisplay _Current_DIsplay, MouseEventArgs e)
        {
            if (_Current_DIsplay.Image != null)
            {
                CogTransform2DLinear Pointer;
                Pointer = _Current_DIsplay.GetTransform("#", "*") as CogTransform2DLinear;
                Pointer.MapPoint(e.X, e.Y, out Point_X, out Point_Y);
                if (Point_X < 0)
                    Point_X = 0;
                if (Point_X > _Current_DIsplay.Image.Width)
                    Point_X = _Current_DIsplay.Image.Width - 1;
                if (Point_Y < 0)
                    Point_Y = 0;
                if (Point_Y > _Current_DIsplay.Image.Height)
                    Point_Y = _Current_DIsplay.Image.Height - 1;
                Line_Draw();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Keys Key = keyData & ~(Keys.Shift | Keys.Control | Keys.Alt);
            switch (Key)
            {
                case Keys.Right:
                    if ((keyData & Keys.Shift) != 0)
                        Position_Move(10, "W");
                    else
                        Position_Move(1, "W");
                    break;
                case Keys.Left:
                    if ((keyData & Keys.Shift) != 0)
                        Position_Move(-10, "W");
                    else
                        Position_Move(-1, "W");
                    break;
                case Keys.Up:
                    if ((keyData & Keys.Shift) != 0)
                        Position_Move(-10, "H");
                    else
                        Position_Move(-1, "H");
                    break;
                case Keys.Down:
                    if ((keyData & Keys.Shift) != 0)
                        Position_Move(10, "H");
                    else
                        Position_Move(1, "H");
                    break;
                case Keys.Enter:
                    if ((keyData & Keys.Shift) != 0)
                    {
                        DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Position_Move(int Pixel, string WH)
        {
            if (WH == "W")
                Point_X += Pixel;
            else
                Point_Y += Pixel;
            if (Point_X < 0)
                Point_X = 0;
            if (Point_X > cogDisplay1.Image.Width)
                Point_X = cogDisplay1.Image.Width - 1;
            if (Point_Y < 0)
                Point_Y = 0;
            if (Point_Y > cogDisplay1.Image.Height)
                Point_Y = cogDisplay1.Image.Height - 1;
            Line_Draw();
        }
    }
}
