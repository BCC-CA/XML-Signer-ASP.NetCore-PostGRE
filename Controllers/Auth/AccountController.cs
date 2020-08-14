using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XmlSigner.Data.Models;
using XmlSigner.ViewModels;

namespace XmlSigner.Controllers.Auth
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> signInManager;
        public AccountController(UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            this._userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Regiter()
        {
            //return View("Views/Home/About.cshtml");
            return View("Views/Auth/Account/Register.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.Email,
                    Email = model.Email,
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, Request.Scheme);

                    /*
                                        // For more information on how to enable account confirmation and password reset please   
                                        //visit https://go.microsoft.com/fwlink/?LinkID=532713  
                                        // Send an email with this link  
                                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                                        var callbackUrl = Url.Action(nameof(ConfirmEmail), "Account",
                                        new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                                        await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                                           $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");

                                        TempData["Message"] = "Confirmation Email has been send to your email. Please check email.";
                                        TempData["MessageValue"] = "1";
                    */
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }
    }
}
