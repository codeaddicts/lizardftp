using System;
using System.IO;
using System.Threading;

namespace Codeaddicts.Lizard
{
    public partial class FtpClient
    {
        public void Login (string user, string password) {
            USER (user);
            if (!string.IsNullOrEmpty (password))
                PASS (password);
        }

        public void Login () {
            Login (User, Password);
        }

        public void UploadFile (string filename) {
            PASV ();
            Console.WriteLine ("Waiting for datastream to become ready");
            Data.Wait ();
            Console.WriteLine ("Data stream is ready");
            Data.Open ();
            TYPE ('I');
            STRU ('F');
            MODE ('S');
            STOR (filename);
            var bytes = File.ReadAllBytes (filename);
            Data.Send (bytes);
            Data.Close ();
        }
    }
}

