using Cafe.Domain.ViewModel.Dish;
using Cafe.Servises.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
namespace Cafe.Controllers
{
    public class DishsController : Controller
    {
        private readonly IDishService _dishServise;

        public DishsController(IDishService dishServise)
        {
            _dishServise = dishServise;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var response = _dishServise.GetDishs();
            if (response.StatusCode == Domain.Enums.StatusCode.OK)
            {
                return View(response.Data);
            }
            return RedirectToAction("Error", $"{response.StatusCode}");
        }

        [HttpGet]
        public ActionResult GetDish(int id, bool isJson)
        {
            var response = _dishServise.GetDish(id);
            if (isJson)
            {
                return Json(response.Data);
            }
            return PartialView("GetDish", response.Data);
        }

        [HttpPost]
        public IActionResult GetDish(string term)
        {
            var response = _dishServise.GetDish(term);
            return Json(response.Data);
        }

        [HttpGet]
        public IActionResult GetByCategory(int id)
        {
            var response = _dishServise.GetByCategory(id);
            if (response.StatusCode == Domain.Enums.StatusCode.OK)
            {
                return View(response.Data);
            }
            return RedirectToAction("Error");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int idD)
        {
            var response = _dishServise.DeleteDish(idD);
            if (response.StatusCode == Domain.Enums.StatusCode.OK)
            {
                return RedirectToAction("GetAll");
            }
            return RedirectToAction("Error");
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Save(int idD)
        {
            if (idD == 0)
            {
                DishViewModel d = new DishViewModel();
                return View(d);
            }
            var response = _dishServise.GetDish(idD);
            if (response.StatusCode == Domain.Enums.StatusCode.OK)
            {
                return View(response.Data);
            }

            return RedirectToAction("Error");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Save(DishViewModel model)
        {
            if (model.IdD == 0)
            {
                byte[] bytes;
                using (var binar = new BinaryReader(model.Avatar.OpenReadStream()))
                {
                    bytes = binar.ReadBytes((int)model.Avatar.Length);
                }
                _dishServise.CreateDish(model, bytes);
            }
            else
            {
                byte[] bytes;
                using (var binar = new BinaryReader(model.Avatar.OpenReadStream()))
                {
                    bytes = binar.ReadBytes((int)model.Avatar.Length);
                }
                _dishServise.Edit(model.IdD, model, bytes);
            }

            return RedirectToAction("GetAll");
        }

        [HttpPost]
        public JsonResult GetCategores()
        {
            var types = _dishServise.GetCategores();
            return Json(types.Data);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckPositiv(decimal price)
        {
            if (price >=0)
                return Json(true);
            return Json(false);
        }
    }
}
