﻿using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HtmlAgilityPack;
using System.Threading.Tasks;

namespace EkonomApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangesPage : ContentPage
    {
        public ChangesPage()
        {
            InitializeComponent();
            ChangesList.RefreshCommand.Execute(null);
        }
    }
}

