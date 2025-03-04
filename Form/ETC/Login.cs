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
    public partial class Login : Form
    {
        public bool Simul = false;
#pragma warning disable CS0108 // 멤버가 상속된 멤버를 숨깁니다. new 키워드가 없습니다.
        private enum Select
#pragma warning restore CS0108 // 멤버가 상속된 멤버를 숨깁니다. new 키워드가 없습니다.
        {
            NON,
            ID,
            PASSWORD
        }

        KeyBoard KeyBoard;
        Select _Select = Select.NON;
        public Login()
        {
            InitializeComponent();
        }

        void KetBoard_SendMsg(string Ask)
        {
            try
            {
                if (_Select == Select.ID)
                    if (Ask == "←")
                        ID_textBox.Text = ID_textBox.Text.Substring(0, ID_textBox.Text.Length - 1);
                    else
                        ID_textBox.Text += Ask.ToString();
                else if (_Select == Select.PASSWORD)
                    if (Ask == "←")
                        Password_textBox.Text = Password_textBox.Text.Substring(0, Password_textBox.Text.Length - 1);
                    else
                        Password_textBox.Text += Ask.ToString();
            }
            catch { }

        }

        private void ID_textBox_Click(object sender, EventArgs e)
        {
            _Select = Select.ID;
            if (KeyBoard != null)
            {
                KeyBoard.Close();
            }
            KeyBoard = new KeyBoard();
            KeyBoard.SendMsg += new KeyBoard.SendMsgDele(KetBoard_SendMsg);
            KeyBoard.TopMost = true;
            KeyBoard.Show();
            ID_textBox.Focus();
        }

        private void Password_textBox_Click(object sender, EventArgs e)
        {
            _Select = Select.PASSWORD;
            if (KeyBoard != null)
            {
                KeyBoard.Close();
            }
            KeyBoard = new KeyBoard();
            KeyBoard.SendMsg += new KeyBoard.SendMsgDele(KetBoard_SendMsg);
            KeyBoard.TopMost = true;
            KeyBoard.Show();
            Password_textBox.Focus();
        }

        private void Close_Button_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (KeyBoard != null)
            {
                KeyBoard.Close();
            }
        }

        private void Login_Button_Click(object sender, EventArgs e)
        {
            for(int Loopcount=0; Loopcount< Login_Information.ID.Length; Loopcount++)
            {
                if (Login_Information.ID[Loopcount] == ID_textBox.Text)
                    if (Login_Information.PASSWORD[Loopcount] == Password_textBox.Text)
                    {
                        Simul = Login_Information.Simul[Loopcount];
                        DialogResult = DialogResult.OK;
                        this.Close();
                        return;
                    }
            }
            ERROR Error = new ERROR(Enum.ERROR.Different_Password);
            Error.ShowDialog();
        }
    }
}
