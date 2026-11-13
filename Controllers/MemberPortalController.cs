using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MawethuGym.Web.Data;
using MawethuGym.Web.Models;

namespace MawethuGym.Web.Controllers
{
    public class MemberPortalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MemberPortalController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /MemberPortal/Index?memberNumber=M001
        public async Task<IActionResult> Index(string memberNumber)
        {
            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.MemberNumber == memberNumber) 
                ?? await _context.Members.FirstOrDefaultAsync();

            if (member == null) return RedirectToAction("Index", "Home");

            // Calculate contract days remaining
            var daysRemaining = (member.ExpiryDate - DateTime.UtcNow).Days;
            ViewBag.DaysRemaining = daysRemaining > 0 ? daysRemaining : 0;

            // Fetch check-ins for streak and history calculations
            var checkIns = await _context.CheckIns
                .Where(c => c.MemberId == member.Id)
                .OrderByDescending(c => c.Timestamp)
                .ToListAsync();

            ViewBag.RecentVisits = checkIns.Take(5).ToList();
            ViewBag.TotalVisits = checkIns.Count;

            return View(member);
        }

        // POST: /MemberPortal/UpdateWeightGoal
        [HttpPost]
        public async Task<IActionResult> UpdateWeightGoal(string memberNumber, double currentWeight, double weightGoal)
        {
            var member = await _context.Members.FirstOrDefaultAsync(m => m.MemberNumber == memberNumber);
            if (member != null)
            {
                member.CurrentWeightKg = currentWeight;
                member.WeightGoalKg = weightGoal;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", new { memberNumber });
        }
    }
}