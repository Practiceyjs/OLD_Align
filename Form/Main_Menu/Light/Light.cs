using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T_Align
{
    public partial class Light : Form
    {
        public delegate void CancelDele(object _null);
        public event CancelDele cancel;

        Button[] Unit_Button;

        Enum.Unit Current_Unit = Enum.Unit.Unit1;
        CH_Control[] Target_Light;
        CH_Control[] Object_Light;

        public Light()
        {
            InitializeComponent();
            Unit_Button = new Button[] { Button_Unit1, Button_Unit2, Button_Unit3, Button_Unit4, Button_Unit5, Button_Unit6, Button_Unit7, Button_Unit8 };
            Target_Light = new CH_Control[] { cH_Control1, cH_Control2, cH_Control3, cH_Control4, cH_Control5, cH_Control6 };
            Object_Light = new CH_Control[] { cH_Control12, cH_Control11, cH_Control10, cH_Control9, cH_Control8, cH_Control7 };
        }

        public void Loading_OK()
        {
            for (int LoopCount = 1; LoopCount < 9; LoopCount++)
            {
                if (COMMON.Frame.Unit[LoopCount] == null)
                    Unit_Button[LoopCount - 1].Visible = false;
            }
            Button_Select((int)Current_Unit);
        }

        private void Unit_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Current_Unit = (Enum.Unit)Convert.ToInt32(button.Text.Substring(1, 1));
            Button_Select((int)Current_Unit);
        }

        private void Button_Select(int Select_Number)
        {
            for (int LoopCount = 0; LoopCount < 8; LoopCount++)
            {
                Unit_Button[LoopCount].Image = Properties.Resources.Unit2;
                Unit_Button[LoopCount].ForeColor = Color.White;
            }
            Unit_Button[Select_Number - 1].Image = Properties.Resources.Unit2_Select;
            Unit_Button[Select_Number - 1].ForeColor = Color.Cyan;
            for (int LoopCount = 0; LoopCount < 6; LoopCount++)
            {
                int CH = COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Target].Ch[LoopCount];
                string Comport = COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Target].ComPort[LoopCount];
                string Light_Name = COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Target].Light_Name[LoopCount];
                ////////////////////////////////////////////////////////////
                /// 1.0.1.3
                if (Light_Name != "")
                {
                    try
                    {
                        int int_Comport = int.Parse(Comport.Substring(3, Comport.Length - 3));
                        Target_Light[LoopCount].Unit_Load(Current_Unit, (Enum.Serial)int_Comport, CH, COMMON.Light_Controlers[int_Comport].Auto_Value[CH], Light_Name);
                        Target_Light[LoopCount].Visible = true;
                    }
                    catch { Target_Light[LoopCount].Visible = false; }
                }
                else
                    Target_Light[LoopCount].Visible = false;
                ////////////////////////////////////////////////////////////
            }
            for (int LoopCount = 0; LoopCount < 6; LoopCount++)
            {
                int CH = COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Object].Ch[LoopCount];
                string Comport = COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Object].ComPort[LoopCount];
                string Light_Name = COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Object].Light_Name[LoopCount];
                ////////////////////////////////////////////////////////////
                /// 1.0.1.3
                if (Light_Name != "")
                {
                    try
                    {
                        int int_Comport = int.Parse(Comport.Substring(3, Comport.Length - 3));
                        Object_Light[LoopCount].Unit_Load(Current_Unit, (Enum.Serial)int_Comport, CH, COMMON.Light_Controlers[int_Comport].Auto_Value[CH], Light_Name);
                        Object_Light[LoopCount].Visible = true;
                    }
                    catch { Object_Light[LoopCount].Visible = false; }
                }
                else
                    Object_Light[LoopCount].Visible = false;
                ////////////////////////////////////////////////////////////
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Keys Key = keyData & ~(Keys.Shift | Keys.Control | Keys.Alt);
            switch (Key)
            {
                case Keys.Escape:
                    cancel(new object());
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
