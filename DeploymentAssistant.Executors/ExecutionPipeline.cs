using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeploymentAssistant.Models;
using log4net;
using DeploymentAssistant.Common;

namespace DeploymentAssistant.Executors
{
    /// <summary>
    /// Represents the execution pipeline of the deployment process
    /// </summary>
    public class ExecutionPipeline : IPipeline
    {
        ILog logger = LogManager.GetLogger(typeof(ExecutionPipeline));
        /// <summary>
        /// represents each step's result in the pipeline
        /// </summary>
        public List<ExecutionResult> Results
        {
            get;
            protected set;
        }

        /// <summary>
        /// represents each step in the pipeline
        /// </summary>  
        public List<ExecutionActivity> Steps
        {
            get;
            protected set;
        }

        /// <summary>
        /// event for each step added
        /// </summary>
        public event EventHandler StepAdded;

        /// <summary>
        /// event for each step completed
        /// </summary>
        public event EventHandler StepCompleted;

        /// <summary>
        /// event for each step starting to execute.
        /// </summary>
        public event EventHandler StepStarted;

        /// <summary>
        /// constructor
        /// </summary>
        public ExecutionPipeline()
        {
            if (this.Steps == null)
            {
                this.Steps = new List<ExecutionActivity>();
            }
        }

        /// <summary>
        /// Adding a step to pipeline
        /// </summary>
        /// <param name="step"></param>
        public void Add(ExecutionActivity step)
        {
            ValidateBasics(step);
            bool isValid = step.IsValid();
            if (isValid)
            {
                this.Steps.Add(step);
                StepAdded(this, new ActivityEventArgs() { Activity = step });
            }
            else
            {
                throw new ApplicationException("Pipeline Validation Failure: Activity Validation Failed.");
            }
        }

        private void ValidateBasics(ExecutionActivity step)
        {
            if (step == null)
            {
                throw new ApplicationException("Pipeline Validation Failure: Activity is null.");
            }
            logger.InfoFormat("Validating step: {0}", step.Name);
            if (string.IsNullOrWhiteSpace(step.Host.HostName))
            {
                throw new ApplicationException("Pipeline Validation Failure: Host details are unavailable.");
            }
        }

        /// <summary>
        /// Execute pipeline
        /// </summary>
        public void Run()
        {
            logger.Info("Pipeline execution starts.");
            foreach (var activityStep in this.Steps)
            {
                logger.InfoFormat("initiating step: {0}", activityStep.Name);
                logger.InfoFormat("Activity Details: {0}", activityStep.ToJson());
                var executor = ExecutorProvider.GetExecutor(activityStep);
                //// publish event - for starting the step
                StepStarted(this, new ActivityEventArgs() { Activity = activityStep });
                //// execute the step
                executor.Execute();
                logger.Info("Step Execution was done.");
                //// verify the execution
                executor.Verify();
                logger.Info("Step verification was done.");
                //// TODO: do the common error handling here. Throw exceptions inside the class.
                var result = executor.Result;
                //// publish event - for completing the step
                StepCompleted(this, new ActivityEventArgs() { Activity = activityStep, Result = result });
            }
            logger.Info("Pipeline execution completed.");
        }
    }
}
