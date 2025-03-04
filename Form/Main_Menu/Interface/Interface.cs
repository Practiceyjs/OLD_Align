using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace T_Align
{
    public partial class Interface : Form
    {
        public delegate void CancelDele(object _null);
        public event CancelDele cancel;
        Button[] Unit_Button;

        Enum.Unit Current_Unit = Enum.Unit.Unit1;
        Thread Display_Thread;
        public Interface()
        {
            InitializeComponent();
            Unit_Button = new Button[] { Button_Unit1, Button_Unit2, Button_Unit3, Button_Unit4, Button_Unit5, Button_Unit6, Button_Unit7, Button_Unit8 };
            for (int LoopCount = 0; LoopCount < 8; LoopCount++)
                Unit_Button[LoopCount].Text = COMMON.U_Setting[LoopCount + 1].Unit_Name;
            Button_Select((int)Current_Unit);
            Display_Thread = new Thread(Display_Logic);
        }

        public void Loading_OK()
        {
            Display_Thread.Start();
            for (int LoopCount = 1; LoopCount < 9; LoopCount++)
            {
                if (COMMON.Frame.Unit[LoopCount] == null)
                    Unit_Button[LoopCount - 1].Visible = false;
            }
        }

        public void Thread_Stop()
        {
            Display_Thread.Join();
        }

        private void Display_Logic()
        {
            while(true)
            {
                if (!COMMON.Program_Run)
                    break;
                Bit_OnOff(PLC_Alive, (COMMON.Receive_Data[0] & 0x0001) == 0x0001);
                Bit_OnOff(PC_Alive, (COMMON.Send_Data[0] & 0x0001) == 0x0001);
                Bit_OnOff(PLC_Auto, (COMMON.Receive_Data[0] & 0x0002) == 0x0002);
                Bit_OnOff(PC_Auto, (COMMON.Send_Data[0] & 0x0002) == 0x0002);
                unit_Interface1.Send_bit(COMMON.Send_Data[(int)Current_Unit]);
                unit_Interface1.Recive_bit(COMMON.Receive_Data[(int)Current_Unit]);
                unit_Interface2.Send_data((int)Current_Unit, COMMON.Send_Data);
                unit_Interface2.Recive_data((int)Current_Unit, COMMON.Receive_Data);
                Thread.Sleep(100);
            }
        }

        private void Bit_OnOff(Label control,bool On_Off)
        {
            if(On_Off)
            {
                control.BackColor = Color.SkyBlue;
            }
                else
            {
                control.BackColor = Color.Black;
            }
        }

        private void Unit_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Current_Unit = (Enum.Unit)Convert.ToInt32(button.Name.Substring(button.Name.Length - 1, 1));
            Button_Select((int)Current_Unit);
        }

        private void Button_Select(int Select_Number)
        {
            for(int LoopCount=0;LoopCount < 8; LoopCount++)
            {
                Unit_Button[LoopCount].Image = Properties.Resources.Unit;
                Unit_Button[LoopCount].ForeColor = Color.White;
            }
            Unit_Button[Select_Number - 1].Image = Properties.Resources.Unit_Select;
            Unit_Button[Select_Number - 1].ForeColor = Color.Cyan;
            unit_Interface1.Unit_Load(Select_Number);
            unit_Interface2.Unit_Load(Select_Number);
            unit_Interface1.Bit_Name_Set(COMMON.U_Setting[(int)Current_Unit].Bit_Recive_Name, COMMON.U_Setting[(int)Current_Unit].Bit_Send_Name);
            unit_Interface2.Word_Name_Set(COMMON.U_Setting[(int)Current_Unit].Word_Recive_Name, COMMON.U_Setting[(int)Current_Unit].Word_Send_Name);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Keys Key = keyData & ~(Keys.Shift | Keys.Control | Keys.Alt);
            switch (Key)
            {
                case Keys.Escape:
                    cancel(new object());
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
