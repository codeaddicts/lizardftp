using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Codeaddicts.Lizard;

namespace Codeaddicts.Lizard.FtpItems
{
    public class FtpFile : FtpItem
    {
        public FtpFile (FtpPermission permission, int size, int owner, int group, DateTime created, string name)
        {
            this.Permission = permission;
            this.Size = size;
            this.Owner = owner;
            this.Group = group;
            this.Created = created;
            this.Name = name;
        }

        public int Size { get; private set; }

        public static FtpFile Parse (string listReturn) {
            var listRegex = new Regex (RegexConstants.LIST_Parse);
            var match = listRegex.Match (listReturn);

            if (match == null)
                throw new ArgumentException ("Invalid Format: " + listReturn);

            if (match.Groups ["perm"].Value.StartsWith ("d"))
                throw new ArgumentException ("Given string represents a directory. Use FtpDirectory.Parse instead.");

            return new FtpFile (
                permission: FtpPermission.Parse (match.Groups ["perm"].Value),
                size: int.Parse (match.Groups ["size"].Value),
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
