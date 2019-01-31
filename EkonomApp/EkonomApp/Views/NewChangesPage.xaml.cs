using HtmlAgilityPack;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EkonomApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewChangesPage : TabbedPage
    {
        public NewChangesPage()
        {
            InitializeComponent();
            DateTime date = DateTime.Now;
            TimeSpan now = date.TimeOfDay;
            int weekday = (int)date.DayOfWeek;
            if (App.Current.Properties.ContainsKey("Last1") && App.Current.Properties.ContainsKey("Last2") &&
                App.Current.Properties.ContainsKey("Last3") && App.Current.Properties.ContainsKey("Last4") &&
                App.Current.Properties.ContainsKey("Last5"))
            {
                if (weekday != 6)
                {
                    string[] lasthours = App.Current.Properties["Last" + (int)date.DayOfWeek].ToString().Split('-');
                    string hour = lasthours[1].Split(':')[0];
                    string minute = lasthours[1].Split(':')[1];
                    TimeSpan last = new TimeSpan(int.Parse(hour), int.Parse(minute), 0);
                    App.Current.Properties["ChangeState"] = 0;
                    App.Current.SavePropertiesAsync();
                    Children.Add(new ChangesPage() { Title = "Dzisiaj" });
                    App.Current.Properties["ChangeState"] = 1;
                    App.Current.SavePropertiesAsync();
                    if (((weekday + 1) < 6) || (weekday == 0))
                    {
                        Children.Add(new ChangesPage() { Title = "Jutro" });
                    }
                    else
                    {
                        Children.Add(new ChangesPage() { Title = "Poniedziałek" });
                    }
                    App.Current.Properties["ChangeState"] = 2;
                    App.Current.SavePropertiesAsync();
                    if (TimeSpan.Compare(now, last) >= 0)
                    {
                        CurrentPage = Children[1];
                    }
                }
                else
                {
                    App.Current.Properties["ChangeState"] = 0;
                    App.Current.SavePropertiesAsync();
                    Children.Add(new ChangesPage() { Title = "Poniedziałek" });
                }
            }
        }
    }
}