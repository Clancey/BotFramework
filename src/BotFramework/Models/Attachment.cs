using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BotFramework
{
	public class Attachment
	{
		[JsonProperty ("contentType")]
		public string ContentType { get; set; }

		[JsonProperty ("content")]
		[JsonConverter (typeof (JsonCardConverter))]
		public Card Content { get; set; }

		public string Url { get; set; }

		public string DocType { get; set; }


		public string Title { get; set; }

		public string SubTitle { get; set; }
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
		[JsonProperty ("type")]
		public CardActionType Type { get; set; }

		[JsonProperty ("title")]
		public string Title { get; set; }

		[JsonProperty ("image")]
		public string Image { get; set; }

		[JsonProperty ("value")]
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

		[JsonProperty ("url")]
		public string Url { get; set; }

		[JsonProperty ("alt")]
		public string Alt { get; set; }
	}
}

