using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;

namespace Align
{
    class ClassINI
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        public static void WriteConfig(string section, string key, string val)
        {
            string FilePath = "C:\\ALIGN\\Recipe\\" + Main_Frame.Recipe + "\\config.ini";
            WritePrivateProfileString(section, key, val, FilePath);
        }

        public static string ReadConfig(string section, string key)
        {
            string FilePath = "C:\\ALIGN\\Recipe\\" + Main_Frame.Recipe + "\\config.ini";
            StringBuilder temp = new StringBuilder(255);
            int ret = GetPrivateProfileString(section, key, null, temp, 255, FilePath);
            return temp.ToString();
        }

        private static string GetFile(string file)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file + ".ini"); ;
        }

        public static void First_Setting()
        {
            string FilePath = "C:\\ALIGN\\Setting";
            DirectoryInfo di = new DirectoryInfo(FilePath);
            if (di.Exists == false)
                di.Create();
            FilePath = "D:\\AlignLog\\" + Convert.ToString(DateTime.Now.Year) + "\\" + Convert.ToString(DateTime.Now.Month) + "\\" + Convert.ToString(DateTime.Now.Day);
            DirectoryInfo dir = new DirectoryInfo(FilePath);
            if (dir.Exists == false)
                dir.Create();
        }

        public static void WriteProject(string section, string key, string val)
        {
            string FilePath = "C:\\ALIGN\\Setting\\Project.ini";
            WritePrivateProfileString(section, key, val, FilePath);
        }

        public static string ReadProject(string section, string key)
        {
            string FilePath = "C:\\ALIGN\\Setting\\Project.ini";
            StringBuilder temp = new StringBuilder(255);
            int ret = GetPrivateProfileString(section, key, null, temp, 255, FilePath);
            return temp.ToString();
        }

        public static void Write_Calibration_Data(int Unit, PointF[] _L, PointF[] _R, ClassEnum.Position First)
        {
            string FilePath = "C:\\ALIGN\\Setting\\Cal_Data.ini";
            for (int i = 0; i < _L.Length; i++)
            {
                WritePrivateProfileString((Unit * 10 + (int)First + 1).ToString(), i.ToString() + "X", _L[i].X.ToString(), FilePath);
                WritePrivateProfileString((Unit * 10 + (int)First + 1).ToString(), i.ToString() + "Y", _L[i].Y.ToString(), FilePath);
                WritePrivateProfileString((Unit * 10 + (int)First + 2).ToString(), i.ToString() + "X", _R[i].X.ToString(), FilePath);
                WritePrivateProfileString((Unit * 10 + (int)First + 2).ToString(), i.ToString() + "Y", _R[i].Y.ToString(), FilePath);
            }
        }

        public static void Write_Calibration_Data(string section, string key, string val)
        {
            string FilePath = "C:\\ALIGN\\Setting\\Cal_Data.ini";
            WritePrivateProfileString(section, key, val, FilePath);
        }

        public static void Read_Calibration_Data(int Unit, out PointF[] _L, out PointF[] _R, ClassEnum.Position First)
        {
            string FilePath = "C:\\ALIGN\\Setting\\Cal_Data.ini";
            StringBuilder temp = new StringBuilder(255);
            PointF[] L = new PointF[28];
            PointF[] R = new PointF[28];
            for (int i = 0; i < 28; i++)
            {
                try
                {
                    GetPrivateProfileString((Unit * 10 + (int)First + 1).ToString(), i.ToString() + "X", null, temp, 255, FilePath);
                    L[i].X = (float)Convert.ToDouble(temp.ToString());
                    GetPrivateProfileString((Unit * 10 + (int)First + 1).ToString(), i.ToString() + "Y", null, temp, 255, FilePath);
                    L[i].Y = (float)Convert.ToDouble(temp.ToString());
                    GetPrivateProfileString((Unit * 10 + (int)First + 2).ToString(), i.ToString() + "X", null, temp, 255, FilePath);
                    R[i].X = (float)Convert.ToDouble(temp.ToString());
                    GetPrivateProfileString((Unit * 10 + (int)First + 2).ToString(), i.ToString() + "Y", null, temp, 255, FilePath);
                    R[i].Y = (float)Convert.ToDouble(temp.ToString());
                }
                catch { break; }
            }
            _L = L;
            _R = R;
        }

        public static string Read_Calibration_Data(string section, string key)
        {
            string FilePath = "C:\\ALIGN\\Setting\\Cal_Data.ini";
            StringBuilder temp = new StringBuilder(255);
            int ret = GetPrivateProfileString(section, key, null, temp, 255, FilePath);
            return temp.ToString();
        }
    }
}
