using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace T_Align
{
    public partial class CH_Control : UserControl
    {
        int Value = 0;
        int CH = 0;
        int Set_Value = 0;
        Enum.Unit Unit = Enum.Unit.Common;
        Enum.Serial Serial = Enum.Serial.NON;

        public CH_Control()
        {
            InitializeComponent();
            timer1.Start();
        }

        public void Unit_Load(Enum.Unit _Unit, Enum.Serial _Port, int _CH,int _Set_Value, string _CH_Name)
        {
            Unit = _Unit;
            Serial = _Port;
            CH = _CH;
            Set_Value = _Set_Value;
            trackBar1.Value = Set_Value;
            label2.Text = Set_Value.ToString();
            CH_Name_Label.Text = _CH_Name;
            trackBar1.Maximum = ETC_Option.Light_Controler_val[(int)_Port];
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (COMMON.Number_KeyBoard != null)
                COMMON.Number_KeyBoard.Close();
            COMMON.Number_KeyBoard = new Number_KeyBoard(Enum.Key_Type.Int_Positive);
            COMMON.Number_KeyBoard.SendMsg += new Number_KeyBoard.SendMsgDele(KetBoard_SendMsg);
            COMMON.Number_KeyBoard.TopMost = true;
            COMMON.Number_KeyBoard.Show();
        }

        private void KetBoard_SendMsg(string _Msg)
        {
            switch(_Msg)
            {
                case "C":
                    Value = 0;
                    break;
                case "←":
                    Value = Value / 10;
                    break;
                case "ㅡ":
                    Value = -Value;
                    break;
                case "OK":
                    COMMON.Light_Controlers[(int)Serial].Manual_Light(CH, Value);
                    break;
                default:
                    Value = Value * 10 + Convert.ToInt32(_Msg);
                    break;
            }
            Value = Value_Limit_Check(Value);
            trackBar1.Value = Value;
            label2.Text = Value.ToString();
            Current_Check();
        }

        private int Value_Limit_Check(int _Value)
        {
            try
            {
                if (_Value < 0)
                    _Value = 0;
                if (_Value > ETC_Option.Light_Controler_val[(int)Serial])
                    _Value = ETC_Option.Light_Controler_val[(int)Serial];
            }
            catch { }

            return _Value;
        }

        private void Current_Check()
        {
            if (Value == Set_Value)
                label2.ForeColor = Color.White;
            else
                label2.ForeColor = Color.Red;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Value = trackBar1.Value;
            label2.Text = Value.ToString();
            Current_Check();
            COMMON.Light_Controlers[(int)Serial].Manual_Light(CH, Value);
        }

        private void Save_Button_Click(object sender, EventArgs e)
        {
            Set_Value = Value;
            Current_Check();
            COMMON.Light_Controlers[(int)Serial].Value_Save(CH, Set_Value);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                double Tic = 0;
                if (COMMON.Light_Controlers[(int)Serial].Start_Light_Time[CH]!=default(DateTime))
                    Tic = (DateTime.Now - COMMON.Light_Controlers[(int)Serial].Start_Light_Time[CH]).TotalSeconds;
                DateTime Light = COMMON.Light_Controlers[(int)Serial].Life_Light_Time[CH].AddSeconds(Tic);
                int Year = (Light.Year - 1) % 100;
                int Month = Light.Month - 1;
                int Day = Light.Day - 1;
                label3.Text = Year.ToString("00") + "." + Month.ToString("00") + "." + Day.ToString("00") + "\n" + Light.ToString("HH:mm:ss");
            }
            catch
            {

            }
        }
    }
}
