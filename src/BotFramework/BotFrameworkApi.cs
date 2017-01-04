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
	public class Conversation : IdClass
	{
		[JsonProperty ("conversationId")]
		public new string Id { get; set; }

		[JsonProperty ("token", NullValueHandling = NullValueHandling.Ignore)]
		public string Token { get; set; }

		int expiresIn;
        [Newtonsoft.Json.JsonProperty ("expires_in")]
		public int ExpiresIn {
			get { return expiresIn;}
			set {
				expiresIn = value;
				//Since the bot framework will not let you refresh expired tokens, we will say they expire before they do.
				TokenExpiredDate = DateTime.Now.AddSeconds(value - (value *.1));
			}
		}

		public DateTime TokenExpiredDate { get; private set; }

		public bool IsTokenValid => !string.IsNullOrWhiteSpace (Token) && TokenExpiredDate > DateTime.Now;

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
        
        [Newtonsoft.Json.JsonProperty ("entities")]
		public Entity [] Entities { get; set; }

		[JsonIgnore]
		public bool HasAttachments => Attachments?.Length > 0;

		[JsonProperty ("replyToId")]
		public string ReplyToId { get; set; }
	}

	public class IdClass
	{
		public string Id { get; set; }
	}

	public class ChannelAccount : IdClass
	{

		[Newtonsoft.Json.JsonProperty ("name")]
		public string Name { get; set; }
	}
	public class ConversationAccount : ChannelAccount
	{
		[Newtonsoft.Json.JsonProperty ("isGroup")]
		public bool IsGroup { get; set; }
	}

	[JsonConverter (typeof (JsonActivityConverter))]
	public class BotActivity : IdClass
	{
		[JsonProperty ("type")]
		public string Type { get; set; }

		public string Text { get; set; }

		[JsonProperty ("channelId")]
		public string ChannelId { get; set; } = "directline";

		public ChannelAccount Conversation { get; set; }

		[JsonIgnore]
		public string ConversationId {
			get { return Conversation?.Id; }
			set { Conversation = new ChannelAccount { Id = value }; }
		}

		public ChannelAccount From { get; set; }

		[Newtonsoft.Json.JsonProperty ("timestamp")]
		public string Timestamp { get; set; }

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
	}

	public class BotActivitySet
	{
		public BotActivity [] Activities { get; set; }
		public string WaterMark { get; set; }
	}

	public class BotFrameworkApi : Api
	{
		protected readonly string Secret;
		public BotFrameworkApi (string secret, HttpMessageHandler handler = null) : base ("botFramework",secret, handler)
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

		public Task EndConversation ()
		{
			key = Secret;
			CurrentConversation = null;
			return PrepareClient (Client);
		}

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

		[Path ("conversations/{conversationId}/upload")]
		public async Task<bool> UploadMessageAttachment (string conversationId,string userid, string fileName,string contentType , byte [] data)
		{
			try {
				var content = new MultipartFormDataContent ();
				content.Add( new ByteArrayContent (data), "file", fileName);
				content.Headers.ContentType = MediaTypeHeaderValue.Parse (contentType);

				var resp = await Post (content,
				                       queryParameters: new Dictionary<string, string> { { "conversationId", conversationId }, { "userId", userid } } );
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
				var resp = await Post<Conversation> (body:null, authenticated: false);
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
