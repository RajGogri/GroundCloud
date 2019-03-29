using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GroundCloud.Contracts
{
    public static class HelperClass
    {
        /// <summary>
        /// Tos the key value.
        /// </summary>
        /// <returns>The key value.</returns>
        /// <param name="metaToken">Meta token.</param>
        public static IDictionary<string, string> ToKeyValue(this object metaToken)
        {
            if (metaToken == null)
            {
                return null;
            }

            JToken token = metaToken as JToken;
            if (token == null)
            {
                return ToKeyValue(JObject.FromObject(metaToken));
            }

            if (token.HasValues)
            {
                var contentData = new Dictionary<string, string>();
                foreach (var child in token.Children().ToList())
                {
                    var childContent = child.ToKeyValue();
                    if (childContent != null)
                    {
                        contentData = contentData.Concat(childContent)
                            .ToDictionary(k => k.Key, v => v.Value);
                    }
                }

                return contentData;
            }

            var jValue = token as JValue;
            if (jValue?.Value == null)
            {
                return null;
            }

            var value = jValue?.Type == JTokenType.Date ?
                jValue?.ToString("o", CultureInfo.InvariantCulture) :
                jValue?.ToString(CultureInfo.InvariantCulture);

            return new Dictionary<string, string> { { token.Path, value } };
        }

        /// <summary>
        /// Serializes the model to json.
        /// </summary>
        /// <returns>The model to json.</returns>
        /// <param name="obj">Object.</param>
        /// <param name="bodySerialization">Body serialization.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static string SerializeModelToJson<T>(T obj,BodySerialization bodySerialization)
        {
            switch (bodySerialization)
            {
                case BodySerialization.DEFAULT:
                    return JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                case BodySerialization.JSON:
                    return JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                case BodySerialization.XML:
                    var stringwriter = new System.IO.StringWriter();
                    var serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(stringwriter, obj);
                    return stringwriter.ToString();
                case BodySerialization.URL_FORM_ENCODED:
                    var keyValues = obj.ToKeyValue();
                    return new FormUrlEncodedContent(keyValues).ToString();
            }
            return JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }

        /// <summary>
        /// method used to deserialize object 
        /// </summary>
        /// <returns>The from json.</returns>
        /// <param name="json">Json.</param>
        /// <param name="bodySerialization">Body serialization.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T DeserializeFromJson<T>(string json,BodySerialization bodySerialization)
        {
            switch (bodySerialization)
            {
                case BodySerialization.DEFAULT:
                    return JsonConvert.DeserializeObject<T>(json);
                case BodySerialization.JSON:
                    return JsonConvert.DeserializeObject<T>(json);
                case BodySerialization.XML:
                    var stringReader = new System.IO.StringReader(json);
                    var serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(stringReader);
                case BodySerialization.URL_FORM_ENCODED: //TODO
                    return JsonConvert.DeserializeObject<T>(json);

            }
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Gets the type of the serialization.
        /// </summary>
        /// <returns>The serialization type.</returns>
        /// <param name="bodySerialization">Body serialization.</param>
        public static string getSerializationType(BodySerialization bodySerialization)
        {
            switch (bodySerialization)
            {
                case BodySerialization.DEFAULT:
                    return "application/json";
                case BodySerialization.JSON:
                    return "application/json";
                case BodySerialization.XML:
                    return "application/xml";
                case BodySerialization.URL_FORM_ENCODED:
                    return "application/x-www-form-urlencoded";

            }
            return "application/json";
        }
    }
}
