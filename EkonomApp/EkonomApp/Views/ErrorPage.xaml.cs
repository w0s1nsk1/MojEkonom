using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EkonomApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ErrorPage : ContentPage
	{
		public ErrorPage()
		{
			InitializeComponent ();
		}

        private void Button_Clicked(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}