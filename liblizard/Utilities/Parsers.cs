﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net;

namespace Codeaddicts.Lizard
{
    public static class Parsers
    {
        public static void ParseResponse (string line, out int code, out bool dash, out string message) {
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

        public static IPEndPoint ParsePasvResponse (string response) {
            var regExMatch = Regex.Match(response, RegexConstants.PASV_Prepare);
            var endPointParts = regExMatch.Value.Split(',');
            var endPointIpString = string.Format(
                "{0}.{1}.{2}.{3}",
                endPointParts[0], endPointParts[1],
                endPointParts[2], endPointParts[3]
            );
            var endPointIp = IPAddress.Parse(endPointIpString);
            var endPointPort = (int.Parse(endPointParts[4]) * 256) + int.Parse(endPointParts[5]);
            var endPoint = new IPEndPoint(endPointIp, endPointPort);
            return endPoint;
        }
    }
}

