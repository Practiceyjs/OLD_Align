using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace T_Align
{
    public class Unit_Log
    {
        public void Sequence_Write(Enum.Unit _Unit, string Messege)
        {
            string Folder_Path = "D:\\AlignLog\\" + Convert.ToString(DateTime.Now.Year) + "\\" + Convert.ToString(DateTime.Now.Month) + "\\" + Convert.ToString(DateTime.Now.Day);
            DirectoryInfo di = new DirectoryInfo(Folder_Path);
            if (di.Exists.Equals(false))
            {
                di.Create();
            }
            string File_Path = Folder_Path + "\\" + _Unit.ToString() + "_Sequence_Log.txt";
            FileInfo dir = new FileInfo(File_Path);
            if (dir.Exists == false)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(File_Path, false, System.Text.Encoding.GetEncoding("utf-8")))
                {
                    file.Write("Sequence Log\n");
                }
            }
            using (System.IO.StreamWriter file = File.AppendText(File_Path))
            {
                DateTime Time = DateTime.Now;
                file.Write(Time.ToString() + "." + Time.Millisecond.ToString() + "," + Messege);
                file.Write("\n");
            }
        }

        public void Tact_Write(Enum.Unit _Unit, string Messege)
        {
            string Folder_Path = "D:\\AlignLog\\" + Convert.ToString(DateTime.Now.Year) + "\\" + Convert.ToString(DateTime.Now.Month) + "\\" + Convert.ToString(DateTime.Now.Day);
            DirectoryInfo di = new DirectoryInfo(Folder_Path);
            if (di.Exists.Equals(false))
            {
                di.Create();
            }
            string File_Path = Folder_Path + "\\" + _Unit.ToString() + "_Tact_Log.txt";
            FileInfo dir = new FileInfo(File_Path);
            if (dir.Exists == false)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(File_Path, false, System.Text.Encoding.GetEncoding("utf-8")))
                {
                    file.Write("Sequence Log\n");
                }
            }
            using (System.IO.StreamWriter file = File.AppendText(File_Path))
            {
                file.Write(DateTime.Now.ToString() + "," + Messege);
                file.Write("\n");
            }
        }

        public void Align_Data_Write(Enum.Unit _Unit, object[] Messege)
        {
            string Folder_Path = "D:\\AlignLog\\" + Convert.ToString(DateTime.Now.Year) + "\\" + Convert.ToString(DateTime.Now.Month) + "\\" + Convert.ToString(DateTime.Now.Day);
            DirectoryInfo di = new DirectoryInfo(Folder_Path);
            if (di.Exists.Equals(false))
            {
                di.Create();
            }
            string File_Path = Folder_Path + "\\" + _Unit.ToString() + "_Align_Data.txt";
            FileInfo dir = new FileInfo(File_Path);
            if (dir.Exists == false)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(File_Path, false, System.Text.Encoding.GetEncoding("utf-8")))
                {
                    switch (ETC_Option.Log_Type[(int)_Unit])
                    {
                        case Enum.Log_Type.CW_Log:
                            file.Write("Time,Cell ID,Tact,R Count,Peeling X,Peeling Y,Peeling T,P Offset X,P Offset Y,P Offset T ,Loading X,Loading Y,Loading T,L Offset X,L Offset Y,L Offset T,Rev X,Rev Y,Rev T,Offset X,Offset Y,Offset T,"); // 항목들 입력
                            for (int LoopCount = 1; LoopCount < 5; LoopCount++)
                                file.Write("T Index" + LoopCount.ToString() + " ,T Mark" + LoopCount.ToString() + " X,T Mark" + LoopCount.ToString() + "Y,T Mark" + LoopCount.ToString() + " S,T Mark" + LoopCount.ToString() + " AR,");
                            for (int LoopCount = 1; LoopCount < 5; LoopCount++)
                                file.Write("O Index" + LoopCount.ToString() + " ,O Mark" + LoopCount.ToString() + " X,O Mark" + LoopCount.ToString() + "Y,O Mark" + LoopCount.ToString() + " S,O Mark" + LoopCount.ToString() + " AR,");
                            file.Write("T Wideth1,T Wideth2,T Heigth1,T Heigth2,O Wideth1,O Wideth2,O Heigth1,O Heigth2\n");
                            break;
                        default:
                            file.Write("Time,Cell ID,Tact,R Count,Last X,Last Y,Last T,Rev X,Rev Y,Rev T,Offset X,Offset Y,Offset T,"); // 항목들 입력
                            for (int LoopCount = 1; LoopCount < 5; LoopCount++)
                                file.Write("T Index" + LoopCount.ToString() + " ,T Mark" + LoopCount.ToString() + " X,T Mark" + LoopCount.ToString() + "Y,T Mark" + LoopCount.ToString() + " S,T Mark" + LoopCount.ToString() + " AR,");
                            for (int LoopCount = 1; LoopCount < 5; LoopCount++)
                                file.Write("O Index" + LoopCount.ToString() + " ,O Mark" + LoopCount.ToString() + " X,O Mark" + LoopCount.ToString() + "Y,O Mark" + LoopCount.ToString() + " S,O Mark" + LoopCount.ToString() + " AR,");
                            file.Write("T Wideth1,T Wideth2,T Heigth1,T Heigth2,O Wideth1,O Wideth2,O Heigth1,O Heigth2\n");
                            break;
                    }
                }
            }
            using (System.IO.StreamWriter file = File.AppendText(File_Path))
            {
                for(int LoopCount=0;LoopCount< Messege.Length;LoopCount++)
                    file.Write(Messege[LoopCount] + ",");
                file.Write("\n");
            }
        }

        public void Align_Data_Write(Enum.Unit _Unit, object[] Messege,string CTQ)
        {
            string Folder_Path = "D:\\CTQ\\" + _Unit.ToString();
            DirectoryInfo di = new DirectoryInfo(Folder_Path);
            if (di.Exists.Equals(false))
            {
                di.Create();
            }
            string File_Path = Folder_Path + "\\" + CTQ.ToString() + ".txt";
            FileInfo dir = new FileInfo(File_Path);
            if (dir.Exists == false)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(File_Path, false, System.Text.Encoding.GetEncoding("utf-8")))
                {
                    file.Write("Time,Cell ID,Tact,R Count,Last X,Last Y,Last T,Rev X,Rev Y,Rev T,Offset X,Offset Y,Offset T,"); // 항목들 입력
                    for (int LoopCount = 1; LoopCount < 5; LoopCount++)
                        file.Write("T Index" + LoopCount.ToString() + " ,T Mark" + LoopCount.ToString() + " X,T Mark" + LoopCount.ToString() + "Y,T Mark" + LoopCount.ToString() + " S,T Mark" + LoopCount.ToString() + " AR,");
                    for (int LoopCount = 1; LoopCount < 5; LoopCount++)
                        file.Write("O Index" + LoopCount.ToString() + " ,O Mark" + LoopCount.ToString() + " X,O Mark" + LoopCount.ToString() + "Y,O Mark" + LoopCount.ToString() + " S,O Mark" + LoopCount.ToString() + " AR,");
                    file.Write("T Wideth1,T Wideth2,T Heigth1,T Heigth2,O Wideth1,O Wideth2,O Heigth1,O Heigth2\n");
                }
            }
            using (System.IO.StreamWriter file = File.AppendText(File_Path))
            {
                for (int LoopCount = 0; LoopCount < Messege.Length; LoopCount++)
                    file.Write(Messege[LoopCount] + ",");
                file.Write("\n");
            }
        }

        public void Inspection_Data_Write(Enum.Unit _Unit, object[] Messege)
        {
            string Folder_Path = "D:\\AlignLog\\" + Convert.ToString(DateTime.Now.Year) + "\\" + Convert.ToString(DateTime.Now.Month) + "\\" + Convert.ToString(DateTime.Now.Day);
            DirectoryInfo di = new DirectoryInfo(Folder_Path);
            if (di.Exists.Equals(false))
            {
                di.Create();
            }
            string File_Path = Folder_Path + "\\" + _Unit.ToString() + "_Insepction_Data.txt";
            FileInfo dir = new FileInfo(File_Path);
            if (dir.Exists == false)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(File_Path, false, System.Text.Encoding.GetEncoding("utf-8")))
                {
                    file.Write("Time,Cell ID,Tact,DIs1 X,DIs1 Y,DIs2 X,DIs2 Y,DIs3 X,DIs3 Y,DIs4 X,DIs4 Y,"); // 항목들 입력
                    for (int LoopCount = 1; LoopCount < 5; LoopCount++)
                        file.Write("T Index" + LoopCount.ToString() + " ,T Mark" + LoopCount.ToString() + " X,T Mark" + LoopCount.ToString() + "Y,T Mark" + LoopCount.ToString() + " S,T Mark" + LoopCount.ToString() + " AR,");
                    for (int LoopCount = 1; LoopCount < 5; LoopCount++)
                        file.Write("O Index" + LoopCount.ToString() + " ,O Mark" + LoopCount.ToString() + " X,O Mark" + LoopCount.ToString() + "Y,O Mark" + LoopCount.ToString() + " S,O Mark" + LoopCount.ToString() + " AR,");
                    file.Write("T Wideth1,T Wideth2,T Heigth1,T Heigth2,O Wideth1,O Wideth2,O Heigth1,O Heigth2\n");
                }
            }
            using (System.IO.StreamWriter file = File.AppendText(File_Path))
            {
                for (int LoopCount = 0; LoopCount < Messege.Length; LoopCount++)
                    file.Write(Messege + ",");
                file.Write("\n");
            }
        }

        public void Picture_Save(int Unit, int Cul_Type, DateTime Time, string Cell_ID)
        {
            string FolderName = "D:\\AlignLog\\" + Convert.ToString(DateTime.Now.Year) + "\\" + Convert.ToString(DateTime.Now.Month) + "\\" + Convert.ToString(DateTime.Now.Day) + "\\Image\\Unit" + Convert.ToString(Unit);
            DirectoryInfo dir = new DirectoryInfo(FolderName + "\\Original");
            if (dir.Exists == false)
            {
                dir.Create();
            }
            for (int i = 0; i < 4; i++)
            {
                if (COMMON.U_Setting[Unit].T_Setting[Cul_Type].Position[i])
                {
                    string Save_File = FolderName + "\\" + Cell_ID + "_" + Cul_Type.ToString() + "_" + Time.ToString("HHmmss") + "_" + Convert.ToString(i) + ".jpg";
                    if (COMMON.U_Setting[Unit].T_Setting[Cul_Type].CCTV && i % 2 == 1)
                    {
                        continue;
                    }
                    try
                    {
                        Image_Save_JPG(COMMON.Frame.Unit[Unit].Type[Cul_Type].Mark[i].Display.CreateContentBitmap(Cognex.VisionPro.Display.CogDisplayContentBitmapConstants.Image), Save_File, 50L);
                        if (Recipe.Orizinal_Image_Save)
                        {
                            Save_File = FolderName + "\\Original\\" + Cell_ID + "_" + Cul_Type.ToString() + "_" + Time.ToString("HHmmss") + "_" + Convert.ToString(i) + ".png";
                            //Image_Save_JPG(Frame.Unit[Unit].Type[Cul_Type].Display[i - 1].Image.ToBitmap(), Save_File, 100L);
                            COMMON.Frame.Unit[Unit].Type[Cul_Type].Mark[i].Display.Image.ToBitmap().Save(Save_File, ImageFormat.Png);
                        }
                    }
                    catch { }
                }
            }
        }

        public void UI_Capture(int Unit, int Cul_Type, DateTime Time, string Cell_ID)
        {
            try
            {
                string FolderName = "D:\\AlignLog\\" + Convert.ToString(DateTime.Now.Year) + "\\" + Convert.ToString(DateTime.Now.Month) + "\\" + Convert.ToString(DateTime.Now.Day) + "\\Image\\Unit" + Convert.ToString(Unit);
                DirectoryInfo dir = new DirectoryInfo(FolderName + "\\UI");
                if (dir.Exists == false)
                {
                    dir.Create();
                }
                string Save_File = FolderName + "\\UI\\" + Cell_ID + "_" + Cul_Type.ToString() + "_" + Time.ToString("HHmmss") + ".jpg";
                Rectangle rectangle = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
                Bitmap bitmap = new Bitmap(rectangle.Width, rectangle.Height, PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(bitmap);
                g.CopyFromScreen(rectangle.Left, rectangle.Top, 0, 0, rectangle.Size);
                bitmap.Save(Save_File);
                bitmap.Dispose();
            }
            catch { }
        }

        public void NG_Picture_Save(int Unit, int Cul_Type, DateTime Time, string Cell_ID, int Mark_Num)
        {
            string FolderName = "D:\\AlignLog\\" + Convert.ToString(DateTime.Now.Year) + "\\" + Convert.ToString(DateTime.Now.Month) + "\\" + Convert.ToString(DateTime.Now.Day) + "\\Image\\Unit" + Convert.ToString(Unit) + "\\NG";
            DirectoryInfo dir = new DirectoryInfo(FolderName);
            if (dir.Exists == false)
            {
                dir.Create();
            }
            string Save_File = FolderName + "\\" + Cell_ID + "_" + Cul_Type.ToString() + "_" + Time.ToString("HHmmss") + "_" + Convert.ToString(Mark_Num) + ".jpg";
            //Image_Save_JPG(Frame.Unit[Unit].Type[Cul_Type].Display[i - 1].Image.ToBitmap(), Save_File, 100L);
            try
            {
                COMMON.Frame.Unit[Unit].Type[Cul_Type].Mark[Mark_Num].Display.Image.ToBitmap().Save(Save_File, ImageFormat.Png);
            }
            catch { }
        }

        private void Image_Save_JPG(Image _image, string Path, long _Quality)
        {
            long jpegQuality = _Quality;
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

        public void CTQ_Accuracy_Write(Enum.Unit _Unit, PointF[,] Messege)
        {
            string Folder_Path = "D:\\CTQ\\" + _Unit.ToString();
            DirectoryInfo di = new DirectoryInfo(Folder_Path);
            if (di.Exists.Equals(false))
            {
                di.Create();
            }
            string File_Path = Folder_Path + "\\_Accuracy.txt";
            FileInfo dir = new FileInfo(File_Path);
            if (dir.Exists == false)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(File_Path, false, System.Text.Encoding.GetEncoding("utf-8")))
                {
                    file.Write("Time,"); // 항목들 입력
                    for (int LoopCount = 1; LoopCount < 5; LoopCount++)
                        file.Write("Mark" + LoopCount.ToString() + " X,Mark" + LoopCount.ToString() + "Y,");
                    file.Write("\n");
                }
            }
            using (System.IO.StreamWriter file = File.AppendText(File_Path))
            {
                for (int LoopCount = 0; LoopCount < Messege.Length / 4; LoopCount++)
                {
                    for (int LoopCount2 = 0; LoopCount2 < 4; LoopCount2++)
                        file.Write(Messege[LoopCount2, LoopCount].X.ToString() + "," + Messege[LoopCount2, LoopCount].Y.ToString() + ",");
                    file.Write("\n");
                }
            }
        }

        public void CTQ_Moveing_Write(Enum.Unit _Unit, PointF[,] Messege)
        {
            string Folder_Path = "D:\\CTQ\\" + _Unit.ToString();
            DirectoryInfo di = new DirectoryInfo(Folder_Path);
            if (di.Exists.Equals(false))
            {
                di.Create();
            }
            string File_Path = Folder_Path + "\\_Moveing.txt";
            FileInfo dir = new FileInfo(File_Path);
            if (dir.Exists == false)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(File_Path, false, System.Text.Encoding.GetEncoding("utf-8")))
                {
                    file.Write("Time,"); // 항목들 입력
                    for (int LoopCount = 1; LoopCount < 5; LoopCount++)
                        file.Write("Mark" + LoopCount.ToString() + " X,Mark" + LoopCount.ToString() + "Y,");
                    file.Write("\n");
                }
            }
            using (System.IO.StreamWriter file = File.AppendText(File_Path))
            {
                file.Write(DateTime.Now.ToString() + ",");
                for (int LoopCount = 0; LoopCount < Messege.Length / 4; LoopCount++)
                {
                    for (int LoopCount2 = 0; LoopCount2 < 4; LoopCount2++)
                        file.Write(Messege[LoopCount2, LoopCount].X.ToString() + "," + Messege[LoopCount2, LoopCount].Y.ToString() + ",");
                    file.Write("\n");
                }

            }
        }
    }
}
