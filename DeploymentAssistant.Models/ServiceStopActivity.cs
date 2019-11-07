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
    public class StopServiceActivity : ExecutionActivity
    {
        public string ServiceName { get; set; }

        /// <summary>
        /// Self validating function
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(ServiceName);
        }

        /// <summary>
        /// activity type
        /// </summary>
        public override ExecutionType Operation
        {
            get
            {
                return ExecutionType.StopService;
            }
        }

        public StopServiceActivity()
        {

        }
    }
}
