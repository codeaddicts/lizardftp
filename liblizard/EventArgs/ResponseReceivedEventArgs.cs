using System;

namespace Codeaddicts.Lizard
{
    public class ResponseReceivedEventArgs : EventArgs
    {
        readonly public FtpResponse Response;

        public ResponseReceivedEventArgs (FtpResponse response) {
            Response = response;
        }
    }
}

