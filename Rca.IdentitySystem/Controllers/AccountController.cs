using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using Rca.IdentitySystem.Models;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Rca.IdentitySystem.Controllers
{
    public class AccountController : Controller
    {

        public RegisterModal registerModal { get; set; } = new RegisterModal();
        public string Message { get; set; }
        public UserManager<User> _userManager { get; }
        public SignInManager<User> _signInManager { get; }

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> ConfirmEmail(string userid, string token)
        {
            var user = await _userManager.FindByIdAsync(userid);
            if (user == null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    this.Message = "The Email Address succesfully Confirmed!";
                    return View();
                }
            }
            this.Message = "Failed to validate email";
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult TwoFactorLogin()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("Login");
        }

        public async Task<IActionResult> SendToRegister(RegisterModal registerModal)
        {
            var user = new User
            {
                Email = registerModal.Email,
                UserName = registerModal.Email,
                Department = registerModal.Department,
                Position = registerModal.Position
            };

            var claimDepartment = new Claim("Departement", registerModal.Department);

            var result = await _userManager.CreateAsync(user, registerModal.Password);

            if (result.Succeeded)
            {
                //it will ass claims dynamically
                await _userManager.AddClaimAsync(user, claimDepartment);

                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                return RedirectToAction("ConfirmEmail", "Account", new { userid = user.Id, token = confirmationToken });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }
            }
            return RedirectToAction("Register");
        }

        public async Task<IActionResult> LogInwithCred(CredentialsModal credentialsModal)
        {

            var result = await _signInManager.PasswordSignInAsync(
                credentialsModal.UserName,
                credentialsModal.Password,
                credentialsModal.RememberMe, false
                );

            if (result.Succeeded)
            {
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction("TwoFactorLogin", new
                    {
                        Email = credentialsModal.UserName,
                        RememberMe = credentialsModal.RememberMe
                    });
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@test.com"),
                    new Claim("Department", "HR"),
                    new Claim("admin", "true"),
                    new Claim("Manager", "true"),
                    new Claim("EmployementDate", "2023-01-01")
                };

                var data = JsonConvert.SerializeObject(claims);

                return Redirect($"https://localhost:5001/Account/LoginSuccess?data={Uri.EscapeDataString(data)}");
            }
            else
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("Login", "you are Locked out");
                }
                else
                {
                    ModelState.AddModelError("Login", "failed to login");
                }
            }

            return RedirectToAction("Login");
        }
    }
}
