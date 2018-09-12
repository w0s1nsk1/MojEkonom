using EkonomApp.Helpers;
using EkonomApp.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EkonomApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClassPage : CarouselPage
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
        public string Class
        {
            get { return Settings.Class; }
            set
            {
                if (Settings.Class == value)
                    return;
                Settings.Class = value;
                OnPropertyChanged();
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
        public ClassPage()
        {
            InitializeComponent();
            string url = "http://www.zse.srem.pl/plan_lekcji/a/lista.html";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument htmldoc = web.Load(url);
            var htmlNodes = htmldoc.DocumentNode.SelectNodes("//li/a");
            foreach (var node in htmlNodes)
            {
                string line = node.InnerHtml;
                if (line == "P.Karliński (PK)")
                    break;
                int x = Int32.Parse(line.Substring(0, 1));
                switch(x)
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
            }
        }
        async void Next_Clicked(object sender, EventArgs e)
        {
            if ((!string.IsNullOrWhiteSpace(Number.Text)) && (Classes.SelectedIndex != -1))
            {
                LuckNumber = Number.Text;
                Class = Classes.Items[Classes.SelectedIndex].ToLower();
                IsFirstTime = "no";
                await Navigation.PushModalAsync(new MainPage());
            }
            else
            {
                if ((Classes.SelectedIndex == -1))
                {
                    await DisplayAlert("Błąd", "Musisz wybrać swoją klasę!", "Ok");
                }
                if (string.IsNullOrWhiteSpace(Number.Text))
                {
                    await DisplayAlert ("Błąd", "Musisz wpisać swój numer!", "Ok");
                }
            }
        }
    }
}