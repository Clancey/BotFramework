using System;
using Xamarin.Forms;

namespace BotFramework.UI
{
	public class ActivityTemplateSelector : Xamarin.Forms.DataTemplateSelector
	{
		protected DataTemplate messageTemplate;
		public ActivityTemplateSelector ()
		{
			// Retain instances!
			this.messageTemplate = new DataTemplate (typeof (MessageView));
		}

		protected override DataTemplate OnSelectTemplate (object item, BindableObject container)
		{
			return messageTemplate;
		}
	}
}
