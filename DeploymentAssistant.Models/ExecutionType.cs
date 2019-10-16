using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Models
{
    /// <summary>
    /// The type of execution activity
    /// </summary>
    public enum ExecutionType
    {
        StopService = 1,
        StartService,
        CopyFiles,
        MoveFiles,
        DeleteFiles,
        StopIISWebServer,
        StartIISWebServer,
        StopIISWebsite,
        StartIISWebsite,
        CreateIISWebsite,
        AddSslCertificate
    }
}
