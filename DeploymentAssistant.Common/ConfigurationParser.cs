using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DeploymentAssistant.Common
{
    public static class ConfigurationParser
    {
        static string _executorConfigFile = string.Empty;

        public static string GetExecutorsConfigFile()
        {
            if (string.IsNullOrWhiteSpace(_executorConfigFile))
            {
                _executorConfigFile = ConfigurationManager.AppSettings["ExecutorsConfigFile"];
            }
            return _executorConfigFile;
        }
    }
}