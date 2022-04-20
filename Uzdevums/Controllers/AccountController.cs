using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uzdevums.Data;
using Uzdevums.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace Uzdevums.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext _dbcontext;
        
        public AccountController(ApplicationContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModule)
        {
            if (ModelState.IsValid)
            {
                User user = _dbcontext.Users.FirstOrDefault(u => u.Name.ToLower() == loginModule.Name.ToLower());

                if (user != null && CheckPassword(user, loginModule.Password))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                        new Claim("id", user.Id.ToString())
                    };

                    if (user.IsAdmin)
                    {
                        claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, "admin"));
                    };

                    ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

                    return Redirect("/");
                }

                ModelState.AddModelError("", "Nepareizs lietotājs vai parole");
                return View(loginModule);
            }
            else
            {
                ModelState.AddModelError("", "Nav ievadīts lietotājs vai parole");
                return View(loginModule);
            }
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public static string GetSalt()
        {
            var rng = RandomNumberGenerator.Create();
            var saltBytes = new byte[16];
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        // So varētu savā klasē, bet testa uzdevumam ir ok

        public static string SaltAndHashPassword(string password, string salt)
        {
            var sha = SHA256.Create();
            var saltedPassword = password + salt;
            return Convert.ToBase64String(sha.ComputeHash(Encoding.Unicode.GetBytes(saltedPassword)));
        }

        private static bool CheckPassword(User user, string password)
        {
            // re-generate the salted and hashed password
            var saltedhashedPassword = SaltAndHashPassword(password, user.Salt);
            return (saltedhashedPassword == user.PasswordHash);
        }
    }
}
