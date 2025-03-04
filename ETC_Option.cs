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
        static public string EQP = "CW SemiAuto Machine";




        static public int Retry_Limit = 0;
        static public int Mode = 2;                             // ALign Theta 연산에 사용할 갯수 4변 평균 2변 평균(장변) 1변은 소스상 선택
        static public int Interface_Offset = 0;





        static public bool Shuttle_Position_Use = false;           // True : Shttle 값으로 자재길이 H산출





        static public bool Mark_Test_Mode = false;           // True : Mark0번의 이미지로 얼라인
        static public bool Cog_Camera = false;          //True : Cognex 카메라연결 False : Eurosys및 CREVIS 카메라연결
        static public bool Enet = false;                //True : 이더넷 False : 광통신
        static public bool Simul_Calibration = false;







        static public Enum.Unit_Type[] Unit_Type = { Enum.Unit_Type.NON,
                                                            Enum.Unit_Type.NON, Enum.Unit_Type.NON, Enum.Unit_Type.NON, Enum.Unit_Type.NON,
                                                            Enum.Unit_Type.NON, Enum.Unit_Type.NON, Enum.Unit_Type.NON, Enum.Unit_Type.NON };


        static public Enum.Log_Type[] Log_Type = {  Enum.Log_Type.TO_Log,
                                                    Enum.Log_Type.TO_Log, Enum.Log_Type.TO_Log, Enum.Log_Type.TO_Log, Enum.Log_Type.TO_Log,
                                                    Enum.Log_Type.TO_Log, Enum.Log_Type.TO_Log, Enum.Log_Type.TO_Log, Enum.Log_Type.TO_Log };

        static public Enum.Motor_Type[] Motor_Type = { Enum.Motor_Type.NON,
                                                            Enum.Motor_Type.NON,Enum.Motor_Type.NON,Enum.Motor_Type.NON,Enum.Motor_Type.NON,
                                                            Enum.Motor_Type.NON,Enum.Motor_Type.NON,Enum.Motor_Type.NON,Enum.Motor_Type.NON};

        static public Enum.Calibration_Type[,] Calibration_Type = {
                                                        {   Enum.Calibration_Type.NON, // Target
                                                            Enum.Calibration_Type.NON,Enum.Calibration_Type.NON,Enum.Calibration_Type.NON,Enum.Calibration_Type.NON,
                                                            Enum.Calibration_Type.NON,Enum.Calibration_Type.NON,Enum.Calibration_Type.NON,Enum.Calibration_Type.NON},
                                                        {   Enum.Calibration_Type.NON, // COMMON
                                                            Enum.Calibration_Type.NON,Enum.Calibration_Type.NON,Enum.Calibration_Type.NON,Enum.Calibration_Type.NON,
                                                            Enum.Calibration_Type.NON,Enum.Calibration_Type.NON,Enum.Calibration_Type.NON,Enum.Calibration_Type.NON},
                                                        {   Enum.Calibration_Type.NON, // Object
                                                            Enum.Calibration_Type.NON,Enum.Calibration_Type.NON,Enum.Calibration_Type.NON,Enum.Calibration_Type.NON,
                                                            Enum.Calibration_Type.NON,Enum.Calibration_Type.NON,Enum.Calibration_Type.NON,Enum.Calibration_Type.NON} };
        static public int[] DA_UNIT = {0,
                                            0, 0, 0, 0,
                                            0, 0, 0, 0 };
        static public int[] UVW_R = {0,
                                            0, 0, 0, 0,
                                            0, 0, 0, 0};

        static public bool[] Second_Grab_First_Use = { false, false, false, false, false, false, false, false, false };

        static public string[] Light_Controler = {  "NON",
                                                    "NON", "NON", "NON", "NON", "NON" ,
                                                    "NON", "NON", "NON", "NON", "NON" ,
                                                    "NON", "NON", "NON", "NON", "NON" };

        static public int[] Light_Controler_Max_CH = { 0,
                                                    0, 0, 0, 0, 0 ,
                                                    0, 0, 0, 0, 0 ,
                                                    0, 0, 0, 0, 0 };

        static public int[] Light_Controler_val = { 255,
                                                    255, 255, 255, 255, 255 ,
                                                    255, 255, 255, 255, 255 ,
                                                    255, 255, 255, 255, 255 };
    }
}
