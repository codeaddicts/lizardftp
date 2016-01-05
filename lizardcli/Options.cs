using System;
using Codeaddicts.libArgument.Attributes;

namespace Codeaddicts.Lizard.Cli
{
	public class Options
	{
		[Argument ("-u")]
		public string Format;
	}
}

