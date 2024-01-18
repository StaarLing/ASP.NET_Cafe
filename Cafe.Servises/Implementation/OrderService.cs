using Cafe.Dal.Interfaces;
using Cafe.Dal.Repositories;
using Cafe.Domain.Entities;
using Cafe.Domain.Enums;
using Cafe.Domain.Extension;
using Cafe.Domain.Response;
using Cafe.Domain.ViewModel.Basket;
using Cafe.Domain.ViewModel.Dish;
using Cafe.Domain.ViewModel.Order;
using Cafe.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IBaseRepository<order> _orderRepository;
        private readonly IBaseRepository<orderitem> _orderitemRepository;
        private readonly IBaseRepository<users> _usersRepository;
        private readonly IBaseRepository<dishs> _dishsRepository;
        private readonly IBaseRepository<basket> _basketRepository;
        public OrderService(IBaseRepository<order> orderRepository,
            IBaseRepository<users> usersRepository,
            IBaseRepository<dishs> dishsRepository,
            IBaseRepository<orderitem> orderitemRepository,
            IBaseRepository<basket> basketRepository)
        {
            _orderitemRepository = orderitemRepository;
            _orderRepository = orderRepository;
            _usersRepository = usersRepository;
            _dishsRepository = dishsRepository;
            _basketRepository = basketRepository;
        }
        public IBaseResponse<IEnumerable<OrderViewModel>> OrderHistory(string UserName)
        {
            try
            {
                var user = _usersRepository.GetAll()
                    .Include(x => x.Profile)
                    .Include(x => x.OrderBasket)
                    .ThenInclude(x => x.orderlists)
                    .ThenInclude(x => x.OrderItems)
                    .FirstOrDefault(x => x.Name == UserName);
                if (user == null)
                    return new BaseResponse<IEnumerable<OrderViewModel>>()
                    {
                        StatusCode = Domain.Enums.StatusCode.NotFoundUserError,
                        Description = "Пользователь не найден",
                    };
                    var responce = from p in user.OrderBasket.orderlists
                                   select new OrderViewModel()
                                   {
                                       id = p.idorder,
                                       Address = p.Address,
                                       DateCreate = p.OrderDate,
                                       UserName = p.UserName,
                                       Phone = p.Phone,
                                       Dishs = (from s in p.OrderItems
                                                select new OrderItemViewModel()
                                                {
                                                    Dishs = _dishsRepository.GetAll().Where(x => x.IdD == s.DishId).FirstOrDefault(),
                                                    Count = s.Count
                                                }).ToList(),
                                       Price = p.TotalPrice,
                                       Status = p.StatusId.GetDisplayName(),
                                       Type = p.TypeId.GetDisplayName(),
                                   };
                return new BaseResponse<IEnumerable<OrderViewModel>>()
                {
                    Data = responce,
                    StatusCode = StatusCode.OK,
                };
            }
            catch(Exception ex)
            {
                return new BaseResponse<IEnumerable<OrderViewModel>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        public IBaseResponse<bool> CreateOrder(OrderViewModel createOrderView, string UserName)
        {
            try
            {
                var User = _usersRepository.GetAll()
                    .Include(x => x.OrderBasket)
                    .Include(x=> x.Basket)
                    .ThenInclude(x=> x.DishList)
                    .FirstOrDefault(x => x.Name == UserName);

                var OrderItems = (from p in User.Basket.DishList
                                 select new orderitem()
                                 {
                                     DishId = p.IdDish,
                                     Count= p.Count,
                                 }).ToList();
                foreach(var item in OrderItems)
                {
                    if (item.Count > _dishsRepository.GetAll().Where(x=>x.IdD==item.DishId).FirstOrDefault().Count)
                    {
                        return new BaseResponse<bool>()
                        {
                            Data = false,
                            StatusCode = StatusCode.OK,
                            Description = "Не удалось сделать заказ, одно из блюд кончилось :("
                        };
                    }
                }
                var Dishs = from p in User.Basket.DishList
                            join c in _dishsRepository.GetAll() on p.IdDish equals c.IdD
                            select c;

                var Items = from p in Dishs
                            select new ItemBasket()
                            {
                                Dish = p,
                                Count = User.Basket.DishList.Where(x => x.IdDish == p.IdD).First().Count,
                            };
                var SummSell = Items.ToArray().Sum(x => (x.Dish.Price * x.Count) - ((x.Dish.Price * x.Count) * x.Dish.Sell / 100));
                var responce = new order()
                {
                    Address = createOrderView.Address,
                    OrderDate = DateTime.Now,
                    OrderItems = OrderItems,
                    StatusId = (StatusOrd)Enum.Parse(typeof(StatusOrd), "0"),
                    orderbasketid = User.OrderBasket.IdOrderBas,
                    Phone = createOrderView.Phone,
                    TotalPrice = SummSell,
                    TypeId = (TypeOrderEnum)Enum.Parse(typeof(TypeOrderEnum), createOrderView.Type),
                    UserName= createOrderView.UserName,
                    Orderbasket = User.OrderBasket
                };
                _orderRepository.Create(responce);
                var Dishss = Dishs.ToList();
                var Itemss = Items.ToList();
                for (int i = 0; i<Dishs.Count(); i++)
                {
                    Dishss[i].Count -= Itemss[i].Count;
                    _dishsRepository.Update(Dishss[i]);
                }
                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK,
                    Description = "Заказ успешно создан!"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        public IBaseResponse<OrderViewModel> OrderDetail(string UserName)
        {
            try
            {
                var user = _usersRepository.GetAll().Where(x => x.Name == UserName)
                .Include(x => x.Profile)
                .FirstOrDefault();

                var responce = new OrderViewModel()
                {
                    Address = user.Profile.Address,
                    UserName= UserName,
                    Phone = user.Phone
                };
                return new BaseResponse<OrderViewModel>()
                {
                    Data = responce,
                    StatusCode = StatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<OrderViewModel>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        public IBaseResponse<IEnumerable<OrderViewModel>> AllOrders()
        {
            try
            {
                var Users = _usersRepository.GetAll().ToList();
                var Orders = _orderRepository.GetAll()
                    .Include(x=> x.OrderItems)
                    .ToList();
                var Items = _orderitemRepository.GetAll();
                var responce = from p in Orders
                               select new OrderViewModel()
                               {
                                   id = p.idorder,
                                   Address = p.Address,
                                   DateCreate = p.OrderDate,
                                   UserName = p.UserName,
                                   Phone = p.Phone,
                                   Dishs = (from pi in p.OrderItems
                                           select new OrderItemViewModel()
                                           {
                                               Dishs = _dishsRepository.GetAll().Where(x => x.IdD == pi.DishId).FirstOrDefault(),
                                               Count = pi.Count
                                           }).ToList(),
                                   Price = p.TotalPrice,
                                   Status = p.StatusId.GetDisplayName(),
                                   Type = p.TypeId.GetDisplayName(),
                               };
                return new BaseResponse<IEnumerable<OrderViewModel>>()
                {
                    Data = responce,
                    StatusCode = StatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<OrderViewModel>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        public IBaseResponse<bool> Edit(int id)
        {
            try
            {
                var responce = _orderRepository.GetAll().Where(x => x.idorder == id).FirstOrDefault();

                var i = (StatusOrd)Enum.GetValues(typeof(StatusOrd)).GetValue((((int)responce.StatusId)+1)%7);
                responce.StatusId = i;
                _orderRepository.Update(responce);
                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message
                };
            }
        }
        public BaseResponse<Dictionary<int, string>> GetType()
        {
            try
            {
                var types = ((TypeOrderEnum[])Enum.GetValues(typeof(TypeOrderEnum)))
                    .ToDictionary(k => (int)k, t => t.GetDisplayName());

                return new BaseResponse<Dictionary<int, string>>()
                {
                    Data = types,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Dictionary<int, string>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<bool> DeleteOrder(int id, string UserName)
        {
            try
            {
                var responce = _orderRepository.GetAll().Where(x => x.idorder == id)
                    .Include(x => x.OrderItems)
                    .FirstOrDefault()
                    ;
                for(int i = responce.OrderItems.Count; i > 0; i--)
                {
                    _orderitemRepository.Delete(responce.OrderItems[i-1]);
                }
                _orderRepository.Delete(responce);


                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Data = true
                };
            }
            catch(Exception ex)
            {
                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description= ex.Message
                };
            }
            
        }
        public BaseResponse<Dictionary<int, string>> GetStatus()
        {
            try
            {
                var types = ((StatusOrd[])Enum.GetValues(typeof(StatusOrd)))
                    .ToDictionary(k => (int)k, t => t.GetDisplayName());

                return new BaseResponse<Dictionary<int, string>>()
                {
                    Data = types,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Dictionary<int, string>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
