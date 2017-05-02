﻿using AdaptiveCards.Rendering;
using AdaptiveCards.Rendering.Config;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BotFramework.UI
{
    public class AdaptiveCardView : CardView
    {

        static HostConfig hostOptions;
        public static HostConfig HostOptions
        {
            get
            {
                return hostOptions ?? (hostOptions = new HostConfig
                {
                    ImageSizes =
                    {
                        Small = 30,
                        Medium = 60,
                        Large = 90,
                    },
                    //Column =
                    //{
                    //    Separation = {
                    //        Default =
                    //        {
                    //            Spacing = 5,
                    //        },
                    //        Strong =
                    //        {
                    //            Spacing = 10,
                    //        }
                    //    }
                    //},
                    //Separation =
                    //{
                    //    Default =
                    //    {
                    //        Spacing = 5
                    //    }
                    //},
                    //AdaptiveCard =
                    //{
                    //     Padding = new BoundaryOptions(2),
                    //},
                    
                });
            }
            set
            {
                hostOptions = value;
                renderer = null;
            }
        }
        static XamlRenderer renderer;
        public static XamlRenderer Renderer
        {
            get { return renderer ?? (renderer = new XamlRenderer(HostOptions, new ResourceDictionary(), null, null)); }
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