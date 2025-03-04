using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ToptecMCP
{
    class ClassState
    {

        private const int BUFFER_SIZE = 32768;
        private Socket worker;
        private byte[] buffer;

        public ClassState(Socket _worker)
        {
            worker = _worker;
            buffer = new byte[32768];
        }

        public Socket Worker
        {
            get
            {
                return worker;
            }
            set
            {
                worker = value;
            }
        }

        public byte[] Buffer
        {
            get
            {
                return buffer;
            }
            set
            {
                buffer = value;
            }
        }

        public int BufferSize
        {
            get
            {
                return 32768;
            }
        }
    }
}
