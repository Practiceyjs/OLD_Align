using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Euresys;
using System.Threading;
using System.Drawing;
using Cognex.VisionPro;
using Cognex.VisionPro.ImageProcessing;
using System.IO;

namespace T_Align
{
    class Euresys_Grab
    {
        Euresys.GenTL genTL;
        public Euresys.FormatConverter.FormatConverter converter;

        public Euresys.EGrabberCallbackOnDemand grabber;

        volatile bool refreshThreadRunning = false;

        //public delegate void Bitmap_Send_Event(int Device, ICogImage Image);
        //public delegate void Bitmap_Send_Event(int Num);
        //public event Bitmap_Send_Event Send;

        public UInt64 imgWidth;
        public UInt64 imgHeight;
        public String imgFormat;
        Thread Grab_Thread;
        int Deviece;

        public bool Grabber_Set(int _Deviece, Euresys.GenTL _genTL)
        {
            try
            {
                Deviece = _Deviece;
                genTL = _genTL;
                converter = new Euresys.FormatConverter.FormatConverter(genTL);
                grabber = new EGrabberCallbackOnDemand(genTL, 0, Deviece);
                grabber.runScript("./config.js");
                imgWidth = grabber.getWidth();
                imgHeight = grabber.getHeight();
                imgFormat = grabber.getPixelFormat();

                //Enable onCicEvent Callbacks
                grabber.enableCicDataEvent();

                //Grabbber Buffer를 할당한다
                grabber.reallocBuffers(100);

                Grab_Thread = new Thread(Grab);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public virtual void onCicEvent(CicData data)
        {

        }

        public void Live_Start()
        {
            Grab_Thread.Start();
        }

        public void Thread_Stop()
        {
            Grab_Thread.Join();
        }

        private void Grab()
        {
            grabber.start();
            refreshThreadRunning = true;
            try
            {
                while (refreshThreadRunning && grabber != null)
                {
                    if (!COMMON.Program_Run)
                        break;
                    //Wait Buffer
                    Euresys.Buffer buffer = new Euresys.Buffer(grabber.pop());
                    if (!refreshThreadRunning)
                    {
                        break;
                    }
                    if (COMMON.Grab_Image[Deviece+2] == null)
                    {
                        //COMMON.Live_Display[Deviece+2].Image = buffer;
                    }
                    else
                    {
                        if (grabber != null)
                        {
                            //Mark Buffer Avaible for new Image
                            buffer.push(grabber);
                        }
                    }
                }
            }
            catch (Euresys.gentl_error exc)
            {
                //if (exc.gc_err != Euresys.gc.GC_ERROR.GC_ERR_ABORT)
                //    MessageBox.Show(exc.Message);
            }
        }
    }
}
