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
    /// Add a SSL certificate to a IIS Web site on a given host - executor
    /// </summary>
    internal class AddSslCertificateExecutor : AbstractExecutor, IExecutor
    {
        public AddSslCertificateExecutor(ExecutionActivity activity, IShellManager shellManager)
            : base(activity, shellManager)
        {
            logger = LogManager.GetLogger(typeof(AddSslCertificateExecutor));
        }

        /// <summary>
        /// Execute step method
        /// </summary>
        public override void Execute()
        {
            logger.Info("Add Ssl certificate - Activity Execution Started.");
            var activity = this.Activity as AddSslCertificateActivity;
            var host = activity.Host.HostName;
            AddSslCertificate(activity, host);
            logger.Info("Activity Execution Finished.");
        }

        private void AddSslCertificate(AddSslCertificateActivity activity, string host)
        {
            try
            {
                var addSslCertificateScript = new ScriptWithParameters();
                addSslCertificateScript.Script = this.ActivityScriptMap.ExecutionScript;
                var addSslCertificateCallScript = new ScriptWithParameters();
                addSslCertificateCallScript.Script = Constants.PowershellScripts.AddSslCertificateCall;
                addSslCertificateCallScript.Params = new List<object>();
                addSslCertificateCallScript.Params.Add(activity.CertificateSharePath);
                addSslCertificateCallScript.Params.Add(activity.CertificateThumbprint);
                addSslCertificateCallScript.Params.Add(activity.CertificatePassword);
                addSslCertificateCallScript.Params.Add(activity.Host.HostName);
                addSslCertificateCallScript.Params.Add(activity.WebsiteName);
                addSslCertificateCallScript.Params.Add(activity.Port);
                addSslCertificateCallScript.Params.Add(activity.HostHeader);
                addSslCertificateCallScript.Params.Add(activity.BindingIp);
                addSslCertificateCallScript.Params.Add(activity.StoreLocation);
                addSslCertificateCallScript.Params.Add(activity.StoreName);
                var response = _shellManager.ExecuteCommands(host, new List<ScriptWithParameters> { addSslCertificateScript, addSslCertificateCallScript }, true);
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
            logger.Info("Add Ssl Certificate - Activity Verification Started.");
            if (quitExecuting)
            {
                logger.Info("Activity Verification skipped. QuitExecuting Flag is true.");
                this.Result = new ExecutionResult();
                return;
            }

            var activity = this.Activity as AddSslCertificateActivity;
            var host = activity.Host.HostName;
            var status = VerifyAddSslCertificate(activity, host);

            this.Result = new ExecutionResult() { IsSuccess = !status.Equals("0") };
            logger.InfoFormat("Verification Finished. Result: {0}", this.Result.ToJson());
        }

        private string VerifyAddSslCertificate(AddSslCertificateActivity activity, string host)
        {
            var status = string.Empty;
            try
            {
                var verifyScript = new ScriptWithParameters();
                verifyScript.Script = this.ActivityScriptMap.VerificationScript;
                var verifyCallScript = new ScriptWithParameters();
                verifyCallScript.Script = Constants.PowershellScripts.VerifyAddSslCertificateCall;
                verifyCallScript.Params = new List<object>();
                verifyCallScript.Params.Add(activity.CertificateSharePath);
                verifyCallScript.Params.Add(activity.CertificateThumbprint);
                verifyCallScript.Params.Add(activity.CertificatePassword);
                verifyCallScript.Params.Add(activity.Host.HostName);
                verifyCallScript.Params.Add(activity.WebsiteName);
                verifyCallScript.Params.Add(activity.Port);
                verifyCallScript.Params.Add(activity.HostHeader);
                verifyCallScript.Params.Add(activity.BindingIp);
                verifyCallScript.Params.Add(activity.StoreLocation);
                verifyCallScript.Params.Add(activity.StoreName);
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
