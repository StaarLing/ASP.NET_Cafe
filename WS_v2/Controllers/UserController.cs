using Cafe.Domain.Extension;
using Cafe.Domain.ViewModel.Dish;
using Cafe.Domain.ViewModel.User;
using Cafe.Servises.Implementation;
using Cafe.Servises.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cafe.Domain.Enums;
using Cafe.Domain.Entities;

namespace Cafe.Controllers
{
        public class UserController : Controller
        {
            private readonly IUserService _userService;

            public UserController(IUserService userService)
            {
                _userService = userService;
            }

        public IActionResult GetUsers()
        {
            var response = _userService.GetUsers();
            if (response.StatusCode == Domain.Enums.StatusCode.OK)
            {
                return View(response.Data);
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult DeleteUser(int id)
        {
            var response = _userService.DeleteUser(id);
            if (response.StatusCode == Domain.Enums.StatusCode.OK)
            {
                return RedirectToAction("GetUsers");
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Save() => PartialView(new UserViewModel());

        [HttpPost]
        public IActionResult Save(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = _userService.Create(model);
                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {
                    return RedirectToAction("GetUsers");
                }
                return BadRequest(new { errorMessage = response.Description });
            }
            var errorMessage = ModelState.Values
                .SelectMany(v => v.Errors.Select(x => x.ErrorMessage)).ToList().Join();
            return StatusCode(StatusCodes.Status500InternalServerError, new { errorMessage });
        }

        [HttpPost]
            public JsonResult GetRoles()
            {
                var types = _userService.GetRoles();
                return Json(types.Data);
            }
        }
    }
