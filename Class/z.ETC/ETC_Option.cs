using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace T_Align
{
    static public class ETC_Option
    {
        static public string VER = "1.1.0.0"; //외부파일에서 읽어오지 않음
        static public int Auto_Login = 0;                      //미사용 : -1 사용 : Login Information 순서 참조   외부파일에서 읽어오지 않음
        static public Color Main_Color = Color.FromArgb(180, 180, 180);//외부파일에서 읽어오지 않음
        static public Color Sub_Color = Color.FromArgb(180, 180, 180);//외부파일에서 읽어오지 않음






        static public string EQP = "";




        static public int Retry_Limit = 0;
        static public int Mode = 0;                             // ALign Theta 연산에 사용할 갯수 4변 평균 2변 평균(장변) 1변은 소스상 선택
        static public int Interface_Offset = 0;





        static public bool Shuttle_Position_Use = false;           // True : Shttle 값으로 자재길이 H산출




        static public bool Live_Use = false;
        static public bool Mark_Test_Mode = false;           // True : Mark0번의 이미지로 얼라인
        static public bool Cog_Camera = false;          //True : Cognex 카메라연결 False : Eurosys및 CREVIS 카메라연결
        static public bool Enet = false;                //True : 이더넷 False : 광통신
        static public bool Simul_Calibration = false;







        static public Enum.Unit_Type[] Unit_Type = new Enum.Unit_Type[9];

        static public Enum.Log_Type[] Log_Type = new Enum.Log_Type[9];

        static public Enum.Motor_Type[] Motor_Type = new Enum.Motor_Type[9];

        static public Enum.Calibration_Type[,] Calibration_Type = new Enum.Calibration_Type[3, 9];
        static public int[] DA_UNIT = new int[9];
        static public int[] UVW_R = new int[9];

        static public bool[] Second_Grab_First_Use = { false, false, false, false, false, false, false, false, false };

        static public string[] Light_Controler = new string[11];

        static public int[] Light_Controler_Max_CH = new int[11];

        static public int[] Light_Controler_val = new int[11];
    }
}
