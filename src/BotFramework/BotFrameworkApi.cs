using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
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
	public class JsonActivityConverter : JsonCreationConverter<ActivityBase>
	{
		protected override ActivityBase Create (System.Type objectType, JObject jsonObject, JsonReader reader)
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
				return new ActivityBase ();
			} catch (Exception ex) {
				Console.WriteLine (ex);
			}
			return new ActivityBase ();
		}
	}
	public class Conversation : IdClass
	{
		[JsonProperty ("conversationId")]
		public new string Id { get; set; }

		[JsonProperty ("token", NullValueHandling = NullValueHandling.Ignore)]
		public string Token { get; set; }


		[JsonProperty ("streamUrl", NullValueHandling = NullValueHandling.Ignore)]
		public string StreamUrl { get; set; }

		public int Expiration { get; set; }

		[JsonProperty ("ETag", NullValueHandling = NullValueHandling.Ignore)]
		public string ETag { get; set; }

		public ObservableCollection<ActivityBase> Messages { get; private set; } = new ObservableCollection<ActivityBase> ();


		[JsonProperty ("watermark", NullValueHandling = NullValueHandling.Ignore)]
		public string WaterMark { get; set; }

		public string MyName { get; set; }
	}

	public class Message : ActivityBase
	{
		public Message ()
		{
			Type = "message";
		}
		[JsonProperty ("created", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime? Created { get; set; }

		[JsonProperty ("channelData", NullValueHandling = NullValueHandling.Ignore)]
		public object [] ChannelData { get; set; }

		[JsonProperty ("images", NullValueHandling = NullValueHandling.Ignore)]
		public string [] Images { get; set; }

		[JsonProperty ("attachments", NullValueHandling = NullValueHandling.Ignore)]
		public Attachment [] Attachments { get; set; }

		public bool HasAttachments => Attachments?.Length > 0;

		[JsonExtensionData]
		[JsonProperty (NullValueHandling = NullValueHandling.Ignore)]
		public IDictionary<string, JToken> AdditionalData;


		[JsonProperty ("replyToId")]
		public string ReplyToId { get; set; }
	}

	public class IdClass {
		public string Id { get; set; }
	}

	[JsonConverter (typeof (JsonActivityConverter))]
	public class ActivityBase : IdClass
	{
		[JsonProperty ("type")]
		public string Type { get; set; }

		public string Text { get; set; }

		[JsonProperty ("channelId")]
		public string ChannelId { get; set; } = "directline";

		public IdClass Conversation { get; set; }

		public string ConversationId {
			get { return Conversation?.Id; }
			set { Conversation = new IdClass { Id = value }; }
		}

		public IdClass From {get;set;}

		public string FromId {
			get { return From?.Id;}
			set { From = new IdClass { Id = value }; }
		}


		[JsonIgnore]
		public bool IsFromMe { get; set; }

	}

	public class MessagesResponse
	{
		public ActivityBase [] Activities { get; set; }
		public string WaterMark { get; set; }
		public string ETag { get; set; }
	}

	public class BotFrameworkApi : Api
	{
		protected readonly string Secret;
		public BotFrameworkApi (string secret, HttpMessageHandler handler = null) : base ("botFramework", handler) //base ("botFramework", null, handler)
		{
			Secret = key = secret;
			BaseAddress = new Uri ("https://ic-dandris-scratch.azurewebsites.net/v3/directline/");
			this.Verbose = true;

			//this.CurrentShowAuthenticator = (BasicAuthAuthenticator obj) => {

			//};	
		}
		public override void OnException (object sender, Exception ex)
		{
			base.OnException (sender, ex);
			Console.WriteLine (ex);
		}
		string key;

		public override async Task PrepareClient (HttpClient client)
		{
			await base.PrepareClient (client);
			Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer", key);
		}

		public Conversation CurrentConversation { get; private set; }

		[Path ("conversations")]
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

		public Task EndConversation ()
		{
			key = Secret;
			CurrentConversation = null;
			return PrepareClient (Client);
		}

		[Path ("conversations/{conversationId}/activities")]
		public Task<MessagesResponse> GetMessages (string conversationId, string watermark = null)
		{
			var parameters = new Dictionary<string, string> {
				{"conversationId",conversationId}
			};
			if (!string.IsNullOrWhiteSpace (watermark))
				parameters.Add ("watermark", watermark);

			return Get<MessagesResponse> (queryParameters: parameters);
		}

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
		public async Task<bool> SendActivity (string conversationId, ActivityBase message)
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

		[Path ("conversations/{conversationId}/upload")]
		public async Task<bool> UploadMessageAttachment (string conversationId, byte [] data)
		{
			try {
				var resp = await Post (new ByteArrayContent (data), queryParameters: new Dictionary<string, string> { { "conversationId", conversationId } });
				Console.WriteLine (resp);
				return true;
			} catch (Exception ex) {
				this.OnException (this, ex);
			}
			return false;
		}


		[Path ("conversations/{conversationId}/upload")]
		public async Task<bool> UploadMessageAttachment (string conversationId, Attachment attachment)
		{
			try {
				var resp = await Post (attachment, queryParameters: new Dictionary<string, string> { { "conversationId", conversationId } });
				Console.WriteLine (resp);
				return true;
			} catch (Exception ex) {
				this.OnException (this, ex);
			}
			return false;
		}


		[Path ("tokens/{conversationId}/renew")]
		public async Task<bool> RenewConversationToken (string conversationId)
		{
			try {
				var resp = await Get (queryParameters: new Dictionary<string, string> { { "conversationId", conversationId } });
				Console.WriteLine (resp);
				return true;
			} catch (Exception ex) {
				this.OnException (this, ex);
			}
			return false;
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
