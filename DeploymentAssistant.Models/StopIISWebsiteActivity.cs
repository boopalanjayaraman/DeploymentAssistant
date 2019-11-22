using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Models
{
    /// <summary>
    /// Represents stopping a IIS website activity
    /// </summary>
    public class StopIISWebsiteActivity : ExecutionActivity
    {
        /// <summary>
        /// Name of the website
        /// </summary>
        public string WebsiteName { get; set; }

        /// <summary>
        /// activity type
        /// </summary>
        public override ExecutionType Operation
        {
            get
            {
                return ExecutionType.StopIISWebsite;
            }
        }

        public StopIISWebsiteActivity()
        {

        }

        /// <summary>
        /// Self validating function
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(WebsiteName);
        }
    }
}
