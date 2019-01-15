using System;
using System.Collections.Generic;

namespace AnyConsole
{
    public class ConsoleDataContext
    {
        private IDictionary<string, object> _untypedData = new Dictionary<string, object>();

        public void SetData(string key, object data)
        {
            //CallContext.SetData(key, data);
            if(!_untypedData.ContainsKey(key))
                _untypedData.Add(key, data);
            else
                _untypedData[key] = data;
        }

        public void SetData<T>(string key, T data)
        {
            //CallContext<T>.SetData(key, data);
            if (!_untypedData.ContainsKey(key))
                _untypedData.Add(key, data);
            else
                _untypedData[key] = data;
        }

        public object GetData(string key)
        {
            //return CallContext.GetData(key);
            if (_untypedData.ContainsKey(key))
                return _untypedData[key];
            return null;
        }

        public T GetData<T>(string key)
        {
            if (_untypedData.ContainsKey(key))
                return (T)_untypedData[key];
            return default(T);
        }
    }
}
