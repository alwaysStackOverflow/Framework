using Newtonsoft.Json;

namespace Common
{
    public static class JsonHelper
    {
        private static readonly JsonSerializerSettings _settings = new() { TypeNameHandling = TypeNameHandling.Auto };
        public static string Serialize(this object obj)
        {
			return JsonConvert.SerializeObject(obj, _settings);
		}

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _settings);
		}
    }
}
