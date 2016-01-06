using System;
using System.Linq;

namespace Codeaddicts.Lizard
{
    public class UriParser
    {
        // Host format: [user[:pass]@]host[:port][/path]

        public string User { get { return GetUser (Format); } }
        public string Path { get { return GetPath (Format); } }
        public string Host { get { return GetHost (Format); } }
        public string Password { get { return GetPassword (Format); } }
        public int Port { get { return GetPort (Format); } }

        string Format;

        public UriParser (string format)
        {
            Format = format;
        }

        static string GetUser (string format) {
            string host = format;
            if (string.IsNullOrEmpty (host))
                return FtpClient.DEFAULT_USER;
            if (host.Contains ("@"))
                host = host.Split ('@').First ();
            else
                return FtpClient.DEFAULT_USER;
            if (host.Contains (":"))
                host = host.Split (':').First ();
            return host;
        }

        static string GetPassword (string format) {
            string host = format;
            if (string.IsNullOrEmpty (host))
                return FtpClient.DEFAULT_PASSWORD;
            if (host.Contains ("@"))
                host = host.Split ('@').First ();
            else
                return FtpClient.DEFAULT_PASSWORD;
            if (host.Contains (":"))
                host = host.Split (':').Skip (1).First ();
            else
                return FtpClient.DEFAULT_PASSWORD;
            return host;
        }

        static string GetPath (string format) {
            string host = format;
            if (string.IsNullOrEmpty (host))
                return FtpClient.DEFAULT_PATH;
            if (host.Contains ("/"))
                host = host.Split ('/').Skip (1).First ();
            else
                return FtpClient.DEFAULT_PATH;
            if (string.IsNullOrEmpty (host))
                host = FtpClient.DEFAULT_PATH;
            return host;
        }

        static string GetHost (string format) {
            string host = format;
            if (string.IsNullOrEmpty (host))
                return FtpClient.DEFAULT_HOST;
            if (host.Contains ("@"))
                host = host.Split ('@').Skip (1).First ();
            if (host.Contains (":"))
                host = host.Split (':').First ();
            if (host.Contains ("/"))
                host = host.Split ('/').First ();
            return host;
        }

        static int GetPort (string format) {
            string host = format;
            if (string.IsNullOrEmpty (host))
                return FtpClient.DEFAULT_PORT;
            if (host.Contains ("@"))
                host = host.Split ('@').Skip (1).First ();
            if (host.Contains (":"))
                host = host.Split (':').Skip (1).First ();
            if (host.Contains ("/"))
                host = host.Split ('/').First ();
            int port;
            if (!int.TryParse (host, out port))
                port = FtpClient.DEFAULT_PORT;
            return port;
        }
    }
}

