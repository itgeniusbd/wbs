using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;

namespace WBS.Web.DbScripts
{
    public static class BengaliTextSeeder
    {
        public static async Task SeedBengaliTextAsync(ApplicationDbContext context)
        {
            // Seed Bengali text for Donation Types
            var donationTypes = await context.DonationTypes.ToListAsync();
            
            foreach (var dt in donationTypes)
            {
                switch (dt.Name.ToLower())
                {
                    case "lillah":
                        dt.NameBn = "???????";
                        break;
                    case "zakat":
                        dt.NameBn = "?????";
                        break;
                    case "sadaqah jariyah":
                        dt.NameBn = "??????? ????????";
                        break;
                    case "winter appeal":
                        dt.NameBn = "???????? ?????";
                        break;
                    case "emergency appeal":
                        dt.NameBn = "????? ?????";
                        break;
                    case "food appeal":
                        dt.NameBn = "????? ?????";
                        break;
                    case "water appeal":
                        dt.NameBn = "???? ?????";
                        break;
                    case "orphan sponsorship":
                        dt.NameBn = "???? ??????????";
                        break;
                    case "medical appeal":
                        dt.NameBn = "??????? ?????";
                        break;
                    case "education":
                        dt.NameBn = "??????";
                        break;
                }
            }
            
            await context.SaveChangesAsync();
            
            // You can add more Bengali translations here for other entities
            // Example: Menus, Pages, etc.
        }
        
        public static async Task UpdateExistingSlidersAsync(ApplicationDbContext context)
        {
            var sliders = await context.Sliders.ToListAsync();
            
            foreach (var slider in sliders)
            {
                // Common translations
                if (slider.ButtonText?.ToLower() == "donate now" && string.IsNullOrEmpty(slider.ButtonTextBn))
                {
                    slider.ButtonTextBn = "???? ??? ????";
                }
                
                if (slider.SecondButtonText?.ToLower() == "learn more" && string.IsNullOrEmpty(slider.SecondButtonText))
                {
                    slider.SecondButtonText = "??? ?????";
                }
            }
            
            await context.SaveChangesAsync();
        }
    }
}
