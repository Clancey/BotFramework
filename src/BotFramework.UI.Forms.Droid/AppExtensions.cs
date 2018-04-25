using System;
using Android.Support.V4.App;
using Xamarin.Forms.Platform.Android;

namespace BotFramework.UI.Forms.Droid
{
    public static class AppExtensions
    {
        public static FragmentManager GetFragmentManager(this Android.Content.Context context)
        {
            if (context is FormsAppCompatActivity activity)
            {
                return activity.SupportFragmentManager;
            }
            return null;
        }
    }
}