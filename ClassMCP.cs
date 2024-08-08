using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace ToptecMCP
{
    public class ClassMCP
    {
        Socket m_ClientSocket;

        private event ClassErrorEventHandler OnError;
        private event ClassReceiveEventHandler OnReceive;

        private byte[] ReceivedData = new byte[0];

        protected int id;
        private bool bReceived=false;

        public bool IsConnect = false;

        int PC_StationNo = 0;
        int PLC_StationNo = 0;
        int NetworkNo = 0;
        int Port = 0;
        string IP = "";

        public void PLC_Setting(string _IP,int _Port,int _NetworkNo, int _PLC_StationNo,int _PC_StationNo)
        {
            this.IP = _IP;
            this.Port = _Port;
            this.NetworkNo = _NetworkNo;
            this.PLC_StationNo = _PLC_StationNo;
            this.PC_StationNo = _PC_StationNo;
            this.Send_BlockWrite[2] = (byte)this.NetworkNo;
            this.Send_BlockWrite[3] = (byte)this.PLC_StationNo;
            this.Send_BlockWrite[6] = (byte)this.PC_StationNo;
            OnError += new ClassErrorEventHandler(Error);
            OnReceive += new ClassReceiveEventHandler(client_OnReceive);
            Receive();
            m_ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        }

        public string Connect()
        {
            if (m_ClientSocket.Connected)
                return "IsConnect";
            IPEndPoint iPEndPoint = new IPEndPoint(Dns.GetHostAddresses(IP)[0], Port);
            try
            {
                m_ClientSocket.BeginConnect(iPEndPoint, new AsyncCallback(this.OnConnectCallback), (object)m_ClientSocket);
            }
            catch (Exception _e)
            {
                return _e.ToString();
            }
            return "0";
        }

        public void DisConnect()
        {
            try
            {
                Socket m_ClientSocket = this.m_ClientSocket;
                m_ClientSocket.Shutdown(SocketShutdown.Both);
                m_ClientSocket.BeginDisconnect(false, new AsyncCallback(this.OnCloseCallBack), (object)m_ClientSocket);
                IsConnect = false;
            }
            catch (Exception ex)
            {
                this.Error(new ClassErrorEventArgs(this.id, ex));
            }
            m_ClientSocket = null;
            m_ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        }

        public int WriteDeviceBlock(int _Address, int[] _data)
        {
            try
            {
                this.Send_BlockWrite[2] = (byte)NetworkNo;
                this.Send_BlockWrite[3] = (byte)PLC_StationNo;
                this.Send_BlockWrite[15] = (byte)(_Address % 256);
                this.Send_BlockWrite[16] = (byte)(_Address / 256);
                this.Send_BlockWrite[17] = (byte)(_Address / 65536);

                int Data_Length = 12 + (_data.Length) * 2;
                this.Send_BlockWrite[7] = (byte)(Data_Length % 256);
                this.Send_BlockWrite[8] = (byte)(Data_Length / 256);

                this.Send_BlockWrite[19] = (byte)(_data.Length % 256);
                this.Send_BlockWrite[20] = (byte)(_data.Length / 256);

                Array.Resize<byte>(ref this.Send_BlockWrite, 21 + _data.Length * 2);
                for (int index3 = 0; index3 < _data.Length; ++index3)
                {
                    this.Send_BlockWrite[21 + (index3 * 2)] = (byte)(_data[index3] % 256);
                    this.Send_BlockWrite[22 + (index3 * 2)] = (byte)(_data[index3] >> 8);
                }

                if (this.m_ClientSocket == null || this.Connection == null)
                    return 1;
                this.m_ClientSocket.Send(this.Send_BlockWrite);
                bReceived = false;
                int tickCount = Environment.TickCount;
                while (!bReceived)
                {
                    if (Environment.TickCount - tickCount > 1000)
                    {
                        IsConnect = false;
                        return 1;
                    }
                }
            }
            catch { return 2;}
            
            return 0;
        }

        public int[] ReadDeviceBlock(int _Address, int _Length)
        {
            int[] array = new int[0];
            try
            {
                this.SendBytes_BlockRead[2] = (byte)NetworkNo;
                this.SendBytes_BlockRead[3] = (byte)PLC_StationNo;
                this.SendBytes_BlockRead[7] = (byte)12;
                this.SendBytes_BlockRead[8] = (byte)0;
                this.SendBytes_BlockRead[11] = (byte)1;
                this.SendBytes_BlockRead[12] = (byte)4;
                this.SendBytes_BlockRead[18] = (byte)168;

                this.SendBytes_BlockRead[15] = (byte)(_Address % 256);
                this.SendBytes_BlockRead[16] = (byte)(_Address / 256);
                this.SendBytes_BlockRead[17] = (byte)(_Address / 65536);

                this.SendBytes_BlockRead[19] = (byte)(_Length % 256);
                this.SendBytes_BlockRead[20] = (byte)(_Length / 256);

                if (this.m_ClientSocket == null || this.Connection == null)
                    return array;
                this.m_ClientSocket.Send(this.SendBytes_BlockRead);

                bReceived = false;
                int tickCount = Environment.TickCount;
                while (!bReceived)
                {
                    if (Environment.TickCount - tickCount > 1000)
                    {
                        IsConnect = false;
                        return array;
                    }
                }

                int newSize2 = array.Length + this.ReceivedData.Length / 2;
                Array.Resize<int>(ref array, newSize2);
                int num5 = this.ReceivedData.Length / 2;
                for (int index3 = 0; index3 < num5; ++index3)
                {
                    array[index3] = (int)this.ReceivedData[index3 * 2 + 1] * 256 + (int)this.ReceivedData[index3 * 2];
                }
            }
            catch { }
            return array;

        }
        #region Event
        private void client_OnReceive(object sender, ClassReceiveEventArgs e)
        {
            try
            {
                this.ReceivedData = new byte[e.ReceiveBytes - 11];
                Array.Copy((Array)e.ReceiveData, 11, (Array)this.ReceivedData, 0, e.ReceiveBytes - 11);
            }
            catch
            {
            }
            this.bReceived = true;
        }

        private void Receive()
        {
            try
            {
                ClassState state = new ClassState(this.m_ClientSocket);
                state.Worker.BeginReceive(state.Buffer, 0, state.BufferSize, SocketFlags.None, new AsyncCallback(this.OnReceiveCallBack), (object)state);
            }
            catch (Exception ex)
            {
                this.Error(new ClassErrorEventArgs(this.id, ex));
            }
        }

        protected virtual void Received(ClassReceiveEventArgs e)
        {
            ClassReceiveEventHandler onReceive = this.OnReceive;
            if (onReceive == null)
                return;
            onReceive((object)this, e);
        }

        private void Error(object sender, ClassErrorEventArgs e)
        {
            IsConnect = false;
        }

        protected virtual void Error(ClassErrorEventArgs e)
        {
            ClassErrorEventHandler onError = this.OnError;
            if (onError == null)
                return;
            onError((object)this, e);
        }

        private Socket Connection
        {
            get
            {
                return this.m_ClientSocket;
            }
            set
            {
                this.m_ClientSocket = value;
            }

        }
        #endregion

        #region CallBack
        private void OnCloseCallBack(IAsyncResult AsyncResult)
        {

        }

        private void OnConnectCallback(IAsyncResult asyncResult)
        {
            try
            {
                Socket asyncState = (Socket)asyncResult.AsyncState;
                asyncState.EndConnect(asyncResult);
                this.m_ClientSocket = asyncState;
                this.Receive();
                IsConnect = true;
            }
            catch (Exception ex)
            {
                this.Error(new ClassErrorEventArgs(this.id, ex));
            }
        }

        private void OnReceiveCallBack(IAsyncResult asyncResult)
        {
            try
            {
                ClassState asyncState = (ClassState)asyncResult.AsyncState;
                int receiveBytes = asyncState.Worker.EndReceive(asyncResult);
                ClassReceiveEventArgs e = new ClassReceiveEventArgs(this.id, receiveBytes, asyncState.Buffer);
                if (receiveBytes > 0)
                    this.Received(e);
                this.Receive();
            }
            catch (Exception ex)
            {
                this.Error(new ClassErrorEventArgs(this.id, ex));
            }
        }
        #endregion

        #region Protocal
        private byte[] Send_BlockWrite = new byte[21]
        {
            (byte) 80, //Subheader                                              0
            (byte) 0,  //Subheader                                              1
            (byte) 1,  //Network No                                             2
            (byte) 1,  //PC station No                                          3
            byte.MaxValue,  //Request destination module I/O No.                4
            (byte) 3,  //Request destination module I/O No.                     5
            (byte) 2,  //Request destination module station No.                 6
            (byte) 32,  //Request data length                                   7
            (byte) 0,  //Request data length                                    8
            (byte) 16,  //Monitorring timer                                     9

            (byte) 0, //Monitorring timer                                       10
            (byte) 1, //Command List     01                                     11
            (byte) 20, //Command List    14                                     12
            (byte) 0,  //Sub Command                                            13
            (byte) 0,  //Sub Command                                            14
            (byte) 0,  //Head Diviec number                                     15
            (byte) 0,  //Head Diviec number                                     16
            (byte) 0,//Head Diviec number                                       17
            (byte) 168,//Divice Code                                            18
            (byte) 7, //Number of device points    960 EA                     19
            (byte) 0    //Number of device points                             20
        };

        private byte[] SendBytes_BlockRead = new byte[21]
        {
            (byte) 80,
            (byte) 0,
            (byte) 1,
            (byte) 1,
            byte.MaxValue,
            (byte) 3,
            (byte) 2,
            (byte) 12,
            (byte) 0,
            (byte) 16,
            (byte) 0,
            (byte) 1,
            (byte) 20,
            (byte) 0,
            (byte) 0,
            (byte) 0,
            (byte) 0,
            (byte) 0,
            (byte) 168,
            (byte) 192,
            (byte) 3
        };
        #endregion
    }
}
