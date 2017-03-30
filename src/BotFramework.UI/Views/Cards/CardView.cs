using System;
using Xamarin.Forms;
namespace BotFramework.UI
{
	public class CardView : StackLayout, IMessageContext
	{
		public CardView ()
		{
			
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();
			ResetView ();
			SetupView ();
		}

		public bool IsFromMe { get; set; }

		protected virtual void SetupView ()
		{
			var message = BindingContext as Attachment;
			if (message == null)
				return;

			AddImages (message);

			AddButtons (message);

		}

		protected virtual void AddButtons (Attachment attachment)
		{
			if (attachment.Content?.Buttons == null) 
				return;
			
			foreach (var b in attachment.Content?.Buttons) {
				Children.Add (CreateView (b));
			}
		}

		protected virtual void AddImages (Attachment attachment)
		{
			if (!string.IsNullOrWhiteSpace(attachment.ThumbnailUrl))
			{
				Children.Add(CreatImageView(attachment, nameof(attachment.ThumbnailUrl)));
			};
			if (attachment.Content?.Images == null)
				return;
			
			foreach (var i in attachment.Content?.Images) {
				Children.Add (CreateView (i));
			}
		}

		protected virtual View CreateView(CardImage image)
		{
			return CreatImageView(image, nameof(image.Url));
		}
		protected virtual View CreatImageView (object imageBinding, string propertyName)
		{
			var view = new Image ();
			view.BindingContext = imageBinding;
			view.SetBinding (Image.SourceProperty, new Binding (propertyName));
			return view;
		}

		protected virtual View CreateView (CardAction button)
		{
			return new ChatButton () {
				BindingContext = button,
				IsFromMe = IsFromMe,
			};
		}

		protected virtual void ResetView ()
		{
			Children.Clear ();
		}
	}
}
