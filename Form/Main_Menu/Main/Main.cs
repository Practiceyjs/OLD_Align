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
    public partial class Main : UserControl
    {
        Button[] Page_Button;
        Panel[] panel;
        int[] Page_Count = { 0, 0, 0, 0 };
        int[] Page_Moduel_Count = { 0, 0, 0, 0,0 };
        public Type_Display[] Target_Display;
        public Type_Display[] Object_Display;
        public Type_Display[] Live_Display;
        public DataGridView[] Unit_Data_View;
        string[] TO_Column_Name = { "Time", "Tact", "Last X", "Last Y", "Last T", "CG X", "CG Y", "CG T", "BA X", "BA Y", "BA T", "Rev X", "Rev Y", "Rev T", "Offset X", "Offset Y", "Offset T", "T Width1", "T Width2", "T Height1", "T Height2", "O Width1", "O Width2", "O Height1", "O Height2" };
        string[] T_Column_Name = { "Time", "Tact", "Last X", "Last Y", "Last T", "Rev X", "Rev Y", "Rev T", "Offset X", "Offset Y", "Offset T", "Width1", "Width2", "Height1", "Height2" };
        string[] CW_PRE_Column_Name = { "Time", "Tact", "Peeling X", "Peeling Y", "Peeling T", "Loading X", "Loading Y", "Loading T", "P Offset X", "P Offset Y", "P Offset T", "L Offset X", "L Offset Y", "L Offset T", "Width1", "Width2", "Height1", "Height2" };
        int[] TO_Column_Width = { 120, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 80, 80, 80, 80, 80, 80, 80, 80 };
        int[] T_Column_Width = { 120, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 80, 80, 80, 80 };
        int[] CW_Column_Width = { 120, 70, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80 };
        string[] Inspec_Column_Name = { "Time", "Tact", "Dis X1", "Dis Y1", "Dis X2", "Dis Y2", "Dis X3", "Dis Y3", "Dis X4", "Dis Y4", "T Width1", "T Width2", "T Height1", "T Height2", "O Width1", "O Width2", "O Height1", "O Height2" };
        int[] Inspec_Column_Width = { 120, 70, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80 };
        public Main()
        {
            InitializeComponent();
            Page_Button = new Button[] { button1, button2, button3, button4 };
            panel = new Panel[] { panel1, panel2, panel3, panel4 };
            Target_Display = new Type_Display[] { T_Display1, T_Display2, T_Display3, T_Display4, T_Display5, T_Display6, T_Display7, T_Display8 };
            Object_Display = new Type_Display[] { O_Display1, O_Display2, O_Display3, O_Display4, O_Display5, O_Display6, O_Display7, O_Display8 };
            Live_Display = new Type_Display[] { L_Display1, L_Display2, L_Display3, L_Display4 };
            Unit_Data_View = new DataGridView[] { T_Data1, T_Data2, T_Data3, T_Data4, T_Data5, T_Data6, T_Data7, T_Data8 };
            Page_Select(1);
        }

        private string[] Log_Select(int Unit)
        {
            string[] Return_String = null;
            if(!COMMON.U_Setting[Unit].Inspection_Use)
            {
                switch (ETC_Option.Log_Type[Unit])
                {
                    case Enum.Log_Type.TO_Log:
                        Return_String = TO_Column_Name;
                        break;
                    case Enum.Log_Type.T_Log:
                        Return_String = T_Column_Name;
                        break;
                    case Enum.Log_Type.CW_Log:
                        Return_String = CW_PRE_Column_Name;
                        break;
                }
            }
            else
            {
                Return_String = Inspec_Column_Name;
            }
            return Return_String;
        }

        private int[] Log_Size(int Unit)
        {
            int[] Return_int = null;
            if (!COMMON.U_Setting[Unit].Inspection_Use)
            {
                switch (ETC_Option.Log_Type[Unit])
                {
                    case Enum.Log_Type.TO_Log:
                        Return_int = TO_Column_Width;
                        break;
                    case Enum.Log_Type.T_Log:
                        Return_int = T_Column_Width;
                        break;
                    case Enum.Log_Type.CW_Log:
                        Return_int = CW_Column_Width;
                        break;
                }
            }
            else
            {
                Return_int = Inspec_Column_Width;
            }
            return Return_int;
        }

        public void Loading_OK()
        {
            for (int LoopCount = 0; LoopCount < Unit_Data_View.Length; LoopCount++)
            {
                string[] Column_Name = Log_Select(LoopCount + 1);
                int[] Column_Width = Log_Size(LoopCount + 1);
                Unit_Data_View[LoopCount].Font = new Font("Franklin Gothic Demi", 10, FontStyle.Regular);
                Unit_Data_View[LoopCount].ReadOnly = true;
                Unit_Data_View[LoopCount].ColumnCount = Column_Name.Length;
                Unit_Data_View[LoopCount].AllowUserToAddRows = false;
                Unit_Data_View[LoopCount].AllowUserToDeleteRows = false;
                for (int LoopCount2 = 0; LoopCount2 < Column_Name.Length; LoopCount2++)
                {
                    Unit_Data_View[LoopCount].Columns[LoopCount2].Name = Column_Name[LoopCount2];
                    Unit_Data_View[LoopCount].Columns[LoopCount2].Width = Column_Width[LoopCount2];
                }
                foreach (DataGridViewColumn item in Unit_Data_View[LoopCount].Columns) { item.SortMode = DataGridViewColumnSortMode.NotSortable; }
            }
            for (int LoopCount = 1; LoopCount < 9; LoopCount++)
            {
                Page_Moduel_Count[COMMON.U_Setting[LoopCount].T_Setting[0].DIsplay_Page]++;
                Page_Moduel_Count[COMMON.U_Setting[LoopCount].T_Setting[2].DIsplay_Page]++;
                if (COMMON.U_Setting[LoopCount].T_Setting[0].DIsplay_Page != 0)
                    for (int DisplayCount = 0; DisplayCount < 4; DisplayCount++)
                        if (COMMON.U_Setting[LoopCount].T_Setting[0].Position[DisplayCount])
                            Page_Count[COMMON.U_Setting[LoopCount].T_Setting[0].DIsplay_Page - 1]++;
                if (COMMON.U_Setting[LoopCount].T_Setting[2].DIsplay_Page != 0)
                    for (int DisplayCount = 0; DisplayCount < 4; DisplayCount++)
                        if (COMMON.U_Setting[LoopCount].T_Setting[2].Position[DisplayCount])
                            Page_Count[COMMON.U_Setting[LoopCount].T_Setting[2].DIsplay_Page - 1]++;
            }
            for (int LoopCount = 1; LoopCount < 5; LoopCount++)
            {
                if (Page_Moduel_Count[LoopCount] == 0)
                    Page_Button[LoopCount - 1].Visible = false;
                else
                {
                    //int _Display_Count, _Type, _Location;
                    //DIsplay_Info.Type_Return(Page_Moduel_Count[LoopCount], Page_Count[LoopCount - 1], out _Type, out _Display_Count, out _Location);
                    //for (int DIsplay_LoopCount = 0; DIsplay_LoopCount < Page_Moduel_Count[LoopCount]; DIsplay_LoopCount++)
                    //{
                    //    Page_Display[LoopCount - 1, DIsplay_LoopCount].Size = DIsplay_Info.Panel_Size[_Display_Count, _Type];
                    //    Page_Display[LoopCount - 1, DIsplay_LoopCount].Location = DIsplay_Info.Display_Location[_Location, DIsplay_LoopCount];
                    //    Page_Display[LoopCount - 1, DIsplay_LoopCount].Type = _Type;
                    //    Page_Display[LoopCount - 1, DIsplay_LoopCount].Display_Count = _Display_Count;
                    //}
                    //for (int Moduel_LoopCount = Page_Moduel_Count[LoopCount]; Moduel_LoopCount < 4; Moduel_LoopCount++)
                    //{
                    //    Page_Display[LoopCount - 1, Moduel_LoopCount].Visible = false;
                    //}
                }

            }
#if CW_SAM
            Target_Display[0].Text = "CHAMBER CG LAST GRAB";
            Target_Display[0].Display_Set = true;
            Object_Display[0].Text = "CHAMBER BA LAST GRAB";
            Object_Display[0].Display_Set = true;
            Target_Display[1].Text = "PRE LAST GRAB";
            Target_Display[1].Display_Set = true;
            Live_Display[0].Text = "CHAMBER CG LIVE";
            Live_Display[0].Display_Set = true;
            Live_Display[1].Text = "CHAMBER BA LIVE";
            Live_Display[1].Display_Set = true;
            Live_Display[2].Text = "PRE LIVE";
            Live_Display[2].Display_Set = true;
#elif T9_LAMI
            Target_Display[0].Text = "OCA LAST GRAB";
            Target_Display[0].Display_Set = true;
            Target_Display[1].Text = "PANEL LAST GRAB";
            Target_Display[1].Display_Set = true;
            Target_Display[4].Text = "CHAMBER WINDOW LAST GRAB";
            Target_Display[4].Display_Set = true;
            Object_Display[4].Text = "CHAMBER PANEL LAST GRAB";
            Object_Display[4].Display_Set = true;
            Live_Display[0].Text = "OCA LIVE";
            Live_Display[0].Display_Set = true;
            Live_Display[1].Text = "PANEL LIVE";
            Live_Display[1].Display_Set = true;
            Live_Display[2].Text = "CHAMBER WINDOW LIVE";
            Live_Display[2].Display_Set = true;
            Live_Display[3].Text = "CHAMBER PANEL LIVE";
            Live_Display[3].Display_Set = true;
#elif LARGE_AREA_LAMI
            for (int LoopCount=0;LoopCount<2;LoopCount++)
            {
                Target_Display[LoopCount].Text = COMMON.U_Setting[LoopCount + 1].Unit_Name + " LAST GRAB";
                Target_Display[LoopCount].Display_Set = true;
            }
            COMMON.Inspection_Display = O_Display8.DIsplay_Set();
            COMMON.Inspection_Tiltle = O_Display8.Title_Set();
            COMMON.Inspection_Data = O_Data8;
            O_Display8.Text = "INSPECTION";
            O_Display8.Display_Set = true;
            COMMON.Inspection_Data.Font = new Font("Franklin Gothic Demi", 10, FontStyle.Regular);
            COMMON.Inspection_Data.ReadOnly = true;
            COMMON.Inspection_Data.ColumnCount = Inspec_Column_Name.Length;
            COMMON.Inspection_Data.AllowUserToAddRows = false;
            COMMON.Inspection_Data.AllowUserToDeleteRows = false;
            for (int LoopCount2 = 0; LoopCount2 < Inspec_Column_Name.Length; LoopCount2++)
            {
                COMMON.Inspection_Data.Columns[LoopCount2].Name = Inspec_Column_Name[LoopCount2];
                COMMON.Inspection_Data.Columns[LoopCount2].Width = Inspec_Column_Width[LoopCount2];
            }
            foreach (DataGridViewColumn item in COMMON.Inspection_Data.Columns) { item.SortMode = DataGridViewColumnSortMode.NotSortable; }
#endif
        }

        private void Page_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Page_Select(Convert.ToInt32(button.Text.Substring(button.Text.Length - 1, 1)));
        }

        private void Page_Select(int _Selcet)
        {
            for (int LoopCount = 0; LoopCount < 4; LoopCount++)
            {
                panel[LoopCount].Visible = false;
                Page_Button[LoopCount].ForeColor = Color.Black;
                Page_Button[LoopCount].Image = global::T_Align.Properties.Resources.Tap;
            }
            panel[_Selcet - 1].Visible = true;
            Page_Button[_Selcet - 1].ForeColor = Color.White;
            Page_Button[_Selcet - 1].Image = global::T_Align.Properties.Resources.Tap_Select;
        }

    }
}
