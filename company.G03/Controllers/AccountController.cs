using company.G03.Controllers;
using company.G03.DAL.Models;
using company.G03.PL.Helper;
using company.G03.PL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Email = company.G03.PL.Helper.Email;

namespace company.G03.PL.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly UserManager<AppUsers> _usermanger;
        private readonly SignInManager<AppUsers> _signInManager;
        public AccountController(UserManager<AppUsers> userManager, SignInManager<AppUsers> signInManager)
        {
            _usermanger = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Signup(SignUpDto dto)
        {
            if (!dto.IsAgree)
            {
                ModelState.AddModelError("IsAgree", "Terms and Conditions must be agreed to.");
                return View(dto);
            }
            if (ModelState.IsValid)
            {
                var user = await _usermanger.FindByNameAsync(dto.UserName);
                if (user == null)
                {
                    user = await _usermanger.FindByEmailAsync(dto.Email);
                    if (user == null)
                    {
                        user = new AppUsers()
                        {
                            UserName = dto.UserName,
                            Email = dto.Email,
                            FirstName = dto.FirstName,
                            LastName = dto.LastName,
                            IsAgree = dto.IsAgree
                        };

                        var result = await _usermanger.CreateAsync(user, dto.Password);
                        if (result.Succeeded)
                        {

                            // ثانياً: التوجيه إلى صفحة تسجيل الدخول
                            return RedirectToAction("SignIn", "Account"); // افترض أن لديك AccountController فيه إجراء SignIn
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "This email is already registered.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "This username is already taken.");
                }
            }

            return View(dto); // البقاء في نفس الصفحة
        }
        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> SignIn(SignInDto dto)
        {
            if (ModelState.IsValid)
            {
                var user = await _usermanger.FindByEmailAsync(dto.Email); // أضيف await هنا

                if (user != null) // تحقق من null بشكل صحيح
                {
                    var flag = await _usermanger.CheckPasswordAsync(user, dto.Password);

                    if (flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, dto.Password, dto.RememberMe,false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    }
                }

                TempData["ErrorMessage"] = "Incorrect email or password";
            }
            return View(dto);
        }
        [HttpGet]
        public new async Task<ActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> SendResetPasswordURL(ForgetPassDto dto)
        {
            if (ModelState.IsValid)
            {
                var user = await _usermanger.FindByEmailAsync(dto.Email);
                if (user != null)
                {
                    try
                    {
                        var token = await _usermanger.GeneratePasswordResetTokenAsync(user);
                        var callbackUrl = Url.Action(
                            "ResetPassword",
                            "Account",
                            new { email = dto.Email, code = token },
                            protocol: Request.Scheme);

                        var email = new Email()
                        {
                            To = dto.Email,
                            Subject = "Reset Your Password",
                            Body = $"Please reset your password by clicking here: <a href='{callbackUrl}'>Reset Password</a>"
                        };
 
                       if (EmailSetting.SendEmail(email))
                        {//دي بتشغل الدالة المسؤولة عن إرسال الايميل فعلياً:
                         //زود Logging عشان تتأكد إن الإيميل اتبعت:
                         //لو نجحت(true): هيعمل رسالة نجاح

                            //لو فشلت(false): هيعمل رسالة خطأ
                            Console.WriteLine($"Email sent at: {DateTime.Now}");
                            TempData["SuccessMessage"] = "Reset link has been sent to your email";
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Failed to send email. Please try again later.";
                            return RedirectToAction("ForgetPassword");
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                        return RedirectToAction("ForgetPassword");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Email not found");
                }
            }

            return View("ForgetPassword", dto);
        }
        [HttpGet]
        [HttpGet]
        public ActionResult ResetPassword(string email, string code)
        {
            TempData["email"] = email;
            TempData["token"] = code;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ResetPassword(ResetPassDto dto)
        {
           
            if (ModelState.IsValid)
            {
                var email =   TempData["email"] as string;
                var token =  TempData["token"] as string;
                if(email == null || token == null)
                {
                    TempData["ErrorMessage"] = "Invalid opertation";
                    return RedirectToAction("ForgetPassoword");
                }
                var user = await _usermanger.FindByEmailAsync(email);
                if (user != null)
                {
                    var result = await _usermanger.ResetPasswordAsync(user, token, dto.NewPassword);
                    if (result.Succeeded)
                    {
                        TempData["SuccessMessage"] = "Password reset successfully";
                        return RedirectToAction("SignIn");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(dto);
        }

    }
}
 