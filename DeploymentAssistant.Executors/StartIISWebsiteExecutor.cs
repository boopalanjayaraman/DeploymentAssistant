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
    /// Start a IIS Web site on a given host - executor
    /// </summary>
    internal class StartIISWebsiteExecutor : AbstractExecutor, IExecutor
    {
        public StartIISWebsiteExecutor(ExecutionActivity activity, IShellManager shellManager)
            : base(activity, shellManager)
        {
            logger = LogManager.GetLogger(typeof(StartIISWebsiteExecutor));
        }

        /// <summary>
        /// Execute step method
        /// </summary>
        public override void Execute()
        {
            logger.Info("Start IIS Web site - Activity Execution Started.");
            var activity = this.Activity as StartIISWebsiteActivity;
            var host = activity.Host.HostName;
            StartIISWebsite(activity, host);
            logger.Info("Activity Execution Finished.");
        }

        private void StartIISWebsite(StartIISWebsiteActivity activity, string host)
        {
            try
            {
                var startIISwebsiteScript = new ScriptWithParameters();
                startIISwebsiteScript.Script = this.ActivityScriptMap.ExecutionScript;
                startIISwebsiteScript.Params = new Dictionary<string, object>();
                startIISwebsiteScript.Params.Add("website", activity.WebsiteName);
                var response = _shellManager.ExecuteCommands(host, new List<ScriptWithParameters> { startIISwebsiteScript }, true);
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
            logger.Info("Start IIS Website - Activity Verification Started.");
            if (quitExecuting)
            {
                logger.Info("Activity Verification skipped. QuitExecuting Flag is true.");
                this.Result = new ExecutionResult();
                return;
            }

            var activity = this.Activity as StartIISWebsiteActivity;
            var host = activity.Host.HostName;
            var status = VerifyStartIISWebsite(activity, host);

            this.Result = new ExecutionResult() { IsSuccess = !status.Equals("0") };
            logger.InfoFormat("Verification Finished. Result: {0}", this.Result.ToJson());
        }

        private string VerifyStartIISWebsite(StartIISWebsiteActivity activity, string host)
        {
            var status = string.Empty;
            try
            {
                var verifyScript = new ScriptWithParameters();
                verifyScript.Script = this.ActivityScriptMap.VerificationScript;
                verifyScript.Params = new Dictionary<string, object>();
                verifyScript.Params.Add("website", activity.WebsiteName);
                var result = _shellManager.ExecuteCommands(host, new List<ScriptWithParameters> { verifyScript }, true);
                status = result[0] != null ? result[0].ToString() : string.Empty;
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
