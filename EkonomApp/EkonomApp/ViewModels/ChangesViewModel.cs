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
        public string Class { get; set; }
        public string Changed { get; set; }
        public string Color { get; set; }
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
        public ObservableCollection<ChangeList> change { get; set; }
        public Command Refresh { get; set; }
        public int weekday;
        public ChangesViewModel()
        {
            Title = "Zastępstwa";
            change = new ObservableCollection<ChangeList>();
            Refresh = new Command(async () => await LoadChanges());
            Refresh.Execute("");
        }
        
        async Task LoadChanges()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            
                change.Clear();
                DateTime date = DateTime.Today;
                weekday = (int)date.DayOfWeek;
                if (weekday>=5)
                {
                    weekday = 0;
                }
                try
                {
                string url = "http://www.zse.srem.pl/index.php?opcja=modules/zastepstwa/view_id&id=" + (weekday + 1);
                HtmlWeb web = new HtmlWeb();
                HtmlDocument htmldoc = await Task.Run(() => web.Load(url));
                var htmlNodes = htmldoc.DocumentNode.SelectNodes("//p/span");
                foreach (var node in htmlNodes)
                {
                    string line = node.InnerHtml;
                    if(line.Contains("\r\n"))
                        line = line.Replace("\r\n", " ");
                    if (line.IndexOf(" l.") != -1)
                    {
                            string Class = line.Substring(0, line.IndexOf(" l."));
                            Class = Class.Substring(4);
                            line = line.Substring(line.IndexOf(" l.") + 1);
                            if (Class.ToLower() != Class1)
                                change.Add(new ChangeList() { Changed = line, Class = Class, Color = "#fff" });
                            else
                                change.Add(new ChangeList() { Changed = line, Class = Class, Color = "#90ee90" });
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