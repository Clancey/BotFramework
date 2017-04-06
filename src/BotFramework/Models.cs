using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BotFramework
{
	public class IdClass
	{
		public string Id { get; set; }
	}

	[Newtonsoft.Json.JsonConverter (typeof (Newtonsoft.Json.Converters.StringEnumConverter))]
	public enum ActivityType
	{
		Message,
		ContactRelationUpdate,
		ConverationUpdate,
		Typing
	}

	/// <summary>
	/// An Activity is the basic communication type for the Bot Framework 3.0
	/// protocol
	/// </summary>
	[JsonConverter (typeof (JsonActivityConverter))]
	public class BotActivity : IdClass
	{
		/// <summary>
		/// Gets or sets the type of the activity
		/// [message|contactRelationUpdate|converationUpdate|typing]
		/// </summary>
		public ActivityType Type { get; set; }

		[JsonIgnore]
		public string ConversationId {
			get { return Conversation?.Id; }
			set { Conversation = new ConversationAccount { Id = value }; }
		}

		/// <summary>
		/// Gets or sets UTC Time when message was sent (Set by service)
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "timestamp", NullValueHandling = NullValueHandling.Ignore)]
		public System.DateTime? Timestamp { get; set; }

		/// <summary>
		/// Gets or sets local time when message was sent (set by client Ex:
		/// 2016-09-23T13:07:49.4714686-07:00)
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "localTimestamp", NullValueHandling = NullValueHandling.Ignore)]
		public System.DateTime? LocalTimestamp { get; set; }

		/// <summary>
		/// Gets or sets service endpoint
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "serviceUrl", NullValueHandling = NullValueHandling.Ignore)]
		public string ServiceUrl { get; set; }

		/// <summary>
		/// Gets or sets channelId the activity was on
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "channelId", NullValueHandling = NullValueHandling.Ignore)]
		public string ChannelId { get; set; }

		/// <summary>
		/// Gets or sets sender address
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "from")]
		public ChannelAccount From { get; set; }

		/// <summary>
		/// Gets or sets conversation
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "conversation")]
		public ConversationAccount Conversation { get; set; }

		/// <summary>
		/// Gets or sets (Outbound to bot only) Bot's address that received the
		/// message
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "recipient", NullValueHandling = NullValueHandling.Ignore)]
		public ChannelAccount Recipient { get; set; }

		/// <summary>
		/// Gets or sets format of text fields [plain|markdown]
		/// Default:markdown
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "textFormat", NullValueHandling = NullValueHandling.Ignore)]
		public string TextFormat { get; set; }

		/// <summary>
		/// Gets or sets attachmentLayout - hint for how to deal with multiple
		/// attachments Values: [list|carousel] Default:list
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "attachmentLayout", NullValueHandling = NullValueHandling.Ignore)]
		public string AttachmentLayout { get; set; }

		/// <summary>
		/// Gets or sets array of address added
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "membersAdded", NullValueHandling = NullValueHandling.Ignore)]
		public ChannelAccount [] MembersAdded { get; set; }

		/// <summary>
		/// Gets or sets array of addresses removed
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "membersRemoved", NullValueHandling = NullValueHandling.Ignore)]
		public ChannelAccount [] MembersRemoved { get; set; }

		/// <summary>
		/// Gets or sets conversations new topic name
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "topicName", NullValueHandling = NullValueHandling.Ignore)]
		public string TopicName { get; set; }

		/// <summary>
		/// Gets or sets the previous history of the channel was disclosed
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "historyDisclosed", NullValueHandling = NullValueHandling.Ignore)]
		public bool? HistoryDisclosed { get; set; }

		/// <summary>
		/// Gets or sets the language code of the Text field
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "locale", NullValueHandling = NullValueHandling.Ignore)]
		public string Locale { get; set; }

		/// <summary>
		/// Gets or sets content for the message
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "text", NullValueHandling = NullValueHandling.Ignore)]
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets text to display if you can't render cards
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "summary", NullValueHandling = NullValueHandling.Ignore)]
		public string Summary { get; set; }

		/// <summary>
		/// Gets or sets attachments
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "attachments", NullValueHandling = NullValueHandling.Ignore)]
		public Attachment [] Attachments { get; set; }

		/// <summary>
		/// Gets or sets collection of Entity objects, each of which contains
		/// metadata about this activity. Each Entity object is typed.
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "entities", NullValueHandling = NullValueHandling.Ignore)]
		public Entity [] Entities { get; set; }

		/// <summary>
		/// Gets or sets channel specific payload
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "channelData", NullValueHandling = NullValueHandling.Ignore)]
		public object ChannelData { get; set; }

		/// <summary>
		/// Gets or sets contactAdded/Removed action
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "action", NullValueHandling = NullValueHandling.Ignore)]
		public string Action { get; set; }

		/// <summary>
		/// Gets or sets the original id this message is a response to
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "replyToId", NullValueHandling = NullValueHandling.Ignore)]
		public string ReplyToId { get; set; }

		[JsonIgnore]
		public string FromId {
			get { return From?.Id; }
			set { From = new ChannelAccount { Id = value }; }
		}


		[JsonIgnore]
		public bool IsFromMe { get; set; }

		[JsonExtensionData]
		[JsonProperty (NullValueHandling = NullValueHandling.Ignore)]
		public IDictionary<string, JToken> AdditionalData;



		[JsonIgnore]
		public bool HasAttachments => Attachments?.Length > 0;
	}

	/// <summary>
	/// A collection of activities
	/// </summary>
	public class BotActivitySet
	{
		/// <summary>
		/// Gets or sets activities
		/// </summary>
		public BotActivity [] Activities { get; set; }
		/// <summary>
		/// Gets or sets maximum watermark of activities within this set
		/// </summary>
		public string WaterMark { get; set; }
	}

	public class Message : BotActivity
	{
		public const ActivityType ContentType = ActivityType.Message;
		public Message ()
		{
			Type = ActivityType.Message;
		}
		[JsonProperty ("created", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime? Created { get; set; }

		[JsonProperty ("images", NullValueHandling = NullValueHandling.Ignore)]
		public string [] Images { get; set; }
	}


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
		[JsonProperty ("thumbnailUrl")]
		public string ThumbnailUrl { get; set; }


		public string DocType { get; set; }

		/// <summary>
		/// Gets or sets (OPTIONAL) The name of the attachment
		/// </summary>
		[JsonProperty ("name")]
		public string Name { get; set; }
	}

	public enum CardActionType
	{
		ImBack,
		OpenUrl,
		PlayAudio,
		PlayVideo,
		PostBack,
		SignIn,
		AdaptiveAction
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
			set { parent = new WeakReference (value); }
		}


		WeakReference actionHandler;
		[JsonIgnore]
		public CardActionHandler ActionHandler {
			get { return actionHandler?.Target as CardActionHandler; }
			set { actionHandler = new WeakReference (value); }
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

	/// <summary>
	/// Channel account information needed to route a message
	/// </summary>
	public class ChannelAccount : IdClass
	{
		/// <summary>
		/// Gets or sets display friendly name
		/// </summary>
		[Newtonsoft.Json.JsonProperty ("name")]
		public string Name { get; set; }
	}

	/// <summary>
	/// Channel account information for a conversation
	/// </summary>
	public class ConversationAccount : ChannelAccount
	{
		[Newtonsoft.Json.JsonProperty ("isGroup")]
		public bool IsGroup { get; set; }
	}

	/// <summary>
	/// An object representing a conversation or a conversation token
	/// </summary>
	public class Conversation : IdClass
	{
		/// <summary>
		/// Gets or sets ID for this conversation
		/// </summary>
		[JsonProperty ("conversationId")]
		public new string Id { get; set; }

		/// <summary>
		/// Gets or sets token scoped to this conversation
		/// </summary>
		[JsonProperty ("token", NullValueHandling = NullValueHandling.Ignore)]
		public string Token { get; set; }

		/// <summary>
		/// Gets or sets expiration for token
		/// </summary>
		int expiresIn;
		[Newtonsoft.Json.JsonProperty ("expires_in")]
		public int ExpiresIn {
			get { return expiresIn; }
			set {
				expiresIn = value;
				//Since the bot framework will not let you refresh expired tokens, we will say they expire before they do.
				TokenExpiredDate = DateTime.Now.AddSeconds (value - (value * .1));
			}
		}

		public DateTime TokenExpiredDate { get; private set; }

		public bool IsTokenValid => !string.IsNullOrWhiteSpace (Token) && TokenExpiredDate > DateTime.Now;

		/// <summary>
		/// Gets or sets URL for this conversation's message stream
		/// </summary>
		[JsonProperty ("streamUrl", NullValueHandling = NullValueHandling.Ignore)]
		public string StreamUrl { get; set; }

		[JsonProperty ("ETag", NullValueHandling = NullValueHandling.Ignore)]
		public string ETag { get; set; }

		public ObservableCollection<BotActivity> Messages { get; private set; } = new ObservableCollection<BotActivity> ();


		[JsonProperty ("watermark", NullValueHandling = NullValueHandling.Ignore)]
		public string WaterMark { get; set; }

		public string MyName { get; set; }
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
		public const string ContentType = "geocoordinates";
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
		public const string ContentType = "place";
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
		public const string ContentType = "mention";
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
