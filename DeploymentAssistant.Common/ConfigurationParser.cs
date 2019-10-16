using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DeploymentAssistant.Common
{
    public static class ConfigurationParser
    {
        static string _authUserName = string.Empty;
        static string _authPassword = string.Empty;

        public static string GetAuthUserName()
        {
            if (string.IsNullOrWhiteSpace(_authUserName))
            {
                _authUserName = ConfigurationManager.AppSettings["AuthUserName"];
            }
            return _authUserName;
        }

        public static string GetAuthPassword()
        {
            if (string.IsNullOrWhiteSpace(_authPassword))
            {
                _authPassword = ConfigurationManager.AppSettings["AuthPassword"];
            }
            return _authPassword;
        }
    }
}