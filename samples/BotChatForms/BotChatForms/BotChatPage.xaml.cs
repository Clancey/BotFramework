using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections;
using BotFramework;
using Plugin.Media;
using System.IO;
using System.Diagnostics;

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
				Debug.WriteLine (ex);
				foreach (DictionaryEntry m in ex.Data)
					Debug.WriteLine ($"{m.Key} - {m.Value}");
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
			currentConversation = await ConversationManager.StartConversation (name, "WfnLyhBjpIE.cwA.VWI.hlOZgHQO2ceQFi0bBPV7cmA55PKBam-rd1arfgSj5nE");
			currentConversation.CardActionTapped = HandleCardActionTapped;
			MessageList.ItemsSource = currentConversation.Conversation.Messages;
			StartStopButton.Text = "End Conversation";
		}

		void HandleCardActionTapped (CardAction action)
		{
			switch (action.Type) {
			case CardActionType.OpenUrl:
				Device.OpenUri (new Uri (action.Value));
				break;
			}
			Debug.WriteLine ($"Unhandled tap: {action.Value}");
		}

		void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{
			var item = (BotActivity)e.Item;
		}

		async Task EndConversation ()
		{
			await currentConversation.EndConversation ();
			currentConversation.CardActionTapped = null;
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

		async void AddPhoto (object sender, System.EventArgs e)
		{
			Task startTask = null;
			if (currentConversation == null) {
				startTask = StartConversation ();
			}
			try {
				var photo = await CrossMedia.Current.PickPhotoAsync ();

				if (!(startTask?.IsCompleted ?? true))
					await startTask;
				
				var fileName = Path.GetFileName (photo.Path);
				var extension = Path.GetExtension (photo.Path).TrimStart('.');
				var type = $"image/{extension}";

				await currentConversation.UploadAttachment (fileName, type, photo.GetStream ());
			} catch (Exception ex) {
				Debug.WriteLine (ex);
			}
		}
	}
}
