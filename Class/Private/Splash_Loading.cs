using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Align
{
    class Splash_Loading
    {
        static Loading SF = null;
        public static void ShowSplashScreen()
        {
            if (SF == null)
            {
                SF = new Loading();
                SF.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                SF.ShowSplashScreen();
            }
        }

        public static void CloseSplashScreen()
        {
            if (SF != null)
            {
                SF.CloseSplashScreen();
                SF = null;
            }
        }

        public static void UpdateStatusText(string Text)
        {
            while (SF == null) { }
            if (!SF.Equals(null))
                SF.UpdataStatusText(Text);
        }
    }
}
