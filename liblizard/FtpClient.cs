using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Codeaddicts.Lizard
{
    public partial class FtpClient : IDisposable
    {
        // Enumerations
        [Flags]
        enum ClientState {
            None = 1 << 0,
            ExitRequested = 1 << 1,
            GoodResponse = 1 << 2,
            BadResponse = 1 << 3
        };

        // Constants
        public const string	DEFAULT_USER     	= "Anonymous";
        public const string	DEFAULT_PASSWORD	= "";
        public const string	DEFAULT_HOST     	= "127.0.0.1";
        public const string	DEFAULT_PATH     	= "/";
        public const int	DEFAULT_PORT     	= 21;

        // Networking stuff
        Socket Client;
        NetworkStream ClientStream;
        StreamReader ClientReader;
        DataStream Data;
        ClientState State;
        AutoResetEvent MessageHandler;

        // Connection details
        string Host;
        string User;
        string Path;
        string Password;
        int    Port;

        // Constructors
        public FtpClient () {
            InitializeParameters ();
        }

        public FtpClient (string formatUri) {
            InitializeParameters (new UriParser (formatUri));
        }

        // IDisposable implementation

        public void Dispose () {
            ClientReader.Dispose ();
            ClientStream.Dispose ();
            Client.Dispose ();
        }
    }
}
