using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Threading;
using SimpleAuth;
using System.Linq;

namespace BotFramework
{
	public class ConversationManager
	{
		static ConversationManager ()
		{
		}
		public Conversation Conversation { get; private set; }

		public static ObservableCollection<ConversationManager> Conversations { get; } = new ObservableCollection<ConversationManager> ();
		public BotFrameworkApi Api { get; private set; }
		public static async Task<ConversationManager> StartConversation (string name, string clientSecret, bool automaticallyGetMessageUpdates = true)
		{
			var api = new BotFrameworkApi (clientSecret);
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
			await Api.EndConversation ();
			await StopMonitoringConversation ();
		}

		public async Task SendMessage (Message message)
		{
			message.FromId = Conversation.MyName;
			await Api.SendMessage (Conversation.Id, message);
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
							var messages = await Api.GetMessages (Conversation.Id, Conversation.WaterMark);
							if (!string.IsNullOrWhiteSpace (messages?.WaterMark))
								Conversation.WaterMark = messages.WaterMark;
							if (messages.Activities?.Length > 0) {
								foreach (var m in messages.Activities)
									AddMessage (m);
							}
						} catch (Exception ex) {
							Console.WriteLine (ex);
						} finally {
							await Task.Delay (1000);
						}
					}
				});
			return Task.FromResult(true);
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

		async void OnMessageRecieved (string json)
		{
			if (string.IsNullOrWhiteSpace (json))
				return;
			Console.WriteLine (json);
			var message = await json.ToObjectAsync<ActivityBase> ().ConfigureAwait (false);
			AddMessage (message);
		}

		void AddMessage (ActivityBase message)
		{
			if (message == null)
				return;
			message.IsFromMe = message.FromId == Conversation.MyName;

			Conversation.Messages.Add (message);
		}

	}
}
