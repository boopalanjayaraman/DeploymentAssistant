using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeploymentAssistant.Common
{
    public class Constants
    {
        public class PowershellScripts
        {
            public const string StartServiceCall = @"StartService {0}";
            public const string StopServiceCall = @"StopService {0}";

            public const string VerifyStartServiceCall = @"VerifyStartService {0}";
            public const string VerifyStopServiceCall = @"VerifyStopService {0}";

            public const string ScriptsFolder = "Scripts";
        } 

    }
}
