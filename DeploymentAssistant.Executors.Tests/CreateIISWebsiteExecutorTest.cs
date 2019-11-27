using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DeploymentAssistant.Models;
using DeploymentAssistant.Executors.Models;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections;

namespace DeploymentAssistant.Executors.Tests
{
    [TestClass]
    public class CreateIISWebsiteExecutorTest
    {
        [TestMethod]
        public void CreateIISWebsiteExecutor_Constructor_Sets_ActivityScriptAndVerificationScript()
        {
            //// Arrange
            var activity = GetCreateIISWebsiteActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);

            //// Act
            var executor = new CreateIISWebsiteExecutor(activity, shellManagerMock.Object);

            //// Assert
            shellManagerMock.Verify();
            Assert.IsNotNull(executor.ActivityScriptMap);
            Assert.IsFalse(string.IsNullOrWhiteSpace(executor.ActivityScriptMap.ExecutionScript));
            Assert.IsFalse(string.IsNullOrWhiteSpace(executor.ActivityScriptMap.VerificationScript));
        }

        [TestMethod]
        public void CreateIISWebsiteExecutor_Execute_CallsShellManager_WithGivenParameters()
        {
            //// Arrange
            var activity = GetCreateIISWebsiteActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            shellManagerMock.Setup(sm => sm.ExecuteCommands(
                                            It.Is<String>(s => IsSameHost(s, activity.Host.HostName)),
                                            It.Is<List<ScriptWithParameters>>(paramList => ContainsParamsFromActivity(paramList.First(), (CreateIISWebsiteActivity)activity)),
                                            true)).Returns(new Collection<object>());
            var executor = new CreateIISWebsiteExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Execute();

            //// Assert
            shellManagerMock.Verify();
        }

        [TestMethod]
        public void CreateIISWebsiteExecutor_Execute_Sets_IsSuccessToFalse_DuringException()
        {
            //// Arrange
            var activity = GetCreateIISWebsiteActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            shellManagerMock.Setup(sm => sm.ExecuteCommands(
                                            It.Is<String>(s => IsSameHost(s, activity.Host.HostName)),
                                            It.Is<List<ScriptWithParameters>>(paramList => ContainsParamsFromActivity(paramList.First(), (CreateIISWebsiteActivity)activity)),
                                            true)).Throws(new ApplicationException("Sample exception"));
            var executor = new CreateIISWebsiteExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Execute();

            //// Assert
            shellManagerMock.Verify();
            Assert.IsFalse(executor.Result.IsSuccess);
        }

        private bool ContainsParamsFromActivity(ScriptWithParameters scriptWithParams, CreateIISWebsiteActivity createIISWebsitesActivity)
        {
            var parameters = scriptWithParams.Params;
            var bindingsParam = parameters["bindings"] as Hashtable;

            return
                createIISWebsitesActivity.WebsiteName.Equals(parameters["websiteName"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && createIISWebsitesActivity.Bindings.Count == bindingsParam.Count
                && createIISWebsitesActivity.LocalPhysicalPath.Equals(parameters["physicalPath"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && createIISWebsitesActivity.OverrideIfExists == (bool)parameters["override"]
                && !string.IsNullOrWhiteSpace(scriptWithParams.Script)
                && parameters.Count == 4;
        }


        private bool IsSameHost(string s, string hostName)
        {
            return s.Equals(hostName, StringComparison.CurrentCultureIgnoreCase);
        }

        private ExecutionActivity GetCreateIISWebsiteActivity()
        {
            return new CreateIISWebsiteActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "SomeHost" },
                Name = "Create Website",
                Order = 4,
                WebsiteName = "TestWebsite",
                LocalPhysicalPath = @"E:\SomeHost\WebFolder",
                Bindings = new Hashtable() { {"http", "*:80:"} },
                OverrideIfExists = true
            };
        }

        [TestMethod]
        public void CreateIISWebsiteExecutor_Verify_CallsShellManager_WithGivenParameters()
        {
            //// Arrange
            var activity = GetCreateIISWebsiteActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            shellManagerMock.Setup(sm => sm.ExecuteCommands(
                                            It.Is<String>(s => IsSameHost(s, activity.Host.HostName)),
                                            It.Is<List<ScriptWithParameters>>(paramList => ContainsParamsFromActivity(paramList.First(), (CreateIISWebsiteActivity)activity)),
                                            true)).Returns(new Collection<object>());
            var executor = new CreateIISWebsiteExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Verify();

            //// Assert
            shellManagerMock.Verify();
        }
    }
    
}
