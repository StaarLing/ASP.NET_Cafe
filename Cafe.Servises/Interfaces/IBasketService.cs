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
    public interface IBasketService
    {
        IBaseResponse<BasketViewModel> GetBasketItem(string UserName);
        IBaseResponse<bool> DeleteItem(string UserName, int id);
        IBaseResponse<bool> AddItem(string UserName, int id);
    }
}
