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
        /// Executor provider 
        /// </summary>
        private IExecutorProvider _executorProvider = null;

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
        internal ExecutionPipeline(IExecutorProvider executorProvider)
        {
            if (this.Steps == null)
            {
                this.Steps = new List<ExecutionActivity>();
            }
            this._executorProvider = executorProvider;
        }

        /// <summary>
        /// Creates an instance of the pipeline for client 
        /// </summary>
        /// <returns></returns>
        public static ExecutionPipeline GetExecutionPipeline()
        {
            return new ExecutionPipeline(new ExecutorProvider());
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
                if (StepAdded != null)
                {
                    StepAdded(this, new ActivityEventArgs() { Activity = step });
                }
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
            if ((step.Host == null) || string.IsNullOrWhiteSpace(step.Host.HostName))
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
                var executor = _executorProvider.GetExecutor(activityStep);
                //// publish event - for starting the step
                if (StepStarted != null)
                {
                    StepStarted(this, new ActivityEventArgs() { Activity = activityStep });
                }
                //// execute the step
                executor.Execute();
                logger.Info("Step Execution was done.");
                //// verify the execution
                executor.Verify();
                logger.Info("Step verification was done.");
                //// TODO: do the common error handling here. Throw exceptions inside the class.
                var result = executor.Result;
                //// publish event - for completing the step
                if (StepCompleted != null)
                {
                    StepCompleted(this, new ActivityEventArgs() { Activity = activityStep, Result = result });
                }
                //// check the ContinueOnError flag and decide the flow based on that.
                if (!activityStep.ContinueOnFailure)
                {
                    //// if result is failure, break.
                    if (!result.IsSuccess)
                    {
                        logger.Info("Activity ContinueOnFailure flag is set to false, and result was failure. Hence, breaking.");
                        logger.Info("Pipeline ends.");
                        break;
                    }
                }
            }
            logger.Info("Pipeline execution completed.");
        }
    }
}
