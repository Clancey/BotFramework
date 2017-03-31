using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BotFramework
{

	public class JsonActivityConverter : JsonCreationConverter<BotActivity>
	{
		public static Dictionary<ActivityType, Type> TypeMappings = new Dictionary<ActivityType, Type>
		{
			{Message.ContentType,typeof(Message)},
		};

		protected override BotActivity Create (System.Type objectType, JObject jsonObject, JsonReader reader)
		{
			try {
				JToken token;
				if (jsonObject.TryGetValue ("type", StringComparison.CurrentCultureIgnoreCase, out token)) {
					var type = (ActivityType)Enum.Parse (typeof (ActivityType), token.ToString (), true);
					Type objType = typeof (Entity);
					TypeMappings.TryGetValue (type, out objType);
					return (BotActivity)Activator.CreateInstance (objType);
				}
				return new BotActivity ();
			} catch (Exception ex) {
				Console.WriteLine (ex);
			}
			return new BotActivity ();
		}
	}
}
