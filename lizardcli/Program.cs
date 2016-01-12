using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Codeaddicts.libArgument;
using Codeaddicts.Lizard;
using Codeaddicts.Lizard.Raw;
using System.Net;
using System.Collections;

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

            /*
             * TODO:
             * - Add a MessageQueue or something similar to allow the final user to grab the results sent by the server.
             * - Finalize Message Processing: How will the user be informed of a failed login without the need to check the Messages? 
             *   Possibly implement TAP for this, so the use of await ftpClient.Messages.GrabLatest() will become possible - otherwise, better solutions?
             * - Add better Debugging. LogMessage will NOT do.
             * - Maybe create a custom class for the CommandStream aswell?
             * - Why isn't the Processor a custom class which can be initialized by handing it the Command Stream?
             * - Fix Multiline Responses somehow: 211 is not the only code which needs to be checked. Possible check before switch(code).
             * - Ftp-Welcome-Message can be Multiline. Will fuck the system.
             * - Rework Multiline-Handling altogether, remove the need to be able to skip the WaitOne on the Processing.
             * - Does the Processor really need the Raw Response? Possibly remove that.
             * - Create advanced server side operation wrappers (@NikxDa)
             * - Add working Events
             * - Add TAP Down/Uploads
             * - Somehow prevent multiple async transmissions, because async actually isnt really able to be used async-ly in this case. It sucks, I know.
             * - PORT-Command ffs
             * - Check the list of commands remaining, try ABOR command again, otherwise implement DataStream Abortion (ha!).
             * - Watch kittens play. Everyone likes to watch kittens play.
             */

            // Subscribe to events
            Client.ConnectionClosed += QuitAndExit;

            // Establish connection
            Client.Connect ();

            // Login
            if (!Client.Login ()) {
                Console.WriteLine ("Login failed (returned false)");
                QuitAndExit (null, null);
            } else
                Console.WriteLine ("Login successful (returned true)");

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
                    Client.DataConnectionClosed -= handler;
                    downloadFinished = true;
                };
                Client.DataConnectionClosed += handler;
                Download ();
                while (!downloadFinished) { }
            }

            // Upload if -ul or --upload was specified
            if (Options.ShouldUpload) {
                // TODO: Write the actual code
            }

            // Run tests
            // TODO: Write actual tests using NUnit
            // Console.ReadLine ();

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
