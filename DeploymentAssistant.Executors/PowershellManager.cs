using DeploymentAssistant.Common;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;


namespace DeploymentAssistant.Executors
{
    /// <summary>
    /// Manages all power shell related execution activities
    /// Uses powershell remoting and system.management.automation api
    /// </summary>
    public class PowershellManager
    {
        ILog logger = null;

        public PowershellManager()
        {
            logger = LogManager.GetLogger(this.GetType());
        }

        public void ExecutePowerShellCommand(string remoteComputer, List<string> commandScripts, bool throwEx = true)
        {
            var connectionUri = new Uri(String.Format("http://{0}:5985/wsman", remoteComputer));
            logger.InfoFormat("connection Uri: {0}.", connectionUri.ToString());
            var connection = new WSManConnectionInfo(connectionUri);
            logger.Info("Establishing connection.");
            connection.AuthenticationMechanism = AuthenticationMechanism.Default;
            var runspace = RunspaceFactory.CreateRunspace(connection);
            runspace.Open();
            logger.Info("Created RunSpace and opened it.");
            using (var powershell = PowerShell.Create())
            {
                powershell.Runspace = runspace;
                foreach (var script in commandScripts)
                {
                    powershell.AddScript(script);
                }
                var results = powershell.Invoke();
                logger.Info("Invoked the script(s).");
                RaisePowerShellExecutionError(powershell);
                runspace.Close();
                logger.Info("Closed RunSpace.");
            }
        }

        private void RaisePowerShellExecutionError(PowerShell powershell, bool throwEx = true)
        {
            if (powershell.Streams.Error.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var error in powershell.Streams.Error)
                {
                    builder.AppendLine(error.ToString());
                }
                var errorMessage = string.Format("PowerShell ScriptExecution Error: {0}", builder.ToString());
                if (throwEx)
                {
                    throw new ApplicationException(errorMessage);
                }
                else
                {
                    logger.Info(errorMessage);
                }
            }
        }
    }
}
