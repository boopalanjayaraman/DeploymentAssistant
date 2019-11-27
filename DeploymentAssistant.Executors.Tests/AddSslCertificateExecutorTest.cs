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
    public class AddSslCertificateExecutorTest
    {
        [TestMethod]
        public void AddSslCertificateExecutor_Constructor_Sets_ActivityScriptAndVerificationScript()
        {
            //// Arrange
            var activity = GetAddSslCertificateActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);

            //// Act
            var executor = new AddSslCertificateExecutor(activity, shellManagerMock.Object);

            //// Assert
            shellManagerMock.Verify();
            Assert.IsNotNull(executor.ActivityScriptMap);
            Assert.IsFalse(string.IsNullOrWhiteSpace(executor.ActivityScriptMap.ExecutionScript));
            Assert.IsFalse(string.IsNullOrWhiteSpace(executor.ActivityScriptMap.VerificationScript));
        }

        [TestMethod]
        public void AddSslCertificateExecutor_Execute_CallsShellManager_WithGivenParameters()
        {
            //// Arrange
            var activity = GetAddSslCertificateActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            shellManagerMock.Setup(sm => sm.ExecuteCommands(
                                            It.Is<String>(s => IsSameHost(s, activity.Host.HostName)),
                                            It.Is<List<ScriptWithParameters>>(paramList => ContainsParamsFromActivity(paramList.First(), (AddSslCertificateActivity)activity)),
                                            true)).Returns(new Collection<object>());
            var executor = new AddSslCertificateExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Execute();

            //// Assert
            shellManagerMock.Verify();
        }

        private bool ContainsParamsFromActivity(ScriptWithParameters scriptWithParams, AddSslCertificateActivity addSslCertificateActivity)
        {
            var parameters = scriptWithParams.Params;
            return
                addSslCertificateActivity.BindingIp.Equals(parameters["bindingIp"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && addSslCertificateActivity.CertificateLocalPath.Equals(parameters["certificateLocalPath"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && addSslCertificateActivity.CertificatePassword.Equals(parameters["pwd"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && addSslCertificateActivity.CertificateThumbprint.Equals(parameters["certificateThumbprint"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && addSslCertificateActivity.StoreLocation.Equals(parameters["storeLocation"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && addSslCertificateActivity.WebsiteName.Equals(parameters["websiteName"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && addSslCertificateActivity.StoreName.Equals(parameters["storeName"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && addSslCertificateActivity.Port.Equals(parameters["port"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && addSslCertificateActivity.HostHeader.Equals(parameters["hostHeader"].ToString(), StringComparison.CurrentCultureIgnoreCase)
                && !string.IsNullOrWhiteSpace(scriptWithParams.Script)
                && parameters.Count == 9;
        }

        private bool IsSameHost(string s, string hostName)
        {
            return s.Equals(hostName, StringComparison.CurrentCultureIgnoreCase);
        }

        private ExecutionActivity GetAddSslCertificateActivity()
        {
            return new AddSslCertificateActivity()
            {
                ContinueOnFailure = false,
                Host = new HostInfo() { HostName = "Host1" },
                Name = "Add Ssl Cert to Web",
                WebsiteName = "TestWebsite",
                CertificateLocalPath = "D:\\SomeFolder\\Cert.pfx",
                CertificateThumbprint = "‎0705f4bedd2913afd6ce7d7312c0ac4c38d0bff6",
                BindingIp = "",
                CertificatePassword = "cert@1234",
                Port = "10443",
                StoreName = "WebHosting",
                HostHeader = "",
                StoreLocation = ""
            };
        }

        [TestMethod]
        public void AddSslCertificateExecutor_Execute_Sets_IsSuccessToFalse_DuringException()
        {
            //// Arrange
            var activity = GetAddSslCertificateActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            shellManagerMock.Setup(sm => sm.ExecuteCommands(
                                            It.Is<String>(s => IsSameHost(s, activity.Host.HostName)),
                                            It.Is<List<ScriptWithParameters>>(paramList => ContainsParamsFromActivity(paramList.First(), (AddSslCertificateActivity)activity)),
                                            true)).Throws(new ApplicationException("Some exception"));
            var executor = new AddSslCertificateExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Execute();

            //// Assert
            shellManagerMock.Verify();
            Assert.IsFalse(executor.Result.IsSuccess);
        }

        [TestMethod]
        public void AddSslCertificateExecutor_Verify_CallsShellManager_WithGivenParameters()
        {
            //// Arrange
            var activity = GetAddSslCertificateActivity();
            var shellManagerMock = new Mock<IShellManager>(MockBehavior.Strict);
            shellManagerMock.Setup(sm => sm.ExecuteCommands(
                                            It.Is<String>(s => IsSameHost(s, activity.Host.HostName)),
                                            It.Is<List<ScriptWithParameters>>(paramList => ContainsParamsFromActivity(paramList.First(), (AddSslCertificateActivity)activity)),
                                            true)).Returns(new Collection<object>());
            var executor = new AddSslCertificateExecutor(activity, shellManagerMock.Object);

            //// Act
            executor.Verify();

            //// Assert
            shellManagerMock.Verify();
        }
    }
}
