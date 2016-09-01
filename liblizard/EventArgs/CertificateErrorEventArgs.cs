using System;
using System.Net.Security;

namespace Codeaddicts.Lizard
{
    public class CertificateErrorEventArgs : EventArgs
    {
        readonly public SslPolicyErrors Errors;

        public CertificateErrorEventArgs (SslPolicyErrors errors) {
            Errors = errors;
        }
    }
}

