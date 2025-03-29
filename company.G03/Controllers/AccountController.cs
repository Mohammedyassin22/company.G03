using company.G03.Controllers;
using company.G03.DAL.Models;
using company.G03.PL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
    }
}
