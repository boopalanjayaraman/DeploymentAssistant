using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeploymentAssistant.Common
{
    /// <summary>
    /// Contains some helper extension methods 
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Converts a string value to boolean
        /// </summary>
        /// <param name="inputString">input string</param>
        /// <returns></returns>
        public static bool ToBoolean(this string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                return false;
            }

            if (string.Equals(inputString, bool.TrueString, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// Compares if the dictionary is same as the target. Check with every key and value combination in the dictionary
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The source item that needs to be compared.</param>
        /// <param name="target">The dictionary needs to be compared against.</param>
        /// <returns></returns>
        public static bool Equal<TKey, TValue>(this IDictionary<TKey, TValue> source, IDictionary<TKey, TValue> target)
        {
            if (source == target) return true;
            if ((source == null) || (target == null)) return false;
            if (source.Count != target.Count) return false;

            var comparer = EqualityComparer<TValue>.Default;
            foreach (KeyValuePair<TKey, TValue> kvp in source)
            {
                TValue secondValue;
                if (!target.TryGetValue(kvp.Key, out secondValue)) return false;
                if (!comparer.Equals(kvp.Value, secondValue)) return false;
            }
            return true;
        }

        /// <summary>
        /// Converts the object into a json string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToJson(this object input)
        {
            if (input != null)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                return Newtonsoft.Json.JsonConvert.SerializeObject(input, Newtonsoft.Json.Formatting.Indented, settings);
            }
            else
            {
                return "{ input: 'null' }";
            }
        }
    }
}
