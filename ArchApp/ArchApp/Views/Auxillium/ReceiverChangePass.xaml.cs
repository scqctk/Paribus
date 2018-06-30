using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArchApp.Views.Auxillium
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceiverChangePass : ContentPage
    {
        public ReceiverChangePass()
        {
            InitializeComponent();
        }
        public static string URL = "http://paribusproapi.ts77daczbt.us-west-2.elasticbeanstalk.com/";
        //public static string URL = "http://localhost:60346/";
        public static string sha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
        private async void submit_Clicked(object sender, EventArgs e)
        {
            // setting these values, by then the user is already logged in so one can use
            //Paribus.Views.Auxillium.ReceiverSignIn.currentUser. for any api requests to the server
            string newPssword = newPass.Text;
            string oldPassword = currPass.Text;
            string email = ReceiverSignIn.currentUser.userEmail;
            if (newPssword == "" || oldPassword == "")
            {
                await DisplayAlert("Error!", "One of the following items was not entered in!", "Close");
                return;
            }
            //




            // for ayush - you can do same thing as with donators to change password
            // probably best to not 
            var httpClient = new HttpClient();

            var token = await httpClient.GetStringAsync(String.Concat(URL, "tokenfoo/"));
            var values = JsonConvert.DeserializeObject<Dictionary<string, int>>(token);

            double convertedint = values["token"] + 0.2423;

            string tokenvar = convertedint.ToString();
            string fullhash = sha256(tokenvar);
            Dictionary<string, string> senderfoo = new Dictionary<string, string>
            {
                {"email", email},
                {"oldpassword", oldPassword},
                { "newpass", newPssword},
                {"hash",fullhash }
            };

            string json = JsonConvert.SerializeObject(senderfoo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await httpClient.PostAsync(String.Concat(URL, "changepassfoo/"), content);


            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                Debug.WriteLine("it Worked");
                ArchApp.Views.Auxillium.ReceiverSignIn.currentUser.password = newPssword;

                await Navigation.PushAsync(new ReceiverAccount());
            }
            else
            {
                await DisplayAlert("error", "error", "error");
            }
            // after successful change bring user back to main page for logged in receivers..

            //also change the logged in user's password


            // retrieve old password and username from current user
            // use the api security and whatnot to change the password here

        }
    }
}