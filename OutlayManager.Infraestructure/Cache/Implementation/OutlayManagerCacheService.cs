using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace OutlayManagerAPI.Infraestructure.Cache.Abstract
{
    internal sealed class OutlayManagerCacheService : IOutlayManagerCache
    {
        private readonly ConcurrentDictionary<string, dynamic> _cache;

        public OutlayManagerCacheService()
        {
            _cache = new ConcurrentDictionary<string, dynamic>();
        }

        public T GetValue<T>(string valueKey) where T:new()
        {
            if(_cache.TryGetValue(valueKey, out dynamic valueSearched))
            {
                if(valueSearched is T valueParsed)
                {   
                    return valueParsed;
                }
                else
                {
                    throw new Exception($"Value {typeof(T)} not match for key {valueKey}");
                }
            }
            else
            {
                return default(T);
            }
        }

        public bool SetValue<T>(string valueKey, T value) where T : new() => _cache.TryAdd(valueKey, value);

        public bool ClearValues(string valueKey) => _cache.TryRemove(valueKey,out dynamic value);

        public IEnumerable<string> KeyValues() => _cache.Keys.AsEnumerable();

        public void ClearAll() => _cache.Clear();
    }
}
