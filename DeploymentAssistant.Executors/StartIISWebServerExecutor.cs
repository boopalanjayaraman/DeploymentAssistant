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
    /// Start IIS Web server on a given host - executor
    /// </summary>
    internal class StartIISWebServerExecutor : AbstractExecutor, IExecutor
    {
        public StartIISWebServerExecutor(ExecutionActivity activity, IShellManager shellManager)
            : base(activity, shellManager)
        {
            logger = LogManager.GetLogger(typeof(StartIISWebServerExecutor));
        }

        /// <summary>
        /// Execute step method
        /// </summary>
        public override void Execute()
        {
            logger.Info("Start IIS Web Server - Activity Execution Started.");
            var activity = this.Activity as StartIISWebServerActivity;
            var host = activity.Host.HostName;
            StartIISWebServer(activity, host);
            logger.Info("Activity Execution Finished.");
        }

        private void StartIISWebServer(StartIISWebServerActivity activity, string host)
        {
            try
            {
                var startIISWebServerScript = new ScriptWithParameters();
                startIISWebServerScript.Script = this.ActivityScriptMap.ExecutionScript;
                var response = _shellManager.ExecuteCommands(host, new List<ScriptWithParameters> { startIISWebServerScript }, true);
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
            logger.Info("Start IIS Web Server - Activity Verification Started.");
            if (quitExecuting)
            {
                logger.Info("Activity Verification skipped. QuitExecuting Flag is true.");
                this.Result = new ExecutionResult();
                return;
            }
            logger.Info("No verification method is implemented / was necessary.");
            this.Result = new ExecutionResult() { IsSuccess = true };
            logger.InfoFormat("Verification Finished. Result: {0}", this.Result.ToJson());
        }


    }
}
