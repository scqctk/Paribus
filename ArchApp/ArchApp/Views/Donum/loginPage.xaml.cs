using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArchApp.Views.Donum
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class loginPage : ContentPage
    {
        private Entry loginName = new Entry();
        private Entry loginPassword = new Entry();//unncessary code that satish and I wrote when we were learning xamarin
        private Button loginButton = new Button();
        public static string currentUser = null;//this static string is for referring back to later in the other screens to pull up account information for the donator
        public loginPage()
        {
            InitializeComponent();
            loginName = Login;
            loginPassword = Password;
            loginButton = LoginButton;
        }

        private async Task LoginButton_Clicked(object sender, EventArgs e)
        {
            bool found = await Services.DonatorFinder.GetDonatorInfo(Login.Text, Password.Text);


            if (!found)
            {
                await Error();//display an error alert if they were not found
            }
            else
            {
                string name = Login.Text;
                await Greet(name);
                currentUser = name;
            }
            //here we set a static string currentUser equal to the username of the donator to use it later to pull up account information for that user
        }

        private async Task Error()
        {
            await DisplayAlert("Error!", "Invalid username or password!", "Try Again");
        }

        private async Task Greet(string name)
        {
            await DisplayAlert("Welcome", "Welcome " + name + "!", "OK");//just a greeting
            await Navigation.PushAsync(new Views.Donum.Account());
        }

        private async void NewUser_Clicked(object sender, EventArgs e)
        {
            bool success = await DonatorFinder.MakeUser(loginName.Text, loginPassword.Text);//if the user selects sign up we just set a bool equal to the result of the POST method from Donator services and display an alert based on wheter or not it worked
            if (success)
            {
                await DisplayAlert("Success!", "Your account has been created successfully!", "OK");
                return;
            }
            else
                await DisplayAlert("Error!", "Your account has not been created successfully!", "OK");
            return;
        }
    }
}