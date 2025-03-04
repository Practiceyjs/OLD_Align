using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using System.Windows.Forms.DataVisualization.Charting;

namespace T_Align
{
    public partial class Data : UserControl
    {

        class CustomDataGridView : DataGridView
        {
            public CustomDataGridView()
            {
                DoubleBuffered = true;
            }
        }

        CustomDataGridView DataGrid;

        double Spec = 0;
        double Tol = 0;

        ToolStripLabel Data_Label;
        int Date_Value = 0;
        Button[] Unit_Button;

        Enum.Unit Current_Unit = Enum.Unit.Unit1;
        Enum.Type Current_Type = Enum.Type.Target;
        CogDisplay[] Displays;

        bool Minus = false;
        bool P_C_Selcect = false;
        int Year = 0;
        int Month = 0;
        int Day = 0;
        int Current_Index = 0;
        double[] _Select_Data;
        int _Select_Count;

        private void DataGrid_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (DataGrid.SelectedCells.Count > 1)
            {
                if (e.ColumnIndex > 3)
                    Chart_Display(Convert.ToString(DataGrid.Rows[0].Cells[e.ColumnIndex].Value), e.ColumnIndex);
                if (!e.RowIndex.Equals(-1) && !DataGrid.NewRowIndex.Equals(e.RowIndex))
                {
                    Current_Index = e.RowIndex;
                    Pictuer_Display();
                }
            }
        }

        private void DataGrid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                DataGridView View = (DataGridView)sender;
                if (View.CurrentCellAddress.X > 3)
                    Chart_Display(Convert.ToString(DataGrid.Rows[0].Cells[View.CurrentCellAddress.X].Value), View.CurrentCellAddress.X);
                if (!View.CurrentCellAddress.Y.Equals(-1) && !DataGrid.NewRowIndex.Equals(View.CurrentCellAddress.Y))
                {
                    Current_Index = View.CurrentCellAddress.Y;
                    Pictuer_Display();
                }
            }
        }

        private void DataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > 3)
                Chart_Display(Convert.ToString(DataGrid.Rows[0].Cells[e.ColumnIndex].Value), e.ColumnIndex);
            if (!e.RowIndex.Equals(-1) && !DataGrid.NewRowIndex.Equals(e.RowIndex))
            {
                Current_Index = e.RowIndex;
                Pictuer_Display();
            }
        }

        private void Chart_Display(string Cul_Columns, int Cell_Number)
        {
            try
            {
                double[] data = new double[DataGrid.Rows.Count-1];
                double[,] List_SelectedColumsIndex = new double[DataGrid.Columns.Count, DataGrid.Rows.Count];

                if (DataGrid.SelectedCells.Count > 1)
                {
                    Selected_Chart_Display(Cell_Number);
                    return;
                }

                Series Chart_Series = new Series(Cul_Columns);

                List_SelectedColumsIndex = new double[DataGrid.Columns.Count, DataGrid.Rows.Count];

                double MaxSize = -9999;
                double MinSize = 9999;
                double OffsetSize;
                double _data;
                List<double> List_Selectedvalue = new List<double>();

                Chart_Series.ChartType = SeriesChartType.Line;
                Chart_Series.MarkerStyle = MarkerStyle.Circle;
                Chart_Series.MarkerColor = Color.Red;
                Chart_Series.MarkerSize = 5;

                for (int i = 1; i <= DataGrid.Rows.Count - 1; i++)
                {
                    _data = Convert.ToDouble(DataGrid.Rows[i].Cells[Cell_Number].Value);

                    List_SelectedColumsIndex[Cell_Number, i] = _data;

                    Chart_Series.Points.AddY(_data);
                    List_Selectedvalue.Add(_data);
                    if (_data > MaxSize)
                        MaxSize = _data;
                    if (_data < MinSize)
                        MinSize = _data;

                    data[i-1] = _data;
                    DataGrid.Rows[Convert.ToInt32(i)].DefaultCellStyle.BackColor = Color.White;
                    DataGrid.Rows[Convert.ToInt32(i)].DefaultCellStyle.ForeColor = Color.Black;
                }

                chart1.Legends.Clear();
                chart1.Legends.Add("temp");
                chart1.Legends[0].Docking = Docking.Top;
                chart1.Legends[0].Alignment = StringAlignment.Center;
                chart1.Series[0].Points.Clear();
                chart1.Series.Clear();
                chart1.Series.Add(Chart_Series);

                if (MaxSize == MinSize)
                {
                    OffsetSize = 0.1;
                    chart1.ChartAreas[0].AxisY.Maximum = MaxSize + OffsetSize;
                    chart1.ChartAreas[0].AxisY.Minimum = MinSize - OffsetSize;
                }
                else
                {
                    OffsetSize = (MaxSize - MinSize) / 2;
                    chart1.ChartAreas[0].AxisY.Maximum = MaxSize + OffsetSize;
                    chart1.ChartAreas[0].AxisY.Minimum = MinSize - OffsetSize;
                }
                _Select_Data = data;
                _Select_Count = DataGrid.Rows.Count - 1;
                Data_Caluration();

            }
            catch { }
        }

        private void Selected_Chart_Display(int Cell_Number)
        {
            try
            {
                if (DataGrid.Rows.Count == 1)
                    return;

                int Selectedcount = DataGrid.SelectedCells.Count;

                int Max_Column = 0, Min_Column = 0, Select_Column = 0;

                for (int i = 0; i < DataGrid.SelectedCells.Count; i++)
                {
                    if (DataGrid.SelectedCells[i].ColumnIndex > Max_Column)
                        Max_Column = DataGrid.SelectedCells[i].ColumnIndex;
                    if (DataGrid.SelectedCells[i].ColumnIndex > Min_Column)
                        Min_Column = DataGrid.SelectedCells[i].ColumnIndex;
                }

                if (Max_Column != Cell_Number)
                    Select_Column = Max_Column;
                else
                    Select_Column = Min_Column;

                if (DataGrid.SelectedCells[0].ColumnIndex != DataGrid.SelectedCells[1].ColumnIndex)
                {
                    //소팅 진행
                }
                if (Selectedcount == 0)
                    return;

                Series Chart_Series = new Series(Convert.ToString(DataGrid.Rows[0].Cells[DataGrid.SelectedCells[0].ColumnIndex].Value));


                chart1.Series[0].Points.Clear();

                chart1.BackColor = Color.FromArgb(224, 224, 224);
                chart1.BackSecondaryColor = Color.FromArgb(244, 244, 244);
                chart1.BackGradientStyle = GradientStyle.TopBottom;

                Chart_Series.ChartType = SeriesChartType.Line;
                Chart_Series.MarkerStyle = MarkerStyle.Circle;
                Chart_Series.MarkerColor = Color.Red;
                Chart_Series.MarkerSize = 20;

                double[] Select_Data = new double[DataGrid.SelectedCells.Count];

                int Select_Count = 0;
                for (int i = 0; i <= Selectedcount - 1; i++)
                {
                    if (Select_Column == (DataGrid.SelectedCells[i].ColumnIndex))
                    {
                        Select_Data[Select_Count] = Convert.ToDouble(DataGrid.SelectedCells[i].Value);
                        Select_Count++;
                    }

                }

                Array.Resize(ref Select_Data, Select_Count);
                if (DataGrid.SelectedCells[0].RowIndex > DataGrid.SelectedCells[DataGrid.SelectedCells.Count - 1].RowIndex)
                    Array.Reverse(Select_Data);

                for (int i = 0; i <= Select_Count - 1; i++)
                    Chart_Series.Points.AddXY(i, Select_Data[i]);

                Array.Sort(Select_Data);

                chart1.ChartAreas[0].AxisY.Maximum = Select_Data[Select_Data.Length - 1] + 0.1;
                chart1.ChartAreas[0].AxisY.Minimum = Select_Data[0] - 0.1;
                chart1.Series.Clear();
                chart1.Series.Add(Chart_Series);
                chart1.Series[0] = Chart_Series;
                _Select_Data = Select_Data;
                _Select_Count = Select_Count;
                Data_Caluration();
            }
            catch { }
        }

        private void Data_Caluration()
        {
            double Min;
            double Max;
            double Max_Min;
            double Average;
            double Stdev;
            double Cp = 0;
            double Cpk = 0;

            Max = _Select_Data.Max();
            Min = _Select_Data.Min();
            Average = _Select_Data.Average();
            Max_Min = Max - Min;

            double[] array = new double[_Select_Count];

            for (int j = 0; j < _Select_Count; j++)
                array[j] = Math.Pow(_Select_Data[j] - Average, 2);

            Stdev = Math.Sqrt(array.Average());
            try
            {
                Cp = Tol / (Stdev * 3);
                Cpk = (1 - (Math.Abs(Spec - Average) / (Tol * 2))) * Cp;
            }
            catch { Cp = 0; Cpk = 0; }
            Data_DIsplay(Average, Max, Min, Stdev, Cp, Cpk);
        }

        private void Data_DIsplay(double avr, double MAX, double MIN, double STD, double CP, double CPK)
        {
            AVR_Label.Text = avr.ToString("0.000");
            MAX_Label.Text = MAX.ToString("0.000");
            MIN_Label.Text = MIN.ToString("0.000");
            STD_Label.Text = STD.ToString("0.000");
            CP_Label.Text = CP.ToString("0.000");
            CPK_Label.Text = CPK.ToString("0.000");
        }

        private void Pictuer_Display()
        {
            if (!Current_Index.Equals(0))
            {
                string Image_File = Right_Text(DataGrid.Rows[Current_Index].Cells[0].Value.ToString(), 8);
                string FolderName = "D:\\AlignLog\\" + Year + "\\" + Month + "\\" + Day + "\\Image\\" + Convert.ToString(Current_Unit);
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(FolderName);

                if (!Directory.Exists(FolderName))
                {
                    return;
                }
                else
                {
                    Parallel.ForEach(Displays, _SelDIsplay =>
                    {
                        int Number = int.Parse(_SelDIsplay.Name.Substring(_SelDIsplay.Name.Length - 1, 1)) - 1;
                        string FullFileName = FolderName + "\\" + DataGrid.Rows[Current_Index].Cells[1].Value.ToString() + "_" + ((int)Current_Type).ToString() + "_" + Time_Text(Image_File) + "_" + Number.ToString() + ".jpg";
                        FileInfo fileInfo = new FileInfo(FullFileName);
                        if (fileInfo.Exists)
                            _SelDIsplay.Image = new CogImage24PlanarColor((Bitmap)Bitmap.FromFile(FullFileName));
                        else
                            _SelDIsplay.Image = null;
                    });
                }

            }
        }

        private string Right_Text(string target, int length)
        {
            if (length <= target.Length)
            {
                return target.Substring(target.Length - length);
            }
            return target;
        }

        private string Time_Text(string target)
        {
            return target.Substring(0, 2) + target.Substring(3, 2) + target.Substring(6, 2);
        }

        public Data()
        {
            InitializeComponent();
            Unit_Button = new Button[] { Button_Unit1, Button_Unit2, Button_Unit3, Button_Unit4, Button_Unit5, Button_Unit6, Button_Unit7, Button_Unit8 };
            Displays = new CogDisplay[] { cogDisplay1, cogDisplay2, cogDisplay3, cogDisplay4 };
            for (int LoopCount = 0; LoopCount < 8; LoopCount++)
                Unit_Button[LoopCount].Text = COMMON.U_Setting[LoopCount + 1].Unit_Name;
            DataGrid = new CustomDataGridView();
            DataGrid.Size = new Size(807, 732);
            DataGrid.Location = new Point(5, 228);
            DataGrid.ReadOnly = true;
            DataGrid.ColumnHeadersVisible = false;
            DataGrid.RowHeadersVisible = false;
            DataGrid.AllowUserToAddRows = false;
            DataGrid.AllowUserToDeleteRows = false;
            DataGrid.AllowUserToResizeColumns = false;
            DataGrid.AllowUserToResizeRows = false;
            DataGrid.CellClick += DataGrid_CellClick;
            DataGrid.KeyUp += DataGrid_KeyUp;
            DataGrid.CellMouseUp += DataGrid_CellMouseUp;
            this.Controls.Add(DataGrid);
            picture_panel.Visible = true;
            Chart_Panel.Visible = false;
            Button_Picture.Image = Properties.Resources.Picture_Select;
            Year_Label.Text = DateTime.Now.Year.ToString("0000");
            Month_Label.Text = DateTime.Now.Month.ToString("00");
            Day_Label.Text = DateTime.Now.Day.ToString("00");
            Year = int.Parse(Year_Label.Text);
            Month = int.Parse(Month_Label.Text);
            Day = int.Parse(Day_Label.Text);
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.MouseWheel += WheelEvent;
        }

        private void WheelEvent(object sender, MouseEventArgs e)
        {
            var chart = (Chart)sender;
            var yAxis = chart1.ChartAreas[0].AxisY;
            double y, yMin, yMax, yMax2, yMin2;
            yMin = yAxis.ScaleView.ViewMinimum;
            yMax = yAxis.ScaleView.ViewMaximum;
            y = yAxis.PixelPositionToValue(e.Location.Y);
            if (e.Delta < 0)
            {
                yMin2 = y - (y - yMin) / 0.9;
                yMax2 = y + (yMax - y) / 0.9;
            }
            else
            {
                yMin2 = y - (y - yMin) * 0.9;
                yMax2 = y + (yMax - y) * 0.9;
            }
            if (yMax2 > 5) yMax2 = 5;
            if (yMin2 < 0) yMin2 = 0;
            chart1.ChartAreas[0].AxisY.Maximum = yMax2;
            chart1.ChartAreas[0].AxisY.Minimum = yMin2;
        }

        private void Date_Click(object sender, EventArgs e)
        {
            ToolStripLabel Label = (ToolStripLabel)sender;
            Date_Keyborad(Label.Name.Substring(0, 1));
        }

        private void DataGridView_DIsplay()
        {
            String FolderName = "D:\\AlignLog\\" + Year + "\\" + Month + "\\" + Day;
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(FolderName);
            try
            {
                foreach (System.IO.FileInfo File in di.GetFiles())
                {
                    if (File.Extension.ToLower().CompareTo(".txt") == 0)
                    {
                        String FileNameOnly = File.Name.Substring(0, File.Name.Length - 4);
                        String FullFileName = File.FullName;

                        if (FileNameOnly == Current_Unit.ToString() + "_Align_Data")
                        {
                            DataGrid.Columns.Clear();
                            DataGrid.ColumnCount = 61;
                            using (FileStream fs = new FileStream(FullFileName, FileMode.Open))
                            {
                                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8, false))
                                {
                                    string strLineValue = null;
                                    string[] values = null;
                                    while ((strLineValue = sr.ReadLine()) != null)
                                    {
                                        values = strLineValue.Split(',');
                                        DataGrid.Rows.Add(values);
                                    }
                                }
                            }
                        }

                    }
                }
            }
            catch { }
            foreach (DataGridViewColumn item in DataGrid.Columns)
            {
                item.SortMode = DataGridViewColumnSortMode.NotSortable;
                if (item.Index != 0 && item.Index != 1)
                    item.Width = 70;
            }
        }

        private void Date_Keyborad(string _Command)
        {
            switch(_Command)
            {
                case "Y":
                    Data_Label = Year_Label;
                    Date_Value = int.Parse(Year_Label.Text);
                    break;
                case "M":
                    Data_Label = Month_Label;
                    Date_Value = int.Parse(Month_Label.Text);
                    break;
                case "D":
                    Data_Label = Day_Label;
                    Date_Value = int.Parse(Day_Label.Text);
                    break;
            }
            if (COMMON.Number_KeyBoard != null)
            {
                COMMON.Number_KeyBoard.Close();
            }
            COMMON.Number_KeyBoard = new Number_KeyBoard(Enum.Key_Type.Int_Positive);
            COMMON.Number_KeyBoard.SendMsg += new Number_KeyBoard.SendMsgDele(Date_KeyBoard_SendMsg);
            COMMON.Number_KeyBoard.TopMost = true;
            COMMON.Number_KeyBoard.Location = new Point(389, 150);
            COMMON.Number_KeyBoard.Show();
        }

        private void Date_Set(ToolStripLabel _DataLaBel)
        {
            switch(_DataLaBel.Name)
            {
                case "Year_Label":
                    Year = int.Parse(_DataLaBel.Text);
                    break;
                case "Month_Label":
                    Month = int.Parse(_DataLaBel.Text);
                    break;
                case "Day_Label":
                    Day = int.Parse(_DataLaBel.Text);
                    break;
            }
        }

        private void Date_KeyBoard_SendMsg(string _Msg)
        {
            switch (_Msg)
            {
                case "C":
                    Date_Value = 0;
                    break;
                case "←":
                    Date_Value = Date_Value / 10;
                    break;
                case "OK":
                    Date_Set(Data_Label);
                    DataGridView_DIsplay();
                    break;
                default:
                    Date_Value = Limit_Check(Data_Label,Date_Value * 10 + Convert.ToInt32(_Msg), Convert.ToInt32(_Msg));
                    break;
            }
            switch (Data_Label.Name.Substring(0, 1))
            {
                case "Y":
                    Data_Label.Text = Date_Value.ToString("0000");
                    break;
                default:
                    Data_Label.Text = Date_Value.ToString("00");
                    break;
            }
        }

        private int Limit_Check(ToolStripLabel control, int _value)     //리밋 방식 현재 미사용
        {
            switch (control.Name.Substring(0, 1))
            {
                case "Y":
                    if (_value > 9999)
                        _value = 9999;
                    break;
                case "M":
                    if (_value > 12)
                        _value = 12;
                    break;
                case "D":
                    if (_value > 31)
                        _value = 31;
                    break;
            }
            return _value;
        }

        private int Limit_Check(ToolStripLabel control, int _value, int _Msg)   //값 초기화 방식 현재 사용
        {
            switch (control.Name.Substring(0, 1))
            {
                case "Y":
                    if (_value > 9999)
                        _value = _Msg;
                    break;
                case "M":
                    if (_value > 12)
                        _value = _Msg;
                    break;
                case "D":
                    if (_value > 31)
                        _value = _Msg;
                    break;
            }
            return _value;
        }

        private void Unit_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Current_Unit = (Enum.Unit)Convert.ToInt32(button.Name.Substring(button.Name.Length - 1, 1));
            Button_Select((int)Current_Unit);
            DataGridView_DIsplay();
        }

        private void Button_Picture_Click(object sender, EventArgs e)
        {
            P_C_Selcect = false;
            Chart_Panel.Visible = false;
            picture_panel.Visible = true;
            Button_Picture.Image = Properties.Resources.Picture_Select;
            Button_Chrat.Image = Properties.Resources.Chart;
        }

        private void Button_Chrat_Click(object sender, EventArgs e)
        {
            P_C_Selcect = true;
            picture_panel.Visible = false;
            Chart_Panel.Visible = true;
            Button_Picture.Image = Properties.Resources.Picture;
            Button_Chrat.Image = Properties.Resources.Chart_Select;
        }

        private void Button_Select(int Select_Number)
        {
            Button_Target.Text = COMMON.U_Setting[(int)Current_Unit].T_Setting[0].Type_Name;
            Button_Object.Text = COMMON.U_Setting[(int)Current_Unit].T_Setting[2].Type_Name;
            if (COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Object].Position[0] == false &&
                COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Object].Position[1] == false &&
                COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Object].Position[2] == false &&
                COMMON.U_Setting[(int)Current_Unit].T_Setting[(int)Enum.Type.Object].Position[3] == false)
            {
                Current_Type = Enum.Type.Target;
                Button_Object.Visible = false;
                Button_Target.Image = global::T_Align.Properties.Resources.Button5_Select;
                Button_Object.Image = global::T_Align.Properties.Resources.Button5;
                Button_Target.ForeColor = Color.Cyan;
                Button_Object.ForeColor = Color.White;
            }
            else
            {
                Button_Object.Visible = true;
            }
            for (int LoopCount = 0; LoopCount < 8; LoopCount++)
            {
                Unit_Button[LoopCount].Image = Properties.Resources.Unit;
                Unit_Button[LoopCount].ForeColor = Color.White;
            }
            Unit_Button[Select_Number - 1].Image = Properties.Resources.Unit_Select;
            Unit_Button[Select_Number - 1].ForeColor = Color.Cyan;
        }

        public void Loading_OK()
        {
            for (int LoopCount = 1; LoopCount < 9; LoopCount++)
            {
                if (COMMON.Frame.Unit[LoopCount] == null)
                    Unit_Button[LoopCount - 1].Visible = false;
            }
            Type_Select("T");
            Button_Select((int)Current_Unit);
        }

        private void Type_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Type_Select(button.Text.Substring(0, 1));
        }

        private void Type_Select(string _Type)
        {
            switch (_Type)
            {
                case "T":
                    Current_Type = Enum.Type.Target;
                    break;
                case "O":
                    Current_Type = Enum.Type.Object;
                    break;
            }
            if (Current_Type == Enum.Type.Target)
            {
                Button_Target.Image = global::T_Align.Properties.Resources.Button5_Select;
                Button_Object.Image = global::T_Align.Properties.Resources.Button5;
                Button_Target.ForeColor = Color.Cyan;
                Button_Object.ForeColor = Color.White;
            }
            else
            {
                Button_Target.Image = global::T_Align.Properties.Resources.Button5;
                Button_Object.Image = global::T_Align.Properties.Resources.Button5_Select;
                Button_Target.ForeColor = Color.White;
                Button_Object.ForeColor = Color.Cyan;
            }
        }
        private void Nmr_Spec_ValueChanged(object sender, EventArgs e)
        {
            Spec = (double)Nmr_Spec.Value;
            try
            {
                Data_Caluration();
            }
            catch { }
        }

        private void Nmr_Tol_ValueChanged(object sender, EventArgs e)
        {
            
            Tol = (double)Nmr_Tol.Value;
            try
            {
                Data_Caluration();
            }
            catch { }
        }
    }
}
