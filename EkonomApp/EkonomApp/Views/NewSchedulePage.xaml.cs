using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EkonomApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewSchedulePage : TabbedPage
    {
        string[] days = { "Pon", "Wt", "Śr", "Czw", "Pt" };
        public NewSchedulePage()
        {
            InitializeComponent();
            for (int i = 0; i < 5; i++)
            {
                App.Current.Properties["Day"] = (i + 1).ToString();
                Children.Add(new SchedulePage() { Title = days[i] });
                App.Current.SavePropertiesAsync();
            }
            DateTime date = DateTime.Now;
            TimeSpan now = date.TimeOfDay;
            int weekday = (int)date.DayOfWeek;
            if (App.Current.Properties.ContainsKey("Last1") && App.Current.Properties.ContainsKey("Last2") &&
                App.Current.Properties.ContainsKey("Last3") && App.Current.Properties.ContainsKey("Last4") &&
                App.Current.Properties.ContainsKey("Last5"))
            {
                if (weekday != 0 && weekday != 6)
                {
                    string[] lasthours = App.Current.Properties["Last" + (int)date.DayOfWeek].ToString().Split('-');
                    string hour = lasthours[1].Split(':')[0];
                    string minute = lasthours[1].Split(':')[1];
                    TimeSpan last = new TimeSpan(int.Parse(hour), int.Parse(minute), 0);
                    if (TimeSpan.Compare(now, last) >= 0)
                    {
                        weekday++;
                    }
                }
                if (weekday == 6 || weekday == 0)
                {
                    weekday = 1;
                }
                CurrentPage = Children[weekday - 1];
            }
        }
        protected override void OnAppearing()
        {
            bool changed = bool.Parse(Xamarin.Forms.Application.Current.Properties["changed"].ToString());
            if (changed)
            {
                Children.Clear();
                for (int i = 0; i < 5; i++)
                {
                    App.Current.Properties["Day"] = (i + 1).ToString();
                    Children.Add(new SchedulePage() { Title = days[i] });
                    App.Current.SavePropertiesAsync();
                }
                DateTime date = DateTime.Now;
                TimeSpan now = date.TimeOfDay;
                int weekday = (int)date.DayOfWeek;
                if (weekday != 0 && weekday != 6)
                {
                    string[] lasthours = App.Current.Properties["Last" + (int)date.DayOfWeek].ToString().Split('-');
                    string hour = lasthours[1].Split(':')[0];
                    string minute = lasthours[1].Split(':')[1];
                    TimeSpan last = new TimeSpan(int.Parse(hour), int.Parse(minute), 0);
                    if (TimeSpan.Compare(now, last) >= 0)
                    {
                        weekday++;
                    }
                }
                if (weekday == 6 || weekday == 0)
                {
                    weekday = 1;
                }
                CurrentPage = Children[weekday - 1];
                Xamarin.Forms.Application.Current.Properties["changed"] = false;
                App.Current.SavePropertiesAsync();
            }
        }
    }
}