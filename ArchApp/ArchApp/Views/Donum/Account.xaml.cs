using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArchApp.Views.Donum
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Account : ContentPage
    {
        public Account()
        {
            InitializeComponent();

        }

        private async void PsswdChange_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PasswordChange());
        }

        private async Task Donate_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Page1());
        }


    }
}