using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace T_Align
{
    static class Log
    {
        static public void Login_Write(bool On_Off)
        {
            string Folder_Path = "D:\\AlignLog\\" + Convert.ToString(DateTime.Now.Year) + "\\" + Convert.ToString(DateTime.Now.Month) + "\\" + Convert.ToString(DateTime.Now.Day);
            DirectoryInfo di = new DirectoryInfo(Folder_Path);
            if (di.Exists.Equals(false))
            {
                di.Create();
            }
            string File_Path = Folder_Path + "\\LogIn_Log.txt";
            FileInfo dir = new FileInfo(File_Path);
            if (dir.Exists == false)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(File_Path, false, System.Text.Encoding.GetEncoding("utf-8")))
                {
                    file.Write("User Login Log\n");
                }
            }
            using (System.IO.StreamWriter file = File.AppendText(File_Path))
            {
                if (On_Off)
                    file.Write(DateTime.Now.ToString() + "," + COMMON.User_Name);
                else
                    file.Write(DateTime.Now.ToString() + ",Log Out");
                file.Write("\n");
            }
        }

        static public void CTQ_Triger_Write(Enum.Unit _Unit, Enum.Mark _Mark, PointF Messege)
        {
            string Folder_Path = "D:\\CTQ\\" + _Unit.ToString();
            DirectoryInfo di = new DirectoryInfo(Folder_Path);
            if (di.Exists.Equals(false))
            {
                di.Create();
            }
            string File_Path = Folder_Path + "\\_Triger_" + _Mark.ToString()+".txt";
            FileInfo dir = new FileInfo(File_Path);
            if (dir.Exists == false)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(File_Path, false, System.Text.Encoding.GetEncoding("utf-8")))
                {
                    file.Write("Time,"); // 항목들 입력
                    file.Write(_Mark.ToString() + " X," + _Mark.ToString() + " Y");
                    file.Write("\n");
                }
            }
            using (System.IO.StreamWriter file = File.AppendText(File_Path))
            {
                file.Write(DateTime.Now.ToString() + ",");
                file.Write(Messege.X.ToString() + "," + Messege.Y.ToString() + ",");
                file.Write("\n");
            }
        }
    }
}
