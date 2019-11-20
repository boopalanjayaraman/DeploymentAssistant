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
    /// delete files from a source path - executor
    /// </summary>
    internal class DeleteFilesExecutor : AbstractExecutor, IExecutor
    {
        public DeleteFilesExecutor(ExecutionActivity activity, IShellManager shellManager)
            : base(activity, shellManager)
        {
            logger = LogManager.GetLogger(typeof(DeleteFilesExecutor));
        }

        /// <summary>
        /// Execute step method
        /// </summary>
        public override void Execute()
        {
            logger.Info("Delete files - Activity Execution Started.");
            var activity = this.Activity as DeleteFilesActivity;
            var host = activity.Host.HostName;
            //// delete the files
            DeleteFiles(activity, host);
            logger.Info("Activity Execution Finished.");
        }

        private void DeleteFiles(DeleteFilesActivity activity, string host)
        {
            try
            {
                var deleteFilesScript = new ScriptWithParameters();
                deleteFilesScript.Script = this.ActivityScriptMap.ExecutionScript;
                deleteFilesScript.Params = new Dictionary<string, object>();
                deleteFilesScript.Params.Add("destinationPath", activity.DestinationPath);
                var response = _shellManager.ExecuteCommands(host, new List<ScriptWithParameters> { deleteFilesScript }, true);
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
            logger.Info("Delete Files - Activity Verification Started.");
            if (quitExecuting)
            {
                logger.Info("Activity Verification skipped. QuitExecuting Flag is true.");
                this.Result = new ExecutionResult();
                return;
            }

            var activity = this.Activity as DeleteFilesActivity;
            var host = activity.Host.HostName;
            var status = VerifyDeleteFiles(activity, host);

            this.Result = new ExecutionResult() { IsSuccess = !status.Equals("0") };
            logger.InfoFormat("Verification Finished. Result: {0}", this.Result.ToJson());
        }

        private string VerifyDeleteFiles(DeleteFilesActivity activity, string host)
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
