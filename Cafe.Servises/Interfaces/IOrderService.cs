using Cafe.Domain.Entities;
using Cafe.Domain.Response;
using Cafe.Domain.ViewModel.Basket;
using Cafe.Domain.ViewModel.Dish;
using Cafe.Domain.ViewModel.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Services.Interfaces
{
    public interface IOrderService
    {
        IBaseResponse<bool> CreateOrder(OrderViewModel createOrderView, string UserName);
        IBaseResponse<IEnumerable<OrderViewModel>> OrderHistory(string UserName);
        IBaseResponse<OrderViewModel> OrderDetail(string UserName);
        IBaseResponse<IEnumerable<OrderViewModel>> AllOrders();
        IBaseResponse<bool> Edit(int id);
        BaseResponse<Dictionary<int, string>> GetType();
        IBaseResponse<bool> DeleteOrder(int id, string UserName);
        BaseResponse<Dictionary<int, string>> GetStatus();
    }
}
