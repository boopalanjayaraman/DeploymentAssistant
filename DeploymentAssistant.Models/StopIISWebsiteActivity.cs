﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Models
{
    /// <summary>
    /// Represents stopping a IIS website activity
    /// </summary>
    public class StopIISWebsiteActivity : ExecutionActivity
    {
        /// <summary>
        /// Name of the website
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Port number
        /// </summary>
        public string Port { get; set; }

        public StopIISWebsiteActivity()
        {

        }
    }
}