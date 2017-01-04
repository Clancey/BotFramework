using System;
using Xamarin.Forms;

namespace BotFramework.UI
{
	public class CardTemplateSelector : Xamarin.Forms.DataTemplateSelector
	{
		protected DataTemplate defaultTemplate;
		protected DataTemplate heroCardTemplate;
		protected DataTemplate thumbnailTemplate;
		protected DataTemplate receiptTemplate;
		protected DataTemplate signInTemplate;
		public CardTemplateSelector ()
		{
			defaultTemplate = new DataTemplate (typeof (Label));
			heroCardTemplate = new DataTemplate (typeof (HeroCardView));
			thumbnailTemplate = new DataTemplate (typeof (ThumbnailCardView));
			receiptTemplate = new DataTemplate (typeof (ReceiptCardView));
			signInTemplate = new DataTemplate (typeof (SignInCardView));

		}

		protected override DataTemplate OnSelectTemplate (object item, BindableObject container)
		{
			var messageVm = item as Attachment;
			if (messageVm == null)
				return null;
			switch(messageVm.ContentType){
			case HeroCard.ContentType:
				return heroCardTemplate;
			case ReceiptCard.ContentType:
				return receiptTemplate;
			case SigninCard.ContentType:
				return signInTemplate;
			case ThumbnailCard.ContentType:
				return thumbnailTemplate;
			case AnimationCard.ContentType:
				return heroCardTemplate;
			default:
				return heroCardTemplate;
			}
		}

	}
}
