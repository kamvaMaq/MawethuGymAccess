using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MawethuGym.Web.Data;
using MawethuGym.Web.ViewModels;

namespace MawethuGym.Web.Controllers
{
    public class MemberController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MemberController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Member/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Member/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(MemberRegisterViewModel model)
        {
            if (!model.BiometricConsentGiven)
            {
                ModelState.AddModelError("BiometricConsentGiven", "You must consent to biometric processing or opt for manual check-in under POPIA guidelines.");
            }

            if (ModelState.IsValid)
            {
                // Generate unique member number
                string memberNumber = "MG-" + new Random().Next(1000, 9999);

                // Calculate expiry date based on selected plan
                int months = model.PlanType switch
                {
                    "3 Months" => 3,
                    "6 Months" => 6,
                    _ => 1
                };

                var member = new Member
                {
                    MemberNumber = memberNumber,
                    FullName = model.FullName,
                    Phone = model.Phone,
                    Email = model.Email,
                    PlanType = model.PlanType,
                    ExpiryDate = DateTime.UtcNow.AddMonths(months),
                    BiometricConsentGiven = model.BiometricConsentGiven
                };

                _context.Members.Add(member);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", new { message = "Registration successful! Your member number is " + memberNumber });
            }

            return View(model);
        }

        // GET: /Member/Login
        [HttpGet]
        public IActionResult Login(string? message)
        {
            ViewBag.Message = message;
            return View();
        }

        // POST: /Member/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(MemberLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var member = await _context.Members
                    .FirstOrDefaultAsync(m => m.Email == model.Identifier || m.Phone == model.Identifier || m.MemberNumber == model.Identifier);

                if (member != null)
                {
                    // Store member ID in session for lightweight authentication simulation
                    HttpContext.Session.SetInt32("LoggedMemberId", member.Id);
                    return RedirectToAction("Home");
                }

                ModelState.AddModelError(string.Empty, "Member not found. Check your email or phone number.");
            }

            return View(model);
        }

        // GET: /Member/Home
        [HttpGet]
        public async Task<IActionResult> Home()
        {
            int? memberId = HttpContext.Session.GetInt32("LoggedMemberId");
            if (memberId == null)
            {
                return RedirectToAction("Login");
            }

            var member = await _context.Members.FindAsync(memberId);
            return View(member);
        }

        // GET: /Member/CheckIn
        [HttpGet]
        public IActionResult CheckIn()
        {
            return View();
        }

        // POST: /Member/CheckIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn(string identifier, string method = "Biometric", bool isOverride = false)
        {
            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.MemberNumber == identifier || m.Email == identifier || m.Phone == identifier);

            if (member == null)
            {
                ViewBag.Error = "Member not found. Please register or check your details.";
                return View();
            }

            // Check if membership is expired
            if (member.ExpiryDate < DateTime.UtcNow)
            {
                ViewBag.Error = $"Access Denied: Membership expired on {member.ExpiryDate:yyyy-MM-dd}. Please renew your plan.";
                return View();
            }

            // Record check-in
            var checkIn = new CheckIn
            {
                MemberId = member.Id,
                Timestamp = DateTime.UtcNow,
                Method = method,
                IsOverride = isOverride
            };

            _context.CheckIns.Add(checkIn);
            await _context.SaveChangesAsync();

            ViewBag.Success = $"Access Granted! Welcome back, {member.FullName}.";
            return View();
        }

        // GET: /Member/AdminDashboard
        [HttpGet]
        public async Task<IActionResult> AdminDashboard()
        {
            var members = await _context.Members.ToListAsync();
            var recentCheckIns = await _context.CheckIns
                .Include(c => c.Member)
                .OrderByDescending(c => c.Timestamp)
                .Take(10)
                .ToListAsync();

            ViewBag.TotalMembers = members.Count;
            ViewBag.ActiveMembers = members.Count(m => m.ExpiryDate >= DateTime.UtcNow);
            
            return View(recentCheckIns);
        }
    }
}