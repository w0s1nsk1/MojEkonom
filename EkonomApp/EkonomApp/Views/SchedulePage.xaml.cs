using HtmlAgilityPack;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EkonomApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    
    public partial class SchedulePage : ContentPage
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
        public ObservableCollection<ScheduleList> Schedule { get; set; }
        public Command Refresh { get; set; }
        public SchedulePage()
        {
            InitializeComponent();
            Schedule = new ObservableCollection<ScheduleList>();
            Day.ItemsSource = Schedule;
            Refresh = new Command(async () => await LoadChanges());
            Day.RefreshCommand = Refresh;
            Refresh.Execute("");
        }
        
        public async Task LoadChanges()
        {
            try
            {
                Schedule.Clear();
                string DayTitle = App.Current.Properties["Day"].ToString();
                HtmlDocument htmldoc = new HtmlDocument();
                htmldoc.Load(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/plan.html");
                htmldoc.DocumentNode.InnerHtml = htmldoc.DocumentNode.InnerHtml.Replace("<br>", ";");
                var hours = htmldoc.DocumentNode.SelectNodes("//td[@class='g']").ToArray();
                for (int i = 0; i < hours.Count(); i++)
                {
                    hours[i].InnerHtml.Replace("- ", "-");
                    hours[i].InnerHtml.Replace("-", " - ");
                }
                var htmlNodes = htmldoc.DocumentNode.SelectNodes("//td[@class='l'][" + DayTitle + "]").ToArray();
                for (int i = 0; i < htmlNodes.Count(); i++)
                {
                    var node = htmlNodes[i];
                    if (node.InnerText != "&nbsp;")
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
                                if (lesson[0].Length == 5)
                                {
                                    lesson[0] = lesson[0] + lesson[1];
                                    lesson[2] = lesson[3];
                                }
                                if (lesson[0].Contains("r_"))
                                    lesson[0] = lesson[0].Replace("r_", "r. ");
                                if (lesson[0].Contains("j."))
                                    lesson[0] = lesson[0].Replace("j.", "j. ");
                                if (lesson[0].Contains("ang_k"))
                                {
                                    lesson[0] = lesson[0].Replace("ang_k", "angielski");
                                }
                                if (lesson[0].Contains("informat."))
                                {
                                    lesson[0] = lesson[0].Replace("informat.", "informatyka");
                                }
                                if (lesson[0].Contains("niem_d"))
                                {
                                    lesson[0] = lesson[0].Replace("niem_d", "niemiecki");
                                }
                                if (lesson[0].Contains('-'))
                                {
                                    string[] grplesson = lesson[0].Split('-');
                                    if (grplesson[0].Contains("wf"))
                                    {
                                        grplesson[0] = grplesson[0].Replace("wf", "W-f");
                                    }
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
                            if (group.Contains("/")&&!group.Contains(","))
                                Schedule.Add(new ScheduleList() { Hour = hours[i].InnerText, Subject = subject, Group = group, Classroom = classroom, Visible = true, Width = 1 });
                            else
                                Schedule.Add(new ScheduleList() { Hour = hours[i].InnerText, Subject = subject, Classroom = classroom, Visible = false, Width = 2 });
                        }
                        else
                        {
                            string icon = string.Empty;
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
                            if (lesson[0].Contains("niem_d"))
                                lesson[0] = lesson[0].Replace("niem_d", "niemiecki");
                            if (lesson[0].Contains("ang_k"))
                                lesson[0] = lesson[0].Replace("ang_k", "angielski");
                            if (lesson[0].Contains("pprzedsięb"))
                                lesson[0] = lesson[0].Replace("pprzedsięb", "p. przedsiębiorczości");
                            if (lesson[0].Contains("e_dla_bezp"))
                                lesson[0] = lesson[0].Replace("e_dla_bezp", "edukacja dla bezp.");
                            if (lesson[0].Contains("kultura"))
                                lesson[0] = lesson[0].Replace("kultura", "wiedza o kulturze");
                            if (lesson[0].Contains("wos"))
                                lesson[0] = lesson[0].Replace("wos", "wiedza o społ.");
                            string subject = lesson[0];
                            if (subject.Contains('-'))
                            {
                                string[] grplesson = subject.Split('-');
                                if (grplesson[0].Contains("wf"))
                                    grplesson[0] = grplesson[0].Replace("wf", "W-f");
                                if (grplesson[0].Contains("informat."))
                                    grplesson[0] = grplesson[0].Replace("informat.", "informatyka");
                                subject = grplesson[0];
                                string group = grplesson[1];
                                string classroom = lesson[2];
                                Schedule.Add(new ScheduleList() { Hour = hours[i].InnerText, Subject = subject, Group = group, Classroom = classroom, Visible = true, Width = 1 });
                            }
                            else
                            {
                                if (subject.Contains("wf"))
                                    subject = subject.Replace("wf", "W-f");
                                string classroom = lesson[2];
                                Schedule.Add(new ScheduleList() { Hour = hours[i].InnerText, Subject = subject, Classroom = classroom, Visible = false, Width = 2 });
                            }
                        }
                    }
                }
                App.Current.Properties["Last"+DayTitle] = Schedule.Last().Hour;
                await App.Current.SavePropertiesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}

