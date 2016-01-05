using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Codeaddicts.Lizard
{
    public partial class FtpClient
    {
        public void ParseResponse (string line, out int code, out bool dash, out string message) {
            code = 0;
            dash = false;
            if (line.Length > 2) {
                var arr = line.Take (3).ToArray ();
                var str = new string (arr);
                code = int.Parse (str);
            }
            dash = line.Length > 3 && line [3] == '-';
            message = line
                .Substring (3 + (dash ? 1 : 0))
                .TrimStart (' ');
        }

        public IPEndPoint ParsePasvResponse (string response) {
            var m = Regex.Match (response, @"(?<=\()((?:(?:[0-9]{1,3})(?:,)?){6})(?=\))");
            var s = m.Value.Split (',');
            var ips = string.Format (
                "{0}.{1}.{2}.{3}",
                s [0], s [1],
                s [2], s [3]
            );
            var ip = IPAddress.Parse (ips);
            var pp1 = int.Parse (s [4]);
            var pp2 = int.Parse (s [5]);
            var port = (pp1 * 255) + pp2;
            var ep = new IPEndPoint (ip, port);
            return ep;
        }
    }
}

