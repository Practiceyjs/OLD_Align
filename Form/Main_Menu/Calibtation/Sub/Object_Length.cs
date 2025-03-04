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
    public partial class Object_Length : Form
    {
        public double Length = 0;

        public Object_Length()
        {
            InitializeComponent();
        }

        private void Close_Button_Click(object sender, EventArgs e)
        {
            OK();
        }

        private void OK()
        {
            try
            {
                Length = double.Parse(Length_Box.Text);
                this.Close();
            }
            catch { }
        }

        private void Length_Box_KeyPress(object sender, KeyPressEventArgs e)
        {
            int Key = Convert.ToInt16(e.KeyChar);
            e.Handled = true;
            if ((char.IsDigit(e.KeyChar)) || Key.Equals(8) || Key.Equals(13) || Key.Equals(46))
            {
                e.Handled = false;
                if (Key.Equals(13))
                    OK();
            }
        }
    }
}
