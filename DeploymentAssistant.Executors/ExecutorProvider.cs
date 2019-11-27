using DeploymentAssistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Executors
{
    /// <summary>
    /// Interface for Executor Provider - To make it testable
    /// </summary>
    interface IExecutorProvider
    {
        AbstractExecutor GetExecutor(ExecutionActivity executionActivity);
    }

    /// <summary>
    /// Factory method provider
    /// </summary>
    internal class ExecutorProvider : IExecutorProvider
    {
        public AbstractExecutor GetExecutor(ExecutionActivity executionActivity)
        {
            var executionType = executionActivity.Operation;
            switch (executionType)
            {
                case ExecutionType.StartService:
                    return new StartServiceExecutor(executionActivity, new PowershellManager());

                case ExecutionType.StopService:
                    return new StopServiceExecutor(executionActivity, new PowershellManager());

                case ExecutionType.AddSslCertificate:
                    return new AddSslCertificateExecutor(executionActivity, new PowershellManager());

                case ExecutionType.CopyFiles:
                    return new CopyFilesExecutor(executionActivity, new PowershellManager());

                case ExecutionType.CreateIISWebsite:
                    return new CreateIISWebsiteExecutor(executionActivity, new PowershellManager());

                case ExecutionType.DeleteFiles:
                    return new DeleteFilesExecutor(executionActivity, new PowershellManager());

                case ExecutionType.MoveFiles:
                    return new MoveFilesExecutor(executionActivity, new PowershellManager());

                case ExecutionType.StartIISWebServer:
                    return new StartIISWebServerExecutor(executionActivity, new PowershellManager());

                case ExecutionType.StartIISWebsite:
                    return new StartIISWebsiteExecutor(executionActivity, new PowershellManager());

                case ExecutionType.StopIISWebServer:
                    return new StopIISWebServerExecutor(executionActivity, new PowershellManager());

                case ExecutionType.StopIISWebsite:
                    return new StopIISWebsiteExecutor(executionActivity, new PowershellManager());

                case ExecutionType.GitClone:
                    return new GitCloneExecutor(executionActivity, new PowershellManager());

                case ExecutionType.SvnCheckout:
                    return new SvnCheckoutExecutor(executionActivity, new PowershellManager());

                case ExecutionType.MsBuild:
                    return new MsBuildExecutor(executionActivity, new PowershellManager());

                default:
                    throw new NotImplementedException("No executors were found for this type.");
            }
        }        
    }
}
