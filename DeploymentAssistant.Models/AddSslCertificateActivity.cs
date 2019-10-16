using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Models
{
    /// <summary>
    /// Represents adding an ssl certificate to a web site activity
    /// </summary>
    public class AddSslCertificateActivity : ExecutionActivity
    {
        /// <summary>
        /// Full file path of the certificate to be applied
        /// </summary>
        public string CertificateFilePath { get; set; }

        /// <summary>
        /// WebHosting / My 
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string CertificatePassword { get; set; }

        /// <summary>
        /// Certificate thumbprint. This will be used while adding the certificate to the site.
        /// </summary>
        public string CertificateThumbprint { get; set; }

        public AddSslCertificateActivity()
        {

        }
    }
}
