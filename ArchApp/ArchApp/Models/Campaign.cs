using System;
using System.Collections.Generic;
using System.Text;

namespace ArchApp.Models
{
    public class Contact
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
    }

    public class Campaign
    {
        public List<Contact> contacts { get; set; }
        public int price_in_cents { get; set; }
        public List<string> brand_codes { get; set; }
        public string expiry { get; set; }
        public string id { get; set; }
        public string delivery_type { get; set; }
        public string message { get; set; }
        public string subject { get; set; }
    }
}
