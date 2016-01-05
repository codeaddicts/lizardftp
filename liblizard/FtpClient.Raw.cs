using System;

namespace Codeaddicts.Lizard
{
    public partial class FtpClient
    {
        public void CDUP () {
            while (!Ready) {}
            Send ("CDUP");
        }

        public void CWD (string directory) {
            while (!Ready) {}
            Send ("CWD {0}", directory);
        }

        public void MKD (string name) {
            while (!Ready) {}
            Send ("MKD {0}", name);
        }

        public void MODE (char mode) {
            while (!Ready) {}
            Send ("MODE {0}", mode);
        }

        public void NLST (string directory = null) {
            while (!Ready) {}
            if (directory == null)
                Send ("NLST");
            else
                Send ("NLST {0}", directory);
        }

        public void USER (string username) {
            while (!Ready) {}
            Send ("USER {0}", username);
        }

        public void PASS (string password) {
            while (!Ready) {}
            Send ("PASS {0}", password);
        }

        public void PWD () {
            while (!Ready) {}
            Send ("PWD");
        }

        public void STOR (string filename) {
            while (!Ready) {}
            Send ("STOR {0}", filename);
        }

        public void SYST () {
            while (!Ready) {}
            Send ("SYST");
        }

        public void FEAT () {
            while (!Ready) {}
            Send ("FEAT");
        }

        public void TYPE (char type) {
            while (!Ready) {}
            Send ("TYPE {0}", type);
        }

        public void STRU (char type) {
            while (!Ready) {}
            Send ("STRU {0}", type);
        }

        public void PASV () {
            while (!Ready) {}
            Send ("PASV");
        }

        public void NOOP () {
            while (!Ready) {}
            Send ("NOOP");
        }
    }
}

