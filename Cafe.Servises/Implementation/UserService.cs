using Cafe.Dal;
using Cafe.Dal.Interfaces;
using Cafe.Dal.Repositories;
using Cafe.Domain.Entities;
using Cafe.Domain.Enums;
using Cafe.Domain.Extension;
using Cafe.Domain.Helpers;
using Cafe.Domain.Response;
using Cafe.Domain.ViewModel.User;
using Cafe.Servises.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Servises.Implementation
{


    public class UserService : IUserService
    {
        private readonly IBaseRepository<users> _userRepository;
        private readonly IBaseRepository<profile> _proFileRepository;
        private readonly IBaseRepository<basket> _basketRepository;
        private readonly ApplicationDBContext _db;
        public UserService(IBaseRepository<users> userRepository, 
            IBaseRepository<profile> proFileRepository,
            IBaseRepository<basket> basketRepository, 
            ApplicationDBContext db)
        {
            _userRepository = userRepository;
            _proFileRepository = proFileRepository;
            _basketRepository = basketRepository;
            _db = db;
        }

        public IBaseResponse<users> Create(UserViewModel model)
        {
            using(_db)
            {
                using(var transact = _db.Database.BeginTransaction())
                {
                    try
                    {
                        var user = _userRepository.GetAll().FirstOrDefault(x => x.Name == model.Name);
                        if (user != null)
                        {
                            return new BaseResponse<users>()
                            {
                                Description = "Пользователь с таким логином уже есть",
                                StatusCode = StatusCode.UserAlreadyExists
                            };
                        }
                        user = new users()
                        {
                            Name = model.Name,
                            idR = Enum.Parse<Role>(model.Role),
                            Password = HashPasswordHelper.HashPassowrd(model.Password),
                            Phone = model.Phone,
                        };

                        _userRepository.Create(user);

                        var profile = new profile()
                        {
                            Address = string.Empty,
                            Age = 0,
                            userid = user.IdU,
                        };

                        _proFileRepository.Create(profile);

                        var basket = new basket()
                        {
                            userid = user.IdU,
                            DishList = new List<dishlist>(),
                            User = user,
                        };

                        _basketRepository.Create(basket);

                        transact.Commit();
                        return new BaseResponse<users>()
                        {
                            Data = user,
                            Description = "Пользователь добавлен",
                            StatusCode = StatusCode.OK
                        };
                    }
                    catch (Exception ex)
                    {
                        transact.Rollback();
                        return new BaseResponse<users>()
                        {
                            StatusCode = StatusCode.InternalServerError,
                            Description = $"Внутренняя ошибка: {ex.Message}"
                        };
                    }
                }
            }
            
        }

        public BaseResponse<Dictionary<int, string>> GetRoles()
        {
            try
            {
                var roles = ((Role[])Enum.GetValues(typeof(Role)))
                    .ToDictionary(k => (int)k, t => t.GetDisplayName());

                return new BaseResponse<Dictionary<int, string>>()
                {
                    Data = roles,
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

        public BaseResponse<IEnumerable<UserViewModel>> GetUsers()
        {
            try
            {
                var users =  _userRepository.GetAll()
                    .Select(x => new UserViewModel()
                    {
                        IdU = x.IdU,
                        Name = x.Name,
                        Role = x.idR.GetDisplayName(),
                        Phone = x.Phone
                    })
                    .ToList();

                return new BaseResponse<IEnumerable<UserViewModel>>()
                {
                    Data = users,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<UserViewModel>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        public IBaseResponse<bool> DeleteUser(long id)
        {
            using (_db)
            {
                using (var transact = _db.Database.BeginTransaction())
                {
                    try
                    {
                        var user = _userRepository.GetAll()
                            .Include(x => x.Basket)
                            .Include(x => x.OrderBasket)
                            .Include(x => x.Profile)
                            .FirstOrDefault(x => x.IdU == id);
                        if (user == null)
                        {
                            return new BaseResponse<bool>
                            {
                                StatusCode = StatusCode.NotFoundUserError,
                                Data = false
                            };
                        }
                        _userRepository.Delete(user);
                        transact.Commit();
                        return new BaseResponse<bool>
                        {
                            StatusCode = StatusCode.OK,
                            Data = true
                        };
                    }
                    catch (Exception ex)
                    {
                        transact.Rollback();
                        return new BaseResponse<bool>()
                        {
                            StatusCode = StatusCode.InternalServerError,
                            Description = $"Внутренняя ошибка: {ex.Message}"
                        };
                    }
                }
            }
                    
        }
    }
}

