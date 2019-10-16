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
        public string SiteName { get; set; }

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
        bool OverrideIfExists { get; set; }

        public CreateIISWebsiteActivity()
        {

        }
    }
}
