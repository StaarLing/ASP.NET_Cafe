using Cafe.Dal;
using Cafe.Dal.Interfaces;
using Cafe.Dal.Repositories;
using Cafe.Domain.Entities;
using Cafe.Domain.Enums;
using Cafe.Domain.Extension;
using Cafe.Domain.Response;
using Cafe.Domain.ViewModel.Basket;
using Cafe.Domain.ViewModel.Dish;
using Cafe.Domain.ViewModel.Order;
using Cafe.Domain.ViewModel.Profile;
using Cafe.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Services.Implementation
{
    public class BasketService : IBasketService
    {
        private readonly IBaseRepository<users> _userRepository;
        private readonly IBaseRepository<dishlist> _dishlistRepository;
        private readonly IBaseRepository<dishs> _dishsRepository;
        private readonly IBaseRepository<basket> _basketRepository;
        private readonly ApplicationDBContext _db;
        public BasketService(IBaseRepository<users> userRepository,
            IBaseRepository<dishlist> dishlistRepository,
            IBaseRepository<dishs> dishsRepository,
            IBaseRepository<basket> basketRepository, 
            ApplicationDBContext db)
        {
            _userRepository = userRepository;
            _dishlistRepository = dishlistRepository;
            _dishsRepository = dishsRepository;
            _basketRepository = basketRepository;
            _db = db;
        }

        public IBaseResponse<bool> AddItem(string UserName, int id)
        {
            using (_db)
            {
                using (var transact = _db.Database.BeginTransaction())
                {
                    try
                    {
                        var user = _userRepository.GetAll()
                            .Include(x => x.Basket)
                            .ThenInclude(x => x.DishList)
                            .FirstOrDefault(x => x.Name == UserName);
                        var responce = user.Basket.DishList.Where(x => x.IdDish == id).FirstOrDefault();
                        if (responce != null)
                        {
                            if (responce.Count >= 1 && responce.Count < _dishsRepository.GetAll().Where(x => x.IdD == responce.IdDish).FirstOrDefault().Count)
                            {
                                responce.Count += 1;
                                _dishlistRepository.Update(responce);
                            }
                        }
                        else
                        {
                            if (_dishsRepository.GetAll().Where(x => x.IdD == id).FirstOrDefault().Count > 0)
                            {
                                dishlist DishList = new dishlist()
                                {
                                    IdDish = id,
                                    Bus = user.Basket.idbus,
                                    basket = user.Basket,
                                    Count = 1
                                };
                                user.Basket.DishList.Add(DishList);
                                _dishlistRepository.Create(DishList);
                            }
                        }
                        transact.Commit();
                        return new BaseResponse<bool>
                        {
                            Data = true,
                            StatusCode = StatusCode.OK
                        };
                    }
                    catch (Exception ex)
                    {
                        transact.Rollback();
                        return new BaseResponse<bool>()
                        {
                            Description = ex.Message,
                            StatusCode = StatusCode.OK
                        };
                    }
                }
            }
                    

        }

        public IBaseResponse<bool> DeleteItem(string UserName, int id)
        {
            using (_db)
            {
                using (var transact = _db.Database.BeginTransaction())
                {
                    try
                    {
                        var user = _userRepository.GetAll()
                            .Include(x => x.Basket)
                            .ThenInclude(x => x.DishList)
                            .FirstOrDefault(x => x.Name == UserName);
                        var responce = user.Basket.DishList.Where(x => x.IdDish == id).First();
                        if (responce.Count == 1)
                        {
                            _dishlistRepository.Delete(user.Basket.DishList.Where(x => x.IdDish == id).FirstOrDefault());
                        }
                        else
                        {
                            responce.Count -= 1;
                            _dishlistRepository.Update(responce);
                        }
                        transact.Commit();
                        return new BaseResponse<bool>
                        {
                            Data = true,
                            StatusCode = StatusCode.OK
                        };
                    }
                    catch (Exception ex)
                    {
                        transact.Rollback();
                        return new BaseResponse<bool>()
                        {
                            Description = ex.Message,
                            StatusCode = StatusCode.OK
                        };
                    }
                }
            }
                    

        }

        public IBaseResponse<BasketViewModel> GetBasketItem(string UserName)
        {
            try
            {
                var user = _userRepository.GetAll()
                    .Include(x => x.Basket)
                    .ThenInclude(x => x.DishList)
                    .FirstOrDefault(x => x.Name == UserName);

                if (user == null)
                {
                    return new BaseResponse<BasketViewModel>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.NotFoundUserError
                    };
                }
                var Dishs = from p in user.Basket.DishList
                            join c in _dishsRepository.GetAll() on p.IdDish equals c.IdD
                            select c;

                var Items = from p in Dishs
                            select new ItemBasket()
                            {
                                Dish = p,
                                Count = user.Basket.DishList.Where(x => x.IdDish == p.IdD).First().Count,
                            };
                foreach (var item in Items)
                {
                    if (item.Count > _dishsRepository.GetAll().Where(x => x.IdD == item.Dish.IdD).FirstOrDefault().Count)
                    {
                        user.Basket.DishList.Where(x => x.IdDish == item.Dish.IdD).First().Count = _dishsRepository.GetAll().Where(x => x.IdD == item.Dish.IdD).FirstOrDefault().Count;
                        _dishlistRepository.Update(user.Basket.DishList.Where(x => x.IdDish == item.Dish.IdD).First());
                    }
                }
                foreach (var item in Items)
                {
                    if (item.Count == 0)
                    {
                        _dishlistRepository.Delete(user.Basket.DishList.Where(x => x.IdDish == item.Dish.IdD).First());
                    }
                }
                var Summ = Items.ToArray().Sum(x => x.Dish.Price * x.Count);
                var SummSell = Items.ToArray().Sum(x => (x.Dish.Price * x.Count) - ((x.Dish.Price * x.Count) * x.Dish.Sell / 100));
                var response = new BasketViewModel()
                {
                    Items = Items.ToList(),
                    TotalPrice = Summ,
                    TotalPriceWithSell = decimal.Round(SummSell, 2),
                };
                return new BaseResponse<BasketViewModel>()
                {
                    Data = response,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<BasketViewModel>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.OK
                };
            }
        }
    }
}