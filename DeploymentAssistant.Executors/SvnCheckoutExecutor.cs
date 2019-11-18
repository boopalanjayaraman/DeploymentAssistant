using DeploymentAssistant.Executors.Models;
using DeploymentAssistant.Models;
using DeploymentAssistant.Common;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Executors
{
    /// <summary>
    /// Does a Svn Checkout operation from given repo url 
    /// </summary>
    internal class SvnCheckoutExecutor : AbstractExecutor, IExecutor
    {
        public SvnCheckoutExecutor(ExecutionActivity activity, IShellManager shellManager)
            : base(activity, shellManager)
        {
            logger = LogManager.GetLogger(typeof(SvnCheckoutExecutor));
        }

        /// <summary>
        /// Execute step method
        /// </summary>
        public override void Execute()
        {
            logger.Info("Svn Checkout - Activity Execution Started.");
            var activity = this.Activity as SvnCheckoutActivity;
            var host = activity.Host.HostName;
            //// move the files
            SvnCheckout(activity, host);
            logger.Info("Activity Execution Finished.");
        }

        private void SvnCheckout(SvnCheckoutActivity activity, string host)
        {
            try
            {
                var svnCheckoutScript = new ScriptWithParameters();
                svnCheckoutScript.Script = this.ActivityScriptMap.ExecutionScript;
                svnCheckoutScript.Params = new Dictionary<string, object>();
                svnCheckoutScript.Params.Add("localDestinationPath", activity.LocalDestinationPath);
                svnCheckoutScript.Params.Add("repoUrl", activity.RepoUrl);
                svnCheckoutScript.Params.Add("userName", activity.UserName);
                svnCheckoutScript.Params.Add("password", activity.Password);
                svnCheckoutScript.Params.Add("useCheckoutOrUpdate", activity.UseCheckoutOrUpdate);
                var response = _shellManager.ExecuteCommands(host, new List<ScriptWithParameters> { svnCheckoutScript }, true);
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
            logger.Info("Svn Checkout - Activity Verification Started.");
            if (quitExecuting)
            {
                logger.Info("Activity Verification skipped. QuitExecuting Flag is true.");
                this.Result = new ExecutionResult();
                return;
            }

            var activity = this.Activity as SvnCheckoutActivity;
            var host = activity.Host.HostName;
            var status = VerifySvnCheckout(activity, host);

            this.Result = new ExecutionResult() { IsSuccess = !status.Equals("0") };
            logger.InfoFormat("Verification Finished. Result: {0}", this.Result.ToJson());
        }

        private string VerifySvnCheckout(SvnCheckoutActivity activity, string host)
        {
            var status = string.Empty;
            try
            {
                var verifyScript = new ScriptWithParameters();
                verifyScript.Script = this.ActivityScriptMap.VerificationScript;
                verifyScript.Params = new Dictionary<string, object>();
                verifyScript.Params.Add("localDestinationPath", activity.LocalDestinationPath);
                verifyScript.Params.Add("repoUrl", activity.RepoUrl);
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
