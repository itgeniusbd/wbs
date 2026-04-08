using WBS.Web.Data;
using WBS.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace WBS.Web.Services
{
    /// <summary>
    /// Sample data seeder for testing the Where We Work feature
    /// This can be called from a controller action or used during development
    /// </summary>
    public class WhereWeWorkSeeder
    {
        private readonly ApplicationDbContext _context;

        public WhereWeWorkSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Seeds sample working districts and upazilas
        /// Example: Mark Dhaka, Sylhet, and Chattogram as working districts with some upazilas
        /// </summary>
        public async Task SeedSampleWorkingAreasAsync()
        {
            // Check if districts exist
            var dhakaDistrict = await _context.Districts.FirstOrDefaultAsync(d => d.Name == "Dhaka");
            var sylhetDistrict = await _context.Districts.FirstOrDefaultAsync(d => d.Name == "Sylhet");
            var chattogramDistrict = await _context.Districts.FirstOrDefaultAsync(d => d.Name == "Chattogram");

            if (dhakaDistrict != null)
            {
                dhakaDistrict.HasWork = true;
                
                // Add sample upazilas for Dhaka
                var dhakaUpazilas = new List<Upazila>
                {
                    new Upazila { DistrictId = dhakaDistrict.Id, Name = "Gulshan", NameBn = "গুলশান", HasWork = true, DisplayOrder = 1 },
                    new Upazila { DistrictId = dhakaDistrict.Id, Name = "Mirpur", NameBn = "মিরপুর", HasWork = true, DisplayOrder = 2 },
                    new Upazila { DistrictId = dhakaDistrict.Id, Name = "Mohammadpur", NameBn = "মোহাম্মদপুর", HasWork = true, DisplayOrder = 3 },
                    new Upazila { DistrictId = dhakaDistrict.Id, Name = "Dhanmondi", NameBn = "ধানমন্ডি", HasWork = false, DisplayOrder = 4 },
                    new Upazila { DistrictId = dhakaDistrict.Id, Name = "Uttara", NameBn = "উত্তরা", HasWork = false, DisplayOrder = 5 }
                };

                foreach (var upazila in dhakaUpazilas)
                {
                    if (!await _context.Upazilas.AnyAsync(u => u.Name == upazila.Name && u.DistrictId == upazila.DistrictId))
                    {
                        _context.Upazilas.Add(upazila);
                    }
                }
            }

            if (sylhetDistrict != null)
            {
                sylhetDistrict.HasWork = true;
                
                // Add sample upazilas for Sylhet
                var sylhetUpazilas = new List<Upazila>
                {
                    new Upazila { DistrictId = sylhetDistrict.Id, Name = "Sylhet Sadar", NameBn = "সিলেট সদর", HasWork = true, DisplayOrder = 1 },
                    new Upazila { DistrictId = sylhetDistrict.Id, Name = "Beanibazar", NameBn = "বিয়ানীবাজার", HasWork = true, DisplayOrder = 2 },
                    new Upazila { DistrictId = sylhetDistrict.Id, Name = "Golapganj", NameBn = "গোলাপগঞ্জ", HasWork = true, DisplayOrder = 3 }
                };

                foreach (var upazila in sylhetUpazilas)
                {
                    if (!await _context.Upazilas.AnyAsync(u => u.Name == upazila.Name && u.DistrictId == upazila.DistrictId))
                    {
                        _context.Upazilas.Add(upazila);
                    }
                }
            }

            if (chattogramDistrict != null)
            {
                chattogramDistrict.HasWork = true;
                
                // Add sample upazilas for Chattogram
                var chattogramUpazilas = new List<Upazila>
                {
                    new Upazila { DistrictId = chattogramDistrict.Id, Name = "Chattogram Sadar", NameBn = "চট্টগ্রাম সদর", HasWork = true, DisplayOrder = 1 },
                    new Upazila { DistrictId = chattogramDistrict.Id, Name = "Hathazari", NameBn = "হাটহাজারী", HasWork = true, DisplayOrder = 2 },
                    new Upazila { DistrictId = chattogramDistrict.Id, Name = "Raozan", NameBn = "রাউজান", HasWork = false, DisplayOrder = 3 }
                };

                foreach (var upazila in chattogramUpazilas)
                {
                    if (!await _context.Upazilas.AnyAsync(u => u.Name == upazila.Name && u.DistrictId == upazila.DistrictId))
                    {
                        _context.Upazilas.Add(upazila);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
