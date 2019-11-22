using DeploymentAssistant.Common;
using DeploymentAssistant.Models;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant
{
    /// <summary>
    /// A helper class that serializes the activities into a configuration json file.
    /// Just to make configuring task easier.
    /// </summary>
    public class ConfigMaker
    {
        List<ActivityConfigEntry> activityEntries = new List<ActivityConfigEntry>();
        ILog logger = LogManager.GetLogger(typeof(ConfigMaker));

        public ConfigMaker()
        {
            activityEntries.Add(new ActivityConfigEntry(ExecutionType.StopService.ToString(), new StopServiceActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "PTPLL695" },
                Name = "MongoDb Service Stop",
                ServiceName = "MongoDB",
                Order = 1
            }));
            activityEntries.Add(new ActivityConfigEntry(ExecutionType.GitClone.ToString(), new GitCloneActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "PTPLL258" },
                Name = "Clone JoinUs",
                Order = 7,
                RepoUrl = "https://gitlab.company.com/BlackPearl/JoinUs.git",
                UseCloneOrPull = false,
                LocalDestinationPath = "E:\\C\\TestProjects\\git_ps_test"
            }));
            activityEntries.Add(new ActivityConfigEntry(ExecutionType.MsBuild.ToString(), new MsBuildActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "PTPLL258" },
                Name = "Build JoinUs",
                Order = 7,
                LocalMsBuildPath = "C:\\Program Files (x86)\\MSBuild\\12.0\\Bin\\MSBuild.exe",
                SolutionPath = "E:\\C\\TestProjects\\git_ps\\JoinUs\\Payoda.JoinUs\\Payoda.JoinUs.sln",
                BuildTargets = "Build",
                BuildProperties = "Configuration=Release"
            }));
            activityEntries.Add(new ActivityConfigEntry(ExecutionType.CopyFiles.ToString(), new CopyFilesActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "PTPLL258" },
                Name = "Copy HeapImplementation Files",
                Order = 3,
                SourcePath = @"E:\TestDepAsst",
                DestinationPath = @"\\PTPLL695\BooFolder",
                ExcludeExtensions = new List<string>() { ".cs", ".pdb" },
                SkipFolders = new List<string>() { @"\obj" }
            }));
            activityEntries.Add(new ActivityConfigEntry(ExecutionType.CopyFiles.ToString(), new CopyFilesActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "PTPLL258" },
                Name = "Copy HeapImplementation Files",
                Order = 3,
                SourcePath = @"E:\TestDepAsst",
                DestinationPath = @"\\PTPLL695\BooFolder",
                ExcludeExtensions = new List<string>() { ".cs", ".pdb" },
                SkipFolders = new List<string>() { @"\obj" }
            }));
            activityEntries.Add(new ActivityConfigEntry(ExecutionType.CreateIISWebsite.ToString(), new CreateIISWebsiteActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "PTPLL695" },
                Name = "Create WebSite 695",
                Order = 6,
                WebsiteName = "TestPsWebDD",
                PhysicalPath = "D:\\BooFolderNotShared2",
                Bindings = new Hashtable() { { "http", "*:8087:" } },
                OverrideIfExists = true
            }));
            activityEntries.Add(new ActivityConfigEntry(ExecutionType.StopIISWebsite.ToString(), new StopIISWebsiteActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "PTPLL695" },
                Name = "Stop WebSite 695",
                Order = 6,
                WebsiteName = "TestPsWebDD"
            }));
            activityEntries.Add(new ActivityConfigEntry(ExecutionType.AddSslCertificate.ToString(), new AddSslCertificateActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "PTPLL695" },
                Name = "Create WebSite 695",
                Order = 6,
                WebsiteName = "TestPsWebDD",
                CertificateSharePath = "D:\\BooCertFolder\\certTestPsWebDd.pfx",
                CertificateThumbprint = "‎0705f4bedd2913afd6ce7d7312c0ac4c38d0bff6",
                BindingIp = "",
                CertificatePassword = "cert@1234",
                Port = "10443",
                StoreName = "WebHosting"
            }));
            activityEntries.Add(new ActivityConfigEntry(ExecutionType.StartIISWebsite.ToString(), new StartIISWebsiteActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "PTPLL695" },
                Name = "Start WebSite 695",
                Order = 6,
                WebsiteName = "TestPsWebDD"
            }));
            activityEntries.Add(new ActivityConfigEntry(ExecutionType.StopIISWebServer.ToString(), new StopIISWebServerActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "PTPLL695" },
                Name = "Stop IIS",
                Order = 2
            }));
            activityEntries.Add(new ActivityConfigEntry(ExecutionType.StartIISWebServer.ToString(), new StartIISWebServerActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "PTPLL695" },
                Name = "Start IIS",
                Order = 2
            }));
            activityEntries.Add(new ActivityConfigEntry(ExecutionType.StartService.ToString(), new StartServiceActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "PTPLL695" },
                Name = "MongoDb Service Start",
                ServiceName = "MongoDB",
                Order = 2
            }));

            /*activityEntries.Add(new ActivityConfigEntry(ExecutionType.StopService.ToString(), new StopServiceActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "PTPLL695" },
                Name = "MongoDb Service Stop",
                ServiceName = "MongoDB",
                Order = 1
            }));*/
            /*activityEntries.Add(new ActivityConfigEntry(ExecutionType.StartService.ToString(), new StartServiceActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "PTPLL695" },
                Name = "MongoDb Service Start",
                ServiceName = "MongoDB",
                Order = 2
            }));*/
            /*activityEntries.Add(new ActivityConfigEntry(ExecutionType.CopyFiles.ToString(), new CopyFilesActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "PTPLL258" },
                Name = "Copy HeapImplementation Files",
                Order = 3,
                SourcePath = @"E:\TestDepAsst",
                DestinationPath = @"\\PTPLL695\BooFolder",
                ExcludeExtensions = new List<string>() { ".cs", ".pdb" },
                SkipFolders = new List<string>() { @"\obj" }
            }));*/
            /*activityEntries.Add(new ActivityConfigEntry(ExecutionType.MoveFiles.ToString(), new MoveFilesActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "PTPLL695" },
                Name = "Move HeapImplementation Files",
                Order = 3,
                SourcePath = @"\\ptpll258\e$\TestDepAsst",
                DestinationPath = @"\\ptpll695\d$\BooFolderNotShared",
            }));*/
            /*activityEntries.Add(new ActivityConfigEntry(ExecutionType.DeleteFiles.ToString(), new DeleteFilesActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "PTPLL258" },
                Name = "Delete old Files",
                Order = 4,
                DestinationPath = @"\\PTPLL686\d$\BooFolder",
            }));*/
            /*activityEntries.Add(new ActivityConfigEntry(ExecutionType.CreateIISWebsite.ToString(), new CreateIISWebsiteActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "PTPLL695" },
                Name = "Create WebSite 695",
                Order = 6,
                WebsiteName = "TestPsWebDD",
                PhysicalPath = "D:\\BooFolderNotShared2",
                Bindings = new Hashtable() { { "http", "*:8087:" }},
                OverrideIfExists = true
            }));*/
            /*activityEntries.Add(new ActivityConfigEntry(ExecutionType.MsBuild.ToString(), new MsBuildActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "PTPLL258" },
                Name = "Build JoinUs",
                Order = 7,
                LocalMsBuildPath = "C:\\Program Files (x86)\\MSBuild\\12.0\\Bin\\MSBuild.exe",
                SolutionPath = "E:\\C\\TestProjects\\git_ps\\JoinUs\\Payoda.JoinUs\\Payoda.JoinUs.sln",
                BuildTargets = "Build",
                BuildProperties = "Configuration=Release"
            }));*/
            /*activityEntries.Add(new ActivityConfigEntry(ExecutionType.GitClone.ToString(), new GitCloneActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "PTPLL258" },
                Name = "Clone JoinUs",
                Order = 7,
                RepoUrl = "https://gitlab.company.com/BlackPearl/JoinUs.git",
                UseCloneOrPull = false,
                LocalDestinationPath = "E:\\C\\TestProjects\\git_ps_test"
            }));*/
            /*activityEntries.Add(new ActivityConfigEntry(ExecutionType.SvnCheckout.ToString(), new SvnCheckoutActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "PTPLL258" },
                Name = "Clone TARG svn",
                Order = 8,
                RepoUrl = "https://svn.company.com/svn/REPO/",
                UserName = "",
                Password = "",
                UseCheckoutOrUpdate = true,
                LocalDestinationPath = "E:\\C\\TestProjects\\svn_ps_test1"
            }));*/
            /*activityEntries.Add(new ActivityConfigEntry(ExecutionType.AddSslCertificate.ToString(), new AddSslCertificateActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "PTPLL695" },
                Name = "Create WebSite 695",
                Order = 6,
                WebsiteName = "TestPsWebDD",
                CertificateSharePath = "D:\\BooCertFolder\\certTestPsWebDd.pfx",
                CertificateThumbprint = "‎0705f4bedd2913afd6ce7d7312c0ac4c38d0bff6",
                BindingIp = "",
                CertificatePassword = "cert@1234",
                Port = "10443",
                StoreName = "WebHosting"
            }));*/


            logger.Info("Configuration Entries are initialized");
        }

        /// <summary>
        /// Dumps the activities list into a json file.
        /// Backs up old files if any in the same name.
        /// </summary>
        public void Dump(string activityConfigurationFile = "")
        {
            logger.Info("Starting config dump operation.");
            if (string.IsNullOrWhiteSpace(activityConfigurationFile))
            {
                activityConfigurationFile = ConfigurationParser.GetExecutorsConfigFile();
            }
            //// If file name does not end with json extension, make it one.
            if (!activityConfigurationFile.EndsWith(".json", StringComparison.CurrentCultureIgnoreCase))
            {
                activityConfigurationFile += ".json";
            }
            //// back up old file if anything is available in the same name
            string activitiesJson = activityEntries.ToJson();
            if (File.Exists(activityConfigurationFile))
            {
                var timestamp = DateTime.Now.ToString("yyyyMMdd_hhmmss");
                var newFileName = string.Format("{0}_{1}", timestamp, activityConfigurationFile);
                File.Copy(activityConfigurationFile, newFileName);
                logger.Info("Backed up old file in a different name.");
            }
            File.WriteAllText(activityConfigurationFile, activitiesJson);
            logger.Info("Finished dump operation.");
        }
    }
}
