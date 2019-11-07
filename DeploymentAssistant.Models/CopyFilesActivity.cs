using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Models
{
    /// <summary>
    /// Represents the activity of Copying files from one location to another
    /// </summary>
    public class CopyFilesActivity : ExecutionActivity
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
        /// Exclude these Folders under the source path from copying 
        /// Should use a preceding backslash
        /// </summary>
        public List<string> SkipFolders { get; set; }

        /// <summary>
        /// Exclude these folders under the source path from copying if they exist already in the target path. Copy otherwise.
        /// Should use a preceding backslash
        /// </summary>
        public List<string> SkipFoldersIfExist { get; set; }

        /// <summary>
        /// Exclude files with these extensions from copying. Ex. ".pdb", ".csproj"
        /// </summary>
        public List<string> ExcludeExtensions { get; set; }

        /// <summary>
        /// Says if the copy activity is being done for a file rather than a folder. 
        /// default:false
        /// </summary>
        public bool IsFile { get; set; }

        /// <summary>
        /// activity type
        /// </summary>
        public override ExecutionType Operation
        {
            get
            {
                return ExecutionType.CopyFiles;
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
