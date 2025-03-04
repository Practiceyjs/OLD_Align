
namespace T_Align
{
    partial class Type_Display
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Type_Display));
            this.border_Panel1 = new T_Align.Border_Panel();
            this.cogDisplay4 = new Cognex.VisionPro.Display.CogDisplay();
            this.cogDisplay3 = new Cognex.VisionPro.Display.CogDisplay();
            this.cogDisplay2 = new Cognex.VisionPro.Display.CogDisplay();
            this.cogDisplay1 = new Cognex.VisionPro.Display.CogDisplay();
            this.label1 = new System.Windows.Forms.Label();
            this.border_Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay1)).BeginInit();
            this.SuspendLayout();
            // 
            // border_Panel1
            // 
            this.border_Panel1.BackColor = ETC_Option.Main_Color;
            this.border_Panel1.Border = 6;
            this.border_Panel1.Border_Color = System.Drawing.Color.Black;
            this.border_Panel1.Controls.Add(this.cogDisplay4);
            this.border_Panel1.Controls.Add(this.cogDisplay3);
            this.border_Panel1.Controls.Add(this.cogDisplay2);
            this.border_Panel1.Controls.Add(this.cogDisplay1);
            this.border_Panel1.Controls.Add(this.label1);
            this.border_Panel1.Location = new System.Drawing.Point(0, 0);
            this.border_Panel1.Name = "border_Panel1";
            this.border_Panel1.Size = new System.Drawing.Size(582, 476);
            this.border_Panel1.TabIndex = 0;
            // 
            // cogDisplay4
            // 
            this.cogDisplay4.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay4.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay4.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay4.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay4.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay4.DoubleTapZoomCycleLength = 2;
            this.cogDisplay4.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplay4.Location = new System.Drawing.Point(294, 254);
            this.cogDisplay4.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay4.MouseWheelSensitivity = 1D;
            this.cogDisplay4.Name = "cogDisplay4";
            this.cogDisplay4.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay4.OcxState")));
            this.cogDisplay4.Size = new System.Drawing.Size(280, 214);
            this.cogDisplay4.TabIndex = 4;
            // 
            // cogDisplay3
            // 
            this.cogDisplay3.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay3.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay3.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay3.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay3.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay3.DoubleTapZoomCycleLength = 2;
            this.cogDisplay3.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplay3.Location = new System.Drawing.Point(8, 254);
            this.cogDisplay3.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay3.MouseWheelSensitivity = 1D;
            this.cogDisplay3.Name = "cogDisplay3";
            this.cogDisplay3.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay3.OcxState")));
            this.cogDisplay3.Size = new System.Drawing.Size(280, 214);
            this.cogDisplay3.TabIndex = 3;
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
            this.cogDisplay2.Location = new System.Drawing.Point(294, 34);
            this.cogDisplay2.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay2.MouseWheelSensitivity = 1D;
            this.cogDisplay2.Name = "cogDisplay2";
            this.cogDisplay2.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay2.OcxState")));
            this.cogDisplay2.Size = new System.Drawing.Size(280, 214);
            this.cogDisplay2.TabIndex = 2;
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
            this.cogDisplay1.Location = new System.Drawing.Point(8, 34);
            this.cogDisplay1.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay1.MouseWheelSensitivity = 1D;
            this.cogDisplay1.Name = "cogDisplay1";
            this.cogDisplay1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay1.OcxState")));
            this.cogDisplay1.Size = new System.Drawing.Size(280, 214);
            this.cogDisplay1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightGreen;
            this.label1.Font = new System.Drawing.Font("Franklin Gothic Demi", 16F);
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(570, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Type Align";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Type_Display
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.border_Panel1);
            this.Name = "Type_Display";
            this.Size = new System.Drawing.Size(582, 476);
            this.border_Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Border_Panel border_Panel1;
        private System.Windows.Forms.Label label1;
        private Cognex.VisionPro.Display.CogDisplay cogDisplay4;
        private Cognex.VisionPro.Display.CogDisplay cogDisplay3;
        private Cognex.VisionPro.Display.CogDisplay cogDisplay2;
        private Cognex.VisionPro.Display.CogDisplay cogDisplay1;
    }
}
