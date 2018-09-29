using HtmlAgilityPack;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EkonomApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatorPage : CarouselPage
    {
        public CreatorPage()
        {
            InitializeComponent();
            string url = "http://www.zse.srem.pl/plan_lekcji/a/lista.html";
            HtmlWeb web = new HtmlWeb();
            try
            {
                HtmlDocument htmldoc = web.Load(url);
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
                Children[1] = new ErrorPage();
            }
        }
        
        async void Next_Clicked(object sender, EventArgs e)
        {
            if (Classes.SelectedIndex != -1)
            {
                try
                {
                    string ClassNumber = (Classes.SelectedIndex + 1).ToString();
                    string url = "http://www.zse.srem.pl/index.php?opcja=modules/plan_lekcji/pokaz_plan";
                    HtmlWeb web = new HtmlWeb();
                    HtmlDocument version = await Task.Run(() => web.Load(url));
                    var versionNode = version.DocumentNode.SelectSingleNode("//div[@class='col-md-12']/a");
                    url = versionNode.GetAttributeValue("href", "http://www.zse.srem.pl/plan_lekcji/b/index.html");
                    string[] url_pieces = url.Substring(6).Split('/');
                    string ScheduleLetter = url_pieces[3];
                    url = url.Replace("index", "plany/o" + ClassNumber);
                    HtmlDocument htmldoc = await Task.Run(() => web.Load(url));
                    htmldoc.DocumentNode.InnerHtml = htmldoc.DocumentNode.SelectSingleNode("//table[@class='tabela']").InnerHtml;
                    htmldoc.Save(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/plan.html");
                    App.Current.Properties["ScheduleLetter"] = ScheduleLetter;
                    Xamarin.Forms.Application.Current.Properties["Class"] = Classes.Items[Classes.SelectedIndex].ToLower();
                    Xamarin.Forms.Application.Current.Properties["ClassNumber"] = (Classes.SelectedIndex + 1).ToString();
                    Xamarin.Forms.Application.Current.Properties["changed"] = false;
                    await App.Current.SavePropertiesAsync();
                    Xamarin.Forms.Application.Current.MainPage = new MainPage();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
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