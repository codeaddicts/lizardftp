using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codeaddicts.Lizard.FtpItems
{
    public class FtpPermission
    {
        public FtpPermission (int user, int group, int world)
        {
            User = (FtpPermissionType)user;
            Group = (FtpPermissionType)group;
            World = (FtpPermissionType)world;
        }

        public FtpPermissionType User { get; private set; }

        public FtpPermissionType Group { get; private set; }

        public FtpPermissionType World { get; private set; }

        public int NumericValue {
            get {
                return ((int)User * 10 + (int)Group) * 10 + (int)World;
            }
        }

        public override string ToString () {
            return NumericValue.ToString ();
        }

        public static FtpPermission Parse (string permissionString) {
            if (permissionString.Length > 10 || permissionString.Length < 9)
                throw new ArgumentException ("Invalid Format: " + permissionString);
            permissionString = ((permissionString.Length == 10) ? permissionString.Substring (1) : permissionString).ToLower ();

            var ftpPermissions = new Queue<int> ();
            for (var i = 0; i < permissionString.Length; i += 3) {
                var read = (permissionString [i] == 'r') ? 4 : 0;
                var write = (permissionString [i + 1] == 'w') ? 2 : 0;
                var exec = (permissionString [i + 2] == 'x') ? 1 : 0;

                ftpPermissions.Enqueue (read + write + exec);
            }

            return new FtpPermission (
                user: ftpPermissions.Dequeue (),
                group: ftpPermissions.Dequeue (),
                world: ftpPermissions.Dequeue ()
            );
        }
    }

    public enum FtpPermissionType
    {
        None = 0,
        Read = 4,
        ReadWrite = 6,
        ReadExecute = 5,
        ReadWriteExecute = 7,
        Write = 2,
        WriteExecute = 3,
        Execute = 1
    }
}
