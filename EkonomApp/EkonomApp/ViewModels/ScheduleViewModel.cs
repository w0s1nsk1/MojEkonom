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
        public string Group { get; set; }
        public int Width { get; set; }
        public bool Visible { get; set; }
    }

    public class ScheduleViewModel : BaseViewModel
    {
        
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
                    string ClassNumber = Xamarin.Forms.Application.Current.Properties["ClassNumber"].ToString();


                    string url = "http://www.zse.srem.pl/index.php?opcja=modules/plan_lekcji/pokaz_plan";
                    HtmlWeb web = new HtmlWeb
                    {
                    CachePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    UsingCache = true
                    };
                    HtmlDocument version = await Task.Run(() => web.Load(url));
                    var versionNode = version.DocumentNode.SelectSingleNode("//div[@class='col-md-12']/a");
                    url = versionNode.GetAttributeValue("href", "http://www.zse.srem.pl/plan_lekcji/b/index.html");
                    url = url.Replace("index", "plany/o" + ClassNumber);
                    Debug.WriteLine(url);
                    HtmlDocument htmldoc = await Task.Run(() => web.Load(url));
                    htmldoc.DocumentNode.InnerHtml = htmldoc.DocumentNode.OuterHtml.Replace("<br>", ";");
                    
                    var hours = htmldoc.DocumentNode.SelectNodes("//td[@class='g']").ToArray();
                    int hour = int.Parse(hours.Last().InnerHtml.Substring(0, 2));
                    
                    if(date.Hour>=hour)
                    {
                        date.AddDays(1);
                    }
                    int weekday = (int)date.DayOfWeek;
                    if (weekday == 6|| weekday == 0)
                        weekday = 1;
                    for(int i=0;i<hours.Count();i++)
                    {
                        hours[i].InnerHtml.Replace("- ","-");
                        hours[i].InnerHtml.Replace("-"," - ");
                    }
                    var htmlNodes = htmldoc.DocumentNode.SelectNodes("//td[@class='l'][" + weekday + "]").ToArray();
                    for(int i=0;i<htmlNodes.Count();i++)
                    {
                        var node = htmlNodes[i];
                        if(node.InnerText != "&nbsp;") 
                        {
                            if (node.InnerText.Contains(";"))
                            {
                                Debug.WriteLine(node.InnerText);
                                string[] lessons = node.InnerText.Split(';');
                                string subject = string.Empty;
                                string classroom = string.Empty;
                                string group = string.Empty;
                                for (int i1 = 0; i1 < lessons.Length; i1++)
                                {
                                    
                                    string[] lesson = lessons[i1].Split(' ');
                                    if (lesson[0].Length==5)
                                    {
                                        lesson[0] = lesson[0]+lesson[1];
                                        lesson[2] = lesson[3];
                                    }
                                    if (lesson[0].Contains("r_"))
                                        lesson[0] = lesson[0].Replace("r_", "r. ");
                                    if (lesson[0].Contains("j."))
                                        lesson[0] = lesson[0].Replace("j.", "j. ");
                                    if (lesson[0].Contains("ang_k"))
                                        lesson[0] = lesson[0].Replace("ang_k", "angielski");
                                    if (lesson[0].Contains("informat."))
                                        lesson[0] = lesson[0].Replace("informat.", "informatyka");
                                    if (lesson[0].Contains("niem_d"))
                                        lesson[0] = lesson[0].Replace("niem_d", "niemiecki");
                                if (lesson[0].Contains('-'))
                                    {
                                        string[] grplesson = lesson[0].Split('-');
                                        if (grplesson[0].Contains("wf"))
                                            grplesson[0] = grplesson[0].Replace("wf", "W-f");
                                        if (!string.IsNullOrWhiteSpace(subject))
                                            subject = subject + "\n" + grplesson[0];
                                        else
                                            subject = grplesson[0];
                                        if (!string.IsNullOrWhiteSpace(group))
                                            group = group + "\n" + grplesson[1];
                                        else
                                            group = grplesson[1];
                                        if (!string.IsNullOrWhiteSpace(classroom))
                                            classroom = classroom + "\n" + lesson[2];
                                        else
                                            classroom = lesson[2];
                                    }
                                }
                                if(group.Contains("/"))
                                    Schedule.Add(new ScheduleList() { Hour = hours[i].InnerText, Subject = subject, Group = group, Classroom = classroom, Visible = true, Width = 1 });
                                else
                                Schedule.Add(new ScheduleList() { Hour = hours[i].InnerText, Subject = subject, Classroom = classroom, Visible = false, Width = 2 });
                        }
                            else
                            {
                                string[] lesson = node.InnerText.Split(' ');
                                if (lesson.Length != 3)
                                {
                                    lesson[0] = lesson[0] + lesson[1];
                                    lesson[2] = lesson[3];
                                }
                                if (lesson[0].Contains("r_"))
                                    lesson[0] = lesson[0].Replace("r_", "r. ");
                                if (lesson[0].Contains("j."))
                                    lesson[0] = lesson[0].Replace("j.", "j. ");
                                if (lesson[0].Contains("u_hist.isp."))
                                    lesson[0] = lesson[0].Replace("u_hist.isp.", "historia i społeczeństwo");
                                if (lesson[0].Contains("Fiz_inż"))
                                    lesson[0] = lesson[0].Replace("Fiz_inż", "fizyka inż.");
                                if (lesson[0].Contains("zaj.wych"))
                                    lesson[0] = lesson[0].Replace("zaj.wych", "zajęcia wychowawcze");
                                string subject = lesson[0];
                                if (subject.Contains('-'))
                                {
                                    string[] grplesson = subject.Split('-');
                                    if (grplesson[0].Contains("wf"))
                                        grplesson[0] = lesson[0].Replace("wf", "W-f");
                                    if (grplesson[0].Contains("ang_k"))
                                        grplesson[0] = grplesson[0].Replace("ang_k", "angielski");
                                    if (grplesson[0].Contains("informat."))
                                        grplesson[0] = grplesson[0].Replace("informat.", "informatyka");
                                    if (grplesson[0].Contains("niem_d"))
                                        grplesson[0] = grplesson[0].Replace("niem_d", "niemiecki");
                                    subject = grplesson[0];
                                    string group = grplesson[1];
                                    string classroom = lesson[2];
                                    Schedule.Add(new ScheduleList() { Hour = hours[i].InnerText, Subject = subject,Group = group, Classroom = classroom, Visible = true,Width= 1 });
                                }
                                else
                                {
                                    if (subject.Contains("wf"))
                                    subject = subject.Replace("wf", "W-f");
                                    string classroom = lesson[2];
                                    Schedule.Add(new ScheduleList() { Hour = hours[i].InnerText, Subject = subject, Classroom = classroom, Visible= false, Width = 2 });
                                }
                            }
                        }
                    }
                    var weekNodes = htmldoc.DocumentNode.SelectNodes("//th[not(text()='Nr')][not(text()='Godz')]").ToArray();
                    if (weekday != 3)
                        Title = "Plan Lekcji na " + weekNodes[weekday-1].InnerText;
                    else
                        Title = "Plan Lekcji na " + weekNodes[weekday-1].InnerText.Replace('a', 'ę');
                IsBusy = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}