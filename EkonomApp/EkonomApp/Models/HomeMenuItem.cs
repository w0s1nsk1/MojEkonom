using System;
using System.Collections.Generic;
using System.Text;

namespace EkonomApp.Models
{
    public enum MenuItemType
    {
        Changes,
        NewSchedule,
        Schedule,
        Options
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
