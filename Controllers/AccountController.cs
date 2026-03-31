using HotelReymer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HotelReymer.Controllers
{
    public class AccountController : Controller
    {
        private readonly HotelContext _context;

        public AccountController(HotelContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string login, string password)
        {
            var hash = ComputeSha256(password);

            var user = await _context.UserAccounts
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Login == login && u.Hash == hash);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim("Login", user.Login),
                    new Claim("UserId", user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.Name)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity));

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Неверный логин или пароль";
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        private static string ComputeSha256(string input)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexStringLower(bytes);
        }
    }
}
