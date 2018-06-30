using Newtonsoft.Json;
using ArchApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArchApp.Views.Auxillium
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceiverSignIn : ContentPage
    {
        public ReceiverSignIn()
        {
            InitializeComponent();
        }
        // setting these to null
        public static User currentUser = new User();
        public static int currentUserId = -1;
        //public static string userEmail = null;
        // I have first and last name to make it easy to show which name is logged in - saying something like,
        // "hello Evan Simmons!"
        //public static string userFirstName = null;
        //public static string userLastName = null;
        public static string URL = "http://paribusproapi.ts77daczbt.us-west-2.elasticbeanstalk.com/";
        //public static string URL = "http://localhost:60346/";

        private async void ReceiverLogin_Clicked(object sender, EventArgs e)
        {

            if (Email.Text == "" || Password.Text == "")
            {
                await DisplayAlert("Error!", "One or more fields was empty!", "Close");
                return;
            }
            string personEmail = Email.Text;
            bool first = personEmail.Contains("@");
            bool second = personEmail.Contains(".com");
            bool third = personEmail.Contains(".net");
            bool fourth = personEmail.Contains(".edu");
            bool fifth = personEmail.Contains(".gov");
            // anything past this point will have passwords that are not empty and match, and the email is valid and has enough to pass as an email
            if (first && (second || third || fourth || fifth))
            {
                //
                // will check if the user's credentials are valid when they have logged in.

                // *********************************
                // make the secure requests here!
                // somewhere once the user is approved: this code must be here obviously:
                // once things are successful too 
                // initialize the three static strings to something

                currentUser.userEmail = personEmail;
                currentUser.password = Password.Text;


                Dictionary<string, string> receiverInfoDict = new Dictionary<string, string>
                {
                    {"email",currentUser.userEmail },
                    {"password",currentUser.password }
                };
                string json = JsonConvert.SerializeObject(receiverInfoDict);
                var content = new StringContent(json, Encoding.UTF8, "application/json");//formatting the string as JSON
                var httpClient = new HttpClient();
                var result = await httpClient.PostAsync(String.Concat(URL, "authfoo/"), content);//change this later
                var contentstr = await result.Content.ReadAsStringAsync();
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(contentstr);
                string converteddict = dict.ToString();
                if (result.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    currentUser.userEmail = dict["email"];
                    currentUser.password = dict["password"];
                    currentUser.firstName = dict["firstName"];
                    currentUser.lastName = dict["lastName"];

                    currentUserId = Convert.ToInt32(dict["id"]);
                    await Navigation.PushAsync(new ReceiverAccount());
                    return;
                }
                else
                {
                    await DisplayAlert("Error!", "Wrong password or email!", "Close");
                    return;
                }



                // ********************************

            }
            else
            {
                // not a valid email
                await DisplayAlert("Error!", "This is not a valid email that we can check! Try again!", "Close");
                return;


            }
        }
    }

}