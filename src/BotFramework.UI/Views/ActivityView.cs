using System;
using System.Linq;
using Xamarin.Forms;

namespace BotFramework.UI
{
	public class ActivityViewCell : ViewCell
	{
		public ActivityViewCell ()
		{
			View = new ActivityView ();
		}
	}
	public class ActivityView : ContentView
	{
		public static readonly BindableProperty ActivityTemplateProperty = BindableProperty.Create (nameof (ActivityTemplate), typeof (DataTemplate), typeof (ActivityView), new ActivityTemplateSelector());

		public DataTemplate ActivityTemplate {
			get { return (DataTemplate)GetValue (ActivityTemplateProperty); }
			set { SetValue (ActivityTemplateProperty, value); }
		}

		public static readonly BindableProperty SmallPaddingProperty = BindableProperty.Create (nameof (SmallPadding), typeof (double), typeof (ActivityView), 10.0);

		public double SmallPadding {
			get { return (double)GetValue (SmallPaddingProperty); }
			set { SetValue (SmallPaddingProperty, value); }
		}
		public static readonly BindableProperty LargePaddingProperty = BindableProperty.Create (nameof (LargePadding), typeof (double), typeof (ActivityView), 40.0);

		public double LargePadding {
			get { return (double)GetValue (LargePaddingProperty); }
			set { SetValue (LargePaddingProperty, value); }
		}

		public readonly Frame Frame;
		public Color IncomingColor = Color.FromHex("#03A9F4");
		public Color OutgoingColor = Color.FromHex ("#F5F5F5");

		Grid mainGrid;
		ColumnDefinition leftPaddingColumn;
		ColumnDefinition rightPaddingColumn;
		public ActivityView ()
		{
			this.ActivityTemplate = new ActivityTemplateSelector ();
			this.Content = mainGrid = new Grid {
				ColumnSpacing = 2,
				Padding = new Thickness(5),
				ColumnDefinitions = new ColumnDefinitionCollection {
					(leftPaddingColumn = new ColumnDefinition{
						Width = new GridLength(10),
					}),
					new ColumnDefinition {Width =  GridLength.Star},
					(rightPaddingColumn =new ColumnDefinition {Width = new GridLength(40)}),
				},
				RowDefinitions = new RowDefinitionCollection {
					new RowDefinition{ Height = GridLength.Auto},
					new RowDefinition { Height = GridLength.Star},
				},
			};
			mainGrid.Children.Add (Frame = new Frame {
				OutlineColor = Color.Transparent,
				HasShadow = false,
			}, 1, 1);

		}

		protected override void OnBindingContextChanged ()
		{
			ResetView ();
			base.OnBindingContextChanged ();
			SetupView ();
		}

		protected virtual void SetupView ()
		{
			var activity = BindingContext as ActivityBase;
			if (activity == null)
				return;
			//var view = (View)Activator.CreateInstance (typeof (MessageView));
			var view = ActivityTemplate?.CreateContent (activity,this) as View ?? new Label ();

			var messageView = view as IMessageContext;
			if (messageView != null)
				messageView.IsFromMe = activity.IsFromMe;

			view.BindingContext = activity;
			
			Frame.Content = view;
			leftPaddingColumn.Width = new GridLength (activity.IsFromMe ? LargePadding : SmallPadding);
			rightPaddingColumn.Width = new GridLength (activity.IsFromMe ? SmallPadding : LargePadding);
			Frame.BackgroundColor = activity.IsFromMe ?  OutgoingColor : IncomingColor;

		}


		protected virtual void ResetView ()
		{
			if(Frame.Content != null)
				Frame.Content = null;
		}

	}
	public static class DataTemplateExtensions
	{
		public static object CreateContent (this DataTemplate self, object item, BindableObject container)
		{
			var selector = self as DataTemplateSelector;
			if (selector != null) {
				self = selector.SelectTemplate (item, container);
			}
			return self.CreateContent ();
		}
	}
}
