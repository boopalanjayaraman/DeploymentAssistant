using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DeploymentAssistant.Executors;
using DeploymentAssistant.Models;
using System.Configuration;

namespace DeploymentAssistant.Tests
{
    [TestClass]
    public class PipelineMakerTest
    {
        public PipelineMakerTest()
        {

        }

        [TestMethod]
        public void PipelineMaker_Constructor_InitializesMembers()
        {
            //// Arrange
            PipelineMaker pipelineMaker = null;
            var mockFileOperations = new Mock<IFileOperations>(MockBehavior.Strict);
            var mockPipeline = new Mock<IPipeline>(MockBehavior.Strict);

            //// Act
            pipelineMaker = new PipelineMaker(mockFileOperations.Object, mockPipeline.Object);

            //// Assert
            Assert.IsNotNull(pipelineMaker.FileOperations);
            Assert.IsNotNull(pipelineMaker.Pipeline);
        }

        [TestMethod]
        public void PipelineMaker_Load_ReadsConfigEntriesFromFile_AddsToPipeline_CallsRun()
        {
            //// Arrange
            PipelineMaker pipelineMaker = null;
            var mockFileOperations = new Mock<IFileOperations>(MockBehavior.Strict);
            var mockPipeline = new Mock<IPipeline>(MockBehavior.Strict);
            var fileName = "DeploymentConfig.json";
            var stringFromFile = GetSampleSerializedValueFromConfig();
            mockFileOperations.Setup(fo => fo.Exists(It.Is<string>(path => path.Equals(fileName)))).Returns(true);
            mockFileOperations.Setup(fo => fo.ReadAllText(
                                                    It.Is<string>(path => path.Equals(fileName)))
                                    ).Returns(stringFromFile);
            mockPipeline.Setup(pl => pl.Add(It.IsAny<ExecutionActivity>())).Verifiable();
            mockPipeline.Setup(pl => pl.Run()).Verifiable();
            pipelineMaker = new PipelineMaker(mockFileOperations.Object, mockPipeline.Object);

            //// Act
            pipelineMaker.Load(fileName);

            //// Assert
            mockPipeline.Verify();
            mockFileOperations.Verify();
        }

        [TestMethod]
        public void PipelineMaker_Load_Returns_IfConfigFileDoesNotExist()
        {
            //// Arrange
            PipelineMaker pipelineMaker = null;
            var mockFileOperations = new Mock<IFileOperations>(MockBehavior.Strict);
            var mockPipeline = new Mock<IPipeline>(MockBehavior.Strict);
            var fileName = "DeploymentConfig2.json";
            mockFileOperations.Setup(fo => fo.Exists(It.Is<string>(path => path.Equals(fileName)))).Returns(false); // file exists false
             
            pipelineMaker = new PipelineMaker(mockFileOperations.Object, mockPipeline.Object);

            //// Act
            pipelineMaker.Load(fileName);

            //// Assert
            mockPipeline.Verify();
            mockFileOperations.Verify();
        }

        private string GetSampleSerializedValueFromConfig()
        {
            return @"[
                      {
                        ""Operation"": ""StopService"",
                        ""Settings"": {
                          ""$type"": ""DeploymentAssistant.Models.StopServiceActivity, DeploymentAssistant.Models"",
                          ""ServiceName"": ""MongoDB"",
                          ""Operation"": 1,
                          ""Order"": 1,
                          ""Name"": ""MongoDb Service Stop"",
                          ""Host"": {
                            ""HostName"": ""SomeHostName"",
                            ""Port"": null
                          },
                          ""ContinueOnFailure"": false
                        }
                      }
               ]             
            ";
        }

        [TestMethod]
        public void PipelineMaker_Load_GetsFileNameFromConfig_IfPassedEmpty()
        {
            //// Arrange
            PipelineMaker pipelineMaker = null;
            var mockFileOperations = new Mock<IFileOperations>(MockBehavior.Strict);
            var mockPipeline = new Mock<IPipeline>(MockBehavior.Strict);
            var fileName = ConfigurationManager.AppSettings["ExecutorsConfigFile"];

            var stringFromFile = GetSampleSerializedValueFromConfig();
            mockFileOperations.Setup(fo => fo.Exists(It.Is<string>(path => path.Equals(fileName)))).Returns(true);
            mockFileOperations.Setup(fo => fo.ReadAllText(
                                                    It.Is<string>(path => path.Equals(fileName)))
                                    ).Returns(stringFromFile);
            mockPipeline.Setup(pl => pl.Add(It.IsAny<ExecutionActivity>())).Verifiable();
            mockPipeline.Setup(pl => pl.Run()).Verifiable();
            pipelineMaker = new PipelineMaker(mockFileOperations.Object, mockPipeline.Object);

            //// Act
            pipelineMaker.Load("");// empty file name

            //// Assert
            mockPipeline.Verify();
            mockFileOperations.Verify();
        }

        [TestMethod]
        public void PipelineMaker_Load_AppendsJsonToFileName()
        {
            //// Arrange
            PipelineMaker pipelineMaker = null;
            var mockFileOperations = new Mock<IFileOperations>(MockBehavior.Strict);
            var mockPipeline = new Mock<IPipeline>(MockBehavior.Strict);
            var fileName = "DeploymentConfig";
            string fileNameWithJson = "DeploymentConfig.json"; 

            var stringFromFile = GetSampleSerializedValueFromConfig();
            mockFileOperations.Setup(fo => fo.Exists(It.Is<string>(path => path.Equals(fileNameWithJson, StringComparison.CurrentCultureIgnoreCase)))).Returns(true);
            mockFileOperations.Setup(fo => fo.ReadAllText(
                                                    It.Is<string>(path => path.Equals(fileNameWithJson, StringComparison.CurrentCultureIgnoreCase)))
                                    ).Returns(stringFromFile);
            mockPipeline.Setup(pl => pl.Add(It.IsAny<ExecutionActivity>())).Verifiable();
            mockPipeline.Setup(pl => pl.Run()).Verifiable();
            pipelineMaker = new PipelineMaker(mockFileOperations.Object, mockPipeline.Object);

            //// Act
            pipelineMaker.Load(fileName); //passing without extension

            //// Assert
            mockPipeline.Verify();
            mockFileOperations.Verify();
        }
    }
}
