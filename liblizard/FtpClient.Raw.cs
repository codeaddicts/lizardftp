using System;

namespace Codeaddicts.Lizard
{
    public partial class FtpClient
    {
        public void USER (string username) {
            Send ("USER {0}\r\n", username);
        }

        public void PASS (string password) {
            Send ("PASS {0}\r\n", password);
        }

        public void PASV () {
            Send ("PASV\r\n");
        }
    }
}

