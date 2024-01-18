using Cafe.Domain.ViewModel.Account;
using Cafe.Servises.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
namespace Cafe.Controllers
{
        public class AccountController : Controller
        {
            private readonly IAccountService _accountService;

            public AccountController(IAccountService accountService)
            {
                _accountService = accountService;
            }

            [HttpGet]
            public IActionResult Register() => View();

            [HttpPost]
            public IActionResult Register(RegisterViewModel model)
            {
                if (ModelState.IsValid)
                {
                    var response = _accountService.Register(model);
                    if (response.StatusCode == Domain.Enums.StatusCode.OK)
                    {
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(response.Data));

                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", response.Description);
                }
                return View(model);
            }

            [HttpGet]
            public IActionResult Login() => View();

            [HttpPost]
            public IActionResult Login(LoginViewModel model)
            {
                if (ModelState.IsValid)
                {
                    var response = _accountService.Login(model);
                    if (response.StatusCode == Domain.Enums.StatusCode.OK)
                    {
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(response.Data));

                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", response.Description);
                }
                return View(model);
            }

            [ValidateAntiForgeryToken]
            public IActionResult Logout()
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Index", "Home");
            }

            [HttpPost]
            public IActionResult ChangePassword(ChangePasswordViewModel model)
            {
                if (ModelState.IsValid)
                {
                    var response = _accountService.ChangePassword(model);
                    if (response.StatusCode == Domain.Enums.StatusCode.OK)
                    {
                        return RedirectToAction("Detail", "Profile");
                    }
                }
                var modelError = ModelState.Values.SelectMany(v => v.Errors);

                return StatusCode(StatusCodes.Status500InternalServerError, new { modelError.FirstOrDefault().ErrorMessage });
            }
        }
    }
