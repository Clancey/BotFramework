using System;
using Xamarin.Forms;

namespace BotFramework.UI
{
    public static class BindingsExtensions
    {
        public static T SetBindings<T>(this T view, BindableProperty target, string path) where T : BindableObject
        {
            view.SetBinding(target, path);
            return view;
        }
    }
}