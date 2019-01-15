using System.Collections.Concurrent;
using System.Threading;
#if FEATURE_ASYNCLOCAL
#else
using System.Runtime.Remoting.Messaging;
#endif

namespace AnyConsole
{
    /// <summary>
    /// Provides a way to set contextual data that flows with the call and 
    /// async context of a test or invocation.
    /// </summary>
    public static class CallContext
    {
#if FEATURE_ASYNCLOCAL
        static ConcurrentDictionary<string, AsyncLocal<object>> state = new ConcurrentDictionary<string, AsyncLocal<object>>();
#endif
        /// <summary>
        /// Stores a given object and associates it with the specified name.
        /// </summary>
        /// <param name="name">The name with which to associate the new item in the call context.</param>
        /// <param name="data">The object to store in the call context.</param>
        public static void SetData(string name, object data)
        {
#if FEATURE_ASYNCLOCAL
            state.GetOrAdd(name, _ => new AsyncLocal<object>()).Value = data;
#else
            System.Runtime.Remoting.Messaging.CallContext.SetData(name, data);
#endif
        }

        /// <summary>
        /// Retrieves an object with the specified name from the <see cref="CallContext"/>.
        /// </summary>
        /// <param name="name">The name of the item in the call context.</param>
        /// <returns>The object in the call context associated with the specified name, or <see langword="null"/> if not found.</returns>
        public static object GetData(string name)
        {
#if FEATURE_ASYNCLOCAL
            return state.TryGetValue(name, out AsyncLocal<object> data) ? data.Value : null;
#else
            return System.Runtime.Remoting.Messaging.CallContext.GetData(name);
#endif
        }
    }

    /// <summary>
    /// Provides a way to set contextual data that flows with the call and 
    /// async context of a test or invocation.
    /// </summary>
    public static class CallContext<T>
    {
#if FEATURE_ASYNCLOCAL
        static ConcurrentDictionary<string, AsyncLocal<T>> state = new ConcurrentDictionary<string, AsyncLocal<T>>();
#endif

        /// <summary>
        /// Stores a given object and associates it with the specified name.
        /// </summary>
        /// <param name="name">The name with which to associate the new item in the call context.</param>
        /// <param name="data">The object to store in the call context.</param>
        public static void SetData(string name, T data)
        {
#if FEATURE_ASYNCLOCAL
            state.GetOrAdd(name, _ => new AsyncLocal<T>()).Value = data;
#else
            System.Runtime.Remoting.Messaging.CallContext.SetData(name, data);
#endif
        }

        /// <summary>
        /// Retrieves an object with the specified name from the <see cref="CallContext"/>.
        /// </summary>
        /// <typeparam name="T">The type of the data being retrieved. Must match the type used when the <paramref name="name"/> was set via <see cref="SetData{T}(string, T)"/>.</typeparam>
        /// <param name="name">The name of the item in the call context.</param>
        /// <returns>The object in the call context associated with the specified name, or a default value for <typeparamref name="T"/> if none is found.</returns>
        public static T GetData(string name)
        {
#if FEATURE_ASYNCLOCAL
            return state.TryGetValue(name, out AsyncLocal<T> data) ? data.Value : default(T);
#else
            return (T)System.Runtime.Remoting.Messaging.CallContext.GetData(name);
#endif
        }
    }
}
