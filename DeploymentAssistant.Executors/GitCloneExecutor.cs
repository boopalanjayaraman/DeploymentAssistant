using DeploymentAssistant.Executors.Models;
using DeploymentAssistant.Models;
using DeploymentAssistant.Common;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;

namespace DeploymentAssistant.Executors
{
    /// <summary>
    /// Does a git clone operation from given repo url 
    /// </summary>
    internal class GitCloneExecutor : AbstractExecutor, IExecutor
    {
        public GitCloneExecutor(ExecutionActivity activity, IShellManager shellManager)
            : base(activity, shellManager)
        {
            logger = LogManager.GetLogger(typeof(GitCloneExecutor));
        }

        /// <summary>
        /// Execute step method
        /// </summary>
        public override void Execute()
        {
            logger.Info("Git Clone - Activity Execution Started.");
            var activity = this.Activity as GitCloneActivity;
            var host = activity.Host.HostName;
            //// move the files
            GitClone(activity, host);
            logger.Info("Activity Execution Finished.");
        }

        private void GitClone(GitCloneActivity activity, string host)
        {
            try
            {
                var gitCloneScript = new ScriptWithParameters();
                gitCloneScript.Script = this.ActivityScriptMap.ExecutionScript;
                gitCloneScript.Params = new Dictionary<string, object>();
                gitCloneScript.Params.Add("localDestinationPath", activity.LocalDestinationPath);
                gitCloneScript.Params.Add("repoUrl", activity.RepoUrl);
                gitCloneScript.Params.Add("useCloneOrPull", activity.UseCloneOrPull);
                var response = _shellManager.ExecuteCommands(host, new List<ScriptWithParameters> { gitCloneScript }, true);
            }
            catch(RemoteException rEx)
            {
                logger.Error(rEx.Message);
                HandleException(rEx, activity);
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
            logger.Info("Git Clone - Activity Verification Started.");
            if (quitExecuting)
            {
                logger.Info("Activity Verification skipped. QuitExecuting Flag is true.");
                this.Result = new ExecutionResult();
                return;
            }

            var activity = this.Activity as GitCloneActivity;
            var host = activity.Host.HostName;
            var status = VerifyGitClone(activity, host);

            this.Result = new ExecutionResult() { IsSuccess = !status.Equals("0") };
            logger.InfoFormat("Verification Finished. Result: {0}", this.Result.ToJson());
        }

        private string VerifyGitClone(GitCloneActivity activity, string host)
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
