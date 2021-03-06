﻿using System;
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
        }
        
        async Task LoadChanges()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            
                Change.Clear();
                DateTime date = DateTime.Today;
                if (App.Current.Properties["ChangeState"].ToString() != "2")
                {
                    Title = App.Current.Properties["ChangeState"].ToString();
                }
                int state = Int32.Parse(Title);
                date = date.AddDays(state);
                var weekday = (int)date.DayOfWeek;
                if ((int)date.DayOfWeek > 5)
                {
                    int ile = 8 - (int)date.DayOfWeek;
                    date = date.AddDays(ile);
                }
                string MyClass = App.Current.Properties["Class"].ToString();
                string day = date.Day.ToString();
                string monthday = date.Month.ToString().PadLeft(2, '0');
            try
            {
                string url = "http://www.zse.srem.pl/index.php?opcja=modules/zastepstwa/view_id&id=" + (int)date.DayOfWeek;
                HtmlWeb web = new HtmlWeb
                {
                    CachePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+"/Change"+state,
                    UsingCache = true
                };
                HtmlDocument htmldoc = await Task.Run(() => web.Load(url));
                var html1 = htmldoc.DocumentNode.SelectNodes("//span[contains(text(),'Zastępstwa')]");
                if (html1 != null)
                {
                    string html1_title = html1[0].InnerText;
          
                    html1_title = html1_title.Replace(" ", "");
                    Debug.WriteLine(html1_title);
                    if (html1_title.Contains(day + "." + monthday))
                    {
                        var htmlNodes = htmldoc.DocumentNode.SelectNodes("//p//span");
                        foreach (var node in htmlNodes)
                        {
                            if (node.SelectSingleNode("//strike") != null)
                            {
                                node.SelectSingleNode("//strike").Remove();
                            }
                            string line = node.InnerText;
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
                                    {
                                        Change.Add(new ChangeList() { Changed = line });
                                    }
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
                        Change.Add(new ChangeList() { Changed = "Zastępstwa na ten dzień nie zostały jeszcze wpisane 😞" });
                    }
                }
                else
                {
                    Change.Add(new ChangeList() { Changed = "Strona zastępstw na ten dzień jest pusta 😞" });
                }
            }
            catch (System.Net.WebException)
            {
                Change.Add(new ChangeList() { Changed = "Brak połączenia z serwerem 🚫" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            IsBusy = false;
        }
    }
}