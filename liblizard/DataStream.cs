using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Codeaddicts.Lizard
{
    public class DataStream : IDisposable
    {
        public TcpClient Client;

        NetworkStream stream;
        public NetworkStream Stream {
            get {
                if (endpoint == null)
                    throw new Exception ("Datastream not yet open");
                return stream;
            }
        }

        IPEndPoint endpoint;
        public IPEndPoint EndPoint {
            get {
                return endpoint;
            }
            set {
                if (value == null)
                    return;
                endpoint = value;
                Connect ();
            }
        }

        public bool Connected { get { return stream != null; } }

        void Connect () {
            Client = new TcpClient ();
            try {
                Client.Connect (EndPoint);
                stream = Client.GetStream ();
            } catch (Exception e) {
                Console.WriteLine (e.Message);
                Client = null;
                stream = null;
                endpoint = null;
            }
        }

        public DataStream () {
            Client = null;
            stream = null;
            endpoint = null;
        }

        public void Close () {
            stream.Dispose ();
            Client = null;
            endpoint = null;
            stream = null;
        }

        public void Send (byte[] data) {
            stream.Write (data, 0, data.Length);
        }

        #region IDisposable implementation

        public void Dispose () {
            Close ();
        }

        #endregion
    }
}

