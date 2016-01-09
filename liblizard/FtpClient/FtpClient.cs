using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

namespace Codeaddicts.Lizard
{
    public partial class FtpClient : IDisposable
    {
        // Enumerations
        [Flags]
        enum ClientState {
            None = 0,
            ExitRequested = 1 << 0,
            GoodResponse = 1 << 1,
            BadResponse = 1 << 2
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
        ManualResetEvent MessageHandler;
        ClientState CurrentState;

        // Connection details
        string Host;
        string User;
        string Path;
        string Password;
        int    Port;
        bool   ServiceReady;

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
