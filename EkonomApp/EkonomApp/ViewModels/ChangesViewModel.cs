using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using HtmlAgilityPack;
using EkonomApp.Helpers;

namespace EkonomApp.ViewModels
{
    public class ChangeList
    {
        public string Changed { get; set; }
    }

    public class ChangesViewModel : BaseViewModel
    {
        public ObservableCollection<ChangeList> Change { get; set; }
        public Command Refresh { get; set; }
        public ChangesViewModel()
        {
            
            Change = new ObservableCollection<ChangeList>();
            Refresh = new Command(async () => await LoadChanges());
            Refresh.Execute("");
        }
        
        async Task LoadChanges()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            
                Change.Clear();
                DateTime date = DateTime.Today;
                var weekday = (int)date.DayOfWeek;
                if ((int)date.DayOfWeek >= 5)
                {
                    int ile = 8 - (int)date.DayOfWeek;
                    date = date.AddDays(ile);
                }
                else
                {
                    date = date.AddDays(1);
                }
                string MyClass = App.Current.Properties["Class"].ToString();
                string day = date.Day.ToString().PadLeft(2, '0');
                string monthday = date.Month.ToString().PadLeft(2, '0');
            try
            {
                string url = "http://www.zse.srem.pl/index.php?opcja=modules/zastepstwa/view_id&id=" + (int)date.DayOfWeek;
                HtmlWeb web = new HtmlWeb
                {
                    CachePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    UsingCache = true
                };
                HtmlDocument htmldoc = await Task.Run(() => web.Load(url));
                var html1 = htmldoc.DocumentNode.SelectNodes("//span[contains(text(),'Zastępstwa')]");
                if (html1 != null)
                {
                    string html1_title = html1[0].InnerHtml;
                    var two = html1.Count;
                    html1_title = html1_title.Replace("\r\n", " ");
                    if (html1_title.Contains(day + "." + monthday + "."))
                        html1_title = html1_title.Substring(0, html1_title.IndexOf(day + "." + monthday + ".") + 5);
                    Title = html1_title;
                    var htmlNodes = htmldoc.DocumentNode.SelectNodes("//p/span");
                    foreach (var node in htmlNodes)
                    {
                        string line = node.InnerHtml;
                        if (node.InnerHtml != "<br>")
                        {
                            if (line.Contains("\r\n"))
                                line = line.Replace("\r\n", " ");
                            if (line.Contains(monthday + "."))
                                line = line.Replace(monthday + ".", monthday);
                            if (line.IndexOf(" l.") != -1)
                            {
                                string Class = line.Substring(0, line.IndexOf(" l."));
                                Class = Class.Substring(4);
                                line = line.Substring(line.IndexOf(" l.") + 1);
                                if (Class.ToLower() == MyClass)
                                    Change.Add(new ChangeList() { Changed = line });
                            }
                        }
                        else
                            break;
                    }
                    if (Change.Count == 0)
                    {
                        Change.Add(new ChangeList() { Changed = "Brak zastępstw dla twojej klasy 😞" });
                    }
                }
                else
                {
                    Title = "Brak Zastępstw";
                    Change.Add(new ChangeList() { Changed = "Strona na jutrzejszy dzień zastępstw jest pusta 😞" });
                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}