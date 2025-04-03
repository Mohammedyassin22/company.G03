using company.G03.DAL.Models;
using company.G03.PL.Helper;
using company.G03.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace company.G03.PL.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly UserManager<AppUsers> _userManager;
        public RoleController(RoleManager<IdentityRole> rolemanager, UserManager<AppUsers> userManager)
        {
            _rolemanager = rolemanager;
            _userManager = userManager;
        }
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<RoleToReturnDto> roles;
            if (string.IsNullOrEmpty(SearchInput))
            {
                roles = _rolemanager.Roles.Select(x => new RoleToReturnDto
                {
                    id = x.Id,
                    name = x.Name,
                });
            }
            else
            {
                roles = _rolemanager.Roles.Select(x => new RoleToReturnDto
                {
                    id = x.Id,
                    name = x.Name,
                }).Where(x => x.name.ToLower().Contains(SearchInput.ToLower()));
            }
            return View(roles);
        }


        [HttpGet]
        public async Task<IActionResult> Details(string? id, string ViewName)
        {
            if (id is null)
            {
                return BadRequest("is valid");
            }
            var role = await _rolemanager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound(new { Message = "Users with id is not found" });
            }
            var dto = new RoleToReturnDto()
            {
                id = role.Id,
                name = role.Name,
            };

            return View(ViewName, dto);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {

            return await Details(id, "Edit");
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleToReturnDto dto)
        {
            if (ModelState.IsValid)
            {
                var role = await _rolemanager.FindByNameAsync(dto.name);
                if (role is null)
                {
                    role = new IdentityRole()
                    {
                        Name = dto.name
                    };
                    var result = await _rolemanager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        TempData["SuccessMessage"] = "User created successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return View(dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleToReturnDto model)
        {
            if (id != model.id)
                return BadRequest("Invalid operation");
            var role = await _rolemanager.FindByIdAsync(id);
            if (role is null)
            {
                return BadRequest("Invalid operation");
            }
            var roleresult = await _rolemanager.FindByNameAsync(model.name);
            if (roleresult is null)
            {
                role.Name = model.name;

                var result = await _rolemanager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "User updated successfully!";
                    return RedirectToAction(nameof(Index));

                }
            }
            ModelState.AddModelError("", "Invalid Operation");
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleToReturnDto model)
        {
            if (id != model.id)
                return BadRequest("Invalid operation");
            var role = await _rolemanager.FindByIdAsync(id);
            if (role is null)
            {
                return BadRequest("Invalid operation");
            }
            var result = await _rolemanager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "User Deleted successfully!";
                return RedirectToAction(nameof(Index));

            }

            ModelState.AddModelError("", "Invalid Operation");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUser(string roleID)
        {
            ViewBag.RoleId = roleID;
            var role = await _rolemanager.FindByIdAsync(roleID);
            if (role is null)
            {
                TempData["ErrorMessage"] = "Not Found";
                return RedirectToAction(nameof(Edit));
            }
            var userinrole = new List<AddInRemoveDto>();
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var userIdrole = new AddInRemoveDto()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    email = user.Email
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userIdrole.IsSelected = true;
                }
                else
                {
                    userIdrole.IsSelected = false;
                }
                userinrole.Add(userIdrole);
            }
            return View(userinrole);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUser(string roleID, List<string> selectedUsers)
        {
            var role = await _rolemanager.FindByIdAsync(roleID);
            if (role is null)
            {
                TempData["ErrorMessage"] = "Role Not Found";
                return RedirectToAction(nameof(Index));
            }

            // Get all users in this role
            var currentUsersInRole = await _userManager.GetUsersInRoleAsync(role.Name);

            // Process all application users
            var allUsers = await _userManager.Users.ToListAsync();
            foreach (var user in allUsers)
            {
                bool shouldBeInRole = selectedUsers.Contains(user.Id);
                bool isCurrentlyInRole = currentUsersInRole.Any(u => u.Id == user.Id);

                if (shouldBeInRole && !isCurrentlyInRole)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!shouldBeInRole && isCurrentlyInRole)
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
            }

            TempData["SuccessMessage"] = "User roles successfully";
            return RedirectToAction(nameof(Edit), new { id = roleID });
        }
    }
}
