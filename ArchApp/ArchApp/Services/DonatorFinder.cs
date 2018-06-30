using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Cryptography;
using ArchApp.Models;

namespace ArchApp.Services
{
    public static class DonatorFinder
    {
        private static string URL = "http://paribusproapi.ts77daczbt.us-west-2.elasticbeanstalk.com/";//unimportant dont worry about this, this was just for rapid url changing for changing between different local hosts and the real api url
        private static int DonatorId = -1;
        /*
         * GetDonatorInfo() takes the username and password given
         * and constructs a donator object and posts it to the api.
         * 
         * The specific method in the django view that it posts it to takes
         * it and checks if there are any donators that match the posted donators and if there are
         * it returns a http status code 202 and if there arent it returns some other status code.
         * 
         * The c# method returns a true or false based on wheter or not the donator was found in the database.
         */
        public static async Task<bool> GetDonatorInfo(string newUsername, string newPassword)
        {
            int placeholderId = 0;//we can give the donator a random id because the API handles assigning IDs
            Donator newDonator = new Donator()
            {
                id = placeholderId,
                username = newUsername,
                password = newPassword,
            };//constructing the donator
            string json = JsonConvert.SerializeObject(newDonator);
            var content = new StringContent(json, Encoding.UTF8, "application/json");//formatting the string as JSON
            var httpClient = new HttpClient();
            var result = await httpClient.PostAsync(String.Concat(URL, "auth/"), content);//change this later
            var contentstr = await result.Content.ReadAsStringAsync();
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(contentstr);
            if (result.StatusCode == System.Net.HttpStatusCode.Created)
            {
                DonatorId = Convert.ToInt32(dict["id"]);
                return true;
            }
            return false;
        }
        /*
         * What the above method does is it creates a get request to the api and deseralizes the json recieved into a List of Donator Objects and returns it
         * 
         * 
         * 
         */
        //Just copy paste this function for hasshing a string
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
        /*
         * This function is for posting an user to the database. Basically it asks for a token with a get request, salts it, and hashes it and sends the token along
         * with the body of the post request to the api which only creates the user if the hashes match within the server.
         */
        public static async Task<bool> MakeUser(string newUsername, string newPassword)
        {
            var httpClient = new HttpClient();
            var token = await httpClient.GetStringAsync(String.Concat(URL, "token/"));//we get the token from the api
            var values = JsonConvert.DeserializeObject<Dictionary<string, int>>(token);

            double convertedint = values["token"] + 0.2423;//values["token"] is the actual token, and we add the salt to it which is 0.2423, it can be anything it can even be a string
            //the sha hash for any int x will be completely different for x+0.2423 because of how sha works
            string tokenvar = convertedint.ToString();//we have to convert it to a string to hash it
            string fullhash = sha256(tokenvar);
            //below we just construct a dictionary and convert it to json and send it to the api, and the donator will only be created if the hashes match.
            Dictionary<string, string> sender = new Dictionary<string, string>
            {
                {"username", newUsername},
                {"password", newPassword},
                { "hash", fullhash}
            };

            string json = JsonConvert.SerializeObject(sender);//converting dictionary to json
            Debug.WriteLine(json);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync(String.Concat(URL, "token2/"), content);


            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)//we do stuff based on the HTTP status codes
            {
                Debug.WriteLine("it Worked");
                return true;
            }
            Debug.WriteLine(result.StatusCode.ToString());
            return false;


        }
        /*
         * What this function above does is it creates a new donator and the password and username for the donator are passed in as arguments,
         * it then seralizes the object into json and posts it to the API and returns true if the method succeded otherwise it returns false.
         */
        //the put request stuff to change info is the same concept as the post request
        public static async Task<bool> PutDonator(string newPassword, string thisusername, string currPass)//thisusername is the username of the account and currpass and newpassword are what their names suggest
        {
            var httpClient = new HttpClient();

            var token = await httpClient.GetStringAsync(String.Concat(URL, "token/"));
            var values = JsonConvert.DeserializeObject<Dictionary<string, int>>(token);

            double convertedint = values["token"] + 0.2423;

            string tokenvar = convertedint.ToString();
            string fullhash = sha256(tokenvar);






            Dictionary<string, string> sender = new Dictionary<string, string>
            {
                {"username", thisusername},
                {"oldpassword", currPass},
                { "newpass", newPassword},
                {"hash",fullhash }
            };
            string json = JsonConvert.SerializeObject(sender);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync(String.Concat(URL, "changepass/"), content);


            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                Debug.WriteLine("it Worked");
                return true;
            }
            else
            {
                return false;
            }
        }
        /*
         * This above function is mainly utilized to change passwords
         * 
         *What the above function does is it sees if a donator exists in the databse with the password that they put in by basically performing a get request. If that user does exist
         * We construct a Donator object with the password field change, serialize it, and perform a PUT request to the API.
         */

    }
}
