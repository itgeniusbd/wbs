namespace WBS.Web.Models
{
    public class SSLCommerzSettings
    {
        public string StoreId { get; set; } = string.Empty;
        public string StorePassword { get; set; } = string.Empty;
        public bool IsLive { get; set; }
        public string SessionUrl { get; set; } = string.Empty;
        public string ValidationUrl { get; set; } = string.Empty;
        public string Currency { get; set; } = "BDT";
    }
}
