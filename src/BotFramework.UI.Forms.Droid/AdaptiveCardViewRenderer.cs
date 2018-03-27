using System;
using Xamarin.Forms.Platform.Android;
using IO.Adaptivecards.Renderer;
using IO.Adaptivecards.Objectmodel;
using IO.Adaptivecards.Renderer.Actionhandler;
using Xamarin.Forms;
using System.Net;
using Android.Views;
using Android.Util;
using Android.App;

[assembly: ExportRenderer(typeof(BotFramework.UI.AdaptiveCardView), typeof(BotFramework.UI.Forms.Droid.AdaptiveCardViewRenderer))]
namespace BotFramework.UI.Forms.Droid
{
	public class AdaptiveCardViewRenderer : ViewRenderer<AdaptiveCardView, Android.Views.View>, ICardActionHandler
	{
		float Density = 1;
		public AdaptiveCardViewRenderer(Android.Content.Context context) : base(context)
		{
			var metrics = new DisplayMetrics();
			if (context is Activity activity)
			{
				activity.WindowManager.DefaultDisplay.GetMetrics(metrics);
				Density = metrics.Density;
			}
			
		}

		Android.Views.View renderedView;
		protected override void OnElementChanged(ElementChangedEventArgs<AdaptiveCardView> e)
		{
			base.OnElementChanged(e);
			try
			{
				var json = ((e.NewElement.BindingContext as Attachment).Content as AdaptiveCard).Json;
				ParseResult parseResult = IO.Adaptivecards.Objectmodel.AdaptiveCard.DeserializeFromString(json, AdaptiveCardRenderer.Version);
				var renderedCard = AdaptiveCardRenderer.Instance.Render(Context, Context.GetFragmentManager(), parseResult.AdaptiveCard, this);
				renderedView = renderedCard.View;
				renderedView.Measure((int)MeasureSpecMode.Unspecified, (int)MeasureSpecMode.Unspecified);
				SetNativeControl(renderedCard.View);
				Element.HeightRequest = renderedView.MeasuredHeight;
				Element.WidthRequest = renderedView.MeasuredWidth;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		public void OnAction(BaseActionElement p0, RenderedAdaptiveCard p1)
		{
			Console.WriteLine("Action!!");
		}

		//protected override void OnLayout(bool changed, int l, int t, int r, int b)
		//{
		//	base.OnLayout(changed, l, t, r, b);
		//	if (!changed)
		//		return;
		//	var width = r - l;
		//	var height = b - t;
		//	renderedView.Measure(width, height);
		//	Element.HeightRequest = renderedView.MeasuredHeight / Density;
		//}

	}
}
