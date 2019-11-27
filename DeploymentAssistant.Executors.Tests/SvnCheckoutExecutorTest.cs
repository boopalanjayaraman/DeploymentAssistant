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
    public class SvnCheckoutExecutorTest
    {
        [TestMethod]
        public void SvnCheckoutExecutor_Constructor_Sets_ActivityScriptAndVerificationScript()
        {
            //// Arrange
            var activity = GetSvnCheckoutActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);

            //// Act
            var executor = new SvnCheckoutExecutor(activity, shellManagerMock.Object);

            //// Assert
            shellManagerMock.Verify();
            Assert.IsNotNull(executor.ActivityScriptMap);
            Assert.IsFalse(string.IsNullOrWhiteSpace(executor.ActivityScriptMap.ExecutionScript));
            Assert.IsFalse(string.IsNullOrWhiteSpace(executor.ActivityScriptMap.VerificationScript));
        }

        [TestMethod]
        public void SvnCheckoutExecutor_Execute_CallsShellManager_WithGivenParameters()
        {
            //// Arrange
            var activity = GetSvnCheckoutActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            shellManagerMock.Setup(sm => sm.ExecuteCommands(
                                            It.Is<String>(s => IsSameHost(s, activity.Host.HostName)),
                                            It.Is<List<ScriptWithParameters>>(paramList => ContainsParamsFromActivity(paramList.First(), (SvnCheckoutActivity)activity)),
                                            true)).Returns(new Collection<object>());
            var executor = new SvnCheckoutExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Execute();

            //// Assert
            shellManagerMock.Verify();
        }

        [TestMethod]
        public void SvnCheckoutExecutor_Execute_Sets_IsSuccessToFalse_DuringException()
        {
            //// Arrange
            var activity = GetSvnCheckoutActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            shellManagerMock.Setup(sm => sm.ExecuteCommands(
                                            It.Is<String>(s => IsSameHost(s, activity.Host.HostName)),
                                            It.Is<List<ScriptWithParameters>>(paramList => ContainsParamsFromActivity(paramList.First(), (SvnCheckoutActivity)activity)),
                                            true)).Throws(new ApplicationException("Sample exception"));
            var executor = new SvnCheckoutExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Execute();

            //// Assert
            shellManagerMock.Verify();
            Assert.IsFalse(executor.Result.IsSuccess);
        }

        private bool ContainsParamsFromActivity(ScriptWithParameters scriptWithParams, SvnCheckoutActivity activity)
        {
            var parameters = scriptWithParams.Params;

            return
                activity.LocalDestinationPath.Equals(parameters["localDestinationPath"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && activity.RepoUrl.Equals(parameters["repoUrl"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && activity.UseCheckoutOrUpdate == (bool)parameters["useCheckoutOrUpdate"]
                && activity.UserName.Equals(parameters["userName"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && activity.Password.Equals(parameters["pwd"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && !string.IsNullOrWhiteSpace(scriptWithParams.Script)
                && parameters.Count == 5;
        }

        private bool VerifyContainsParamsFromActivity(ScriptWithParameters scriptWithParams, SvnCheckoutActivity activity)
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

        private ExecutionActivity GetSvnCheckoutActivity()
        {
            return new SvnCheckoutActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "SomeHost" },
                Name = "Svn checkout ProjectCode",
                Order = 2,
                RepoUrl = "https://svn.company.com/Project/trunk",
                UseCheckoutOrUpdate = true,
                LocalDestinationPath = "E:\\C\\TestProjects\\SvnCheckout",
                UserName = "",
                Password = ""
            };
        }

        [TestMethod]
        public void SvnCheckoutExecutor_Verify_CallsShellManager_WithGivenParameters()
        {
            //// Arrange
            var activity = GetSvnCheckoutActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            shellManagerMock.Setup(sm => sm.ExecuteCommands(
                                            It.Is<String>(s => IsSameHost(s, activity.Host.HostName)),
                                            It.Is<List<ScriptWithParameters>>(paramList => VerifyContainsParamsFromActivity(paramList.First(), (SvnCheckoutActivity)activity)),
                                            true)).Returns(new Collection<object>());
            var executor = new SvnCheckoutExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Verify();

            //// Assert
            shellManagerMock.Verify();
        }
    }
}
