using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherBusinessFactory
{
    public class InstanceResolvingEventArgs : EventArgs
    {
        /// <summary>
        /// The Type requested
        /// </summary>
        public Type RequestedType { get; private set; }

        /// <summary>
        /// The suggested result for the request
        /// </summary>
        public object SuggestedResult
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="requestedType"></param>
        public InstanceResolvingEventArgs(Type requestedType)
        {
            RequestedType = requestedType;
        }

        /// <summary>
        /// Attempts to suggests a result for the requested type
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TrySuggestResult(object result)
        {
            if (result != null && result.GetType().IsAssignableFrom(RequestedType))
            {
                SuggestedResult = result;
                return true;
            }
            return false;
        }
    }
    }
