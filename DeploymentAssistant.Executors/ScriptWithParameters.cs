using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Executors
{
    /// <summary>
    /// Class to hold script block and its associated parameters as objects
    /// </summary>
    internal class ScriptWithParameters
    {
        public string Script { get; set; }

        public List<object> Params { get; set; }
    }
}
