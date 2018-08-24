using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace Hmj.ScheduleService.Code
{
    public class JsonHelper
    {
        public static string SerializeObject(object o)
        {

            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            //这里使用自定义日期格式，如果不使用的话，默认是ISO8601格式     
            timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters = new List<JsonConverter> { timeConverter };
            settings.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(o, Formatting.None, settings);
        }

        public static T DeserializeObject<T>(string json) where T : class
        {
            T t = JsonConvert.DeserializeObject<T>(json);
            return t;
        }

        public static List<T> DeserializeObjectList<T>(string json) where T : class
        {
            var list = JsonConvert.DeserializeObject<List<T>>(json);
            return list;
        }

        public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject) 
        {
            T t = JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject);
            return t;
        }
    }
}
