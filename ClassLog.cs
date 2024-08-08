using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Cognex.VisionPro.ImageFile;
using System.Drawing;
using System.Drawing.Imaging;

namespace Align
{
    public class ClassLog
    {
        Main_Frame Frame;

        public void Log_Set(Main_Frame _Frame)
        {
            Frame = _Frame;
        }

        public void Log_Write(int Unit, int Cul_Type, DateTime Time, string Cell_ID, int Recipe, double Find_X, double Find_Y, double Find_T, double Offset_X, double Offset_Y, double Offset_T)
        {
            string Folder_Path = "D:\\AlignLog\\" + Convert.ToString(DateTime.Now.Year) + "\\" + Convert.ToString(DateTime.Now.Month) + "\\" + Convert.ToString(DateTime.Now.Day);
            DirectoryInfo di = new DirectoryInfo(Folder_Path);
            if (di.Exists.Equals(false))
            {
                di.Create();
                DateTime date = DateTime.Now.AddDays((double)(-1 * Frame.Setup_Display.Data_Save_Day_numericUpDown.Value));
                string Delte_Patch = "D:\\AlignLog\\";
                try
                {
                    if (System.IO.Directory.Exists(Delte_Patch))
                    {
                        System.IO.DirectoryInfo del = new System.IO.DirectoryInfo(Delte_Patch);
                        foreach (var item in del.GetDirectories())
                        {
                            int Folder = Convert.ToInt16(item.Name);
                            if (Folder < date.Year)
                            {
                                item.Delete(true);
                            }
                        }
                    }
                    Delte_Patch += date.Year;
                    if (System.IO.Directory.Exists(Delte_Patch))
                    {
                        System.IO.DirectoryInfo del = new System.IO.DirectoryInfo(Delte_Patch);
                        foreach (var item in del.GetDirectories())
                        {
                            int Folder = Convert.ToInt16(item.Name);
                            if (Folder < date.Month)
                            {
                                item.Delete(true);
                            }
                        }
                    }
                    Delte_Patch += "\\" + date.Month;
                    if (System.IO.Directory.Exists(Delte_Patch))
                    {
                        System.IO.DirectoryInfo del = new System.IO.DirectoryInfo(Delte_Patch);
                        foreach (var item in del.GetDirectories())
                        {
                            int Folder = Convert.ToInt16(item.Name);
                            if (Folder < date.Day)
                            {
                                item.Delete(true);
                            }
                        }
                    }
                }
                catch { }
            }
            string File_Path = "D:\\AlignLog\\" + Convert.ToString(DateTime.Now.Year) + "\\" + Convert.ToString(DateTime.Now.Month) + "\\" + Convert.ToString(DateTime.Now.Day) + "\\Unit" + Convert.ToString(Unit) + "_AlignLog.txt";
            FileInfo dir = new FileInfo(File_Path);
            if (dir.Exists == false)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(File_Path, false, System.Text.Encoding.GetEncoding("utf-8")))
                {
                    file.Write("Time,Cell ID,Recipe No,Target_X,Target_Y,Target_T,Find_X,Find_Y,Find_T,Offset_X,Offset_Y,Offset_T," +
                                "Mark0_X,Mark0_Y,Match Rate0,Mark1_X,Mark1_Y,Match Rate1,Mark2_X,Mark2_Y,Match Rate2,Mark3_X,Mark3_Y,Match Rate3,L Check1,L Check2,L Check3,L Check4\n");
                }
            }
            using (System.IO.StreamWriter file = File.AppendText(File_Path))
            {
                file.Write(Time.ToString("yyyy-MM-dd HH:mm:ss") + "," + Convert.ToString("Cell ID") + "," + Convert.ToString("Recipe No") + "," +
                    (Find_X + Offset_X).ToString("0.000") + "," + (Find_Y + Offset_Y).ToString("0.000") + "," + (Find_T + Offset_T).ToString("0.000") + "," +
                    Find_X.ToString("0.000") + "," + Find_Y.ToString("0.000") + "," + Find_T.ToString("0.000") + "," +
                    Offset_X.ToString("0.000") + "," + Offset_Y.ToString("0.000") + "," + Offset_T.ToString("0.000") + ",");
                for (int i = 0; i < 4; i++)
                {
                    file.Write(Frame.Unit[Unit].Type[Cul_Type].X[i].ToString("0.000") + "," + Frame.Unit[Unit].Type[Cul_Type].Y[i].ToString("0.000") + "," + Frame.Unit[Unit].Type[Cul_Type].S[i].ToString("0.000") + ",");
                }
                for (int i = 0; i < 4; i++)
                {
                    file.Write(Frame.Unit[Unit].Type[Cul_Type].L_Check[i].ToString("0.000"));
                    file.Write(",");
                }
                file.Write("\n");
            }

        }
        //이미지 저장 구간
        public void Picture_Save(int Unit, int Cul_Type, DateTime Time, string Cell_ID)
        {
            string FolderName = "C:\\AlignLog\\" + Convert.ToString(DateTime.Now.Year) + "\\" + Convert.ToString(DateTime.Now.Month) + "\\" + Convert.ToString(DateTime.Now.Day) + "\\Image\\Unit" + Convert.ToString(Unit);
            DirectoryInfo dir = new DirectoryInfo(FolderName + "\\Original");
            if (dir.Exists == false)
            {
                dir.Create();
            }
            int Display = 4;
            if (!Frame.Unit[Unit].Type[Cul_Type].Second_Grab_Use)
                Display = 2;
            for (int i = 1; i <= Display; i++)
            {
                string Save_File = FolderName + "\\" + Cell_ID + "_" + Time.ToString("HHmmss") + "_" + Convert.ToString(i) + ".jpg";
                if (!Frame.Unit[Unit].Type[Cul_Type].One_Camera_2Point_Use)
                {
                    Image_Save_JPG(Frame.Unit[Unit].Type[Cul_Type].Display[i - 1].Image.ToBitmap(), Save_File,50L);
                    if (!Frame.Setup_Display.Orignal_Unsave_checkBox.Checked)
                    {
                        Sequence_Log_Write(Unit, ClassEnum.Align_Sequence.TEST_Picture1);
                        Save_File = FolderName + "\\Original\\" + Cell_ID + "_" + Time.ToString("HHmmss") + "_" + Convert.ToString(i) + ".bmp";
                        Sequence_Log_Write(Unit, ClassEnum.Align_Sequence.TEST_Picture2);
                        //CogImageFileTool cogImageFile = new CogImageFileTool();
                        //Sequence_Log_Write(1, ClassEnum.Align_Sequence.TEST_Picture3);
                        //cogImageFile.InputImage = Frame.Unit[Unit].Type[Cul_Type].Display[i - 1].Image;
                        //Sequence_Log_Write(1, ClassEnum.Align_Sequence.TEST_Picture4);
                        //cogImageFile.Operator.Open(Save_File, CogImageFileModeConstants.Write);
                        //Sequence_Log_Write(1, ClassEnum.Align_Sequence.TEST_Picture5);
                        //cogImageFile.Run();
                        //Sequence_Log_Write(1, ClassEnum.Align_Sequence.TEST_Picture6);
                        Image_Save_JPG(Frame.Unit[Unit].Type[Cul_Type].Display[i - 1].Image.ToBitmap(), Save_File, 100L);
                    }
                }
                else
                {
                    Bitmap bmp_Image = new Bitmap(Frame.Unit[Unit].Type[Cul_Type].Display[(i - 1) / 2 * 2].Image.ToBitmap());
                    Image_Save_JPG(Frame.Unit[Unit].Type[Cul_Type].Display[(i - 1) / 2 * 2].Image.ToBitmap(), Save_File,50L);
                    if (!Frame.Setup_Display.Orignal_Unsave_checkBox.Checked)
                    {
                        Sequence_Log_Write(Unit, ClassEnum.Align_Sequence.TEST_Picture1);
                        Save_File = FolderName + "\\Original\\" + Cell_ID + "_" + Time.ToString("HHmmss") + "_" + Convert.ToString(i) + ".bmp";
                        Sequence_Log_Write(Unit, ClassEnum.Align_Sequence.TEST_Picture2);
                        //CogImageFileTool cogImageFile = new CogImageFileTool();
                        //Sequence_Log_Write(1, ClassEnum.Align_Sequence.TEST_Picture3);
                        //cogImageFile.InputImage = Frame.Unit[Unit].Type[Cul_Type].Display[i - 1].Image;
                        //Sequence_Log_Write(1, ClassEnum.Align_Sequence.TEST_Picture4);
                        //cogImageFile.Operator.Open(Save_File, CogImageFileModeConstants.Write);
                        //Sequence_Log_Write(1, ClassEnum.Align_Sequence.TEST_Picture5);
                        //cogImageFile.Run();
                        //Sequence_Log_Write(1, ClassEnum.Align_Sequence.TEST_Picture6);
                        Image_Save_JPG(Frame.Unit[Unit].Type[Cul_Type].Display[(i - 1) / 2 * 2].Image.ToBitmap(), Save_File, 100L);
                    }
                }
            }
        }
        //Log 써지는 곳
        public void Sequence_Log_Write(int Unit, ClassEnum.Align_Sequence Align_Sequence)
        {
            string File_Path = "D:\\AlignLog\\" + Convert.ToString(DateTime.Now.Year) + "\\" + Convert.ToString(DateTime.Now.Month) + "\\" + Convert.ToString(DateTime.Now.Day) + "\\Unit" + Convert.ToString(Unit) + "_SecuenceLog.txt";
            FileInfo dir = new FileInfo(File_Path);
            if (dir.Exists == false)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(File_Path, false, System.Text.Encoding.GetEncoding("utf-8")))
                {
                    file.Write("Unit" + Unit.ToString() + "Sequence Log\n\n");
                }
            }
            using (System.IO.StreamWriter file = File.AppendText(File_Path))
            {
                DateTime time = DateTime.Now;
                file.Write(time.ToString("HH:mm:ss") + "." + time.Millisecond.ToString() + "," + Convert.ToString(Align_Sequence));
                file.Write("\n");
                if (Align_Sequence.Equals(ClassEnum.Align_Sequence.Ready))
                    file.Write("\n");
            }
        }

        public void Inspection_Log_Write(int Unit, ClassEnum.Inspection_Sequence Align_Sequence)
        {
            string File_Path = "D:\\AlignLog\\" + Convert.ToString(DateTime.Now.Year) + "\\" + Convert.ToString(DateTime.Now.Month) + "\\" + Convert.ToString(DateTime.Now.Day) + "\\Unit" + Convert.ToString(Unit) + "_InspectionLog.csv";
            FileInfo dir = new FileInfo(File_Path);
            if (dir.Exists == false)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(File_Path, false, System.Text.Encoding.GetEncoding("utf-8")))
                {
                    file.Write("Unit" + Unit.ToString() + "Sequence Log\n\n");
                }
            }
            using (System.IO.StreamWriter file = File.AppendText(File_Path))
            {
                DateTime time = DateTime.Now;
                file.Write(time.ToString("HH:mm:ss") + "." + time.Millisecond.ToString() + "," + Convert.ToString(Align_Sequence));
                file.Write("\n");
                if (Align_Sequence.Equals(ClassEnum.Align_Sequence.Ready))
                    file.Write("\n");
            }
        }

        public void PLC_Log_Write(int Unit, int Align_Sequence)
        {
            string File_Path = "D:\\AlignLog\\" + Convert.ToString(DateTime.Now.Year) + "\\" + Convert.ToString(DateTime.Now.Month) + "\\" + Convert.ToString(DateTime.Now.Day) + "\\Unit" + Convert.ToString(Unit) + "_PLCLog.csv";
            FileInfo dir = new FileInfo(File_Path);
            if (dir.Exists == false)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(File_Path, false, System.Text.Encoding.GetEncoding("utf-8")))
                {
                    file.Write("Unit" + Unit.ToString() + "PLC Log\n\n");
                }
            }
            using (System.IO.StreamWriter file = File.AppendText(File_Path))
            {
                DateTime time = DateTime.Now;
                file.Write(time.ToString("HH:mm:ss") + "." + time.Millisecond.ToString() + "," + Convert.ToString(Align_Sequence));
                file.Write("\n");
                if (Align_Sequence.Equals(ClassEnum.Align_Sequence.Ready))
                    file.Write("\n");
            }
        }

        static private void Image_Save_JPG(Image _image, string Path, long _Quality)
        {
            long jpegQuality = _Quality;  // 이 곳에 압축율을 적어주세요. 0 ~ 100
            Image image = _image;

            EncoderParameters ep = new EncoderParameters(1);
            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, jpegQuality);
            ImageCodecInfo ici = GetEncoderInfo("image/jpeg");
            image.Save(Path, ici, ep);
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < encoders.Length; ++i)
            {
                if (encoders[i].MimeType == mimeType)
                    return encoders[i];
            }
            return null;
        }

        public void Test_Write(int Unit, int Cul_Type, DateTime Time, Point[] _XY)
        {
            string File_Path = "D:\\AlignLog\\" + Convert.ToString(DateTime.Now.Year) + "\\" + Convert.ToString(DateTime.Now.Month) + "\\" + Convert.ToString(DateTime.Now.Day) + "\\Unit" + Convert.ToString(Unit) + "_TestLog.txt";
            FileInfo dir = new FileInfo(File_Path);
            if (dir.Exists == false)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(File_Path, false, System.Text.Encoding.GetEncoding("utf-8")))
                {
                    file.Write("Time,X1,X2,X3,X4,Y1,Y2,Y3,Y4,Theta1,Theta2,Theta3,Theta4\n");
                }
            }
            using (System.IO.StreamWriter file = File.AppendText(File_Path))
            {
                file.Write(Time.ToString("yyyy-MM-dd HH:mm:ss,"));
                for (int i = 0; i < 4; i++)
                {
                    file.Write(Convert.ToString(_XY[i].X));
                    file.Write(",");
                }
                for (int i = 0; i < 4; i++)
                {
                    file.Write(Convert.ToString(_XY[i].Y));
                    file.Write(",");
                }
                for (int i = 0; i < 4; i++)
                {
                    file.Write(Convert.ToString(Frame.Unit[Unit].Type[Cul_Type].Align_T[i]));
                    file.Write(",");
                }
                file.Write("\n");
            }

        }
        public void Tact_Write(int Unit, double Tact)
        {
            string File_Path = "D:\\AlignLog\\" + Convert.ToString(DateTime.Now.Year) + "\\" + Convert.ToString(DateTime.Now.Month) + "\\" + Convert.ToString(DateTime.Now.Day) + "\\Unit" + Convert.ToString(Unit) + "_TactLog.txt";
            FileInfo dir = new FileInfo(File_Path);
            if (dir.Exists == false)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(File_Path, false, System.Text.Encoding.GetEncoding("utf-8")))
                {
                    file.Write("Tact");
                }
            }
            using (System.IO.StreamWriter file = File.AppendText(File_Path))
            {
                file.Write(Tact.ToString("0.000"));
                file.Write("\n");
            }

        }


    }
}
