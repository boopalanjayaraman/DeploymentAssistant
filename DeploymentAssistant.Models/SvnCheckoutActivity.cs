using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Models
{
    /// <summary>
    /// Activity representing a Svn Update operation
    /// </summary>
    public class SvnCheckoutActivity : ExecutionActivity
    {
        /// <summary>
        /// Local directory path into which repo will be checked out
        /// </summary>
        public string LocalDestinationPath { get; set; }

        /// <summary>
        /// Git Repo Url
        /// </summary>
        public string RepoUrl { get; set; }

        /// <summary>
        /// Username - for svn repo access
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password - for svn repo access
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// When true, it tries to do a svn check out first, but if the repo copy already exists, it does a svn update.
        /// When false, only does a check out.
        /// </summary>
        public bool UseCheckoutOrUpdate { get; set; }

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
                return ExecutionType.SvnCheckout;
            }
        }

        public SvnCheckoutActivity()
        {

        }
    }
}
