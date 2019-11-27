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
    public class StartIISWebServerExecutorTest
    {
        [TestMethod]
        public void StartIISWebServerExecutor_Constructor_Sets_ActivityScriptAndVerificationScript()
        {
            //// Arrange
            var activity = GetStartIISWebServerActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);

            //// Act
            var executor = new StartIISWebServerExecutor(activity, shellManagerMock.Object);

            //// Assert
            shellManagerMock.Verify();
            Assert.IsNotNull(executor.ActivityScriptMap);
            Assert.IsFalse(string.IsNullOrWhiteSpace(executor.ActivityScriptMap.ExecutionScript));
            Assert.IsTrue(string.IsNullOrWhiteSpace(executor.ActivityScriptMap.VerificationScript));
        }

        [TestMethod]
        public void StartIISWebServerExecutor_Execute_CallsShellManager_WithGivenParameters()
        {
            //// Arrange
            var activity = GetStartIISWebServerActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            shellManagerMock.Setup(sm => sm.ExecuteCommands(
                                            It.Is<String>(s => IsSameHost(s, activity.Host.HostName)),
                                            It.Is<List<ScriptWithParameters>>(paramList => ContainsParamsFromActivity(paramList.First(), (StartIISWebServerActivity)activity)),
                                            true)).Returns(new Collection<object>());
            var executor = new StartIISWebServerExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Execute();

            //// Assert
            shellManagerMock.Verify();
        }

        [TestMethod]
        public void StartIISWebServerExecutor_Execute_Sets_IsSuccessToFalse_DuringException()
        {
            //// Arrange
            var activity = GetStartIISWebServerActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            shellManagerMock.Setup(sm => sm.ExecuteCommands(
                                            It.Is<String>(s => IsSameHost(s, activity.Host.HostName)),
                                            It.Is<List<ScriptWithParameters>>(paramList => ContainsParamsFromActivity(paramList.First(), (StartIISWebServerActivity)activity)),
                                            true)).Throws(new ApplicationException("Sample exception"));
            var executor = new StartIISWebServerExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Execute();

            //// Assert
            shellManagerMock.Verify();
            Assert.IsFalse(executor.Result.IsSuccess);
        }

        private bool ContainsParamsFromActivity(ScriptWithParameters scriptWithParams, StartIISWebServerActivity activity)
        {
            var parameters = scriptWithParams.Params;

            return
                !String.IsNullOrWhiteSpace(scriptWithParams.Script)
                && !string.IsNullOrWhiteSpace(scriptWithParams.Script)
                && parameters == null;
        }

        private bool IsSameHost(string s, string hostName)
        {
            return s.Equals(hostName, StringComparison.CurrentCultureIgnoreCase);
        }

        private ExecutionActivity GetStartIISWebServerActivity()
        {
            return new StartIISWebServerActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "SomeHost" },
                Name = "Start IIS",
                Order = 4,
            };
        }

        [TestMethod]
        public void StartIISWebServerExecutor_Verify_SetsIsSuccess_ToTrueDirectly()
        {
            //// Arrange
            var activity = GetStartIISWebServerActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            var executor = new StartIISWebServerExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Verify();

            //// Assert
            shellManagerMock.Verify();
            Assert.IsTrue(executor.Result.IsSuccess);
        }
    }
}
