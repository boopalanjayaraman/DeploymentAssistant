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
    public class ExecutionPipelineTest
    {
        [TestMethod]
        public void ExecutionPipeline_Constructor_InitializesMembers()
        {
            //// Arrange
            ExecutionPipeline pipeline = null;
            var executorProviderMock = new Mock<IExecutorProvider>();

            //// Act
            pipeline = new ExecutionPipeline(executorProviderMock.Object);

            //// Assert
            Assert.IsNotNull(pipeline.Steps);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException), "Pipeline Validation Failure: Activity is null.")]
        public void ExecutionPipeline_Add_ThrowsException_WhenActivityIsNull()
        {
            //// Arrange
            var executorProviderMock = new Mock<IExecutorProvider>();
            ExecutionPipeline pipeline = new ExecutionPipeline(executorProviderMock.Object);
            ExecutionActivity invalidActivity = null;

            //// Act
            pipeline.Add(invalidActivity);

            //// Assert
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException), "Pipeline Validation Failure: Host details are unavailable")]
        public void ExecutionPipeline_Add_ThrowsException_WhenActivityHasEmptyHost()
        {
            //// Arrange
            var executorProviderMock = new Mock<ExecutorProvider>();
            ExecutionPipeline pipeline = new ExecutionPipeline(executorProviderMock.Object);
            ExecutionActivity invalidActivity = new CopyFilesActivity() { Host = new HostInfo() };

            //// Act
            pipeline.Add(invalidActivity);

            //// Assert
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException), "Pipeline Validation Failure: Activity Validation Failed.")]
        public void ExecutionPipeline_Add_ThrowsException_WhenActivityFailsValidation()
        {
            //// Arrange
            var executorProviderMock = new Mock<IExecutorProvider>();
            ExecutionPipeline pipeline = new ExecutionPipeline(executorProviderMock.Object);
            ExecutionActivity invalidActivity = GetInvalidActivity();

            //// Act
            pipeline.Add(invalidActivity);

            //// Assert
            Assert.Fail();
        }

        private ExecutionActivity GetInvalidActivity()
        {
            //// returning without filling mandatory fields but bare minimum details to pass basic validation
            return new
                CopyFilesActivity() { Host = new HostInfo() { HostName = "SomeHost" } };
        }

        [TestMethod]
        public void ExecutionPipeline_Add_AddsStep_WhenValid()
        {
            //// Arrange
            var executorProviderMock = new Mock<IExecutorProvider>();
            ExecutionPipeline pipeline = new ExecutionPipeline(executorProviderMock.Object);
            ExecutionActivity validActivity = GetValidActivity();

            //// Act
            pipeline.Add(validActivity);

            //// Assert
            Assert.IsTrue(pipeline.Steps.Count == 1);
            Assert.IsTrue(object.ReferenceEquals(pipeline.Steps.First(), validActivity));
        }

        private ExecutionActivity GetValidActivity()
        {
            return new
                CopyFilesActivity() { Host = new HostInfo() { HostName = "SomeHost" }, SourcePath = "C:\\SourcePath", DestinationPath = "D:\\DestinationPath" };
        }

        [TestMethod]
        public void ExecutionPipeline_Add_RaisesEvent_AfterAddingStep_AndPassesActivityInEventArgs()
        {
            //// Arrange
            var executorProviderMock = new Mock<IExecutorProvider>();
            ExecutionPipeline pipeline = new ExecutionPipeline(executorProviderMock.Object);
            ExecutionActivity validActivity = GetValidActivity();
            bool eventRaised = false;
            EventArgs addedEventArgs = null;
            pipeline.StepAdded += (sender, e) =>
            {
                eventRaised = true;
                addedEventArgs = e;
            };

            //// Act
            pipeline.Add(validActivity);

            //// Assert
            Assert.IsTrue(eventRaised);
            Assert.IsTrue(object.ReferenceEquals(((ActivityEventArgs)addedEventArgs).Activity, validActivity));
        }

        /*
        [TestMethod]
        public void ExecutionPipeline_Run_CallsExecute_AndVerify()
        {
            //// Arrange
            var executorProviderMock = new Mock<IExecutorProvider>();
            var executorMock = new Mock<AbstractExecutor>();
            ExecutionPipeline pipeline = new ExecutionPipeline(executorProviderMock.Object);
            ExecutionActivity validActivity = GetValidActivity();
            pipeline.Add(validActivity);
            bool executeCalled = false;
            bool verifyCalled = false;
            executorMock.Setup(e => e.Execute()).Callback(() => { executeCalled = true; });
            executorMock.Setup(e => e.Verify()).Callback(() => { verifyCalled = true; });
            executorMock.Setup(e => e.Result).Returns(new ExecutionResult() { IsSuccess = true });
            executorProviderMock.Setup(ep => ep.GetExecutor(It.Is<ExecutionActivity>(act => object.ReferenceEquals(act, validActivity)))).Returns(executorMock.Object);

            //// Act
            pipeline.Run();

            //// Assert
            executorMock.Verify();
            executorProviderMock.Verify();
            Assert.IsTrue(executeCalled);
            Assert.IsTrue(verifyCalled);
        }*/


    }
}
