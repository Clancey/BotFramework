using System;
namespace BotFramework
{
	/// <summary>
	/// Object describing a media thumbnail
	/// </summary>
	public partial class ThumbnailUrl
	{
		/// <summary>
		/// Gets or sets url pointing to an thumbnail to use for media content
		/// </summary>
		[Newtonsoft.Json.JsonProperty ("url")]
		public string Url { get; set; }

		/// <summary>
		/// Gets or sets alt text to display for screen readers on the
		/// thumbnail image
		/// </summary>
		[Newtonsoft.Json.JsonProperty ("alt")]
		public string Alt { get; set; }
	}

	/// <summary>
	/// MediaUrl data
	/// </summary>
	public partial class MediaUrl
	{
		/// <summary>
		/// Gets or sets url for the media
		/// </summary>
		[Newtonsoft.Json.JsonProperty ("url")]
		public string Url { get; set; }

		/// <summary>
		/// Gets or sets optional profile hint to the client to differentiate
		/// multiple MediaUrl objects from each other
		/// </summary>
		[Newtonsoft.Json.JsonProperty ("profile")]
		public string Profile { get; set; }
	}

	public class MediaCard : Card
	{
		/// <summary>
		/// Gets or sets aspect ratio (16:9)(4:3)
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "aspect")]
		public string Aspect { get; set; }

		/// <summary>
		/// Gets or sets thumbnail placeholder
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "image")]
		public ThumbnailUrl Image { get; set; }

		/// <summary>
		/// Gets or sets array of media Url objects
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "media")]
		public MediaUrl [] Media { get; set; }



		/// <summary>
		/// Gets or sets is it OK for this content to be shareable with others
		/// (default:true)
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "shareable")]
		public bool? Shareable { get; set; }

		/// <summary>
		/// Gets or sets should the client loop playback at end of content
		/// (default:true)
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "autoloop")]
		public bool? Autoloop { get; set; }

		/// <summary>
		/// Gets or sets should the client automatically start playback of
		/// video in this card (default:true)
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "autostart")]
		public bool? Autostart { get; set; }
	}

	public class AudioCard : MediaCard
	{
		public const string ContentType = "application/vnd.microsoft.card.audio";
	}
	public class AnimationCard : MediaCard
	{
		public const string ContentType = "application/vnd.microsoft.card.animation";
	}

	public class VideoCard : MediaCard
	{
		public const string ContentType = "application/vnd.microsoft.card.video";
	}
}
