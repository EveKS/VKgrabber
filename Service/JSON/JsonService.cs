using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VkGroupManager.Service.JSON
{
    public class JsonService : IJsonService
    {
        T IJsonService.JsonConvertDeserializeObjectWithNull<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
            }
            catch
            {
                return default(T);
            }
        }

        T IJsonService.JsonConvertDeserializeObject<T>(string json)
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
