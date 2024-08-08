using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToptecMCP
{

    public delegate void ClassReceiveEventHandler(object sender, ClassReceiveEventArgs e);
    public class ClassReceiveEventArgs : EventArgs
    {
        private readonly int id = 0;
        private readonly int receiveBytes;
        private readonly byte[] receiveData;

        public ClassReceiveEventArgs(int _id, int _receiveBytes, byte[] _receiveData)
        {
            id = _id;
            receiveBytes = _receiveBytes;
            receiveData = _receiveData;
        }

        public int ReceiveBytes
        {
            get
            {
                return receiveBytes;
            }
        }

        public byte[] ReceiveData
        {
            get
            {
                return receiveData;
            }
        }

        public int ID
        {
            get
            {
                return id;
            }
        }
    }
}
