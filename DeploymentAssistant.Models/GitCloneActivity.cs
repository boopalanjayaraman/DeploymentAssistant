using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Models
{
    /// <summary>
    /// Activity representing a Git clone operation
    /// </summary>
    public class GitCloneActivity : ExecutionActivity
    {
        /// <summary>
        /// Local directory path into which repo will be cloned
        /// </summary>
        public string LocalDestinationPath { get; set; }

        /// <summary>
        /// Git Repo Url
        /// </summary>
        public string RepoUrl { get; set; }

        /// <summary>
        /// When true, it tries to clone first, but if the repo clone already exists, it does a pull.
        /// When false, only does a clone.
        /// </summary>
        public bool UseCloneOrPull { get; set; }

        /// <summary>
        /// Self validating function
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            return (!string.IsNullOrWhiteSpace(LocalDestinationPath))
                && (!string.IsNullOrWhiteSpace(RepoUrl));
        }

        /// <summary>
        /// activity type
        /// </summary>
        public override ExecutionType Operation
        {
            get
            {
                return ExecutionType.GitClone;
            }
        }

        public GitCloneActivity()
        {

        }
    }
}
