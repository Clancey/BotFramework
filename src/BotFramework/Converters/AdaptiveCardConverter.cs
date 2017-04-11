using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
namespace BotFramework
{
	public class AdaptiveCardConverter : JsonCreationConverter<AdaptiveCardWrapper>
	{

		protected override AdaptiveCardWrapper Create (System.Type objectType, JObject jsonObject, JsonReader reader)
		{
			return new AdaptiveCardWrapper();
		}

		public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			try {
				// This is ugly, but a few cards dont use json like Alerts...
				var card = base.ReadJson (reader, objectType, existingValue, serializer) as AdaptiveCards.AdaptiveCard;
				return new AdaptiveCardWrapper { Card = card };
			} catch (Exception ex) {
				Console.WriteLine (ex);
			}
			return new Card { Text = reader.Value.ToString () };
		}

	}
}
