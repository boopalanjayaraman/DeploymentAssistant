using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Models
{
    /// <summary>
    /// Represents the activity of deleting files in a folder
    /// </summary>
    public class DeleteFilesActivity : ExecutionActivity
    {
        /// <summary>
        /// Target path on the given hosts.
        /// </summary>
        public string DestinationPath { get; set; }

        /// <summary>
        /// activity type
        /// </summary>
        public override ExecutionType Operation
        {
            get
            {
                return ExecutionType.DeleteFiles;
            }
        }

        /// <summary>
        /// Self validating function
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(DestinationPath);
        }
    }
}
