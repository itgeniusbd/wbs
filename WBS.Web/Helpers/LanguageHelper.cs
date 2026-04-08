using Microsoft.AspNetCore.Localization;

namespace WBS.Web.Helpers
{
    public static class LanguageHelper
    {
        public static bool IsEnglish(HttpContext context)
        {
            var feature = context.Features.Get<IRequestCultureFeature>();
            var culture = feature?.RequestCulture.Culture.TwoLetterISOLanguageName ?? "en";
            return culture == "en";
        }

        public static string GetLocalizedText(string textEn, string? textBn, string currentCulture)
        {
            if (currentCulture == "bn" && !string.IsNullOrEmpty(textBn))
                return textBn;
            return textEn;
        }

        public static Dictionary<string, string> CommonTexts = new()
        {
            // Navigation
            { "Home", "হোম" },
            { "About", "সম্পর্কে" },
            { "Our Work", "আমাদের কাজ" },
            { "Activities", "কার্যক্রম" },
            { "Get Involved", "যুক্ত হন" },
            { "Contact", "যোগাযোগ" },
            { "Donate Now", "দান করুন" },
            { "Login", "লগইন" },
            { "Logout", "লগআউট" },
            { "Admin Panel", "অ্যাডমিন প্যানেল" },
            
            // Buttons
            { "Read More", "আরও পড়ুন" },
            { "Learn More", "আরও জানুন" },
            { "View All", "সব দেখুন" },
            { "Submit", "জমা দিন" },
            { "Cancel", "বাতিল" },
            { "Save", "সংরক্ষণ" },
            { "Edit", "সম্পাদনা" },
            { "Delete", "মুছুন" },
            
            // Common Words
            { "Welcome", "স্বাগতম" },
            { "Search", "অনুসন্ধান" },
            { "Loading...", "লোড হচ্ছে..." },
            { "Please wait", "অনুগ্রহ করে অপেক্ষা করুন" },
            { "Thank you", "ধন্যবাদ" },
            { "Success", "সফল" },
            { "Error", "ত্রুটি" },
            
            // Donation
            { "One-time", "একবার" },
            { "Monthly", "মাসিক" },
            { "Amount", "পরিমাণ" },
            { "Donate", "দান করুন" },
            { "Lillah", "লিল্লাহ" },
            { "Zakat", "যাকাত" },
            { "Sadaqah", "সাদাকাহ" },
            
            // Appeals
            { "Current Appeals", "বর্তমান আবেদন" },
            { "Urgent", "জরুরি" },
            { "Featured", "বৈশিষ্ট্যযুক্ত" },
            { "Raised", "সংগৃহীত" },
            { "Goal", "লক্ষ্য" },
            { "Target", "লক্ষ্য" },
            
            // Footer
            { "Quick Links", "দ্রুত লিংক" },
            { "Contact Info", "যোগাযোগের তথ্য" },
            { "Newsletter", "নিউজলেটার" },
            { "Privacy Policy", "গোপনীয়তা নীতি" },
            { "Terms & Conditions", "শর্তাবলী" },
            { "All rights reserved", "সর্বস্বত্ব সংরক্ষিত" }
        };

        public static string Translate(string key, string currentCulture)
        {
            if (currentCulture == "bn" && CommonTexts.ContainsKey(key))
                return CommonTexts[key];
            return key;
        }
    }
}
