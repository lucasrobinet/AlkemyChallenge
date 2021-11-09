using AhoraSi.Models;
using AhoraSi.Models.ViewModels;
using AspNetCoreEmailConfirmationSendGrid.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhoraSi.Controllers
{
    public class AuthsController : Controller
    {
        //private readonly DataBaseContext _context;
        private readonly UserManager<IdentityUser> userGestion;
        private readonly SignInManager<IdentityUser> loginGestion;
        private readonly RoleManager<IdentityRole> rolGestion;
        private IEmailSender mailService;
        private IConfiguration _configuration;

        public AuthsController(UserManager<IdentityUser> userGestion, SignInManager<IdentityUser> loginGestion, IEmailSender mailService, IConfiguration configuration, RoleManager<IdentityRole> rolGestion)
        {
            this.userGestion = userGestion;
            this.loginGestion = loginGestion;
            this.mailService = mailService;
            this._configuration = configuration;
            this.rolGestion = rolGestion;
        }

        [HttpGet]
        [Route("Auth/Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Auth/Register")]
        public async Task<IActionResult> Register(Auth obj)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = obj.Name,
                    Email = obj.Email
                };

                var result = await userGestion.CreateAsync(user, obj.Password);

                if (result.Succeeded)
                {
                    var confirEmailToken = await userGestion.GenerateEmailConfirmationTokenAsync(user);
                    var encodedEmailToken = Encoding.UTF8.GetBytes(confirEmailToken);   //corroborar utilidad de esta linea y la de abajo
                    var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);
                    var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, token = validEmailToken }, Request.Scheme);
                    //string url = $"{_configuration["AppUrl"]}/Auth/ConfirmEmail?userid={user.Id}&token={validEmailToken}";
                    await mailService.SendEmailAsync(obj.Email, "Confirm your account!", "<h1>Gracias por registrarte en Cuevana 15!</h1>"+$"<p>Confirma la cuenta haciendo haciendo <a href='{confirmationLink}'>click aqui</a></p>");

                    //await mailService.SendEmailAsync(obj.Email, "New Account", "<h1>Gracias por registrarte en Cuevana 15, recuerda registrarte en Cuevana 16 la proxima semana!</h1>");
                    await loginGestion.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index","Home");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(obj);
        }

        [HttpPost]
        [Route("Auth/CloseSession")]
        public async Task<IActionResult> CloseSession()
        {
            await loginGestion.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("Auth/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Auth/Login")]
        public async Task<IActionResult> Login(LoginViewModel obj)
        {
            if (ModelState.IsValid)
            {
                var result = await loginGestion.PasswordSignInAsync(
                    obj.Name, obj.Password, obj.Remember, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login");
            }

            return View(obj);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Auth/ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await userGestion.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var decodedToken = WebEncoders.Base64UrlDecode(token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await userGestion.ConfirmEmailAsync(user, normalToken);
            var rol = await rolGestion.FindByNameAsync("User");

            if (result.Succeeded)
            {
                await userGestion.AddToRoleAsync(user, rol.Name);
                return View();
            }

            return View("Error");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Auth/ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Auth/ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userGestion.FindByEmailAsync(model.Email);

                if(user != null && await userGestion.IsEmailConfirmedAsync(user))
                {
                    var token = await userGestion.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "Auth",
                        new { email = model.Email, token = token }, Request.Scheme);
                    await mailService.SendEmailAsync(model.Email, "Reset Password", $"<p>if you want to reset your password <a href='{passwordResetLink}'>click aqui</a> , otherwise ignore this email</p>");

                    return View("ForgotPasswordConfirmation");
                }
                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Auth/ResetPassword")]
        public IActionResult ResetPassword(string token, string email)
        {
            if(token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Auth/ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userGestion.FindByEmailAsync(model.Email);
                if(user != null)
                {
                    var result = await userGestion.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
                return View("ResetPasswordConfirmation");
            }
            return View(model);
        }
    }
}
