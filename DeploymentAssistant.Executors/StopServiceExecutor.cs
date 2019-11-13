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
    /// Windows Service - stop executor
    /// </summary>
    internal class StopServiceExecutor : AbstractExecutor, IExecutor
    {
        public StopServiceExecutor(ExecutionActivity activity, IShellManager shellManager)
            : base(activity, shellManager)
        {
            logger = LogManager.GetLogger(typeof(StopServiceExecutor));
        }

        /// <summary>
        /// Execute step method
        /// </summary>
        public override void Execute()
        {
            logger.Info("Service Stop - Activity Execution Started.");
            var activity = this.Activity as StopServiceActivity;
            var remoteComputerName = activity.Host.HostName;
            //// start the service through command
            StopService(activity, remoteComputerName);
            logger.Info("Activity Execution Finished.");
        }

        /// <summary>
        /// Verification Method - after executing the action
        /// </summary>
        public override void Verify()
        {
            logger.Info("Service Stop - Activity Verification Started.");
            if (quitExecuting)
            {
                logger.Info("Activity Verification skipped. QuitExecuting Flag is true.");
                this.Result = new ExecutionResult();
                return;
            }
            var activity = this.Activity as StopServiceActivity;
            var remoteComputerName = activity.Host.HostName;
            //// verify the status of the service
            string status = VerifyService(activity, remoteComputerName);
            this.Result = new ExecutionResult() { IsSuccess = !status.Equals("0") };
            logger.InfoFormat("Verification Finished. Result: {0}", this.Result.ToJson());
        }

        private string VerifyService(StopServiceActivity activity, string host)
        {
            var status = string.Empty;
            try
            {
                var verifyScript = new ScriptWithParameters();
                verifyScript.Script = this.ActivityScriptMap.VerificationScript;
                verifyScript.Params = new Dictionary<string, object>();
                verifyScript.Params.Add("serviceName", activity.ServiceName);
                var response = _shellManager.ExecuteCommands(host, new List<ScriptWithParameters> { verifyScript }, true);
            }
            catch (ApplicationException appEx)
            {
                logger.Error(appEx.Message);
                HandleException(appEx, activity);
            }

            return status;
        }

        private void StopService(StopServiceActivity activity, string host)
        {
            try
            {
                var stopServiceScript = new ScriptWithParameters();
                stopServiceScript.Script = this.ActivityScriptMap.ExecutionScript;
                stopServiceScript.Params = new Dictionary<string, object>();
                stopServiceScript.Params.Add("serviceName", activity.ServiceName);
                var response = _shellManager.ExecuteCommands(host, new List<ScriptWithParameters> { stopServiceScript }, true);
            }
            catch (ApplicationException appEx)
            {
                logger.Error(appEx.Message);
                HandleException(appEx, activity);
            }
        }


    }
}
