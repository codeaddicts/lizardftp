using System;

namespace Codeaddicts.Lizard
{
    public partial class FtpClient
    {
        const string REGEX_FEAT_NO_FEATURES	= @"211 [a-zA-Z0-9]*";
        const string REGEX_FEAT_BEGIN		= @"211-[a-zA-Z0-9]*";
        const string REGEX_FEAT_END			= @"211 End";
        const string REGEX_PASV_PREPARE		= @"(?<=\()((?:(?:[0-9]{1,3})(?:,)?){6})(?=\))";
    }
}

