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
            if (Current.Properties.ContainsKey("Class")&& Current.Properties.ContainsKey("ClassNumber"))
            {
                // if this is the first time, set it to "No" and load the
                // Main Page ,which will show at the first time use
                try
                {
                    string ClassNumber = App.Current.Properties["ClassNumber"].ToString();
                    string url = "http://www.zse.srem.pl/index.php?opcja=modules/plan_lekcji/pokaz_plan";
                    HtmlWeb web = new HtmlWeb();
                    HtmlDocument version = web.Load(url);
                    var versionNode = version.DocumentNode.SelectSingleNode("//div[@class='col-md-12']/a");
                    url = versionNode.GetAttributeValue("href", "http://www.zse.srem.pl/plan_lekcji/b/index.html");
                    string[] url_pieces = url.Substring(6).Split('/');
                    string ScheduleLetter = url_pieces[3];
                    if (!App.Current.Properties.ContainsKey("ScheduleLetter"))
                    {
                        App.Current.Properties["ScheduleLetter"] = ScheduleLetter;
                        Current.SavePropertiesAsync();
                        url = url.Replace("index", "plany/o" + ClassNumber);
                        HtmlDocument htmldoc = web.Load(url);
                        htmldoc.DocumentNode.InnerHtml = htmldoc.DocumentNode.SelectSingleNode("//table[@class='tabela']").InnerHtml;
                        htmldoc.Save(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/plan.html");
                    }
                    else
                    {
                        string LocalScheduleLetter = App.Current.Properties["ScheduleLetter"].ToString();
                        if (LocalScheduleLetter != ScheduleLetter)
                        {
                            App.Current.Properties["ScheduleLetter"] = ScheduleLetter;
                            Current.SavePropertiesAsync();
                            url = url.Replace("index", "plany/o" + ClassNumber);
                            HtmlDocument htmldoc = web.Load(url);
                            htmldoc.DocumentNode.InnerHtml = htmldoc.DocumentNode.SelectSingleNode("//table[@class='tabela']").InnerHtml;
                            htmldoc.Save(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/plan.html");
                        }
                    }
                }
                catch (Exception)
                {
                    MainPage = new MainPage();
                }
                finally
                {
                    MainPage = new MainPage();
                }
            }
            else
            {
                // If this is not the first time,
                // Go to the CreatorPage
                MainPage = new CreatorPage();
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
