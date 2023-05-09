using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace NewOnlineVotingApplication.Controllers
{
    public class AdminController : Controller
    {
        private const string AdminUsername = "Admin";
        private const string AdminPassword = "abc@123";
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            if (username == AdminUsername && password == AdminPassword)
            {
                // Set a session variable to indicate that the Admin is logged in
                HttpContext.Session.SetString("AdminLoggedIn", "true");

                // Create and set authentication cookie
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                var authScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                HttpContext.SignInAsync(authScheme, new ClaimsPrincipal(new ClaimsIdentity(authClaims, authScheme)), authProperties);

                return RedirectToAction(nameof(AdminPage));
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View();
            }
        }

        //Admin/Logout 
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Logout()
        {
            // Remove the "AdminLoggedIn" session variable
            HttpContext.Session.Remove("AdminLoggedIn");

            // Sign out the authentication cookie
            var authScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            HttpContext.SignOutAsync(authScheme);

            return RedirectToAction(nameof(Login));
        }
        //Admin/Admin Page 
        [Authorize(Roles ="Admin")]
        public IActionResult AdminPage()
        {
            return View();

        }
    }
}
