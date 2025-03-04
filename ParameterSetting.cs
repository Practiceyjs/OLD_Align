using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DES;
using T_Align;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Parameter_Setting
{
    public partial class ParameterSetting : Form
    {
        Microsoft.Office.Interop.Excel.Application Application = new Microsoft.Office.Interop.Excel.Application();
        Workbook Workbook;
        Worksheet Worksheet;
        Range range;
        int Unit_Number = 1;
        string[] Interface_Data = null;
        public ParameterSetting()
        {
            InitializeComponent();
            string[] Parameter = DES.DES.Parameser_Get();
            if (Parameter != null)
            {
                EQP_IP_Textbox.Text = Parameter[0];
                Retry_Count_Cbx.Text = Parameter[1];
                AlignMode_Cbx.Text = Parameter[2];
                Interface_Offset_Nmr.Value = int.Parse(Parameter[3]);
                Shuttle_Use_Chk.Checked = bool.Parse(Parameter[4]);
                CogCamera_Chk.Checked = bool.Parse(Parameter[5]);
                Enet_Chk.Checked = bool.Parse(Parameter[6]);
                Mark_Test_Chk.Checked = bool.Parse(Parameter[7]);
                Simul_Calibration_Chk.Checked = bool.Parse(Parameter[8]);
                Channel_Cbx.Text = Parameter[9];
                Load_Unit_Parameter(Slice(Parameter, 10));
                Interface_Data = Slice(Parameter, 98);
                Unit_Parameter_Ref();
                Channel_Panel.Visible = !Enet_Chk.Checked;
            }
            else
                MessageBox.Show("Parameter가 손상되었습니다.");
        }

        public string[] Slice(string[] source, int start)
        {
            int len = source.Length - start;

            string[] res = new string[len];
            for (int i = 0; i < len; i++)
            {
                res[i] = source[i + start];
            }
            return res;
        }

        private void Unit_Parameter_Ref()
        {
            Unit_Type_Cbx.Text = ETC_Option.Unit_Type[Unit_Number].ToString();
            DA_Unit_Cbx.Text = ETC_Option.DA_UNIT[Unit_Number].ToString();
            Motor_Type_Cbx.Text = ETC_Option.Motor_Type[Unit_Number].ToString();
            UVW_R_Nmr.Value = (decimal)ETC_Option.UVW_R[Unit_Number];
            T_Cal_Type_Cbx.Text = ETC_Option.Calibration_Type[0, Unit_Number].ToString();
            O_Cal_Type_Cbx.Text = ETC_Option.Calibration_Type[2, Unit_Number].ToString();
            Second_Grab_First_Chk.Checked = ETC_Option.Second_Grab_First_Use[Unit_Number];
            Log_Type_Cbx.Text = ETC_Option.Log_Type[Unit_Number].ToString();
            Controler_Cbx.Text = ETC_Option.Light_Controler[Unit_Number].ToString();
            Max_CH_Nmr.Value = (decimal)ETC_Option.Light_Controler_Max_CH[Unit_Number];
            Max_Value_Nmr.Value = (decimal)ETC_Option.Light_Controler_val[Unit_Number];
        }

        private void Save_Button_Click(object sender, EventArgs e)
        {
            string[] Parameter = new string[10];
            Parameter[0] = EQP_IP_Textbox.Text;
            Parameter[1] = Retry_Count_Cbx.Text;
            Parameter[2] = AlignMode_Cbx.Text;
            Parameter[3] = Interface_Offset_Nmr.Value.ToString();
            Parameter[4] = Shuttle_Use_Chk.Checked.ToString();
            Parameter[5] = CogCamera_Chk.Checked.ToString();
            Parameter[6] = Enet_Chk.Checked.ToString();
            Parameter[7] = Mark_Test_Chk.Checked.ToString();
            Parameter[8] = Simul_Calibration_Chk.Checked.ToString();
            Parameter[9] = Channel_Cbx.Text;

            var list = new List<string>();
            list.AddRange(Parameter);
            list.AddRange(Save_Unit_Parameter());
            //list.AddRange(Interface_Data);
            if (list.Count == 717)
            {
                string[] All_Parameter = list.ToArray();

                DES.DES.Parameser_Set(All_Parameter);
                this.Close();
            }
            else
                MessageBox.Show("Paremater Error");
        }

        private string[] Save_Unit_Parameter()
        {
            string[] Parameter = new string[11 * 8];
            for (int LoopCount = 0; LoopCount < 8; LoopCount++)
            {
                Parameter[LoopCount * 11 + 0] = ETC_Option.Unit_Type[LoopCount + 1].ToString();
                Parameter[LoopCount * 11 + 1] = ETC_Option.DA_UNIT[LoopCount + 1].ToString();
                Parameter[LoopCount * 11 + 2] = ETC_Option.Motor_Type[LoopCount + 1].ToString();
                Parameter[LoopCount * 11 + 3] = ETC_Option.UVW_R[LoopCount + 1].ToString();
                Parameter[LoopCount * 11 + 4] = ETC_Option.Calibration_Type[0, LoopCount + 1].ToString();
                Parameter[LoopCount * 11 + 5] = ETC_Option.Calibration_Type[2, LoopCount + 1].ToString();
                Parameter[LoopCount * 11 + 6] = ETC_Option.Second_Grab_First_Use[LoopCount + 1].ToString();
                Parameter[LoopCount * 11 + 7] = ETC_Option.Log_Type[LoopCount + 1].ToString();
                Parameter[LoopCount * 11 + 8] = ETC_Option.Light_Controler[LoopCount + 1].ToString();
                Parameter[LoopCount * 11 + 9] = ETC_Option.Light_Controler_Max_CH[LoopCount + 1].ToString();
                Parameter[LoopCount * 11 + 10] = ETC_Option.Light_Controler_val[LoopCount + 1].ToString();
            }
            return Parameter;
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

        private void Unit_Type_Cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            ETC_Option.Unit_Type[Unit_Number] = T_Align.Enum.Unit_Type_Parse(Unit_Type_Cbx.Text);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Unit_Number = int.Parse(comboBox1.Text);
            Unit_Parameter_Ref();
        }

        private void DA_Unit_Cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            ETC_Option.DA_UNIT[Unit_Number] = int.Parse(DA_Unit_Cbx.Text);
        }

        private void Motor_Type_Cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            ETC_Option.Motor_Type[Unit_Number] = T_Align.Enum.Motor_Type_Parse(Motor_Type_Cbx.Text);
        }

        private void UVW_R_Nmr_ValueChanged(object sender, EventArgs e)
        {
            ETC_Option.UVW_R[Unit_Number] = (int)UVW_R_Nmr.Value;
        }

        private void T_Cal_Type_Cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            ETC_Option.Calibration_Type[0, Unit_Number] = T_Align.Enum.Calibration_Type_Parse(T_Cal_Type_Cbx.Text);
        }

        private void O_Cal_Type_Cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            ETC_Option.Calibration_Type[2, Unit_Number] = T_Align.Enum.Calibration_Type_Parse(O_Cal_Type_Cbx.Text);
        }

        private void Second_Grab_First_Chk_CheckedChanged(object sender, EventArgs e)
        {
            ETC_Option.Second_Grab_First_Use[Unit_Number] = Second_Grab_First_Chk.Checked;
        }

        private void Log_Type_Cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            ETC_Option.Log_Type[Unit_Number] = T_Align.Enum.Log_Type_Parse(Log_Type_Cbx.Text);
        }

        private void Controler_Cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            ETC_Option.Light_Controler[Unit_Number] = Controler_Cbx.Text;
        }

        private void Max_CH_Nmr_ValueChanged(object sender, EventArgs e)
        {
            ETC_Option.Light_Controler_Max_CH[Unit_Number] = (int)Max_CH_Nmr.Value;
        }

        private void Max_Value_Nmr_ValueChanged(object sender, EventArgs e)
        {
            ETC_Option.Light_Controler_val[Unit_Number] = (int)Max_Value_Nmr.Value;
        }

        private void label15_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Workbook = Application.Workbooks.Open(openFileDialog.FileName);
                Worksheet = Workbook.Sheets[1];
                range = Worksheet.UsedRange;
                Interface_Data = Excel_Interface_Load();
                richTextBox1.Text = openFileDialog.SafeFileName;
                DeleteObject(range); DeleteObject(Worksheet); DeleteObject(Workbook); Application.Quit(); DeleteObject(Application);
            }
            openFileDialog.Dispose();
        }

        private string[] Excel_Interface_Load()
        {
            string Address_Type = "";
            string Read_Address = "";
            string Write_Address = "";
            string[] Unit_Name = new string[8];
            string[] Target_Name = new string[8];
            string[] Object_Name = new string[8];
            string[] Read_Bit_Name = new string[8 * 16];
            string[] Write_Bit_Name = new string[8 * 16];
            string[] Read_Word_Name = new string[8 * 15];
            string[] Write_Word_Name = new string[8 * 15];
            string[] Target_Light_Name = new string[8 * 6];
            string[] Object_Light_Name = new string[8 * 6];
            Address_Type = (range.Cells[1, 4] as Range).Value2.ToString();
            Read_Address = (range.Cells[1, 5] as Range).Value2.ToString();
            Write_Address = (range.Cells[1, 9] as Range).Value2.ToString();
            for (int i = 2; i <= 9; ++i)
            {
                Unit_Name[i - 2] = (range.Cells[i, 3] as Range).Value2.ToString();
                try
                {
                    Target_Name[i - 2] = (range.Cells[i, 13] as Range).Value2.ToString();
                }
                catch { Target_Name[i - 2] = ""; }
                try
                {
                    Object_Name[i - 2] = (range.Cells[i, 16] as Range).Value2.ToString();
                }
                catch { Object_Name[i - 2] = ""; }
            }
            for (int Unit = 0; Unit < 8; Unit++)
            {
                for (int i = 0; i < 16; ++i)
                {
                    try
                    {
                        Read_Bit_Name[(Unit * 16) + i] = (range.Cells[Unit * 17 + 29 + i, 3] as Range).Value2.ToString();
                    }
                    catch { Read_Bit_Name[(Unit * 16) + i] = ""; }
                    try
                    {
                        Write_Bit_Name[(Unit * 16) + i] = (range.Cells[Unit * 17 + 29 + i, 7] as Range).Value2.ToString();
                    }
                    catch { Write_Bit_Name[(Unit * 16) + i] = ""; }
                }
            }
            for (int Unit = 0; Unit < 8; Unit++)
            {
                for (int i = 0; i < 15; i++)
                {
                    try
                    {
                        Read_Word_Name[(Unit * 15) + i] = (range.Cells[Unit * 30 + 165 + i * 2, 3] as Range).Value2.ToString();
                    }
                    catch { Read_Word_Name[(Unit * 15) + i] = ""; }
                    try
                    {
                        Write_Word_Name[(Unit * 15) + i] = (range.Cells[Unit * 30 + 165 + i * 2, 7] as Range).Value2.ToString();
                    }
                    catch { Write_Word_Name[(Unit * 15) + i] = ""; }
                }
            }
            for (int Unit = 0; Unit < 8; Unit++)
            {
                for (int i = 0; i < 6; i++)
                {
                    try
                    {
                        Target_Light_Name[(Unit * 6) + i] = (range.Cells[Unit * 17 + 29 + i, 13] as Range).Value2.ToString();
                    }
                    catch { Target_Light_Name[(Unit * 6) + i] = ""; }
                    try
                    {
                        Object_Light_Name[(Unit * 6) + i] = (range.Cells[Unit * 17 + 29 + i, 16] as Range).Value2.ToString();
                    }
                    catch { Object_Light_Name[(Unit * 6) + i] = ""; }
                }
            }
            var list = new List<string>();
            list.Add(Address_Type);
            list.Add(Read_Address);
            list.Add(Write_Address);
            list.AddRange(Unit_Name);
            list.AddRange(Target_Name);
            list.AddRange(Object_Name);
            list.AddRange(Read_Bit_Name);
            list.AddRange(Write_Bit_Name);
            list.AddRange(Read_Word_Name);
            list.AddRange(Write_Word_Name);
            list.AddRange(Target_Light_Name);
            list.AddRange(Object_Light_Name);
            return list.ToArray();
        }
        private void DeleteObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch(Exception ex)
            {
                obj = null;
                MessageBox.Show("메모리 할당을 해제하는 중 문제가 발생하였습니다." + ex.ToString(), "경고!");
            }
            finally
            {
                GC.Collect();
            }
        }

        private void Enet_Chk_CheckedChanged(object sender, EventArgs e)
        {
            Channel_Panel.Visible = !Enet_Chk.Checked;
        }
    }
}
