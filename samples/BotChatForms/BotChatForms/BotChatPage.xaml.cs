using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections;
using BotFramework;

namespace BotChatForms
{
	public partial class BotChatPage : ContentPage
	{
		string name = "Clancey";
		public BotChatPage ()
		{
			InitializeComponent ();
			MessageList.HasUnevenRows = true;
			MessageList.ItemSelected += (sender, e) => {
				((ListView)sender).SelectedItem = null;
			};
		}
		ConversationManager currentConversation;
		async void StartStopConversation (object sender, System.EventArgs e)
		{
			try {
				if (currentConversation == null)
					await StartConversation ();
				else
					await EndConversation ();
			} catch (Exception ex) {
				Console.WriteLine (ex);
				foreach (DictionaryEntry m in ex.Data)
					Console.WriteLine ($"{m.Key} - {m.Value}");
			}
		}

		protected async override void OnAppearing ()
		{
			base.OnAppearing ();
			await Task.Delay (100);
			if (string.IsNullOrWhiteSpace (name)) {
				name = await this.GetAlertText ("Hello", "Please enter your name:");
			}
		}

		async Task StartConversation ()
		{
			currentConversation = await ConversationManager.StartConversation (name, "R9i5KxgSHDg.cwA.r8A.SV1JeSldHUIsRVgUm6L0qH29NvPRmTT4vZg6m171hOs");
			MessageList.ItemsSource = currentConversation.Conversation.Messages;
			StartStopButton.Text = "End Conversation";
		}

		async Task EndConversation ()
		{
			await currentConversation.EndConversation ();
			currentConversation = null;
			MessageList.ItemsSource = null;
			StartStopButton.Text = "Start Conversation";
		}

		async void SendMessage (object sender, System.EventArgs e)
		{
			if (currentConversation == null) {
				await StartConversation ();
			}
			if (string.IsNullOrWhiteSpace (Text.Text))
				return;
			await currentConversation.SendMessage (new Message {Text = Text.Text });
			Text.Text = "";
		}

		//async void AddPhoto (object sender, System.EventArgs e)
		//{
		//	Task startTask = null;
		//	if (App.Api.CurrentConversation == null) {
		//		startTask = StartConversation ();
		//	}
		//	try {
		//		var photo = await CrossMedia.Current.PickPhotoAsync ();

		//		if (!(startTask?.IsCompleted ?? true))
		//			await startTask;
		//		//var data = DependencyService.Get<IFileLoader> ().ReadAllBytes (photo.Path);

		//		var s = await App.Api.UploadMessageAttachment (App.Api.CurrentConversation.Id, new Attachment { DocType = "image", Url = photo.Path});
		//		Console.WriteLine (s);
		//	} catch (Exception ex) {
		//		Console.WriteLine (ex);
		//	}
		//}
	}
}
