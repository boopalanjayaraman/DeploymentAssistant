using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Models
{
    /// <summary>
    /// represents starting the IIS web server activity
    /// </summary>
    public class StartIISWebServerActivity : ExecutionActivity
    {

        /// <summary>
        /// activity type
        /// </summary>
        public override ExecutionType Operation
        {
            get
            {
                return ExecutionType.StartIISWebServer;
            }
        }

        public StartIISWebServerActivity()
        {

        }
    }
}
