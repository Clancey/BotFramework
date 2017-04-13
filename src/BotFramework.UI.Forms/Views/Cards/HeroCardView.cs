using System;
using Xamarin.Forms;
namespace BotFramework.UI
{
	public class HeroCardView : CardView
	{
		Label Text;
		Label SubText;
		public HeroCardView ()
		{
			Text = new Label ();
			Text.SetBinding (Label.TextProperty, new Binding ("Content.Title"));
			SubText = new Label ();
			SubText.SetBinding (Label.TextProperty, new Binding ("Content.Subtitle"));
		}

		protected override void SetupView ()
		{
			var attachment = BindingContext as Attachment;
			Text.TextColor = SubText.TextColor = IsFromMe ? Color.Black : Color.White;
			AddImages (attachment);
			Children.Add (Text);
			Children.Add (SubText);
			AddButtons (attachment);
		}
	}
}
