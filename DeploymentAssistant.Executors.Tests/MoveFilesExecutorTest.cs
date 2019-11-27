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
    public class MoveFilesExecutorTest
    {
        [TestMethod]
        public void MoveFilesExecutor_Constructor_Sets_ActivityScriptAndVerificationScript()
        {
            //// Arrange
            var activity = GetMoveFilesActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);

            //// Act
            var executor = new MoveFilesExecutor(activity, shellManagerMock.Object);

            //// Assert
            shellManagerMock.Verify();
            Assert.IsNotNull(executor.ActivityScriptMap);
            Assert.IsFalse(string.IsNullOrWhiteSpace(executor.ActivityScriptMap.ExecutionScript));
            Assert.IsFalse(string.IsNullOrWhiteSpace(executor.ActivityScriptMap.VerificationScript));
        }

        [TestMethod]
        public void MoveFilesExecutor_Execute_CallsShellManager_WithGivenParameters()
        {
            //// Arrange
            var activity = GetMoveFilesActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            var outputDestPath = "E:\\SomeHost\\WebFolder\\Backups1122";
            var outputHashtable = new Hashtable() { {"Count", 1}, {"DestinationPath", outputDestPath} };
            shellManagerMock.Setup(sm => sm.ExecuteCommands(
                                            It.Is<String>(s => IsSameHost(s, activity.Host.HostName)),
                                            It.Is<List<ScriptWithParameters>>(paramList => ContainsParamsFromActivity(paramList.First(), (MoveFilesActivity)activity)),
                                            true)).Returns(new Collection<object>() { outputHashtable });
            var executor = new MoveFilesExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Execute();

            //// Assert
            shellManagerMock.Verify();
            Assert.IsTrue((activity as MoveFilesActivity).DestinationPath.Equals(outputDestPath, StringComparison.CurrentCultureIgnoreCase));
        }

        [TestMethod]
        public void MoveFilesExecutor_Execute_Sets_IsSuccessToFalse_DuringException()
        {
            //// Arrange
            var activity = GetMoveFilesActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            shellManagerMock.Setup(sm => sm.ExecuteCommands(
                                            It.Is<String>(s => IsSameHost(s, activity.Host.HostName)),
                                            It.Is<List<ScriptWithParameters>>(paramList => ContainsParamsFromActivity(paramList.First(), (MoveFilesActivity)activity)),
                                            true)).Throws(new ApplicationException("Sample exception"));
            var executor = new MoveFilesExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Execute();

            //// Assert
            shellManagerMock.Verify();
            Assert.IsFalse(executor.Result.IsSuccess);
        }

        private bool ContainsParamsFromActivity(ScriptWithParameters scriptWithParams, MoveFilesActivity activity)
        {
            var parameters = scriptWithParams.Params;

            return
                activity.DestinationPath.Equals(parameters["destinationPath"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && activity.SourcePath.Equals(parameters["sourcePath"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && activity.AddTimeStampForFolder == (bool)parameters["addTimeStampForFolder"]
                && parameters.Count == 3;
        }

        private bool VerifyContainsParamsFromActivity(ScriptWithParameters scriptWithParams, MoveFilesActivity activity)
        {
            var parameters = scriptWithParams.Params;

            return
                activity.DestinationPath.Equals(parameters["destinationPath"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && activity.SourcePath.Equals(parameters["sourcePath"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && !string.IsNullOrWhiteSpace(scriptWithParams.Script)
                && parameters.Count == 2;
        }

        private bool IsSameHost(string s, string hostName)
        {
            return s.Equals(hostName, StringComparison.CurrentCultureIgnoreCase);
        }

        private ExecutionActivity GetMoveFilesActivity()
        {
            return new MoveFilesActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "SomeHost" },
                Name = "Backup deployment Files",
                Order = 4,
                DestinationPath = @"\\SomeHost\WebFolder\Backups",
                SourcePath = @"\\SomeHost\WebFolder",
                AddTimeStampForFolder = true
            };
        }

        [TestMethod]
        public void MoveFilesExecutor_Verify_CallsShellManager_WithGivenParameters()
        {
            //// Arrange
            var activity = GetMoveFilesActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            shellManagerMock.Setup(sm => sm.ExecuteCommands(
                                            It.Is<String>(s => IsSameHost(s, activity.Host.HostName)),
                                            It.Is<List<ScriptWithParameters>>(paramList => VerifyContainsParamsFromActivity(paramList.First(), (MoveFilesActivity)activity)),
                                            true)).Returns(new Collection<object>());
            var executor = new MoveFilesExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Verify();

            //// Assert
            shellManagerMock.Verify();
        }
    }
}
