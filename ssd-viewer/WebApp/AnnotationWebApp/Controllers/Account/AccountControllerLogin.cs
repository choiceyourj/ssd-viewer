using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Net;
using Microsoft.AspNetCore.Http;
using AnnotationWebApp.Models.Account;

namespace AnnotationWebApp.Controllers.Account
{
    public partial class AccountController : Controller
    {
        /*
         * Partial class for handling login method.
         * 
         */    


        /// <summary>
        /// HttpGet. Provide login page for user.
        /// </summary>
        /// <param name="returnUrl">Return URL after logged in.</param>
        /// <returns>Dashboard or page requested by return URL</returns>
        [HttpGet]
        public IActionResult Login()
        {
            bool isUserSignIn = _signInManager.IsSignedIn(User);

            if (isUserSignIn)
            {
                _logger.LogWarning("User already logged in, redirect to the previous page.");
                return Redirect("/Home/Index");
            }

            // build a model so we know what to show on the login page
            var vm = new LoginModel();

            return View(vm);
        }

        /// <summary>
        /// HttpPost. Handling user provided data for login
        /// </summary>
        /// <param name="input">LoginModel</param>
        /// <returns>Dashboard or page requested by return URL</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel input)
        {
            if (!ModelState.IsValid)
            {
                LoginModel vm = BuildLoginViewModelAsync(input);
                return View(vm);
            }

            var result = await _signInManager.PasswordSignInAsync(input.Email, input.Password, input.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation($"User '{input.Email}' logged in.");
                return Redirect("/");
            }
            else
            {
                ModelState.AddModelError(String.Empty, "아이디 혹은 비밀번호를 확인하여 주세요.");
                _logger.LogWarning($"Fail to login by user ({input.Email})");
                LoginModel vm = BuildLoginViewModelAsync(input);
                return View(vm);
            }
        }

        private LoginModel BuildLoginViewModelAsync(LoginModel model)
        {
            LoginModel vm = new LoginModel();
            vm.Email = model.Email;
            vm.RememberMe = model.RememberMe;
            return vm;
        }




    }
}
