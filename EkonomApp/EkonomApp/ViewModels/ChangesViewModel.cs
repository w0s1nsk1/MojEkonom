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
        public string Class1
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
                if ((int)date.DayOfWeek == 5)
                {
                    date = date.AddDays(3);
                }
                if ((int)date.DayOfWeek == 6)
                {
                    date = date.AddDays(2);
                }
                if ((int)date.DayOfWeek == 7)
                {
                    date = date.AddDays(1);
                }
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
                var html1 = htmldoc.DocumentNode.SelectNodes("//span[contains(text(),'Zastępstwa na')]");
                string html1_title = html1[0].InnerHtml;
                var two = html1.Count;

                if (html1_title.Contains("\r\n"))
                    html1_title = html1_title.Replace("\r\n", " ");
                if (html1_title.Contains(day +"."+ monthday + ". "))
                    html1_title = html1_title.Substring(0,html1_title.IndexOf(day + "." + monthday + ".") +5);
                Title = html1_title;
                HtmlNodeCollection htmlNodes;
                if (two != 1)
                {
                    htmlNodes = htmldoc.DocumentNode.SelectNodes("//span/p/span");
                }
                else
                {
                    htmlNodes = htmldoc.DocumentNode.SelectNodes("//p/span");
                }
                foreach (var node in htmlNodes)
                {
                    string line = node.InnerHtml;
                    if(line.Contains("\r\n"))
                        line = line.Replace("\r\n", " ");
                    if (line.Contains(monthday + "."))
                        line = line.Replace(monthday + ".", monthday);
                    if (line.IndexOf(" l.") != -1)
                    {
                            string Class = line.Substring(0, line.IndexOf(" l."));
                            Class = Class.Substring(4);
                            line = line.Substring(line.IndexOf(" l.") + 1);
                        if (Class.ToLower() == Class1)
                            Change.Add(new ChangeList() { Changed = line});
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}