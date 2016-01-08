using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Codeaddicts.libArgument;
using Codeaddicts.Lizard;
using Codeaddicts.Lizard.FtpItems;
using System.Threading;

namespace Codeaddicts.Lizard.Cli
{
    class MainClass
    {
        readonly Options Options;
        readonly FtpClient Client;

        public static void Main (string[] args) {
            new MainClass (args).Main ();
        }

        public MainClass (string[] args) {
            Options = ArgumentParser.Parse<Options> (args);
            Client = new FtpClient (Options.Format);
        }

        public void Main () {

            // Subscribe to events
            Client.EConnectionClosed += QuitAndExit;

            // Establish connection
            Client.Connect ();

            // Login
            Client.Login ();

            // Retrieve OS information
            Client.SYST ();

            // Get current directory
            Client.PWD ();

            // Retrieve feature information
            Client.FEAT ();

            // Enter passive transfer mode
            Client.ConnectPassive ();

            // Download if -dl or --download was specified
            if (Options.ShouldDownload) {
                bool downloadFinished = false;
                EventHandler handler = null;
                handler = (sender, e) => {
                    Client.EDataConnectionClosed -= handler;
                    downloadFinished = true;
                };
                Client.EDataConnectionClosed += handler;
                Download ();
                while (!downloadFinished) { }
            }

            // Upload if -ul or --upload was specified
            if (Options.ShouldUpload) {
                // TODO: Write the actual code
            }

            // Run tests
            // TODO: Write actual tests using NUnit
            TestDownloadAbortion ();

            // Quit and exit
            QuitAndExit (null, null);
        }

        void TestDownloadAbortion () {
            // TODO: Add async downloading ability
        }

        // Initiate a file download
        void Download () {
            
            // Create variables to hold path data
            var dir = string.Empty;
            var dirs = new [] { dir };
            var file = new SmartPath (Options.Download);

            // Get remote target path
            if (file.Value.Contains (Path.DirectorySeparatorChar.ToString ())) {
                var arr = file.Value.Split (Path.DirectorySeparatorChar);
                dirs = arr.Reverse ().Skip (1).Reverse ().ToArray ();
                dir = string.Join (
                    separator: Path.DirectorySeparatorChar.ToString (),
                    values: (IEnumerable<string>) dirs
                );
                file = new SmartPath (arr.Last ());
            }

            // Get local target path
            var localdir = Options.PathSpecified
                ? Options.Path
                : dir == "/"
                ? file.Value
                : new SmartPath (dir) + file.Value;

            // Change to the target file directory
            if (!string.IsNullOrEmpty (dir)) {
                foreach (var dirpart in dirs)
                    Client.CWD (dirpart);
            }

            // Download the file
            Client.DownloadFile (file, localdir);
        }

        void QuitAndExit (object sender, EventArgs e) {
            Client.QUIT ();
            Environment.Exit (0);
        }
    }
}
