using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BotFramework
{

	public class JsonEntityConverter : JsonCreationConverter<Entity>
	{
		protected override Entity Create (System.Type objectType, JObject jsonObject, JsonReader reader)
		{
			string type = "";
			try {
				JToken token;
				if (jsonObject.TryGetValue ("type", StringComparison.CurrentCultureIgnoreCase, out token)) {
					type = token.ToString ();
					switch (type) {
					case "Place":
						return new Place ();
					case "GeoCoordinates":
						return new GeoCoordinates ();
					}
				}
				return new Entity ();
			} catch (Exception ex) {
				Console.WriteLine (ex);
			}
			return new Entity ();
		}
	}

	[JsonConverter (typeof (JsonEntityConverter))]
	public class Entity
	{
		public string Type { get; set; }

		[JsonExtensionData]
		[JsonProperty (NullValueHandling = NullValueHandling.Ignore)]
		public IDictionary<string, JToken> AdditionalData;
	}

	public class GeoCoordinates : Entity
	{

		[Newtonsoft.Json.JsonProperty ("elevation")]
		public string Elevation { get; set; }

		[Newtonsoft.Json.JsonProperty ("latitude")]
		public string Latitude { get; set; }

		[Newtonsoft.Json.JsonProperty ("longitude")]
		public string Longitude { get; set; }

		[Newtonsoft.Json.JsonProperty ("name")]
		public string Name { get; set; }
	}
	public partial class Place : Entity
	{
		[Newtonsoft.Json.JsonProperty ("address")]
		public string Address { get; set; }

		[Newtonsoft.Json.JsonProperty ("geo")]
		public GeoCoordinates Geo { get; set; }

		[Newtonsoft.Json.JsonProperty ("hasMap")]
		public string HasMap { get; set; }

		[Newtonsoft.Json.JsonProperty ("name")]
		public string Name { get; set; }
	}
}
