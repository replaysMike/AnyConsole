using System.Collections.Generic;

namespace AnyConsole
{
    /// <summary>
    /// The data context stores data that can be used by custom Component's to transfer information
    /// </summary>
    public class ConsoleDataContext
    {
        /// <summary>
        /// The data store
        /// </summary>
        private readonly IDictionary<string, object> _untypedData = new Dictionary<string, object>();

        private static readonly ConsoleDataContext _instance = new ConsoleDataContext();

        /// <summary>
        /// A convienience singleton instance of the data context.
        /// It is preferred you create your data contexts and bubble them up your IoC chain, however
        /// this is an alternative if not using IoC.
        /// </summary>
        public static ConsoleDataContext Instance
        {
            get
            {
                return _instance;
            }
        }

        static ConsoleDataContext() { }

        /// <summary>
        /// Set data in the context using a key
        /// </summary>
        /// <param name="key">Key to use for the data</param>
        /// <param name="data"></param>
        public void SetData(string key, object data)
        {
            //CallContext.SetData(key, data);
            if (!_untypedData.ContainsKey(key))
                _untypedData.Add(key, data);
            else
                _untypedData[key] = data;
        }

        /// <summary>
        /// Set data in the context using a key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Key to use for the data</param>
        /// <param name="data"></param>
        public void SetData<T>(string key, T data)
        {
            //CallContext<T>.SetData(key, data);
            if (!_untypedData.ContainsKey(key))
                _untypedData.Add(key, data);
            else
                _untypedData[key] = data;
        }

        /// <summary>
        /// Get data from the context
        /// </summary>
        /// <param name="key">Key to use for the data</param>
        /// <returns></returns>
        public object GetData(string key)
        {
            //return CallContext.GetData(key);
            if (_untypedData.ContainsKey(key))
                return _untypedData[key];
            return null;
        }

        /// <summary>
        /// Get data from the context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Key to use for the data</param>
        /// <returns></returns>
        public T GetData<T>(string key)
        {
            if (_untypedData.ContainsKey(key))
                return (T)_untypedData[key];
            return default(T);
        }
    }
}
