using System;
using Codeaddicts.libArgument;
using Codeaddicts.Lizard;

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
			Client.Wait ();
		}
	}
}
