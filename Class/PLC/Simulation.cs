using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace T_Align
{
    class Simulation
    {
        Thread Simul_Thread;

        public void Simulation_Run()
        {
            Simul_Thread = new Thread(Simulation_Program);
            Simul_Thread.Start();
        }

        public void Thread_Stop()
        {
            Simul_Thread.Join();
        }

        private void Simulation_Program()
        {
            while(true)
            {
                if (!COMMON.Program_Run)
                    break;
            }
        }
    }
}
