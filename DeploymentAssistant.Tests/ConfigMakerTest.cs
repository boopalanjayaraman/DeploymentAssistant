using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DeploymentAssistant.Common;

namespace DeploymentAssistant.Tests
{
    [TestClass]
    public class ConfigMakerTest
    {
        [TestMethod]
        public void ConfigMaker_Constructor_InitializesMembers()
        {
            //// Arrange
            ConfigMaker configMaker = null;
            var mockFileOperations = new Mock<IFileOperations>(MockBehavior.Strict);

            //// Act
            configMaker = new ConfigMaker(mockFileOperations.Object);

            //// Assert
            Assert.IsNotNull(configMaker.FileOperations);
            Assert.IsNotNull(configMaker.ActivityConfigEntries);
            Assert.IsTrue(configMaker.ActivityConfigEntries.Count > 0);
        }

        [TestMethod]
        public void ConfigMaker_Dump_CallsFileOperations_ToWriteActivityEntriesJson_ToFile()
        {
            //// Arrange
            var mockFileOperations = new Mock<IFileOperations>(MockBehavior.Strict);
            ConfigMaker configMaker = new ConfigMaker(mockFileOperations.Object);
            string fileName = "DeploymentConfig1.Json";
            mockFileOperations.Setup(fo => fo.Exists(It.Is<string>(path => path.Equals(fileName)))).Returns(false);
            mockFileOperations.Setup(fo => fo.WriteAllText(
                                                    It.Is<string>(path => path.Equals(fileName)),
                                                    It.Is<string>(contents => contents.Equals(configMaker.ActivityConfigEntries.ToJson()))
                                                    )
                                    ).Verifiable();

            //// Act
            configMaker.Dump(fileName);

            //// Assert
            mockFileOperations.Verify();
        }

        [TestMethod]
        public void ConfigMaker_Dump_CallsFileOperations_BacksUp_ExistingFile_BeforeWriting()
        {
            //// Arrange
            var mockFileOperations = new Mock<IFileOperations>(MockBehavior.Strict);
            ConfigMaker configMaker = new ConfigMaker(mockFileOperations.Object);
            string fileName = "DeploymentConfig1.Json";
            // pass file.exists as true
            mockFileOperations.Setup(fo => fo.Exists(It.Is<string>(path => path.Equals(fileName)))).Returns(true);
            mockFileOperations.Setup(fo => fo.Copy(It.Is<string>(path => path.Equals(fileName)),
                                                    It.IsAny<string>())).Verifiable();
            mockFileOperations.Setup(fo => fo.WriteAllText(
                                                    It.Is<string>(path => path.Equals(fileName)),
                                                    It.Is<string>(contents => contents.Equals(configMaker.ActivityConfigEntries.ToJson()))
                                                    )
                                    ).Verifiable();

            //// Act
            configMaker.Dump(fileName);

            //// Assert
            mockFileOperations.Verify();
        }
    }
}
