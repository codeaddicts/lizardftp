using System;

namespace Codeaddicts.Lizard
{
    public partial class FtpClient
    {        
        public void CDUP () {
            Send ("CDUP");
            MessageHandled.WaitOne();
        }

        public void CWD (string directory) {
            Send("CWD {0}", directory);
            MessageHandled.WaitOne();
        }

        public void MKD (string name) {
            Send("MKD {0}", name);
            MessageHandled.WaitOne();
        }

        public void MODE (char mode) {
            Send("MODE {0}", mode);
            MessageHandled.WaitOne();
        }

        public void NLST (string directory = null) {
            if (directory == null)
                Send ("NLST");
            else
                Send("NLST {0}", directory);
            MessageHandled.WaitOne();
        }

        public void USER (string username) {
            Send("USER {0}", username);
            MessageHandled.WaitOne();
        }

        public void PASS (string password) {
            Send("PASS {0}", password);
            MessageHandled.WaitOne();
        }

        public void PWD () {
            Send("PWD");
            MessageHandled.WaitOne();
        }

        public void STOR (string filename) {
            Send("STOR {0}", filename);
            MessageHandled.WaitOne();
        }

        public void RETR(string fileName)
        {
            Send("RETR {0}", fileName);
            MessageHandled.WaitOne();
        }

        public void LIST(string fileName = "")
        {
            Send((fileName == "") ? "LIST" : "LIST {0}", fileName);
            MessageHandled.WaitOne();
        }

        public void SYST () {
            Send("SYST");
            MessageHandled.WaitOne();
        }

        public void FEAT () {
            Send("FEAT");
            MessageHandled.WaitOne();
        }

        public void TYPE (char type) {
            Send("TYPE {0}", type);
            MessageHandled.WaitOne();
        }

        public void STRU (char type) {
            Send("STRU {0}", type);
            MessageHandled.WaitOne();
        }

        public void PASV () {
            Send("PASV");
            MessageHandled.WaitOne();
        }

        public void NOOP () {
            Send("NOOP");
            MessageHandled.WaitOne();
        }
    }
}

