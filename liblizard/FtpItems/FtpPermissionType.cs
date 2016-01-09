using System;

namespace Codeaddicts.Lizard
{
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

    public enum FtpPermission {
        None = 0,
        Execute = 1 << 0,
        Write = 1 << 1,
        Read = 1 << 2
    }
}

