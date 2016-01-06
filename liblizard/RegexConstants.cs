using System;

namespace Codeaddicts.Lizard
{
    public static class RegexConstants
    {
        public const string REGEX_FEAT_NO_FEATURES	= @"211 [a-zA-Z0-9]*";
        public const string REGEX_FEAT_BEGIN = @"211-[a-zA-Z0-9]*";
        public const string REGEX_FEAT_END = @"211 End";
        public const string REGEX_PASV_PREPARE = @"(?<=\()((?:(?:[0-9]{1,3})(?:,)?){6})(?=\))";
        public const string REGEX_LIST_PARSE = @"^(?<perm>[drwx\-]{10})\s+(?<links>[0-9]+)\s+(?<owner>[0-9]+)\s+(?<group>[0-9]+)\s+(?<size>[0-9]+)\s+(?<date>[A-Za-z]{3}\s[0-9]{2}\s+(?:[0-9]{2}:[0-9]{2}|[0-9]{4}))\s+(?<name>.+)$";
    }
}

