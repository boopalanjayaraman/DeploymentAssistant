using DeploymentAssistant.Common;
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
        /// Execute step method
        /// </summary>
        public override void Execute()
        {
            logger.Info("Copy files - Activity Execution Started.");
            var activity = this.Activity as CopyFilesActivity;
            var host = activity.Host.HostName;
            //// Copy the files
            CopyFiles(activity, host);
            logger.Info("Service Start - Activity Execution Finished.");
        }

        private void CopyFiles(CopyFilesActivity activity, string host)
        {
            try
            {
                //// TODO: call execute commands with arguments (c# objects)
                //var copyFilesScriptCall = string.Format(Constants.PowershellScripts.StopServiceCall, activity.);
                //_shellManager.ExecuteCommands(host, new List<string> { this.ActivityScriptMap.ExecutionScript, copyFilesScriptCall }, true);
            }
            catch(ApplicationException appEx)
            {
                logger.Error(appEx.Message);
                HandleException(appEx, activity);
            }
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
