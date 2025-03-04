namespace T_Align
{
    partial class Manual_Mark
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Manual_Mark));
            this.Title_toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cogDisplay2 = new Cognex.VisionPro.Display.CogDisplay();
            this.Button_NG = new System.Windows.Forms.Button();
            this.Button_OK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton_10 = new System.Windows.Forms.RadioButton();
            this.radioButton_7 = new System.Windows.Forms.RadioButton();
            this.radioButton_5 = new System.Windows.Forms.RadioButton();
            this.radioButton_3 = new System.Windows.Forms.RadioButton();
            this.label50 = new System.Windows.Forms.Label();
            this.Button_L = new System.Windows.Forms.Button();
            this.Button_R = new System.Windows.Forms.Button();
            this.Button_RR = new System.Windows.Forms.Button();
            this.Button_U = new System.Windows.Forms.Button();
            this.Button_LL = new System.Windows.Forms.Button();
            this.Button_D = new System.Windows.Forms.Button();
            this.Button_DD = new System.Windows.Forms.Button();
            this.Button_UU = new System.Windows.Forms.Button();
            this.cogDisplay1 = new Cognex.VisionPro.Display.CogDisplay();
            this.Title_toolStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay2)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay1)).BeginInit();
            this.SuspendLayout();
            // 
            // Title_toolStrip
            // 
            this.Title_toolStrip.AutoSize = false;
            this.Title_toolStrip.BackColor = System.Drawing.Color.Black;
            this.Title_toolStrip.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold);
            this.Title_toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.toolStripSeparator2});
            this.Title_toolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.Title_toolStrip.Location = new System.Drawing.Point(0, 0);
            this.Title_toolStrip.Name = "Title_toolStrip";
            this.Title_toolStrip.Size = new System.Drawing.Size(1418, 57);
            this.Title_toolStrip.TabIndex = 5;
            this.Title_toolStrip.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AutoSize = false;
            this.toolStripSeparator1.ForeColor = System.Drawing.Color.White;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 58);
            this.toolStripSeparator1.Visible = false;
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.AutoSize = false;
            this.toolStripLabel2.BackColor = System.Drawing.Color.DarkRed;
            this.toolStripLabel2.Font = new System.Drawing.Font("Franklin Gothic Demi", 24F);
            this.toolStripLabel2.ForeColor = System.Drawing.Color.White;
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(300, 58);
            this.toolStripLabel2.Text = "MANUAL MARK";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AutoSize = false;
            this.toolStripSeparator2.ForeColor = System.Drawing.Color.White;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 58);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.panel1.Controls.Add(this.cogDisplay2);
            this.panel1.Controls.Add(this.Button_NG);
            this.panel1.Controls.Add(this.Button_OK);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.cogDisplay1);
            this.panel1.Location = new System.Drawing.Point(6, 58);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1406, 836);
            this.panel1.TabIndex = 6;
            // 
            // cogDisplay2
            // 
            this.cogDisplay2.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay2.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay2.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay2.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay2.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay2.DoubleTapZoomCycleLength = 2;
            this.cogDisplay2.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplay2.Location = new System.Drawing.Point(1142, 610);
            this.cogDisplay2.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay2.MouseWheelSensitivity = 1D;
            this.cogDisplay2.Name = "cogDisplay2";
            this.cogDisplay2.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay2.OcxState")));
            this.cogDisplay2.Size = new System.Drawing.Size(253, 220);
            this.cogDisplay2.TabIndex = 146;
            // 
            // Button_NG
            // 
            this.Button_NG.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.Button_NG.FlatAppearance.BorderSize = 0;
            this.Button_NG.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_NG.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_NG.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_NG.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_NG.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.Button_NG.ForeColor = System.Drawing.Color.White;
            this.Button_NG.Image = global::T_Align.Properties.Resources.Button6;
            this.Button_NG.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_NG.Location = new System.Drawing.Point(1277, 338);
            this.Button_NG.Name = "Button_NG";
            this.Button_NG.Size = new System.Drawing.Size(111, 37);
            this.Button_NG.TabIndex = 145;
            this.Button_NG.Text = "NG";
            this.Button_NG.UseVisualStyleBackColor = true;
            this.Button_NG.Click += new System.EventHandler(this.Button_NG_Click);
            // 
            // Button_OK
            // 
            this.Button_OK.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.Button_OK.FlatAppearance.BorderSize = 0;
            this.Button_OK.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_OK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_OK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_OK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_OK.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.Button_OK.ForeColor = System.Drawing.Color.White;
            this.Button_OK.Image = global::T_Align.Properties.Resources.Button6;
            this.Button_OK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_OK.Location = new System.Drawing.Point(1150, 338);
            this.Button_OK.Name = "Button_OK";
            this.Button_OK.Size = new System.Drawing.Size(111, 37);
            this.Button_OK.TabIndex = 144;
            this.Button_OK.Text = "OK";
            this.Button_OK.UseVisualStyleBackColor = true;
            this.Button_OK.Click += new System.EventHandler(this.Button_OK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton_10);
            this.groupBox1.Controls.Add(this.radioButton_7);
            this.groupBox1.Controls.Add(this.radioButton_5);
            this.groupBox1.Controls.Add(this.radioButton_3);
            this.groupBox1.Controls.Add(this.label50);
            this.groupBox1.Controls.Add(this.Button_L);
            this.groupBox1.Controls.Add(this.Button_R);
            this.groupBox1.Controls.Add(this.Button_RR);
            this.groupBox1.Controls.Add(this.Button_U);
            this.groupBox1.Controls.Add(this.Button_LL);
            this.groupBox1.Controls.Add(this.Button_D);
            this.groupBox1.Controls.Add(this.Button_DD);
            this.groupBox1.Controls.Add(this.Button_UU);
            this.groupBox1.Location = new System.Drawing.Point(1142, -1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(253, 322);
            this.groupBox1.TabIndex = 143;
            this.groupBox1.TabStop = false;
            // 
            // radioButton_10
            // 
            this.radioButton_10.AutoSize = true;
            this.radioButton_10.Checked = true;
            this.radioButton_10.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.radioButton_10.Location = new System.Drawing.Point(185, 63);
            this.radioButton_10.Name = "radioButton_10";
            this.radioButton_10.Size = new System.Drawing.Size(56, 32);
            this.radioButton_10.TabIndex = 146;
            this.radioButton_10.TabStop = true;
            this.radioButton_10.Text = "10";
            this.radioButton_10.UseVisualStyleBackColor = true;
            this.radioButton_10.CheckedChanged += new System.EventHandler(this.Pixel_Click);
            // 
            // radioButton_7
            // 
            this.radioButton_7.AutoSize = true;
            this.radioButton_7.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.radioButton_7.Location = new System.Drawing.Point(130, 63);
            this.radioButton_7.Name = "radioButton_7";
            this.radioButton_7.Size = new System.Drawing.Size(43, 32);
            this.radioButton_7.TabIndex = 145;
            this.radioButton_7.Text = "7";
            this.radioButton_7.UseVisualStyleBackColor = true;
            this.radioButton_7.CheckedChanged += new System.EventHandler(this.Pixel_Click);
            // 
            // radioButton_5
            // 
            this.radioButton_5.AutoSize = true;
            this.radioButton_5.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.radioButton_5.Location = new System.Drawing.Point(76, 63);
            this.radioButton_5.Name = "radioButton_5";
            this.radioButton_5.Size = new System.Drawing.Size(43, 32);
            this.radioButton_5.TabIndex = 144;
            this.radioButton_5.Text = "5";
            this.radioButton_5.UseVisualStyleBackColor = true;
            this.radioButton_5.CheckedChanged += new System.EventHandler(this.Pixel_Click);
            // 
            // radioButton_3
            // 
            this.radioButton_3.AutoSize = true;
            this.radioButton_3.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.radioButton_3.Location = new System.Drawing.Point(18, 63);
            this.radioButton_3.Name = "radioButton_3";
            this.radioButton_3.Size = new System.Drawing.Size(43, 32);
            this.radioButton_3.TabIndex = 143;
            this.radioButton_3.Text = "3";
            this.radioButton_3.UseVisualStyleBackColor = true;
            this.radioButton_3.CheckedChanged += new System.EventHandler(this.Pixel_Click);
            // 
            // label50
            // 
            this.label50.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.label50.Font = new System.Drawing.Font("Franklin Gothic Demi", 20F);
            this.label50.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label50.Location = new System.Drawing.Point(5, 11);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(243, 37);
            this.label50.TabIndex = 142;
            this.label50.Text = "Pixel Move";
            this.label50.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Button_L
            // 
            this.Button_L.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.Button_L.FlatAppearance.BorderSize = 0;
            this.Button_L.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_L.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_L.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_L.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_L.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.Button_L.ForeColor = System.Drawing.Color.White;
            this.Button_L.Image = global::T_Align.Properties.Resources.M_L;
            this.Button_L.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_L.Location = new System.Drawing.Point(64, 183);
            this.Button_L.Name = "Button_L";
            this.Button_L.Size = new System.Drawing.Size(16, 48);
            this.Button_L.TabIndex = 105;
            this.Button_L.UseVisualStyleBackColor = true;
            this.Button_L.Click += new System.EventHandler(this.Move_Click);
            // 
            // Button_R
            // 
            this.Button_R.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.Button_R.FlatAppearance.BorderSize = 0;
            this.Button_R.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_R.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_R.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_R.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_R.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.Button_R.ForeColor = System.Drawing.Color.White;
            this.Button_R.Image = global::T_Align.Properties.Resources.M_R;
            this.Button_R.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_R.Location = new System.Drawing.Point(163, 184);
            this.Button_R.Name = "Button_R";
            this.Button_R.Size = new System.Drawing.Size(16, 48);
            this.Button_R.TabIndex = 106;
            this.Button_R.UseVisualStyleBackColor = true;
            this.Button_R.Click += new System.EventHandler(this.Move_Click);
            // 
            // Button_RR
            // 
            this.Button_RR.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.Button_RR.FlatAppearance.BorderSize = 0;
            this.Button_RR.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_RR.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_RR.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_RR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_RR.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.Button_RR.ForeColor = System.Drawing.Color.White;
            this.Button_RR.Image = global::T_Align.Properties.Resources.M_RR;
            this.Button_RR.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_RR.Location = new System.Drawing.Point(190, 184);
            this.Button_RR.Name = "Button_RR";
            this.Button_RR.Size = new System.Drawing.Size(24, 48);
            this.Button_RR.TabIndex = 112;
            this.Button_RR.UseVisualStyleBackColor = true;
            this.Button_RR.Click += new System.EventHandler(this.Move_Click);
            // 
            // Button_U
            // 
            this.Button_U.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.Button_U.FlatAppearance.BorderSize = 0;
            this.Button_U.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_U.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_U.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_U.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_U.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.Button_U.ForeColor = System.Drawing.Color.White;
            this.Button_U.Image = global::T_Align.Properties.Resources.M_U;
            this.Button_U.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_U.Location = new System.Drawing.Point(98, 151);
            this.Button_U.Name = "Button_U";
            this.Button_U.Size = new System.Drawing.Size(48, 16);
            this.Button_U.TabIndex = 107;
            this.Button_U.UseVisualStyleBackColor = true;
            this.Button_U.Click += new System.EventHandler(this.Move_Click);
            // 
            // Button_LL
            // 
            this.Button_LL.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.Button_LL.FlatAppearance.BorderSize = 0;
            this.Button_LL.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_LL.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_LL.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_LL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_LL.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.Button_LL.ForeColor = System.Drawing.Color.White;
            this.Button_LL.Image = global::T_Align.Properties.Resources.M_LL;
            this.Button_LL.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_LL.Location = new System.Drawing.Point(29, 183);
            this.Button_LL.Name = "Button_LL";
            this.Button_LL.Size = new System.Drawing.Size(24, 48);
            this.Button_LL.TabIndex = 111;
            this.Button_LL.UseVisualStyleBackColor = true;
            this.Button_LL.Click += new System.EventHandler(this.Move_Click);
            // 
            // Button_D
            // 
            this.Button_D.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.Button_D.FlatAppearance.BorderSize = 0;
            this.Button_D.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_D.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_D.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_D.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_D.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.Button_D.ForeColor = System.Drawing.Color.White;
            this.Button_D.Image = global::T_Align.Properties.Resources.M_D;
            this.Button_D.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_D.Location = new System.Drawing.Point(98, 248);
            this.Button_D.Name = "Button_D";
            this.Button_D.Size = new System.Drawing.Size(48, 16);
            this.Button_D.TabIndex = 108;
            this.Button_D.UseVisualStyleBackColor = true;
            this.Button_D.Click += new System.EventHandler(this.Move_Click);
            // 
            // Button_DD
            // 
            this.Button_DD.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.Button_DD.FlatAppearance.BorderSize = 0;
            this.Button_DD.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_DD.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_DD.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_DD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_DD.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.Button_DD.ForeColor = System.Drawing.Color.White;
            this.Button_DD.Image = global::T_Align.Properties.Resources.M_DD;
            this.Button_DD.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_DD.Location = new System.Drawing.Point(98, 275);
            this.Button_DD.Name = "Button_DD";
            this.Button_DD.Size = new System.Drawing.Size(48, 24);
            this.Button_DD.TabIndex = 110;
            this.Button_DD.UseVisualStyleBackColor = true;
            this.Button_DD.Click += new System.EventHandler(this.Move_Click);
            // 
            // Button_UU
            // 
            this.Button_UU.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.Button_UU.FlatAppearance.BorderSize = 0;
            this.Button_UU.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_UU.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_UU.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Button_UU.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_UU.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.Button_UU.ForeColor = System.Drawing.Color.White;
            this.Button_UU.Image = global::T_Align.Properties.Resources.M_UU;
            this.Button_UU.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Button_UU.Location = new System.Drawing.Point(98, 116);
            this.Button_UU.Name = "Button_UU";
            this.Button_UU.Size = new System.Drawing.Size(48, 24);
            this.Button_UU.TabIndex = 109;
            this.Button_UU.UseVisualStyleBackColor = true;
            this.Button_UU.Click += new System.EventHandler(this.Move_Click);
            // 
            // cogDisplay1
            // 
            this.cogDisplay1.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay1.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay1.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay1.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay1.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay1.DoubleTapZoomCycleLength = 2;
            this.cogDisplay1.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplay1.Location = new System.Drawing.Point(6, 6);
            this.cogDisplay1.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay1.MouseWheelSensitivity = 1D;
            this.cogDisplay1.Name = "cogDisplay1";
            this.cogDisplay1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay1.OcxState")));
            this.cogDisplay1.Size = new System.Drawing.Size(1125, 824);
            this.cogDisplay1.TabIndex = 0;
            this.cogDisplay1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cogDisplay1_MouseDown);
            this.cogDisplay1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cogDisplay1_MouseUp);
            this.cogDisplay1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.cogDisplay1_MouseMove);
            // 
            // Manual_Mark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1418, 900);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Title_toolStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Manual_Mark";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manual_Mark";
            this.Title_toolStrip.ResumeLayout(false);
            this.Title_toolStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip Title_toolStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Panel panel1;
        private Cognex.VisionPro.Display.CogDisplay cogDisplay1;
        private System.Windows.Forms.Button Button_RR;
        private System.Windows.Forms.Button Button_LL;
        private System.Windows.Forms.Button Button_DD;
        private System.Windows.Forms.Button Button_UU;
        private System.Windows.Forms.Button Button_D;
        private System.Windows.Forms.Button Button_U;
        private System.Windows.Forms.Button Button_R;
        private System.Windows.Forms.Button Button_L;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton_10;
        private System.Windows.Forms.RadioButton radioButton_7;
        private System.Windows.Forms.RadioButton radioButton_5;
        private System.Windows.Forms.RadioButton radioButton_3;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Button Button_NG;
        private System.Windows.Forms.Button Button_OK;
        private Cognex.VisionPro.Display.CogDisplay cogDisplay2;
    }
}