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
        public string CertificateLocalPath { get; set; }

        /// <summary>
        /// LocalMachine 
        /// </summary>
        public string StoreLocation { get; set; }

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

        /// <summary>
        /// Website Name to which ssl certificate is going to be attached to
        /// </summary>
        public string WebsiteName { get; set; }

        /// <summary>
        /// SSL Port of website
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// Host Header of website
        /// </summary>
        public string HostHeader { get; set; }

        /// <summary>
        /// Binding IP value
        /// </summary>
        public string BindingIp { get; set; }

        /// <summary>
        /// activity type
        /// </summary>
        public override ExecutionType Operation
        {
            get
            {
                return ExecutionType.AddSslCertificate;
            }
        }

        public AddSslCertificateActivity()
        {

        }

        /// <summary>
        /// Self validating function
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(CertificateLocalPath)
                && !string.IsNullOrWhiteSpace(StoreName)
                && !string.IsNullOrWhiteSpace(WebsiteName);
        }
    }
}
