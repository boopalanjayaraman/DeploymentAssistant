using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeploymentAssistant.Models;
using Moq;
using DeploymentAssistant.Executors.Models;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections;

namespace DeploymentAssistant.Executors.Tests
{
    [TestClass]
    public class CopyFilesExecutorTest
    {
        [TestMethod]
        public void CopyFilesExecutor_Constructor_Sets_ActivityScriptAndVerificationScript()
        {
            //// Arrange
            var activity = GetCopyFilesActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);

            //// Act
            var executor = new CopyFilesExecutor(activity, shellManagerMock.Object);

            //// Assert
            shellManagerMock.Verify();
            Assert.IsNotNull(executor.ActivityScriptMap);
            Assert.IsFalse(string.IsNullOrWhiteSpace(executor.ActivityScriptMap.ExecutionScript));
            Assert.IsFalse(string.IsNullOrWhiteSpace(executor.ActivityScriptMap.VerificationScript));
        }

        [TestMethod]
        public void CopyFilesExecutor_Execute_CallsShellManager_WithGivenParameters()
        {
            //// Arrange
            var activity = GetCopyFilesActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            var outputDestPath = "E:\\SomeHost\\WebFolder\\Backups1122";
            var outputHashtable = new Hashtable() { { "Count", 1 }, { "DestinationPath", outputDestPath } };
            shellManagerMock.Setup(sm => sm.ExecuteCommands(
                                            It.Is<String>(s => IsSameHost(s, activity.Host.HostName)),
                                            It.Is<List<ScriptWithParameters>>(paramList => ContainsParamsFromActivity(paramList.First(), (CopyFilesActivity)activity)),
                                            true)).Returns(new Collection<object>() { outputHashtable });
            var executor = new CopyFilesExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Execute();

            //// Assert
            shellManagerMock.Verify();
            Assert.IsTrue((activity as CopyFilesActivity).DestinationPath.Equals(outputDestPath, StringComparison.CurrentCultureIgnoreCase));
        }

        [TestMethod]
        public void CopyFilesExecutor_Execute_Sets_IsSuccessToFalse_DuringException()
        {
            //// Arrange
            var activity = GetCopyFilesActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            shellManagerMock.Setup(sm => sm.ExecuteCommands(
                                            It.Is<String>(s => IsSameHost(s, activity.Host.HostName)),
                                            It.Is<List<ScriptWithParameters>>(paramList => ContainsParamsFromActivity(paramList.First(), (CopyFilesActivity)activity)),
                                            true)).Throws(new ApplicationException("Sample exception"));
            var executor = new CopyFilesExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Execute();

            //// Assert
            shellManagerMock.Verify();
            Assert.IsFalse(executor.Result.IsSuccess);
        }

        private bool ContainsParamsFromActivity(ScriptWithParameters scriptWithParams, CopyFilesActivity copyFilesActivity)
        {
            var parameters = scriptWithParams.Params;
            var skipFoldersIfExistParam = parameters["skipFoldersIfExist"] as string[];
            var skipFoldersParam = parameters["skipFolders"] as string[];
            var excludeExtensionsParam = parameters["excludeExtensions"] as string[];

            return
                copyFilesActivity.SourcePath.Equals(parameters["sourcePath"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && copyFilesActivity.SkipFoldersIfExist.All(f => skipFoldersIfExistParam.Contains(f))
                && copyFilesActivity.SkipFolders.All(f => skipFoldersParam.Contains(f))
                && copyFilesActivity.ExcludeExtensions.All(f => excludeExtensionsParam.Contains(f))
                && copyFilesActivity.DestinationPath.Equals(parameters["destinationPath"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && copyFilesActivity.AddTimeStampForFolder == (bool)parameters["addTimeStampForFolder"]
                && !string.IsNullOrWhiteSpace(scriptWithParams.Script)
                && parameters.Count == 6;
        }

        private bool VerifyContainsParamsFromActivity(ScriptWithParameters scriptWithParams, CopyFilesActivity copyFilesActivity)
        {
            var parameters = scriptWithParams.Params;
            return copyFilesActivity.DestinationPath.Equals(parameters["destinationPath"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && !string.IsNullOrWhiteSpace(scriptWithParams.Script)
                && parameters.Count == 1;
        }

        private bool IsSameHost(string s, string hostName)
        {
            return s.Equals(hostName, StringComparison.CurrentCultureIgnoreCase);
        }

        private ExecutionActivity GetCopyFilesActivity()
        {
            return new CopyFilesActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "SomeHost" },
                Name = "Copy Web Project Files",
                Order = 4,
                SourcePath = @"E:\BuildFiles",
                DestinationPath = @"\\SomeHost\WebFolder",
                ExcludeExtensions = new List<string>() { ".cs", ".pdb", ".csproj" },
                SkipFolders = new List<string>() { @"\obj" },
                SkipFoldersIfExist = new List<string>() { @"\Scripts", @"\Styles" },
                AddTimeStampForFolder = true
            };
        }

        [TestMethod]
        public void CopyFilesExecutor_Verify_CallsShellManager_WithGivenParameters()
        {
            //// Arrange
            var activity = GetCopyFilesActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            shellManagerMock.Setup(sm => sm.ExecuteCommands(
                                            It.Is<String>(s => IsSameHost(s, activity.Host.HostName)),
                                            It.Is<List<ScriptWithParameters>>(paramList => VerifyContainsParamsFromActivity(paramList.First(), (CopyFilesActivity)activity)),
                                            true)).Returns(new Collection<object>());
            var executor = new CopyFilesExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Verify();

            //// Assert
            shellManagerMock.Verify();
        }
    }
}
