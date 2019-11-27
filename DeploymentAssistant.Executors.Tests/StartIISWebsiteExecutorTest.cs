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
    public class StartIISWebsiteExecutorTest
    {
        [TestMethod]
        public void StartIISWebsiteExecutor_Constructor_Sets_ActivityScriptAndVerificationScript()
        {
            //// Arrange
            var activity = GetStartIISWebsiteActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);

            //// Act
            var executor = new StartIISWebsiteExecutor(activity, shellManagerMock.Object);

            //// Assert
            shellManagerMock.Verify();
            Assert.IsNotNull(executor.ActivityScriptMap);
            Assert.IsFalse(string.IsNullOrWhiteSpace(executor.ActivityScriptMap.ExecutionScript));
            Assert.IsFalse(string.IsNullOrWhiteSpace(executor.ActivityScriptMap.VerificationScript));
        }

        [TestMethod]
        public void StartIISWebsiteExecutor_Execute_CallsShellManager_WithGivenParameters()
        {
            //// Arrange
            var activity = GetStartIISWebsiteActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            shellManagerMock.Setup(sm => sm.ExecuteCommands(
                                            It.Is<String>(s => IsSameHost(s, activity.Host.HostName)),
                                            It.Is<List<ScriptWithParameters>>(paramList => ContainsParamsFromActivity(paramList.First(), (StartIISWebsiteActivity)activity)),
                                            true)).Returns(new Collection<object>());
            var executor = new StartIISWebsiteExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Execute();

            //// Assert
            shellManagerMock.Verify();
        }

        [TestMethod]
        public void StartIISWebsiteExecutor_Execute_Sets_IsSuccessToFalse_DuringException()
        {
            //// Arrange
            var activity = GetStartIISWebsiteActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            shellManagerMock.Setup(sm => sm.ExecuteCommands(
                                            It.Is<String>(s => IsSameHost(s, activity.Host.HostName)),
                                            It.Is<List<ScriptWithParameters>>(paramList => ContainsParamsFromActivity(paramList.First(), (StartIISWebsiteActivity)activity)),
                                            true)).Throws(new ApplicationException("Sample exception"));
            var executor = new StartIISWebsiteExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Execute();

            //// Assert
            shellManagerMock.Verify();
            Assert.IsFalse(executor.Result.IsSuccess);
        }

        private bool ContainsParamsFromActivity(ScriptWithParameters scriptWithParams, StartIISWebsiteActivity activity)
        {
            var parameters = scriptWithParams.Params;

            return
                activity.WebsiteName.Equals(parameters["website"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && !(string.IsNullOrWhiteSpace(scriptWithParams.Script))
                && !string.IsNullOrWhiteSpace(scriptWithParams.Script)
                && parameters.Count == 1;
        }


        private bool IsSameHost(string s, string hostName)
        {
            return s.Equals(hostName, StringComparison.CurrentCultureIgnoreCase);
        }

        private ExecutionActivity GetStartIISWebsiteActivity()
        {
            return new StartIISWebsiteActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "SomeHost" },
                Name = "Start Website",
                Order = 4,
                WebsiteName = "TestWeb"
            };
        }

        [TestMethod]
        public void StartIISWebsiteExecutor_Verify_CallsShellManager_WithGivenParameters()
        {
            //// Arrange
            var activity = GetStartIISWebsiteActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            shellManagerMock.Setup(sm => sm.ExecuteCommands(
                                            It.Is<String>(s => IsSameHost(s, activity.Host.HostName)),
                                            It.Is<List<ScriptWithParameters>>(paramList => ContainsParamsFromActivity(paramList.First(), (StartIISWebsiteActivity)activity)),
                                            true)).Returns(new Collection<object>());
            var executor = new StartIISWebsiteExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Verify();

            //// Assert
            shellManagerMock.Verify();
        }
    }
}
