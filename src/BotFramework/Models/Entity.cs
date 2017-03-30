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
					type = token.ToString ().ToLower();
					switch (type) {
					case "place":
						return new Place ();
					case "geocoordinates":
						return new GeoCoordinates ();
					case "mention":
						return new Mention ();
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
		/// <summary>
		/// Gets or sets entity Type (typically from schema.org types)
		/// </summary>
		public string Type { get; set; }


		/// <summary>
		/// Gets or sets the name of the thing
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "name")]
		public string Name { get; set; }

		[JsonExtensionData]
		[JsonProperty (NullValueHandling = NullValueHandling.Ignore)]
		public IDictionary<string, JToken> AdditionalData;
	}

	/// <summary>
	/// GeoCoordinates (entity type: "https://schema.org/GeoCoordinates")
	/// </summary>
	public class GeoCoordinates : Entity
	{
		/// <summary>
		/// Gets or sets elevation of the location [WGS
		/// 84](https://en.wikipedia.org/wiki/World_Geodetic_System)
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "elevation")]
		public double? Elevation { get; set; }

		/// <summary>
		/// Gets or sets latitude of the location [WGS
		/// 84](https://en.wikipedia.org/wiki/World_Geodetic_System)
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "latitude")]
		public double? Latitude { get; set; }

		/// <summary>
		/// Gets or sets longitude of the location [WGS
		/// 84](https://en.wikipedia.org/wiki/World_Geodetic_System)
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "longitude")]
		public double? Longitude { get; set; }

	}

	/// <summary>
	/// Place (entity type: "https://schema.org/Place")
	/// </summary>
	public partial class Place : Entity
	{
		/// <summary>
		/// Gets or sets address of the place (may be `string` or complex
		/// object of type `PostalAddress`)
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "address")]
		public object Address { get; set; }

		/// <summary>
		/// Gets or sets geo coordinates of the place (may be complex object of
		/// type `GeoCoordinates` or `GeoShape`)
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "geo")]
		public object Geo { get; set; }

		/// <summary>
		/// Gets or sets map to the place (may be `string` (URL) or complex
		/// object of type `Map`)
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "hasMap")]
		public object HasMap { get; set; }
	}

	/// <summary>
	/// Mention information (entity type: "mention")
	/// </summary>
	public partial class Mention : Entity
	{
		/// <summary>
		/// Gets or sets the mentioned user
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "mentioned")]
		public ChannelAccount Mentioned { get; set; }

		/// <summary>
		/// Gets or sets sub Text which represents the mention (can be null or
		/// empty)
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "text")]
		public string Text { get; set; }

	}
}
