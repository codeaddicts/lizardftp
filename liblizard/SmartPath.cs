using System;
using System.Reflection;

namespace System.IO
{
    public class SmartPath
    {
        public static SmartPath EntryPath {
            get { return Assembly.GetEntryAssembly ().Location; }
        }

        string value;
        public string Value { get { return Normalize (); } }

        public SmartPath (string value) {
            this.value = value;
        }

        string Normalize () {
            value = value
                .Replace ('\\', '/')
                .Replace ('/', Path.DirectorySeparatorChar)
                .TrimEnd (Path.DirectorySeparatorChar);
            return value;
        }

        string Concat (string second) {
            var newval = string.Format (
                format: "{0}{1}{2}",
                arg0: value,
                arg1: Path.DirectorySeparatorChar,
                arg2: second
            );
            return new SmartPath (newval).Normalize ();
        }

        public SmartPath Absolute () {
            return Path.GetFullPath (value);
        }

        public static implicit operator string (SmartPath path) {
            return path.Value;
        }

        public static implicit operator SmartPath (string path) {
            return new SmartPath (path);
        }

        public static SmartPath operator / (SmartPath path1, string path2) {
            return path1.Concat (path2);
        }

        public static SmartPath operator / (SmartPath path1, SmartPath path2) {
            return path1.Concat (path2);
        }

        public static SmartPath operator / (string path1, SmartPath path2) {
            return new SmartPath (path1).Concat (path2);
        }

        public override string ToString () {
            return Value;
        }
    }
}
