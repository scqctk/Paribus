using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArchApp.Views.Donum
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordChange : ContentPage
    {
        private string newPassword;
        private string Username;
        public PasswordChange()
        {
            InitializeComponent();
        }

        private async Task Submit_Clicked(object sender, EventArgs e)
        {
            newPassword = newPass.Text;
            Username = ArchApp.Views.Donum.loginPage.currentUser;//we refer back to that static string to get the username
            // donator 
            bool result = await Services.DonatorFinder.PutDonator(newPassword, Username, currPass.Text);//we leverage the PutDonator method to change the password and display a success message if it worked and an error if it didn't
            if (result)
                await DisplayAlert("Success", "Your password has been changed!", "OK");
            else
                await DisplayAlert("Error", "An error has occured. Your password has not been changed!", "Try Again");
        }
    }
}