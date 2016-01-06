using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Codeaddicts.Lizard.FtpItems
{
    public class FtpDirectory : FtpItem
    {
        public FtpDirectory (FtpPermission permission, int links, int owner, int group, DateTime created, string name)
        {
            Permission = permission;
            Links = links;
            this.Owner = owner;
            this.Group = group;
            this.Created = created;
            this.Name = name;
        }

        public int Links { get; private set; }

        public static FtpDirectory Parse (string listReturn) {
            var listRegex = new Regex (RegexConstants.REGEX_LIST_PARSE);
            var match = listRegex.Match (listReturn);

            if (match == null)
                throw new ArgumentException ("Invalid Format: " + listReturn);

            if (!match.Groups ["perm"].Value.StartsWith ("d"))
                throw new ArgumentException ("Given string does not represent a directory. Use FtpFile.Parse instead.");

            return new FtpDirectory (
                permission: FtpPermission.Parse (match.Groups ["perm"].Value),
                links: int.Parse (match.Groups ["links"].Value),
                owner: int.Parse (match.Groups ["owner"].Value),
                group: int.Parse (match.Groups ["group"].Value),
                created: (match.Groups ["date"].Value.Contains (":"))
                                ? DateTime.ParseExact (match.Groups ["date"].Value, "MMM dd HH:mm", null)
                                : DateTime.Parse (match.Groups ["date"].Value),
                name: match.Groups ["name"].Value
            );
        }
    }
}
