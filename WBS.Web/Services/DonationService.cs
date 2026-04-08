using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;
using WBS.Web.ViewModels;

namespace WBS.Web.Services
{
    public interface IDonationService
    {
        Task<List<DonationTypeViewModel>> GetActiveDonationTypesAsync();
        Task<Donation> CreateDonationAsync(DonationViewModel model);
        Task<Donation?> GetDonationByIdAsync(int id);
        Task<List<Donation>> GetDonationsAsync(int page = 1, int pageSize = 20);
        Task UpdateDonationStatusAsync(int id, DonationStatus status, string? transactionId = null);
        Task<decimal> GetTotalDonationsAsync();
        Task<int> GetDonationsCountAsync();
    }

    public class DonationService : IDonationService
    {
        private readonly ApplicationDbContext _context;

        public DonationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DonationTypeViewModel>> GetActiveDonationTypesAsync()
        {
            return await _context.DonationTypes
                .Where(d => d.IsActive)
                .OrderBy(d => d.DisplayOrder)
                .Select(d => new DonationTypeViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    NameBn = d.NameBn,
                    Description = d.Description,
                    DescriptionBn = d.DescriptionBn,
                    Icon = d.Icon
                })
                .ToListAsync();
        }

        public async Task<Donation> CreateDonationAsync(DonationViewModel model)
        {
            var donation = new Donation
            {
                DonorName = model.DonorName,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                Amount = model.Amount,
                Currency = model.Currency,
                DonationTypeId = model.DonationTypeId,
                AppealId = model.AppealId,
                PaymentMethod = model.PaymentMethod,
                IsRecurring = model.IsRecurring,
                RecurringFrequency = model.RecurringFrequency,
                IsAnonymous = model.IsAnonymous,
                Notes = model.Notes,
                Status = DonationStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Donations.Add(donation);
            await _context.SaveChangesAsync();
            return donation;
        }

        public async Task<Donation?> GetDonationByIdAsync(int id)
        {
            return await _context.Donations
                .Include(d => d.DonationType)
                .Include(d => d.Appeal)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<List<Donation>> GetDonationsAsync(int page = 1, int pageSize = 20)
        {
            return await _context.Donations
                .Include(d => d.DonationType)
                .Include(d => d.Appeal)
                .OrderByDescending(d => d.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task UpdateDonationStatusAsync(int id, DonationStatus status, string? transactionId = null)
        {
            var donation = await _context.Donations.FindAsync(id)
                ?? throw new KeyNotFoundException("Donation not found");

            donation.Status = status;
            if (!string.IsNullOrEmpty(transactionId))
                donation.TransactionId = transactionId;

            if (status == DonationStatus.Completed)
            {
                donation.PaidAt = DateTime.UtcNow;

                // Update appeal raised amount
                if (donation.AppealId.HasValue)
                {
                    var appeal = await _context.Appeals.FindAsync(donation.AppealId.Value);
                    if (appeal != null)
                    {
                        appeal.RaisedAmount += donation.Amount;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetTotalDonationsAsync()
        {
            var total = await _context.Donations
                .Where(d => d.Status == DonationStatus.Completed)
                .SumAsync(d => (decimal?)d.Amount);
            
            return total ?? 0;
        }

        public async Task<int> GetDonationsCountAsync()
        {
            return await _context.Donations.CountAsync();
        }
    }
}
