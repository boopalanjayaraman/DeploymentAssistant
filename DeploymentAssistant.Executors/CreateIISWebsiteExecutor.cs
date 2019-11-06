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
    /// Create a IIS Web site on a given host - executor
    /// </summary>
    internal class CreateIISWebsiteExecutor : AbstractExecutor, IExecutor
    {
        public CreateIISWebsiteExecutor(ExecutionActivity activity, IShellManager shellManager)
            : base(activity, shellManager)
        {
            logger = LogManager.GetLogger(typeof(CreateIISWebsiteExecutor));
        }

        /// <summary>
        /// Execute step method
        /// </summary>
        public override void Execute()
        {
            logger.Info("Create IIS Web site - Activity Execution Started.");
            var activity = this.Activity as CreateIISWebsiteActivity;
            var host = activity.Host.HostName;
            CreateIISWebsite(activity, host);
            logger.Info("Activity Execution Finished.");
        }

        private void CreateIISWebsite(CreateIISWebsiteActivity activity, string host)
        {
            try
            {
                var createIISwebsiteScript = new ScriptWithParameters();
                createIISwebsiteScript.Script = this.ActivityScriptMap.ExecutionScript;
                var createIISWebsiteCallScript = new ScriptWithParameters();
                createIISWebsiteCallScript.Script = Constants.PowershellScripts.CreateIISWebsiteCall;
                createIISWebsiteCallScript.Params = new List<object>();
                createIISWebsiteCallScript.Params.Add(activity.WebsiteName);
                createIISWebsiteCallScript.Params.Add(activity.Bindings);
                createIISWebsiteCallScript.Params.Add(activity.PhysicalPath);
                createIISWebsiteCallScript.Params.Add(activity.OverrideIfExists);
                var response = _shellManager.ExecuteCommands(host, new List<ScriptWithParameters> { createIISwebsiteScript, createIISWebsiteCallScript }, true);
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
            logger.Info("Create IIS Website - Activity Verification Started.");
            if (quitExecuting)
            {
                logger.Info("Activity Verification skipped. QuitExecuting Flag is true.");
                this.Result = new ExecutionResult();
                return;
            }

            var activity = this.Activity as CreateIISWebsiteActivity;
            var host = activity.Host.HostName;
            var status = VerifyCreateIISWebsite(activity, host);

            this.Result = new ExecutionResult() { IsSuccess = !status.Equals("0") };
            logger.InfoFormat("Verification Finished. Result: {0}", this.Result.ToJson());
        }

        private string VerifyCreateIISWebsite(CreateIISWebsiteActivity activity, string host)
        {
            var status = string.Empty;
            try
            {
                var verifyScript = new ScriptWithParameters();
                verifyScript.Script = this.ActivityScriptMap.VerificationScript;
                var verifyCallScript = new ScriptWithParameters();
                verifyCallScript.Script = Constants.PowershellScripts.VerifyCreateIISWebsiteCall;
                verifyCallScript.Params = new List<object>();
                verifyCallScript.Params.Add(activity.WebsiteName);
                verifyCallScript.Params.Add(activity.Bindings);
                verifyCallScript.Params.Add(activity.PhysicalPath);
                verifyCallScript.Params.Add(activity.OverrideIfExists);
                var result = _shellManager.ExecuteCommands(host, new List<ScriptWithParameters> { verifyScript, verifyCallScript }, true);
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
