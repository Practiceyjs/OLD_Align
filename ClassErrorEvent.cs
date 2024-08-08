using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToptecMCP
{

    public delegate void ClassErrorEventHandler(object sender, ClassErrorEventArgs e);
    public class ClassErrorEventArgs : EventArgs
    {
        private readonly int id = 0;
        private readonly Exception Exception;

        public ClassErrorEventArgs(int _id, Exception _Exception)
        {
            id = _id;
            Exception = _Exception;
        }

        public Exception MCPException
        {
            get
            {
                return Exception;
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
