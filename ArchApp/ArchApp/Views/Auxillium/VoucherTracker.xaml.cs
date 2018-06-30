
using Newtonsoft.Json;

using ArchApp.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArchApp.Views.Auxillium
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VoucherTracker : ContentPage
    {

        // we will need the first name, last name, and email of the user in order to send a gift card
        string firstName = ArchApp.Views.Auxillium.ReceiverSignIn.currentUser.firstName;
        string lastName = ArchApp.Views.Auxillium.ReceiverSignIn.currentUser.lastName;
        string email = ArchApp.Views.Auxillium.ReceiverSignIn.currentUser.userEmail;

        public VoucherTracker()
        {

            InitializeComponent();

            // passing new person through constructor in the page

            //System.Diagnostics.Debug.WriteLine(signer.socialSecurity);
            //string fullName = signer.firstName +  " " + signer.lastName;
            Greeting.Text = "Hello, " + firstName + " " + lastName + "!";


            // checking to see if the signer is the person who is currently logged in
        }


        private async void Food_Clicked(object sender, EventArgs e)
        {
            VoucherOutput.Text = "You have successfully selected the food voucher option!";
            VoucherOutput.IsVisible = true;
            //await DisplayAlert("Transaction", "A 50 Dollar Voucher has been created for you!", "Good");
            // alerting the user
            // need to draw money from the bank

            // new httpclient is created here, establishes connection with the API database of the bank

            // I will fix this part later

            /*
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("http://paribusapienv.3ux9m2mjwg.us-west-2.elasticbeanstalk.com/bank/?format=json");
            // creates a list of banks (one bank doesn't work as I have a list of one item, which is still a list)
            // so the total money is users[0] - the money there
            var users = JsonConvert.DeserializeObject<List<MainBank>>(response);
            int totalBalance = users[0].money;
            // alerting the money and subtracting 50 dollars of it
            RemainingBalance.Text = totalBalance.ToString();
            // now we have the money and we can subtract the necessary amount off of it, in this case, is an $50 value
            */

            // get the total amount from the giftbit credit card account
            string btoken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJTSEEyNTYifQ==.ck5uMDloSGhkKzBzOGpicWVxdi9UajNhR3hqbEVJN0t6SkFwRjMxamhaZzVQVDJjU0xTVFdESTcxYzJ0eHRpeEh5ZDJtY3pZRnMyNzQ3bmZ3T2FTTU5GNWxYNGNiWloreXVybHY0U3VEckxWMFNTRCtqM3BwUW9hZ3UzUHZSTzY=.aKvD0flORBGm5NlgciKBGjjq9NyPlFMhfVgaL9wrUAs=";
            // bearer token connected to my testbed account here
            HttpClient clienter = new HttpClient();
            //System.


            clienter.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", btoken);
            var userCards = await clienter.GetStringAsync("https://api-testbed.giftbit.com/papi/v1/funds");
            var totalFunds = JsonConvert.DeserializeObject<Funds>(userCards);
            int totalCents = totalFunds.fundsbycurrency.USD.available_in_cents;
            double totalBalance = totalCents / 100.0;
            // checking out the current amount of moey in the USD account associated with my giftbit account

            totalBalance = totalBalance - 50;

            if (totalBalance <= 0)
            {
                await DisplayAlert("Error!", "Server taking too long to respond!", "Close");//not enough money in the bank, the people don't need to know that
                return;

            }
            else
            {
                // creating a new main bank with the lowered money and using a put request to send that back to the database

                RemainingBalance.Text = totalBalance.ToString();


                /* uncomment if you want to generate a pdf for later
                PdfDocument document = new PdfDocument();
                //new document 
                //Add the page
                PdfPage page = document.Pages.Add();

                PdfGraphics graphics = page.Graphics;

                //Create a solid brush.

                PdfBrush brush = new PdfSolidBrush(Syncfusion.Drawing.Color.Black);

                PdfBrush brush1 = new PdfSolidBrush(Syncfusion.Drawing.Color.Red);

                //Set the font.

                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 18);

                //Draw the text.
                Random rnd = new Random();
                int userNumber = rnd.Next(1, 10000);

                // at this point we should have the email, first and last name here

                // a random number generated maybe a post request of the id of the user with the random number would make it unique
                string display = "";
                // usernumber will need to be unique
                string randUser = firstName + lastName +  userNumber.ToString();
                // random user number, in the future this will be based on the user passed in from a while ago

                graphics.DrawString("$50 Voucher PDF courtesy of Paribus! User " + randUser, font, brush, new PointF(20, 20));
                graphics.DrawString("This (Test) Voucher can be Used at:", font, brush, new PointF(20, 40));
                // creating a decent looking document
                graphics.DrawString("Walmart (for now)", font, brush1, new PointF(40, 60));
                graphics.DrawString("Notes:", font, brush1, new PointF(40, 80));
                graphics.DrawString("You cannot buy cigarettes, alcohol, etc here", font, brush1, new PointF(40, 100));
                graphics.DrawString("This is a food voucher", font, brush1, new PointF(40, 120));
                //graphics.DrawString("Trader Joe's", font, brush1, new PointF(40, 140));


                //graphics.DrawString("$50 Voucher PDF courtesy of Paribus! Enjoy!", font, brush, new PointF(20, 20));

                //Save and close the document.

                MemoryStream stream = new MemoryStream();
                document.Save(stream);
                // saving the document in the stream
                document.Close();
                // using the ISave to save for windows - just windows so far.
                await DependencyService.Get<ISave>().SaveTextAsync("ParibusFoodVoucher.pdf", "application/pdf", stream);
                */


                //System.ArgumentNullException in System.Runtime.WindowsRuntime.dll;
                string expiringDate = "2018-12-01";
                // the expiration date must be not one year ahead of when this gift was sent. 
                // that's a noob move!
                int priceCents = 50 * 100;
                string choiceBrand = "walmart";
                // just for now because of the testbed account

                List<Contact> receivers = new List<Contact>();
                Contact mattStrong = new Contact()
                {
                    firstname = firstName,
                    lastname = lastName,
                    email = email
                };

                string myFullName = firstName + " " + lastName;
                receivers.Add(mattStrong);
                List<string> brander = new List<string>();
                brander.Add(choiceBrand);

                string delivery = "GIFTBIT_EMAIL";
                string userMessage = "Greetings, " + myFullName + "! " + "Take this voucher gift card from Udana! You cannot spend it on items such as alcohol or cigarettes, etc.";
                string userSubject = "Udana Reception";

                Random rando = new Random();
                int randoInt = rando.Next(1, 10000);
                string idParam = randoInt.ToString();
                Campaign newCamp = new Campaign()
                {
                    contacts = receivers,

                    price_in_cents = priceCents,

                    brand_codes = brander,

                    expiry = expiringDate,

                    id = idParam,

                    delivery_type = delivery,

                    message = userMessage,
                    // message will show up in the email


                    subject = userSubject,
                    // id will need to be unique

                };

                var serializer = JsonConvert.SerializeObject(newCamp);

                //await DisplayAlert("Good Job", "A Reward has Been Sent", "Exit");
                var contentString = new StringContent(serializer, Encoding.UTF8, "application/json");


                // posting the result to this api which will generate the gift card voucher
                var campaignResult = await clienter.PostAsync("https://api-testbed.giftbit.com/papi/v1/campaign", contentString);
                System.Diagnostics.Debug.WriteLine(idParam);
                if (campaignResult.IsSuccessStatusCode)
                {
                    await DisplayAlert("Congratulations!", "Successful gift card generation for $50 food voucher!", "Close");
                }
                else
                {
                    await DisplayAlert("Error!", "Unsuccessful gift card generation!", "Try Again");

                }




                // 
                // need to know the current user who is logged in to send an email and generate useful id
                // requires binding context to know who is currently logged in 





                // displaying new balance
                // back to the home page
                // need something that ensures that people cannot spam click the food option and receive absurd amounts of money.
                // returning user back to main page
                await Navigation.PushAsync(new MainPage());
                // then create PDF/Gift card simulated of the user's transaction

            }

            // look in the database, find the already validated user, create some new file with SSN, money amount, etc.

            // should then direct user to new page based on their credentials.

            // will tell the user that they selected the option (from the previous login page, their SSN and full name
            // have already been validated 

            // do stuff when the button is clicked and the user is requesting for some kind of food voucher


        }
    }
}
