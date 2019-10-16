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
            var remoteComputerName = string.IsNullOrWhiteSpace(activity.Host.HostName) ? activity.Host.Ip : string.Empty;
            logger.Info(string.Format("Remote Computer Name: {0}", remoteComputerName));
            if (string.IsNullOrWhiteSpace(remoteComputerName))
            {
                return;
            }
            //// start the service through command
            StopService(activity, remoteComputerName);
            if (quitExecuting)
            {
                return;
            }
        }

        /// <summary>
        /// Verification Method - after executing the action
        /// </summary>
        public override void Verify()
        {
            logger.Info("Service Stop - Activity Verification Started.");
            var activity = this.Activity as StopServiceActivity;
            var remoteComputerName = string.IsNullOrWhiteSpace(activity.Host.HostName) ? activity.Host.Ip : string.Empty;
            //// verify the status of the service
            string status = VerifyService(activity, remoteComputerName);
            if (quitExecuting)
            {
                return;
            }
            this.Result = new ExecutionResult() { IsSuccess = status.Equals(Constants.PowershellScripts.RunningStatus) };
            logger.InfoFormat("Verification Info: {0}", this.Result.ToJson());
        }

        private string VerifyService(StopServiceActivity activity, string remoteComputerName)
        {
            var status = string.Empty;
            try
            {
                var getServiceScripts = new List<string>
                    {
                        Constants.PowershellScripts.GetServiceFunction,
                        string.Format(Constants.PowershellScripts.GetService, activity.ServiceName)
                    };
                status = _shellManager.GetValue(remoteComputerName, getServiceScripts, true);
            }
            catch (ApplicationException appEx)
            {
                logger.Error(appEx.Message);
                HandleException(appEx, activity);
            }

            return status;
        }

        private void StopService(StopServiceActivity activity, string remoteComputerName)
        {
            try
            {
                var stopServiceScript = string.Format(Constants.PowershellScripts.StopService, activity.ServiceName);
                _shellManager.ExecuteCommands(remoteComputerName, new List<string> { stopServiceScript }, true);
            }
            catch (ApplicationException appEx)
            {
                logger.Error(appEx.Message);
                HandleException(appEx, activity);
            }
        }


    }
}
