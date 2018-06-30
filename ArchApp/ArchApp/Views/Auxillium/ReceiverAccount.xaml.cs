using ArchApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArchApp.Views.Auxillium
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceiverAccount : ContentPage
    {
        public ReceiverAccount()
        {
            // this page is the main base basically for the receivers who have SUCCESSFULLY logged in 
            InitializeComponent();
        }




        private async void changePass_Clicked(object sender, EventArgs e)
        {
            // changing password
            await Navigation.PushAsync(new ReceiverChangePass());

        }

        private async void receive_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VoucherTracker());

        }

        private async void Exit_Clicked(object sender, EventArgs e)
        {
            // going home and exiting
            await Navigation.PushAsync(new MainPage());

        }
    }
}