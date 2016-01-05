using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Codeaddicts.Lizard
{
    public class DataStream : IDisposable
    {
        public TcpClient Client;
        public IPEndPoint EndPoint { get; set; }
        public NetworkStream Stream { get; private set; }

        public DataStream () {
            Client = null;
            Stream = null;
            EndPoint = null;
        }

        public void Connect(IPEndPoint ep = null)
        {
            if (ep != null) EndPoint = ep;
            if (EndPoint == null) throw new Exception("Invalid Arguments");

            if (Client != null) {
                Stream.Dispose ();
                Client = null;
            }

            Client = new TcpClient ();

            try {
                Client.Connect (EndPoint);
                Stream = Client.GetStream();
            } catch (Exception e) {
                Console.WriteLine (e.Message);
                Client = null;
                return;
            }
        }

        public void Close () {
            Stream.Dispose ();
            Client = null;
            EndPoint = null;
        }

        public void Send (byte[] data) {
            Stream.Write (data, 0, data.Length);
        }

        #region IDisposable implementation

        public void Dispose () {
            Stream.Dispose ();
        }

        #endregion
    }
}

