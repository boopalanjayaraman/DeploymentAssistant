using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Models
{
    /// <summary>
    /// Activity representing a windows service stop
    /// </summary>
    public class ServiceStopActivity : ExecutionActivity
    {
        public string ServiceName { get; set; }

    }
}
