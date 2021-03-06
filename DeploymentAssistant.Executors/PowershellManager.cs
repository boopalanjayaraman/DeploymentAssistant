﻿using DeploymentAssistant.Common;
using DeploymentAssistant.Executors.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;


namespace DeploymentAssistant.Executors
{
    /// <summary>
    /// Interface for shell manager
    /// </summary>
    internal interface IShellManager
    {
        void ExecuteCommands(string remoteComputer, List<string> commandScripts, bool throwEx = true);

        string GetValue(string remoteComputer, List<string> commandScripts, bool throwEx = true);

        Collection<object> GetObjects(string remoteComputer, params string[] commandScripts);

        Collection<object> ExecuteCommands(string remoteComputer, List<ScriptWithParameters> commandScripts, bool throwEx = true);
    }

    /// <summary>
    /// Manages all power shell related execution activities
    /// Uses powershell remoting and system.management.automation api
    /// </summary>
    internal class PowershellManager : IShellManager
    {
        ILog logger = null;

        public PowershellManager()
        {
            logger = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Executes a powershell command or set of commands
        /// </summary>
        /// <param name="remoteComputer">remote computer name or ip</param>
        /// <param name="commandScripts">list of strings containing powershell scripts</param>
        /// <param name="throwEx">if false, any exception will be just logged and not thrown</param>
        public void ExecuteCommands(string remoteComputer, List<string> commandScripts, bool throwEx = true)
        {
            Runspace runspace = GetRunspace(remoteComputer);
            runspace.Open();
            using (var powershell = PowerShell.Create())
            {
                logger.Info("Adding scripts to the runspace.");
                powershell.Runspace = runspace;
                foreach (var script in commandScripts)
                {
                    powershell.AddScript(script);
                }
                //// Invoke the script
                var results = powershell.Invoke();
                //// Collect the output information
                logger.Info("Invoked the script(s).");
                ThrowPSExecutionError(powershell);
                runspace.Close();
            }
        }

        private Runspace GetRunspace(string remoteComputer)
        {
            var connectionUri = new Uri(String.Format("http://{0}:5985/wsman", remoteComputer));
            logger.InfoFormat("connection Uri: {0}. Establishing Connection.", connectionUri.ToString());
            var connection = new WSManConnectionInfo(connectionUri);
            connection.AuthenticationMechanism = AuthenticationMechanism.Default;
            connection.EnableNetworkAccess = true;
            var runspace = RunspaceFactory.CreateRunspace(connection);
            return runspace;
        }

        private void ThrowPSExecutionError(PowerShell powershell, bool throwEx = true)
        {
            //// Check if there are error(s), and if so, prepare to throw it
            if (powershell.Streams.Error.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var error in powershell.Streams.Error)
                {
                    builder.AppendLine(error.ToString());
                }
                //// Include any debug information as well for diagnosing purposes.
                if(powershell.Streams.Debug.Count > 0)
                {
                    builder.AppendLine("Debug Information.");
                }
                foreach(var debugInfo in powershell.Streams.Debug)
                {
                    builder.AppendLine(debugInfo.ToString());
                }
                //// Include verbose information too for diagnosing purposes.
                if (powershell.Streams.Verbose.Count > 0)
                {
                    builder.AppendLine("Verbose Information.");
                }
                foreach (var verbose in powershell.Streams.Verbose)
                {
                    builder.AppendLine(verbose.ToString());
                }
                //// throw the exception
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

        /// <summary>
        /// Executes a powershell command and returns the output value as string
        /// </summary>
        /// <param name="remoteComputer">remote host name or ip</param>
        /// <param name="commandScripts">powershell script</param>
        /// <param name="throwEx">if false, any exception will be just logged and not thrown</param>
        /// <returns></returns>
        public string GetValue(string remoteComputer, List<string> commandScripts, bool throwEx = true)
        {
            Runspace runspace = GetRunspace(remoteComputer);
            runspace.Open();
            var response = string.Empty;
            using (var powershell = PowerShell.Create())
            {
                logger.Info("Adding scripts to the runspace.");
                powershell.Runspace = runspace;
                foreach (var commandScript in commandScripts)
                {
                    powershell.AddScript(commandScript);
                }
                //// Invoke the script
                var results = powershell.Invoke();
                //// Collect the output information
                response = results[0] != null ? results[0].ToString() : string.Empty;
                logger.Info("Invoked the scripts. Response received.");
                ThrowPSExecutionError(powershell, throwEx);
                runspace.Close();
            }

            return response;
        }

        /// <summary>
        /// Executes a powershell command and expects the output to be in the form of collection of objects. Return those objects.
        /// </summary>
        /// <param name="remoteComputer">remote computer host name or ip</param>
        /// <param name="commandScripts">powershell command script</param>
        /// <returns></returns>
        public Collection<object> GetObjects(string remoteComputer, params string[] commandScripts)
        {
            Runspace runspace = GetRunspace(remoteComputer);
            runspace.Open();
            Collection<object> response = new Collection<object>();
            using (var powershell = PowerShell.Create())
            {
                logger.Info("Adding scripts to the runspace.");
                powershell.Runspace = runspace;
                foreach (var commandScript in commandScripts)
                {
                    powershell.AddScript(commandScript);
                }
                //// Invoke the script
                var results = powershell.Invoke();
                //// Collect the output information
                results.ToList().ForEach(pso =>
                {
                    response.Add(pso);
                });
                logger.Info("Invoked the scripts. Response received.");
                ThrowPSExecutionError(powershell, false);
                runspace.Close();
            }

            return response;
        }

        /// <summary>
        /// This version of execute commands takes in scripts and parameters for them respectively 
        /// Executes the scripts and returns the output
        /// </summary>
        /// <param name="remoteComputer">remote host name in which script will be executed</param>
        /// <param name="commandScripts">scripts list</param>
        /// <param name="throwEx">when true, any error will be thrown</param>
        /// <returns>the output, collection of objects from PSObjects </returns>
        public Collection<object> ExecuteCommands(string remoteComputer, List<ScriptWithParameters> commandScripts, bool throwEx = true)
        {
            Runspace runspace = GetRunspace(remoteComputer);
            runspace.Open();
            Collection<object> response = new Collection<object>();
            using (var powershell = PowerShell.Create())
            {
                logger.Info("Adding scripts to the runspace.");
                powershell.Runspace = runspace;
                foreach (var script in commandScripts)
                {
                    if (script.IsCommand)
                    {
                        powershell.AddCommand(script.Script);
                    }
                    else
                    {
                        powershell.AddScript(script.Script);
                    }
                    if ((script.Params != null) && (script.Params.Count > 0))
                    {
                        foreach (var param in script.Params)
                        {
                            powershell.AddParameter(param.Key, param.Value);
                        }
                    }
                }
                //// Invoke the script
                var results = powershell.Invoke();
                //// Collect the output information
                results.ToList().ForEach(pso =>
                {
                    response.Add(pso.BaseObject);
                });
                logger.Info("Invoked the script(s).");
                ThrowPSExecutionError(powershell);
                runspace.Close();
            }
            return response;
        }
    }
}
