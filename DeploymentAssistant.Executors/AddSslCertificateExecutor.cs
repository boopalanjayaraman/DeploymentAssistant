using DeploymentAssistant.Common;
using DeploymentAssistant.Executors.Models;
using DeploymentAssistant.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
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
                addSslCertificateScript.Params = new Dictionary<string, object>();
                addSslCertificateScript.Params.Add("CertificateSharePath", activity.CertificateSharePath);
                addSslCertificateScript.Params.Add("CertificateThumbPrint", activity.CertificateThumbprint);
                addSslCertificateScript.Params.Add("pwd", activity.CertificatePassword);
                addSslCertificateScript.Params.Add("hostIp", activity.Host.HostName);
                addSslCertificateScript.Params.Add("websiteName", activity.WebsiteName);
                addSslCertificateScript.Params.Add("port", activity.Port);
                addSslCertificateScript.Params.Add("hostHeader", activity.HostHeader);
                addSslCertificateScript.Params.Add("bindingIp", activity.BindingIp);
                addSslCertificateScript.Params.Add("storeLocation", activity.StoreLocation);
                addSslCertificateScript.Params.Add("storeName", activity.StoreName);
                var response = _shellManager.ExecuteCommands(host, new List<ScriptWithParameters> { addSslCertificateScript }, true);
            }
            catch (RemoteException rEx)
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
                verifyScript.Params = new Dictionary<string, object>();
                verifyScript.Params.Add("CertificateSharePath", activity.CertificateSharePath);
                verifyScript.Params.Add("CertificateThumbPrint", activity.CertificateThumbprint);
                verifyScript.Params.Add("pwd", activity.CertificatePassword);
                verifyScript.Params.Add("hostIp", activity.Host.HostName);
                verifyScript.Params.Add("websiteName", activity.WebsiteName);
                verifyScript.Params.Add("port", activity.Port);
                verifyScript.Params.Add("hostHeader", activity.HostHeader);
                verifyScript.Params.Add("bindingIp", activity.BindingIp);
                verifyScript.Params.Add("storeLocation", activity.StoreLocation);
                verifyScript.Params.Add("storeName", activity.StoreName);
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
