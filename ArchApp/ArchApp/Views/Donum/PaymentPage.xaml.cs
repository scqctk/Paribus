using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;

using ArchApp.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace ArchApp.Views.Donum
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentPage : ContentPage
    {
        private Entry paymentAmount = new Entry();//payment amount entry -> user input
        private Button paymentButton = new Button();//submit button to process the transaction

        public PaymentPage()
        {
            InitializeComponent();
            paymentAmount = Donation_Amount;//payment amount entry -> user input
            paymentButton = Submit_Donation;//submit button to process the transaction
        }

        //Button Clicked Function to process the transaction
        private async void Submit_Donation_Clicked(object sender, EventArgs e)
        {
            double someVar = 0.0;//set a default value someVar to 0.0
            try
            {
                someVar = Convert.ToDouble(paymentAmount.Text);//tries to convert the user input to double
            }
            finally
            {
                if (someVar == 0.0 || someVar < 0)//if the user input fails to convert to double, or if it is negative or 0.0
                {
                    DonaAmt.Text = "Invalid Entry! Please enter again!";//displays that the number that the user entered is invalid
                }
                else//if the number that the user entered successfully converts to double, then
                {
                    var httpClient = new HttpClient();//new httpclient object
                    var response = await httpClient.GetStringAsync("http://paribusapienv.3ux9m2mjwg.us-west-2.elasticbeanstalk.com/bank/?format=json");//responser to get string data from the api
                    var bankInfo = JsonConvert.DeserializeObject<List<ArchApp.Models.MainBank>>(response);//deserialize

                    int totalBankAmt = 0;//declare and initialize totalbankamt to 0 
                    foreach (Models.MainBank OurBank in bankInfo)//look for the money
                    {
                        totalBankAmt = OurBank.money;//money in the bank
                        System.Diagnostics.Debug.WriteLine("This is the bank amount: " + totalBankAmt);//debug
                        break;//break
                    }

                    DonaAmt.Text = "Donation amount that you entered is: $" + Convert.ToString(someVar);//prints whatever the user inputs
                    string bankName = "The Baby Bobby Bank";//bank name
                    MainBank newBank = new MainBank()//create a new bank object
                    {
                        id = Convert.ToInt32(1),//set id to 1
                        money = Convert.ToInt32(paymentAmount.Text) + totalBankAmt,//set new balance = current balance + user input
                        name = Convert.ToString(bankName)//bank name
                    };

                    var json = JsonConvert.SerializeObject(newBank);//serialize to put
                    var content = new StringContent(json, Encoding.UTF8, "application/json");//application/json format

                    HttpClient client1 = new HttpClient();//new httpclient object
                    var result = await client1.PutAsync("http://paribusapienv.3ux9m2mjwg.us-west-2.elasticbeanstalk.com/bank/", content);//put functionality

                    if (result.IsSuccessStatusCode)//if theres no error putting to database then 
                    {
                        await DisplayAlert("Thank You!", string.Concat("You have donated $", paymentAmount.Text, " for a good cause."), "K, lol");//this alert is shown
                        await Navigation.PushModalAsync(new MainPage());//and when the button is clicked, the user is redirected to the homepage
                    }
                    else//if there is an error
                    {
                        await DisplayAlert("Oops!", "Something went wrong! Please try again.", "Try again");//then inform the user that something went wrong
                    }
                }
            }
        }
    }
}