using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.Dimensioning;
using Cognex.VisionPro.Implementation;

namespace T_Align
{
    public class ClassUnit
    {
        public ClassType[] Type = new ClassType[3];
        Enum.Unit Current_Unit = Enum.Unit.Common;
        public Sequence Sequence = new Sequence();
        Thread Sequence_Thread;
        bool Thread_Run = false;
        DataGridView Unit_Data_View;

        public double[] Distance_X = new double[4];
        public double[] Distance_Y = new double[4];

        public void Unit_Set(Enum.Unit _Unit, Type_Display _Target_Display, Type_Display _Object_Display, DataGridView _Unit_Data_View)
        {
            Unit_Data_View = _Unit_Data_View;
            Current_Unit = _Unit;
            int Num = 0;
            Type[Num] = new ClassType();
            Type[Num].Type_Set(Current_Unit, (Enum.Type)Num, _Target_Display);
            Num = 2;
            Type[Num] = new ClassType();
            Type[Num].Type_Set(Current_Unit, (Enum.Type)Num, _Object_Display);
            Sequence.Sequence_Set(Current_Unit, this);
        }

        public int Inspection_Start()
        {
            for (int LoopCount = 0; LoopCount < 4; LoopCount++)
            {
                if (Type[(int)Enum.Type.Target].Mark[LoopCount].Mark_Error)
                    return 2;
                if (Type[(int)Enum.Type.Object].Mark[LoopCount].Mark_Error)
                    return 2;
            }

            Type[(int)Enum.Type.Target].Inspection_L_Check();
            Type[(int)Enum.Type.Object].Inspection_L_Check();

            double[] Target_X = new double[4];
            double[] Target_Y = new double[4];
            double[] Object_X = new double[4];
            double[] Object_Y = new double[4];

            for (int LoopCount=0;LoopCount<4;LoopCount++)
            {
                Target_X[LoopCount] = Type[(int)Enum.Type.Target].Mark[LoopCount].X;
                Target_Y[LoopCount] = Type[(int)Enum.Type.Target].Mark[LoopCount].Y;
                Object_X[LoopCount] = Type[(int)Enum.Type.Object].Mark[LoopCount].X;
                Object_Y[LoopCount] = Type[(int)Enum.Type.Object].Mark[LoopCount].Y;
            }
            double[] Target_Theta = new double[2];
            double[] Object_Theta = new double[2];
            double[] Target_Width = new double[2];
            double[] Object_Width = new double[2];
            double[] Target_Height = new double[2];
            double[] Object_Height = new double[2];
            Target_Width[0] = Type[(int)Enum.Type.Target].Camera_Length + ((Type[(int)Enum.Type.Target].Mark[0].Camera_Width - Type[(int)Enum.Type.Target].Mark[0].X) * Type[(int)Enum.Type.Target].Mark[0].Resolution_X) + (Type[(int)Enum.Type.Target].Mark[1].X * Type[(int)Enum.Type.Target].Mark[1].Resolution_X);
            Target_Width[1] = Type[(int)Enum.Type.Target].Camera_Length + ((Type[(int)Enum.Type.Target].Mark[2].Camera_Width - Type[(int)Enum.Type.Target].Mark[2].X) * Type[(int)Enum.Type.Target].Mark[2].Resolution_X) + (Type[(int)Enum.Type.Target].Mark[3].X * Type[(int)Enum.Type.Target].Mark[3].Resolution_X);
            Object_Width[0] = Type[(int)Enum.Type.Object].Camera_Length + ((Type[(int)Enum.Type.Object].Mark[0].Camera_Width - Type[(int)Enum.Type.Object].Mark[0].X) * Type[(int)Enum.Type.Object].Mark[0].Resolution_X) + (Type[(int)Enum.Type.Object].Mark[1].X * Type[(int)Enum.Type.Object].Mark[1].Resolution_X);
            Object_Width[1] = Type[(int)Enum.Type.Object].Camera_Length + ((Type[(int)Enum.Type.Object].Mark[2].Camera_Width - Type[(int)Enum.Type.Object].Mark[2].X) * Type[(int)Enum.Type.Object].Mark[2].Resolution_X) + (Type[(int)Enum.Type.Object].Mark[3].X * Type[(int)Enum.Type.Object].Mark[3].Resolution_X);
            Target_Height[0] = (Type[(int)Enum.Type.Target].Mark[0].Y - Type[(int)Enum.Type.Target].Mark[1].Y) * ((Type[(int)Enum.Type.Target].Mark[0].Resolution_Y + Type[(int)Enum.Type.Target].Mark[1].Resolution_Y) / 2);
            Target_Height[1] = (Type[(int)Enum.Type.Target].Mark[2].Y - Type[(int)Enum.Type.Target].Mark[3].Y) * ((Type[(int)Enum.Type.Target].Mark[2].Resolution_Y + Type[(int)Enum.Type.Target].Mark[3].Resolution_Y) / 2);
            Object_Height[0] = (Type[(int)Enum.Type.Object].Mark[0].Y - Type[(int)Enum.Type.Object].Mark[1].Y) * ((Type[(int)Enum.Type.Object].Mark[0].Resolution_Y + Type[(int)Enum.Type.Object].Mark[1].Resolution_Y) / 2);
            Object_Height[1] = (Type[(int)Enum.Type.Object].Mark[2].Y - Type[(int)Enum.Type.Object].Mark[3].Y) * ((Type[(int)Enum.Type.Object].Mark[2].Resolution_Y + Type[(int)Enum.Type.Object].Mark[3].Resolution_Y) / 2);
            Target_Theta[0] = Math.Atan(Target_Height[0] / Target_Width[0]);
            Target_Theta[1] = Math.Atan(Target_Height[1] / Target_Width[1]);
            Object_Theta[0] = Math.Atan(Object_Height[0] / Object_Width[0]);
            Object_Theta[1] = Math.Atan(Object_Height[1] / Object_Width[1]);
            for (int LoopCount = 0; LoopCount < 4; LoopCount++) // 수직 :  -1/기울기
            {
                CogCreateLineTool Target_Line = new CogCreateLineTool();
                CogCreateLineTool Object_Line = new CogCreateLineTool();
                CogIntersectLineLineTool Cross_Line_Tool = new CogIntersectLineLineTool();
                CogDistancePointPointTool pointTool_X = new CogDistancePointPointTool();
                CogDistancePointPointTool pointTool_Y = new CogDistancePointPointTool();
                Target_Line.InputImage = Type[(int)Enum.Type.Target].Mark[LoopCount].Last_Grab_Image;
                Object_Line.InputImage = Type[(int)Enum.Type.Object].Mark[LoopCount].Last_Grab_Image;
                Cross_Line_Tool.InputImage = Type[(int)Enum.Type.Target].Mark[LoopCount].Last_Grab_Image;
                pointTool_X.InputImage = Type[(int)Enum.Type.Target].Mark[LoopCount].Last_Grab_Image;
                pointTool_Y.InputImage = Type[(int)Enum.Type.Target].Mark[LoopCount].Last_Grab_Image;
                Target_Line.Line.X = Target_X[LoopCount];
                Target_Line.Line.Y = Target_Y[LoopCount];
                Target_Line.Line.Rotation = Target_Theta[LoopCount / 2];
                Target_Line.Run();
                Object_Line.Line.X = Object_X[LoopCount];
                Object_Line.Line.Y = Object_Y[LoopCount];
                double T = Custom_Math.RadianToDegree(Object_Theta[LoopCount / 2]);
                Object_Line.Line.Rotation = Custom_Math.DegreeToRadian(Custom_Math.RadianToDegree(Object_Theta[LoopCount / 2])-90);
                Object_Line.Run();
                Cross_Line_Tool.LineA = Target_Line.GetOutputLine();
                Cross_Line_Tool.LineB = Object_Line.GetOutputLine();
                Cross_Line_Tool.Run();
                pointTool_X.StartX = Cross_Line_Tool.X;
                pointTool_X.StartY = Cross_Line_Tool.Y;
                pointTool_Y.StartX = Cross_Line_Tool.X;
                pointTool_Y.StartY = Cross_Line_Tool.Y;
                pointTool_X.EndX = Target_X[LoopCount];
                pointTool_X.EndY = Target_Y[LoopCount];
                pointTool_Y.EndX = Object_X[LoopCount];
                pointTool_Y.EndY = Object_Y[LoopCount];
                pointTool_X.Run();
                pointTool_Y.Run();
                Distance_X[LoopCount] = pointTool_X.Distance * Type[(int)Enum.Type.Target].Mark[LoopCount].Resolution_X / 1000;
                Distance_Y[LoopCount] = pointTool_Y.Distance * Type[(int)Enum.Type.Target].Mark[LoopCount].Resolution_Y / 1000;
                COMMON.Inspection_Display[LoopCount].InteractiveGraphics.Clear();
                ICogRecord record = pointTool_Y.CreateLastRunRecord();
                //COMMON.Inspection_Display[LoopCount].InteractiveGraphics.Add(Target_Line.GetOutputLine(), "Dis", false);
                //COMMON.Inspection_Display[LoopCount].InteractiveGraphics.Add(Object_Line.GetOutputLine(), "Dis", false);
                COMMON.Inspection_Display[LoopCount].InteractiveGraphics.Add(record.SubRecords["InputImage"].SubRecords["Arrow"].Content as ICogGraphicInteractive, "Dis", false);
                record = pointTool_X.CreateLastRunRecord();
                COMMON.Inspection_Display[LoopCount].InteractiveGraphics.Add(record.SubRecords["InputImage"].SubRecords["Arrow"].Content as ICogGraphicInteractive, "Dis", false);
                DrawResultString(COMMON.Inspection_Display[LoopCount].InteractiveGraphics, Cross_Line_Tool.X, Cross_Line_Tool.Y + (Cross_Line_Tool.Y - Object_Line.Line.Y) / 2, Distance_X[LoopCount], Distance_Y[LoopCount], LoopCount);
                //COMMON.Inspection_Display[LoopCount].InteractiveGraphics.Add(pointTool_X,"Distance",false);
            }
            return 0;
        }

        private void DrawResultString(CogInteractiveGraphicsContainer G, double _X, double _Y, double Distance_X, double Distance_Y,int Mark_Number)
        {
            CogGraphicLabel label = new CogGraphicLabel();
            string strData = "Dis" + (Mark_Number + 1).ToString() +  " X : " + Distance_X.ToString("0.000") + "    " + "Dis" + (Mark_Number + 1).ToString() + " Y : " + Distance_Y.ToString("0.000");

            label.SetXYText(_X, _Y, strData);

            label.Alignment = CogGraphicLabelAlignmentConstants.TopCenter;
            label.BackgroundColor = CogColorConstants.White;
            label.Color = CogColorConstants.Blue;
            G.Add(label, "Label", false);
        }

        public void Recipe_Load()
        {
            int Num = 0;
            Type[Num].Recipe_Load();
            Num = 2;
            Type[Num].Recipe_Load();
        }

        public void Thread_Start()
        {
            Thread_Run = true;
            Sequence_Thread = new Thread(new ThreadStart(S_Thread));
            Sequence_Thread.Start();
        }

        public void Thread_Stop()
        {
            if(Thread_Run)
            {
                Thread_Run = false;
                Sequence_Thread.Join();
            }
        }

        private void S_Thread()
        {
            while(Thread_Run)
            {
                if (!COMMON.Program_Run)
                    break;
                Sequence.Unit_Sequence();
                Thread.Sleep(50);
            }
        }

        public void Data_Display(object[] Data)
        {
            if (Unit_Data_View.InvokeRequired)
            {
                Unit_Data_View.Invoke((MethodInvoker)delegate
                {
                    Unit_Data_View.Rows.Insert(0, Data);
                    if (Unit_Data_View.Rows.Count > 40)
                        Unit_Data_View.Rows.RemoveAt(40);
                });
            }
            else
            {
                Unit_Data_View.Rows.Insert(0, Data);
                if (Unit_Data_View.Rows.Count > 40)
                    Unit_Data_View.Rows.RemoveAt(40);
            }
        }

        public void Inspection_Data_Display(object[] Data)
        {
            if (COMMON.Inspection_Data.InvokeRequired)
            {
                COMMON.Inspection_Data.Invoke((MethodInvoker)delegate
                {
                    COMMON.Inspection_Data.Rows.Insert(0, Data);
                    if (COMMON.Inspection_Data.Rows.Count > 40)
                        COMMON.Inspection_Data.Rows.RemoveAt(40);
                });
            }
            else
            {
                COMMON.Inspection_Data.Rows.Insert(0, Data);
                if (COMMON.Inspection_Data.Rows.Count > 40)
                    COMMON.Inspection_Data.Rows.RemoveAt(40);
            }
        }
    }
}
