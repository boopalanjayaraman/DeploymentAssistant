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
    /// Stop IIS Web server on a given host - executor
    /// </summary>
    internal class StopIISWebServerExecutor : AbstractExecutor, IExecutor
    {
        public StopIISWebServerExecutor(ExecutionActivity activity, IShellManager shellManager)
            : base(activity, shellManager)
        {
            logger = LogManager.GetLogger(typeof(StopIISWebServerExecutor));
        }

        /// <summary>
        /// Execute step method
        /// </summary>
        public override void Execute()
        {
            logger.Info("Stop IIS Web Server - Activity Execution Started.");
            var activity = this.Activity as StopIISWebServerActivity;
            var host = activity.Host.HostName;
            StopIISWebServer(activity, host);
            logger.Info("Activity Execution Finished.");
        }

        private void StopIISWebServer(StopIISWebServerActivity activity, string host)
        {
            try
            {
                var stopIISWebServerScript = new ScriptWithParameters();
                stopIISWebServerScript.Script = this.ActivityScriptMap.ExecutionScript;
                var response = _shellManager.ExecuteCommands(host, new List<ScriptWithParameters> { stopIISWebServerScript }, true);
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
            logger.Info("Stop IIS Web Server - Activity Verification Started.");
            logger.Info("No verification method is implemented / was necessary.");
            this.Result = new ExecutionResult() { IsSuccess = true };
            logger.InfoFormat("Verification Finished. Result: {0}", this.Result.ToJson());
        }


    }
}
