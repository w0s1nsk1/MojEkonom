using HtmlAgilityPack;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EkonomApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatorPage : TabbedPage
    {
        public CreatorPage()
        {
            InitializeComponent();
            string url = "http://www.zse.srem.pl/plan_lekcji/a/lista.html";
            HtmlWeb web = new HtmlWeb();
            try
            {
                HtmlDocument htmldoc = @web.Load(url);
                if (htmldoc != null)
                {
                    var htmlNodes = htmldoc.DocumentNode.SelectNodes("//li/a[not(string-length(text())>3)]");
                    foreach (var node in htmlNodes)
                    {
                        string line = node.InnerHtml;
                        int x = Int32.Parse(line.Substring(0, 1));
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
                    }
                }
            }
            catch (System.Net.WebException)
            {
                DisplayAlert("Błąd", "Brak Dostępu do Internetu", "Ok");
            }
        }
        
        async void Next_Clicked(object sender, EventArgs e)
        {
            if (Classes.SelectedIndex != -1)
            {
                App.Current.Properties["Class"] = Classes.Items[Classes.SelectedIndex].ToLower();
                App.Current.Properties["ClassNumber"] = (Classes.SelectedIndex + 1).ToString();
                await App.Current.SavePropertiesAsync();
                Xamarin.Forms.Application.Current.MainPage = new MainPage();
            }
            else
            {
                await DisplayAlert("Błąd", "Musisz wybrać swoją klasę!", "Ok");
            }
        }

        private async void NextPage1_Clicked(object sender, EventArgs e)
        {
            var lastpage = CurrentPage;
            CurrentPage = Children[Children.IndexOf(CurrentPage)+1];
            await Task.Delay(250);
            Children.Remove(lastpage);
        }
    }
}