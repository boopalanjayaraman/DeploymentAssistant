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
        /// Build targets
        /// Ex: Build or Clean 
        /// This will get transformed to -t:Build in the script. So, not necessary to prepend -t:
        /// Multiple targets can be specified with semicolon. Ex. PrepareResources;Compile
        /// </summary>
        public string BuildTargets { get; set; }

        /// <summary>
        /// Build Properties
        /// Ex: Configuration=Release
        /// this will get transformed into -p:Configuration=Release in the script. Not necessary to prepend - p:
        /// Multiple properties can be specified with semicolon. Ex. Configuration=Release;OutDir=bin\Prod
        /// </summary>
        public string BuildProperties { get; set; }

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
