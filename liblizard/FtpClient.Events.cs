using System;
using System.Net;

namespace Codeaddicts.Lizard
{
    public partial class FtpClient
    {
        public event EventHandler EConnectionClosed;
        public event EventHandler EDownloadStarted;
        public event EventHandler EUploadStarted;
        public event EventHandler EDataConnectionClosed;
    }
}

