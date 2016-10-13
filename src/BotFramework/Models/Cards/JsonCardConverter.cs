using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BotFramework
{
	public class JsonCardConverter : JsonCreationConverter<Card>
	{
		protected override Card Create (System.Type objectType, JObject jsonObject, JsonReader reader)
		{
			try {
				//This is ugly, but is the best way I could think of to get the type, which resides on the parent object.
				var root = ((reader as JTokenReader).CurrentToken.Root as JObject).SelectToken(reader.Path.Replace(".content",""));
				JToken token;
				if ((root as JObject).TryGetValue ("contentType", StringComparison.CurrentCultureIgnoreCase, out token)) {
					var type = token.ToString ();
					switch (type) {
					case HeroCard.ContentType:
						return new HeroCard ();
					case ReceiptCard.ContentType:
						return new ReceiptCard ();
					case SigninCard.ContentType:
						return new SigninCard ();
					case ThumbnailCard.ContentType:
						return new ThumbnailCard ();
					}
				}
				return new Card ();
			} catch (Exception ex) {
				Console.WriteLine (ex);
			}
			return new Card ();
		}
	}
}
