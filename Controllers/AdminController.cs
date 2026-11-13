using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MawethuGym.Web.Data;
using MawethuGym.Web.Models;

namespace MawethuGym.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsAdminAuthenticated() => HttpContext.Session.GetString("StaffRole") == "Admin";

        // GET: /Admin/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            if (!IsAdminAuthenticated()) return RedirectToAction("Login", "Staff");

            ViewBag.StaffMembers = await _context.Staff.ToListAsync();
            ViewBag.Payments = await _context.Payments
                .Include(p => p.Member)
                .Include(p => p.Staff)
                .OrderByDescending(p => p.Timestamp)
                .ToListAsync();
            
            ViewBag.MembersWithStaff = await _context.Members
                .Include(m => m.RegisteredByStaff)
                .OrderByDescending(m => m.Id)
                .ToListAsync();

            return View();
        }

        // POST: /Admin/AddStaff
        [HttpPost]
        public async Task<IActionResult> AddStaff(Staff staff)
        {
            if (!IsAdminAuthenticated()) return RedirectToAction("Login", "Staff");

            _context.Staff.Add(staff);
            await _context.SaveChangesAsync();
            return RedirectToAction("Dashboard");
        }

        // POST: /Admin/DeleteStaff/5
        [HttpPost]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            if (!IsAdminAuthenticated()) return RedirectToAction("Login", "Staff");

            var staff = await _context.Staff.FindAsync(id);
            if (staff != null)
            {
                _context.Staff.Remove(staff);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Dashboard");
        }
    }
}