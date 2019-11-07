using DeploymentAssistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeploymentAssistant.Common;
using log4net;
using log4net.Config;

namespace DeploymentAssistant
{
    class Program
    {
        static void Main(string[] args)
        {
            ILog logger = LogManager.GetLogger(typeof(Program));
            logger.Info("Entered DeploymentAssistant.");

            var mode = args.FirstOrDefault();

            if((mode == null) || (mode.Equals("RunPipeline", StringComparison.CurrentCultureIgnoreCase)))
            {
                logger.Info("Executing the default mode. Mode: RunPipeline.");
                PipelineMaker pipelineMaker = new PipelineMaker();
                pipelineMaker.Load();
            }
            else if(mode.Equals("DumpConfig", StringComparison.CurrentCultureIgnoreCase))
            {
                logger.Info("Executing the config helper mode. Mode: DumpConfig.");
                ConfigMaker configMaker = new ConfigMaker();
                configMaker.Dump();
            }

            logger.Info("Finished the program.");
            Console.Read();
        }
    }
}
