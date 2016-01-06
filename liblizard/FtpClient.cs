using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Codeaddicts.Lizard
{
    public partial class FtpClient : IDisposable
    {
        public const string	DEFAULT_USER     	= "Anonymous";
        public const string	DEFAULT_PASSWORD	= "";
        public const string	DEFAULT_HOST     	= "127.0.0.1";
        public const string	DEFAULT_PATH     	= "/";
        public const int	DEFAULT_PORT     	= 21;

        Socket Client;
        NetworkStream ClientStream;
        StreamReader ClientReader;
        DataStream Data;
        string Host;
        string User;
        string Path;
        string Password;
        int    Port;
        volatile bool Quit;

        AutoResetEvent MessageHandler;

        public FtpClient () {
            InitializeParameters ();
        }

        public FtpClient (string formatUri) {
            InitializeParameters (
                parser: new UriParser (formatUri)
            );
        }

        public void Connect () {
            Client = new Socket (
                addressFamily: AddressFamily.InterNetwork,
                socketType: SocketType.Stream,
                protocolType: ProtocolType.Tcp
            );
            Client.Connect (Host, Port);
            Task.Factory.StartNew (StartListening);
        }

        public void Connect (
            string host,
            int port = DEFAULT_PORT,
            string user = DEFAULT_USER,
            string password = DEFAULT_PASSWORD,
            string path = DEFAULT_PATH) {
            InitializeParameters (
                host: host,
                port: port,
                user: user,
                path: path,
                password: password
            );
            Connect ();
        }

        public void Connect (string user, string password = DEFAULT_PASSWORD) {
            Connect (
                host: Host,
                port: Port,
                user: user,
                password: password
            );
        }

        public void Wait () {
            while (!Quit && Client.Connected)
                Thread.Sleep (1);
        }

        void StartListening () {
            ClientStream = new NetworkStream (Client, false);
            ClientReader = new StreamReader (ClientStream, Encoding.ASCII, false);
            while (!Quit && Client.Connected) {
                AcceptResponse ();
            }
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

        void Send (string format, params object[] args) {
            var text = string.Format (
                format: "{0}\r\n",
                arg0: string.Format (format, args)
            );
            var data = Encoding.ASCII.GetBytes (text);
            SendRaw (data);
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
            /*
            Console.WriteLine (
                "[000] {0}:{1}@{2}:{3}",
                User, Password, Host, Port
            );
            */
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

        #region IDisposable implementation

        public void Dispose () {
            ClientReader.Dispose ();
            ClientStream.Dispose ();
            Client.Dispose ();
        }

        #endregion
    }
}
