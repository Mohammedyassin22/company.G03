using company.G03.DAL.Models;
using company.G03.PL.Helper;
using company.G03.PL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace company.G03.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUsers> _userManager;
        public UserController (UserManager<AppUsers> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<UserToReturnDto> users;
            if (string.IsNullOrEmpty(SearchInput))
            {
                users = _userManager.Users.Select(x => new UserToReturnDto
                {
                    Id = x.Id,
                    FirstName= x.FirstName,
                    LastName= x.LastName,
                    Email=x.Email,
                    UserName=x.UserName,
                    Roles=_userManager.GetRolesAsync(x).Result
                });
            }
            else
            {
                users = _userManager.Users.Select(x => new UserToReturnDto
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    UserName = x.UserName,
                    Roles = _userManager.GetRolesAsync(x).Result
                }).Where(x => x.FirstName.ToLower().Contains(SearchInput.ToLower()));
            }
            return View(users);
        }


        [HttpGet]
        public async Task<IActionResult> Details(string? id, string ViewName)
        {
            if (id is null)
            {
                return BadRequest("is valid");
            }
            var emp = await _userManager.FindByIdAsync(id);
            if (emp == null)
            {
                return NotFound(new { Message = "Users with id is not found" });
            }
            var dto = new UserToReturnDto() {
            Id = emp.Id,
            FirstName = emp.FirstName,
            LastName = emp.LastName,
            Email=emp.Email,
            UserName=emp.UserName,
            Roles=_userManager.GetRolesAsync(emp).Result
            };

            return View(ViewName, dto);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {

            return await Details(id, "Edit"); 
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserToReturnDto model)
        {
            if (id != model.Id)
                return BadRequest("Invalid operation");
            var user=await _userManager.FindByIdAsync(id);
            if(user is null)
            {
                return BadRequest("Invalid operation");
            }
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.UserName;
            var result=await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "User updated successfully!";
                return RedirectToAction(nameof(Index));
                
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, UserToReturnDto model)
        {
            if (id != model.Id)
                return BadRequest("Invalid operation");
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return BadRequest("Invalid operation");
            }
            
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "User deleted successfully!";
                return RedirectToAction(nameof(Index));
                
            }
            return View(model);
        }
    }
}
