using Cafe.Dal.Interfaces;
using Cafe.Domain.Entities;
using Cafe.Domain.ViewModel.Order;
using Cafe.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cafe.Controllers
{
    public class OrderController : Controller
    {
        private IBaseRepository<users> _userRepository;
        private IOrderService _orderService;
        public OrderController(IOrderService orderService,
            IBaseRepository<users> userService)
        {

            _orderService = orderService;
            _userRepository = userService;
        }
        [HttpGet]
        public IActionResult OrderHistory()
        {
            var response = _orderService.OrderHistory(User.Identity.Name);
            if (response.StatusCode == Domain.Enums.StatusCode.OK)
            {
                return View(response.Data);
            }
            return RedirectToAction("Error", $"{response.StatusCode}");
        }
        [HttpGet]
        public IActionResult AllOrders()
        {
            var response = _orderService.AllOrders();
            if (response.StatusCode == Domain.Enums.StatusCode.OK)
            {
                return View(response.Data);
            }
            return RedirectToAction("Error", $"{response.StatusCode}");
        }
        [HttpGet]
        public IActionResult Save(int id)
        {
            if (id == 0)
            {
                var responce = _orderService.OrderDetail(User.Identity.Name).Data;
                return View(responce);
            }
            


            return RedirectToAction("Error");
        }

        [HttpPost]
        public IActionResult Save(OrderViewModel model)
        {
            var response = _orderService.CreateOrder(model, User.Identity.Name);
            return Json(new { description = response.Description, statuscode = response.StatusCode });

        }
        [HttpPost]
        public new JsonResult GetType()
        {
            var types = _orderService.GetType();
            return Json(types.Data);
        }
        public IActionResult Delete(int id)
        {
            _orderService.DeleteOrder(id, User.Identity.Name);

            return RedirectToAction("OrderHistory", "Order");
        }

        [HttpGet]
        public IActionResult EditOrder(int id)
        {
            _orderService.Edit(id);
            return RedirectToAction("AllOrders", "Order");
        }

        [HttpPost]
        public JsonResult GetStatus()
        {
            var types = _orderService.GetStatus();
            return Json(types.Data);
        }
    }
}
