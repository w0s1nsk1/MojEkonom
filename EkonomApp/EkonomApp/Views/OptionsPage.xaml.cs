using EkonomApp.Helpers;
using HtmlAgilityPack;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EkonomApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OptionsPage : ContentPage
    {
        public string Class = Xamarin.Forms.Application.Current.Properties["Class"].ToString();
        public OptionsPage()
        {
            InitializeComponent();
                        string url = "http://www.zse.srem.pl/plan_lekcji/a/lista.html";
            string xpath = "//ul";
            HtmlWeb web = new HtmlWeb
            {
                CachePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                UsingCache = true
            };
            HtmlDocument htmldoc = web.Load(url);
            var classes = htmldoc.DocumentNode.SelectSingleNode(xpath);
            xpath = "//li/a";
            htmldoc.LoadHtml(classes.InnerHtml);
            var htmlNodes = htmldoc.DocumentNode.SelectNodes(xpath);
                for(int i=0;i<htmlNodes.Count;i++)
                {
                    string line = htmlNodes[i].InnerHtml;
                    int x = int.Parse(line.Substring(0, 1));
                    switch (x)
                    {
                        case 1:
                            line = "I " + line.Substring(1);
                            break;
                        case 2:
                            line = "II " + line.Substring(1);
                            break;
                        case 3:
                            line = "III " + line.Substring(1);
                            break;
                        case 4:
                            line = "IV " + line.Substring(1);
                            break;
                    }
                    Classes.Items.Add(line);
                    if (line.ToLower() == Class)
                    {
                        Classes.SelectedIndex = i;
                    }
                }
        }
        public async void Save_Item(object sender, EventArgs e)
        {
            Xamarin.Forms.Application.Current.Properties["Class"] = Classes.Items[Classes.SelectedIndex].ToLower();
            Xamarin.Forms.Application.Current.Properties["ClassNumber"] = (Classes.SelectedIndex+1).ToString();
            await App.Current.SavePropertiesAsync();
            XFToast.ShortMessage("Pomyślnie zapisano!");
        }
    }
}
