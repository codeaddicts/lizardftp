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

        public void ConnectPassive () {
            // Send PASV Command, wait for Answer, parse Answer
            PASV ();
        }

        public void UploadFile (string fileName) {
            
            // Check if the file exists
            if (!File.Exists (fileName))
                throw new FileNotFoundException (fileName);

            // Initialize file transfer
            STOR (fileName);

            using (var file = File.OpenRead (fileName))
            using (var reader = new BinaryReader (file)) {
                while (reader.BaseStream.Position < reader.BaseStream.Length) {
                    var buf = reader.ReadBytes (512);
                    Data.Send (buf);
                }
            }
            Data.Close ();

            // Wait for Confirmation on File Transfer
            MessageHandler.WaitOne ();
        }

        public void DownloadFile (string remotefilename, string localfilename = null) {

            if (string.IsNullOrEmpty (localfilename))
                localfilename = remotefilename;
            
            // Intitialize file transfer
            RETR (remotefilename);

            using (var file = File.Create (localfilename))
            using (var writer = new BinaryWriter (file)) {
                var buf = new byte[512];
                while (Data.Stream.Read (buf, 0, 512) > 0) {
                    writer.Write (buf);
                }
                writer.Flush ();
            }

            // Wait for Confirmation on File Transfer. Stream will be closed by the Server!
            MessageHandler.WaitOne ();
        }

        public FtpFile GetFileInfo (string fileName) {
            LIST (fileName);            
            return FtpFile.Parse (new StreamReader (Data.Stream).ReadLine ());
        }

        public IEnumerable<FtpItem> GetDirectoryContents () {
            // Intitialize File Transfer
            LIST ();

            // Read Bytes from Stream, save them to disk
            var result = new StreamReader (Data.Stream).ReadToEnd ();
            foreach (string line in result.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)) {
                if (line.StartsWith ("d"))
                    yield return FtpDirectory.Parse (line);
                else
                    yield return FtpFile.Parse (line);
            }

            // Wait for Confirmation on File Transfer. Stream will be closed by the Server!
            MessageHandler.WaitOne ();
        }
    }
}

