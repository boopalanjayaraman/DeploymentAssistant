using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Models
{
    /// <summary>
    /// Represents creating a IIS website activity
    /// </summary>
    public class CreateIISWebsiteActivity : ExecutionActivity
    {
        /// <summary>
        /// Website Name
        /// </summary>
        public string WebsiteName { get; set; }

        /// <summary>
        /// Bindings
        /// ex: {"http" : "*:80:"}
        /// </summary>
        public Dictionary<string, string> Bindings { get; set; }

        /// <summary>
        /// local folder path to which the website is to be mapped
        /// </summary>
        public string PhysicalPath { get; set; }

        /// <summary>
        /// If the web site already exists, update the settings rather than stopping.
        /// default : false. Leaves if exists.
        /// </summary>
        public bool OverrideIfExists { get; set; }

        /// <summary>
        /// activity type
        /// </summary>
        public override ExecutionType Operation
        {
            get
            {
                return ExecutionType.CreateIISWebsite;
            }
        }

        public CreateIISWebsiteActivity()
        {

        }

        /// <summary>
        /// Self validating function
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(WebsiteName)
                && !string.IsNullOrWhiteSpace(PhysicalPath)
                && Bindings != null
                && Bindings.Count > 0;
        }
    }
}
