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
    public partial class KeyBoard : Form
    {
        public delegate void SendMsgDele(string Ask);
        public event SendMsgDele SendMsg;

        bool Shift = false;
        string[] apabat = { "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "a", "s", "d", "f", "g", "h", "j", "k", "l", "z", "x", "c", "v", "b", "n", "m" };
        string[] Apabat = { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "A", "S", "D", "F", "G", "H", "J", "K", "L", "Z", "X", "C", "V", "B", "N", "M" };
        Button[] Char_Button;
        public KeyBoard()
        {
            InitializeComponent();
            Char_Button = new Button[] { C1, C2, C3, C4, C5, C6, C7, C8, C9, C10, C11, C12, C13, C14, C15, C16, C17, C18, C19, C20, C21, C22, C23, C24, C25, C26 };
        }

        private void Send_Msg(string _Msg)
        {
            SendMsg(_Msg);
        }

        private void Shift_Click(object sender, EventArgs e)
        {
            Shift = !Shift;
            if (Shift)
                for (int LoopCount = 0; LoopCount < Char_Button.Length; LoopCount++)
                    Char_Button[LoopCount].Text = Apabat[LoopCount];
            else
                for (int LoopCount = 0; LoopCount < Char_Button.Length; LoopCount++)
                    Char_Button[LoopCount].Text = apabat[LoopCount];
        }

        private void BE_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Send_Msg(button.Text);
        }
    }
}
