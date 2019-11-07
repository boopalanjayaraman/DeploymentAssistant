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
        const string executorConfigFileDefault = "DeploymentConfig.json";

        public static string GetExecutorsConfigFile()
        {
            if (string.IsNullOrWhiteSpace(_executorConfigFile))
            {
                var configEntry = ConfigurationManager.AppSettings["ExecutorsConfigFile"];
                if (configEntry == null)
                {
                    _executorConfigFile = executorConfigFileDefault;
                }
                else
                {
                    _executorConfigFile = configEntry.ToString();
                }
            }
            return _executorConfigFile;
        }
    }
}