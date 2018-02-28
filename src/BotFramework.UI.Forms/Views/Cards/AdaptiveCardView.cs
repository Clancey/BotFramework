#if AdaptiveCard
using AdaptiveCards.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BotFramework.UI
{
    public class AdaptiveCardView : CardView
    {
        static AdaptiveCardRenderer renderer;
        public static XamlRenderer Renderer
        {
            get { return renderer ?? (renderer = new XamlRenderer(new AdaptiveCards.Rendering.RenderOptions(), new ResourceDictionary(), null, null)); }
            set { renderer = value; }
        }
        protected override void SetupView()
        {
            var message = (BindingContext as Attachment)?.Content as AdaptiveCard;
            if (message == null)
                return;
            Children.Add(Renderer.RenderAdaptiveCard(message));

        }
    }
}
#endif