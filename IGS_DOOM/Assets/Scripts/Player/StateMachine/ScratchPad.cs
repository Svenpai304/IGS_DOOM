using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class ScratchPad
    {
        private Dictionary<string, object> data = new();

        public T Get<T>(string key)
        {
            if (data.ContainsKey(key))
            {
                return (T)data[key];
            }
            return default;
        }

        public void Set<T>(string key, T value)
        {
            data[key] = value;
        }

        public void LogElements()
        {
            foreach (var VARIABLE in data)
            {
                Debug.Log(VARIABLE);
            }
        }
    }
}
