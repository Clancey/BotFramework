using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleAuth;
namespace BotFramework
{
	//public class BotFrameworkAuthenticator : BasicAuthAuthenticator
	//{
	//	public BotFrameworkAuthenticator (HttpClient client, string url) : base (client,
	//	{

	//	}
	//}

	public abstract class JsonCreationConverter<T> : JsonConverter
	{
		protected abstract T Create (System.Type objectType, JObject jsonObject, JsonReader reader);

		public override bool CanConvert (System.Type objectType)
		{
			return typeof (T).IsAssignableFrom (objectType);
		}

		public override object ReadJson (JsonReader reader, System.Type objectType,
		  object existingValue, JsonSerializer serializer)
		{
			var jsonObject = JObject.Load (reader);
			var target = Create (objectType, jsonObject, reader);
			serializer.Populate (jsonObject.CreateReader (), target);
			return target;
		}
		public override bool CanWrite => false;

		public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException ();
		}

	}
	public class JsonActivityConverter : JsonCreationConverter<BotActivity>
	{
		protected override BotActivity Create (System.Type objectType, JObject jsonObject, JsonReader reader)
		{
			try {
				JToken token;
				if (jsonObject.TryGetValue ("type", StringComparison.CurrentCultureIgnoreCase, out token)) {
					var type = token.ToString ();
					switch (type) {
					case "message":
						return new Message ();
					}
				}
				return new BotActivity ();
			} catch (Exception ex) {
				Console.WriteLine (ex);
			}
			return new BotActivity ();
		}
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

	public class Message : BotActivity
	{
		public Message ()
		{
			Type = ActivityType.Message;
		}
		[JsonProperty ("created", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime? Created { get; set; }

		[JsonProperty ("images", NullValueHandling = NullValueHandling.Ignore)]
		public string [] Images { get; set; }


	}

	public class IdClass
	{
		public string Id { get; set; }
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
		[Newtonsoft.Json.JsonProperty (PropertyName = "timestamp")]
		public System.DateTime? Timestamp { get; set; }

		/// <summary>
		/// Gets or sets local time when message was sent (set by client Ex:
		/// 2016-09-23T13:07:49.4714686-07:00)
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "localTimestamp")]
		public System.DateTime? LocalTimestamp { get; set; }

		/// <summary>
		/// Gets or sets service endpoint
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "serviceUrl")]
		public string ServiceUrl { get; set; }

		/// <summary>
		/// Gets or sets channelId the activity was on
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "channelId")]
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
		[Newtonsoft.Json.JsonProperty (PropertyName = "recipient")]
		public ChannelAccount Recipient { get; set; }

		/// <summary>
		/// Gets or sets format of text fields [plain|markdown]
		/// Default:markdown
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "textFormat")]
		public string TextFormat { get; set; }

		/// <summary>
		/// Gets or sets attachmentLayout - hint for how to deal with multiple
		/// attachments Values: [list|carousel] Default:list
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "attachmentLayout")]
		public string AttachmentLayout { get; set; }

		/// <summary>
		/// Gets or sets array of address added
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "membersAdded")]
		public ChannelAccount [] MembersAdded { get; set; }

		/// <summary>
		/// Gets or sets array of addresses removed
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "membersRemoved")]
		public ChannelAccount [] MembersRemoved { get; set; }

		/// <summary>
		/// Gets or sets conversations new topic name
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "topicName")]
		public string TopicName { get; set; }

		/// <summary>
		/// Gets or sets the previous history of the channel was disclosed
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "historyDisclosed")]
		public bool? HistoryDisclosed { get; set; }

		/// <summary>
		/// Gets or sets the language code of the Text field
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "locale")]
		public string Locale { get; set; }

		/// <summary>
		/// Gets or sets content for the message
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "text")]
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets text to display if you can't render cards
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "summary")]
		public string Summary { get; set; }

		/// <summary>
		/// Gets or sets attachments
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "attachments")]
		public Attachment [] Attachments { get; set; }

		/// <summary>
		/// Gets or sets collection of Entity objects, each of which contains
		/// metadata about this activity. Each Entity object is typed.
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "entities")]
		public Entity [] Entities { get; set; }

		/// <summary>
		/// Gets or sets channel specific payload
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "channelData")]
		public object ChannelData { get; set; }

		/// <summary>
		/// Gets or sets contactAdded/Removed action
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "action")]
		public string Action { get; set; }

		/// <summary>
		/// Gets or sets the original id this message is a response to
		/// </summary>
		[Newtonsoft.Json.JsonProperty (PropertyName = "replyToId")]
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

	public class BotFrameworkApi : Api
	{
		protected readonly string Secret;
		public BotFrameworkApi (string secret, HttpMessageHandler handler = null) : base ("botFramework", secret, handler)
		{
			Secret = key = secret;
			BaseAddress = new Uri ("https://directline.botframework.com/v3/directline/");
			this.Verbose = true;
		}
		public override void OnException (object sender, Exception ex)
		{
			base.OnException (sender, ex);
			PrintDeepException (ex);
		}

		void PrintDeepException (Exception ex)
		{
			if (ex == null)
				return;

			Console.WriteLine (ex);
			PrintDeepException (ex?.InnerException);
		}

		string key;

		public override async Task PrepareClient (HttpClient client)
		{
			await base.PrepareClient (client);
			Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer", key);
		}

		protected override async Task VerifyCredentials ()
		{
			if (!CurrentConversation?.IsTokenValid ?? false) {
				var resp = await this.RenewConversationToken ();
				if (resp == null)
					return;
				CurrentConversation.Token = resp.Token;
				CurrentConversation.ExpiresIn = resp.ExpiresIn;
				CurrentConversation.Id = resp.Id;
				CurrentConversation.StreamUrl = resp.StreamUrl;
				await PrepareClient (Client);
			}
		}

		public Conversation CurrentConversation { get; private set; }

		/// <summary>
		/// Start a new conversation
		/// </summary>
		[Path ("conversations")]
		[Accepts ("application/json")]
		public async Task<Conversation> StartConversation (string name)
		{
			//Always use main key to start a convo
			key = Secret;
			await PrepareClient (Client);
			var conversation = await GetToken ();

			var json = await this.Post (body: null);
			CurrentConversation = Deserialize<Conversation> (json, conversation);
			CurrentConversation.MyName = name;
			//Use the new key to post messages
			key = CurrentConversation.Token;
			await PrepareClient (Client);
			return CurrentConversation;
		}
		/// <summary>
		/// Get information about an existing conversation
		/// </summary>
		/// <param name='conversationId'>
		/// </param>
		/// <param name='watermark'>
		/// </param>
		public Task<Conversation> ReconnectToConversation (string conversationId, string watermark = default (string))
		{
			if (conversationId == null) {
				throw new System.Exception ("Parameter 'conversationId' cannot be null");
			}
			var path = "conversations/{conversationId}";
			var queryParameters = new Dictionary<string, string> ();
			if (conversationId != null) {
				queryParameters.Add ("conversationId", string.Format ("{0}", conversationId));
			}
			if (watermark != null) {
				queryParameters.Add ("watermark", string.Format ("{0}", watermark));
			}
			return Get<Conversation> (path: path, queryParameters: queryParameters, authenticated: false);
		}
		public Task EndConversation ()
		{
			key = Secret;
			CurrentConversation = null;
			return PrepareClient (Client);
		}

		/// <summary>
		/// Get activities in this conversation. This method is paged with the
		/// 'watermark' parameter.
		/// </summary>
		/// <param name='conversationId'>
		/// Conversation ID
		/// </param>
		/// <param name='watermark'>
		/// (Optional) only returns activities newer than this watermark
		/// </param>
		[Path ("conversations/{conversationId}/activities")]
		public Task<BotActivitySet> GetMessages (string conversationId, string watermark = null)
		{
			var parameters = new Dictionary<string, string> {
				{"conversationId",conversationId}
			};
			if (!string.IsNullOrWhiteSpace (watermark))
				parameters.Add ("watermark", watermark);

			return Get<BotActivitySet> (queryParameters: parameters);
		}

		/// <summary>
		/// Send an activity
		/// </summary>
		/// <param name='conversationId'>
		/// Conversation ID
		/// </param>
		/// <param name='activity'>
		/// Activity to send
		/// </param>
		[Path ("conversations/{conversationId}/activities")]
		public async Task<bool> SendMessage (string conversationId, Message message)
		{
			try {
				message.ConversationId = conversationId;
				message.FromId = CurrentConversation.MyName;
				message.IsFromMe = true;
				var resp = await Post<Message> (message, queryParameters: new Dictionary<string, string> { { "conversationId", conversationId } });
				Console.WriteLine (resp);
				return true;
			} catch (Exception ex) {
				this.OnException (this, ex);
			}
			return false;
		}

		[Path ("conversations/{conversationId}/activities")]
		public async Task<bool> SendActivity (string conversationId, BotActivity message)
		{
			try {
				var resp = await Post (message, queryParameters: new Dictionary<string, string> { { "conversationId", conversationId } });
				Console.WriteLine (resp);
				return true;
			} catch (Exception ex) {
				this.OnException (this, ex);
			}
			return false;
		}

		/// <summary>
		/// Upload file(s) and send as attachment(s)
		/// </summary>
		/// <param name='conversationId'>
		/// </param>
		/// <param name='file'>
		/// </param>
		/// <param name='userId'>
		/// </param>
		[Path ("conversations/{conversationId}/upload")]
		public async Task<bool> UploadMessageAttachment (string conversationId, string userid, string fileName, string contentType, byte [] data)
		{
			try {
				var content = new MultipartFormDataContent ();
				content.Add (new ByteArrayContent (data), "file", fileName);
				content.Headers.ContentType = MediaTypeHeaderValue.Parse (contentType);

				var resp = await Post (content,
									   queryParameters: new Dictionary<string, string> { { "conversationId", conversationId }, { "userId", userid } });
				Console.WriteLine (resp);
				return true;
			} catch (Exception ex) {
				this.OnException (this, ex);
			}
			return false;
		}

		[Path ("conversations/{conversationId}/upload")]
		public async Task<bool> UploadMessageAttachment (string conversationId, string userid, string fileName, string contentType, Stream stream)
		{
			try {
				var content = new MultipartFormDataContent ();
				content.Add (new StreamContent (stream), "file", fileName);
				content.Headers.ContentType = MediaTypeHeaderValue.Parse (contentType);

				var resp = await Post (content,
									   queryParameters: new Dictionary<string, string> { { "conversationId", conversationId }, { "userId", userid } });
				Console.WriteLine (resp);
				return true;
			} catch (Exception ex) {
				this.OnException (this, ex);
			}
			return false;
		}

		[Path ("tokens/refresh")]
		public async Task<Conversation> RenewConversationToken ()
		{
			try {
				var resp = await Post<Conversation> (body: null, authenticated: false);
				Console.WriteLine (resp);
				return resp;
			} catch (Exception ex) {
				this.OnException (this, ex);
			}
			return null;
		}

		[Path ("tokens/generate")]
		public async Task<Conversation> GetToken ()
		{
			//Always use main key to start a convo
			key = Secret;
			await PrepareClient (Client);

			var resp = await Post<Conversation> (body: null);
			return resp;

		}

		protected override T Deserialize<T> (string data)
		{
			Console.WriteLine (data);
			return base.Deserialize<T> (data);
		}
		protected override T Deserialize<T> (string data, object inObject)
		{
			Console.WriteLine (data);
			return base.Deserialize<T> (data, inObject);
		}
	}
}
