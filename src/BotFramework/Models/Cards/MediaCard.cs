using System;
namespace BotFramework
{

	public partial class ThumbnailUrl
	{

		[Newtonsoft.Json.JsonProperty ("url")]
		public string Url { get; set; }

		[Newtonsoft.Json.JsonProperty ("alt")]
		public string Alt { get; set; }
	}

	public partial class MediaUrl
	{
		[Newtonsoft.Json.JsonProperty ("url")]
		public string Url { get; set; }

		[Newtonsoft.Json.JsonProperty ("profile")]
		public string Profile { get; set; }
	}

	public class MediaCard : Card
	{

		[Newtonsoft.Json.JsonProperty ("image")]
		public ThumbnailUrl Image { get; set; }

		[Newtonsoft.Json.JsonProperty ("media")]
		public MediaUrl [] Media { get; set; }

		[Newtonsoft.Json.JsonProperty ("shareable")]
		public bool Shareable { get; set; }

		[Newtonsoft.Json.JsonProperty ("autoloop")]
		public bool Autoloop { get; set; }

		[Newtonsoft.Json.JsonProperty ("autostart")]
		public bool Autostart { get; set; }
	}

	public class AudioCard : MediaCard
	{

        [Newtonsoft.Json.JsonProperty ("aspect")]
		public string Aspect { get; set; }
	}
	public class AnimationCard : MediaCard
	{
		public const string ContentType = "application/vnd.microsoft.card.animation";
	}
}
