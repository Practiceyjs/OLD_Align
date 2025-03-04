using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cognex.VisionPro.Display;

namespace T_Align
{
    public partial class Type_Display : UserControl
    {
        public CogDisplay[] displays;
        private int display_Count = 4;

        private bool display_Set = false;

        private string text = "";

        public bool Display_Set
        {
            get
            {
                return display_Set;
            }
            set
            {
                if(value == true)
                {
                    //cogDisplay1.Size = new Size(this.Width, this.Height);
                    ReSize();
                }
            }
        }

#pragma warning disable CS0114 // 멤버가 상속된 멤버를 숨깁니다. override 키워드가 없습니다.
        public string Text
#pragma warning restore CS0114 // 멤버가 상속된 멤버를 숨깁니다. override 키워드가 없습니다.
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                if (label1.InvokeRequired)
                    label1.Invoke((MethodInvoker)delegate
                    {
                        label1.Text = text;
                    });
                else
                    label1.Text = text;
            }
        }
        public int Display_Count
        {
            get
            {
                return display_Count;
            }
            set
            {
                if (value >= 0 && value <= 4)
                {
                    display_Count = value;
                    //ReSize();
                }
            }
        }

        public Type_Display()
        {
            InitializeComponent();
            displays = new CogDisplay[] { cogDisplay1, cogDisplay2, cogDisplay3, cogDisplay4 };
        }

        public CogDisplay[] DIsplay_Set()
        {
            return displays;
        }

        public Label Title_Set()
        {
            return label1;
        }

        private void ReSize()
        {
            //this.Size = DIsplay_Info.Panel_Size[display_Count,0];
            border_Panel1.Size = this.Size;
            label1.Size = new Size(this.Width - 12, 25);
            // 8 6 8
            // 34 6 8



            if (display_Count == 1)
            {
                int size_X = (this.Width - 16);
                int size_Y = (this.Height - 42);
                displays[0].Size = new Size(size_X, size_Y);
                for (int LoopCount = 1; LoopCount < 4; LoopCount++)
                {
                    displays[LoopCount].Visible = false;
                }
            }
            else if (display_Count == 2)
            {
                int size_X = (this.Width - 22) / 2;
                int size_Y = (this.Height - 42);
                for (int LoopCount = 0; LoopCount < 4; LoopCount++)
                {
                    displays[LoopCount].Visible = true;
                    displays[LoopCount].Size = new Size(size_X, size_Y);
                }
                displays[1].Location = new Point(14 + size_X, 34);
                displays[2].Visible = false;
                displays[3].Visible = false;
            }
            else if (display_Count == 3)
            {
                int size_X = (this.Width - 16);
                int size_Y = (this.Height - 48) / 2;
                for (int LoopCount = 0; LoopCount < 4; LoopCount++)
                {
                    displays[LoopCount].Visible = true;
                    displays[LoopCount].Size = new Size(size_X, size_Y);
                }
                displays[2].Location = new Point(8, 40 + size_Y);
                displays[1].Visible = false;
                displays[3].Visible = false;
            }
            else
            {
                int size_X = (this.Width - 22) / 2;
                int size_Y = (this.Height - 48) / 2;
                for (int LoopCount = 0; LoopCount < 4; LoopCount++)
                {
                    displays[LoopCount].Visible = true;
                    displays[LoopCount].Size = new Size(size_X, size_Y);
                }
                displays[1].Location = new Point(14 + size_X, 34);
                displays[2].Location = new Point(8, 40 + size_Y);
                displays[3].Location = new Point(14 + size_X, 40 + size_Y);
            }
        }

        private void Location_Set(int Type)
        {
            if (Type == 0)
            {

                displays[1].Location = new Point(588, 34);
                displays[2].Location = new Point(8, 495);
                displays[3].Location = new Point(588, 495);
            }
            else if (Type == 1)
            {
                displays[1].Location = new Point(469, 34);
                displays[2].Location = new Point(8, 396);
                displays[3].Location = new Point(469, 396);
            }
            else if (Type == 2)
            {
                displays[1].Location = new Point(294, 34);
                displays[2].Location = new Point(8, 254);
                displays[3].Location = new Point(294, 254);
            }
        }
    }
}
