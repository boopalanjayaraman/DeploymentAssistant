using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Executors.Models
{
    /// <summary>
    /// Class to hold script block and its associated parameters as objects
    /// </summary>
    internal class ScriptWithParameters
    {
        /// <summary>
        /// Holds the powershell script text to be executed
        /// </summary>
        public string Script { get; set; }

        /// <summary>
        /// Parameters to be passed in to the script. Represented by key-value pairs.
        /// </summary>
        public Dictionary<string, object> Params { get; set; }

        public bool IsCommand { get; set; }
    }
}
