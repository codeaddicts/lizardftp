using System;
using System.Net;

namespace Codeaddicts.Lizard
{
    public partial class FtpClient
    {
        public event EventHandler ConnectionClosed;
        public event EventHandler DataConnectionClosed;

        public event EventHandler DownloadStarted;
        public event EventHandler UploadStarted;

        public event EventHandler TransmissionFinished;
    }
}

