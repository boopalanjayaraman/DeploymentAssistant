using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeploymentAssistant.Models;

namespace DeploymentAssistant.Tests
{
    [TestClass]
    public class ActivityConfigEntryTest
    {
        [TestMethod]
        public void ActivityConfigEntry_DefaultConstructor_Sets_OperationAndSettings_Properties()
        {
            //// Arrange
            ActivityConfigEntry activityConfigEntry = null;

            //// Act
            activityConfigEntry = new ActivityConfigEntry("CopyFiles", new CopyFilesActivity());

            //// Assert
            Assert.IsNotNull(activityConfigEntry.Settings);
            Assert.IsFalse(string.IsNullOrWhiteSpace(activityConfigEntry.Operation));
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException), "Operation type is not found / implemented.")]
        public void ActivityConfigEntry_DefaultConstructor_SettingOperation_ToInvalidValue_ThrowsError()
        {
            //// Arrange
            ActivityConfigEntry activityConfigEntry = null;

            //// Act
            activityConfigEntry = new ActivityConfigEntry("CopyFilesAbc", new CopyFilesActivity());

            //// Assert
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException), "Setting / Activity does not correspond to the operation type mentioned.")]
        public void ActivityConfigEntry_DefaultConstructor_SettingSettings_ToInvalidValue_ThrowsError()
        {
            //// Arrange
            ActivityConfigEntry activityConfigEntry = null;

            //// Act
            activityConfigEntry = new ActivityConfigEntry("CopyFiles", new MoveFilesActivity());

            //// Assert
            Assert.Fail();
        }
    }
}
