using System;

namespace Codeaddicts.Lizard
{
    public partial class FtpClient
    {
        public void CDUP () {
            Send ("CDUP");
        }

        public void CWD (string directory) {
            Send ("CWD {0}", directory);
        }

        public void MKD (string name) {
            Send ("MKD {0}", name);
        }

        public void MODE (char mode) {
            Send ("MODE {0}", mode);
        }

        public void NLST (string directory = null) {
            if (directory == null)
                Send ("NLST");
            else
                Send ("NLST {0}", directory);
        }

        public void USER (string username) {
            Send ("USER {0}", username);
        }

        public void PASS (string password) {
            Send ("PASS {0}", password);
        }

        public void PWD () {
            Send ("PWD");
        }

        public void STOR (string filename) {
            Send ("STOR {0}", filename);
        }

        public void RETR (string fileName) {
            Send ("RETR {0}", fileName);
        }

        public void LIST (string fileName = "") {
            Send (fileName == string.Empty ? "LIST" : "LIST {0}", fileName);
        }

        public void SYST () {
            Send ("SYST");
        }

        public void FEAT () {
            Send ("FEAT");
        }

        public void TYPE (char type) {
            Send ("TYPE {0}", type);
        }

        public void STRU (char type) {
            Send ("STRU {0}", type);
        }

        public void PASV () {
            Send ("PASV");
        }

        public void NOOP () {
            Send ("NOOP");
        }

        public void QUIT () {
            Send ("QUIT");
        }
    }
}

