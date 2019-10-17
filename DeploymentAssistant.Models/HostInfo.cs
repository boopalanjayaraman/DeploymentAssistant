using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Models
{
    /// <summary>
    /// Represents the configuration of the machine / host where an activity might be performed
    /// </summary>
    public class HostInfo
    {
        /// <summary>
        /// Can mention either hostname of the computer or IP
        /// </summary>
        public string HostName { get; set; }

        public string Port { get; set; }
    }
}
