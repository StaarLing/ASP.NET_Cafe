using Cafe.Domain.Entities;
using Cafe.Domain.ViewModel.Dish;
using Cafe.Servises.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using Cafe.Domain.Enums;
using System;
using Cafe.Dal.Interfaces;
using Cafe.Servises.Implementation;
using Cafe.Services.Interfaces;

namespace Cafe.Controllers
{
    public class BasketController : Controller
    {
        private IBaseRepository<users> _userRepository;
        private IBasketService _basketService;
        public BasketController(IBasketService basketService, 
            IBaseRepository<users> userService)
        {
            
            _basketService = basketService;
            _userRepository= userService;
        }

        [HttpGet]
        public IActionResult GetBasketItem()
        {
            var UserName = User.Identity.Name;
            var user = _userRepository.GetAll().Where(x => x.Name == UserName).FirstOrDefault();
            var response = _basketService.GetBasketItem(UserName);
            if (response.StatusCode == Domain.Enums.StatusCode.OK)
            {
                return View(response.Data);
            }
            return RedirectToAction("Error", $"{response.StatusCode}");
        }
        public IActionResult AddItem(int id)
        {
            var UserName = User.Identity.Name;
            var user = _userRepository.GetAll().Where(x => x.Name == UserName).FirstOrDefault();
            var response = _basketService.AddItem(UserName, id);

            if (response.StatusCode == Domain.Enums.StatusCode.OK)
            {
                return RedirectToAction("GetBasketItem");
            }
            return RedirectToAction("Error", $"{response.StatusCode}");
        }
        public IActionResult DeleteItem(int id)
        {
            var UserName = User.Identity.Name;
            var user = _userRepository.GetAll().Where(x => x.Name == UserName).FirstOrDefault();
            var response = _basketService.DeleteItem(UserName, id);

            if (response.StatusCode == Domain.Enums.StatusCode.OK)
            {
                return RedirectToAction("GetBasketItem");
            }
            return RedirectToAction("Error", $"{response.StatusCode}");
        }
    }
}