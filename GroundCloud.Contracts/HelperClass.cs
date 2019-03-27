using System;
using Newtonsoft.Json;

namespace GroundCloud.Contracts
{
    public class HelperClass
    {
        public HelperClass()
        {
        }
        public static string SerializeModelToJson(object obj)
        {
            try
            {
                //SetValidUserId(obj);
                return JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            }
            catch
            {
                return string.Empty;
            }
        }

        public static T DeserializeFromJson<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return default(T);
            }
        }
    }
}
