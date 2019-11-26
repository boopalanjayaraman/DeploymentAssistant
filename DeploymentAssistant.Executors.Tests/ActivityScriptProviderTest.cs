using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeploymentAssistant.Models;

namespace DeploymentAssistant.Executors.Tests
{
    [TestClass]
    public class ActivityScriptProviderTest
    {
        [TestMethod]
        public void ActivityScriptProvider_GetActivityScriptMap_ReturnsMapObject_ForAddSslCertificate()
        {
            //// Arrange
            var executionType = ExecutionType.AddSslCertificate;

            //// Act
            var map = ActivityScriptProvider.GetActivityScriptMap(executionType);

            //// Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(map.ExecutionScriptFile));
            Assert.IsFalse(string.IsNullOrWhiteSpace(map.VerificationScriptFile));
        }

        [TestMethod]
        public void ActivityScriptProvider_GetActivityScriptMap_ReturnsMapObject_ForCopyFiles()
        {
            //// Arrange
            var executionType = ExecutionType.CopyFiles;

            //// Act
            var map = ActivityScriptProvider.GetActivityScriptMap(executionType);

            //// Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(map.ExecutionScriptFile));
            Assert.IsFalse(string.IsNullOrWhiteSpace(map.VerificationScriptFile));
        }

        [TestMethod]
        public void ActivityScriptProvider_GetActivityScriptMap_ReturnsMapObject_ForCreateIISWebsite()
        {
            //// Arrange
            var executionType = ExecutionType.CreateIISWebsite;

            //// Act
            var map = ActivityScriptProvider.GetActivityScriptMap(executionType);

            //// Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(map.ExecutionScriptFile));
            Assert.IsFalse(string.IsNullOrWhiteSpace(map.VerificationScriptFile));
        }

        [TestMethod]
        public void ActivityScriptProvider_GetActivityScriptMap_ReturnsMapObject_ForDeleteFiles()
        {
            //// Arrange
            var executionType = ExecutionType.DeleteFiles;

            //// Act
            var map = ActivityScriptProvider.GetActivityScriptMap(executionType);

            //// Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(map.ExecutionScriptFile));
            Assert.IsFalse(string.IsNullOrWhiteSpace(map.VerificationScriptFile));
        }

        [TestMethod]
        public void ActivityScriptProvider_GetActivityScriptMap_ReturnsMapObject_ForMoveFiles()
        {
            //// Arrange
            var executionType = ExecutionType.MoveFiles;

            //// Act
            var map = ActivityScriptProvider.GetActivityScriptMap(executionType);

            //// Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(map.ExecutionScriptFile));
            Assert.IsFalse(string.IsNullOrWhiteSpace(map.VerificationScriptFile));
        }

        [TestMethod]
        public void ActivityScriptProvider_GetActivityScriptMap_ReturnsMapObject_ForStartIISWebServer()
        {
            //// Arrange
            var executionType = ExecutionType.StartIISWebServer;

            //// Act
            var map = ActivityScriptProvider.GetActivityScriptMap(executionType);

            //// Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(map.ExecutionScriptFile));
            Assert.IsTrue(string.IsNullOrWhiteSpace(map.VerificationScriptFile));
        }

        [TestMethod]
        public void ActivityScriptProvider_GetActivityScriptMap_ReturnsMapObject_ForStartIISWebsite()
        {
            //// Arrange
            var executionType = ExecutionType.StartIISWebsite;

            //// Act
            var map = ActivityScriptProvider.GetActivityScriptMap(executionType);

            //// Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(map.ExecutionScriptFile));
            Assert.IsFalse(string.IsNullOrWhiteSpace(map.VerificationScriptFile));
        }

        [TestMethod]
        public void ActivityScriptProvider_GetActivityScriptMap_ReturnsMapObject_ForStartService()
        {
            //// Arrange
            var executionType = ExecutionType.StartService;

            //// Act
            var map = ActivityScriptProvider.GetActivityScriptMap(executionType);

            //// Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(map.ExecutionScriptFile));
            Assert.IsFalse(string.IsNullOrWhiteSpace(map.VerificationScriptFile));
        }

        [TestMethod]
        public void ActivityScriptProvider_GetActivityScriptMap_ReturnsMapObject_ForStopIISWebServer()
        {
            //// Arrange
            var executionType = ExecutionType.StopIISWebServer;

            //// Act
            var map = ActivityScriptProvider.GetActivityScriptMap(executionType);

            //// Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(map.ExecutionScriptFile));
            Assert.IsTrue(string.IsNullOrWhiteSpace(map.VerificationScriptFile));
        }

        [TestMethod]
        public void ActivityScriptProvider_GetActivityScriptMap_ReturnsMapObject_ForStopIISWebsite()
        {
            //// Arrange
            var executionType = ExecutionType.StopIISWebsite;

            //// Act
            var map = ActivityScriptProvider.GetActivityScriptMap(executionType);

            //// Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(map.ExecutionScriptFile));
            Assert.IsFalse(string.IsNullOrWhiteSpace(map.VerificationScriptFile));
        }

        [TestMethod]
        public void ActivityScriptProvider_GetActivityScriptMap_ReturnsMapObject_ForStopService()
        {
            //// Arrange
            var executionType = ExecutionType.StopService;

            //// Act
            var map = ActivityScriptProvider.GetActivityScriptMap(executionType);

            //// Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(map.ExecutionScriptFile));
            Assert.IsFalse(string.IsNullOrWhiteSpace(map.VerificationScriptFile));
        }

        [TestMethod]
        public void ActivityScriptProvider_GetActivityScriptMap_ReturnsMapObject_ForGitClone()
        {
            //// Arrange
            var executionType = ExecutionType.GitClone;

            //// Act
            var map = ActivityScriptProvider.GetActivityScriptMap(executionType);

            //// Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(map.ExecutionScriptFile));
            Assert.IsFalse(string.IsNullOrWhiteSpace(map.VerificationScriptFile));
        }

        [TestMethod]
        public void ActivityScriptProvider_GetActivityScriptMap_ReturnsMapObject_ForSvnCheckout()
        {
            //// Arrange
            var executionType = ExecutionType.SvnCheckout;

            //// Act
            var map = ActivityScriptProvider.GetActivityScriptMap(executionType);

            //// Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(map.ExecutionScriptFile));
            Assert.IsFalse(string.IsNullOrWhiteSpace(map.VerificationScriptFile));
        }

        [TestMethod]
        public void ActivityScriptProvider_GetActivityScriptMap_ReturnsMapObject_ForMsBuild()
        {
            //// Arrange
            var executionType = ExecutionType.MsBuild;

            //// Act
            var map = ActivityScriptProvider.GetActivityScriptMap(executionType);

            //// Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(map.ExecutionScriptFile));
            Assert.IsTrue(string.IsNullOrWhiteSpace(map.VerificationScriptFile));
        }
    }
}
