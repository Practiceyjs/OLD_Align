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
    public partial class ERROR : Form
    {
        public ERROR(Enum.ERROR _Error)
        {
            InitializeComponent();
            switch(_Error)
            {
                case Enum.ERROR.NOT_Login:
                    Error_Label.Text = "You are not logging in.";
                    break;
                case Enum.ERROR.Auto_Run:
                    Error_Label.Text = "It's automatic.\nPlease change it manually.";
                    break;
                case Enum.ERROR.Different_Password:
                    Error_Label.Text = "The password is different or ID does not exist.";
                    break;
                case Enum.ERROR.Current_Recipe:
                    Error_Label.Text = "The recipe you are using cannot be changed.";
                    break;
                case Enum.ERROR.Name_Null:
                    Error_Label.Text = "No spaces are allowed in the name.";
                    break;
                case Enum.ERROR.Null_Recipe:
                    Error_Label.Text = "The recipe can't be empty.";
                    break;
            }
        }

        private void Close_Button_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
