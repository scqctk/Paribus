using System;
using System.Collections.Generic;
using ArchApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

/* important directives to take note of:
 * using Xamarin.Forms.Xaml enables you to take in the user inputted data and use them here
 * using Newtonsoft.Json allows you to use the newtonsoft json package so that you can use JsonConvert to deserialize a json file
 * using System.Net.Http allows you to create an Http client so that you can get a file from the web
 * */

namespace ArchApp.Views.Auxillium
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginReceive : ContentPage
    {
        public LoginReceive()
        {
            InitializeComponent();

            //this function is called to get the data of the valid receivers
            //GetValidReceivers();
        }






        private async void SignInProcedure(object sender, EventArgs e)
        {
            // signing in as an existing user.
            await DisplayAlert("Alert", "Sending you to the login page", "Close");

            // push to page where user can actually sign in instead of signing up if they already have an account
            await Navigation.PushAsync(new ReceiverSignIn());

        }



        private async void createApproved_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Alert", "Sending you to account creation", "Close");
            await Navigation.PushAsync(new W2Submit());

            // creating a new account - will bring user to page where they can submit images from their device to the W2 form!
            // brings user to new page then

        }

        private async void Exit_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Alert", "Sending you to the home page", "Close");
            await Navigation.PushAsync(new MainPage());

        }
    }
}