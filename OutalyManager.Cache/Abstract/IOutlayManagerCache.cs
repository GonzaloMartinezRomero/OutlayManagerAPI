using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutalyManager.Cache.Abstract
{
    public interface IOutlayManagerCache
    {
        /// <summary>
        /// Returns value in cache. NULL if not found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueKey"></param>
        /// <returns></returns>
        T GetValue<T>(string valueKey) where T : new();

        /// <summary>
        /// Asign a value to cache matched to key. Update if exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueKey"></param>
        /// <param name="value"></param>
        bool SetValue<T>(string valueKey, T value) where T : new();

        bool ClearValues(string valueKey);

        void ClearAll();

        /// <summary>
        /// Return all key values allocated in cache
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> KeyValues();
    }
}
