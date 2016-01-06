using Codeaddicts.Lizard.FtpItems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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

        public void ConnectPassive()
        {
            // Send PASV Command, wait for Answer, parse Answer
            PASV ();
        }

        public void UploadFile (string fileName) {
            // Does the File even exist?
            if (!File.Exists (fileName)) throw new FileNotFoundException (fileName);

            // Initialize File Transfer
            STOR(fileName);

            // Read Bytes, then send them and close Stream
            using (var fileStream = File.OpenRead(fileName))
            {
                fileStream.CopyTo(Data.Stream, 512);
            }
            Data.Close ();

            // Wait for Confirmation on File Transfer
            MessageHandler.WaitOne ();
        }

        public void DownloadFile(string fileName)
        {
            // Intitialize File Transfer
            RETR(fileName);

            // Read Bytes from Stream, save them to disk
            using (var fileStream = File.Create(fileName))
            {
                Data.Stream.CopyTo(fileStream, 512);
            }

            // Wait for Confirmation on File Transfer. Stream will be closed by the Server!
            MessageHandler.WaitOne();
        }

        public FtpFile GetFileInfo(string fileName)
        {
            LIST(fileName);            
            return FtpFile.Parse(new StreamReader(Data.Stream).ReadLine());
        }

        public IEnumerable<FtpItem> GetDirectoryContents()
        {
            // Intitialize File Transfer
            LIST ();

            // Read Bytes from Stream, save them to disk
            var result = new StreamReader(Data.Stream).ReadToEnd();
            foreach (string line in result.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (line.StartsWith("d"))
                    yield return FtpDirectory.Parse(line);
                else
                    yield return FtpFile.Parse(line);
            }

            // Wait for Confirmation on File Transfer. Stream will be closed by the Server!
            MessageHandler.WaitOne();
        }
    }
}

