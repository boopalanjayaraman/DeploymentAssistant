using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Models
{
    /// <summary>
    /// Represents a full pipeline configuration
    /// This will be probably loaded with information read from a json file
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Name of the configuration - ex., machine name, instance name - prod / qa / uat, etc.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Steps / Activities in the pipeline to be executed
        /// </summary>
        public List<ExecutionActivity> Steps { get; set; }

        public Configuration()
        {

        }
    }
}
