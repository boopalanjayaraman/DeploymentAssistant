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
        /// <summary>
        /// Activity under context
        /// </summary>
        ExecutionActivity Activity { get; }

        /// <summary>
        /// output of the execution step
        /// </summary>
        ExecutionResult Result { get; }

        /// <summary>
        /// Executing method
        /// </summary>
        void Execute();

        /// <summary>
        /// Verifying method - after the action has been done
        /// </summary>
        void Verify();
    }
}
