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
        public LuckyPage()
        {
                InitializeComponent();
                var loader = new Loader
                {
                        url = "http://zse.srem.pl",
                        xpath = "//h5/b"
                };
                HtmlNode DigitNode = loader.GetNode();
                if(DigitNode!=null)
                {
                    string Digit = DigitNode.InnerHtml;
                    if (Digit != "wstrzymano")
                    {
                        LuckyNumber.Text = Digit;
                    }
                }
        }
    }
}