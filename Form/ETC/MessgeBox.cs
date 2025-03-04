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
    public partial class MessgeBox : Form
    {
        DateTime Today;
        BackgroundWorker worker;
        bool Form_Close = false;

        public MessgeBox()
        {
            InitializeComponent();
            this.Shown += new EventHandler(Delete_Run);
        }

        public MessgeBox(DateTime _Today)
        {
            InitializeComponent();
            Today = _Today;
            this.Shown += new EventHandler(Delete_Run2);

        }

        public void Delete_Run(object sender, EventArgs e)
        {
            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(Data_Delete2);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Delete_Complete);
            worker.RunWorkerAsync();
        }

        public void Delete_Run2(object sender, EventArgs e)
        {
            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(Data_Delete);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Delete_Complete);
            worker.RunWorkerAsync();
        }
        public MessgeBox(string Messege)
        {
            InitializeComponent();
            label3.Text = Messege;
            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(Waiting);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Delete_Complete);
            worker.RunWorkerAsync();
        }

        public void Close(bool _Form_Close)
        {
            Form_Close = _Form_Close;
        }

        private void Delete_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        private void Waiting(object sender, DoWorkEventArgs e)
        {
            while (!Form_Close)
                Thread.Sleep(200);
        }

        private void Data_Delete(object sender, DoWorkEventArgs e)
        {
            DateTime Target_Date = Today.AddDays(-Recipe.Data_Save_Day);
            int Year = Target_Date.Year;
            int Month = Target_Date.Month;
            int Day = Target_Date.Day;
            string Folder_Path = "D:\\AlignLog";
            try
            {
                System.IO.DirectoryInfo Dinfo = new System.IO.DirectoryInfo(Folder_Path);
                foreach (var item in Dinfo.GetDirectories())
                {
                    try
                    {
                        int Object_Year = int.Parse(item.Name);
                        if (Object_Year < Year)
                            item.Delete(true);
                    }
                    catch { }
                }
                Dinfo = new System.IO.DirectoryInfo(Folder_Path + "\\" + Year.ToString());
                foreach (var item in Dinfo.GetDirectories())
                {
                    try
                    {
                        int Object_Month = int.Parse(item.Name);
                        if (Object_Month < Month)
                            item.Delete(true);
                    }
                    catch { }
                }
                Dinfo = new System.IO.DirectoryInfo(Folder_Path + "\\" + Year.ToString() + "\\" + Month.ToString());
                foreach (var item in Dinfo.GetDirectories())
                {
                    try
                    {
                        int Object_Day = int.Parse(item.Name);
                        if (Object_Day < Day)
                            item.Delete(true);
                    }
                    catch { }
                }
            }
            catch { }
        }

        private void Data_Delete2(object sender, DoWorkEventArgs e)
        {
            string Folder_Path = "D:\\AlignLog";
            int Target_Year = 9999;
            int Target_Month = 9999;
            int Target_Day = 9999;
            try
            {
                Year:
                System.IO.DirectoryInfo Dinfo = new System.IO.DirectoryInfo(Folder_Path);
                foreach (var item in Dinfo.GetDirectories())
                {
                    try
                    {
                        int Object_Year = int.Parse(item.Name);
                        if (Object_Year < Target_Year)
                            Target_Year = Object_Year;
                    }
                    catch { }
                }
                Month:
                Dinfo = new System.IO.DirectoryInfo(Folder_Path + "\\" + Target_Year.ToString());
                foreach (var item in Dinfo.GetDirectories())
                {
                    try
                    {
                        int Object_Month = int.Parse(item.Name);
                        if (Object_Month < Target_Month)
                            Target_Month = Object_Month;
                    }
                    catch { }
                }
                if (Dinfo.GetDirectories().Length == 0)
                {
                    Dinfo.Delete(true);
                    goto Year;
                }
                Dinfo = new System.IO.DirectoryInfo(Folder_Path + "\\" + Target_Year.ToString() + "\\" + Target_Month.ToString());
                foreach (var item in Dinfo.GetDirectories())
                {
                    try
                    {
                        int Object_Day = int.Parse(item.Name);
                        if (Object_Day < Target_Day)
                            Target_Day = Object_Day;
                    }
                    catch { }
                }
                if (Dinfo.GetDirectories().Length == 0)
                {
                    Dinfo.Delete(true);
                    goto Month;
                }
                Dinfo = new System.IO.DirectoryInfo(Folder_Path + "\\" + Target_Year.ToString() + "\\" + Target_Month.ToString() + "\\" + Target_Day.ToString());
                Dinfo.Delete(true);
            }
            catch { }
        }
    }
}
