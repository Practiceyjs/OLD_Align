
namespace T_Align
{
    partial class Cog_Panel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Cog_Panel));
            this.Pattern_Display = new Cognex.VisionPro.Display.CogDisplay();
            ((System.ComponentModel.ISupportInitialize)(this.Pattern_Display)).BeginInit();
            this.SuspendLayout();
            // 
            // Pattern_Display
            // 
            this.Pattern_Display.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.Pattern_Display.ColorMapLowerRoiLimit = 0D;
            this.Pattern_Display.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.Pattern_Display.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.Pattern_Display.ColorMapUpperRoiLimit = 1D;
            this.Pattern_Display.DoubleTapZoomCycleLength = 2;
            this.Pattern_Display.DoubleTapZoomSensitivity = 2.5D;
            this.Pattern_Display.Location = new System.Drawing.Point(0, 0);
            this.Pattern_Display.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.Pattern_Display.MouseWheelSensitivity = 1D;
            this.Pattern_Display.Name = "Pattern_Display";
            this.Pattern_Display.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("Pattern_Display.OcxState")));
            this.Pattern_Display.Size = new System.Drawing.Size(268, 243);
            this.Pattern_Display.TabIndex = 102;
            // 
            // Cog_Panel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.Pattern_Display);
            this.Name = "Cog_Panel";
            this.Size = new System.Drawing.Size(268, 243);
            ((System.ComponentModel.ISupportInitialize)(this.Pattern_Display)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public Cognex.VisionPro.Display.CogDisplay Pattern_Display;
    }
}
