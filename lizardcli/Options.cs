using System;
using Codeaddicts.libArgument.Attributes;

namespace Codeaddicts.Lizard.Cli
{
    public class Options
    {
        [Argument ("-u")]
        public string Format;

        [Argument ("-dl", "--download")]
        public string Download;

        [Argument ("-ul", "--upload")]
        public string Upload;

        [Argument ("-o")]
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

