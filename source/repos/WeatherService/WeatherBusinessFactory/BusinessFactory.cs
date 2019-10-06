
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using System.Reflection;


namespace WeatherBusinessFactory
{
    public static class BusinessFactory
    {
        /// <summary>
            /// The container
            /// </summary>
            public readonly static IServiceLocator container = new Container("DataLayer");
            /// <summary>
            /// The sender
            /// </summary>
            private readonly static Type sender = typeof(BusinessFactory);
            /// <summary>
            /// Occurs when [instance resolved].
            /// </summary>
            internal static event EventHandler<InstanceResolvedEventArgs> InstanceResolved;
            /// <summary>
            /// Occurs when [instance resolving].
            /// </summary>
            internal static event EventHandler<InstanceResolvingEventArgs> InstanceResolving;

            /// <summary>
            /// Gets the component.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public static T GetComponent<T>()
            {
                object suggested = RaiseResolvingEvent(typeof(T));
                T result;
                if (suggested == null)
                {
                    result = container.GetInstance<T>();
                }
                else
                {
                    result = (T)suggested;
                }
                RaiseResolvedEvent(typeof(T), result);
                return result;
            }

            /// <summary>
            /// Gets the component.
            /// </summary>
            /// <param name="type">The type.</param>
            /// <returns></returns>
            /// <exception cref="System.ArgumentNullException">type</exception>
            public static object GetComponent(Type type)
            {
                object result = RaiseResolvingEvent(type);
                if (type == null)
                {
                    throw new ArgumentNullException("type");
                }
                if (result == null)
                {
                    result = container.GetInstance(type);
                }
                RaiseResolvedEvent(type, result);
                return result;

            }

            /// <summary>
            /// Gets the component.
            /// </summary>
            /// <param name="typeName">Name of the type.</param>
            /// <returns></returns>
            public static object GetComponent(string typeName)
            {
                Type type = Type.GetType(typeName);
                return GetComponent(type);
            }

            /// <summary>
            /// Raises the resolved event.
            /// </summary>
            /// <param name="requestedType">Type of the requested.</param>
            /// <param name="resolvedInstance">The resolved instance.</param>
            private static void RaiseResolvedEvent(Type requestedType, object resolvedInstance)
            {
                if (InstanceResolved != null)
                {
                    InstanceResolved(sender, new InstanceResolvedEventArgs
                        (requestedType, resolvedInstance));
                }
            }

            /// <summary>
            /// Raises the resolving event.
            /// </summary>
            /// <param name="requestedType">Type of the requested.</param>
            /// <returns></returns>
            private static object RaiseResolvingEvent(Type requestedType)
            {
                if (InstanceResolving != null)
                {
                    var args = new InstanceResolvingEventArgs(requestedType);
                    InstanceResolving(sender, args);
                    return args.SuggestedResult;
                }
                return null;
            }

        }
    }

