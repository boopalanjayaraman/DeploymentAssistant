using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeploymentAssistant.Common
{
    public static class ExtensionMethods
    {
        const string AppString = "[DeploymentAssistant]: ";

        public static string SurroundLines(this string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                return inputString;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(AppString);
            builder.Append(inputString);
            builder.Append("\r\n");

            return builder.ToString();
        }

        public static bool Compare(this string inputString, string compareString)
        {
            if (string.Equals(inputString.Replace(" ", ""), compareString.Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string ConvertToString(this object inputObject)
        {
            if (inputObject == null)
            {
                return string.Empty;
            }
            else
            {
                return inputObject.ToString();
            }
        }

        public static double ToDouble(this object inputObject)
        {
            IConvertible convert = inputObject as IConvertible;
            if (convert != null && convert != DBNull.Value)
            {
                return convert.ToDouble(null);
            }
            else
            {
                return 0d;
            }
        }

        public static int ToInt(this object inputObject)
        {
            IConvertible convert = inputObject as IConvertible;
            if (convert != null && convert != DBNull.Value)
            {
                return convert.ToInt32(null);
            }
            else
            {
                return 0;
            }
        }

        public static long ToLong(this object inputObject)
        {
            IConvertible convert = inputObject as IConvertible;
            if (convert != null && convert != DBNull.Value)
            {
                return convert.ToInt64(null);
            }
            else
            {
                return 0;
            }
        }

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

        public static void AddRange(this Hashtable src, Hashtable newRange)
        {
            foreach (DictionaryEntry item in newRange)
            {
                src.Add(item.Key, item.Value);
            }
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

        public static DateTime ToEst(this DateTime utcTime)
        {
            DateTime estTime = DateTime.Now;

            try
            {
                TimeZoneInfo estZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                estTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, estZone);
            }
            catch (Exception exception)
            {
                //Logging.LogException(TypeName, exception.Message, exception);
            }
            return estTime;
        }

        public static string ToJson(this object input)
        {
            if (input != null)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(input, Newtonsoft.Json.Formatting.Indented);
            }
            else
            {
                return "{ input: 'null' }";
            }
        }
    }
}
