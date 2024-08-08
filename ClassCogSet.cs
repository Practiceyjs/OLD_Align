using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognex.VisionPro;
using Cognex.VisionPro.FGGigE;
using System.Drawing;

namespace Align
{
    class ClassCogSet
    {
        static public Cognex.VisionPro.ICogFrameGrabber[] gGrabber_count;
        public Cognex.VisionPro.ICogAcqFifo[] gAcqFifo;

        Main_Frame Frame;

        public void Init_Camera(Main_Frame _Frame)
        {
            Frame = _Frame;
            Cognex.VisionPro.CogFrameGrabbers grabber = new Cognex.VisionPro.CogFrameGrabbers();

            gGrabber_count = new Cognex.VisionPro.ICogFrameGrabber[grabber.Count];
            gAcqFifo = new Cognex.VisionPro.ICogAcqFifo[grabber.Count];
            for (int i = 0; i < grabber.Count; i++)
            {
                gGrabber_count[i] = grabber[i];
                gAcqFifo[i] = gGrabber_count[i].CreateAcqFifo("Generic GigEVision (Mono)", Cognex.VisionPro.CogAcqFifoPixelFormatConstants.Format8Grey, 0, true);
            }
        }

        public Cognex.VisionPro.ICogAcqFifo[] Camera_IP_Set(string[] _IP_add)
        {
            Cognex.VisionPro.ICogAcqFifo[] _returnFifo = new Cognex.VisionPro.ICogAcqFifo[_IP_add.Length];

            for (int i = 0; i < _IP_add.Length; i++)
            {
                if (gGrabber_count == null)
                    break;

                for (int k = 0; k < gGrabber_count.Length; k++)
                {
                    if (gGrabber_count[k].OwnedGigEAccess.HostIPAddress == _IP_add[i])
                    {
                        for (int l = 1; l <= 8; l++)
                            if (ClassINI.ReadProject("Cam", "Cam" + l.ToString() + " IP") == _IP_add[i])
                                Frame.Cam_Label[l - 1].Label_Color = Color.Lime;
                        _returnFifo[i] = gAcqFifo[k];
                        break;
                    }
                }
            }
            return _returnFifo;
        }
    }
}
