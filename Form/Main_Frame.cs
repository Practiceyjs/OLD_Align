using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

using Cognex.VisionPro;
using Cognex.VisionPro.Display;

namespace T_Align
{
    public partial class  Main_Frame : Form
    {
        bool Interface_On = false;
        bool Light_On = false;
        bool PLC_Auto_Check = false;
        bool[] CamState = new bool[] { false, false, false, false, false, false, false, false };

        PerformanceCounter Cpu_Counter;
        PerformanceCounter Memory_Counter;
        PerformanceCounter C_Drive;
        PerformanceCounter D_Drive;
        Temperature temperature = new Temperature();

        public ClassPLC PLC = new ClassPLC();
        public Calibration Calibration;
        public ClassUnit[] Unit = new ClassUnit[9];

        Interface Interface;
        Light Light;
        Main Main;
        Model Model;
        Data Data;
        Setup Setup;
        Cognex_Grabber Cognex_Grabber = new Cognex_Grabber();
        //Euresys_Grab[] Euresys_Grab = new Euresys_Grab[9];
        Euresys_Callback[] Euresys_Callback = new Euresys_Callback[9];
        CREVIS_Grab CREVIS_Grab = new CREVIS_Grab();
        Balser_Grabber balser_Grabber;
        Thread Live_Thread;

        Euresys.EGenTL GenTL;

        ToolStripSeparator[] Cam_Separator;
        ToolStripSeparator[] Unit_Separator;

        DateTime Last_Day;

        int Alive_Count = 0;

        public Main_Frame()
        {
            COMMON.Program_Run = true;
            ////////////////////////////////////////////////////////////////////////////
            ///로딩창 영역
            Thread Splashthread = new Thread(new ThreadStart(Splash_Loading.ShowSplashScreen));
            Splashthread.IsBackground = true;
            Splashthread.Start();
            for (int LoopCount = 1; LoopCount < 9; LoopCount++)
            {
                COMMON.U_Setting[LoopCount] = new COMMON_Unit_Setting();
                COMMON.U_Setting[LoopCount].T_Setting[0] = new COMMON_Type_Setting();
                COMMON.U_Setting[LoopCount].T_Setting[2] = new COMMON_Type_Setting();
            }
            COMMON.Recipe_Select = new Recipe_Select();
            COMMON.Recipe_Select.Recipe_Change += new Recipe_Select.Recipe_Change_Delegate(Recipe_Change);
            COMMON.Extenal_Setting = new Extenal_Setting();
            ////////////////////////////////////////////////////////////////////////////
            ///
            ////////////////////////////////////////////////////////////////////////////
            ///외부파일 읽기
            Splash_Loading.UpdateStatusText("10");
            string[] Parameter = DES.DES.Parameser_Get();
            string[] Unit_Name = new string[8];
            string[] Target_Name = new string[8];
            string[] Object_Name = new string[8];
            string[] Read_Bit_Name = new string[8 * 16];
            string[] Write_Bit_Name = new string[8 * 16];
            string[] Read_Word_Name = new string[8 * 15];
            string[] Write_Word_Name = new string[8 * 15];
            string[] Target_Light_Name = new string[8 * 6];
            string[] Object_Light_Name = new string[8 * 6];
            if (Parameter != null)
            {
                ETC_Option.EQP = Parameter[0];
                ETC_Option.Retry_Limit = int.Parse(Parameter[1]);
                ETC_Option.Mode = int.Parse(Parameter[2]);
                ETC_Option.Interface_Offset = int.Parse(Parameter[3]);
                ETC_Option.Shuttle_Position_Use = bool.Parse(Parameter[4]);
                ETC_Option.Cog_Camera = bool.Parse(Parameter[5]);
                ETC_Option.Enet = bool.Parse(Parameter[6]);
                ETC_Option.Mark_Test_Mode = bool.Parse(Parameter[7]);
                ETC_Option.Simul_Calibration = bool.Parse(Parameter[8]);
                ClassInterface.Channel = int.Parse(Parameter[9]);
                Load_Unit_Parameter(Slice(Parameter, 10, 97));
                if (Slice(Parameter, "98") == "W")
                {
                    ETC_Option.Enet = false;
                    ClassInterface.Recive_Address = Convert.ToInt32(Slice(Parameter, "99"), 16);
                    ClassInterface.Send_Address = Convert.ToInt32(Slice(Parameter, "100"), 16);
                }
                else if(Slice(Parameter, "98") == "D")
                {
                    ETC_Option.Enet = true;
                    ClassInterface.Recive_Address = Convert.ToInt32(Slice(Parameter, "99"));
                    ClassInterface.Send_Address = Convert.ToInt32(Slice(Parameter, "100"));
                }

                int count = 101;
                Unit_Name = Slice(Parameter, count, count + Unit_Name.Length - 1);
                count = count + Unit_Name.Length;
                Target_Name = Slice(Parameter, count, count + Target_Name.Length - 1);
                count = count + Target_Name.Length;
                Object_Name = Slice(Parameter, count, count + Object_Name.Length - 1);
                count = count + Object_Name.Length;
                Read_Bit_Name = Slice(Parameter, count, count + Read_Bit_Name.Length - 1);
                count = count + Read_Bit_Name.Length;
                Write_Bit_Name = Slice(Parameter, count, count + Write_Bit_Name.Length - 1);
                count = count + Write_Bit_Name.Length;
                Read_Word_Name = Slice(Parameter, count, count + Read_Word_Name.Length - 1);
                count = count + Read_Word_Name.Length;
                Write_Word_Name = Slice(Parameter, count, count + Write_Word_Name.Length - 1);
                count = count + Write_Word_Name.Length;
                Target_Light_Name = Slice(Parameter, count, count + Target_Light_Name.Length - 1);
                count = count + Target_Light_Name.Length;
                Object_Light_Name = Slice(Parameter, count, count + Object_Light_Name.Length - 1);
            }
            else
            {
                MessageBox.Show("Parameter가 손상되었습니다.");
                Environment.Exit(0);
            }
            for (int Unit_Count = 0; Unit_Count < 8; Unit_Count++)
            {
                if (Unit_Name[Unit_Count].ToUpper() == "SPARE")
                    COMMON.U_Setting[Unit_Count + 1].Unit_Name = "";
                else
                    COMMON.U_Setting[Unit_Count + 1].Unit_Name = Unit_Name[Unit_Count];
                COMMON.U_Setting[Unit_Count + 1].Bit_Recive_Name = Slice(Read_Bit_Name, Unit_Count * 16, Unit_Count * 16 + 15);
                COMMON.U_Setting[Unit_Count + 1].Bit_Send_Name = Slice(Write_Bit_Name, Unit_Count * 16, Unit_Count * 16 + 15);
                COMMON.U_Setting[Unit_Count + 1].Word_Recive_Name = Slice(Read_Word_Name, Unit_Count * 15, Unit_Count * 15 + 14);
                COMMON.U_Setting[Unit_Count + 1].Word_Send_Name = Slice(Write_Word_Name, Unit_Count * 15, Unit_Count * 15 + 14);
                COMMON.U_Setting[Unit_Count + 1].T_Setting[0].Type_Name = Target_Name[Unit_Count];
                COMMON.U_Setting[Unit_Count + 1].T_Setting[2].Type_Name = Object_Name[Unit_Count];
                COMMON.U_Setting[Unit_Count + 1].T_Setting[0].Light_Name = Slice(Target_Light_Name, Unit_Count * 6, Unit_Count * 6 + 5);
                COMMON.U_Setting[Unit_Count + 1].T_Setting[2].Light_Name = Slice(Object_Light_Name, Unit_Count * 6, Unit_Count * 6 + 5);
            }
            Calibration = new Calibration();
            Interface = new Interface();
            Light = new Light();
            Main = new Main();
            Model = new Model();
            Data = new Data();
            Setup = new Setup();
            COMMON.RECIPE = ClassINI.ReadProject("RECIPE", "Current");
            try
            {
                COMMON.RECIPE_NUM = int.Parse(ClassINI.ReadProject("RECIPE", "No"));
            }
            catch {
                COMMON.RECIPE_NUM = 0;
            }
            if (COMMON.RECIPE == "")
            {
                COMMON.Extenal_Setting.ShowDialog();
                COMMON.Recipe_Select.ShowDialog();
            }
            else
            {
                COMMON.Extenal_Setting.COMMON_Data_Load();
                //익스터널 파일들 로드
                for (int LoopCount = 1; LoopCount < 9; LoopCount++)
                {
                    COMMON.U_Setting[LoopCount].Inspection_Use = bool.Parse(ClassINI.ReadProject("Unit" + LoopCount.ToString(), "Inspection_Use"));
                    for (int Type_LoopCount = 0; Type_LoopCount < 3; Type_LoopCount = Type_LoopCount + 2)
                    {
                        for (int Position_Count = 0; Position_Count < 4; Position_Count++)
                            COMMON.U_Setting[LoopCount].T_Setting[Type_LoopCount].Position[Position_Count] = bool.Parse(ClassINI.ReadProject("Unit" + LoopCount.ToString(), ((Enum.Type)Type_LoopCount).ToString() + "_Position_" + Position_Count.ToString()));
                        for (int ComPort_Count = 0; ComPort_Count < 6; ComPort_Count++)
                        {
                            COMMON.U_Setting[LoopCount].T_Setting[Type_LoopCount].ComPort[ComPort_Count] = ClassINI.ReadProject("Unit" + LoopCount.ToString(), ((Enum.Type)Type_LoopCount).ToString() + "_ComPort_" + ComPort_Count.ToString());
                            COMMON.U_Setting[LoopCount].T_Setting[Type_LoopCount].Ch[ComPort_Count] = int.Parse(ClassINI.ReadProject("Unit" + LoopCount.ToString(), ((Enum.Type)Type_LoopCount).ToString() + "_Ch_" + ComPort_Count.ToString()));
                        }
                        for (int Cam_Count = 0; Cam_Count < 2; Cam_Count++)
                            COMMON.U_Setting[LoopCount].T_Setting[Type_LoopCount].Cam[Cam_Count] = int.Parse(ClassINI.ReadProject("Unit" + LoopCount.ToString(), ((Enum.Type)Type_LoopCount).ToString() + "_Cam_" + Cam_Count.ToString()));
                        for (int Direction_Count = 0; Direction_Count < 3; Direction_Count++)
                            COMMON.U_Setting[LoopCount].T_Setting[Type_LoopCount].Direction[Direction_Count] = int.Parse(ClassINI.ReadProject("Unit" + LoopCount.ToString(), ((Enum.Type)Type_LoopCount).ToString() + "_Direction_" + Direction_Count.ToString()));
                        COMMON.U_Setting[LoopCount].T_Setting[Type_LoopCount].DIsplay_Page = int.Parse(ClassINI.ReadProject("Unit" + LoopCount.ToString(), ((Enum.Type)Type_LoopCount).ToString() + "_DIsplay_Page"));
                        COMMON.U_Setting[LoopCount].T_Setting[Type_LoopCount].CCTV = bool.Parse(ClassINI.ReadProject("Unit" + LoopCount.ToString(), ((Enum.Type)Type_LoopCount).ToString() + "_CCTV"));
                        if (Type_LoopCount == 0)
                            COMMON.U_Setting[LoopCount].T_Setting[Type_LoopCount].Light_Name = Slice(Target_Light_Name, (LoopCount - 1) * 6, (LoopCount - 1) * 6 + 5);
                        else
                            COMMON.U_Setting[LoopCount].T_Setting[Type_LoopCount].Light_Name = Slice(Object_Light_Name, (LoopCount - 1) * 6, (LoopCount - 1) * 6 + 5);
                    }
                }
                //레시피 리스트 로드
                //LOADING
            }
            Splash_Loading.UpdateStatusText("20");
            ////////////////////////////////////////////////////////////////////////////
            ///
            ////////////////////////////////////////////////////////////////////////////
            ///카메라 연결
            for (int LoopCount = 1; LoopCount < 9; LoopCount++)
            {
                string FIle_Path = "C:\\Simul_Image\\" + LoopCount.ToString() + ".bmp";
                //COMMON.Camera_Image[LoopCount] = new Cognex.VisionPro.CogImage8Grey((Bitmap)Bitmap.FromFile(FIle_Path));
            }
            Camera_Connection();
            Splash_Loading.UpdateStatusText("30");
            ////////////////////////////////////////////////////////////////////////////
            ///
            ////////////////////////////////////////////////////////////////////////////
            ///UI로딩
            Interface.Show();
            Light.Show();
            COMMON.Frame = this;
            InitializeComponent();
            COMMON.Cam_Labels = new ToolStripLabel[] { C1_Label, C2_Label, C3_Label, C4_Label, C5_Label, C6_Label, C7_Label, C8_Label };
            COMMON.Unit_Labels = new ToolStripLabel[] { null, U1_Label, U2_Label, U3_Label, U4_Label, U5_Label, U6_Label, U7_Label, U8_Label };
            Cam_Separator = new ToolStripSeparator[] { toolStripSeparator9, toolStripSeparator10, toolStripSeparator11, toolStripSeparator12, toolStripSeparator13, toolStripSeparator14, toolStripSeparator15, toolStripSeparator16 };
            Unit_Separator = new ToolStripSeparator[] { null, toolStripSeparator18 , toolStripSeparator19 , toolStripSeparator20 , toolStripSeparator21 , toolStripSeparator22 , toolStripSeparator23 , toolStripSeparator24 , toolStripSeparator25 };
            toolStripEQP.Text = ETC_Option.EQP;
            toolStripRECIPE.Text = COMMON.RECIPE;
            toolStripVER.Text = ETC_Option.VER;
            Button_Select("H");
            Cpu_Counter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            Memory_Counter = new PerformanceCounter("Memory", "% Committed Bytes In Use");
            C_Drive = new PerformanceCounter("LogicalDisk", "% Free space", "C:");
            D_Drive = new PerformanceCounter("LogicalDisk", "% Free space", "D:");
            Tick_Timer.Start();
            GC_Timer.Start();
            for (int LoopCount = 0; LoopCount < 8; LoopCount++)
            {
                if (CamState[LoopCount])
                {
                    COMMON.Cam_Labels[LoopCount].ForeColor = Color.LightGreen;
                }
                if (COMMON.CAMERA_IP[LoopCount] == "")
                {
                    COMMON.Cam_Labels[LoopCount].Visible = false;
                    Cam_Separator[LoopCount].Visible = false;
                    toolStripLabel3.Size = new Size(toolStripLabel3.Size.Width + 64, toolStripLabel3.Size.Height);
                }
            }
            Splash_Loading.UpdateStatusText("40");
            ////////////////////////////////////////////////////////////////////////////
            ///
            ////////////////////////////////////////////////////////////////////////////
            ///PLC연결
            if (PLC.OPEN() == 0)
                PLC_Label.ForeColor = Color.LightGreen;
            Splash_Loading.UpdateStatusText("50");
            //MessageBox.Show(a.ToString());
            ////////////////////////////////////////////////////////////////////////////
            ///
            ////////////////////////////////////////////////////////////////////////////
            ///자동로그인
            if (ETC_Option.Auto_Login != -1)
            {
                COMMON.User_Name = Login_Information.ID[ETC_Option.Auto_Login];
                COMMON.Login = true;
                //COMMON.Current_Mode = Enum.Mode.Auto;
                COMMON.Simul_Allow = Login_Information.Simul[ETC_Option.Auto_Login];
                Setup.Private_Panel(COMMON.Simul_Allow);
                Login_Button.Image = global::T_Align.Properties.Resources.UnLock;
                //Auto_Button.Image = global::T_Align.Properties.Resources.Manual;
            }
            Splash_Loading.UpdateStatusText("60");
            ////////////////////////////////////////////////////////////////////////////
            ///
            ////////////////////////////////////////////////////////////////////////////
            for (int LoopCount = 1; LoopCount < 9; LoopCount++)
            {
                if (COMMON.U_Setting[LoopCount].Unit_Name != "")
                {
                    Unit[LoopCount] = new ClassUnit();
#if CW_SAM
                    if (LoopCount == 3) //CW단동기 한정 한화면에 두개 디스플레이
                        Unit[LoopCount].Unit_Set((Enum.Unit)LoopCount, Main.Target_Display[LoopCount - 2], Main.Object_Display[LoopCount - 2], Main.Unit_Data_View[LoopCount - 1]);
                    else
                        Unit[LoopCount].Unit_Set((Enum.Unit)LoopCount, Main.Target_Display[LoopCount - 1], Main.Object_Display[LoopCount - 1], Main.Unit_Data_View[LoopCount - 1]);
#else
                    Unit[LoopCount].Unit_Set((Enum.Unit)LoopCount, Main.Target_Display[LoopCount - 1], Main.Object_Display[LoopCount - 1], Main.Unit_Data_View[LoopCount - 1]);
#endif
                    COMMON.Unit_Labels[LoopCount].ForeColor = Color.LightGreen;
                    for(int Type_Loop=0; Type_Loop<6; Type_Loop++)
                    {
                        try
                        {
                            int int_COMPORT = int.Parse(COMMON.U_Setting[LoopCount].T_Setting[(int)Enum.Type.Target].ComPort[Type_Loop].Substring(3, COMMON.U_Setting[LoopCount].T_Setting[(int)Enum.Type.Target].ComPort[Type_Loop].Length - 3));
                            COMMON.U_Setting[LoopCount].T_Setting[(int)Enum.Type.Target].Useing_Comport[int_COMPORT] = true;
                            if (COMMON.Light_Controlers[int_COMPORT] == null)
                            {
                                COMMON.Light_Controlers[int_COMPORT] = new Light_Controler();
                                COMMON.Light_Controlers[int_COMPORT].COMPORT_OPEN((Enum.Serial)int_COMPORT);
                                COMMON.Light_Controlers[int_COMPORT].Value_Load();
                            }
                        }
                        catch { }
                        try
                        {
                            int int_COMPORT = int.Parse(COMMON.U_Setting[LoopCount].T_Setting[(int)Enum.Type.Object].ComPort[Type_Loop].Substring(3, COMMON.U_Setting[LoopCount].T_Setting[(int)Enum.Type.Target].ComPort[Type_Loop].Length - 3));
                            COMMON.U_Setting[LoopCount].T_Setting[(int)Enum.Type.Object].Useing_Comport[int_COMPORT] = true;
                            if (COMMON.Light_Controlers[int_COMPORT] == null)
                            {
                                COMMON.Light_Controlers[int_COMPORT] = new Light_Controler();
                                COMMON.Light_Controlers[int_COMPORT].COMPORT_OPEN((Enum.Serial)int_COMPORT);
                                COMMON.Light_Controlers[int_COMPORT].Value_Load();
                            }
                        }
                        catch { }
                    }
                }
                else
                {
                    COMMON.Unit_Labels[LoopCount].Visible = false;
                    Unit_Separator[LoopCount].Visible = false;
                }
            }
            Splash_Loading.UpdateStatusText("70");
            ////////////////////////////////////////////////////////////////////////////
            ///
            ////////////////////////////////////////////////////////////////////////////
            ///LOADING 후 실행
            COMMON.Extenal_Setting.Loading_OK();
            COMMON.Extenal_Setting.Button_Select(1);
            COMMON.Recipe_Select.Loading_OK();
            Main.Loading_OK();
            Model.Loading_OK();
            Setup.Loading_OK();
            Calibration.Loading_OK();
            Interface.Loading_OK();
            Light.Loading_OK();
            Data.Loading_OK();
            Interface.cancel += Display_Cancel;
            Light.cancel += Display_Cancel;
            Splash_Loading.UpdateStatusText("80");
            ////////////////////////////////////////////////////////////////////////////
            ///
            try
            {
                for (int i = 2; i < 6; i++)
                {
                    Euresys_Callback[i].Grab_Start();
                }
                CREVIS_Grab.Live_Start();
            }
            catch { }
            Live_Thread = new Thread(Live_Loop);
            //Live_Thread.Start();
            Splash_Loading.UpdateStatusText("100");
        }

        private void Load_Unit_Parameter(string[] Parameter)
        {
            for (int LoopCount = 0; LoopCount < 8; LoopCount++)
            {
                ETC_Option.Unit_Type[LoopCount + 1] = T_Align.Enum.Unit_Type_Parse(Parameter[LoopCount * 11 + 0]);
                ETC_Option.DA_UNIT[LoopCount + 1] = int.Parse(Parameter[LoopCount * 11 + 1]);
                ETC_Option.Motor_Type[LoopCount + 1] = T_Align.Enum.Motor_Type_Parse(Parameter[LoopCount * 11 + 2]);
                ETC_Option.UVW_R[LoopCount + 1] = int.Parse(Parameter[LoopCount * 11 + 3]);
                ETC_Option.Calibration_Type[0, LoopCount + 1] = T_Align.Enum.Calibration_Type_Parse(Parameter[LoopCount * 11 + 4]);
                ETC_Option.Calibration_Type[2, LoopCount + 1] = T_Align.Enum.Calibration_Type_Parse(Parameter[LoopCount * 11 + 5]);
                ETC_Option.Second_Grab_First_Use[LoopCount + 1] = bool.Parse(Parameter[LoopCount * 11 + 6]);
                ETC_Option.Log_Type[LoopCount + 1] = T_Align.Enum.Log_Type_Parse(Parameter[LoopCount * 11 + 7]);
                ETC_Option.Light_Controler[LoopCount + 1] = Parameter[LoopCount * 11 + 8];
                ETC_Option.Light_Controler_Max_CH[LoopCount + 1] = int.Parse(Parameter[LoopCount * 11 + 9]);
                ETC_Option.Light_Controler_val[LoopCount + 1] = int.Parse(Parameter[LoopCount * 11 + 10]);
            }
        }

        public string[] Slice(string[] source, int start,int End)
        {
            int Len = End - start + 1;
            string[] res = new string[Len];
            for (int i = 0; i < Len; i++)
            {
                res[i] = source[i + start];
            }
            return res;
        }
        public string Slice(string[] source, string start) {return source[int.Parse(start)];}

        private void Live_Loop()
        {
            while(true)
            {
                for (int LoopCount = 1; LoopCount < 9; LoopCount++)
                    if (Unit[LoopCount] != null)
                        Unit_Sequence_Start((Enum.Unit)LoopCount);
                Thread.Sleep(50);
            }
        }

        private void Unit_Sequence_Start(Enum.Unit _Unit_Num)
        {
            if (COMMON.Current_Mode.Equals(Enum.Mode.Auto) || COMMON.Current_Mode.Equals(Enum.Mode.Simul))
            {
                if (Unit[(int)_Unit_Num].Sequence.Sequences.Equals(Enum.Sequence.Ready))
                {
                    ClassInterface.Bit_On_Off((int)_Unit_Num, (int)PLC_COMMAND.READY, true);
                    if (ClassInterface.Bit_Check((int)_Unit_Num, (int)PLC_COMMAND.Align_Start))                  // 얼라인 신호시
                    {
                        Unit[(int)_Unit_Num].Sequence.Sequences = Enum.Sequence.Align;
                        Unit[(int)_Unit_Num].Sequence.Align_Sequence = Enum.Align_Sequence.Align_Start_Req;
                        Unit[(int)_Unit_Num].Thread_Start();
                    }
                    else if (ClassInterface.Bit_Check((int)_Unit_Num, (int)PLC_COMMAND.Inspection_Start))                  // 얼라인 신호시
                    {
                        Unit[(int)_Unit_Num].Sequence.Sequences = Enum.Sequence.Inspection;
                        Unit[(int)_Unit_Num].Sequence.Inspection_Sequence = Enum.Inspection_Sequence.Inspecion_Start_Req;
                        Unit[(int)_Unit_Num].Thread_Start();
                    }
                    else if (ClassInterface.Bit_Check((int)_Unit_Num, (int)PLC_COMMAND.Cal_Start))     // 캘리브레이션 신호시
                    {
                        Unit[(int)_Unit_Num].Sequence.Sequences = Enum.Sequence.Calibration;
                        Unit[(int)_Unit_Num].Sequence.Calibration_Sequence = Enum.Calibration_Sequence.Calibration_Start_Req;
                        Unit[(int)_Unit_Num].Thread_Start();
                    }
                }
            }
            else
                ClassInterface.Bit_On_Off((int)_Unit_Num, (int)PLC_COMMAND.READY, false);
        }

        private enum PLC_COMMAND
        {
            READY,
            ERROR,
            RESET,
            N1St_Ack,
            Cal_Start,
            Cal_End,
            Align_Start,
            AlignEnd,
            ReAlign,
            Spare,
            Target_Object,
            N2nd_Ack,
            Move_Ack,
            Move_Done_Req,
            Inspection_Start,
            Inspection_End
        }

        private void Camera_Connection()
        {


            if (ETC_Option.Cog_Camera)
            {
#if CW_SAM
#elif T9_LAMI
                //Project 마다 변경 필요
                //
                CogDisplay[] Live = Main.Live_Display[0].DIsplay_Set();
                //맨좌측
                COMMON.Live_Display[3] = Live[0];
                COMMON.Live_Display[4] = Live[1];
                Live = Main.Live_Display[1].DIsplay_Set();
                //중간
                COMMON.Live_Display[1] = Live[0];
                COMMON.Live_Display[2] = Live[1];
                Live = Main.Live_Display[2].DIsplay_Set();
                //Pre, 맨 우측 들어오는 애들
                COMMON.Live_Display[5] = Live[0];
                COMMON.Live_Display[6] = Live[1];
                Live = Main.Live_Display[3].DIsplay_Set();
                //Pre, 맨 우측 들어오는 애들
                COMMON.Live_Display[7] = Live[0];
                COMMON.Live_Display[8] = Live[1];
#endif
                balser_Grabber = new Balser_Grabber();
                CamState = balser_Grabber.Balser_Grabber_Connect(COMMON.CAMERA_IP);
                //CamState = Cognex_Grabber.Cognex_Camera_Set(COMMON.CAMERA_IP);

            }
            else
            {
#if CW_SAM
                //Project 마다 변경 필요
                //
                CogDisplay[] Live = Main.Live_Display[0].DIsplay_Set();
                //맨좌측
                COMMON.Live_Display[3] = Live[0];
                COMMON.Live_Display[5] = Live[1];
                Live = Main.Live_Display[1].DIsplay_Set();
                //중간
                COMMON.Live_Display[4] = Live[0];
                COMMON.Live_Display[6] = Live[1];
                Live = Main.Live_Display[2].DIsplay_Set();
                //Pre, 맨 우측 들어오는 애들
                COMMON.Live_Display[1] = Live[0];
                COMMON.Live_Display[2] = Live[1];
#endif
                //GenTL = new Euresys.EGenTL();
                try
                {
                    GenTL = new Euresys.EGenTL();
                    for (int LoopCount = 0; LoopCount < COMMON.CAMERA_IP.Length; LoopCount++)
                    {
                        try
                        {
                            if (COMMON.CAMERA_IP[LoopCount].Substring(0, 6).ToLower() == "device")
                            {
                                Euresys_Callback[LoopCount] = new Euresys_Callback();
                                CamState[LoopCount] = Euresys_Callback[LoopCount].Euresys_Set(GenTL, int.Parse(COMMON.CAMERA_IP[LoopCount].Substring(6, 1)), LoopCount + 1);
                            }
                        }
                        catch { }
                        
                    }
                    bool[] Crevis_State = CREVIS_Grab.Crevis_Open(COMMON.CAMERA_IP);
                    for (int LoopCount = 0; LoopCount < CamState.Length; LoopCount++)
                        CamState[LoopCount] = CamState[LoopCount] | Crevis_State[LoopCount];
                }
                catch { }
            }
        }

        private void Menu_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Button_Select(button.Name.Substring(button.Name.Length - 1, 1));
        }

        private void Button_Select(string _Select)
        {
            Button_H.Image = global::T_Align.Properties.Resources.Home;
            Button_C.Image = global::T_Align.Properties.Resources.Calibration;
            Button_M.Image = global::T_Align.Properties.Resources.Model;
            Button_D.Image = global::T_Align.Properties.Resources.Data;
            Button_S.Image = global::T_Align.Properties.Resources.Setup;
            Main_Panel.Controls.Clear();
            switch (_Select)
            {
                case "H":
                    Button_H.Image = global::T_Align.Properties.Resources.Home_Select;
                    Main_Panel.Controls.Add(Main);
                    break;
                case "C":
                    Button_C.Image = global::T_Align.Properties.Resources.Calibration_Select;
                    Main_Panel.Controls.Add(Calibration);
                    break;
                case "M":
                    Button_M.Image = global::T_Align.Properties.Resources.Model_Select;
                    Main_Panel.Controls.Add(Model);
                    break;
                case "D":
                    Button_D.Image = global::T_Align.Properties.Resources.Data_Select;
                    Main_Panel.Controls.Add(Data);
                    break;
                case "S":
                    Button_S.Image = global::T_Align.Properties.Resources.Setup_Select;
                    Main_Panel.Controls.Add(Setup);
                    break;
            }
            if (_Select != "C")
                Calibration.Live_Stop();
            if (_Select != "M")
                Model.Live_Stop();
        }

        private void Button_Interface_Click(object sender, EventArgs e)
        {
            Interface_On = !Interface_On;
            if (Interface_On)
            {
                Interface.Visible = true;
                Button_Interface.Image = global::T_Align.Properties.Resources.Interface_Select;
            }
            else
            {
                Interface.Visible = false;
                Button_Interface.Image = global::T_Align.Properties.Resources.Interface;
            }
        }

        private void Button_Light_Click(object sender, EventArgs e)
        {
            Light_On = !Light_On;
            if (Light_On)
            {
                Light.Visible = true;
                Button_Light.Image = global::T_Align.Properties.Resources.Light_Select;
            }
            else
            {
                Light.Visible = false;
                Button_Light.Image = global::T_Align.Properties.Resources.Light;
            }
        }

        private void Interface_Display_On()
        {
            Interface_On = true;
            Interface.Visible = true;
            Button_Interface.Image = global::T_Align.Properties.Resources.Interface_Select;
        }

        private void Light_Display_On()
        {
            Light_On = true;
            Light.Visible = true;
            Button_Light.Image = global::T_Align.Properties.Resources.Light_Select;
        }

        private void Display_Cancel(object sender)
        {
            if (Light_On)
            {
                Light_On = false;
                Light.Visible = false;
                Button_Light.Image = global::T_Align.Properties.Resources.Light;
            }
            if (Interface_On)
            {
                Interface_On = false;
                Interface.Visible = false;
                Button_Interface.Image = global::T_Align.Properties.Resources.Interface;
            }
        }

        private void Auto_Button_Click(object sender, EventArgs e)
        {
            if(COMMON.Login)
            {
                if (COMMON.Current_Mode == Enum.Mode.Manual)
                {
                    COMMON.Current_Mode = Enum.Mode.Auto;
                    Auto_Button.Image = global::T_Align.Properties.Resources.Manual;
                    ClassInterface.Bit_On_Off(0, 1, true);
                    for (int LoopCount = 0; LoopCount < COMMON.Light_Controlers.Length; LoopCount++)
                    {
                        if (COMMON.Light_Controlers[LoopCount] != null)
                            COMMON.Light_Controlers[LoopCount].All_On();
                    }
                }
                else if (COMMON.Current_Mode == Enum.Mode.Auto)
                {
                    if(COMMON.Simul_Allow)
                    {
                        COMMON.Current_Mode = Enum.Mode.Simul;
                        Auto_Button.Image = global::T_Align.Properties.Resources.Simul;
                        ClassInterface.Bit_On_Off(0, 1, true);
                    }
                    else
                    {
                        COMMON.Current_Mode = Enum.Mode.Manual;
                        Auto_Button.Image = global::T_Align.Properties.Resources.Auto;
                        ClassInterface.Bit_On_Off(0, 1, false);
                        if(Recipe.Auto_Light)
                        {
                            for (int LoopCount = 0; LoopCount < COMMON.Light_Controlers.Length; LoopCount++)
                            {
                                if (COMMON.Light_Controlers[LoopCount] != null)
                                    COMMON.Light_Controlers[LoopCount].All_Off();
                            }
                        }

                    }
                }
                else if (COMMON.Current_Mode == Enum.Mode.Simul)
                {
                    COMMON.Current_Mode = Enum.Mode.Manual;
                    Auto_Button.Image = global::T_Align.Properties.Resources.Auto;
                    ClassInterface.Bit_On_Off(0, 1, false);
                    if (Recipe.Auto_Light)
                    {
                        for (int LoopCount = 0; LoopCount < COMMON.Light_Controlers.Length; LoopCount++)
                        {
                            if (COMMON.Light_Controlers[LoopCount] != null)
                                COMMON.Light_Controlers[LoopCount].All_Off();
                        }
                    }
                }
            }
            else
            {
                ERROR Error = new ERROR(Enum.ERROR.NOT_Login);
                Error.ShowDialog();
            }
        }

        private void Login_Button_Click(object sender, EventArgs e)
        {
            if (!COMMON.Login)
            {
                Login login = new Login();
                if (login.ShowDialog() == DialogResult.OK)
                {
                    COMMON.User_Name = login.ID_textBox.Text;
                    Login_Button.Image = global::T_Align.Properties.Resources.UnLock;
                    COMMON.Simul_Allow = login.Simul;
                    Log.Login_Write(true);
                    COMMON.Login = !COMMON.Login;
                    if(COMMON.User_Name == "ME")
                    {
                        Setup.Private_Panel(true);
                    }
                }
            }
            else
            {
                COMMON.User_Name = "";
                Login_Button.Image = global::T_Align.Properties.Resources.Lock;
                COMMON.Simul_Allow = false;
                Log.Login_Write(false);
                COMMON.Login = !COMMON.Login;
                Setup.Private_Panel(false);
            }
        }

        private void Button_Close_Click(object sender, EventArgs e)
        {
            if (!(COMMON.Current_Mode == Enum.Mode.Auto))
            {
                for (int LoopCount = 0; LoopCount < COMMON.Light_Controlers.Length; LoopCount++)
                {
                    if (COMMON.Light_Controlers[LoopCount] != null)
                        COMMON.Light_Controlers[LoopCount].All_Off();
                }
                balser_Grabber.Balser_Close();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                COMMON.Program_Run = false;
                CREVIS_Grab.Thread_Stop();
                CREVIS_Grab = null;
                PLC.Thread_Stop();
                balser_Grabber.Balser_Close();
                //Simulation
                //for (int i = 2; i < 6; i++)
                //{
                //    Euresys_Grab[i].Thread_Stop();
                //}
                for (int LoopCount = 1; LoopCount < 9; LoopCount++)
                {
                    if (COMMON.U_Setting[LoopCount].Unit_Name != "")
                    {
                        Unit[LoopCount].Thread_Stop();
                    }
                }
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                ERROR Error = new ERROR(Enum.ERROR.Auto_Run);
                Error.ShowDialog();
            }
        }

        private void Button_H_MouseHover(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Explanation_Display(button.Name.Substring(button.Name.Length - 1, 1));
        }

        private void Button_H_MouseLeave(object sender, EventArgs e)
        {
            Explanation_panel.Visible = false;
        }

        private void Explanation_Display(string _Select)
        {
            switch (_Select)
            {
                case "H":
                    Explanation_Label.Text = "Main";
                    break;
                case "C":
                    Explanation_Label.Text = "Calibration";
                    break;
                case "M":
                    Explanation_Label.Text = "Model";
                    break;
                case "D":
                    Explanation_Label.Text = "Data";
                    break;
                case "S":
                    Explanation_Label.Text = "Setup";
                    break;
                case "e":
                    Explanation_Label.Text = "Interface";
                    break;
                case "t":
                    Explanation_Label.Text = "Light";
                    break;
            }
            Explanation_panel.Visible = true;
        }

        private void Auto_Button_MouseHover(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Explanation_Display2(button.Name.Substring(0, 1));
        }

        private void Auto_Button_MouseLeave(object sender, EventArgs e)
        {
            Explanation_panel2.Visible = false;
        }

        private void Explanation_Display2(string _Select)
        {
            switch (_Select)
            {
                case "A":
                    if (COMMON.Current_Mode == Enum.Mode.Auto)
                        Explanation_Label2.Text = "Auto";
                    else if (COMMON.Current_Mode == Enum.Mode.Manual)
                        Explanation_Label2.Text = "Manual";
                    else if (COMMON.Current_Mode == Enum.Mode.Simul)
                        Explanation_Label2.Text = "Simulation";
                    break;
                case "L":
                    Explanation_Label2.Text = "Login";
                    break;
                case "R":
                    Explanation_Label2.Text = "Reset";
                    break;
            }
            Explanation_panel2.Visible = true;
        }

        //private void Live_Veiw()
        //{
        //    for (int LoopCount = 2; LoopCount < 6; LoopCount++)
        //    {
        //        if (LoopCount < 6)
        //        {
        //            if (COMMON.Image_buffers[LoopCount] == null)
        //                continue;

        //            if (COMMON.Image_buffers[LoopCount] == null)
        //                continue;

        //            IntPtr imgPtr;
        //            COMMON.Image_buffers[LoopCount].getInfo(Euresys_Grab[LoopCount].grabber, Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_BASE, out imgPtr);

        //            System.Drawing.Imaging.BitmapData bmpData = null;
        //            try
        //            {
        //                bmpData = COMMON.Bitmap_Image[LoopCount].LockBits(new Rectangle(0, 0, (int)Euresys_Grab[LoopCount].imgWidth, (int)Euresys_Grab[LoopCount].imgHeight),
        //                    System.Drawing.Imaging.ImageLockMode.WriteOnly,
        //                    System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        //                Euresys_Grab[LoopCount].converter.toBGR8(bmpData.Scan0, imgPtr, Euresys_Grab[LoopCount].imgFormat, Euresys_Grab[LoopCount].imgWidth, Euresys_Grab[LoopCount].imgHeight);
        //            }
        //            finally
        //            {
        //                if (bmpData != null)
        //                {
        //                    COMMON.Bitmap_Image[LoopCount].UnlockBits(bmpData);
        //                    COMMON.Camera_Image[LoopCount+1] = new CogImage8Grey(COMMON.Bitmap_Image[LoopCount]);
        //                }
        //            }

        //            Euresys.Buffer bufferTmp = COMMON.Image_buffers[LoopCount];
        //            COMMON.Image_buffers[LoopCount] = null;
        //            if (Euresys_Grab[LoopCount].grabber != null)
        //            {
        //                bufferTmp.push(Euresys_Grab[LoopCount].grabber);
        //            }
        //        }
        //    }
        //}

        private void Tick_Timer_Tick(object sender, EventArgs e)
        {
            //Live_Veiw();
            for (int LoopCount = 1; LoopCount < 9; LoopCount++)
                if (Unit[LoopCount] != null)
                    Unit_Sequence_Start((Enum.Unit)LoopCount);
            Alive_Count++;
            if(Alive_Count==5)
            {
                Alive_Count = 0;
                if ((COMMON.Send_Data[0] & 0x0001) == 1)
                    ClassInterface.Bit_On_Off(0, 0, false);
                else
                    ClassInterface.Bit_On_Off(0, 0, true);
            }
            toolStripDATE_TIME.Text = Convert.ToString(DateTime.Now.Year) + "/" + Convert.ToString(DateTime.Now.Month) + "/" + Convert.ToString(DateTime.Now.Day) + "\n" + DateTime.Now.Hour.ToString("00") + " : " + DateTime.Now.Minute.ToString("00") + " : " + DateTime.Now.Second.ToString("00");

            float usage = Cpu_Counter.NextValue();
            if (CPU_ProgressBar.InvokeRequired)
                CPU_ProgressBar.Invoke((MethodInvoker)delegate
                {
                    CPU_ProgressBar.Value = ((int)usage);
                });
            else
                CPU_ProgressBar.Value = ((int)usage);

            usage = Memory_Counter.NextValue();
            if (RAM_ProgressBar.InvokeRequired)
                RAM_ProgressBar.Invoke((MethodInvoker)delegate
                {
                    RAM_ProgressBar.Value = (int)usage;
                });
            else
                RAM_ProgressBar.Value = (int)usage;

            usage = 100 - C_Drive.NextValue();
            if (C_ProgressBar.InvokeRequired)
                C_ProgressBar.Invoke((MethodInvoker)delegate
                {
                    C_ProgressBar.Value = (int)usage;
                });
            else
                C_ProgressBar.Value = (int)usage;

            usage = 100 - D_Drive.NextValue();
            if (D_ProgressBar.InvokeRequired)
                D_ProgressBar.Invoke((MethodInvoker)delegate
                {
                    D_ProgressBar.Value = (int)usage;
                });
            else
                D_ProgressBar.Value = (int)usage;
            if(usage > Recipe.Data_Storage)
            {
                MessgeBox messgeBox = new MessgeBox();
                messgeBox.ShowDialog();
            }
            if(COMMON.Current_Mode == Enum.Mode.Manual)
            {
                if(Last_Day.ToString("yy.MM.dd") != DateTime.Now.ToString("yy.MM.dd"))
                {
                    Last_Day = DateTime.Now;
                    MessgeBox messgeBox = new MessgeBox(DateTime.Now);
                    messgeBox.ShowDialog();
                }
            }
            if ((COMMON.Receive_Data[0] & 0x0002) == 0x0002 && !PLC_Auto_Check)
            {
                PLC_Auto_Check = true;
                COMMON.CTQ_Use = false;
            }
            else
            {
                if (PLC_Auto_Check && (COMMON.Receive_Data[0] & 0x0002) == 0x0000)
                {
                    Last_Day = DateTime.Now;
                    MessgeBox messgeBox = new MessgeBox(DateTime.Now);
                    messgeBox.ShowDialog();
                    PLC_Auto_Check = false;
                }
            }
            try 
            { 
                Temperature.Temp_Infomation temp_Infomation = temperature.Get_Temperature();
                Display_Temperature(CPU_temp, temp_Infomation.CPU_Temp);
                Display_Temperature(MainBorad_temp, temp_Infomation.MainBoard_Temp);
                Display_Temperature(HDD_temp, temp_Infomation.HDD_Temp);
            }
            catch { }
        }

        private void Display_Temperature(Control control, float value)
        {
            //if (value > 60)
            //    control.ForeColor = Color.Red;
            //else
            //    control.ForeColor = Color.SkyBlue;
            control.Text = value.ToString("00");
        }

        private void GC_Timer_Tick(object sender, EventArgs e)
        {
            GC.Collect();
        }

        private void Main_Frame_Load(object sender, EventArgs e)
        {
            ////////////////////////////////////////////////////////////////////////////
            ///모든 폼 로딩
            Button_Select("S");
            Button_Select("D");
            Button_Select("M");
            Button_Select("C");
            Button_Select("H");
            Light.WindowState = FormWindowState.Normal;
            Light.Visible = false;
            Interface.WindowState = FormWindowState.Normal;
            Interface.Visible = false;
            ////////////////////////////////////////////////////////////////////////////
            ///
            ////////////////////////////////////////////////////////////////////////////
            ///로딩창 종료
            Splash_Loading.UpdateStatusText("STOP");
            int tickCount2 = Environment.TickCount;
            while (true)
            {
                if (Environment.TickCount - tickCount2 > 1000)
                {
                    break;
                }
            }
            Splash_Loading.CloseSplashScreen();
            ////////////////////////////////////////////////////////////////////////////
            this.WindowState = FormWindowState.Maximized;
        }

        private void Recipe_Change()
        {
            try
            {
                toolStripRECIPE.Text = COMMON.RECIPE;
                for (int LoopCount = 1; LoopCount < 9; LoopCount++)
                {
                    if (COMMON.U_Setting[LoopCount].Unit_Name != "")
                    {
                        Unit[LoopCount].Recipe_Load();
                    }
                }
                Model.Loading_OK();
                Setup.Loading_OK();
                Calibration.Loading_OK();
            }
            catch { }
        }

        private void Reset_Button_Click(object sender, EventArgs e)
        {
            if (!(COMMON.Current_Mode == Enum.Mode.Auto))
            {
                for (int LoopCount = 1; LoopCount < 9; LoopCount++)
                    if (Unit[LoopCount] != null)
                    {
                        Unit[LoopCount].Sequence.Unit_Reset();
                    }
            }
            else
            {
                ERROR Error = new ERROR(Enum.ERROR.Auto_Run);
                Error.ShowDialog();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Keys Key = keyData & ~(Keys.Shift | Keys.Control | Keys.Alt);
            switch(Key)
            {
                case Keys.F1:
                    if ((keyData & Keys.Control) != 0)
                    {
                        Interface_Display_On();
                    }
                    else
                        Button_Select("H");
                    break;
                case Keys.F2:
                    if ((keyData & Keys.Control) != 0)
                    {
                        Light_Display_On();
                    }
                    else
                        Button_Select("C");
                    break;
                case Keys.F3:
                    if ((keyData & Keys.Control) != 0)
                    { }
                    else
                        Button_Select("M");
                    break;
                case Keys.F4:
                    if ((keyData & Keys.Control) != 0)
                    { }
                    else
                        Button_Select("D");
                    break;
                case Keys.F5:
                    if ((keyData & Keys.Control) != 0)
                    { }
                    else
                        Button_Select("S");
                    break;
                case Keys.Escape:
                    Display_Cancel(new object());
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
