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
        const int args_index_mode = 0;
        const int args_index_file = 1;

        static void Main(string[] args)
        {
            ILog logger = LogManager.GetLogger(typeof(Program));
            logger.Info("Entered DeploymentAssistant.");

            var mode = args.FirstOrDefault();

            if((mode == null) || (mode.Equals("RunPipeline", StringComparison.CurrentCultureIgnoreCase)))
            {
                logger.Info("Executing the default mode. Mode: RunPipeline.");
                var fileName = string.Empty;
                if(args.ElementAtOrDefault(args_index_file) != null)
                {
                    fileName = args.ElementAt(args_index_file);
                }
                //// Call pipeline Maker
                PipelineMaker pipelineMaker = new PipelineMaker();
                pipelineMaker.Load(fileName);
            }
            else if(mode.Equals("DumpConfig", StringComparison.CurrentCultureIgnoreCase))
            {
                logger.Info("Executing the config helper mode. Mode: DumpConfig.");
                var fileName = string.Empty;
                if (args.ElementAtOrDefault(args_index_file) != null)
                {
                    fileName = args.ElementAt(args_index_file);
                }
                //// Call config Maker
                ConfigMaker configMaker = new ConfigMaker();
                configMaker.Dump(fileName);
            }

            logger.Info("Finished the program.");
            Console.Read();
        }
    }
}
