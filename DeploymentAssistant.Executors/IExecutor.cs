using DeploymentAssistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Executors
{
    /// <summary>
    /// interface for an executor - of a step / activity
    /// </summary>
    public interface IExecutor
    {
        ExecutionActivity Activity { get; }

        ExecutionResult Result { get; }

        void Execute();
    }
}
