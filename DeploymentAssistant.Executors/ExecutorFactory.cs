using DeploymentAssistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Executors
{
    /// <summary>
    /// Factory method provider
    /// </summary>
    internal static class ExecutorProvider
    {
        internal static AbstractExecutor GetExecutor(ExecutionActivity executionActivity)
        {
            var executionType = executionActivity.Operation;
            switch (executionType)
            {
                case ExecutionType.StartService:
                    return new StartServiceExecutor(executionActivity, new PowershellManager());
                case ExecutionType.StopService:
                    return new StopServiceExecutor(executionActivity, new PowershellManager());
                case ExecutionType.AddSslCertificate:
                case ExecutionType.CopyFiles:
                case ExecutionType.CreateIISWebsite:
                case ExecutionType.DeleteFiles:
                case ExecutionType.MoveFiles:
                case ExecutionType.StartIISWebServer:
                case ExecutionType.StartIISWebsite:
                case ExecutionType.StopIISWebServer:
                case ExecutionType.StopIISWebsite:
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
