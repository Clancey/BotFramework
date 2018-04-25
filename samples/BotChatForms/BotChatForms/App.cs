using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BotChatForms
{
    public class App : Application
    {
        public App()
        {
            // The root page of your applicatio
            MainPage = new BotChatPage();
        }

        protected override void OnStart()
        {
            // Handle when your app start
        }

        protected override void OnSleep()
        {
            // Handle when your app sleep
        }

        protected override void OnResume()
        {
            // Handle when your app resume
        }
    }
}