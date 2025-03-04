using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T_Align
{
    public partial class Unit_Interface : UserControl
    {
        Label[] L_Labels;
        Label[] R_Labels;
        Label[] L_Result_Labels;
        Label[] R_Result_Labels;

        bool[] On_Off;

        public bool Simul_On = false;
        public int Unit_Num;
        public void Unit_Load(int _Unit_Num)
        {
            Unit_Num = _Unit_Num;
        }

        public Unit_Interface()
        {
            On_Off = new bool[16] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
            InitializeComponent();
            L_Labels = new Label[] { Name_Label1, Name_Label2, Name_Label3, Name_Label4, Name_Label5, Name_Label6, Name_Label7, Name_Label8, Name_Label9, Name_Label10, Name_Label11, Name_Label12, Name_Label13, Name_Label14, Name_Label15, Name_Label16 };
            R_Labels = new Label[] { Name_Label17, Name_Label18, Name_Label19, Name_Label20, Name_Label21, Name_Label22, Name_Label23, Name_Label24, Name_Label25, Name_Label26, Name_Label27, Name_Label28, Name_Label29, Name_Label30, Name_Label31, Name_Label32 };
            L_Result_Labels = new Label[] { Result_Label1, Result_Label2, Result_Label3, Result_Label4, Result_Label5, Result_Label6, Result_Label7, Result_Label8, Result_Label9, Result_Label10, Result_Label11, Result_Label12, Result_Label13, Result_Label14, Result_Label15, Result_Label16 };
            R_Result_Labels = new Label[] { Result_Label17, Result_Label18, Result_Label19, Result_Label20, Result_Label21, Result_Label22, Result_Label23, Result_Label24, Result_Label25, Result_Label26, Result_Label27, Result_Label28, Result_Label29, Result_Label30, Result_Label31, Result_Label32 };
        }

        public void Bit_Name_Set(string[] L_Name, string[] R_Name)
        {
            for (int i = 0; i < 16; i++)
            {
                if (L_Labels[i].InvokeRequired)
                {
                    L_Labels[i].Invoke((MethodInvoker)delegate
                    {
                        L_Labels[i].Text = L_Name[i];
                    });
                }
                else
                {
                    L_Labels[i].Text = L_Name[i];
                }
            }
            for (int i = 0; i < 16; i++)
            {
                if (R_Labels[i].InvokeRequired)
                {
                    R_Labels[i].Invoke((MethodInvoker)delegate
                    {
                        R_Labels[i].Text = R_Name[i];
                    });
                }
                else
                {
                    R_Labels[i].Text = R_Name[i];
                }
            }
        }

        public void Word_Name_Set(string[] L_Name, string[] R_Name)
        {
            for (int i = 0; i < 15; i++)
            {
                if (L_Labels[i].InvokeRequired)
                {
                    L_Labels[i].Invoke((MethodInvoker)delegate
                    {
                        L_Labels[i].Text = L_Name[i];
                    });
                }
                else
                {
                    L_Labels[i].Text = L_Name[i];
                }
            }
            for (int i = 0; i < 15; i++)
            {
                if (R_Labels[i].InvokeRequired)
                {
                    R_Labels[i].Invoke((MethodInvoker)delegate
                    {
                        R_Labels[i].Text = R_Name[i];
                    });
                }
                else
                {
                    R_Labels[i].Text = R_Name[i];
                }
            }
            if (L_Labels[15].InvokeRequired)
            {
                L_Labels[15].Invoke((MethodInvoker)delegate
                {
                    L_Labels[15].Text = "";
                });
            }
            else
            {
                L_Labels[15].Text = "";
            }
            if (R_Labels[15].InvokeRequired)
            {
                R_Labels[15].Invoke((MethodInvoker)delegate
                {
                    R_Labels[15].Text = "";
                });
            }
            else
            {
                R_Labels[15].Text = "";
            }
        }

        public void Name_Set(string[] L_Name, string[] R_Name, bool Data_Use)
        {
            for (int i = 0; i < 16; i++)
            {
                if (L_Labels[i].InvokeRequired)
                {
                    L_Labels[i].Invoke((MethodInvoker)delegate
                    {
                        L_Labels[i].Text = L_Name[i];
                    });
                }
                else
                {
                    L_Labels[i].Text = L_Name[i];
                }
            }
            for (int i = 0; i < 16; i++)
            {
                if (R_Labels[i].InvokeRequired)
                {
                    R_Labels[i].Invoke((MethodInvoker)delegate
                    {
                        R_Labels[i].Text = R_Name[i];
                    });
                }
                else
                {
                    R_Labels[i].Text = R_Name[i];
                }
            }
            if (Data_Use)
            {
                for (int i = 0; i < 16; i++)
                {
                    if (L_Result_Labels[i].InvokeRequired)
                    {
                        L_Result_Labels[i].Invoke((MethodInvoker)delegate
                        {
                            L_Result_Labels[i].Text = "0.000";
                        });
                    }
                    else
                    {
                        L_Result_Labels[i].Text = "0.000";
                    }
                }
                for (int i = 0; i < 16; i++)
                {
                    if (R_Result_Labels[i].InvokeRequired)
                    {
                        R_Result_Labels[i].Invoke((MethodInvoker)delegate
                        {
                            R_Result_Labels[i].Text = "0.000";
                        });
                    }
                    else
                    {
                        R_Result_Labels[i].Text = "0.000";
                    }
                }
            }
        }

        public void Recive_bit(int _Data)
        {
            bool[] Receive = new bool[20];

            Receive = ClassInterface.Bit_Data(_Data);
            for (int i = 0; i < 16; i++)
            {
                if (Receive[i])
                    L_Result_Labels[i].BackColor = Color.SkyBlue;
                else
                    L_Result_Labels[i].BackColor = Color.Black;
            }

        }

        public void Send_bit(int _Data)
        {
            bool[] Receive = new bool[20];

            Receive = ClassInterface.Bit_Data(_Data);
            for (int i = 0; i < 16; i++)
            {
                if (Receive[i])
                    R_Result_Labels[i].BackColor = Color.SkyBlue;
                else
                    R_Result_Labels[i].BackColor = Color.Black;
            }

        }

        public void Send_data(int Index, int[] _Data)
        {
            Index--;
            for (int i = 0; i < 15; i++)
            {
                double Data = ((double)((Int32)_Data[Index * 30 + 10 + (i * 2) + ETC_Option.Interface_Offset] | (Int32)(_Data[Index * 30 + 10 + (i * 2 + 1) + ETC_Option.Interface_Offset] << 16)) / 10000);

                if (R_Labels[i].Text != "")
                {
                    if (R_Result_Labels[i].InvokeRequired)
                    {
                        R_Result_Labels[i].Invoke((MethodInvoker)delegate
                        {
                            R_Result_Labels[i].Text = Data.ToString("0.000");
                        });
                    }
                    else
                    {
                        R_Result_Labels[i].Text = Data.ToString("0.000");
                    }
                }
                else
                {
                    if (R_Result_Labels[i].InvokeRequired)
                    {
                        R_Result_Labels[i].Invoke((MethodInvoker)delegate
                        {
                            R_Result_Labels[i].Text = "";
                        });
                    }
                    else
                    {
                        R_Result_Labels[i].Text = "";
                    }
                }
            }
        }

        public void Recive_data(int Index, int[] _Data)
        {
            Index--;
            for (int i = 0; i < 15; i++)
            {
                double Data = ((double)((Int32)_Data[Index * 30 + 10 + (i * 2) + ETC_Option.Interface_Offset] | (Int32)(_Data[Index * 30 + 10 + (i * 2 + 1) + ETC_Option.Interface_Offset] << 16)) / 10000);

                if (L_Labels[i].Text != "")
                {
                    if (L_Result_Labels[i].InvokeRequired)
                    {
                        L_Result_Labels[i].Invoke((MethodInvoker)delegate
                        {
                            L_Result_Labels[i].Text = Data.ToString("0.000");
                        });
                    }
                    else
                    {
                        L_Result_Labels[i].Text = Data.ToString("0.000");
                    }
                }
                else
                {
                    if (L_Result_Labels[i].InvokeRequired)
                    {
                        L_Result_Labels[i].Invoke((MethodInvoker)delegate
                        {
                            L_Result_Labels[i].Text = "";
                        });
                    }
                    else
                    {
                        L_Result_Labels[i].Text = "";
                    }
                }
            }
        }

        private void PLC_Click(object sender, EventArgs e)
        {
            if (Simul_On)
            {
                Label label = (Label)sender;
                int Num = Convert.ToInt32(label.Name.Substring(12, label.Name.Length - 12)) - 1;
                if (!On_Off[Num])
                    COMMON.Receive_Data[Unit_Num] = COMMON.Receive_Data[Unit_Num] | (int)Math.Pow(2, Num);
                else
                    COMMON.Receive_Data[Unit_Num] = COMMON.Receive_Data[Unit_Num] & (0xffff - (int)Math.Pow(2, Num));
                On_Off[Num] = !On_Off[Num];
            }
        }

        private void Simul_Bit_Click(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            int Label_Number = int.Parse(label.Name.Substring(12, label.Name.Length - 12)) - 1;
            if(Label_Number >= 16)
            {
                //PC 불필요
            }
            else
            {
                //PLC
                if(COMMON.Current_Mode == Enum.Mode.Simul && L_Labels[Label_Number].Text != "" && L_Result_Labels[Label_Number].Text == "")
                {
                    if((COMMON.Receive_Data[Unit_Num] & (int)Math.Pow(2, Label_Number)) == 0)
                    {
                        COMMON.Receive_Data[Unit_Num] += (int)Math.Pow(2, Label_Number);
                    }
                    else
                    {
                        COMMON.Receive_Data[Unit_Num] -= (int)Math.Pow(2, Label_Number);
                    }
                }
            }
        }
    }
}
