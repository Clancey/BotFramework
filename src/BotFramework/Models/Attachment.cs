using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BotFramework
{ 
	// <summary>
	/// An attachment within an activity
	/// </summary>
	public class Attachment
	{
		/// <summary>
		/// Gets or sets mimetype/Contenttype for the file
		/// </summary>
		[JsonProperty ("contentType")]
		public string ContentType { get; set; }

		/// <summary>
		/// Gets or sets embedded content
		/// </summary>
		[JsonProperty ("content")]
		[JsonConverter (typeof (JsonCardConverter))]
		public Card Content { get; set; }

		/// <summary>
		/// Gets or sets content Url
		/// </summary>
		[JsonProperty ("contentUrl")]
		public string Url { get; set; }

		/// <summary>
		/// Gets or sets (OPTIONAL) Thumbnail associated with attachment
		/// </summary>
		[JsonProperty("thumbnailUrl")]
		public string ThumbnailUrl { get; set; }


		public string DocType { get; set; }

		/// <summary>
		/// Gets or sets (OPTIONAL) The name of the attachment
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }
	}

	public enum CardActionType
	{
		ImBack,
		OpenUrl,
		PlayAudio,
		PlayVideo,
		PostBack,
		SignIn
	}

	public class CardAction
	{
		/// <summary>
		/// Gets or sets defines the type of action implemented by this button.
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "type")]
		public CardActionType Type { get; set; }

		[JsonProperty ("title")]
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets URL Picture which will appear on the button, next to
		/// text label.
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "image")]
		public string Image { get; set; }

		/// <summary>
		/// Gets or sets supplementary parameter for action. Content of this
		/// property depends on the ActionType
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "value")]
		public string Value { get; set; }

		WeakReference parent;
		[JsonIgnore]
		public Card Parent { 
			get { return parent?.Target as Card; }
			set { parent = new WeakReference (value);}
		}


		WeakReference actionHandler;
		[JsonIgnore]
		public CardActionHandler ActionHandler { 
			get { return actionHandler?.Target as CardActionHandler; }
			set { actionHandler = new WeakReference (value);}
		}
	}
	public class CardImage
	{

		/// <summary>
		/// Gets or sets URL Thumbnail image for major content property.
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "url")]
		public string Url { get; set; }

		/// <summary>
		/// Gets or sets image description intended for screen readers
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "alt")]
		public string Alt { get; set; }

		/// <summary>
		/// Gets or sets action assigned to specific Attachment.E.g.navigate to
		/// specific URL or play/open media content
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "tap")]
		public CardAction Tap { get; set; }
	}
}

