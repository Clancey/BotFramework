using System;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Xamarin.Forms;
using AdaptiveCards.Rendering.Xamarin.iOS;

[assembly: ExportRenderer(typeof(BotFramework.UI.AdaptiveCardView), typeof(BotFramework.UI.Forms.iOS.AdaptiveCardViewRenderer))]
namespace BotFramework.UI.Forms.iOS
{
	public class AdaptiveCardViewRenderer : ViewRenderer<AdaptiveCardView, UIView>
	{
		public static void Init()
		{

		}
		protected override void OnElementChanged(ElementChangedEventArgs<AdaptiveCardView> e)
		{
			base.OnElementChanged(e);

			var json = ((e.NewElement.BindingContext as Attachment).Content as AdaptiveCard).Json;
			var card = ACOAdaptiveCard.FromJson(json);
			if (!card.IsValid)
				return;
			var hostConfig = new ACOHostConfig();
			//hostConfig.Init();
			var parseResult = ACRRenderer.Render(card.Card, 
			                                     hostConfig,
			                                     //ACOHostConfig.FromJson(hostconfig).Config,
			                                     new CoreGraphics.CGRect(10,
																			  UIScreen.MainScreen.Bounds.Size.Height * 0.1f + 1,
																			  UIScreen.MainScreen.Bounds.Size.Width - 20,
																				   UIScreen.MainScreen.Bounds.Size.Height * 0.9f - 2));
			SetNativeControl(parseResult.View);
			Element.HeightRequest = parseResult.View.Frame.Height;
			Element.WidthRequest = parseResult.View.Frame.Width;
		}
	}
}
