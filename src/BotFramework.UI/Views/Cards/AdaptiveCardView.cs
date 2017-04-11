using AdaptiveCards.Rendering;
using Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace BotFramework.UI
{
    public class AdaptiveCardView : CardView
    {
        static XamlRenderer renderer;
        public static XamlRenderer Renderer {
            get { return renderer ?? (renderer = new XamlRenderer(new AdaptiveCards.Rendering.RenderOptions(), new ResourceDictionary(), null, null)); }
            set { renderer = value; }
        }
    protected override void SetupView()
        {
            var message = (BindingContext as Attachment)?.Content as AdaptiveCardWrapper;
            if (message == null)
                return;
            Children.Add(Renderer.RenderAdaptiveCard(message.Card));

        }
    }
}
