using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;

namespace T_Align
{
    class ClassINI
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        public static void WriteConfig(string section, string key, object val)
        {
            string Dir = "C:\\ALIGN PARAM\\Recipe\\" + COMMON.RECIPE;
            DirectoryInfo di = new DirectoryInfo(Dir);
            if (di.Exists.Equals(false))
            {
                di.Create();
            }
            string FilePath = "C:\\ALIGN PARAM\\Recipe\\" + COMMON.RECIPE + "\\config.ini";
            WritePrivateProfileString(section, key, val.ToString(), FilePath);
        }

        public static string ReadConfig(string section, string key)
        {
            string FilePath = "C:\\ALIGN PARAM\\Recipe\\" + COMMON.RECIPE + "\\config.ini";
            StringBuilder temp = new StringBuilder(255);
            GetPrivateProfileString(section, key, null, temp, 255, FilePath);
            return temp.ToString();
        }

        //public static void First_Setting()
        //{
        //    string FilePath = "C:\\ALIGN PARAM\\Setting";
        //    DirectoryInfo di = new DirectoryInfo(FilePath);
        //    if (di.Exists == false)
        //        di.Create();
        //    FilePath = "D:\\AlignLog\\" + Convert.ToString(DateTime.Now.Year) + "\\" + Convert.ToString(DateTime.Now.Month) + "\\" + Convert.ToString(DateTime.Now.Day);
        //    DirectoryInfo dir = new DirectoryInfo(FilePath);
        //    if (dir.Exists == false)
        //        dir.Create();
        //}

        public static void WriteProject(string section, string key, object val)
        {
            string Dir = "C:\\ALIGN PARAM\\Setting";
            DirectoryInfo di = new DirectoryInfo(Dir);
            if (di.Exists.Equals(false))
            {
                di.Create();
            }
            string FilePath = "C:\\ALIGN PARAM\\Setting\\Project.ini";
            WritePrivateProfileString(section, key, val.ToString(), FilePath);
        }

        public static string ReadProject(string section, string key)
        {
            string FilePath = "C:\\ALIGN PARAM\\Setting\\Project.ini";
            StringBuilder temp = new StringBuilder(255);
            GetPrivateProfileString(section, key, null, temp, 255, FilePath);
            return temp.ToString();
        }

        public static void WriteRecipe_List(string Number, string Name)
        {
            string Dir = "C:\\ALIGN PARAM\\Setting";
            DirectoryInfo di = new DirectoryInfo(Dir);
            if (di.Exists.Equals(false))
            {
                di.Create();
            }
            string FilePath = "C:\\ALIGN PARAM\\Setting\\Recipe_List.ini";
            WritePrivateProfileString("RECIPE", Number, Name, FilePath);
        }

        public static string ReadRecipe_List(int Number)
        {
            string FilePath = "C:\\ALIGN PARAM\\Setting\\Recipe_List.ini";
            StringBuilder temp = new StringBuilder(255);
            GetPrivateProfileString("RECIPE", Number.ToString(), null, temp, 255, FilePath);
            return temp.ToString();
        }

        public static void Write_Calibration_Data(string section, string key, string val)
        {
            string FilePath = "C:\\ALIGN PARAM\\Recipe\\" + COMMON.RECIPE + "\\Cal_Data.ini";
            WritePrivateProfileString(section, key, val, FilePath);
        }

        public static string Read_Calibration_Data(string section, string key)
        {
            string FilePath = "C:\\ALIGN PARAM\\Recipe\\" + COMMON.RECIPE + "\\Cal_Data.ini";
            StringBuilder temp = new StringBuilder(255);
            int ret = GetPrivateProfileString(section, key, null, temp, 255, FilePath);
            return temp.ToString();
        }

        public static void Write_Light_Data(string section, string key, string val)
        {
            string FilePath = "C:\\ALIGN PARAM\\Recipe\\" + COMMON.RECIPE + "\\Light.ini";
            WritePrivateProfileString(section, key, val, FilePath);
        }

        public static string Read_Light_Data(string section, string key)
        {
            string FilePath = "C:\\ALIGN PARAM\\Recipe\\" + COMMON.RECIPE + "\\Light.ini";
            StringBuilder temp = new StringBuilder(255);
            int ret = GetPrivateProfileString(section, key, null, temp, 255, FilePath);
            return temp.ToString();
        }

        public static void Write_Light_LifeLine(string section, string key, string val)
        {
            string FilePath = "C:\\ALIGN PARAM\\Setting\\Light_LifeLine.ini";
            WritePrivateProfileString(section, key, val, FilePath);
        }

        public static string Read_Light_LifeLine(string section, string key)
        {
            string FilePath = "C:\\ALIGN PARAM\\Setting\\Light_LifeLine.ini";
            StringBuilder temp = new StringBuilder(255);
            int ret = GetPrivateProfileString(section, key, null, temp, 255, FilePath);
            return temp.ToString();
        }
    }
}
