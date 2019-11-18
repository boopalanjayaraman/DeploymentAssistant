using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Models
{
    /// <summary>
    /// Activity representing a MS Build operation
    /// </summary>
    public class MsBuildActivity : ExecutionActivity
    {
        /// <summary>
        /// Local MsBuild exe full path
        /// </summary>
        public string LocalMsBuildPath { get; set; }

        /// <summary>
        /// Solution Full Path
        /// </summary>
        public string SolutionPath { get; set; }

        /// <summary>
        /// Self validating function
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            return (!string.IsNullOrWhiteSpace(LocalMsBuildPath))
                && (!string.IsNullOrWhiteSpace(SolutionPath));
        }

        /// <summary>
        /// activity type
        /// </summary>
        public override ExecutionType Operation
        {
            get
            {
                return ExecutionType.MsBuild;
            }
        }

        public MsBuildActivity()
        {

        }
    }
}
