using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WBS.Web.Data;
using WBS.Web.Models;

namespace WBS.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonorTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DonorTypesController> _logger;

        public DonorTypesController(ApplicationDbContext context, ILogger<DonorTypesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/donortypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonorTypeCategory>>> GetDonorTypes()
        {
            try
            {
                _logger.LogInformation("Fetching donor types...");
                var donorTypes = await _context.DonorTypeCategories
                    .Where(dt => dt.IsActive && dt.IsVisible)
                    .OrderBy(dt => dt.DisplayOrder)
                    .ToListAsync();
                
                _logger.LogInformation("Found {Count} donor types", donorTypes.Count);
                return Ok(donorTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching donor types");
                return StatusCode(500, new { error = "Error fetching donor types", message = ex.Message });
            }
        }

        // GET: api/donortypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DonorTypeCategory>> GetDonorType(int id)
        {
            try
            {
                var donorType = await _context.DonorTypeCategories.FindAsync(id);

                if (donorType == null)
                {
                    return NotFound();
                }

                return Ok(donorType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching donor type {Id}", id);
                return StatusCode(500, new { error = "Error fetching donor type", message = ex.Message });
            }
        }

        // POST: api/donortypes
        [HttpPost]
        public async Task<ActionResult<DonorTypeCategory>> CreateDonorType(DonorTypeCategory donorType)
        {
            try
            {
                donorType.CreatedAt = DateTime.UtcNow;
                
                // Get the next display order
                var maxOrder = await _context.DonorTypeCategories.MaxAsync(dt => (int?)dt.DisplayOrder) ?? 0;
                donorType.DisplayOrder = maxOrder + 1;

                _context.DonorTypeCategories.Add(donorType);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Created donor type: {Name}", donorType.Name);
                return CreatedAtAction(nameof(GetDonorType), new { id = donorType.Id }, donorType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating donor type");
                return StatusCode(500, new { error = "Error creating donor type", message = ex.Message });
            }
        }

        // PUT: api/donortypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDonorType(int id, DonorTypeCategory donorType)
        {
            if (id != donorType.Id)
            {
                return BadRequest(new { error = "ID mismatch" });
            }

            try
            {
                donorType.UpdatedAt = DateTime.UtcNow;
                _context.Entry(donorType).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                _logger.LogInformation("Updated donor type: {Name}", donorType.Name);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonorTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating donor type {Id}", id);
                return StatusCode(500, new { error = "Error updating donor type", message = ex.Message });
            }
        }

        // DELETE: api/donortypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonorType(int id)
        {
            try
            {
                var donorType = await _context.DonorTypeCategories.FindAsync(id);
                if (donorType == null)
                {
                    return NotFound();
                }

                // Soft delete
                donorType.IsActive = false;
                donorType.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Deleted donor type: {Name}", donorType.Name);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting donor type {Id}", id);
                return StatusCode(500, new { error = "Error deleting donor type", message = ex.Message });
            }
        }

        private bool DonorTypeExists(int id)
        {
            return _context.DonorTypeCategories.Any(e => e.Id == id);
        }
    }
}
