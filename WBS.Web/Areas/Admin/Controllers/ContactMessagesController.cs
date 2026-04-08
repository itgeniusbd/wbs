using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Attributes;
using WBS.Web.Data;
using WBS.Web.Models;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ContactMessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ContactMessagesController> _logger;

        public ContactMessagesController(ApplicationDbContext context, ILogger<ContactMessagesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Admin/ContactMessages
        [Permission("Contact Messages", "View")]
        public async Task<IActionResult> Index(string filter = "all")
        {
            IQueryable<ContactMessage> query = _context.ContactMessages;

            switch (filter.ToLower())
            {
                case "unread":
                    query = query.Where(m => !m.IsRead);
                    break;
                case "read":
                    query = query.Where(m => m.IsRead);
                    break;
                case "replied":
                    query = query.Where(m => m.IsReplied);
                    break;
                case "unreplied":
                    query = query.Where(m => !m.IsReplied);
                    break;
            }

            var messages = await query
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();

            ViewBag.Filter = filter;
            ViewBag.UnreadCount = await _context.ContactMessages.CountAsync(m => !m.IsRead);
            ViewBag.TotalCount = await _context.ContactMessages.CountAsync();
            ViewBag.RepliedCount = await _context.ContactMessages.CountAsync(m => m.IsReplied);

            return View(messages);
        }

        // GET: Admin/ContactMessages/Details/5
        [Permission("Contact Messages", "View")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.ContactMessages
                .FirstOrDefaultAsync(m => m.Id == id);

            if (message == null)
            {
                return NotFound();
            }

            // Mark as read
            if (!message.IsRead)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
            }

            return View(message);
        }

        // POST: Admin/ContactMessages/MarkAsRead/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Contact Messages", "Reply")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            message.IsRead = true;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Message marked as read";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/ContactMessages/MarkAsUnread/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Contact Messages", "Reply")]
        public async Task<IActionResult> MarkAsUnread(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            message.IsRead = false;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Message marked as unread";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/ContactMessages/MarkAsReplied/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Contact Messages", "Reply")]
        public async Task<IActionResult> MarkAsReplied(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            message.IsReplied = true;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Message marked as replied";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/ContactMessages/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Contact Messages", "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.ContactMessages.Remove(message);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Message deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/ContactMessages/DeleteSelected
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSelected(int[] selectedIds)
        {
            if (selectedIds == null || selectedIds.Length == 0)
            {
                TempData["Error"] = "No messages selected";
                return RedirectToAction(nameof(Index));
            }

            var messages = await _context.ContactMessages
                .Where(m => selectedIds.Contains(m.Id))
                .ToListAsync();

            _context.ContactMessages.RemoveRange(messages);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"{messages.Count} message(s) deleted successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
