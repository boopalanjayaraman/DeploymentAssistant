using DeploymentAssistant.Common;
using DeploymentAssistant.Executors.Models;
using DeploymentAssistant.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
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
                copyFilesScript.Params = new Dictionary<string, object>();
                copyFilesScript.Params.Add("sourcePath", activity.SourcePath);
                copyFilesScript.Params.Add("destinationPath", activity.DestinationPath);
                copyFilesScript.Params.Add("excludeExtensions", activity.ExcludeExtensions.ToArray());
                copyFilesScript.Params.Add("skipFolders", activity.SkipFolders.ToArray());
                copyFilesScript.Params.Add("skipFoldersIfExist", activity.SkipFoldersIfExist.ToArray());

                var response = _shellManager.ExecuteCommands(host, new List<ScriptWithParameters> { copyFilesScript }, true);
            }
            catch (RemoteException rEx)
            {
                logger.Error(rEx.Message);
                HandleException(rEx, activity);
            }
            catch (ApplicationException appEx)
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
            if (quitExecuting)
            {
                logger.Info("Activity Verification skipped. QuitExecuting Flag is true.");
                this.Result = new ExecutionResult();
                return;
            }

            var activity = this.Activity as CopyFilesActivity;
            var host = activity.Host.HostName;
            var status = VerifyCopyFiles(activity, host);

            this.Result = new ExecutionResult() { IsSuccess = !status.Equals("0") };
            logger.InfoFormat("Verification Finished. Result: {0}", this.Result.ToJson());
        }

        private string VerifyCopyFiles(CopyFilesActivity activity, string host)
        {
            var status = string.Empty;
            try
            {
                var verifyScript = new ScriptWithParameters();
                verifyScript.Script = this.ActivityScriptMap.VerificationScript;
                verifyScript.Params = new Dictionary<string, object>();
                verifyScript.Params.Add("destinationPath", activity.DestinationPath);
                var result = _shellManager.ExecuteCommands(host, new List<ScriptWithParameters> { verifyScript }, true);
                status = result.FirstOrDefault() != null ? result[0].ToString() : "0";
            }
            catch (ApplicationException appEx)
            {
                logger.Error(appEx.Message);
                HandleException(appEx, activity);
            }

            return status;
        }
    }
}
