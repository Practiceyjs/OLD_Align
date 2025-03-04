using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace T_Align
{
    public partial class Loading : Form
    {
        delegate void StringParameterDelegate(string Text);
        delegate void SolashShowCloseDelegate();

        bool CloseSpashScreenFlag = false;
        SolidBrush brush = new SolidBrush(Color.Black);

        Thread Start_Thread;
        int tickCount = Environment.TickCount;
        bool Stop = false;

        public Loading()
        {
            InitializeComponent();
            //////////////////////////////////////////////////////////////////////////////
            ///1.0.1.1
            Vervion_label.Text = ETC_Option.VER;
            //////////////////////////////////////////////////////////////////////////////
            Start_Thread = new Thread(_Start_Thread);
            Start_Thread.Start();
            circularProgressBar1.Value = 0;
        }

        private void _Start_Thread()
        {
            int count = 0;
            while (true)
            {
                if (!COMMON.Program_Run)
                    break;
                //circularProgressBar1.StartAngle = count;
                count++;
                if (count.Equals(360))
                    count = 0;
                if(!Stop)
                {
                    if (Time_Label.InvokeRequired)
                    {
                        Time_Label.Invoke((MethodInvoker)delegate
                        {
                            Time_Label.Text = ((double)(Environment.TickCount - tickCount) / 1000).ToString("0.000");
                        });
                    }
                    else
                    {
                        Time_Label.Text = ((double)(Environment.TickCount - tickCount) / 1000).ToString("0.000");
                    }
                }
                Thread.Sleep(10);
            }
        }

        public void ShowSplashScreen()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new SolashShowCloseDelegate(ShowSplashScreen));
                return;
            }
            this.Show();
            Application.Run(this);
        }

        public void CloseSplashScreen()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new SolashShowCloseDelegate(CloseSplashScreen));
                return;
            }
            CloseSpashScreenFlag = true;
            Start_Thread.Abort();
            Start_Thread.Join();
            this.Close();
        }

        public void UpdataStatusText(string Text)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new StringParameterDelegate(UpdataStatusText), new object[] { Text });
                return;
            }
            switch (Text)
            {
                case "STOP":
                    Stop = true;
                    break;
                default:
                    circularProgressBar1.Value = int.Parse(Text);
                    richTextBox1.Text += Underbar_to_Null(((Process_Sequence)int.Parse(Text)).ToString()) + "\n";
                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    richTextBox1.ScrollToCaret();
                    label1.Focus();
                    break;
            }
        }

        private void SplashForm_FormClosing(object Sender, FormClosedEventArgs e)
        {
            if (!CloseSpashScreenFlag)
            {
            }
        }

        private enum Process_Sequence : int
        {
            External_File_Load = 10,
            Recipe_Data_Load = 20,
            Camera_Connect = 30,
            UI_Initialization = 40,
            PLC_Connect = 50,
            Login_Checking_the_authority = 60,
            Unit_Initialization = 70,
            UI_Create = 80,
            Loading_OK = 100
        }

        private string Underbar_to_Null(string Messege)
        {
            return Messege.Replace('_', ' ');
        }

        private void richTextBox1_Click(object sender, EventArgs e)
        {
            label1.Focus();
        }
    }
}
