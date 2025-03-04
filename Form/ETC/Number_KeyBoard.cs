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
    public partial class Number_KeyBoard : Form
    {
        public delegate void SendMsgDele(string Ask);
        public event SendMsgDele SendMsg;
        Enum.Key_Type Current_Type;

        public Number_KeyBoard(Enum.Key_Type _Current_Type)
        {
            Current_Type = _Current_Type;
            InitializeComponent();
        }

        private void Send_Msg(string _Msg)
        {
            switch(Current_Type)
            {
                case Enum.Key_Type.ALL:
                    SendMsg(_Msg);
                    break;
                case Enum.Key_Type.Positive:
                    if(_Msg != "ㅡ")
                        SendMsg(_Msg);
                    break;
                case Enum.Key_Type.Int_Positive:
                    if (_Msg != "ㅡ" || _Msg != ".")
                        SendMsg(_Msg);
                    break;
                case Enum.Key_Type.Int:
                    if (_Msg != ".")
                        SendMsg(_Msg);
                    break;
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Send_Msg(button.Text);
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            Send_Msg("OK");
            this.Close();
        }
    }
}
