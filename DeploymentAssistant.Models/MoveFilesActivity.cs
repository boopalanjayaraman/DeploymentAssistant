using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Models
{
    /// <summary>
    /// Represents the activity of moving files from one location to another
    /// </summary>
    public class MoveFilesActivity : ExecutionActivity
    {
        /// <summary>
        /// Source folder path of the files - full path.
        /// //HostName/Path
        /// </summary>
        public string SourcePath { get; set; }

        /// <summary>
        /// Target path on the given hosts - will be appended to each host name in host info.
        /// </summary>
        public string DestinationPath { get; set; }

        /// <summary>
        /// Add timestamp for destination folder name (if destination path is a folder), when it gets created freshly (if it was not already there).
        /// </summary>
        public bool AddTimeStampForFolder { get; set; }

        /// <summary>
        /// activity type
        /// </summary>
        public override ExecutionType Operation
        {
            get
            {
                return ExecutionType.MoveFiles;
            }
        }

        /// <summary>
        /// Self validating function
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(SourcePath)
                && !string.IsNullOrWhiteSpace(DestinationPath);
        }
    }
}
