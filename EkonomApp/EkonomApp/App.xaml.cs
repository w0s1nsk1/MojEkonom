using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EkonomApp.Views;
using System.Net;
using EkonomApp.Helpers;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace EkonomApp
{
    public partial class App : Application
    {
        public string IsFirstTime
        {
            get { return Settings.GeneralSettings; }
            set
            {
                if (Settings.GeneralSettings == value)
                    return;
                Settings.GeneralSettings = value;
                OnPropertyChanged();
            }
        }
        public App()
        {
            InitializeComponent();
            if (IsFirstTime != "no")
            {
                // if this is the first time, set it to "No" and load the
                // Main Page ,which will show at the first time use
                MainPage = new CreatorPage();
            }
            else
            {
                // If this is not the first time,
                // Go to the login page
                MainPage = new MainPage();
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
