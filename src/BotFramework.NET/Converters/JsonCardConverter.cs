using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BotFramework
{
	public class JsonCardConverter : JsonCreationConverter<Card>
	{
		public static Dictionary<string, Type> TypeMappings = new Dictionary<string, Type>
		{
			{HeroCard.ContentType,typeof(HeroCard)},
			{ReceiptCard.ContentType,typeof(ReceiptCard)},
			{SigninCard.ContentType,typeof(SigninCard)},
			{ThumbnailCard.ContentType,typeof(ThumbnailCard)},
			{AnimationCard.ContentType,typeof(AnimationCard)},
			{AudioCard.ContentType,typeof(AudioCard)},
			{VideoCard.ContentType,typeof(VideoCard)},
	#if AdaptiveCard
            {AdaptiveCard.ContentType,typeof(AdaptiveCard)},
	#endif
        };
		protected override Card Create(System.Type objectType, JObject jsonObject, JsonReader reader)
		{
			string type = "";
			try
			{
				//This is ugly, but is the best way I could think of to get the type, which resides on the parent object.
				var root = ((reader as JTokenReader).CurrentToken.Root as JObject).SelectToken(reader.Path.Replace(".content", ""));
				JToken token;
				if ((root as JObject).TryGetValue("contentType", StringComparison.CurrentCultureIgnoreCase, out token))
				{
					type = token.ToString();
					Type cardType = typeof(Card);
					TypeMappings.TryGetValue(type, out cardType);
					return (Card)Activator.CreateInstance(cardType);
				}
				return new Card();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			return new Card();
		}
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			try
			{
				// This is ugly, but a few cards dont use json like Alerts...
				if (reader.TokenType == JsonToken.String)
				{
					var card = Create(objectType, null, reader);
					card.Text = (string)new JValue(reader.Value);
					return card;
				}
				//Set the card actions parents
				return SetCardActionsParent(base.ReadJson(reader, objectType, existingValue, serializer) as Card);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			return new Card { Text = reader.Value.ToString() };
		}

		Card SetCardActionsParent(Card card)
		{
			card?.Buttons?.ToList().ForEach(x => x.Parent = card);
			return card;
		}
	}
}
