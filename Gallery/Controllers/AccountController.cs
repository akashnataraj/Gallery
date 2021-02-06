using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Gallery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Gallery.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //signin attempt
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                    //onsuccess redirect to home page
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Gallery", "Home");
                    }

                    ModelState.AddModelError("customError", _configuration.GetValue<string>("Message:InvalidLogin"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Create IdentityUser model
                    var user = new IdentityUser
                    {
                        UserName = model.Email,
                        Email = model.Email
                    };

                    //Create user
                    var result = await _userManager.CreateAsync(user, model.Password);

                    //onsuccess reditect to Home page
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Gallery", "Home");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("customError", error.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}