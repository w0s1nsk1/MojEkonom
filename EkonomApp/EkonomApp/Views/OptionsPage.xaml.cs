using EkonomApp.Helpers;
using HtmlAgilityPack;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
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
        async void LoadSchedule(object sender, EventArgs e)
        {
            if ((Xamarin.Forms.Application.Current.Properties["Class"].ToString() != (Classes.SelectedIndex + 1).ToString()) && (Xamarin.Forms.Application.Current.Properties["ClassNumber"].ToString() != (Classes.SelectedIndex + 1).ToString()))
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
                    App.Current.Properties["ScheduleLetter"] = ScheduleLetter;
                    await App.Current.SavePropertiesAsync();
                    url = url.Replace("index", "plany/o" + ClassNumber);
                    HtmlDocument htmldoc = await Task.Run(() => web.Load(url));
                    htmldoc.DocumentNode.InnerHtml = htmldoc.DocumentNode.SelectSingleNode("//table[@class='tabela']").InnerHtml;
                    htmldoc.Save(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/plan.html");
                    Xamarin.Forms.Application.Current.Properties["Class"] = Classes.Items[Classes.SelectedIndex].ToLower();
                    Xamarin.Forms.Application.Current.Properties["ClassNumber"] = (Classes.SelectedIndex + 1).ToString();
                    Xamarin.Forms.Application.Current.Properties["changed"] = true;
                    await App.Current.SavePropertiesAsync();
                    XFToast.ShortMessage("Pomyślnie zapisano!");
                }
                catch (Exception ex)
                {
                    XFToast.ShortMessage("Błąd podczas zapisywania.");
                    Debug.WriteLine(ex);
                }
            }
        }
    }
}
