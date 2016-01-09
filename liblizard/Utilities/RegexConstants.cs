using System;
using System.Linq;

namespace Codeaddicts.Lizard
{
	public static class RegexConstants
	{
		const string REGEX_FEAT_NO_FEATURES	= @"211 [a-zA-Z0-9]*";
		const string REGEX_FEAT_BEGIN = @"211-[a-zA-Z0-9]*";
		const string REGEX_FEAT_END = @"211 End";
		const string REGEX_PASV_PREPARE = @"(?<=\()((?:(?:[0-9]{1,3})(?:,)?){6})(?=\))";
		const string REGEX_LIST_PARSE = @"
		^(?<perm>[drwx\-]{10})
		\s+(?<links>[0-9]+)
		\s+(?<owner>[0-9]+)
		\s+(?<group>[0-9]+)
		\s+(?<size>[0-9]+)
		\s+(?<date>[A-Za-z]{3}
		\s[0-9]{2}
		\s+(?:[0-9]{2}:[0-9]{2}|[0-9]{4}))
		\s+(?<name>.+)$";

		public static string FEAT_NoFeatures {
			get { return Sanitize (REGEX_FEAT_NO_FEATURES); }
		}

		public static string FEAT_Begin {
			get { return Sanitize (REGEX_FEAT_BEGIN); }
		}

		public static string FEAT_End {
			get { return Sanitize (REGEX_FEAT_BEGIN); }
		}

		public static string PASV_Prepare {
			get { return Sanitize (REGEX_PASV_PREPARE); }
		}

		public static string LIST_Parse {
			get { return Sanitize (REGEX_LIST_PARSE); }
		}

		static string Sanitize (string regexp) {
			var parts = regexp
				.Replace ("\r", string.Empty)
				.Split ('\n');
			for (var i = 0; i < parts.Length; i++)
				parts [i] = parts [i].TrimStart (' ', '\t');
			return string.Join (string.Empty, parts);
		}
	}
}

