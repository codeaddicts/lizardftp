using System;
using Codeaddicts.libArgument;
using Codeaddicts.Lizard;
using Codeaddicts.Lizard.FtpItems;

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
            Client.Connect ();
<<<<<<< HEAD
            Client.Login();

            Client.TYPE('a');

            Client.ConnectPassive();
            Client.DownloadFile("100mphs.jpg");

=======
            Client.Login ();
            Client.ConnectPassive();
            foreach (FtpItem x in Client.GetDirectoryContents())
            {
                Console.WriteLine("{0}:{1}", x.Name, x.Permission.ToString());
            }
>>>>>>> 28a16c7e10bb45a53018c7750384c50948d9537b
            Client.Wait ();
        }
    }
}
