﻿using System;
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
            ExitRequested = 1 << 1,
            GoodResponse = 1 << 2,
            BadResponse = 1 << 3,
            OperationFailed = 1 << 4,
        };

        // Constants
        public const string	DEFAULT_USER     	= "Anonymous";
        public const string	DEFAULT_PASSWORD	= "";
        public const string	DEFAULT_HOST     	= "127.0.0.1";
        public const string	DEFAULT_PATH     	= "/";
        public const int	DEFAULT_PORT     	= 21;

        // Networking stuff
        Socket Client;
        Stream ClientStream;
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

        public FtpClient (string host, string user, string pass, int port, string path) {
            InitializeParameters (host, port, user, path, pass);
        }

        // IDisposable implementation
        public void Dispose () {
            ClientReader.Dispose ();
            ClientStream.Dispose ();
            Client.Dispose ();
        }
    }
}
