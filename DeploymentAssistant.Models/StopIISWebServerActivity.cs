using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Models
{
    /// <summary>
    /// represents Stopping the IIS web server activity
    /// </summary>
    public class StopIISWebServerActivity : ExecutionActivity
    {

        /// <summary>
        /// activity type
        /// </summary>
        public override ExecutionType Operation
        {
            get
            {
                return ExecutionType.StopIISWebServer;
            }
        }

        public StopIISWebServerActivity()
        {

        }
    }
}
