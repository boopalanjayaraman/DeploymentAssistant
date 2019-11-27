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
    public class MsBuildExecutorTest
    {
        [TestMethod]
        public void MsBuildExecutor_Constructor_Sets_ActivityScriptAndVerificationScript()
        {
            //// Arrange
            var activity = GetMsBuildActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);

            //// Act
            var executor = new MsBuildExecutor(activity, shellManagerMock.Object);

            //// Assert
            shellManagerMock.Verify();
            Assert.IsNotNull(executor.ActivityScriptMap);
            Assert.IsFalse(string.IsNullOrWhiteSpace(executor.ActivityScriptMap.ExecutionScript));
            Assert.IsTrue(string.IsNullOrWhiteSpace(executor.ActivityScriptMap.VerificationScript));
        }

        [TestMethod]
        public void MsBuildExecutor_Execute_CallsShellManager_WithGivenParameters()
        {
            //// Arrange
            var activity = GetMsBuildActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            shellManagerMock.Setup(sm => sm.ExecuteCommands(
                                            It.Is<String>(s => IsSameHost(s, activity.Host.HostName)),
                                            It.Is<List<ScriptWithParameters>>(paramList => ContainsParamsFromActivity(paramList.First(), (MsBuildActivity)activity)),
                                            true)).Returns(new Collection<object>());
            var executor = new MsBuildExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Execute();

            //// Assert
            shellManagerMock.Verify();
        }

        [TestMethod]
        public void MsBuildExecutor_Execute_Sets_IsSuccessToFalse_DuringException()
        {
            //// Arrange
            var activity = GetMsBuildActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            shellManagerMock.Setup(sm => sm.ExecuteCommands(
                                            It.Is<String>(s => IsSameHost(s, activity.Host.HostName)),
                                            It.Is<List<ScriptWithParameters>>(paramList => ContainsParamsFromActivity(paramList.First(), (MsBuildActivity)activity)),
                                            true)).Throws(new ApplicationException("Sample exception"));
            var executor = new MsBuildExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Execute();

            //// Assert
            shellManagerMock.Verify();
            Assert.IsFalse(executor.Result.IsSuccess);
        }

        private bool ContainsParamsFromActivity(ScriptWithParameters scriptWithParams, MsBuildActivity activity)
        {
            var parameters = scriptWithParams.Params;

            return
                activity.LocalMsBuildPath.Equals(parameters["localMsBuildPath"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && activity.SolutionPath.Equals(parameters["solutionPath"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && activity.BuildProperties.Equals(parameters["buildProperties"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && activity.BuildTargets.Equals(parameters["buildTargets"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && !string.IsNullOrWhiteSpace(scriptWithParams.Script)
                && parameters.Count == 4;
        }

        private bool IsSameHost(string s, string hostName)
        {
            return s.Equals(hostName, StringComparison.CurrentCultureIgnoreCase);
        }

        private ExecutionActivity GetMsBuildActivity()
        {
            return new MsBuildActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "SomeHost" },
                Name = "Build Project",
                Order = 4,
                LocalMsBuildPath = "C:\\Program Files (x86)\\MSBuild\\12.0\\Bin\\MSBuild.exe",
                SolutionPath = "E:\\C\\TestProjects\\git_ps\\JoinUs\\Payoda.JoinUs\\Payoda.JoinUs.sln",
                BuildTargets = "Build",
                BuildProperties = "Configuration=Release"
            };
        }

        [TestMethod]
        public void MsBuildExecutor_Verify_SetsIsSuccess_ToTrueDirectly()
        {
            //// Arrange
            var activity = GetMsBuildActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            var executor = new MsBuildExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Verify();

            //// Assert
            shellManagerMock.Verify();
            Assert.IsTrue(executor.Result.IsSuccess);
        }
    }
}
