using DeploymentAssistant.Common;
using DeploymentAssistant.Executors;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant
{
    /// <summary>
    /// Creates a deployment execution pipeline by loading a configuration file. 
    /// </summary>
    internal class PipelineMaker
    {
        ILog logger = LogManager.GetLogger(typeof(PipelineMaker));

        /// <summary>
        /// FileOperations 
        /// </summary>
        public IFileOperations FileOperations { get; private set; }

        public IPipeline Pipeline { get; private set; }

        /// <summary>
        /// constructor
        /// </summary>
        public PipelineMaker(IFileOperations fileOperations, IPipeline pipeline)
        {
            //// Initialize
            this.FileOperations = fileOperations;
            this.Pipeline = pipeline;
        }

        /// <summary>
        /// Loads config and creates pipeline
        /// </summary>
        /// <param name="activityConfigurationFile">path of configuration file containing activity entries</param>
        public void Load(string activityConfigurationFile = "")
        {
            logger.Info("Starting pipeline - config load operation.");
            if (string.IsNullOrWhiteSpace(activityConfigurationFile))
            {
                activityConfigurationFile = ConfigurationParser.GetExecutorsConfigFile();
            }

            //// If file name does not end with json extension, make it one.
            if (!activityConfigurationFile.EndsWith(".json", StringComparison.CurrentCultureIgnoreCase))
            {
                activityConfigurationFile += ".json";
            }

            if (!FileOperations.Exists(activityConfigurationFile))
            {
                logger.Info("Config file with given name / path does not exist.");
                return;
            }
            //// Load config
            var configJson = FileOperations.ReadAllText(activityConfigurationFile);
            JsonSerializerSettings settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
            List<ActivityConfigEntry> activities = JsonConvert.DeserializeObject<List<ActivityConfigEntry>>(configJson, settings);

            //// Create pipeline
            this.Pipeline.StepStarted += pipeline_StepStarted;
            this.Pipeline.StepCompleted += pipeline_StepCompleted;

            foreach(var executionStep in activities)
            {
                var activity = executionStep.Settings;
                this.Pipeline.Add(activity);
            }
            logger.Info("Pipeline is created and loaded with settings.");

            //// Run the pipeline
            this.Pipeline.Run();
        }

        void pipeline_StepCompleted(object sender, EventArgs e)
        {
            //// Do nothing for now.
        }

        void pipeline_StepStarted(object sender, EventArgs e)
        {
            //// Do nothing for now.
        }
    }
}
