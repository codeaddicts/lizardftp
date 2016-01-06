using System;
using Codeaddicts.libArgument.Attributes;

namespace Codeaddicts.Lizard.Cli
{
    public class Options
    {
        [Argument ("-u", "from-server", "to-server", "from-host", "to-host")]
        public string Format;

        [Argument ("grab", "get", "pull", "download")]
        public string Download;

        [Argument ("send", "push", "upload")]
        public string Upload;

        [Argument ("from-file", "to-file")]
        public string Path;

        public bool ShouldDownload {
            get { return !string.IsNullOrEmpty (Download); }
        }

        public bool ShouldUpload {
            get { return !string.IsNullOrEmpty (Upload); }
        }

        public bool PathSpecified {
            get { return !string.IsNullOrEmpty (Path); }
        }
    }
}

