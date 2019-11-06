using DeploymentAssistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Executors
{
    /// <summary>
    /// Data model for activity and script mapping
    /// </summary>
    internal class ActivityScriptMap
    {
        /// <summary>
        /// Activity type
        /// </summary>
        public ExecutionType ActivityType { get; private set; }

        /// <summary>
        /// Execution script file 
        /// </summary>
        public string ExecutionScriptFile { get; private set; }

        /// <summary>
        /// Verification script 
        /// </summary>
        public string VerificationScriptFile { get; private set; }

        /// <summary>
        /// Execution script  
        /// </summary>
        public string ExecutionScript { get; set; }

        /// <summary>
        /// Verification script 
        /// </summary>
        public string VerificationScript { get; set; }


        public ActivityScriptMap(ExecutionType executionType, string executionScriptFile, string verificationScriptFile = "", string executionScript = "", string verificationScript = "")
        {
            this.ActivityType = executionType;
            this.ExecutionScriptFile = executionScriptFile;
            this.VerificationScriptFile = verificationScriptFile;
            this.ExecutionScript = executionScript;
            this.VerificationScript = verificationScript;
        }
    }
}
