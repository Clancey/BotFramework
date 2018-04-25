using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BotFramework
{
    public class JsonEntityConverter : JsonCreationConverter<Entity>
    {
        public static Dictionary<string, Type> TypeMappings = new Dictionary<string, Type>
        {
            {Place.ContentType,typeof(Place)},
            {GeoCoordinates.ContentType,typeof(GeoCoordinates)},
            {Mention.ContentType,typeof(Mention)},
        };

        protected override Entity Create(System.Type objectType, JObject jsonObject, JsonReader reader)
        {
            string type = "";
            try
            {
                JToken token;
                if (jsonObject.TryGetValue("type", StringComparison.CurrentCultureIgnoreCase, out token))
                {
                    type = token.ToString();
                    Type objType = typeof(Entity);
                    TypeMappings.TryGetValue(type, out objType);
                    return (Entity)Activator.CreateInstance(objType);
                }
                return new Entity();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return new Entity();
        }
    }
}