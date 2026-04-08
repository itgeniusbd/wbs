namespace WBS.Web.Models
{
    public class SSLCommerzResponse
    {
        public string status { get; set; } = string.Empty;
        public string failedreason { get; set; } = string.Empty;
        public string sessionkey { get; set; } = string.Empty;
        public object? gw { get; set; }  // Changed from string to object to handle complex gateway info
        public string redirectGatewayURL { get; set; } = string.Empty;
        public string directPaymentURLBank { get; set; } = string.Empty;
        public string directPaymentURLCard { get; set; } = string.Empty;
        public string directPaymentURL { get; set; } = string.Empty;
        public string redirectGatewayURLFailed { get; set; } = string.Empty;
        public string GatewayPageURL { get; set; } = string.Empty;
        public string storeBanner { get; set; } = string.Empty;
        public string storeLogo { get; set; } = string.Empty;
        public string store_amount { get; set; } = string.Empty;
        public object? desc { get; set; }  // Changed from string to object to handle both array and string
        public string is_direct_pay_enable { get; set; } = string.Empty;
    }

    public class SSLCommerzValidationResponse
    {
        public string status { get; set; } = string.Empty;
        public string tran_date { get; set; } = string.Empty;
        public string tran_id { get; set; } = string.Empty;
        public string val_id { get; set; } = string.Empty;
        public decimal amount { get; set; }
        public string store_amount { get; set; } = string.Empty;
        public string currency { get; set; } = string.Empty;
        public string bank_tran_id { get; set; } = string.Empty;
        public string card_type { get; set; } = string.Empty;
        public string card_no { get; set; } = string.Empty;
        public string card_issuer { get; set; } = string.Empty;
        public string card_brand { get; set; } = string.Empty;
        public string card_issuer_country { get; set; } = string.Empty;
        public string card_issuer_country_code { get; set; } = string.Empty;
        public string currency_type { get; set; } = string.Empty;
        public string currency_amount { get; set; } = string.Empty;
        public string currency_rate { get; set; } = string.Empty;
        public string base_fair { get; set; } = string.Empty;
        public string value_a { get; set; } = string.Empty;
        public string value_b { get; set; } = string.Empty;
        public string value_c { get; set; } = string.Empty;
        public string value_d { get; set; } = string.Empty;
        public string risk_level { get; set; } = string.Empty;
        public string risk_title { get; set; } = string.Empty;
        public string error { get; set; } = string.Empty;
    }

    public class SSLCommerzIPNRequest
    {
        public string val_id { get; set; } = string.Empty;
        public string tran_id { get; set; } = string.Empty;
        public decimal amount { get; set; }
        public string card_type { get; set; } = string.Empty;
        public string store_amount { get; set; } = string.Empty;
        public string card_no { get; set; } = string.Empty;
        public string bank_tran_id { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
        public string tran_date { get; set; } = string.Empty;
        public string currency { get; set; } = string.Empty;
        public string card_issuer { get; set; } = string.Empty;
        public string card_brand { get; set; } = string.Empty;
        public string card_sub_brand { get; set; } = string.Empty;
        public string card_issuer_country { get; set; } = string.Empty;
        public string card_issuer_country_code { get; set; } = string.Empty;
        public string store_id { get; set; } = string.Empty;
        public string verify_sign { get; set; } = string.Empty;
        public string verify_key { get; set; } = string.Empty;
        public string verify_sign_sha2 { get; set; } = string.Empty;
        public string currency_type { get; set; } = string.Empty;
        public string currency_amount { get; set; } = string.Empty;
        public string currency_rate { get; set; } = string.Empty;
        public string base_fair { get; set; } = string.Empty;
        public string value_a { get; set; } = string.Empty;
        public string value_b { get; set; } = string.Empty;
        public string value_c { get; set; } = string.Empty;
        public string value_d { get; set; } = string.Empty;
        public string risk_level { get; set; } = string.Empty;
        public string risk_title { get; set; } = string.Empty;
    }
}
