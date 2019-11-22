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
    /// Does a Svn Checkout operation from given repo url 
    /// </summary>
    internal class MsBuildExecutor : AbstractExecutor, IExecutor
    {
        public MsBuildExecutor(ExecutionActivity activity, IShellManager shellManager)
            : base(activity, shellManager)
        {
            logger = LogManager.GetLogger(typeof(MsBuildExecutor));
        }

        /// <summary>
        /// Execute step method
        /// </summary>
        public override void Execute()
        {
            logger.Info("MsBuild - Activity Execution Started.");
            var activity = this.Activity as MsBuildActivity;
            var host = activity.Host.HostName;
            //// move the files
            MsBuild(activity, host);
            logger.Info("Activity Execution Finished.");
        }

        private void MsBuild(MsBuildActivity activity, string host)
        {
            try
            {
                var msBuildScript = new ScriptWithParameters();
                msBuildScript.Script = this.ActivityScriptMap.ExecutionScript;
                msBuildScript.Params = new Dictionary<string, object>();
                msBuildScript.Params.Add("localMsBuildPath", activity.LocalMsBuildPath);
                msBuildScript.Params.Add("solutionPath", activity.SolutionPath);
                msBuildScript.Params.Add("buildTargets", activity.BuildTargets);
                msBuildScript.Params.Add("buildProperties", activity.BuildProperties);
                var response = _shellManager.ExecuteCommands(host, new List<ScriptWithParameters> { msBuildScript }, true);
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
            logger.Info("MsBuild - Activity Verification Started.");
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
