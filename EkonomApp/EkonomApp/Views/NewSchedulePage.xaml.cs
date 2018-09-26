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
        public Command Refresh { get; set; }
        public NewSchedulePage()
        {
            InitializeComponent();
            for (int i = 0; i < 5; i++)
            {
                Children.Add(new SchedulePage() { Title = days[i] });
            }
            ScheduleLoad(this, null);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            bool changed = bool.Parse(Xamarin.Forms.Application.Current.Properties["changed"].ToString());
            if (changed)
            {
                ScheduleLoad(this, null);
                Xamarin.Forms.Application.Current.Properties["changed"] = false;
                App.Current.SavePropertiesAsync();
            }
        }
        async void ScheduleLoad(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                App.Current.Properties["Day"] = (i + 1).ToString();
                Children[i] = new SchedulePage() { Title = days[i] };
                await App.Current.SavePropertiesAsync();
            }
            DateTime date = DateTime.Now;
            TimeSpan now = date.TimeOfDay;
            int weekday = (int)date.DayOfWeek;
            if (weekday != 0 && weekday != 6)
            {
                string[] lasthours = App.Current.Properties["Last" + (int)date.DayOfWeek].ToString().Split('-');
                string hour = lasthours[1].Split(':')[0];
                string minute = lasthours[1].Split(':')[1];
                Debug.WriteLine(hour + ":" + minute);
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
}