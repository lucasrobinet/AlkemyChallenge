using AhoraSi.Models;
using AhoraSi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AhoraSi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> rolGestion;
        private readonly UserManager<IdentityUser> userGestion;
        private readonly DataBaseContext _context;
        public RolesController(RoleManager<IdentityRole> rolGestion, UserManager<IdentityUser> userGestion, DataBaseContext context)
        {
            this.rolGestion = rolGestion;
            this.userGestion = userGestion;
            _context = context;
        }


        [HttpGet]
        [Route("Roles/Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("Roles/Create")]
        public async Task<IActionResult> Create(Roles obj)
        {
            if (ModelState.IsValid)
            {
                var identityRole = new IdentityRole
                {
                    Name = obj.RolName
                };

                IdentityResult result = await rolGestion.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("RoleList", "Roles");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(obj);
        }

        [HttpGet]
        [Route("Roles/List")]
        public IActionResult RoleList()
        {
            var roles = rolGestion.Roles;
            return View(roles);
        }

        [HttpGet]
        [Route("Roles/RoleEdit")]
        public async Task<IActionResult> RoleEdit(string id)
        {
            var rol = await rolGestion.FindByIdAsync(id);

            if (rol == null)
            {
                return NotFound();
            }

            var model = new EditRoles
            {
                Id = rol.Id,
                RolName = rol.Name
            };

            foreach (var user in userGestion.Users)
            {
                if (await userGestion.IsInRoleAsync(user, rol.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }

        [HttpPost]
        [Route("Roles/RoleEdit")]
        public async Task<IActionResult> RoleEdit(EditRoles obj)
        {
            var rol = await rolGestion.FindByIdAsync(obj.Id);

            if (rol == null)
            {
                return NotFound()
;
            }
            else
            {
                rol.Name = obj.RolName;

                var result = await rolGestion.UpdateAsync(rol);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(obj);
        }

        [HttpGet]
        [Route("Roles/EditUserRol")]
        public async Task<IActionResult> EditUserRol(string rolId)
        {
            ViewBag.rolId = rolId;

            var role = await rolGestion.FindByIdAsync(rolId);

            if (role == null)
            {
                return NotFound();
            }

            var model = new List<UserRoles>();

            foreach (var user in userGestion.Users)
            {
                var userRoles = new UserRoles
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await userGestion.IsInRoleAsync(user, role.Name))
                {
                    userRoles.IsChecked = true;
                }
                else
                {
                    userRoles.IsChecked = false;
                }

                model.Add(userRoles);
            }

            return View(model);
        }

        [HttpPost]
        [Route("Roles/EditUserRol")]
        public async Task<IActionResult> EditUserRol(List<UserRoles> model, string rolId)
        {
            var rol = await rolGestion.FindByIdAsync(rolId);

            if (rol == null)
            {
                return NotFound();
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userGestion.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsChecked && !(await userGestion.IsInRoleAsync(user, rol.Name)))
                {
                    result = await userGestion.AddToRoleAsync(user, rol.Name);
                }
                else if (!model[i].IsChecked && await userGestion.IsInRoleAsync(user, rol.Name))
                {
                    result = await userGestion.RemoveFromRoleAsync(user, rol.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("Index", "Home", new { Id = rolId });
                }
            }

            return RedirectToAction("Index", "Home", new { Id = rolId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Roles/Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var rol = await rolGestion.FindByIdAsync(id);

            if (rol == null)
            {
                ViewBag.ErrorMessage = "El rol indicado no existe";
                return View("Error");
            }
            else
            {
                var result = await rolGestion.DeleteAsync(rol);

                if (result.Succeeded)
                {
                    return RedirectToAction("RoleList");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("RoleList");
            }
        }
    }
}
