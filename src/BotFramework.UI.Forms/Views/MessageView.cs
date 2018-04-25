using System;
using System.Linq;
using Xamarin.Forms;

namespace BotFramework.UI
{
    public class MessageView : StackLayout, IMessageContext
    {
        public static readonly BindableProperty AttachmentTemplateProperty = BindableProperty.Create(nameof(AttachmentTemplate), typeof(DataTemplate), typeof(ActivityView), new CardTemplateSelector());

        public DataTemplate AttachmentTemplate
        {
            get { return (DataTemplate)GetValue(AttachmentTemplateProperty); }
            set { SetValue(AttachmentTemplateProperty, value); }
        }

        public bool IsFromMe { get; set; }
        public ViewCell HostingCell { get; set; }

        private Label Text;

        public MessageView()
        {
            Text = new Label();
            Text.SetBinding(Label.TextProperty, new Binding("Text"));
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            ResetView();
            SetupView();
        }

        protected virtual void SetupView()
        {
            Children.Add(Text);
            var message = BindingContext as Message;
            if (message == null)
                return;
            Text.TextColor = IsFromMe ? Color.Black : Color.White;
            if (message.Attachments != null)
                foreach (var a in message.Attachments)
                {
                    var child = CreateView(a);
                    child.SizeChanged += Child_SizeChanged;
                    Children.Add(child);
                }
        }

        protected virtual View CreateView(Attachment attachement)
        {
            var view = AttachmentTemplate?.CreateContent(attachement, this) as View ?? new Label();
            var messageView = view as IMessageContext;
            if (messageView != null)
            {
                messageView.IsFromMe = IsFromMe;
                messageView.HostingCell = HostingCell;
            }

            view.BindingContext = attachement;
            return view;
        }

        protected virtual void ResetView()
        {
            foreach (var childView in Children)
                childView.SizeChanged -= Child_SizeChanged;

            Children.Clear();
        }

        private void Child_SizeChanged(object sender, EventArgs e)
        {
            HostingCell?.ForceUpdateSize();
        }
    }
}
