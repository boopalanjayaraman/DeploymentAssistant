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
        public string HostName { get; set; }

        public string Ip { get; set; }

        public string Port { get; set; }
    }
}
