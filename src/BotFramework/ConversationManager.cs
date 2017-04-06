using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Threading;
using SimpleAuth;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace BotFramework
{
	public interface CardActionHandler
	{
		Task HandleTap (CardAction cardAction);
	}
	public class ConversationManager : CardActionHandler
	{
		static ConversationManager ()
		{

		}
		public Conversation Conversation { get; private set; }

		public BotFrameworkApi Api { get; private set; }

		public static async Task<ConversationManager> StartConversation (string name, string clientSecret, bool automaticallyGetMessageUpdates = true, Uri baseUri = null)
		{
			var api = new BotFrameworkApi (clientSecret);
			if (baseUri != null)
				api.BaseAddress = baseUri;
			var conversation = await api.StartConversation (name);
			var manager = new ConversationManager () {
				Conversation = conversation,
				Api = api,
			};
			if (automaticallyGetMessageUpdates)
				await manager.MonitorConversation ();
			return manager;
		}

		public async Task EndConversation ()
		{
			Conversation.Messages.Clear ();
			await Api.EndConversation ();
			await StopMonitoringConversation ();
			Conversation = null;
		}

		public Task SendMessage (string text)
		{
			return SendMessage (new Message { Text = text });
		}
		public Task SendMessage (Message message)
		{
			message.FromId = Conversation.MyName;
			return Api.SendMessage (Conversation.Id, message);
		}

		public Task UploadAttachment (string fileName, string contentType, byte [] data)
		{
			return Api.UploadMessageAttachment (Conversation.Id, Conversation.MyName, fileName, contentType, data);
		}
		public Task UploadAttachment (string fileName, string contentType, Stream stream)
		{
			return Api.UploadMessageAttachment (Conversation.Id, Conversation.MyName, fileName, contentType, stream);
		}

		bool shouldBePolling;
		Task messagePollingTask;
		public Task<bool> MonitorConversation ()
		{
			shouldBePolling = true;
			if (messagePollingTask?.IsCompleted ?? true)
				messagePollingTask = Task.Run (async () => {
					while (Conversation != null && shouldBePolling) {
						try {
							await FetchMessages ();
						} catch (Exception ex) {
							Console.WriteLine (ex);
						} finally {
							await Task.Delay (1000);
						}
					}
				});
			return Task.FromResult (true);
		}
		public Task<bool> StopMonitoringConversation ()
		{
			shouldBePolling = false;
			return Task.FromResult (true);
		}

		//TODO: find a nice XPlat way to do web sockets, or wait for .net standard 2.0
		//WebSockets.WebSocketClient socket;
		//public async Task<bool> MonitorConversation ()
		//{
		//	try {
		//		if (socket == null) {
		//			socket = new WebSockets.WebSocketClient();
		//			socket.MessageRecieved = OnMessageRecieved;
		//		}
		//		if (socket.IsOpen)
		//			return true;
		//		await socket.Connect (new Uri (Conversation.StreamUrl), CancellationToken.None);


		//		return socket.IsOpen;
		//	} catch (Exception e) {
		//		Console.WriteLine (e);
		//	}
		//	return false;
		//}

		//public async Task<bool> StopMonitoringConversation ()
		//{
		//	if (!(socket?.IsOpen ?? false)) {
		//		socket.MessageRecieved = null;
		//		await socket.Close (100, "Closed");
		//	}
		//	socket = null;
		//}
		public async Task FetchMessages ()
		{
			if (Conversation == null)
				throw new Exception ("You need to start a conversation before fetching messages");
			var messages = await Api.GetMessages (Conversation.Id, Conversation.WaterMark);
			if (!string.IsNullOrWhiteSpace (messages?.WaterMark))
				Conversation.WaterMark = messages.WaterMark;
			if (messages.Activities?.Length > 0) {
				foreach (var m in messages.Activities)
					AddMessage (m);
			}
		}

		async void OnMessageRecieved (string json)
		{
			if (string.IsNullOrWhiteSpace (json))
				return;
			Console.WriteLine (json);
			var message = await json.ToObjectAsync<BotActivity> ().ConfigureAwait (false);
			AddMessage (message);
		}

		void AddMessage (BotActivity message)
		{
			if (message == null || Conversation == null)
				return;
			message.IsFromMe = message.FromId == Conversation.MyName;
			//Set the callback for the actions!
			(message as Message)?.Attachments?.ToList ()?.ForEach (x => x?.Content?.Buttons?.ToList ().ForEach (y => y.ActionHandler = this));
			Conversation.Messages.Add (message);
		}
		public Action<CardAction> CardActionTapped { get; set; }
		public virtual async Task HandleTap (CardAction action)
		{
			switch (action.Type) {
			case CardActionType.ImBack:
			case CardActionType.PostBack:
				await SendMessage (action.Value);
				return;
			}
			CardActionTapped?.Invoke (action);
		}
	}
}
