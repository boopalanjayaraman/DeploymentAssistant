using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeploymentAssistant.Models;
using DeploymentAssistant.Common;
using log4net;
using log4net.Repository.Hierarchy;

namespace DeploymentAssistant.Executors
{
    /// <summary>
    /// Windows Service - start activity - executor
    /// </summary>
    internal class StartServiceExecutor : AbstractExecutor, IExecutor
    {
        public StartServiceExecutor(ExecutionActivity activity, IShellManager shellManager)
            : base(activity, shellManager)
        {
            logger = LogManager.GetLogger(typeof(StartServiceExecutor));
        }

        /// <summary>
        /// Execute step method
        /// </summary>
        public override void Execute()
        {
            logger.Info("Service Start - Activity Execution Started.");
            var activity = this.Activity as StartServiceActivity;
            var remoteComputerName = activity.Host.HostName;
            //// start the service through command
            StartService(activity, remoteComputerName);
            logger.Info("Service Start - Activity Execution Finished.");
        }

        /// <summary>
        /// Verification Method - after executing the action
        /// </summary>
        public override void Verify()
        {
            logger.Info("Service Start - Activity Verification Started.");
            if (quitExecuting)
            {
                logger.Info("Service Start - Activity Verification skipped. QuitExecuting Flag is true.");
                this.Result = new ExecutionResult();
                return;
            }
            var activity = this.Activity as StartServiceActivity;
            var remoteComputerName = activity.Host.HostName;
            //// verify the status of the service
            string status = VerifyService(activity, remoteComputerName);

            this.Result = new ExecutionResult() { IsSuccess = !status.Equals("0") };
            logger.InfoFormat("Verification Finished. Result: {0}", this.Result.ToJson());
        }

        private string VerifyService(StartServiceActivity activity, string remoteComputerName)
        {
            var status = string.Empty;
            try
            {
                var verifyStartServiceCallScript = string.Format(Constants.PowershellScripts.VerifyStartServiceCall, activity.ServiceName);
                status = _shellManager.GetValue(remoteComputerName, new List<string> { this.ActivityScriptMap.VerificationScript, verifyStartServiceCallScript }, true);
            }
            catch (ApplicationException appEx)
            {
                logger.Error(appEx.Message);
                HandleException(appEx, activity);
            }

            return status;
        }

        private void StartService(StartServiceActivity activity, string remoteComputerName)
        {
            try
            {
                var startServiceCallScript = string.Format(Constants.PowershellScripts.StartServiceCall, activity.ServiceName);
                _shellManager.ExecuteCommands(remoteComputerName, new List<string> { this.ActivityScriptMap.ExecutionScript, startServiceCallScript }, true);
            }
            catch (ApplicationException appEx)
            {
                logger.Error(appEx.Message);
                HandleException(appEx, activity);
            }
        }
    }
}
