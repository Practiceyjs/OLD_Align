using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognex.VisionPro;
using System.Drawing;
using Cognex.VisionPro.Display;
using System.Windows.Forms;

namespace T_Align
{
    static public class COMMON
    {
        static public bool Program_Run = false;

        static public Main_Frame Frame;
        static public bool Login = false;
        static public string User_Name = "";
        static public bool Simul_Allow = false;
        static public Enum.Mode Current_Mode = Enum.Mode.Manual;
        static public string RECIPE = "YOON RECIPE";
        static public int RECIPE_NUM = 0;
        static public bool Simul_Run = false;

        static public string IP = "100.100.100.20";
        static public int PORT = 3001;
        static public int NETWORK = 2;
        static public int PLC_STATION = 1;
        static public int PC_STATION = 4;
        static public string Interface_Tpe = "";
        static public int Recive_Address = 0;
        static public int Send_Address = 0;

        static public bool Write_Bit = false;

        static public ToolStripLabel[] Unit_Labels;
        static public ToolStripLabel[] Cam_Labels;

        static public string[] CAMERA_IP = { "", "", "", "", "", "", "", "" };
        static public Enum.Camera_Rotate[] CAMERA_Rotate = { Enum.Camera_Rotate.NON, Enum.Camera_Rotate.NON, Enum.Camera_Rotate.NON, Enum.Camera_Rotate.NON, Enum.Camera_Rotate.NON, Enum.Camera_Rotate.NON, Enum.Camera_Rotate.NON, Enum.Camera_Rotate.NON };

        static public CogDisplay[] Live_Display = { null, null, null, null, null, null, null, null, null };
        static public CogDisplay[] Inspection_Display;
        static public Label Inspection_Tiltle;
        static public DataGridView Inspection_Data;
        static public ICogImage[] Grab_Image = { null, null, null, null, null, null, null, null, null };

        static public string[] Recipe_List = new string[1000];

        static public Extenal_Setting Extenal_Setting;

        static public Recipe_Select Recipe_Select;

        static public Number_KeyBoard Number_KeyBoard;

        static public KeyBoard KeyBoard;

        static public COMMON_Unit_Setting[] U_Setting = new COMMON_Unit_Setting[9];
        static public Light_Controler[] Light_Controlers = new Light_Controler[17];

        static public int[] Send_Data = new int[500];
        static public int[] Receive_Data = new int[500];

        static public void Rotate_Set(int _Cam, string _Rotate)
        {
            switch (_Rotate)
            {
                case "NON":
                    CAMERA_Rotate[_Cam] = Enum.Camera_Rotate.NON;
                    break;
                case "90":
                    CAMERA_Rotate[_Cam] = Enum.Camera_Rotate._90;
                    break;
                case "180":
                    CAMERA_Rotate[_Cam] = Enum.Camera_Rotate._180;
                    break;
                case "270":
                    CAMERA_Rotate[_Cam] = Enum.Camera_Rotate._270;
                    break;
                case "FLIP":
                    CAMERA_Rotate[_Cam] = Enum.Camera_Rotate.FLIP;
                    break;
                case "FLIP90":
                    CAMERA_Rotate[_Cam] = Enum.Camera_Rotate.FLIP90;
                    break;
                case "FLIP180":
                    CAMERA_Rotate[_Cam] = Enum.Camera_Rotate.FLIP180;
                    break;
                case "FLIP270":
                    CAMERA_Rotate[_Cam] = Enum.Camera_Rotate.FLIP270;
                    break;
            }
        }

        static public string Rotate_Get(int _Cam, Enum.Camera_Rotate _Rotate)
        {
            switch (_Rotate)
            {
                case Enum.Camera_Rotate.NON:
                    return "NON";
                case Enum.Camera_Rotate._90:
                    return "90";
                case Enum.Camera_Rotate._180:
                    return "180";
                case Enum.Camera_Rotate._270:
                    return "270";
                case Enum.Camera_Rotate.FLIP:
                    return "FLIP";
                case Enum.Camera_Rotate.FLIP90:
                    return "FLIP90";
                case Enum.Camera_Rotate.FLIP180:
                    return "FLIP180";
                case Enum.Camera_Rotate.FLIP270:
                    return "FLIP270";
                default: return "";
            }
        }
        static public bool CTQ_Use = false;
        static public string CTQ_COMMAND = "";
        static public bool CTQ_Zero_Align = false;
        static public bool CTQ_Random_Align = false;
        static public int CTQ_Zero_Count = 10;
    }
}
