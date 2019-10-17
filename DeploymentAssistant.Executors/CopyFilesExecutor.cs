using DeploymentAssistant.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Executors
{
    /// <summary>
    /// copy files from a source path executor
    /// </summary>
    internal class CopyFilesExecutor : AbstractExecutor, IExecutor
    {
        public CopyFilesExecutor(ExecutionActivity activity, IShellManager shellManager)
            : base(activity, shellManager)
        {
            logger = LogManager.GetLogger(typeof(StopServiceExecutor));
        }

        /// <summary>
        /// Execute the step 
        /// </summary>
        public override void Execute()
        {
            logger.Info("Copy files Activity Execution Started.");
            var activity = this.Activity as CopyFilesActivity;
            var source = activity.Host.HostName;
            logger.Info(string.Format("Remote Computer Name: {0}", source));
            if (string.IsNullOrWhiteSpace(source))
            {
                return;
            }

            ////Copy the files
            CopyFiles(activity, source);
        }

        private void CopyFiles(CopyFilesActivity activity, string source)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Verify the execution
        /// </summary>
        public override void Verify()
        {
            throw new NotImplementedException();
        }
    }
}
