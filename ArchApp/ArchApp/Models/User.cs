using System;
using System.Collections.Generic;
using System.Text;

namespace ArchApp.Models
{
    public class User
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userEmail { get; set; }
        public string password { get; set; }
        // all of the valid inputs for the new improved user model here
        // your standard constructor
        public User()
        {
            firstName = null;
            lastName = null;
            userEmail = null;
            password = null;
        }
    }
}
