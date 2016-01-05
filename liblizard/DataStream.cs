using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Codeaddicts.Lizard
{
    public class DataStream : IDisposable
    {
        public StreamReader Reader;
        public StreamWriter Writer;

        Socket Client;
        NetworkStream Stream;

        public void Connect (IPEndPoint ep) {
            Client = new Socket (
                addressFamily: AddressFamily.InterNetwork,
                socketType: SocketType.Stream,
                protocolType: ProtocolType.Tcp
            );
            Client.Connect (ep);
            Stream = new NetworkStream (Client, false);
            Reader = new StreamReader (Stream);
            Writer = new StreamWriter (Stream);
        }

        public static DataStream Open (IPEndPoint ep) {
            var ds = new DataStream ();
            ds.Connect (ep);
            return ds;
        }

        #region IDisposable implementation

        public void Dispose () {
            Reader.Dispose ();
            Writer.Dispose ();
            Stream.Dispose ();
            Client.Dispose ();
        }

        #endregion
    }
}

