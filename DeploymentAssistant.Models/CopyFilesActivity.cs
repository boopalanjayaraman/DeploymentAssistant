﻿using System;
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
        public string TargetPath { get; set; }

        /// <summary>
        /// Says if the copy activity is being done for a file rather than a folder. 
        /// default:false
        /// </summary>
        public bool IsFile { get; set; }
    }
}
