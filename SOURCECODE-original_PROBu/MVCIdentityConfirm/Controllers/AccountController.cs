﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using MVCIdentityConfirm.Models;
using System.Net.Mail;
using Newtonsoft.Json;
using System.Net;
using System.Data.SqlClient;
using System.Configuration;

namespace MVCIdentityConfirm.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {

            var response = HttpContext.Request.Form["g-recaptcha-response"];
            string secretKey = "6Leh4yQTAAAAAN-muZrU0mBJcrh2Efxr3aDDT9JN";
            var client = new WebClient();
            // get captcha verification result
            var verificationResultJson = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            // convert json to object
            var verificationResult = JsonConvert.DeserializeObject<CaptchaResponse>(verificationResultJson);
            // process verification result
            if (!verificationResult.Success)
            {
                if (verificationResult.ErrorCodes.Count <= 0)
                {
                    return View(model);
                }
                var error = verificationResult.ErrorCodes[0].ToLower();
                switch (error)
                {
                    case ("missing-input-secret"):
                        ModelState.AddModelError("", "The secret parameter is missing.");
                        ViewBag.CaptchaMessage = "The secret parameter is missing.";
                        break;
                    case ("invalid-input-secret"):
                        ModelState.AddModelError("", "The secret parameter is invalid or malformed.");
                        ViewBag.CaptchaMessage = "The secret parameter is invalid or malformed.";
                        break;

                    case ("missing-input-response"):
                        ViewBag.Warning = "Potwierdź że jesteś człowiekiem!";
                        ViewBag.CaptchaMessage = "The captcha input is missing.";
                        break;
                    case ("invalid-input-response"):
                        ModelState.AddModelError("", "The captcha input is invalid or malformed.");
                        ViewBag.CaptchaMessage = "The captcha input is invalid or malformed.";
                        break;

                    default:
                        ModelState.AddModelError("", "Error occured. Please try again");
                        ViewBag.CaptchaMessage = "Error occured. Please try again";
                        break;
                }
                return View(model);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var user = await UserManager.FindAsync(model.UserName, model.Password);


                    if (user != null)
                    {
                        if (user.ConfirmedEmail == true)
                        {
                            await SignInAsync(user, model.RememberMe); return RedirectToLocal(returnUrl);
                        }
                        else
                        {
                            ViewBag.Warning = "Potwierdź adres email!";
                        }
                    }
                    else
                    {
                        ViewBag.Warning = "Zła nazwa użytkonika lub hasło!";
                    }
                }
                // If we got this far, something failed, redisplay form
                return View(model);

            }





            
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {

            var response = HttpContext.Request.Form["g-recaptcha-response"];
            string secretKey = "6Leh4yQTAAAAAN-muZrU0mBJcrh2Efxr3aDDT9JN";
            var cli = new WebClient();
            // get captcha verification result
            var verificationResultJson = cli.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            // convert json to object
            var verificationResult = JsonConvert.DeserializeObject<CaptchaResponse>(verificationResultJson);
            // process verification result
            if (!verificationResult.Success)
            {
                if (verificationResult.ErrorCodes.Count <= 0)
                {
                    return View(model);
                }
                var error = verificationResult.ErrorCodes[0].ToLower();
                switch (error)
                {
                    case ("missing-input-secret"):
                        ModelState.AddModelError("", "The secret parameter is missing.");
                        ViewBag.CaptchaMessage = "The secret parameter is missing.";
                        break;
                    case ("invalid-input-secret"):
                        ModelState.AddModelError("", "The secret parameter is invalid or malformed.");
                        ViewBag.CaptchaMessage = "The secret parameter is invalid or malformed.";
                        break;

                    case ("missing-input-response"):
                        ModelState.AddModelError("", "Proszę potwierdzić że jesteś człowiekiem ;)");
                        ViewBag.CaptchaMessage = "The captcha input is missing.";
                        break;
                    case ("invalid-input-response"):
                        ModelState.AddModelError("", "The captcha input is invalid or malformed.");
                        ViewBag.CaptchaMessage = "The captcha input is invalid or malformed.";
                        break;

                    default:
                        ModelState.AddModelError("", "Error occured. Please try again");
                        ViewBag.CaptchaMessage = "Error occured. Please try again";
                        break;
                }
                return View(model);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    userDB userS = new userDB();
                    var existEmail = from x in userS.AspNetUsers where x.Email == model.Email select x.Email;
                    var existLogin = from a in userS.AspNetUsers where a.UserName == model.UserName select a.UserName;

                    if (existEmail.Count() == 0)
                    {
                        if (existLogin.Count() == 0)
                        {
                            try
                            {
                                var user = new ApplicationUser() { UserName = model.UserName };
                                user.Email = model.Email;
                                user.Imie = model.Imie;
                                user.Nazwisko = model.Nazwisko;
                                user.Adres = model.Adres;
                                user.ConfirmedEmail = false;
                                
                                
                                
                                    SmtpClient client = new SmtpClient
                                    {
                                        Port = 587,
                                        Host = "smtp.wp.pl",
                                        EnableSsl = true,
                                        Timeout = 900000,
                                        DeliveryMethod = SmtpDeliveryMethod.Network,
                                        UseDefaultCredentials = false,
                                        Credentials = new System.Net.NetworkCredential("marcin_dziekan@wp.pl", "kolmag1")
                                    };
                                    var objeto_mail = new MailMessage { From = new MailAddress("\"Milczenie Praconików\" <marcin_dziekan@wp.pl>") };
                                    objeto_mail.To.Add(new MailAddress(user.Email));
                                    objeto_mail.Subject = "Potwierdzenie adresu email.";
                                    objeto_mail.Body = string.Format("Drogi/a {0}. Dziękujemy za rejestrację, żeby ukończyć proces rejestracji kliknij link: {1}", user.UserName, Url.Action("ConfirmEmail", "Account", new { Token = user.Id, Email = user.Email }, Request.Url.Scheme));
                                    client.Send(objeto_mail);
                                    var result = await UserManager.CreateAsync(user, model.Password);
                                if (result.Succeeded)
                                {
                                    return RedirectToAction("Confirm", "Account", new { Email = user.Email });
                                }
                                else
                                {
                                    AddErrors(result);
                                }
                            }catch
                            {
                                ModelState.AddModelError("", "Podany adres e-mail nie istnieje!");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Nistety ten login jest już zajęty!");
                        }
                    }else
                    {
                        ModelState.AddModelError("", "Nistety ten e-mail został już użyty wcześniej!");
                    }
                    
                }
                // If we got this far, something failed, redisplay form
                return View(model);





            }
        }

        [AllowAnonymous]
        public ActionResult Confirm(string Email)
        {
            ViewBag.Email = Email;
            return View();
        }
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string Token, string Email)
        {
            ApplicationUser user = this.UserManager.FindById(Token);
            if (user != null)
            {
                if (user.Email == Email)
                {
                    user.ConfirmedEmail = true;
                    await UserManager.UpdateAsync(user);
                    await SignInAsync(user, isPersistent: false);
                    return RedirectToAction("ChooseCompany", "Home", new { ConfirmedEmail = user.Email });
                }
                else
                {
                    return RedirectToAction("Confirm", "Account", new { Email = user.Email });
                }
            }
            else
            {
                return RedirectToAction("Confirm", "Account", new { Email = "" });
            }

        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Twoje hasło zostało zmienione."
                : message == ManageMessageId.SetPasswordSuccess ? "Twoje hasło zostało ustawione."
                : message == ManageMessageId.RemoveLoginSuccess ? "Zewnetrzny login został usunięty."
                : message == ManageMessageId.Error ? "Przepraszmy, wystąpił błąd!."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}