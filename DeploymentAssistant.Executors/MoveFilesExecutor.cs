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
    /// move files from a source path executor
    /// </summary>
    internal class MoveFilesExecutor : AbstractExecutor, IExecutor
    {
        public MoveFilesExecutor(ExecutionActivity activity, IShellManager shellManager)
            : base(activity, shellManager)
        {
            logger = LogManager.GetLogger(typeof(MoveFilesExecutor));
        }

        /// <summary>
        /// Execute step method
        /// </summary>
        public override void Execute()
        {
            logger.Info("Move files - Activity Execution Started.");
            var activity = this.Activity as MoveFilesActivity;
            var host = activity.Host.HostName;
            //// move the files
            MoveFiles(activity, host);
            logger.Info("Activity Execution Finished.");
        }

        private void MoveFiles(MoveFilesActivity activity, string host)
        {
            try
            {
                var moveFilesScript = new ScriptWithParameters();
                moveFilesScript.Script = this.ActivityScriptMap.ExecutionScript;
                moveFilesScript.Params = new Dictionary<string, object>();
                moveFilesScript.Params.Add("sourcePath", activity.SourcePath);
                moveFilesScript.Params.Add("destinationPath", activity.DestinationPath);
                var response = _shellManager.ExecuteCommands(host, new List<ScriptWithParameters> { moveFilesScript }, true);
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
            logger.Info("Move Files - Activity Verification Started.");
            if (quitExecuting)
            {
                logger.Info("Activity Verification skipped. QuitExecuting Flag is true.");
                this.Result = new ExecutionResult();
                return;
            }

            var activity = this.Activity as MoveFilesActivity;
            var host = activity.Host.HostName;
            var status = VerifyMoveFiles(activity, host);

            this.Result = new ExecutionResult() { IsSuccess = !status.Equals("0") };
            logger.InfoFormat("Verification Finished. Result: {0}", this.Result.ToJson());
        }

        private string VerifyMoveFiles(MoveFilesActivity activity, string host)
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
