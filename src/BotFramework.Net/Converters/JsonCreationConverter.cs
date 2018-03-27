using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BotFramework
{
	public abstract class JsonCreationConverter<T> : JsonConverter
	{
		protected abstract T Create (System.Type objectType, JObject jsonObject, JsonReader reader);

		public override bool CanConvert (System.Type objectType)
		{
			return typeof(T).IsAssignableFrom (objectType);
		}

		public override object ReadJson (JsonReader reader, System.Type objectType,
		  object existingValue, JsonSerializer serializer)
		{
			var jsonObject = JObject.Load (reader);
			var target = Create (objectType, jsonObject, reader);
			serializer.Populate (jsonObject.CreateReader (), target);
			return target;
		}
		public override bool CanWrite => false;

		public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException ();
		}

	}
}
