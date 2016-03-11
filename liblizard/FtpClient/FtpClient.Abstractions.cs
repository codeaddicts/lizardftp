using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Codeaddicts.Lizard.FtpItems;
using Codeaddicts.Lizard.Raw;

namespace Codeaddicts.Lizard
{
    public partial class FtpClient
    {
        public bool ExitRequested {
            get { return CurrentState.HasFlag (ClientState.ExitRequested); }
        }

        public bool GoodResponse {
            get { return CurrentState.HasFlag (ClientState.GoodResponse); }
        }

        public bool BadResponse {
            get { return CurrentState.HasFlag (ClientState.BadResponse); }
        }

        public void Connect () {
            Client = new Socket (
                addressFamily: AddressFamily.InterNetwork,
                socketType: SocketType.Stream,
                protocolType: ProtocolType.Tcp
            );
            Client.Connect (Host, Port);

            Task.Factory.StartNew (StartListening);
            MessageHandler.WaitOne ();
        }

        public void Connect (
            string host,
            int port = DEFAULT_PORT,
            string user = DEFAULT_USER,
            string password = DEFAULT_PASSWORD,
            string path = DEFAULT_PATH) {
            InitializeParameters (
                host: host,
                port: port,
                user: user,
                path: path,
                password: password
            );
            Connect ();
        }

        public void Connect (string user, string password = DEFAULT_PASSWORD) {
            Connect (
                host: Host,
                port: Port,
                user: user,
                password: password
            );
        }

        public void Send (string format, params object[] args) {
            var text = string.Format (
                format: "{0}\r\n",
                arg0: string.Format (format, args)
            );
            var data = Encoding.ASCII.GetBytes (text);
            SendRaw (data);
        }

        public void Wait () {
            while (!ExitRequested && Client.Connected)
                Thread.Sleep (1);
        }

        public bool Login (string user, string password) {
            this.USER (user);
            if (!string.IsNullOrEmpty (password))
                this.PASS (password);
            return GoodResponse;
        }

        public bool Login () {
            return Login (User, Password);
        }

        public void ConnectPassive () {
            // Send PASV Command, wait for Answer, parse Answer
            this.PASV ();
        }

        public void UploadFile (string localFileName, string remoteFileName = null) {
            
            // Check if the file exists
            if (!File.Exists (localFileName))
                throw new FileNotFoundException (localFileName);

            // Check if the remote file name was specified
            if (string.IsNullOrEmpty (remoteFileName))
                remoteFileName = localFileName;

            // Initialize file transfer
            this.STOR (localFileName);

            using (var file = File.OpenRead (localFileName))
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

        public void DownloadFile (string remoteFileName, string localFileName = null) {

            if (string.IsNullOrEmpty (localFileName))
                localFileName = remoteFileName;
            
            // Intitialize file transfer
            this.RETR (remoteFileName);

            using (var file = File.Create (localFileName))
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
            this.LIST (fileName);            
            return FtpFile.Parse (new StreamReader (Data.Stream).ReadLine ());
        }

        public IEnumerable<FtpItem> GetDirectoryContents () {
            // Intitialize File Transfer
            this.LIST ();

            // Read Bytes from Stream, save them to disk
            var result = new StreamReader (Data.Stream).ReadToEnd ();
            foreach (string line in result.Split(new [] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)) {
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

