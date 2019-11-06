using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Models
{
    /// <summary>
    /// Activity representing a windows service start
    /// </summary>
    public class StartServiceActivity : ExecutionActivity
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

        public StartServiceActivity()
        {
            this.Operation = ExecutionType.StartService;
        }
    }
}
