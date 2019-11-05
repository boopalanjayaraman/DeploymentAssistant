using DeploymentAssistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Executors
{
    /// <summary>
    /// Maps activity and script file(s) necessary to run the activity
    /// </summary>
    internal class ActivityScriptProvider
    {
        static Dictionary<ExecutionType, ActivityScriptMap> activityScriptMapDictionary = new Dictionary<ExecutionType, ActivityScriptMap>();

        public ActivityScriptProvider()
        {

        }

        static ActivityScriptProvider()
        {
            activityScriptMapDictionary.Add(ExecutionType.AddSslCertificate, new ActivityScriptMap(
                ExecutionType.AddSslCertificate,
                executionScriptFile: "AddSslCertificate.ps1",
                verificationScriptFile: "Verify_AddSslCertificate.ps1"
            ));
            activityScriptMapDictionary.Add(ExecutionType.CopyFiles, new ActivityScriptMap(
                ExecutionType.CopyFiles,
                executionScriptFile: "CopyFiles.ps1",
                verificationScriptFile: ""
            ));
            activityScriptMapDictionary.Add(ExecutionType.CreateIISWebsite, new ActivityScriptMap(
                ExecutionType.CreateIISWebsite,
                executionScriptFile: "CreateIISWebsite.ps1",
                verificationScriptFile: "Verify_CreateIISWebsite.ps1"
            ));
            activityScriptMapDictionary.Add(ExecutionType.DeleteFiles, new ActivityScriptMap(
                ExecutionType.DeleteFiles,
                executionScriptFile: "DeleteFiles.ps1",
                verificationScriptFile: "Verify_DeleteFiles.ps1"
            ));
            activityScriptMapDictionary.Add(ExecutionType.MoveFiles, new ActivityScriptMap(
                ExecutionType.MoveFiles,
                executionScriptFile: "MoveFiles.ps1",
                verificationScriptFile: "Verify_MoveFiles.ps1"
            ));
            activityScriptMapDictionary.Add(ExecutionType.StartIISWebServer, new ActivityScriptMap(
                ExecutionType.StartIISWebServer,
                executionScriptFile: "StartIISWebServer.ps1",
                verificationScriptFile: ""
            ));
            activityScriptMapDictionary.Add(ExecutionType.StartIISWebsite, new ActivityScriptMap(
                ExecutionType.StartIISWebsite,
                executionScriptFile: "StartIISWebsite.ps1",
                verificationScriptFile: "Verify_StartIISWebsite.ps1"
            ));
            activityScriptMapDictionary.Add(ExecutionType.StartService, new ActivityScriptMap(
                ExecutionType.StartService,
                executionScriptFile: "StartService.ps1",
                verificationScriptFile: "Verify_StartService.ps1"
            ));
            activityScriptMapDictionary.Add(ExecutionType.StopIISWebServer, new ActivityScriptMap(
                ExecutionType.StopIISWebServer,
                executionScriptFile: "StopIISWebServer.ps1",
                verificationScriptFile: ""
            ));
            activityScriptMapDictionary.Add(ExecutionType.StopIISWebsite, new ActivityScriptMap(
                ExecutionType.StopIISWebsite,
                executionScriptFile: "StopIISWebsite.ps1",
                verificationScriptFile: "Verify_StopIISWebsite.ps1"
            ));
            activityScriptMapDictionary.Add(ExecutionType.StopService, new ActivityScriptMap(
                ExecutionType.StopService,
                executionScriptFile: "StopService.ps1",
                verificationScriptFile: "Verify_StopService.ps1"
            ));
        }

        /// <summary>
        /// Gets the activity script map
        /// </summary>
        /// <param name="type">execution type</param>
        /// <returns>map object</returns>
        public static ActivityScriptMap GetActivityScriptMap(ExecutionType type)
        {
            if (activityScriptMapDictionary.ContainsKey(type))
            {
                return activityScriptMapDictionary[type];
            }
            else
            {
                return null;
            }
        }

    }

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
