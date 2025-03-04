
namespace T_Align
{
    partial class ERROR
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ERROR));
            this.Title_toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Close_Button = new System.Windows.Forms.Button();
            this.Error_Label = new System.Windows.Forms.Label();
            this.Title_toolStrip.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.Title_toolStrip.Size = new System.Drawing.Size(490, 57);
            this.Title_toolStrip.TabIndex = 4;
            this.Title_toolStrip.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AutoSize = false;
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
            this.toolStripLabel2.Size = new System.Drawing.Size(150, 58);
            this.toolStripLabel2.Text = "ERROR";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AutoSize = false;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 58);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.panel1.Controls.Add(this.Close_Button);
            this.panel1.Controls.Add(this.Error_Label);
            this.panel1.Location = new System.Drawing.Point(5, 57);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(480, 239);
            this.panel1.TabIndex = 5;
            // 
            // Close_Button
            // 
            this.Close_Button.BackColor = System.Drawing.Color.Black;
            this.Close_Button.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.Close_Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.Close_Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.Close_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Close_Button.Font = new System.Drawing.Font("Franklin Gothic Demi", 24F);
            this.Close_Button.ForeColor = System.Drawing.Color.White;
            this.Close_Button.Location = new System.Drawing.Point(331, 160);
            this.Close_Button.Margin = new System.Windows.Forms.Padding(1);
            this.Close_Button.Name = "Close_Button";
            this.Close_Button.Size = new System.Drawing.Size(112, 50);
            this.Close_Button.TabIndex = 11;
            this.Close_Button.Text = "Close";
            this.Close_Button.UseVisualStyleBackColor = false;
            this.Close_Button.Click += new System.EventHandler(this.Close_Button_Click);
            // 
            // Error_Label
            // 
            this.Error_Label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Error_Label.Font = new System.Drawing.Font("Franklin Gothic Demi", 24F);
            this.Error_Label.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Error_Label.Location = new System.Drawing.Point(29, 49);
            this.Error_Label.Name = "Error_Label";
            this.Error_Label.Size = new System.Drawing.Size(426, 93);
            this.Error_Label.TabIndex = 6;
            this.Error_Label.Text = "Messege";
            this.Error_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ERROR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(490, 302);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Title_toolStrip);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ERROR";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ERROR";
            this.TopMost = true;
            this.Title_toolStrip.ResumeLayout(false);
            this.Title_toolStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip Title_toolStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button Close_Button;
        private System.Windows.Forms.Label Error_Label;
    }
}