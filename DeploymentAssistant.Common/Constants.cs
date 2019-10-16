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
            public const string StartService = @"Start-Service {0}";
            public const string StopService = @"Stop-Service {0}";
            public const string GetService = @"GetServiceStatus {0}";
            public const string GetServiceFunction = @"
            function GetServiceStatus()
            {{
                param([String]$serviceName)

                $status = Get-Service $serviceName
                return $status.Status
            }}
            ";
            public const string RunningStatus = "Running";
            public const string StoppedStatus = "Stopped";
        } 

    }
}
