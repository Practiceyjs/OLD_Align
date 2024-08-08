using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognex.VisionPro;
using Cognex.VisionPro.FGGigE;
using Cognex.VisionPro.Display;
using System.Windows.Forms;
using System.Drawing;
using Cognex.VisionPro.ImageFile;
using System.Threading;


namespace Align
{
    public class ClassUnit
    {
        Cognex.VisionPro.ICogImage[] cogGrabImage;
        Cognex.VisionPro.ICogAcqFifo[] camera;

        ClassCogSet cam_set = new ClassCogSet();
        public ClassType[] Type = new ClassType[3];
        public Main_Frame Frame;

        public bool Object_Use = false;

        public ClassSequence Sequence = new ClassSequence();
        Thread Sequence_Thread;

        public bool Program_run = true;

        int cur_Unit;

        string Recipe;

        public double Limit_X = 3;
        public double Limit_Y = 3;
        public double Limit_T = 3;

        public bool Inspection_Use = false;

        public void Recipe_Send(string _Recipe)
        {
            Recipe = _Recipe;
            try { Limit_X = Convert.ToDouble(ClassINI.ReadConfig("Limit", "Unit" + cur_Unit.ToString() + "_Limit_X")); }
            catch { }
            try { Limit_Y = Convert.ToDouble(ClassINI.ReadConfig("Limit", "Unit" + cur_Unit.ToString() + "_Limit_Y")); }
            catch { }
            try { Limit_T = Convert.ToDouble(ClassINI.ReadConfig("Limit", "Unit" + cur_Unit.ToString() + "_Limit_T")); }
            catch { }
            for (int l = 0; l <= 2; l++)
            {
                for (int i = 0; i <= 3; i++)
                {
                    Type[l].Model[i].Recipe_Send(Recipe);
                    Type[l].Model[i].Mark_Load();
                    Type[l].Model[i].Parameter_load();
                }
                if (l.Equals(0))
                    Type[l].Type_Setting(cur_Unit, Sequence, camera, ClassEnum.Type.Target, Frame);
                else if (l.Equals(2))
                    Type[l].Type_Setting(cur_Unit, Sequence, camera, ClassEnum.Type.Object, Frame);
            }
        }

        public void Unit_Setting(Main_Frame _Frame, int _cur_Unit)
        {
            cur_Unit = _cur_Unit;
            Frame = _Frame;
            if (ClassINI.ReadProject("Unit", "Unit" + Convert.ToString(cur_Unit) + " Object_Use").Equals("True"))
                Object_Use = true;
            else
                Object_Use = false;
            cam_set.Init_Camera(Frame);
            camera = cam_set.Camera_IP_Set(IP_Load(cur_Unit)); //Camera Setting Unit에 맞춰 진행
            cogGrabImage = new Cognex.VisionPro.ICogImage[camera.Length];
            for (int l = 0; l <= 2; l++)
            {
                Type[l] = new ClassType();
                for (int i = 0; i <= 3; i++)
                {
                    Type[l].Model[i] = new ClassMark();
                    if (l.Equals(0))
                    {
                        Type[l].Model[i].Mark_Set(ClassEnum.Type.Target, cur_Unit, Frame, i);
                    }
                    else if (l.Equals(2))
                    {
                        Type[l].Model[i].Mark_Set(ClassEnum.Type.Object, cur_Unit, Frame, i);
                    }
                }
            }
            if (ClassINI.ReadProject("Unit", "Unit" + Convert.ToString(cur_Unit) + " Inspection_Use").Equals("True"))
                Inspection_Use = true;
            else
                Inspection_Use = false;
            Sequence.Sequence_Set(Frame, cur_Unit);
            Sequence_Thread = new Thread(S_Thread);
            Sequence_Thread.Start();
        }

        private void S_Thread(object Unit)
        {
            while (Program_run)
            {
                Sequence.Unit_Sequence(Frame.PLC.Receive_Data, Frame.PLC.Send_Data);    //시퀀스 확인
                Thread.Sleep(5);
            }
        }

        private string[] IP_Load(int _Cur_Unit)
        {
            string[] _IP = new string[4];
            string temp = ClassINI.ReadProject("Unit", "Unit" + _Cur_Unit.ToString() + " Cam1");
            if (temp != "")
                _IP[0] = ClassINI.ReadProject("Cam", temp + " IP");
            temp = ClassINI.ReadProject("Unit", "Unit" + _Cur_Unit.ToString() + " Cam2");
            if (temp != "")
                _IP[1] = ClassINI.ReadProject("Cam", temp + " IP");
            temp = ClassINI.ReadProject("Unit", "Unit" + _Cur_Unit.ToString() + " Cam3");
            if (temp != "")
                _IP[2] = ClassINI.ReadProject("Cam", temp + " IP");
            temp = ClassINI.ReadProject("Unit", "Unit" + _Cur_Unit.ToString() + " Cam4");
            if (temp != "")
                _IP[3] = ClassINI.ReadProject("Cam", temp + " IP");
            return _IP;
        }

        public void DIsplay_Setting(CogDisplay[] _T_Display, CogDisplay[] _O_Display)
        {
            Type[0].Display = _T_Display;
            Type[2].Display = _O_Display;
            Type[0].Cal_Display = new CogDisplay[2] { Frame.Calibration_Display.cogDisplay1, Frame.Calibration_Display.cogDisplay2 };
            Type[2].Cal_Display = new CogDisplay[2] { Frame.Calibration_Display.cogDisplay1, Frame.Calibration_Display.cogDisplay2 };
        }
    }
}
