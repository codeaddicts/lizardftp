using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codeaddicts.Lizard
{
    public struct FtpResponse
    {
        public FtpResponse (int code, string message, bool multiline) : this() {
            Message = message;
            Code = code;
            Multiline = multiline;

            // Reconstruct Message, yay
            Raw = String.Format ("{0}{1}{2}",
                Code.ToString (),
                (Multiline) ? "-" : " ",
                Message);
        }

        public FtpResponse (string response) {
            Parsers.ParseResponse (response, out Code, out Multiline, out Message);
            Raw = response;
        }

        public readonly string Message;
        public readonly bool Multiline;
        public readonly int Code;
        public readonly string Raw;
    }
}