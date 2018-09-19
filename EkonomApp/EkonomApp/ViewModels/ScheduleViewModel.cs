using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using HtmlAgilityPack;
using EkonomApp.Helpers;
using System.Linq;

namespace EkonomApp.ViewModels
{
    public class ScheduleList
    {
        public string Hour { get; set; }
        public string Subject { get; set; }
        public string Classroom { get; set; }
    }

    public class ScheduleViewModel : BaseViewModel
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
        public string Group
        {
            get { return Settings.Group; }
            set
            {
                if (Settings.Group == value)
                    return;
                Settings.Group = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ScheduleList> Schedule { get; set; }
        public Command Refresh { get; set; }
        public ScheduleViewModel()
        {

            Schedule = new ObservableCollection<ScheduleList>();
            Refresh = new Command(async () => await LoadChanges());
            Refresh.Execute("");
        }
        
        async Task LoadChanges()
        {
                if (IsBusy)
                    return;
                IsBusy = true;
                try
                {
                    Schedule.Clear();
                    DateTime date = DateTime.Today;
                    int weekday = (int)date.DayOfWeek;
                    string url = "http://www.zse.srem.pl/plan_lekcji/b/plany/o5.html";
                    HtmlWeb web = new HtmlWeb
                    {
                    CachePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    UsingCache = true
                    };
                    HtmlDocument htmldoc = await Task.Run(() => web.Load(url));
                    htmldoc.DocumentNode.InnerHtml = htmldoc.DocumentNode.OuterHtml.Replace("<br>", ";");
                    var htmlNodes = htmldoc.DocumentNode.SelectNodes("//td[@class='l']["+weekday+"]");
                    var hoursNodes = htmldoc.DocumentNode.SelectNodes("//td[@class='g']");
                    var hours = new string[10];
                    int i1 = 0;
                    foreach (var node in hoursNodes)
                    {
                        hours[i1] = node.InnerText.Replace("- ","-");
                        hours[i1] = hours[i1].Replace("-", " - ");
                        i1++;
                    }
                    i1 = 0;
                    foreach (var node in htmlNodes)
                    {
                        if(node.InnerText != "&nbsp;")
                        {
                            if (node.InnerText.Contains(";"))
                            {
                                Debug.WriteLine(node.InnerText);
                                string[] lessons = node.InnerText.Split(';');
                                for (int i = 0; i < lessons.Length; i++)
                                {
                                    string[] lesson = lessons[i].Split(' ');
                                    string subject;
                                    if (lesson[0].Length!=5)
                                        subject = lesson[0];
                                    else
                                    {
                                        subject = lesson[0]+lesson[1];
                                        lesson[2] = lesson[3];
                                    }
                                    if (subject.Contains('-'))
                                    {
                                        string[] grplesson = subject.Split('-');
                                        subject = grplesson[0];
                                        string group = grplesson[1];
                                        Debug.WriteLine(group);
                                        if (group == "1/2")
                                        {
                                            string classroom = lesson[2];
                                            Schedule.Add(new ScheduleList() { Hour = hours[i1], Subject = subject, Classroom = classroom });
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string[] lesson = node.InnerText.Split(' ');
                                string subject = lesson[0];
                                if (subject.Contains('-'))
                                {
                                    string[] grplesson = subject.Split('-');
                                    subject = grplesson[0];
                                    string group = grplesson[1];
                                    if (group == "1/2")
                                    {
                                        string classroom = lesson[2];
                                        Schedule.Add(new ScheduleList() { Hour = hours[i1], Subject = subject, Classroom = classroom });
                                    }
                                }
                                else
                                {
                                    string classroom = lesson[2];
                                    Schedule.Add(new ScheduleList() { Hour = hours[i1], Subject = subject, Classroom = classroom });
                                }
                            }
                        }
                        i1++;
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