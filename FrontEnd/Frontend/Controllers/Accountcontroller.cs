using Microsoft.AspNetCore.Mvc;
using Frontend.Models;

namespace Frontend.Controllers
{
    public class AccountController : Controller
    {
        // ── GET /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ModelState.Clear();
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        // ── POST /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError(string.Empty, "Username and password are required.");
                ViewBag.ShowErrors = true;
                return View(model);
            }

            if (model.Email == "admin" && model.Password == "admin123")
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);

                return RedirectToAction("Dashboard", "Account");
            }

            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            ViewBag.ShowErrors = true;
            return View(model);
        }

        // ── GET /Account/ForgotPassword
        [HttpGet]
        public IActionResult ForgotPassword() =>
            Content("Forgot password — coming soon.");

        // ── Views
        public IActionResult Dashboard() => View("Dashboard");
        public IActionResult Students() => View("Students");
        public IActionResult Teachers() => View("Teachers");
        public IActionResult Courses() => View("Courses");
        public IActionResult Classes() => View("Classes");
        public IActionResult Enrollment() => View("Enrollment");
        public IActionResult Attendance() => View("Attendance");
        public IActionResult Reports() => View("Reports");
        public IActionResult Settings() => View("Settings");
        public IActionResult Imports() => View("Imports");
        public IActionResult Start() => View("Start");
        // ── POST /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout() =>
            RedirectToAction("Login", "Account");
    }
}