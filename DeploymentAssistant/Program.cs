using DeploymentAssistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeploymentAssistant.Common;

namespace DeploymentAssistant
{
    class Program
    {
        static void Main(string[] args)
        {
            StopServiceActivity stopService = new StopServiceActivity();
            stopService.Name = "Stop Redis";
            stopService.Host = new HostInfo();
            stopService.Host.HostName = "PTPLL258";
            stopService.Order = 1;
            stopService.ServiceName = "rediscacheservice";

            Console.WriteLine(stopService.ToJson());
            Console.Read();
        }
    }
}
