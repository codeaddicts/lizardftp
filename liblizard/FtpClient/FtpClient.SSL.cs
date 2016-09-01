using Codeaddicts.Lizard.Raw;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Codeaddicts.Lizard {
    public partial class FtpClient {
        public Stream ClientStream2;
        public void EstablishSSLConnection () {
            this.AUTH ("TLS");
            SecureStream ();
            this.ClientReader = new StreamReader ((SslStream)ClientStream);
        }

        public void SecureStream () {
            RemoteCertificateValidationCallback callback = new RemoteCertificateValidationCallback (CertificateValidationCallback);
            SslStream _sslStream = new SslStream (this.ClientStream, false, callback);//,new RemoteCertificateValidationCallback(ValidateServerCertificate), null);

            try {


                _sslStream.AuthenticateAsClient (
                    this.Host,
                    new X509CertificateCollection (),
                    SslProtocols.Ssl3 | SslProtocols.Tls,
                    true);

                if (_sslStream.IsAuthenticated)
                    this.ClientStream2 = _sslStream;
                else
                    throw new IOException ("Not authenticated");

            } catch (Exception ex) {
                throw ex;
            }
        }

        private bool CertificateValidationCallback (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors) {
            Console.WriteLine ("Server Certificate Issued To: {0}", certificate.Subject);
            Console.WriteLine ("Server Certificate Issued By: {0}", certificate.Issuer);
            
            // The certificate can also be manually verified to make sure it meets your specific policies by interrogating the x509Certificate object.

            if (policyErrors != SslPolicyErrors.None) {
                // Something is wrong
                Console.WriteLine (policyErrors.ToString ());
                return false;
            } else {
                // Yay, everything alright
                return true;
            }
        }
    }
}
