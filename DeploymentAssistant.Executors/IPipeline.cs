using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeploymentAssistant.Models;


namespace DeploymentAssistant.Executors
{
    /// <summary>
    /// Interface that represents the pipeline of activities / deployment steps
    /// </summary>
    public interface IPipeline
    {
        /// <summary>
        /// represents each step in the pipeline
        /// </summary>
        List<ExecutionActivity> Steps { get; }

        /// <summary>
        /// represents each step's result in the pipeline
        /// </summary>
        List<ExecutionResult> Results { get; }

        /// <summary>
        /// event for each step starting to execute.
        /// </summary>
        event EventHandler StepStarted;

        /// <summary>
        /// event for each step completed
        /// </summary>
        event EventHandler StepCompleted;

        /// <summary>
        /// event for each step added
        /// </summary>
        event EventHandler StepAdded;

        /// <summary>
        /// Adding a step to pipeline
        /// </summary>
        /// <param name="step"></param>
        void Add(ExecutionActivity step);

        /// <summary>
        /// Execute pipeline
        /// </summary>
        void Run();
    }
}
