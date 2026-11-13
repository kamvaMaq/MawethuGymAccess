using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MawethuGym.Web.Data;
using MawethuGym.Web.Models;

namespace MawethuGym.Web.Controllers
{
    public class StaffController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StaffController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Check if user is logged in as staff
        private bool IsStaffAuthenticated() => HttpContext.Session.GetString("StaffRole") != null;

        public IActionResult Dashboard()
        {
            if (!IsStaffAuthenticated()) return RedirectToAction("Login");
            return View();
        }

        // GET: /Staff/RegisterClient
        public IActionResult RegisterClient()
        {
            if (!IsStaffAuthenticated()) return RedirectToAction("Login");
            return View();
        }

        // POST: /Staff/RegisterClient
        [HttpPost]
        public async Task<IActionResult> RegisterClient(Member member, string faceDescriptorJson)
        {
            if (!IsStaffAuthenticated()) return RedirectToAction("Login");

            // Track which staff member registered this client
            var staffUsername = HttpContext.Session.GetString("StaffName");
            var staff = await _context.Staff.FirstOrDefaultAsync(s => s.Username == staffUsername);

            member.RegisteredByStaffId = staff?.Id;
            member.FaceDescriptorJson = faceDescriptorJson;
            member.ExpiryDate = DateTime.UtcNow.AddMonths(1); // Default initial plan

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Client {member.FullName} registered successfully!";
            return RedirectToAction("Dashboard");
        }

        // POST: /Staff/RecordPayment
        [HttpPost]
        public async Task<IActionResult> RecordPayment(int memberId, decimal amount, string method)
        {
            if (!IsStaffAuthenticated()) return RedirectToAction("Login");

            var staffUsername = HttpContext.Session.GetString("StaffName");
            var staff = await _context.Staff.FirstOrDefaultAsync(s => s.Username == staffUsername);

            var payment = new Payment
            {
                MemberId = memberId,
                Amount = amount,
                Method = method, // Cash, Card, EFT
                StaffId = staff?.Id ?? 1,
                Timestamp = DateTime.UtcNow
            };

            // Extend client expiry date by 1 month upon payment
            var member = await _context.Members.FindAsync(memberId);
            if (member != null)
            {
                member.ExpiryDate = member.ExpiryDate > DateTime.UtcNow 
                    ? member.ExpiryDate.AddMonths(1) 
                    : DateTime.UtcNow.AddMonths(1);
            }

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Payment recorded and membership extended!";
            return RedirectToAction("Dashboard");
        }
    }
}
