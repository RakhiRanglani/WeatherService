using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherBusinessFactory
{
    public class InstanceResolvedEventArgs : EventArgs
    {
        /// <summary>
        /// The Type requested
        /// </summary>
        public Type RequestedType { get; private set; }
        /// <summary>
        /// The instance returned
        /// </summary>
        public object ResolvedInstance { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="requestedType"></param>
        /// <param name="resolvedInstance"></param>
        public InstanceResolvedEventArgs(Type requestedType, object resolvedInstance)
        {
            RequestedType = requestedType;
            ResolvedInstance = resolvedInstance;
        }
    }
}
