using System;

namespace Codeaddicts.Lizard
{
    public partial class FtpClient
    {
        public void CDUP () {
            Send ("CDUP");
            MessageHandler.WaitOne ();
        }

        public void CWD (string directory) {
            Send ("CWD {0}", directory);
            MessageHandler.WaitOne ();
        }

        public void MKD (string name) {
            Send ("MKD {0}", name);
            MessageHandler.WaitOne ();
        }

        public void MODE (char mode) {
            Send ("MODE {0}", mode);
            MessageHandler.WaitOne ();
        }

        public void NLST (string directory = null) {
            if (directory == null)
                Send ("NLST");
            else
                Send ("NLST {0}", directory);
            MessageHandler.WaitOne ();
        }

        public void USER (string username) {
            Send ("USER {0}", username);
            MessageHandler.WaitOne ();
        }

        public void PASS (string password) {
            Send ("PASS {0}", password);
            MessageHandler.WaitOne ();
        }

        public void PWD () {
            Send ("PWD");
            MessageHandler.WaitOne ();
        }

        public void STOR (string filename) {
            Send ("STOR {0}", filename);
            MessageHandler.WaitOne ();
        }

        public void RETR (string fileName) {
            Send ("RETR {0}", fileName);
            MessageHandler.WaitOne ();
        }

        public void LIST (string fileName = "") {
            Send (fileName == string.Empty ? "LIST" : "LIST {0}", fileName);
            MessageHandler.WaitOne ();
        }

        public void SYST () {
            Send ("SYST");
            MessageHandler.WaitOne ();
        }

        public void FEAT () {
            Send ("FEAT");
            MessageHandler.WaitOne ();
        }

        public void TYPE (char type) {
            Send ("TYPE {0}", type);
            MessageHandler.WaitOne ();
        }

        public void STRU (char type) {
            Send ("STRU {0}", type);
            MessageHandler.WaitOne ();
        }

        public void PASV () {
            Send ("PASV");
            MessageHandler.WaitOne ();
        }

        public void NOOP () {
            Send ("NOOP");
            MessageHandler.WaitOne ();
        }

        public void QUIT () {
            Send ("QUIT");
            MessageHandler.WaitOne ();
        }
    }
}

