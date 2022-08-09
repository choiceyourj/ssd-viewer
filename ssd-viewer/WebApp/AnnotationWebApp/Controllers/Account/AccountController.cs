
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
using AnnotationWebApp.Data;

namespace AnnotationWebApp.Controllers.Account
{
    /*
     * 211230 - mjcho
     * 1. Add Email Templete Generator
     *
     * 2022.01.28 - jgbak
     * 1. Change to Partial Class
     * 
     */

    public partial class AccountController : Controller
    {
        private readonly SignInManager<SsdUser> _signInManager;
        private readonly UserManager<SsdUser> _userManager;
        private readonly AppDbContext _dbContext;
        private readonly ILogger<AccountController> _logger;

        public AccountController(SignInManager<SsdUser> signInManager, UserManager<SsdUser> userManager, AppDbContext dbContext, ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        //jhlee
        [HttpGet]
        public IActionResult Index()
        {
            return View("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            if (_signInManager.IsSignedIn(User))
            {
                await _signInManager.SignOutAsync();
                return View();
            }

            return Redirect("/Home");
        }


        /// <summary>
        /// HttpGet. Provide register (signUp) page for new user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult SignUp()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return Redirect("/");
            }

            var vm = new SignUpViewModel();

            return View(vm);
        }

        /// <summary>
        /// HttpPost. Handling user provided data and create new user.
        /// </summary>
        /// <param name="input">SignupInputModel</param>
        /// <returns>If success, redirect to EMF home, else redirect to sigup page.</returns>
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpInputModel input)
        {
            // Validate Model state
            if (!ModelState.IsValid)
            {
                SignUpViewModel vm = BuildSignUpViewModel(input);
                return View(vm);
            }


            // Check existence of email address
            bool isExistEmail = await _dbContext.Users.Where(i => i.NormalizedEmail == input.Email.ToUpper()).AnyAsync();
            
            if (isExistEmail)
            {
                _logger.LogWarning($"Email {input.Email} is already exist.");
                ModelState.AddModelError("Email", "이메일 주소가 이미 등록되어 있습니다.");
                SignUpViewModel vm = BuildSignUpViewModel(input);
                return View(vm);
            }

            // Create New User
            var newUser = new SsdUser
            {
                UserDisplayName = input.Email,
                UserName = input.Email,
                Email = input.Email,
            };

            var userCreationResult = await _userManager.CreateAsync(newUser, input.Password);

            if (userCreationResult.Succeeded)
            {
                _logger.LogInformation($"New account has been created with email {input.Email}");

                var loginResult = await _signInManager.PasswordSignInAsync(input.Email, input.Password, false, lockoutOnFailure: false);
                if (loginResult.Succeeded)
                {
                    _logger.LogInformation($"User '{input.Email}' logged in.");
                    return Redirect("/");
                }
                else
                {
                    _logger.LogError($"Login failed for the user just created.");
                    return Redirect("/");
                }
            }
            else
            {
                foreach (var item in userCreationResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                SignUpViewModel vm = BuildSignUpViewModel(input);
                return View(vm);
            }
        }

        /// <summary>
        /// Build ViewModel for SignUp Page
        /// </summary>
        /// <param name="model">SignUp Input Model</param>
        /// <returns>SignUpViewModel</returns>
        private SignUpViewModel BuildSignUpViewModel(SignUpInputModel model)
        {
            var vm = new SignUpViewModel();
            vm.Email = model.Email;
            vm.Password = model.Password;
            vm.ConfirmPassword = model.ConfirmPassword;
            return vm;
        }
    }
}
