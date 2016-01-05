using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Codeaddicts.Lizard
{
    public class DataStream : IDisposable
    {
        bool available;
        public bool Available;
        public TcpClient Client;
        IPEndPoint EndPoint;

        public DataStream () {
            available = false;
            Available = false;
            Client = null;
        }

        internal void SetAvailable (IPEndPoint ep) {
            EndPoint = ep;
            available = true;
        }

        public void Open () {
            if (!available) {
                Console.WriteLine ("[000] Can't create socket: No endpoint available");
                return;
            }
            Console.WriteLine ("[000] Creating socket");
            if (Client != null) {
                Client.GetStream ().Dispose ();
                Client = null;
            }
            Client = new TcpClient ();
            Console.WriteLine ("[000] Connecting to {0}", EndPoint);
            try {
                Client.Connect (EndPoint);
            } catch (Exception e) {
                Console.WriteLine (e.Message);
                Client = null;
                available = false;
                Available = false;
                return;
            }
            Console.WriteLine ("[000] Data stream opened");
            Available = true;
        }

        public void Close () {
            available = false;
            Available = false;
            Client.GetStream ().Dispose ();
            Client = null;
        }

        public void Wait () {
            while (!available)
                Thread.Sleep (1);
        }

        public void Send (byte[] data) {
            Wait ();
            Client.NoDelay = true;
            NetworkStream ns = Client.GetStream ();
            ns.Write (data, 0, data.Length);
            Client.NoDelay = false;
        }

        #region IDisposable implementation

        public void Dispose () {
            Available = false;
            Client.GetStream ().Dispose ();
        }

        #endregion
    }
}

