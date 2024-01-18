using Cafe.Dal;
using Cafe.Dal.Interfaces;
using Cafe.Domain.Entities;
using Cafe.Domain.Enums;
using Cafe.Domain.Extension;
using Cafe.Domain.Response;
using Cafe.Domain.ViewModel.Dish;
using Cafe.Servises.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Servises.Implementation
{
    public class DishService : IDishService
    {
        private readonly IBaseRepository<dishs> _dishRepository;
        private readonly ApplicationDBContext _db;

        public DishService(IBaseRepository<dishs> dishRepository,ApplicationDBContext db)
        {
            _dishRepository = dishRepository;
            _db = db;
        }
        public IBaseResponse<dishs> CreateDish(DishViewModel dishViewModel, byte[] b)
        {
            using(_db)
            {
                using (var transact = _db.Database.BeginTransaction())
                {
                    try
                    {
                        var dish = new dishs()
                        {
                            Name = dishViewModel.Name,
                            Description = dishViewModel.Description,
                            Price = dishViewModel.Price,
                            Sell = dishViewModel.Sell,
                            IdK = (Category)Enum.Parse(typeof(Category), dishViewModel.IdK),
                            Avatar = b,
                            Count = dishViewModel.Count,
                        };
                        _dishRepository.Create(dish);
                        transact.Commit();
                        return new BaseResponse<dishs>
                        {
                            Data = dish,
                            StatusCode = StatusCode.OK
                        };
                    }
                    catch (Exception ex)
                    {
                        transact.Rollback();
                        return new BaseResponse<dishs>()
                        {
                            Description = $"CreateDish : {ex.Message}",
                            StatusCode = StatusCode.InternalServerError
                        };
                    }
                }
            }
            
            
        }
        public IBaseResponse<bool> DeleteDish(int id)
        {
            using (_db)
            {
                using (var transact = _db.Database.BeginTransaction())
                {
                    try
                    {
                        var dish = _dishRepository.GetAll().FirstOrDefault(i => i.IdD == id);
                        if (dish == null)
                        {
                            return new BaseResponse<bool>()
                            {
                                Description = "Обьект не найден",
                                StatusCode = StatusCode.NotFoundDishError
                            };
                        }

                        _dishRepository.Delete(dish);
                        transact.Commit();
                        return new BaseResponse<bool>()
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
                            Description = $"DeleteDish : {ex.Message}",
                            StatusCode = StatusCode.InternalServerError
                        };
                    }
                }
            }
                    
        }

        public IBaseResponse<dishs> Edit(int id, DishViewModel model, byte[] b)
        {
            try
            {
                var dish = _dishRepository.GetAll().FirstOrDefault(i => i.IdD == id);
                if (dish == null)
                {
                    return new BaseResponse<dishs>()
                    {
                        Description = "Error",
                        StatusCode = StatusCode.InternalServerError
                    };
                }

                dish.Description = model.Description;
                dish.Price = model.Price;
                dish.Name = model.Name;
                dish.Sell = model.Sell;
                dish.IdK = (Category) Enum.Parse(typeof( Category),model.IdK);
                dish.Avatar = b;
                dish.Count = model.Count;
                _dishRepository.Update(dish);

                return new BaseResponse<dishs>()
                {
                    Data = dish,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<dishs>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<IEnumerable<dishs>> GetByCategory(int id)
        {
            try
            {
                var dish = _dishRepository.GetAll().Where(k => k.IdK == (Category)id).ToList();
                if (dish.Count() == 0)
                {
                    return new BaseResponse<IEnumerable<dishs>>()
                    {
                        Description = "Найдено 0 элементов",
                        StatusCode = StatusCode.OK
                    };
                }
                return new BaseResponse<IEnumerable<dishs>>() 
                { 
                    Data = dish, 
                    StatusCode = StatusCode.OK 
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<dishs>>()
                {
                    Description = $"[GetDishByCategory] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public BaseResponse<Dictionary<int, string>> GetCategores()
        {
            try
            {
                var types = ((Category[])Enum.GetValues(typeof(Category)))
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

        public IBaseResponse<DishViewModel> GetDish(int id)
        {
            try
            {
                var dish = _dishRepository.GetAll().FirstOrDefault(i => i.IdD == id);
                if (dish == null) 
                {
                   return new BaseResponse<DishViewModel>()
                    {
                        Description = "Не найдено",
                        StatusCode = StatusCode.OK
                    };
                }
                var data = new DishViewModel()
                {
                    IdD= dish.IdD,
                    Name = dish.Name,
                    Description = dish.Description,
                    Price = dish.Price,
                    Sell= dish.Sell,
                    Image = dish.Avatar,
                    IdK = dish.IdK.GetDisplayName(),
                    Count = dish.Count,
                };
                return new BaseResponse<DishViewModel>()
                {
                    Data = data,
                    StatusCode = StatusCode.OK
                };
            }
            catch(Exception ex)
            {
                return new BaseResponse<DishViewModel>()
                {
                    Description = $"GetDish : {ex.Message}",
                    StatusCode = StatusCode.NotFoundDishError
                };
            }
        }
        public BaseResponse<Dictionary<int, string>> GetDish(string term)
        {
            var baseResponse = new BaseResponse<Dictionary<int, string>>();
            try
            {
                var dish = _dishRepository.GetAll()
                    .Select(x => new DishViewModel()
                    {
                        IdD = x.IdD,
                        Name = x.Name,
                        Description = x.Description,
                        Price = x.Price,
                        Sell = x.Sell,
                        Image = x.Avatar,
                        IdK = x.IdK.GetDisplayName(),
                        Count = x.Count,
                    })
                    .Where(x => EF.Functions.Like(x.Name, $"%{term}%"))
                    .ToDictionary(x => x.IdD, t => t.Name);

                baseResponse.Data = dish;
                return baseResponse;
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
        public IBaseResponse<IEnumerable<dishs>> GetDishs()
        {
            try
            {
                var dish = _dishRepository.GetAll().ToList();
                if (dish.Count() == 0) 
                {
                    new BaseResponse<IEnumerable<dishs>>()
                    {
                        Description = "Найдено 0 элементов",
                        StatusCode = StatusCode.OK
                    };
                }
                return new BaseResponse<IEnumerable<dishs>>()
                {
                    Data = dish,
                    StatusCode = StatusCode.OK
                };
            }
            catch(Exception ex)
            {
                return new BaseResponse<IEnumerable<dishs>>()
                {
                    Description = $"[GetDish] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
