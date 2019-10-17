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
            logger.Info(string.Format("Remote Computer Name: {0}", remoteComputerName));
            if (string.IsNullOrWhiteSpace(remoteComputerName))
            {
                return;
            }
            //// start the service through command
            StartService(activity, remoteComputerName);
        }

        /// <summary>
        /// Verification Method - after executing the action
        /// </summary>
        public override void Verify()
        {
            logger.Info("Service Start - Activity Verification Started.");
            var activity = this.Activity as StartServiceActivity;
            var remoteComputerName = activity.Host.HostName;
            //// verify the status of the service
            string status = VerifyService(activity, remoteComputerName);
            if (quitExecuting)
            {
                return;
            }
            this.Result = new ExecutionResult() { IsSuccess = status.Equals(Constants.PowershellScripts.RunningStatus) };
            logger.InfoFormat("Verification Info: {0}", this.Result.ToJson());
        }

        private string VerifyService(StartServiceActivity activity, string remoteComputerName)
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

        private void StartService(StartServiceActivity activity, string remoteComputerName)
        {
            try
            {
                var startServiceScript = string.Format(Constants.PowershellScripts.StartService, activity.ServiceName);
                _shellManager.ExecuteCommands(remoteComputerName, new List<string> { startServiceScript }, true);
            }
            catch (ApplicationException appEx)
            {
                logger.Error(appEx.Message);
                HandleException(appEx, activity);
            }
        }
    }
}
