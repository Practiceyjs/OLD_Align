using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Align
{
    public class ClassAlign
    {
        int R = 48;    //UVW 회전중심부터 모터까지

        ClassType Type;

        public void Align_Set(ClassType _Type)
        {
            Type = _Type;
        }

        //Calibration시 가로방향 Master Theta 계산
        public double Cal_Theta_W(double M_X1, double M_Y1, double M_X2, double M_Y2, double C_X1, double C_Y1, double C_X2, double C_Y2, ref double Width)
        {
            Width = Math.Sqrt(Math.Pow(((C_X1 - M_X1) - (C_X2 - M_X2)), 2) + Math.Pow((C_Y1 - C_Y2), 2));
            double T = (M_Y1 - M_Y2) / Width;
            return -Math.Atan(T) * 180 / Math.PI;
        }

        //Calibration시 세로방향 Master Theta 계산
        public double Cal_Theta_H(double M_X1, double M_Y1, double M_X2, double M_Y2, double C_X1, double C_Y1, double C_X2, double C_Y2, ref double Height)
        {
            Height = Math.Sqrt(Math.Pow(((C_X1 - M_X1) - (C_X2 - M_X2)), 2) + Math.Pow((C_Y1 - C_Y2), 2));
            double T = (M_X1 - M_X2) / Height;
            return -Math.Atan(T) * 180 / Math.PI;
        }

        //Align시 가로방향 Theta 계산
        public double Align_Theta_W(double _Y1, double _Y2, double Width)
        {
            double T = (_Y1 - _Y2) / Width;
            return -Math.Asin(T) * 180 / Math.PI;
        }

        public double Align_Width(ClassType Type, int Num1, int Num2, double Cam_Teaching)
        {
            //double _X = Type.Cam_Current - ((Type.Align_X[Num1] - Type.Align_X[Num2]) - Cam_Teaching) * (( +) / 2);
            //기존
            //double _X = ((Type.Cam_Current - Cam_Teaching)*2) + Type.Mark_To_Mark + ((Type.Display[Num1].Image.Width - Type.Align_X[Num1]) * Type.Model[Num1].resolution_X) + (Type.Align_X[Num2] * Type.Model[Num2].resolution_X);
            //21.02.15 L-Check 관련 수정
            double _X = ((Type.Cam_Current - Cam_Teaching)) + Type.Mark_To_Mark + ((Type.Display[Num1].Image.Width - Type.Align_X[Num1]) * Type.Model[Num1].resolution_X) + (Type.Align_X[Num2] * Type.Model[Num2].resolution_X);
            double _Y = (Type.Align_Y[Num1] - Type.Align_Y[Num2]) * ((Type.Model[Num1].resolution_Y + Type.Model[Num2].resolution_Y) / 2);
            return Math.Sqrt(Math.Pow(_X, 2) + Math.Pow(_Y, 2));
        }

        public double Cal_Width(ClassType Type, int Num1, int Num2, double Cam_Teaching)
        {
            double _X = ((Type.Cam_Current - Cam_Teaching) * 2) + Type.Mark_To_Mark + ((Type.Cal_Display[Num1%2].Image.Width - Type.Model[Num1].Master_X + Type.Model[Num2].Master_X)) * ((Type.Model[Num1].resolution_X + Type.Model[Num2].resolution_X) / 2);
            double _Y = (Type.Model[Num1].Master_Y - Type.Model[Num2].Master_Y) * ((Type.Model[Num1].resolution_Y + Type.Model[Num2].resolution_Y) / 2);
            return Math.Sqrt(Math.Pow(_X, 2) + Math.Pow(_Y, 2));
        }

        //21.02.09  Align_Height 생성 (1축만 사용)
        //Y축이 두개 일 때
        public double Align_Height_2(ClassType Type, int Num1, int Num2, int Teaching_1, int Teaching_2)
        {
            double _X;
            double _Y;
            if (!Type.One_Camera_2Point_Use)
            {
                _X = (Type.Model[Num1].Master_X - Type.Model[Num2].Master_X) * ((Type.Model[Num1].resolution_X + Type.Model[Num2].resolution_X) / 2);
                _Y = Math.Abs(Math.Abs(Teaching_1) - Math.Abs(Teaching_2)) + (((Type.Display[Num1].Image.Height / 2) - Type.Align_Y[Num1]) * Type.Model[Num1].resolution_Y) + ((Type.Align_Y[Num2] - (Type.Display[Num2].Image.Height / 2)) * Type.Model[Num2].resolution_Y);
            }
            else
            {
                _X = (Type.Model[Num1].Master_X - Type.Model[Num2].Master_X) * ((Type.Model[Num1].resolution_X + Type.Model[Num2].resolution_X) / 2);
                _Y = Math.Abs(Math.Abs(Teaching_1) - Math.Abs(Teaching_2)) + (((Type.Display[0].Image.Height / 2) - Type.Align_Y[Num1]) * Type.Model[Num1].resolution_Y) + ((Type.Align_Y[Num1] - (Type.Display[2].Image.Height / 2)) * Type.Model[Num2].resolution_Y);
            }
            
            return Math.Sqrt(Math.Pow(_Y, 2) + Math.Pow(_X, 2));
        }

        //Y축이 1개 일때
        public double Align_Height(ClassType Type, int Num1, int Num2, int Teaching_1)
        {
            double _X;
            double _Y;
            if (!Type.One_Camera_2Point_Use)
            {
                _X = (Type.Model[Num1].Master_X - Type.Model[Num2].Master_X) * ((Type.Model[Num1].resolution_X + Type.Model[Num2].resolution_X) / 2);
                _Y = Math.Abs(Math.Abs(Teaching_1)) + (((Type.Display[Num1].Image.Height / 2) - Type.Align_Y[Num1]) * Type.Model[Num1].resolution_Y) + ((Type.Align_Y[Num2] - (Type.Display[Num2].Image.Height / 2)) * Type.Model[Num2].resolution_Y);
            }
            else
            {
                _X = (Type.Model[Num1].Master_X - Type.Model[Num2].Master_X) * ((Type.Model[Num1].resolution_X + Type.Model[Num2].resolution_X) / 2);
                _Y = Math.Abs(Math.Abs(Teaching_1)) + (((Type.Display[0].Image.Height / 2) - Type.Align_Y[Num1]) * Type.Model[Num1].resolution_Y) + ((Type.Align_Y[Num1] - (Type.Display[2].Image.Height / 2)) * Type.Model[Num2].resolution_Y);
            }

            return Math.Sqrt(Math.Pow(_Y, 2) + Math.Pow(_X, 2));
        }

        //Y축이 2개 일 때
        public double Cal_Height_2(ClassType Type, int Num1, int Num2, int Teaching_1, int Teaching_2)
        {
            double _X = (Type.Model[Num1].Master_X - Type.Model[Num2].Master_X) * ((Type.Model[Num1].resolution_X + Type.Model[Num2].resolution_X) / 2);
            double _Y = Math.Abs(Math.Abs(Teaching_1) - Math.Abs(Teaching_2)) + (((Type.Cal_Display[Num1%2].Image.Height/2) - Type.Model[Num1].Master_Y) * Type.Model[Num1].resolution_Y) + ((Type.Model[Num2].Master_Y-(Type.Cal_Display[Num2 % 2].Image.Height / 2)) * Type.Model[Num2].resolution_Y);
            return Math.Sqrt(Math.Pow(_Y, 2) + Math.Pow(_X, 2));
        }

        //Y축이 1개 일 때
        public double Cal_Height(ClassType Type, int Num1, int Num2, int Teaching_1)
        {
            double _X = (Type.Model[Num1].Master_X - Type.Model[Num2].Master_X) * ((Type.Model[Num1].resolution_X + Type.Model[Num2].resolution_X) / 2);
            double _Y = Math.Abs(Math.Abs(Teaching_1)) + (((Type.Cal_Display[Num1 % 2].Image.Height / 2) - Type.Model[Num1].Master_Y) * Type.Model[Num1].resolution_Y) + ((Type.Model[Num2].Master_Y - (Type.Cal_Display[Num2 % 2].Image.Height / 2)) * Type.Model[Num2].resolution_Y);
            return Math.Sqrt(Math.Pow(_Y, 2) + Math.Pow(_X, 2));
        }

        //Align시 세로방향 Theta 계산
        public double Align_Theta_H(double _X1, double _X2, double Height)
        {
            double T = (_X1 - _X2) / Height;
            return -Math.Asin(T) * 180 / Math.PI;
        }

        //Theta 보정시 UVW 이동량 계산
        public double UVW_Theta(double _theta)
        {
            //21.02.15
            //중심반경 *  cos(요구각도 + 중심각도 + 초기위치 ) - 중심반경 * sin (중심각도 + 초기 위치 각도)
            //return (R * Math.Cos(Deg_To_Radians(_theta + 270 + 0))) - (R * Math.Cos(Deg_To_Radians(270 + 0)));
            return (R * Math.Cos(Deg_To_Radians(_theta + 90 + 0))) - (R * Math.Cos(Deg_To_Radians(90 + 0)));
        }

        public double UVW_Theta_XYT(double _X, double _Y, double _T, ref double x, ref double y1, ref double y2)
        {
            
        }

        private double Deg_To_Radians(double Deg)
        {
            return Deg * Math.PI / 180;
        }
    }
}
