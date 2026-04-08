using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;
using WBS.Web.Attributes;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ContactListsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ContactListsController> _logger;

        public ContactListsController(
            ApplicationDbContext context,
            ILogger<ContactListsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Admin/ContactLists
        [Permission("Contact Lists", "View")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var groups = await _context.ContactGroups
                    .Include(g => g.Contacts)
                    .Where(g => g.IsActive)
                    .OrderBy(g => g.GroupName)
                    .ToListAsync();

                return View(groups);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading contact lists");
                TempData["Error"] = "Error loading contact lists.";
                return View(new List<ContactGroup>());
            }
        }

        // POST: Admin/ContactLists/CreateGroup
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Contact Lists", "Create")]
        public async Task<IActionResult> CreateGroup(string groupName, string? description)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(groupName))
                {
                    return Json(new { success = false, message = "Group name is required." });
                }

                var group = new ContactGroup
                {
                    GroupName = groupName,
                    Description = description,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.ContactGroups.Add(group);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Contact group created successfully.", groupId = group.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating contact group");
                return Json(new { success = false, message = "Error creating contact group." });
            }
        }

        // POST: Admin/ContactLists/AddContact
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddContact(int groupId, string name, string phone, string? email, string? type)
        {
            try
            {
                var contact = new ContactListItem
                {
                    ContactGroupId = groupId,
                    Name = name,
                    PhoneNumber = phone,
                    Email = email,
                    Type = type,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.ContactListItems.Add(contact);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Contact added successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding contact");
                return Json(new { success = false, message = "Error adding contact." });
            }
        }

        // POST: Admin/ContactLists/DeleteContact
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteContact(int id)
        {
            try
            {
                var contact = await _context.ContactListItems.FindAsync(id);
                if (contact == null)
                {
                    return Json(new { success = false, message = "Contact not found." });
                }

                contact.IsActive = false;
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Contact deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting contact");
                return Json(new { success = false, message = "Error deleting contact." });
            }
        }

        // POST: Admin/ContactLists/DeleteGroup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            try
            {
                var group = await _context.ContactGroups.FindAsync(id);
                if (group == null)
                {
                    return Json(new { success = false, message = "Group not found." });
                }

                group.IsActive = false;
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Contact group deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting contact group");
                return Json(new { success = false, message = "Error deleting contact group." });
            }
        }

        // GET: Admin/ContactLists/GetGroupContacts/5
        [HttpGet]
        public async Task<IActionResult> GetGroupContacts(int groupId)
        {
            try
            {
                var contacts = await _context.ContactListItems
                    .Where(c => c.ContactGroupId == groupId && c.IsActive)
                    .OrderBy(c => c.Name)
                    .Select(c => new
                    {
                        id = c.Id,
                        name = c.Name,
                        phone = c.PhoneNumber,
                        email = c.Email,
                        type = c.Type
                    })
                    .ToListAsync();

                return Json(contacts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting group contacts");
                return Json(new List<object>());
            }
        }
    }
}
