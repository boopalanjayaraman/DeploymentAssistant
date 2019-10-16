using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Models
{
    /// <summary>
    /// Abstract type representing Execution Step
    /// Common parameters
    /// </summary>
    public abstract class ExecutionActivity
    {
        /// <summary>
        /// Order of the execution, if blank, is filled automatically in the order the activity appears
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Name of the step / activity - serves as identifier
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of operation / activity that is being done
        /// </summary>
        public ExecutionType Operation { get; set; }

        /// <summary>
        /// HostInfo for executing the operation
        /// </summary>
        public HostInfo Host { get; set; }

        /// <summary>
        /// If true, pipeline execution continues even though there may be an exception / failure in the step
        /// default : false
        /// </summary>
        public bool ContinueOnFailure { get; set; }

        /// <summary>
        /// Validates self and returns a boolean
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValid()
        {
            return true;
        }

        public ExecutionActivity()
        {

        }
    }
}
