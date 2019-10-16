using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentAssistant.Models
{
    /// <summary>
    /// represents execution result of a step / activity
    /// </summary>
    public class ExecutionResult
    {
        /// <summary>
        /// Contains error message(s) if any.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// indicates if the execution was successful
        /// </summary>
        public bool IsSuccess { get; set; }


        public ExecutionResult()
        {

        }

        public ExecutionResult(Exception ex)
        {
            this.Message = string.Format("Message: {0}. StackTrace: {1}. InnerException.{2}", ex.Message, ex.StackTrace, ex.InnerException);
            this.IsSuccess = false;
        }

    }
}
