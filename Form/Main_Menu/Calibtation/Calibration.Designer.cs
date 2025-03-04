
namespace T_Align
{
    partial class Calibration
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Calibration));
            this.Cal_Display1 = new Cognex.VisionPro.Display.CogDisplay();
            this.Cal_Display2 = new Cognex.VisionPro.Display.CogDisplay();
            this.panel12 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Textbox_RY = new System.Windows.Forms.TextBox();
            this.Textbox_RX = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Explanation_Label2 = new System.Windows.Forms.Label();
            this.Textbox_LY = new System.Windows.Forms.TextBox();
            this.Textbox_LX = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Textbox_Mouse_Bright = new System.Windows.Forms.TextBox();
            this.Textbox_Mouse_Y = new System.Windows.Forms.TextBox();
            this.Textbox_Mouse_X = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel10 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.Button_Copy = new System.Windows.Forms.Button();
            this.Mark_Display = new Cognex.VisionPro.Display.CogDisplay();
            this.Button_Find = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.Nud_Cal_Pitch_T2 = new System.Windows.Forms.NumericUpDown();
            this.Nud_Cal_Pitch_Y1 = new System.Windows.Forms.NumericUpDown();
            this.Nud_Cal_Pitch_X0 = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.Button_CTQ = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.Button_Mark4 = new System.Windows.Forms.Button();
            this.Button_Mark2 = new System.Windows.Forms.Button();
            this.Button_Mark3 = new System.Windows.Forms.Button();
            this.Button_Mark1 = new System.Windows.Forms.Button();
            this.Button_Cal_Result = new System.Windows.Forms.Button();
            this.Button_Mesure = new System.Windows.Forms.Button();
            this.Button_Custom_Line = new System.Windows.Forms.Button();
            this.Button_Center_Line = new System.Windows.Forms.Button();
            this.Button_Object = new System.Windows.Forms.Button();
            this.Button_Target = new System.Windows.Forms.Button();
            this.Button_Live = new System.Windows.Forms.Button();
            this.Button_Grab = new System.Windows.Forms.Button();
            this.Button_Clear = new System.Windows.Forms.Button();
            this.Button_Unit8 = new System.Windows.Forms.Button();
            this.Button_Unit7 = new System.Windows.Forms.Button();
            this.Button_Unit6 = new System.Windows.Forms.Button();
            this.Button_Unit5 = new System.Windows.Forms.Button();
            this.Button_Unit4 = new System.Windows.Forms.Button();
            this.Button_Unit3 = new System.Windows.Forms.Button();
            this.Button_Unit2 = new System.Windows.Forms.Button();
            this.Button_Unit1 = new System.Windows.Forms.Button();
            this.Triger_panel = new System.Windows.Forms.Panel();
            this.Triger_Label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Cal_Display1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cal_Display2)).BeginInit();
            this.panel12.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Mark_Display)).BeginInit();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Nud_Cal_Pitch_T2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Nud_Cal_Pitch_Y1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Nud_Cal_Pitch_X0)).BeginInit();
            this.Triger_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Cal_Display1
            // 
            this.Cal_Display1.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.Cal_Display1.ColorMapLowerRoiLimit = 0D;
            this.Cal_Display1.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.Cal_Display1.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.Cal_Display1.ColorMapUpperRoiLimit = 1D;
            this.Cal_Display1.DoubleTapZoomCycleLength = 2;
            this.Cal_Display1.DoubleTapZoomSensitivity = 2.5D;
            this.Cal_Display1.Location = new System.Drawing.Point(12, 102);
            this.Cal_Display1.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.Cal_Display1.MouseWheelSensitivity = 1D;
            this.Cal_Display1.Name = "Cal_Display1";
            this.Cal_Display1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("Cal_Display1.OcxState")));
            this.Cal_Display1.Size = new System.Drawing.Size(945, 709);
            this.Cal_Display1.TabIndex = 22;
            this.Cal_Display1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Cal_Display1_MouseDown);
            this.Cal_Display1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Cal_Display1_MouseUp);
            this.Cal_Display1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.cogDisplay1_MouseMove);
            // 
            // Cal_Display2
            // 
            this.Cal_Display2.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.Cal_Display2.ColorMapLowerRoiLimit = 0D;
            this.Cal_Display2.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.Cal_Display2.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.Cal_Display2.ColorMapUpperRoiLimit = 1D;
            this.Cal_Display2.DoubleTapZoomCycleLength = 2;
            this.Cal_Display2.DoubleTapZoomSensitivity = 2.5D;
            this.Cal_Display2.Location = new System.Drawing.Point(963, 102);
            this.Cal_Display2.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.Cal_Display2.MouseWheelSensitivity = 1D;
            this.Cal_Display2.Name = "Cal_Display2";
            this.Cal_Display2.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("Cal_Display2.OcxState")));
            this.Cal_Display2.Size = new System.Drawing.Size(945, 709);
            this.Cal_Display2.TabIndex = 23;
            this.Cal_Display2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Cal_Display2_MouseDown);
            this.Cal_Display2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Cal_Display2_MouseUp);
            this.Cal_Display2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Cal_Display2_MouseMove);
            // 
            // panel12
            // 
            this.panel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.panel12.Controls.Add(this.label2);
            this.panel12.Controls.Add(this.label3);
            this.panel12.Controls.Add(this.Textbox_RY);
            this.panel12.Controls.Add(this.Textbox_RX);
            this.panel12.Controls.Add(this.label1);
            this.panel12.Controls.Add(this.Explanation_Label2);
            this.panel12.Controls.Add(this.Textbox_LY);
            this.panel12.Controls.Add(this.Textbox_LX);
            this.panel12.Location = new System.Drawing.Point(359, 825);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(382, 125);
            this.panel12.TabIndex = 32;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.label2.Font = new System.Drawing.Font("Franklin Gothic Demi", 24F);
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(192, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 37);
            this.label2.TabIndex = 11;
            this.label2.Text = "RY";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.label3.Font = new System.Drawing.Font("Franklin Gothic Demi", 24F);
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(192, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 37);
            this.label3.TabIndex = 10;
            this.label3.Text = "RX";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Textbox_RY
            // 
            this.Textbox_RY.Font = new System.Drawing.Font("Franklin Gothic Demi", 24F);
            this.Textbox_RY.Location = new System.Drawing.Point(254, 68);
            this.Textbox_RY.Name = "Textbox_RY";
            this.Textbox_RY.ReadOnly = true;
            this.Textbox_RY.Size = new System.Drawing.Size(118, 44);
            this.Textbox_RY.TabIndex = 9;
            this.Textbox_RY.Text = "0.000";
            this.Textbox_RY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Textbox_RX
            // 
            this.Textbox_RX.Font = new System.Drawing.Font("Franklin Gothic Demi", 24F);
            this.Textbox_RX.Location = new System.Drawing.Point(254, 14);
            this.Textbox_RX.Name = "Textbox_RX";
            this.Textbox_RX.ReadOnly = true;
            this.Textbox_RX.Size = new System.Drawing.Size(118, 44);
            this.Textbox_RX.TabIndex = 8;
            this.Textbox_RX.Text = "0.000";
            this.Textbox_RX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.label1.Font = new System.Drawing.Font("Franklin Gothic Demi", 24F);
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(1, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 37);
            this.label1.TabIndex = 7;
            this.label1.Text = "LY";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Explanation_Label2
            // 
            this.Explanation_Label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Explanation_Label2.Font = new System.Drawing.Font("Franklin Gothic Demi", 24F);
            this.Explanation_Label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Explanation_Label2.Location = new System.Drawing.Point(1, 17);
            this.Explanation_Label2.Name = "Explanation_Label2";
            this.Explanation_Label2.Size = new System.Drawing.Size(60, 37);
            this.Explanation_Label2.TabIndex = 6;
            this.Explanation_Label2.Text = "LX";
            this.Explanation_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Textbox_LY
            // 
            this.Textbox_LY.Font = new System.Drawing.Font("Franklin Gothic Demi", 24F);
            this.Textbox_LY.Location = new System.Drawing.Point(62, 68);
            this.Textbox_LY.Name = "Textbox_LY";
            this.Textbox_LY.ReadOnly = true;
            this.Textbox_LY.Size = new System.Drawing.Size(118, 44);
            this.Textbox_LY.TabIndex = 1;
            this.Textbox_LY.Text = "0.000";
            this.Textbox_LY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Textbox_LX
            // 
            this.Textbox_LX.Font = new System.Drawing.Font("Franklin Gothic Demi", 24F);
            this.Textbox_LX.Location = new System.Drawing.Point(62, 14);
            this.Textbox_LX.Name = "Textbox_LX";
            this.Textbox_LX.ReadOnly = true;
            this.Textbox_LX.Size = new System.Drawing.Size(118, 44);
            this.Textbox_LX.TabIndex = 0;
            this.Textbox_LX.Text = "0.000";
            this.Textbox_LX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Textbox_Mouse_Bright);
            this.panel1.Controls.Add(this.Textbox_Mouse_Y);
            this.panel1.Controls.Add(this.Textbox_Mouse_X);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(870, 815);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(262, 145);
            this.panel1.TabIndex = 33;
            // 
            // Textbox_Mouse_Bright
            // 
            this.Textbox_Mouse_Bright.Font = new System.Drawing.Font("Franklin Gothic Demi", 24F);
            this.Textbox_Mouse_Bright.Location = new System.Drawing.Point(138, 98);
            this.Textbox_Mouse_Bright.Name = "Textbox_Mouse_Bright";
            this.Textbox_Mouse_Bright.ReadOnly = true;
            this.Textbox_Mouse_Bright.Size = new System.Drawing.Size(118, 44);
            this.Textbox_Mouse_Bright.TabIndex = 12;
            this.Textbox_Mouse_Bright.Text = "0.000";
            this.Textbox_Mouse_Bright.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Textbox_Mouse_Y
            // 
            this.Textbox_Mouse_Y.Font = new System.Drawing.Font("Franklin Gothic Demi", 24F);
            this.Textbox_Mouse_Y.Location = new System.Drawing.Point(138, 51);
            this.Textbox_Mouse_Y.Name = "Textbox_Mouse_Y";
            this.Textbox_Mouse_Y.ReadOnly = true;
            this.Textbox_Mouse_Y.Size = new System.Drawing.Size(118, 44);
            this.Textbox_Mouse_Y.TabIndex = 11;
            this.Textbox_Mouse_Y.Text = "0.000";
            this.Textbox_Mouse_Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Textbox_Mouse_X
            // 
            this.Textbox_Mouse_X.Font = new System.Drawing.Font("Franklin Gothic Demi", 24F);
            this.Textbox_Mouse_X.Location = new System.Drawing.Point(138, 4);
            this.Textbox_Mouse_X.Name = "Textbox_Mouse_X";
            this.Textbox_Mouse_X.ReadOnly = true;
            this.Textbox_Mouse_X.Size = new System.Drawing.Size(118, 44);
            this.Textbox_Mouse_X.TabIndex = 10;
            this.Textbox_Mouse_X.Text = "0.000";
            this.Textbox_Mouse_X.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Franklin Gothic Demi", 24F);
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(3, 101);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(129, 37);
            this.label6.TabIndex = 9;
            this.label6.Text = "BRIGHT";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Franklin Gothic Demi", 24F);
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(3, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(129, 37);
            this.label5.TabIndex = 8;
            this.label5.Text = "Y";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Franklin Gothic Demi", 24F);
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(3, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 37);
            this.label4.TabIndex = 7;
            this.label4.Text = "X";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.Black;
            this.panel10.Location = new System.Drawing.Point(213, 811);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(6, 155);
            this.panel10.TabIndex = 34;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.Location = new System.Drawing.Point(745, 811);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(6, 155);
            this.panel2.TabIndex = 35;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Black;
            this.panel3.Location = new System.Drawing.Point(1138, 811);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(6, 155);
            this.panel3.TabIndex = 36;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.Button_Copy);
            this.panel4.Controls.Add(this.Mark_Display);
            this.panel4.Controls.Add(this.Button_Find);
            this.panel4.Location = new System.Drawing.Point(1688, 817);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(232, 145);
            this.panel4.TabIndex = 34;
            // 
            // Button_Copy
            // 
            this.Button_Copy.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Copy.FlatAppearance.BorderSize = 0;
            this.Button_Copy.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Copy.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Copy.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Copy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Copy.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.Button_Copy.ForeColor = System.Drawing.Color.White;
            this.Button_Copy.Image = ((System.Drawing.Image)(resources.GetObject("Button_Copy.Image")));
            this.Button_Copy.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_Copy.Location = new System.Drawing.Point(142, 76);
            this.Button_Copy.Name = "Button_Copy";
            this.Button_Copy.Size = new System.Drawing.Size(80, 59);
            this.Button_Copy.TabIndex = 44;
            this.Button_Copy.Text = "COPY";
            this.Button_Copy.UseVisualStyleBackColor = true;
            this.Button_Copy.Click += new System.EventHandler(this.Button_Copy_Click);
            // 
            // Mark_Display
            // 
            this.Mark_Display.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.Mark_Display.ColorMapLowerRoiLimit = 0D;
            this.Mark_Display.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.Mark_Display.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.Mark_Display.ColorMapUpperRoiLimit = 1D;
            this.Mark_Display.DoubleTapZoomCycleLength = 2;
            this.Mark_Display.DoubleTapZoomSensitivity = 2.5D;
            this.Mark_Display.Location = new System.Drawing.Point(3, 4);
            this.Mark_Display.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.Mark_Display.MouseWheelSensitivity = 1D;
            this.Mark_Display.Name = "Mark_Display";
            this.Mark_Display.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("Mark_Display.OcxState")));
            this.Mark_Display.Size = new System.Drawing.Size(134, 138);
            this.Mark_Display.TabIndex = 23;
            // 
            // Button_Find
            // 
            this.Button_Find.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Find.FlatAppearance.BorderSize = 0;
            this.Button_Find.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Find.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Find.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Find.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Find.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.Button_Find.ForeColor = System.Drawing.Color.White;
            this.Button_Find.Image = ((System.Drawing.Image)(resources.GetObject("Button_Find.Image")));
            this.Button_Find.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_Find.Location = new System.Drawing.Point(142, 8);
            this.Button_Find.Name = "Button_Find";
            this.Button_Find.Size = new System.Drawing.Size(80, 59);
            this.Button_Find.TabIndex = 43;
            this.Button_Find.Text = "FIND";
            this.Button_Find.UseVisualStyleBackColor = true;
            this.Button_Find.Click += new System.EventHandler(this.Button_Find_Click);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Black;
            this.panel5.Location = new System.Drawing.Point(1456, 811);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(6, 155);
            this.panel5.TabIndex = 37;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.Nud_Cal_Pitch_T2);
            this.panel6.Controls.Add(this.Nud_Cal_Pitch_Y1);
            this.panel6.Controls.Add(this.Nud_Cal_Pitch_X0);
            this.panel6.Controls.Add(this.label8);
            this.panel6.Controls.Add(this.label9);
            this.panel6.Controls.Add(this.label10);
            this.panel6.Location = new System.Drawing.Point(1263, 815);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(186, 145);
            this.panel6.TabIndex = 34;
            // 
            // Nud_Cal_Pitch_T2
            // 
            this.Nud_Cal_Pitch_T2.DecimalPlaces = 3;
            this.Nud_Cal_Pitch_T2.Font = new System.Drawing.Font("Franklin Gothic Demi", 24F);
            this.Nud_Cal_Pitch_T2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.Nud_Cal_Pitch_T2.Location = new System.Drawing.Point(59, 96);
            this.Nud_Cal_Pitch_T2.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Nud_Cal_Pitch_T2.Name = "Nud_Cal_Pitch_T2";
            this.Nud_Cal_Pitch_T2.Size = new System.Drawing.Size(118, 44);
            this.Nud_Cal_Pitch_T2.TabIndex = 12;
            this.Nud_Cal_Pitch_T2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Nud_Cal_Pitch_T2.ValueChanged += new System.EventHandler(this.Calibration_Pitch_Changed);
            // 
            // Nud_Cal_Pitch_Y1
            // 
            this.Nud_Cal_Pitch_Y1.DecimalPlaces = 3;
            this.Nud_Cal_Pitch_Y1.Font = new System.Drawing.Font("Franklin Gothic Demi", 24F);
            this.Nud_Cal_Pitch_Y1.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.Nud_Cal_Pitch_Y1.Location = new System.Drawing.Point(59, 49);
            this.Nud_Cal_Pitch_Y1.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.Nud_Cal_Pitch_Y1.Name = "Nud_Cal_Pitch_Y1";
            this.Nud_Cal_Pitch_Y1.Size = new System.Drawing.Size(118, 44);
            this.Nud_Cal_Pitch_Y1.TabIndex = 11;
            this.Nud_Cal_Pitch_Y1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Nud_Cal_Pitch_Y1.ValueChanged += new System.EventHandler(this.Calibration_Pitch_Changed);
            // 
            // Nud_Cal_Pitch_X0
            // 
            this.Nud_Cal_Pitch_X0.DecimalPlaces = 3;
            this.Nud_Cal_Pitch_X0.Font = new System.Drawing.Font("Franklin Gothic Demi", 24F);
            this.Nud_Cal_Pitch_X0.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.Nud_Cal_Pitch_X0.Location = new System.Drawing.Point(59, 2);
            this.Nud_Cal_Pitch_X0.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.Nud_Cal_Pitch_X0.Name = "Nud_Cal_Pitch_X0";
            this.Nud_Cal_Pitch_X0.Size = new System.Drawing.Size(118, 44);
            this.Nud_Cal_Pitch_X0.TabIndex = 10;
            this.Nud_Cal_Pitch_X0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Nud_Cal_Pitch_X0.ValueChanged += new System.EventHandler(this.Calibration_Pitch_Changed);
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Franklin Gothic Demi", 24F);
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(3, 101);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 37);
            this.label8.TabIndex = 9;
            this.label8.Text = "T";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Franklin Gothic Demi", 24F);
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(3, 54);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 37);
            this.label9.TabIndex = 8;
            this.label9.Text = "Y";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Franklin Gothic Demi", 24F);
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(3, 7);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(50, 37);
            this.label10.TabIndex = 7;
            this.label10.Text = "X";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Button_CTQ
            // 
            this.Button_CTQ.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.Button_CTQ.FlatAppearance.BorderSize = 0;
            this.Button_CTQ.FlatAppearance.CheckedBackColor = System.Drawing.Color.Black;
            this.Button_CTQ.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_CTQ.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_CTQ.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_CTQ.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.Button_CTQ.ForeColor = System.Drawing.Color.White;
            this.Button_CTQ.Image = global::T_Align.Properties.Resources._string;
            this.Button_CTQ.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_CTQ.Location = new System.Drawing.Point(1752, 6);
            this.Button_CTQ.Name = "Button_CTQ";
            this.Button_CTQ.Size = new System.Drawing.Size(60, 37);
            this.Button_CTQ.TabIndex = 47;
            this.Button_CTQ.Text = "CTQ";
            this.Button_CTQ.UseVisualStyleBackColor = true;
            this.Button_CTQ.Click += new System.EventHandler(this.Button_CTQ_Click);
            // 
            // button16
            // 
            this.button16.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.button16.FlatAppearance.BorderSize = 0;
            this.button16.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.button16.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.button16.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.button16.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button16.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.button16.ForeColor = System.Drawing.Color.Black;
            this.button16.Image = ((System.Drawing.Image)(resources.GetObject("button16.Image")));
            this.button16.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button16.Location = new System.Drawing.Point(1149, 825);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(110, 127);
            this.button16.TabIndex = 46;
            this.button16.Text = "CAL\nPITCH";
            this.button16.UseVisualStyleBackColor = true;
            // 
            // button11
            // 
            this.button11.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.button11.FlatAppearance.BorderSize = 0;
            this.button11.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.button11.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.button11.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button11.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.button11.ForeColor = System.Drawing.Color.Black;
            this.button11.Image = ((System.Drawing.Image)(resources.GetObject("button11.Image")));
            this.button11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button11.Location = new System.Drawing.Point(756, 825);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(110, 127);
            this.button11.TabIndex = 45;
            this.button11.Text = "MOUSE\nPOINT";
            this.button11.UseVisualStyleBackColor = true;
            // 
            // Button_Mark4
            // 
            this.Button_Mark4.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Mark4.FlatAppearance.BorderSize = 0;
            this.Button_Mark4.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Mark4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Mark4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Mark4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Mark4.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.Button_Mark4.ForeColor = System.Drawing.Color.White;
            this.Button_Mark4.Image = ((System.Drawing.Image)(resources.GetObject("Button_Mark4.Image")));
            this.Button_Mark4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_Mark4.Location = new System.Drawing.Point(1578, 892);
            this.Button_Mark4.Name = "Button_Mark4";
            this.Button_Mark4.Size = new System.Drawing.Size(106, 59);
            this.Button_Mark4.TabIndex = 42;
            this.Button_Mark4.Text = "MARK4";
            this.Button_Mark4.UseVisualStyleBackColor = true;
            this.Button_Mark4.Click += new System.EventHandler(this.Mark_Click);
            // 
            // Button_Mark2
            // 
            this.Button_Mark2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Mark2.FlatAppearance.BorderSize = 0;
            this.Button_Mark2.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Mark2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Mark2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Mark2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Mark2.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.Button_Mark2.ForeColor = System.Drawing.Color.White;
            this.Button_Mark2.Image = ((System.Drawing.Image)(resources.GetObject("Button_Mark2.Image")));
            this.Button_Mark2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_Mark2.Location = new System.Drawing.Point(1578, 824);
            this.Button_Mark2.Name = "Button_Mark2";
            this.Button_Mark2.Size = new System.Drawing.Size(106, 59);
            this.Button_Mark2.TabIndex = 41;
            this.Button_Mark2.Text = "MARK2";
            this.Button_Mark2.UseVisualStyleBackColor = true;
            this.Button_Mark2.Click += new System.EventHandler(this.Mark_Click);
            // 
            // Button_Mark3
            // 
            this.Button_Mark3.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Mark3.FlatAppearance.BorderSize = 0;
            this.Button_Mark3.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Mark3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Mark3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Mark3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Mark3.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.Button_Mark3.ForeColor = System.Drawing.Color.White;
            this.Button_Mark3.Image = ((System.Drawing.Image)(resources.GetObject("Button_Mark3.Image")));
            this.Button_Mark3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_Mark3.Location = new System.Drawing.Point(1468, 892);
            this.Button_Mark3.Name = "Button_Mark3";
            this.Button_Mark3.Size = new System.Drawing.Size(106, 59);
            this.Button_Mark3.TabIndex = 40;
            this.Button_Mark3.Text = "MARK3";
            this.Button_Mark3.UseVisualStyleBackColor = true;
            this.Button_Mark3.Click += new System.EventHandler(this.Mark_Click);
            // 
            // Button_Mark1
            // 
            this.Button_Mark1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Mark1.FlatAppearance.BorderSize = 0;
            this.Button_Mark1.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Mark1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Mark1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Mark1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Mark1.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.Button_Mark1.ForeColor = System.Drawing.Color.White;
            this.Button_Mark1.Image = ((System.Drawing.Image)(resources.GetObject("Button_Mark1.Image")));
            this.Button_Mark1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_Mark1.Location = new System.Drawing.Point(1468, 824);
            this.Button_Mark1.Name = "Button_Mark1";
            this.Button_Mark1.Size = new System.Drawing.Size(106, 59);
            this.Button_Mark1.TabIndex = 39;
            this.Button_Mark1.Text = "MARK1";
            this.Button_Mark1.UseVisualStyleBackColor = true;
            this.Button_Mark1.Click += new System.EventHandler(this.Mark_Click);
            // 
            // Button_Cal_Result
            // 
            this.Button_Cal_Result.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.Button_Cal_Result.FlatAppearance.BorderSize = 0;
            this.Button_Cal_Result.FlatAppearance.CheckedBackColor = System.Drawing.Color.Black;
            this.Button_Cal_Result.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Cal_Result.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Cal_Result.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Cal_Result.Image = global::T_Align.Properties.Resources.Calibration_Result;
            this.Button_Cal_Result.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_Cal_Result.Location = new System.Drawing.Point(1818, 6);
            this.Button_Cal_Result.Name = "Button_Cal_Result";
            this.Button_Cal_Result.Size = new System.Drawing.Size(90, 90);
            this.Button_Cal_Result.TabIndex = 37;
            this.Button_Cal_Result.UseVisualStyleBackColor = true;
            this.Button_Cal_Result.Click += new System.EventHandler(this.Button_Cal_Result_Click);
            // 
            // Button_Mesure
            // 
            this.Button_Mesure.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Mesure.FlatAppearance.BorderSize = 0;
            this.Button_Mesure.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Mesure.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Mesure.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Mesure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Mesure.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.Button_Mesure.ForeColor = System.Drawing.Color.White;
            this.Button_Mesure.Image = global::T_Align.Properties.Resources.Button2;
            this.Button_Mesure.Location = new System.Drawing.Point(223, 825);
            this.Button_Mesure.Name = "Button_Mesure";
            this.Button_Mesure.Size = new System.Drawing.Size(132, 127);
            this.Button_Mesure.TabIndex = 31;
            this.Button_Mesure.Text = "MESURE";
            this.Button_Mesure.UseVisualStyleBackColor = true;
            this.Button_Mesure.Click += new System.EventHandler(this.Button_Mesure_Click);
            // 
            // Button_Custom_Line
            // 
            this.Button_Custom_Line.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.Button_Custom_Line.FlatAppearance.BorderSize = 0;
            this.Button_Custom_Line.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Custom_Line.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Custom_Line.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Custom_Line.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Custom_Line.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.Button_Custom_Line.ForeColor = System.Drawing.Color.White;
            this.Button_Custom_Line.Image = ((System.Drawing.Image)(resources.GetObject("Button_Custom_Line.Image")));
            this.Button_Custom_Line.Location = new System.Drawing.Point(9, 893);
            this.Button_Custom_Line.Name = "Button_Custom_Line";
            this.Button_Custom_Line.Size = new System.Drawing.Size(199, 59);
            this.Button_Custom_Line.TabIndex = 30;
            this.Button_Custom_Line.Text = "CUSTOM LINE";
            this.Button_Custom_Line.UseVisualStyleBackColor = true;
            this.Button_Custom_Line.Click += new System.EventHandler(this.Button_Custom_Line_Click);
            // 
            // Button_Center_Line
            // 
            this.Button_Center_Line.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.Button_Center_Line.FlatAppearance.BorderSize = 0;
            this.Button_Center_Line.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Center_Line.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Center_Line.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Center_Line.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Center_Line.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.Button_Center_Line.ForeColor = System.Drawing.Color.White;
            this.Button_Center_Line.Image = ((System.Drawing.Image)(resources.GetObject("Button_Center_Line.Image")));
            this.Button_Center_Line.Location = new System.Drawing.Point(9, 825);
            this.Button_Center_Line.Name = "Button_Center_Line";
            this.Button_Center_Line.Size = new System.Drawing.Size(199, 59);
            this.Button_Center_Line.TabIndex = 29;
            this.Button_Center_Line.Text = "CENTER LINE";
            this.Button_Center_Line.UseVisualStyleBackColor = true;
            this.Button_Center_Line.Click += new System.EventHandler(this.Button_Center_Line_Click);
            // 
            // Button_Object
            // 
            this.Button_Object.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Object.FlatAppearance.BorderSize = 0;
            this.Button_Object.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Object.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Object.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Object.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Object.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.Button_Object.ForeColor = System.Drawing.Color.White;
            this.Button_Object.Image = ((System.Drawing.Image)(resources.GetObject("Button_Object.Image")));
            this.Button_Object.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_Object.Location = new System.Drawing.Point(215, 33);
            this.Button_Object.Name = "Button_Object";
            this.Button_Object.Size = new System.Drawing.Size(199, 59);
            this.Button_Object.TabIndex = 28;
            this.Button_Object.Text = "OBJECT";
            this.Button_Object.UseVisualStyleBackColor = true;
            this.Button_Object.Click += new System.EventHandler(this.Type_Click);
            // 
            // Button_Target
            // 
            this.Button_Target.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Target.FlatAppearance.BorderSize = 0;
            this.Button_Target.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Target.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Target.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Target.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Target.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.Button_Target.ForeColor = System.Drawing.Color.White;
            this.Button_Target.Image = ((System.Drawing.Image)(resources.GetObject("Button_Target.Image")));
            this.Button_Target.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_Target.Location = new System.Drawing.Point(12, 33);
            this.Button_Target.Name = "Button_Target";
            this.Button_Target.Size = new System.Drawing.Size(199, 59);
            this.Button_Target.TabIndex = 27;
            this.Button_Target.Text = "TARGET";
            this.Button_Target.UseVisualStyleBackColor = true;
            this.Button_Target.Click += new System.EventHandler(this.Type_Click);
            // 
            // Button_Live
            // 
            this.Button_Live.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.Button_Live.FlatAppearance.BorderSize = 0;
            this.Button_Live.FlatAppearance.CheckedBackColor = System.Drawing.Color.Black;
            this.Button_Live.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Live.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Live.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Live.Image = global::T_Align.Properties.Resources.Live;
            this.Button_Live.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_Live.Location = new System.Drawing.Point(1703, 46);
            this.Button_Live.Name = "Button_Live";
            this.Button_Live.Size = new System.Drawing.Size(50, 50);
            this.Button_Live.TabIndex = 26;
            this.Button_Live.UseVisualStyleBackColor = true;
            this.Button_Live.Click += new System.EventHandler(this.Button_Live_Click);
            // 
            // Button_Grab
            // 
            this.Button_Grab.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.Button_Grab.FlatAppearance.BorderSize = 0;
            this.Button_Grab.FlatAppearance.CheckedBackColor = System.Drawing.Color.Black;
            this.Button_Grab.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Grab.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Grab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Grab.Image = global::T_Align.Properties.Resources.Grab;
            this.Button_Grab.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_Grab.Location = new System.Drawing.Point(1647, 46);
            this.Button_Grab.Name = "Button_Grab";
            this.Button_Grab.Size = new System.Drawing.Size(50, 50);
            this.Button_Grab.TabIndex = 25;
            this.Button_Grab.UseVisualStyleBackColor = true;
            this.Button_Grab.Click += new System.EventHandler(this.Button_Grab_Click);
            // 
            // Button_Clear
            // 
            this.Button_Clear.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.Button_Clear.FlatAppearance.BorderSize = 0;
            this.Button_Clear.FlatAppearance.CheckedBackColor = System.Drawing.Color.Black;
            this.Button_Clear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Clear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Clear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Clear.Image = global::T_Align.Properties.Resources.Clear;
            this.Button_Clear.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_Clear.Location = new System.Drawing.Point(1759, 46);
            this.Button_Clear.Name = "Button_Clear";
            this.Button_Clear.Size = new System.Drawing.Size(50, 50);
            this.Button_Clear.TabIndex = 24;
            this.Button_Clear.UseVisualStyleBackColor = true;
            this.Button_Clear.Click += new System.EventHandler(this.Button_Clear_Click);
            // 
            // Button_Unit8
            // 
            this.Button_Unit8.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.Button_Unit8.FlatAppearance.BorderSize = 0;
            this.Button_Unit8.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Unit8.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Unit8.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Unit8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Unit8.Font = new System.Drawing.Font("Franklin Gothic Demi", 13F);
            this.Button_Unit8.ForeColor = System.Drawing.Color.White;
            this.Button_Unit8.Image = ((System.Drawing.Image)(resources.GetObject("Button_Unit8.Image")));
            this.Button_Unit8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_Unit8.Location = new System.Drawing.Point(1143, 46);
            this.Button_Unit8.Name = "Button_Unit8";
            this.Button_Unit8.Size = new System.Drawing.Size(91, 50);
            this.Button_Unit8.TabIndex = 21;
            this.Button_Unit8.Text = "Unit8";
            this.Button_Unit8.UseVisualStyleBackColor = true;
            this.Button_Unit8.Click += new System.EventHandler(this.Unit_Click);
            // 
            // Button_Unit7
            // 
            this.Button_Unit7.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.Button_Unit7.FlatAppearance.BorderSize = 0;
            this.Button_Unit7.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Unit7.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Unit7.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Unit7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Unit7.Font = new System.Drawing.Font("Franklin Gothic Demi", 13F);
            this.Button_Unit7.ForeColor = System.Drawing.Color.White;
            this.Button_Unit7.Image = ((System.Drawing.Image)(resources.GetObject("Button_Unit7.Image")));
            this.Button_Unit7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_Unit7.Location = new System.Drawing.Point(1046, 46);
            this.Button_Unit7.Name = "Button_Unit7";
            this.Button_Unit7.Size = new System.Drawing.Size(91, 50);
            this.Button_Unit7.TabIndex = 20;
            this.Button_Unit7.Text = "Unit7";
            this.Button_Unit7.UseVisualStyleBackColor = true;
            this.Button_Unit7.Click += new System.EventHandler(this.Unit_Click);
            // 
            // Button_Unit6
            // 
            this.Button_Unit6.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.Button_Unit6.FlatAppearance.BorderSize = 0;
            this.Button_Unit6.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Unit6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Unit6.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Unit6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Unit6.Font = new System.Drawing.Font("Franklin Gothic Demi", 13F);
            this.Button_Unit6.ForeColor = System.Drawing.Color.White;
            this.Button_Unit6.Image = ((System.Drawing.Image)(resources.GetObject("Button_Unit6.Image")));
            this.Button_Unit6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_Unit6.Location = new System.Drawing.Point(949, 46);
            this.Button_Unit6.Name = "Button_Unit6";
            this.Button_Unit6.Size = new System.Drawing.Size(91, 50);
            this.Button_Unit6.TabIndex = 19;
            this.Button_Unit6.Text = "Unit6";
            this.Button_Unit6.UseVisualStyleBackColor = true;
            this.Button_Unit6.Click += new System.EventHandler(this.Unit_Click);
            // 
            // Button_Unit5
            // 
            this.Button_Unit5.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.Button_Unit5.FlatAppearance.BorderSize = 0;
            this.Button_Unit5.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Unit5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Unit5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Unit5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Unit5.Font = new System.Drawing.Font("Franklin Gothic Demi", 13F);
            this.Button_Unit5.ForeColor = System.Drawing.Color.White;
            this.Button_Unit5.Image = ((System.Drawing.Image)(resources.GetObject("Button_Unit5.Image")));
            this.Button_Unit5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_Unit5.Location = new System.Drawing.Point(852, 46);
            this.Button_Unit5.Name = "Button_Unit5";
            this.Button_Unit5.Size = new System.Drawing.Size(91, 50);
            this.Button_Unit5.TabIndex = 18;
            this.Button_Unit5.Text = "Unit5";
            this.Button_Unit5.UseVisualStyleBackColor = true;
            this.Button_Unit5.Click += new System.EventHandler(this.Unit_Click);
            // 
            // Button_Unit4
            // 
            this.Button_Unit4.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.Button_Unit4.FlatAppearance.BorderSize = 0;
            this.Button_Unit4.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Unit4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Unit4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Unit4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Unit4.Font = new System.Drawing.Font("Franklin Gothic Demi", 13F);
            this.Button_Unit4.ForeColor = System.Drawing.Color.White;
            this.Button_Unit4.Image = ((System.Drawing.Image)(resources.GetObject("Button_Unit4.Image")));
            this.Button_Unit4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_Unit4.Location = new System.Drawing.Point(755, 46);
            this.Button_Unit4.Name = "Button_Unit4";
            this.Button_Unit4.Size = new System.Drawing.Size(91, 50);
            this.Button_Unit4.TabIndex = 17;
            this.Button_Unit4.Text = "Unit4";
            this.Button_Unit4.UseVisualStyleBackColor = true;
            this.Button_Unit4.Click += new System.EventHandler(this.Unit_Click);
            // 
            // Button_Unit3
            // 
            this.Button_Unit3.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.Button_Unit3.FlatAppearance.BorderSize = 0;
            this.Button_Unit3.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Unit3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Unit3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Unit3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Unit3.Font = new System.Drawing.Font("Franklin Gothic Demi", 13F);
            this.Button_Unit3.ForeColor = System.Drawing.Color.White;
            this.Button_Unit3.Image = ((System.Drawing.Image)(resources.GetObject("Button_Unit3.Image")));
            this.Button_Unit3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_Unit3.Location = new System.Drawing.Point(658, 46);
            this.Button_Unit3.Name = "Button_Unit3";
            this.Button_Unit3.Size = new System.Drawing.Size(91, 50);
            this.Button_Unit3.TabIndex = 16;
            this.Button_Unit3.Text = "Unit3";
            this.Button_Unit3.UseVisualStyleBackColor = true;
            this.Button_Unit3.Click += new System.EventHandler(this.Unit_Click);
            // 
            // Button_Unit2
            // 
            this.Button_Unit2.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.Button_Unit2.FlatAppearance.BorderSize = 0;
            this.Button_Unit2.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Unit2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Unit2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Unit2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Unit2.Font = new System.Drawing.Font("Franklin Gothic Demi", 13F);
            this.Button_Unit2.ForeColor = System.Drawing.Color.White;
            this.Button_Unit2.Image = ((System.Drawing.Image)(resources.GetObject("Button_Unit2.Image")));
            this.Button_Unit2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_Unit2.Location = new System.Drawing.Point(561, 46);
            this.Button_Unit2.Name = "Button_Unit2";
            this.Button_Unit2.Size = new System.Drawing.Size(91, 50);
            this.Button_Unit2.TabIndex = 15;
            this.Button_Unit2.Text = "Unit2";
            this.Button_Unit2.UseVisualStyleBackColor = true;
            this.Button_Unit2.Click += new System.EventHandler(this.Unit_Click);
            // 
            // Button_Unit1
            // 
            this.Button_Unit1.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.Button_Unit1.FlatAppearance.BorderSize = 0;
            this.Button_Unit1.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Unit1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Unit1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_Unit1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Unit1.Font = new System.Drawing.Font("Franklin Gothic Demi", 13F);
            this.Button_Unit1.ForeColor = System.Drawing.Color.White;
            this.Button_Unit1.Image = ((System.Drawing.Image)(resources.GetObject("Button_Unit1.Image")));
            this.Button_Unit1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_Unit1.Location = new System.Drawing.Point(464, 46);
            this.Button_Unit1.Name = "Button_Unit1";
            this.Button_Unit1.Size = new System.Drawing.Size(91, 50);
            this.Button_Unit1.TabIndex = 14;
            this.Button_Unit1.Text = "Unit1";
            this.Button_Unit1.UseVisualStyleBackColor = true;
            this.Button_Unit1.Click += new System.EventHandler(this.Unit_Click);
            // 
            // Triger_panel
            // 
            this.Triger_panel.Controls.Add(this.Triger_Label);
            this.Triger_panel.Location = new System.Drawing.Point(809, 102);
            this.Triger_panel.Name = "Triger_panel";
            this.Triger_panel.Size = new System.Drawing.Size(300, 50);
            this.Triger_panel.TabIndex = 48;
            this.Triger_panel.Visible = false;
            // 
            // Triger_Label
            // 
            this.Triger_Label.Font = new System.Drawing.Font("Franklin Gothic Demi", 24F);
            this.Triger_Label.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Triger_Label.Location = new System.Drawing.Point(3, 7);
            this.Triger_Label.Name = "Triger_Label";
            this.Triger_Label.Size = new System.Drawing.Size(294, 37);
            this.Triger_Label.TabIndex = 10;
            this.Triger_Label.Text = "TRIGER COUNT : ";
            this.Triger_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Calibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Controls.Add(this.Triger_panel);
            this.Controls.Add(this.Button_CTQ);
            this.Controls.Add(this.button16);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.Button_Mark4);
            this.Controls.Add(this.Button_Mark2);
            this.Controls.Add(this.Button_Mark3);
            this.Controls.Add(this.Button_Mark1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.Button_Cal_Result);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel10);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel12);
            this.Controls.Add(this.Button_Mesure);
            this.Controls.Add(this.Button_Custom_Line);
            this.Controls.Add(this.Button_Center_Line);
            this.Controls.Add(this.Button_Object);
            this.Controls.Add(this.Button_Target);
            this.Controls.Add(this.Button_Live);
            this.Controls.Add(this.Button_Grab);
            this.Controls.Add(this.Button_Clear);
            this.Controls.Add(this.Cal_Display2);
            this.Controls.Add(this.Cal_Display1);
            this.Controls.Add(this.Button_Unit8);
            this.Controls.Add(this.Button_Unit7);
            this.Controls.Add(this.Button_Unit6);
            this.Controls.Add(this.Button_Unit5);
            this.Controls.Add(this.Button_Unit4);
            this.Controls.Add(this.Button_Unit3);
            this.Controls.Add(this.Button_Unit2);
            this.Controls.Add(this.Button_Unit1);
            this.DoubleBuffered = true;
            this.Name = "Calibration";
            this.Size = new System.Drawing.Size(1920, 965);
            this.Load += new System.EventHandler(this.Calibration_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Cal_Display1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cal_Display2)).EndInit();
            this.panel12.ResumeLayout(false);
            this.panel12.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Mark_Display)).EndInit();
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Nud_Cal_Pitch_T2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Nud_Cal_Pitch_Y1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Nud_Cal_Pitch_X0)).EndInit();
            this.Triger_panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Button_Unit8;
        private System.Windows.Forms.Button Button_Unit7;
        private System.Windows.Forms.Button Button_Unit6;
        private System.Windows.Forms.Button Button_Unit5;
        private System.Windows.Forms.Button Button_Unit4;
        private System.Windows.Forms.Button Button_Unit3;
        private System.Windows.Forms.Button Button_Unit2;
        private System.Windows.Forms.Button Button_Unit1;
        private System.Windows.Forms.Button Button_Clear;
        private System.Windows.Forms.Button Button_Grab;
        private System.Windows.Forms.Button Button_Live;
        private System.Windows.Forms.Button Button_Target;
        private System.Windows.Forms.Button Button_Object;
        private System.Windows.Forms.Button Button_Center_Line;
        private System.Windows.Forms.Button Button_Custom_Line;
        private System.Windows.Forms.Button Button_Mesure;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Textbox_RY;
        private System.Windows.Forms.TextBox Textbox_RX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Explanation_Label2;
        private System.Windows.Forms.TextBox Textbox_LY;
        private System.Windows.Forms.TextBox Textbox_LX;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox Textbox_Mouse_Bright;
        private System.Windows.Forms.TextBox Textbox_Mouse_Y;
        private System.Windows.Forms.TextBox Textbox_Mouse_X;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button Button_Cal_Result;
        private System.Windows.Forms.Panel panel4;
        private Cognex.VisionPro.Display.CogDisplay Mark_Display;
        private System.Windows.Forms.Button Button_Copy;
        private System.Windows.Forms.Button Button_Find;
        private System.Windows.Forms.Button Button_Mark3;
        private System.Windows.Forms.Button Button_Mark1;
        private System.Windows.Forms.Button Button_Mark4;
        private System.Windows.Forms.Button Button_Mark2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button16;
        public Cognex.VisionPro.Display.CogDisplay Cal_Display1;
        public Cognex.VisionPro.Display.CogDisplay Cal_Display2;
        private System.Windows.Forms.NumericUpDown Nud_Cal_Pitch_T2;
        private System.Windows.Forms.NumericUpDown Nud_Cal_Pitch_Y1;
        private System.Windows.Forms.NumericUpDown Nud_Cal_Pitch_X0;
        private System.Windows.Forms.Button Button_CTQ;
        private System.Windows.Forms.Panel Triger_panel;
        private System.Windows.Forms.Label Triger_Label;
    }
}
