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
