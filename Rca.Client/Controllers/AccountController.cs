using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rca.Client.Models;
using System.Security.Claims;

namespace Rca.Client.Controllers
{
    public class AccountController : Controller
    {
        [BindProperty]
        public CredentialsModal CredentialsModal { get; set; }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("LogIn");
        }

        public async Task<IActionResult> LogInwithCred(CredentialsModal credentialsModal)
        {
            if (credentialsModal.UserName == "admin" && credentialsModal.Password == "password")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@test.com"),
                    new Claim("Department", "HR"),
                    new Claim("admin", "true"),
                    new Claim("Manager", "true"),
                    new Claim("EmployementDate", "2023-01-01")
                };

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = credentialsModal.RememberMe
                };

                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, authProperties);

                return Redirect("/Home/Index");
            }
            return RedirectToAction("LogIn");
        }
    }
}
