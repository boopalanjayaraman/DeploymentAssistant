using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeploymentAssistant.Models;

namespace DeploymentAssistant
{
    /// <summary>
    /// Represents each step (activity) in the execution pipeline
    /// </summary>
    internal class ActivityConfigEntry
    {
        private string _operation = string.Empty;
        /// <summary>
        /// String value of ExecutionType enum.
        /// </summary>
        public string Operation
        {
            get
            {
                return _operation;
            }
            private set
            {
                if (Enum.IsDefined(typeof(ExecutionType), value))
                {
                    _operation = value;
                }
                else
                {
                    throw new ApplicationException("Operation type is not found / implemented.");
                }
            }
        }

        private ExecutionActivity _settings = null;
        /// <summary>
        /// equivalent execution activity object
        /// </summary>
        public ExecutionActivity Settings
        {
            get
            {
                return _settings;
            }
            private set
            {
                if (string.Equals(value.Operation.ToString(), this.Operation, StringComparison.CurrentCultureIgnoreCase))
                {
                    _settings = value;
                }
                else
                {
                    throw new ApplicationException("Setting / Activity does not correspond to the operation type mentioned.");
                }
            }
        }

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="operation">operation string - as per ExecutionType enum.</param>
        /// <param name="activitySetting">setting - equivalent activity object</param>
        public ActivityConfigEntry(string operation, ExecutionActivity settings)
        {
            /// Do not change this assigning order - so the property validations happen in order.
            this.Operation = operation;
            this.Settings = settings;
        }
    }
}
