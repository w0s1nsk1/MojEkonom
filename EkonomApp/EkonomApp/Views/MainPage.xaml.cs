using EkonomApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EkonomApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();
            int id = int.Parse(App.Current.Properties["startscreen"].ToString());
            switch (id)
            {
                case (int)MenuItemType.Changes:
                    Detail = new NavigationPage(new NewChangesPage());
                    break;
                case (int)MenuItemType.Schedule:
                    Detail = new NavigationPage(new NewSchedulePage());
                    break;
            }
            MasterBehavior = MasterBehavior.Popover;
            MenuPages.Add(id, (NavigationPage) Detail);
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Changes:
                        MenuPages.Add(id, new NavigationPage(new NewChangesPage()));
                        break;
                    case (int)MenuItemType.Schedule:
                        MenuPages.Add(id, new NavigationPage(new NewSchedulePage()));
                        break;
                    case (int)MenuItemType.Options:
                        MenuPages.Add(id, new NavigationPage(new OptionsPage()));
                        break;
                }
            }
            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(0);

                IsPresented = false;
            }
        }
    }
}