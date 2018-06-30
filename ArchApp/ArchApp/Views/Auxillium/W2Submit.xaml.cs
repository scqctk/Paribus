using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using ArchApp.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Security.Cryptography;
using System.Diagnostics;
//line 466 is where all conditions are met and finally a user can then be created
namespace ArchApp.Views.Auxillium
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class W2Submit : ContentPage
    {
        public W2Submit()
        {
            InitializeComponent();
        }
        string myFirstName = null;
        string myLastName = null;

        public static string URL = "http://paribusproapi.ts77daczbt.us-west-2.elasticbeanstalk.com/";
        //public static string URL = "http://localhost:60346/";


        private static bool isValidEmail(string myEmail)
        {
            // Matthew Strong isValid email algorithm
            // inputs: string email
            // outputs: bool that returns a if the given string is a valid email
            // Big O: O(n)
            if (myEmail[0] == '.')
            {
                return false;
            }
            int countAt = 0;
            int countPeriod = 0;
            int thisPerIndex = 0;
            bool encodedTagExists = false;
            foreach (var character in myEmail)
            {
                // making sure that there are not too many characters to have here for periods and @signs
                if (character == '@')
                {
                    countAt = countAt + 1;

                }
                if (character == '.')
                {
                    thisPerIndex = myEmail.IndexOf(character);
                    if (thisPerIndex == myEmail.Count() - 1)
                    {
                        // in the case that the period is the last character here
                        return false;
                    }
                    if (myEmail[thisPerIndex] == myEmail[thisPerIndex + 1])
                    {
                        return false;
                        // there are two consecutive periods here and that is not valid
                    }
                }
                if (character == ' ')
                {
                    return false;
                }
                if (character == '<')
                {
                    encodedTagExists = true;
                }
                if (character == '>')
                {
                    if (encodedTagExists)
                    {

                        return false;
                    }
                }
            }
            if (countAt > 1 || countAt == 0)
            {
                return false;
            }
            // we can only have one at sign and we also cannot have 0
            //myEmail.C
            bool first = myEmail.Contains("@");
            bool second = myEmail.Contains(".com");
            bool third = myEmail.Contains(".net");
            bool fourth = myEmail.Contains(".edu");
            bool fifth = myEmail.Contains(".gov");
            if ((first && (second || third || fourth || fifth)))
            {
                string[] name = myEmail.Split('@');
                int value = name[0].Count() - 1;
                if (value < 0)
                {
                    return false;
                }
                char isPeriod = name[0][value];
                // is there a period right before the @sign
                if (name[0].Contains(".com") || name[0].Contains(".net") || name[0].Contains(".edu") || name[0].Contains(".gov"))
                {
                    return false;
                }
                // gets the last value in the index to make sure it isnt a period
                if (isPeriod == '.')
                {
                    return false;
                }
                if (name[1][0] == '-')
                {
                    return false;
                }
                // split the name up here
                bool hasValidName = name[0].ToCharArray().Any(char.IsLetterOrDigit);
                // we won't have extra .'s here after splitting by the @ sign
                // matt.s.s@gma.il.com
                // gma.il.com -- too many periods
                // exception to the rule here
                List<string> containsOneDomain = new List<string>();
                containsOneDomain.Add(".com");
                containsOneDomain.Add(".net");
                containsOneDomain.Add(".edu");
                containsOneDomain.Add(".gov");
                bool containsMoreOne = false;
                // accounting for cases of more than .com and .gov
                foreach (var domain in containsOneDomain)
                {
                    if (name[1].Contains(domain))
                    {
                        // if the end contains the domain!
                        if (!name[1].EndsWith(domain))
                        {
                            return false;

                        }
                        if (containsMoreOne)
                        {
                            // more than one domain!
                            return false;
                        }
                        else
                        {
                            containsMoreOne = true;
                        }

                    }

                }
                // if the stuff before the @ sign has any letters or digits
                // but also need code to account from something like f.s.g.com, that will not split it.
                int lastPeriod = name[1].LastIndexOf('.');
                char placementPeriod = name[1][lastPeriod];
                string[] webTitle = name[1].Split(placementPeriod);
                // example is gmail.com
                if (name[0].Count() == 0)
                {
                    hasValidName = false;
                }

                // name[1] is everything after the @ sign
                bool hasValidTitle = webTitle[0].ToCharArray().Any(char.IsLetterOrDigit);
                if (webTitle[0].Count() == 0)
                {
                    hasValidTitle = false;
                }

                // if the stuff between the @ and . (ex: gmail) has any letters or digits...

                // is it REALLY valid?!?!?!
                bool trulyEmail = ((hasValidName && hasValidTitle));
                if (trulyEmail)
                {
                    return true;
                }
                else
                {
                    return false;

                }
            }
            else
            {
                return false;
            }

        }







        public double calcSimilarity(string baseWord, string compareWord)
        {
            // currently under construction
            //baseWord.
            //baseWord.
            double length = baseWord.Count();
            double score = 0;
            //System.Diagnostics.Debug.WriteLine(baseWord);
            if (baseWord.Count() <= compareWord.Count())
            {
                foreach (var character in baseWord)
                {
                    int theValue = baseWord.IndexOf(character);
                    //character
                    if ((character) == (compareWord[theValue]))
                    {
                        score = score + 1;
                    }
                }
            }
            else
            {
                // base word is bigger
                foreach (var chart in compareWord)
                {
                    int myValue = compareWord.IndexOf(chart);
                    if ((chart == baseWord[myValue]))
                    {
                        score = score + 1;
                    }
                }
            }
            int differential = Math.Abs(compareWord.Count() - baseWord.Count());
            //double actual = 0.5 * differential;
            //score = score - actual + 0.25;
            double result = score / length;
            //System.Diagnostics.Debug.WriteLine(result);
            return result;
        }

        private async void submitForm_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            // creates new instance of photo taking device
            MediaFile picture;
            // if I can take a photo here
            // takes a photo from my computer
            // if everything fails - still under construction
            picture = await CrossMedia.Current.PickPhotoAsync();
            OcrResults text;

            // new OcrResults object
            var client = new VisionServiceClient("88c4bf5790fd45ed907013b510ea7fca", "https://eastus.api.cognitive.microsoft.com/vision/v1.0");

            using (var photoStream = picture.GetStream())
            {
                // text is ocrresults object
                text = await client.RecognizeTextAsync(photoStream);
            }

            System.Diagnostics.Debug.WriteLine("boy");
            string cumulative = "";
            List<string> forms = new List<string>();
            List<string> wLines = new List<string>();
            foreach (var region in text.Regions)
            {
                //text.Regions.OrderBy<>
                System.Diagnostics.Debug.WriteLine("New Region!");
                //System.Diagnostics.Debug.WriteLine(region.BoundingBox);
                // go through each region
                foreach (var line in region.Lines)
                {
                    string added = "";
                    // go through each line
                    foreach (var word in line.Words)
                    {
                        // print out word.Text, the seen word, for what was read in via ocr
                        string boy = word.Text;
                        added = added + " " + boy;
                        cumulative = cumulative + " " + boy;
                        //System.Diagnostics.Debug.WriteLine(boy);
                        forms.Add(boy);
                    }
                    System.Diagnostics.Debug.WriteLine(added);
                    wLines.Add(added);
                    // this is each line 
                    // writes out the line so far 
                    // we can find the full name of the person from here...
                }
            }
            System.Diagnostics.Debug.WriteLine(cumulative);
            // use similarity score!!!



            List<string> wageIndicator = new List<string>();

            List<int> keyIndex = new List<int>();
            List<int> floatIndex = new List<int>();


            string period = ".";
            wageIndicator.Add(period + "00");
            wageIndicator.Add(period + "01");
            wageIndicator.Add(period + "02");
            wageIndicator.Add(period + "03");
            wageIndicator.Add(period + "04");
            wageIndicator.Add(period + "05");
            wageIndicator.Add(period + "06");
            wageIndicator.Add(period + "07");
            wageIndicator.Add(period + "08");
            wageIndicator.Add(period + "09");
            for (int i = 10; i < 100; i++)
            {
                wageIndicator.Add(period + i.ToString());
            }
            int index = 0;
            List<string> keyWords = new List<string>();
            keyWords.Add("tips");
            keyWords.Add("compensation");
            keyWords.Add("other");
            // have code that groups the other items together
            List<string> possibleWages = new List<string>();
            foreach (var stringer in forms)
            {

                //stringer.Replace('$', '');

                //System.Diagnostics.Debug.WriteLine(stringer);
                foreach (var important in keyWords)
                {
                    if (calcSimilarity(important, stringer) > 0.8)
                    {
                        System.Diagnostics.Debug.WriteLine("Added");
                        keyIndex.Add(index);
                    }
                }
                foreach (var dec in wageIndicator)
                {
                    // look for every possible monetary value here
                    if (stringer.Contains(dec))
                    {
                        // indicates the monetary amount that appears on the form
                        possibleWages.Add(stringer);
                        floatIndex.Add(index);
                        break;

                    }
                }
                index = index + 1;
            }
            //   Department of the Treasury — Intemal Revenue Service
            bool govReqMet = false;
            List<string> treasuryService = new List<string>();
            treasuryService.Add("Wage");
            treasuryService.Add("Tax");
            treasuryService.Add("Statement");
            treasuryService.Add("Department");
            treasuryService.Add("Treasury");
            treasuryService.Add("Revenue");
            treasuryService.Add("Service");

            // probably 5 out of 7 options here should work
            System.Diagnostics.Debug.WriteLine("Test");
            int countSatisfied = 0;
            int options = 0;
            // boolean that tests whether the w2 form is valid...
            bool isValid = false;
            foreach (var word in forms)
            {
                if (calcSimilarity(treasuryService[countSatisfied], word) >= 0.8)
                {
                    // if close enough match to the treasuryService with the word
                    // match in that index
                    countSatisfied = countSatisfied + 1;
                    options = options + 1;
                    System.Diagnostics.Debug.WriteLine("Test");
                    if (countSatisfied == 7)
                    {
                        // all counts satisfied
                        isValid = true;
                        break;
                    }
                }


                // in order to find the employees name look for the words related to it..
            }
            if (options >= 5)
            {
                isValid = true;
            }
            System.Diagnostics.Debug.WriteLine(isValid);
            if (isValid)
            {
                int indexer = 0;
                string nameLine = "";
                List<string> numbers = new List<string>();
                for (int i = 0; i < 10; i++)
                {
                    numbers.Add(i.ToString());
                }
                System.Diagnostics.Debug.WriteLine(wLines.Count);
                foreach (var line in wLines)
                {
                    if ((line.Contains("Employee's") && line.Contains("name") && line.Contains("and")) || (line.Contains("initial")) || (line.Contains("first")))
                    {

                        nameLine = wLines[indexer + 1];
                        bool broken = false;
                        foreach (var num in numbers)
                        {
                            if (nameLine.Contains(num))
                            {
                                broken = true;

                            }
                        }
                        if (broken == false)
                        {
                            // a likely match was found for the name
                            break;
                        }
                        else
                        {
                            nameLine = "";
                            // still didn't find a match for the user's name
                        }
                    }
                    else if (line.Contains("Last") && line.Contains("name"))
                    {
                        nameLine = wLines[indexer + 1];
                        bool broken = false;
                        foreach (var num in numbers)
                        {
                            if (nameLine.Contains(num))
                            {
                                broken = true;
                            }
                        }
                        if (broken == false)
                        {
                            // a likely match was found for the name
                            break;
                        }
                        else
                        {
                            nameLine = "";
                            // still didn't find a match for the user's name
                        }
                    }

                    indexer = indexer + 1;
                    System.Diagnostics.Debug.WriteLine(indexer);

                    // going through each line that was found in the w2 form
                }


                List<int> differences = new List<int>();
                foreach (var testing in floatIndex)
                {
                    int sumIndex = 0;
                    // go through the indices of monetary amounts, then see how close it is to the 
                    foreach (var item in keyIndex)
                    {
                        sumIndex = sumIndex + Math.Abs(testing - item);
                    }
                    // the closer item is to these key words the better
                    differences.Add(sumIndex);
                }
                System.Diagnostics.Debug.WriteLine("best");
                if (differences.Count == 0)
                {
                    // no matches
                    await DisplayAlert("Error!", "Cannot find information.", "Close");
                    return;
                }
                System.Diagnostics.Debug.WriteLine(differences.Count);

                int minValueIndex = differences.IndexOf(differences.Min());
                System.Diagnostics.Debug.WriteLine(minValueIndex);
                string monetaryValue = possibleWages[minValueIndex];
                System.Diagnostics.Debug.WriteLine(monetaryValue + " is the best match for yearly wages!");


                // the index at which the smallest value is located
                // get rid of any possible dollar signs and commas
                if (monetaryValue.Contains("$"))
                {
                    monetaryValue = monetaryValue.Replace("$", "");

                }
                if (monetaryValue.Contains(","))
                {
                    monetaryValue = monetaryValue.Replace(",", "");
                }
                double calcValue = 0;

                bool successConvert = double.TryParse(monetaryValue, out calcValue);
                if (successConvert)
                {
                    if (calcValue <= 50000)
                    {
                        if (nameLine == "")
                        {
                            await DisplayAlert("Congratulations!", "You qualify for benefits, your wage was $" + monetaryValue + " and we could not find your name.", "Close");
                            submitLabel.Text = "Enter in account information.";
                            submitForm.IsVisible = false;

                            inputEmail.IsVisible = true;
                            email.IsVisible = true;
                            inputPassword.IsVisible = true;
                            password.IsVisible = true;
                            reEnter.IsVisible = true;
                            againPassword.IsVisible = true;
                            createAccount.IsVisible = true;
                            inputFirstName.IsVisible = true;
                            firstName.IsVisible = true;
                            inputLastName.IsVisible = true;
                            lastName.IsVisible = true;
                            // making all of the other options visible
                            return;

                            // need first name and last name
                        }
                        else
                        {
                            await DisplayAlert("Congratulations!", "You qualify for benefits, your wage was $" + monetaryValue + " and is your name " + nameLine + "?", "Close");

                            submitLabel.Text = "Is your name " + nameLine + "?";
                            submitForm.IsVisible = false;
                            decisionButton.IsVisible = true;
                            string[] splitter = nameLine.Split(' ');
                            myFirstName = nameLine.Split(' ')[0];
                            myLastName = nameLine.Split(' ')[1];
                            foreach (var item in splitter)
                            {
                                System.Diagnostics.Debug.WriteLine(item);
                            }

                            if (!Regex.IsMatch(myFirstName, @"^[a-zA-Z]+$"))
                            {
                                myFirstName = nameLine.Split(' ')[1];
                                myLastName = nameLine.Split(' ')[2];
                            }
                            else if (!Regex.IsMatch(myLastName, @"^[a-zA-Z]+$"))
                            {
                                myLastName = nameLine.Split(' ')[2];
                            }


                            noButton.IsVisible = true;
                            // set a name to the possible name
                            return;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Sorry!", "You do not qualify for assistance, you made " + monetaryValue + " in a year.", "Close");
                    }

                    foreach (var value in possibleWages)
                    {
                        System.Diagnostics.Debug.WriteLine(value);

                    }
                    //await DisplayAlert("Answer", "Your answer was " + cumulative, "Exit");
                    //await DisplayAlert("Answer", monetaryValue + " is the best match for yearly wages!" + cumulative, "Exit");

                }
                else
                {
                    await DisplayAlert("Error!", "Failed to extract a monetary value from the W-2. Make sure that you have the format $dollars.cents.", "OK");
                    return;
                    // failure to convert the double
                }
                // what happens if this messes up 


            }
            else
            {
                // couldn't read the w2 form properly, might have been too blurry or there were other issues...
                await DisplayAlert("Error", "Failed to extract information from the W2!", "Close");//^^
            }
        }
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
        private async void createAccount_Clicked(object sender, EventArgs e)
        {
            // this button was only visible when the user was approved to actually construct an account
            // email.Text
            // password.Text
            // againPassword.Text
            if (myFirstName == null && myLastName == null)
            {
                // user wanted to initialize them because the W2 form could not find them or it simply was not correct at all with the name finding 
                if (lastName.Text == "" || firstName.Text == "")
                {
                    await DisplayAlert("Invalid", "One or more names was not entered in!", "Try Again");
                    return;
                }
                else
                {
                    myLastName = lastName.Text;
                    myFirstName = firstName.Text;

                }

            }

            if (password.Text != againPassword.Text)
            {
                await DisplayAlert("Invalid", "The passwords do not match!", "Try Again");
                return;

            }
            if (password.Text == "" || againPassword.Text == "" || email.Text == "")
            {
                await DisplayAlert("Invalid", "Something was not entered in!", "Try Again");
                return;

            }

            ArchApp.Views.Auxillium.ReceiverSignIn.currentUser.firstName = myFirstName;
            ArchApp.Views.Auxillium.ReceiverSignIn.currentUser.lastName = myLastName;


            string personEmail = email.Text;
            bool validEmail = isValidEmail(personEmail);
            System.Diagnostics.Debug.WriteLine(validEmail);
            // anything past this point will have passwords that are not empty and match, and the email is valid and has enough to pass as an email
            //System.
            if (validEmail)
            {
                // valid eamail, everything is good
                ArchApp.Views.Auxillium.ReceiverSignIn.currentUser.userEmail = personEmail;
                string validPassword = password.Text;
                ArchApp.Views.Auxillium.ReceiverSignIn.currentUser.password = validPassword;
                User newEntry = new User()
                {
                    userEmail = personEmail,
                    password = validPassword,
                    firstName = myFirstName,
                    lastName = myLastName
                };
                // we shouldn't need to post the id here, it will be automatically generated when I post to the django rest api

                // create the user here because everything has been approved

                // this is a valid email that is entered here so the user can create an account
                // after posting this securely to the api database then we need to make sure to go to the Receiver Account page here..

                var httpClient = new HttpClient();


                var token = await httpClient.GetStringAsync(String.Concat(URL, "tokenfoo/"));//we get the token from the api


                var values = JsonConvert.DeserializeObject<Dictionary<string, int>>(token);

                double convertedint = values["token"] + 0.2423;//values["token"] is the actual token, and we add the salt to it which is 0.2423, it can be anything it can even be a string
                                                               //the sha hash for any int x will be completely different for x+0.2423 because of how sha works
                string tokenvar = convertedint.ToString();//we have to convert it to a string to hash it
                string fullhash = sha256(tokenvar);

                await DisplayAlert("Success", "Your account has now been created!", "OK");
                Dictionary<string, string> senderfoo = new Dictionary<string, string>
                {
                    {"username", personEmail},
                    {"password", validPassword},
                    {"firstName", myFirstName },
                    {"lastName", myFirstName },
                    { "hash", fullhash}
                };


                string json = JsonConvert.SerializeObject(senderfoo);//converting dictionary to json
                Debug.WriteLine(json);
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                var result = await httpClient.PostAsync(String.Concat(URL, "token2foo/"), content);


                if (result.StatusCode == System.Net.HttpStatusCode.Accepted)//we do stuff based on the HTTP status codes
                {
                    Debug.WriteLine("it Worked");
                    await Navigation.PushAsync(new ReceiverAccount());
                    return;
                }
                Debug.WriteLine(result.StatusCode.ToString());
                await DisplayAlert("Invalid", "The email entered is not a valid email.", "Try Again");

                return;

            }
            else
            {
                await DisplayAlert("Invalid", "The email entered is not a valid email.", "Try Again");
                return;
            }
            // everything after this is valid inputs for the user
            //string ema
        }

        private void decisionButton_Clicked(object sender, EventArgs e)
        {
            // yes was hit so no name for here then

            decisionButton.IsVisible = false;
            noButton.IsVisible = false;

            submitLabel.Text = "Enter in account information.";
            submitForm.IsVisible = false;

            inputEmail.IsVisible = true;
            email.IsVisible = true;
            inputPassword.IsVisible = true;
            password.IsVisible = true;
            reEnter.IsVisible = true;
            againPassword.IsVisible = true;
            createAccount.IsVisible = true;

        }

        private void noButton_Clicked(object sender, EventArgs e)
        {
            myLastName = null;
            myFirstName = null;
            // these fields were already initialized, but are now rendered null because they were not accurate - user will enter them in now
            // no, need field for user's name
            decisionButton.IsVisible = false;
            noButton.IsVisible = false;

            submitLabel.Text = "Enter in account information.";
            submitForm.IsVisible = false;

            inputEmail.IsVisible = true;
            email.IsVisible = true;
            inputPassword.IsVisible = true;
            password.IsVisible = true;
            reEnter.IsVisible = true;
            againPassword.IsVisible = true;
            createAccount.IsVisible = true;
            inputFirstName.IsVisible = true;
            firstName.IsVisible = true;
            inputLastName.IsVisible = true;
            lastName.IsVisible = true;

        }
    }

}