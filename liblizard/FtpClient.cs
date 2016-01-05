using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        string Host;
        string User;
        string Path;
        string Password;
        int    Port;
        volatile bool Quit;

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

        public void Login (string user, string password) {
            USER (user);
            if (!string.IsNullOrEmpty (password))
                PASS (password);
        }

        public void Login () {
            Login (User, Password);
        }

        public void Wait () {
            while (!Quit && Client.Connected)
                Thread.Sleep (1);
        }

        void StartListening () {
            var ns = new NetworkStream (Client, false);
            var reader = new StreamReader (ns, Encoding.ASCII, false);
            while (!Quit && Client.Connected) {
                AcceptLine (reader);
            }
            reader.Dispose ();
            ns.Dispose ();
            Dispose ();
        }

        void AcceptLine (TextReader reader) {
            var line = reader.ReadLine ();
            int code;
            bool dash;
            string message;
            ParseResponse (line, out code, out dash, out message);
            ProcessMessage (code, dash, message);
        }

        void ProcessMessage (int code, bool dashed, string message) {
            LogMessage (code, message);
            switch (code) {
            case 110:
                // Restart marker reply
                break;
            case 120:
                // Service ready in nnn minutes
                break;
            case 125:
                // Data connection already open; transfer starting
                break;
            case 150:
                // File status okay; about to open data connection
                break;
            case 200:
                // Command okay
                break;
            case 202:
                // Command not implemented, superfluous at this site
                break;
            case 211:
                // System status, or system help reply
                break;
            case 212:
                // Directory status
                break;
            case 213:
                // File status
                break;
            case 214:
                // Help message
                break;
            case 215:
                // NAME system type
                break;
            case 220:
                // Service ready for new user
                break;
            case 221:
                // Service closing control connection
                break;
            case 225:
                // Data connection open; no transfer in progress
                break;
            case 226:
                // Closing data connection
                break;
            case 227:
                // Entering passive mode (h1,h2,h3,h4,p1,p2)
                var ep = ParsePasvResponse (message);
                LogMessage (000, ep.ToString ());
                break;
            case 230:
                // User logged in, proceed
                break;
            case 250:
                // Requested file action okay, completed
                break;
            case 257:
                // "PATHNAME" created
            case 331:
                // User name okay, need password
                Quit |= string.IsNullOrEmpty (Password);
                break;
            case 332:
                // Need account for login
                break;
            case 350:
                // Requested file action pending further information
            case 421:
                // Service not available, closing control connection
                // Also: Login time exceeded
                Quit = true;
                break;
            case 425:
                // Can't open data connection
                break;
            case 426:
                // Connection closed; transfer aborted
                break;
            case 450:
                // Requested file action not taken
                break;
            case 451:
                // Requested action aborted. Local error in processing
                break;
            case 452:
                // Requested action not taken
                // Insufficient storage space in system
                break;
            case 500:
                // Syntax error or
                // Command unrecognized
                break;
            case 501:
                // Syntax error in parameters or arguments
                break;
            case 502:
                // Command not implemented
                break;
            case 503:
                // Bad sequence of commands
                break;
            case 504:
                // Command not implemented for that parameter
                break;
            case 530:
                // Not logged in
                break;
            case 532:
                // Need account for storing files
                break;
            case 550:
                // Requested action not taken
                break;
            case 551:
                // Requested action aborted. Page type unknown
                break;
            case 552:
                // Requested file action aborted
                // Exceeded storage allocation
                break;
            case 553:
                // Requested action not taken
                // File name not allowed
                break;
            }
        }

        void LogMessage (int code, string message) {
            Console.WriteLine ("[{0:000}] {1}", code, message);
        }

        void Send (string format, params object[] args) {
            var text = string.Format (format, args);
            var data = Encoding.ASCII.GetBytes (text);
            SendRaw (data);
        }

        void SendRaw (byte[] data) {
            SocketError error;
            Client.NoDelay = true;
            Client.Send (
                buffer: data,
                offset: 0,
                size: data.Length,
                socketFlags: SocketFlags.None,
                errorCode: out error
            );
            Client.NoDelay = false;
        }

        void InitializeParameters (
            string host,
            int port,
            string user,
            string path,
            string password) {
            Host = host;
            Port = port;
            User = user;
            Path = path;
            Password = password;
            Console.WriteLine (
                "[000] {0}:{1}@{2}:{3}",
                User, Password, Host, Port
            );
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
            Client.Dispose ();
        }

        #endregion
    }
}
