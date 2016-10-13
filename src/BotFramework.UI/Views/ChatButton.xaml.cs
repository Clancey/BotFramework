using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace BotFramework.UI
{
	public partial class ChatButton : ContentView
	{
		public ChatButton ()
		{
			InitializeComponent ();

			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.SetBinding (TapGestureRecognizer.CommandProperty, "TapCommand");
			TapCommand = new Command (HandleTap);
			this.GestureRecognizers.Add (tapGestureRecognizer);
			//You have to set the binding context after it's added
			tapGestureRecognizer.BindingContext = this;
		}
		public ICommand TapCommand { get; private set; }

		void HandleTap (object arg)
		{
			Console.WriteLine ("Tapped");
		}
		public bool IsFromMe { get; set; }
	}
}
