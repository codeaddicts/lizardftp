using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Codeaddicts.Lizard
{
    public partial class FtpClient
    {
        void StartListening () {
            ClientStream = new NetworkStream (Client, false);
            ClientReader = new StreamReader (ClientStream, Encoding.ASCII, false);
            while (!ExitRequested && Client.Connected)
                AcceptResponse ();
            if (EConnectionClosed != null)
                EConnectionClosed (this, EventArgs.Empty);
            Dispose ();
        }

        void AcceptResponse (string line = null) {
            if (line == null)
                line = ClientReader.ReadLine ();
            int code;
            bool dash;
            string message;
            ParseResponse (line, out code, out dash, out message);
            ProcessMessage (code, dash, message, line);
        }

        void LogMessage (int code, string message) {
            Console.WriteLine ("[{0:000}] {1}", code, message);
        }

        void SendRaw (byte[] data) {
            Client.NoDelay = true;
            Client.Send (data);
            Client.NoDelay = false;
            MessageHandler.WaitOne ();
        }

        void InitializeParameters (string host, int port, string user, string path, string password) {
            Host = host;
            Port = port;
            User = user;
            Path = path;
            Password = password;
            MessageHandler = new AutoResetEvent (false);
            Data = new DataStream ();
            State = ClientState.None;
        }

        void InitializeParameters (UriParser parser) {
            InitializeParameters (
                host: parser.Host,
                port: parser.Port,
                user: parser.User,
                path: parser.Path,
                password: parser.Password
            );
        }

        void InitializeParameters () {
            InitializeParameters (
                host: DEFAULT_HOST,
                port: DEFAULT_PORT,
                user: DEFAULT_USER,
                path: DEFAULT_PATH,
                password: DEFAULT_PASSWORD
            );
        }
    }
}

