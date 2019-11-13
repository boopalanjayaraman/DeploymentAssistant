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
            public const string StartServiceCall = @"StartService";
            public const string StopServiceCall = @"StopService";
            public const string CopyFilesCall = @"CopyFiles";
            public const string MoveFilesCall = @"MoveFiles";
            public const string DeleteFilesCall = @"DeleteFiles";
            public const string StopIISWebServerCall = @"StopIISWebServer";
            public const string StartIISWebServerCall = @"StartIISWebServer";
            public const string StopIISWebsiteCall = @"StopIISWebsite";
            public const string StartIISWebsiteCall = @"StartIISWebsite";
            public const string CreateIISWebsiteCall = @"CreateIISWebsite";
            public const string AddSslCertificateCall = @"AddSslCertificate";

            public const string VerifyStartServiceCall = @"VerifyStartService";
            public const string VerifyStopServiceCall = @"VerifyStopService";
            public const string VerifyMoveFilesCall = @"VerifyMoveFiles";
            public const string VerifyDeleteFilesCall = @"VerifyDeleteFiles";
            public const string VerifyStopIISWebsiteCall = @"VerifyStopIISWebsite";
            public const string VerifyStartIISWebsiteCall = @"VerifyStartIISWebsite";
            public const string VerifyCreateIISWebsiteCall = @"VerifyCreateIISWebsite";
            public const string VerifyAddSslCertificateCall = @"VerifyAddSslCertificate";

            public const string ScriptsFolder = "Scripts";
        } 

    }
}
