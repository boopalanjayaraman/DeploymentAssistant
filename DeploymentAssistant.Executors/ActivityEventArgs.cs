using DeploymentAssistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Executors
{
    /// <summary>
    /// Activity event args for raising activity related events
    /// </summary>
    public class ActivityEventArgs : EventArgs
    {
        public ExecutionActivity Activity { get; set; }

        public ExecutionResult Result { get; set; }
    }
}
