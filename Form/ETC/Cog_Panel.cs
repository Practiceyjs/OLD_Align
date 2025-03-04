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
    public partial class Cog_Panel : UserControl
    {
        public Cog_Panel()
        {
            InitializeComponent();
        }

        public void Re_Size()
        {
            Pattern_Display.Size = this.Size;
        }
    }
}
