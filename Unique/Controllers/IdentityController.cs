using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Unique.Models;

namespace Unique.Controllers
{
    public class IdentityController : Controller
    {
        public class AuthenticateController : Controller
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly SignInManager<ApplicationUser> _signInManager;

            public AuthenticateController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
            {
                _userManager = userManager;
                _signInManager = signInManager;
            }

            public async Task<IActionResult> Login()
            {
                if (User.Identity.IsAuthenticated)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("Login");
                };

                return View();

            }

            [HttpPost]
            public async Task<IActionResult> Login(string userName, string password)
            {
                //login functionality
                var user = await _userManager.FindByNameAsync(userName);


                if (user != null)
                {
                    var role = user.Access;

                    //sign in
                    var signInResult = await _signInManager.PasswordSignInAsync(user.UserName, password, false, false);

                    if (signInResult.Succeeded)
                    {
                        if (role == "Admin")
                        {
                            return RedirectToAction("Index", "Admin", new { area = "" });

                        }
                        else if (role == "Student")
                        {

                            return RedirectToAction("Index", "Student", new { area = "" });
                        }

                    }
                }

                return View();
            }


            //[Authorize(Roles = "Admin")]
            public IActionResult CreateStudent()
            {

                return View();
            }

            [HttpPost]
            public async Task<IActionResult> CreateStudent(string password, string userName, string firstName, string lastName, string homeAddress, string role)
            {
                //register functionality

                var user = new ApplicationUser
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Access = role,
                    HomeAddress = homeAddress,
                    UserName = userName,
                };

                var _password = password;

                var result = await _userManager.CreateAsync(user, _password);

                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync(user.UserName);

                    return RedirectToAction("Index", "Admin", new { area = "" });

                }

                return RedirectToAction("Index", "Admin", new { area = "" });
            }


            public async Task<IActionResult> LogOut()
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login");
            }

        }

    }
}
