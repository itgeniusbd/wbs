namespace WBS.Web.ViewModels
{
    public class WhereWeWorkViewModel
    {
        public int TotalDistricts { get; set; } = 64;
        public int CoveredDistricts { get; set; }
        public int TotalUpazilas { get; set; } = 495;
        public int CoveredUpazilas { get; set; }
        public List<DistrictWorkInfo> DistrictWorkInfos { get; set; } = new();
    }

    public class DistrictWorkInfo
    {
        public int DistrictId { get; set; }
        public string DistrictName { get; set; } = string.Empty;
        public string DistrictNameBn { get; set; } = string.Empty;
        public bool HasWork { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int TotalUpazilas { get; set; }
        public int CoveredUpazilas { get; set; }
        public List<string> UpazilaNames { get; set; } = new();
        public List<string> UpazilaNamesEn { get; set; } = new();
    }
}
