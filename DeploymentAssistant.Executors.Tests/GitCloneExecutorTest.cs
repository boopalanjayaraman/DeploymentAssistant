using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeploymentAssistant.Models;
using Moq;
using DeploymentAssistant.Executors.Models;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace DeploymentAssistant.Executors.Tests
{
    [TestClass]
    public class GitCloneExecutorTest
    {
        [TestMethod]
        public void GitCloneExecutor_Constructor_Sets_ActivityScriptAndVerificationScript()
        {
            //// Arrange
            var activity = GetGitCloneActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);

            //// Act
            var executor = new GitCloneExecutor(activity, shellManagerMock.Object);

            //// Assert
            shellManagerMock.Verify();
            Assert.IsNotNull(executor.ActivityScriptMap);
            Assert.IsFalse(string.IsNullOrWhiteSpace(executor.ActivityScriptMap.ExecutionScript));
            Assert.IsFalse(string.IsNullOrWhiteSpace(executor.ActivityScriptMap.VerificationScript));
        }

        [TestMethod]
        public void GitCloneExecutor_Execute_CallsShellManager_WithGivenParameters()
        {
            //// Arrange
            var activity = GetGitCloneActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            shellManagerMock.Setup(sm => sm.ExecuteCommands(
                                            It.Is<String>(s => IsSameHost(s, activity.Host.HostName)),
                                            It.Is<List<ScriptWithParameters>>(paramList => ContainsParamsFromActivity(paramList.First(), (GitCloneActivity)activity)),
                                            true)).Returns(new Collection<object>());
            var executor = new GitCloneExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Execute();

            //// Assert
            shellManagerMock.Verify();
        }

        [TestMethod]
        public void GitCloneExecutor_Execute_Sets_IsSuccessToFalse_DuringException()
        {
            //// Arrange
            var activity = GetGitCloneActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            shellManagerMock.Setup(sm => sm.ExecuteCommands(
                                            It.Is<String>(s => IsSameHost(s, activity.Host.HostName)),
                                            It.Is<List<ScriptWithParameters>>(paramList => ContainsParamsFromActivity(paramList.First(), (GitCloneActivity)activity)),
                                            true)).Throws(new ApplicationException("Sample exception"));
            var executor = new GitCloneExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Execute();

            //// Assert
            shellManagerMock.Verify();
            Assert.IsFalse(executor.Result.IsSuccess);
        }

        private bool ContainsParamsFromActivity(ScriptWithParameters scriptWithParams, GitCloneActivity activity)
        {
            var parameters = scriptWithParams.Params;

            return
                activity.LocalDestinationPath.Equals(parameters["localDestinationPath"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && activity.RepoUrl.Equals(parameters["repoUrl"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && activity.UseCloneOrPull == (bool)parameters["useCloneOrPull"]
                && !string.IsNullOrWhiteSpace(scriptWithParams.Script)
                && parameters.Count == 3;
        }

        private bool VerifyContainsParamsFromActivity(ScriptWithParameters scriptWithParams, GitCloneActivity activity)
        {
            var parameters = scriptWithParams.Params;

            return
                activity.LocalDestinationPath.Equals(parameters["localDestinationPath"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && activity.RepoUrl.Equals(parameters["repoUrl"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && !string.IsNullOrWhiteSpace(scriptWithParams.Script)
                && parameters.Count == 2;
        }

        private bool IsSameHost(string s, string hostName)
        {
            return s.Equals(hostName, StringComparison.CurrentCultureIgnoreCase);
        }

        private ExecutionActivity GetGitCloneActivity()
        {
            return new GitCloneActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "SomeHost" },
                Name = "Clone ProjectCode",
                Order = 2,
                RepoUrl = "https://gitlab.company.com/BlackPearl/Project.git",
                UseCloneOrPull = false,
                LocalDestinationPath = "E:\\C\\TestProjects\\GitClone"
            };
        }

        [TestMethod]
        public void GitCloneExecutor_Verify_CallsShellManager_WithGivenParameters()
        {
            //// Arrange
            var activity = GetGitCloneActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            shellManagerMock.Setup(sm => sm.ExecuteCommands(
                                            It.Is<String>(s => IsSameHost(s, activity.Host.HostName)),
                                            It.Is<List<ScriptWithParameters>>(paramList => VerifyContainsParamsFromActivity(paramList.First(), (GitCloneActivity)activity)),
                                            true)).Returns(new Collection<object>());
            var executor = new GitCloneExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Verify();

            //// Assert
            shellManagerMock.Verify();
        }
    }
}
