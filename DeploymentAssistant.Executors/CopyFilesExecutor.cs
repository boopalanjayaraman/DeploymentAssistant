using DeploymentAssistant.Common;
using DeploymentAssistant.Executors.Models;
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
            logger = LogManager.GetLogger(typeof(CopyFilesExecutor));
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
            logger.Info("Activity Execution Finished.");
        }

        private void CopyFiles(CopyFilesActivity activity, string host)
        {
            try
            {
                var copyFilesScript = new ScriptWithParameters();
                copyFilesScript.Script = this.ActivityScriptMap.ExecutionScript;
                var copyFilesCall = new ScriptWithParameters();
                copyFilesCall.Script = Constants.PowershellScripts.CopyFilesCall;
                copyFilesCall.Params = new List<object>();
                copyFilesCall.Params.Add(activity.SourcePath);
                copyFilesCall.Params.Add(activity.DestinationPath);
                copyFilesCall.Params.Add(activity.ExcludeExtensions);
                copyFilesCall.Params.Add(activity.SkipFolders);
                copyFilesCall.Params.Add(activity.SkipFoldersIfExist);
                var response = _shellManager.ExecuteCommands(host, new List<ScriptWithParameters> { copyFilesScript, copyFilesCall }, true);
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
            logger.Info("Copy Files - Activity Verification Started.");
            logger.Info("No verification method is implemented / was necessary.");
            this.Result = new ExecutionResult() { IsSuccess = true };
            logger.InfoFormat("Verification Finished. Result: {0}", this.Result.ToJson());
        }
    }
}
