using DeploymentAssistant.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Executors
{
    /// <summary>
    /// Abstract base implementation of an executor
    /// </summary>
    internal abstract class AbstractExecutor : IExecutor
    {
        /// <summary>
        /// Activity object in context
        /// </summary>
        public ExecutionActivity Activity { get; protected set; }

        /// <summary>
        /// Execution step's result
        /// </summary>
        public ExecutionResult Result { get; protected set; }

        protected IShellManager _shellManager = null;
        protected ILog logger = null;
        protected Exception _lastException = null;
        protected bool quitExecuting = false;

        /// <summary>
        /// Execute step method
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Execute verifying method after the action has been done
        /// </summary>
        public abstract void Verify();

        /// <summary>
        /// Handle exception
        /// </summary>
        /// <param name="appEx">application exception</param>
        /// <param name="activity">activity in context</param>
        protected void HandleException(ApplicationException appEx, ExecutionActivity activity)
        {
            _lastException = appEx;
            if (!activity.ContinueOnFailure)
            {
                throw appEx;
            }
            else
            {
                quitExecuting = true;
                this.Result = new ExecutionResult() { IsSuccess = false, Message = appEx.Message };
            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="shellManager"></param>
        public AbstractExecutor(ExecutionActivity activity, IShellManager shellManager)
        {
            this.Activity = activity;
            _shellManager = shellManager;
        }
    }
}
