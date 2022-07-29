using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using asdfqsf.Models;
using System.Text.RegularExpressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
namespace asdfqsf.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<SiteUser> UserAuth;
        private readonly SignInManager<SiteUser> UserSignIn;

        public AuthController(UserManager<SiteUser> UserAuth, SignInManager<SiteUser> UserSignIn)
        {
            this.UserAuth = UserAuth;
            this.UserSignIn = UserSignIn;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await this.UserSignIn.SignOutAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUser model)
        {
            if (ModelState.IsValid)
            {
                SiteUser user = new SiteUser()
                {
                    Email = model.Email,
                    UserName = model.FirstName,
                    FirstName = model.FirstName
                };

                IdentityResult result = await this.UserAuth.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await this.UserSignIn.SignInAsync(user, isPersistent: true);
                    return RedirectToAction(nameof(Index), "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUser model)
        {
            if (ModelState.IsValid)
            {
                SiteUser user;
                if (model.Email.Contains("@"))
                {
                    user = await this.UserAuth.FindByEmailAsync(model.Email);
                }
                else
                {
                    user = await this.UserAuth.FindByNameAsync(model.Email);
                }
                var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.Id)),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim("FavoriteDrink", "Tea")
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                {
                    IsPersistent = model.RememberMe
                });
                var result = await this.UserSignIn.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Manage", "Home");
                }
            }
            return View(model);
        }
    }
}
