using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net;
using EkonomApp.Helpers;
using HtmlAgilityPack;

namespace EkonomApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LuckyPage : ContentPage
    {
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public string LuckNumber
        {
            get { return Settings.Number; }
            set
            {
                if (Settings.Number == value)
                    return;
                Settings.Number = value;
                OnPropertyChanged();
            }
        }
        public LuckyPage()
        {
            InitializeComponent();
            try
            {
                string url = "http://www.zse.srem.pl";
                HtmlWeb web = new HtmlWeb();
                HtmlDocument html = web.Load(url);
                var node = html.DocumentNode.SelectSingleNode("//h5/b");
                string LN = string.Empty;
                LN = node.InnerHtml;
                if (LN != "wstrzymano")
                {
                    LuckyNumber.Text = LN;
                    if (LN == LuckNumber)
                    {
                        LuckyNumber.TextColor = Color.FromHex("#90ee90");
                    }
                }
                else
                {
                    LuckyNumber.FontSize = 50;
                    LuckyNumber.Text = "Wstrzymano";
                }
            }
            catch 
            {
            }
        }
    }
}