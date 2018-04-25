using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BotFramework.UI
{
    public class ActivityTemplateSelector : Xamarin.Forms.DataTemplateSelector
    {
        public static Dictionary<ActivityType, DataTemplate> TypeMappings = new Dictionary<ActivityType, DataTemplate>
        {
            {Message.ContentType,new DataTemplate (typeof (MessageView))},
        };

        protected DataTemplate defaultTemplate;

        public ActivityTemplateSelector()
        {
            // Retain instances!
            this.defaultTemplate = new DataTemplate(typeof(MessageView));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var activity = item as BotActivity;
            if (activity == null)
                return null;
            DataTemplate template = null;
            TypeMappings.TryGetValue(activity.Type, out template);
            return template ?? defaultTemplate;
        }
    }
}