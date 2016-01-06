using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codeaddicts.Lizard.FtpItems
{
    public class FtpItem
    {
        public FtpPermission Permission { get; protected set; }
        public int Owner { get; protected set; }
        public int Group { get; protected set; }
        public string Name { get; protected set; }
        public DateTime Created { get; protected set; }
    }
}
