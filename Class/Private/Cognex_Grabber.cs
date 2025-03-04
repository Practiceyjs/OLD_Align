using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognex.VisionPro;

namespace T_Align
{
    class Cognex_Grabber
    {
        ICogFrameGrabber[] CogFrameGrabbers;
        ICogAcqFifo[] cogAcqFifos;

        Cognex_Camera[] Cam = new Cognex_Camera[8];

        public bool[] Cognex_Camera_Set(string[] _IP_add)
        {
            bool[] Connect = new bool[8];
            CogFrameGrabbers Grabbers = new CogFrameGrabbers();
            CogFrameGrabbers = new ICogFrameGrabber[Grabbers.Count];
            cogAcqFifos = new ICogAcqFifo[_IP_add.Length];
            ICogAcqFifo[] Sort_Fifo = new ICogAcqFifo[Grabbers.Count];
            for (int LoopCount = 0; LoopCount < Grabbers.Count; LoopCount++)
            {
                CogFrameGrabbers[LoopCount] = Grabbers[LoopCount];
                Sort_Fifo[LoopCount] = CogFrameGrabbers[LoopCount].CreateAcqFifo("Generic GigEVision (Mono)", Cognex.VisionPro.CogAcqFifoPixelFormatConstants.Format8Grey, 0, true);
            }
            for (int i = 0; i < _IP_add.Length; i++)
            {
                if (CogFrameGrabbers == null)
                    break;

                for (int k = 0; k < CogFrameGrabbers.Length; k++)
                {
                    if (CogFrameGrabbers[k].OwnedGigEAccess.HostIPAddress == _IP_add[i])
                    {
                        for (int l = 0; l < 8; l++)
                            if (ClassINI.ReadProject("CAMERA", "IP" + l.ToString()) == _IP_add[i])
                            {
                                Connect[l] = true;
                                cogAcqFifos[i] = Sort_Fifo[k];
                            }
                        break;
                    }
                }
            }
            for (int LoopCount = 0; LoopCount < Grabbers.Count; LoopCount++)
            {
                if(Connect[LoopCount])
                {
                    Cam[LoopCount] = new Cognex_Camera();
                    Cam[LoopCount].Camera_Set(cogAcqFifos[LoopCount], LoopCount);
                }
            }
            return Connect;
        }
    }
}
