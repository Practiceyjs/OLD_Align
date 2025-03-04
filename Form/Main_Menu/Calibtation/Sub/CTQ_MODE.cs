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
    public partial class CTQ_MODE : Form
    {
        public delegate void TrigerEvevt(CTQ_MODE Control);
        public event TrigerEvevt Triger;

        public int Triger_Count = 20;
        public CTQ_MODE()
        {
            InitializeComponent();
            CTQ_Use_check_Button.Check = COMMON.CTQ_Use;
            Command_richBox.Text = COMMON.CTQ_COMMAND;
            Zero_CheckButton.Check = COMMON.CTQ_Zero_Align;
            Random_CheckButton.Check = COMMON.CTQ_Random_Align;
            Nmr_Realign_Count.Value = (decimal)COMMON.CTQ_Zero_Count;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string Command = button.Name.Substring(11, button.Name.Length - 11);
            switch(Command)
            {
                case "L":
                    Command_richBox.Text += "← ";
                    break;
                case "R":
                    Command_richBox.Text += "→ ";
                    break;
                case "U":
                    Command_richBox.Text += "↑ ";
                    break;
                case "D":
                    Command_richBox.Text += "↓ ";
                    break;
                case "turn":
                    Command_richBox.Text += "↘ ";
                    break;
                case "Return":
                    Command_richBox.Text += "↙ ";
                    break;
                case "Center":
                    Command_richBox.Text += "H ";
                    break;
            }
        }

        private void Button_Clear_Click(object sender, EventArgs e)
        {
            Command_richBox.Text = "";
        }

        private void Check_Button_Click(object sender, EventArgs e)
        {
            Check_Button button = (Check_Button)sender;
            if(!button.Check)
            {
                Zero_CheckButton.Check = false;
                Random_CheckButton.Check = false;
            }
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            COMMON.CTQ_Use = CTQ_Use_check_Button.Check;
            COMMON.CTQ_COMMAND = Command_richBox.Text;
            COMMON.CTQ_Zero_Align = Zero_CheckButton.Check;
            COMMON.CTQ_Random_Align = Random_CheckButton.Check;
            COMMON.CTQ_Zero_Count = (int)Nmr_Realign_Count.Value;
            this.Close();
        }

        private void Button_Triger_Click(object sender, EventArgs e)
        {
            COMMON.CTQ_Use = CTQ_Use_check_Button.Check;
            COMMON.CTQ_COMMAND = Command_richBox.Text;
            COMMON.CTQ_Zero_Align = Zero_CheckButton.Check;
            COMMON.CTQ_Random_Align = Random_CheckButton.Check;
            COMMON.CTQ_Zero_Count = (int)Nmr_Realign_Count.Value;
            Triger_Count = (int)Nmr_Triger_Count.Value;
            Triger(this);
            this.Close();
        }
    }
}
