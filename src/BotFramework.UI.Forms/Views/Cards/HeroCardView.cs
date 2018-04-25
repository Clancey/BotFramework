using System;
using Xamarin.Forms;

namespace BotFramework.UI
{
    public class HeroCardView : CardView
    {
        private Label Title;
        private Label SubText;
        private Label Text;

        public HeroCardView()
        {
            Title = new Label();
            Title.SetBinding(Label.TextProperty, new Binding("Content.Title"));
            SubText = new Label();
            SubText.SetBinding(Label.TextProperty, new Binding("Content.Subtitle"));
            Text = new Label();
            Text.SetBinding(Label.TextProperty, new Binding("Content.Text"));
        }

        protected override void SetupView()
        {
            var attachment = BindingContext as Attachment;
            Title.TextColor = SubText.TextColor = IsFromMe ? Color.Black : Color.White;
            AddImages(attachment);
            Children.Add(Title);
            Children.Add(SubText);
            Children.Add(Text);
            AddButtons(attachment);
        }
    }
}
