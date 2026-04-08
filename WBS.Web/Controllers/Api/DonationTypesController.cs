using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;

namespace WBS.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DonationTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/DonationTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonationType>>> GetDonationTypes()
        {
            return await _context.DonationTypes
                .Where(d => d.IsActive)
                .OrderBy(d => d.DisplayOrder)
                .ToListAsync();
        }

        // GET: api/DonationTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DonationType>> GetDonationType(int id)
        {
            var donationType = await _context.DonationTypes.FindAsync(id);

            if (donationType == null)
            {
                return NotFound();
            }

            return donationType;
        }

        // POST: api/DonationTypes
        [HttpPost]
        public async Task<ActionResult<DonationType>> PostDonationType(DonationType donationType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.DonationTypes.Add(donationType);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDonationType), new { id = donationType.Id }, donationType);
        }

        // PUT: api/DonationTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDonationType(int id, DonationType donationType)
        {
            if (id != donationType.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(donationType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonationTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool DonationTypeExists(int id)
        {
            return _context.DonationTypes.Any(e => e.Id == id);
        }
    }
}
