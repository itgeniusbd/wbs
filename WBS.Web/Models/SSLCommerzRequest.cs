namespace WBS.Web.Models
{
    public class SSLCommerzRequest
    {
        public string store_id { get; set; } = string.Empty;
        public string store_passwd { get; set; } = string.Empty;
        public decimal total_amount { get; set; }
        public string currency { get; set; } = "BDT";
        public string tran_id { get; set; } = string.Empty;
        public string success_url { get; set; } = string.Empty;
        public string fail_url { get; set; } = string.Empty;
        public string cancel_url { get; set; } = string.Empty;
        public string ipn_url { get; set; } = string.Empty;
        
        // Customer Information
        public string cus_name { get; set; } = string.Empty;
        public string cus_email { get; set; } = string.Empty;
        public string cus_add1 { get; set; } = string.Empty;
        public string cus_add2 { get; set; } = string.Empty;
        public string cus_city { get; set; } = string.Empty;
        public string cus_state { get; set; } = string.Empty;
        public string cus_postcode { get; set; } = string.Empty;
        public string cus_country { get; set; } = "Bangladesh";
        public string cus_phone { get; set; } = string.Empty;
        public string cus_fax { get; set; } = string.Empty;
        
        // Shipment Information
        public string shipping_method { get; set; } = "NO";  // Added missing field
        public string ship_name { get; set; } = string.Empty;
        public string ship_add1 { get; set; } = string.Empty;
        public string ship_add2 { get; set; } = string.Empty;
        public string ship_city { get; set; } = string.Empty;
        public string ship_state { get; set; } = string.Empty;
        public string ship_postcode { get; set; } = string.Empty;
        public string ship_country { get; set; } = "Bangladesh";
        
        // Product Information
        public string product_name { get; set; } = string.Empty;
        public string product_category { get; set; } = "Donation";
        public string product_profile { get; set; } = "general";
        
        // Optional
        public string value_a { get; set; } = string.Empty; // Can be used for donation ID
        public string value_b { get; set; } = string.Empty;
        public string value_c { get; set; } = string.Empty;
        public string value_d { get; set; } = string.Empty;
    }
}
