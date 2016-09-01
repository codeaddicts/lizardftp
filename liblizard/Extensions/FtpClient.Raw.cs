using System;

namespace Codeaddicts.Lizard.Raw
{
    public static class RawCommands
    {
        public static void ABOR (this FtpClient client)
            => client.Send ("ABOR");

        public static void CDUP (this FtpClient client)
            => client.Send ("CDUP");

        public static void CWD (this FtpClient client, string directory)
            => client.Send ("CWD {0}", directory);

        public static void MKD (FtpClient client, string name)
            => client.Send ("MKD {0}", name);

        public static void MODE (this FtpClient client, char mode)
            => client.Send ("MODE {0}", mode);

        public static void USER (this FtpClient client, string username)
            => client.Send ("USER {0}", username);

        public static void PASS (this FtpClient client, string password)
            => client.Send ("PASS {0}", password);

        public static void PWD (this FtpClient client)
            => client.Send ("PWD");

        public static void STOR (this FtpClient client, string filename)
            => client.Send ("STOR {0}", filename);

        public static void RETR (this FtpClient client, string fileName)
            => client.Send ("RETR {0}", fileName);

        public static void SYST (this FtpClient client)
            => client.Send ("SYST");

        public static void FEAT (this FtpClient client)
            => client.Send ("FEAT");

        public static void TYPE (this FtpClient client, char type)
            => client.Send ("TYPE {0}", type);

        public static void STRU (this FtpClient client, char type)
            => client.Send ("STRU {0}", type);

        public static void PASV (this FtpClient client)
            => client.Send ("PASV");

        public static void NOOP (this FtpClient client)
            => client.Send ("NOOP");

        public static void QUIT (this FtpClient client)
            => client.Send ("QUIT");

        public static void NLST (this FtpClient client, string directory = null)
            => client.Send (string.Format ("NLST {0}", directory ?? string.Empty).TrimEnd ());

        public static void LIST (this FtpClient client, string fileName = "")
            => client.Send (string.Format ("LIST {0}", fileName ?? string.Empty).TrimEnd ());

        public static void AUTH (this FtpClient client, string method = "TLS")
            => client.Send ($"AUTH {method}");
    }
}