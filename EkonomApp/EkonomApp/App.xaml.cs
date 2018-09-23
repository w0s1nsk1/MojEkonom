using Xamarin.Forms.Xaml;
using EkonomApp.Views;
using EkonomApp.Helpers;
using HtmlAgilityPack;
using System.Linq;
using System;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace EkonomApp
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            InitializeComponent();

            if (App.Current.Properties.ContainsKey("Class")&& App.Current.Properties.ContainsKey("ClassNumber"))
            {
                // if this is the first time, set it to "No" and load the
                // Main Page ,which will show at the first time use
                if(App.Current.Properties.ContainsKey("ClassNumber").ToString() == App.Current.Properties.ContainsKey("Class").ToString())
                {
                    try
                    {
                        string url = "http://www.zse.srem.pl/plan_lekcji/a/lista.html";
                        HtmlWeb web = new HtmlWeb();
                        HtmlDocument htmldoc = web.Load(url);
                        if (Settings.Class.StartsWith("I "))
                            Settings.Class.Replace("I ", "1");
                        if (Settings.Class.StartsWith("II "))
                            Settings.Class.Replace("II ", "2");
                        if (Settings.Class.StartsWith("III "))
                            Settings.Class.Replace("III ", "3");
                        if (Settings.Class.StartsWith("IV "))
                            Settings.Class.Replace("IV ", "4");
                        var htmlNodes = htmldoc.DocumentNode.SelectNodes("//li/a[not(string-length(text())>3)]").ToArray();
                        for (int i = 0; i < htmlNodes.Length; i++)
                        {
                            if (htmlNodes[i].InnerHtml == Settings.Class)
                            {
                                App.Current.Properties["ClassNumber"] = (i + 1).ToString();
                                break;
                            }
                        }
                    }
                    catch (System.Net.WebException)
                    {
                        System.Environment.Exit(1);
                    }
                    App.Current.SavePropertiesAsync();
                }
                MainPage = new MainPage();
            }
            else
            {
                // If this is not the first time,
                // Go to the CreatorPage
                if (Settings.Class != string.Empty)
                {

                    App.Current.Properties["Class"] = Settings.Class;
                    try
                    {
                        string url = "http://www.zse.srem.pl/plan_lekcji/a/lista.html";
                        HtmlWeb web = new HtmlWeb();
                        HtmlDocument htmldoc = web.Load(url);
                        if (Settings.Class.StartsWith("I "))
                            Settings.Class.Replace("I ", "1");
                        if (Settings.Class.StartsWith("II "))
                            Settings.Class.Replace("II ", "2");
                        if (Settings.Class.StartsWith("III "))
                            Settings.Class.Replace("III ", "3");
                        if (Settings.Class.StartsWith("IV "))
                            Settings.Class.Replace("IV ", "4");
                        var htmlNodes = htmldoc.DocumentNode.SelectNodes("//li/a[not(string-length(text())>3)]").ToArray();
                        for (int i = 0; i < htmlNodes.Length; i++)
                        {
                            if (htmlNodes[i].InnerHtml == Settings.Class)
                            {
                                App.Current.Properties["ClassNumber"] = (i + 1).ToString();
                                break;
                            }
                        }
                    }
                    catch (System.Net.WebException)
                    {
                        System.Environment.Exit(1);
                    }
                    App.Current.SavePropertiesAsync();
                    MainPage = new MainPage();
                }
                else
                    MainPage = new CreatorPage();
            }
        }
private void DisplayAlert(string v1, string v2, string v3)
        {
            throw new NotImplementedException();
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
