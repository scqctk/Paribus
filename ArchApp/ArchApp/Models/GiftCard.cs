using System;
using System.Collections.Generic;
using System.Text;

namespace ArchApp.Models
{
    public class Gift
    {
        public string uuid { get; set; }
        public string campaign_uuid { get; set; }
        public string delivery_status { get; set; }
        public string status { get; set; }
        public string management_dashboard_link { get; set; }
        public int redelivery_count { get; set; }
        public string recipient_email { get; set; }
        public string campaign_id { get; set; }
        public string recipient_name { get; set; }
        public int price_in_cents { get; set; }
        public string brand_code { get; set; }
        public string marketplacegift_id { get; set; }
        public string created_date { get; set; }
        public string delivery_date { get; set; }
    }

    public class Info
    {
        public string code { get; set; }
        public string name { get; set; }
        public string message { get; set; }
    }

    public class GiftCard
    {
        public List<Gift> gifts { get; set; }
        public int number_of_results { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public int total_count { get; set; }
        public Info info { get; set; }
        public int status { get; set; }
    }
}
