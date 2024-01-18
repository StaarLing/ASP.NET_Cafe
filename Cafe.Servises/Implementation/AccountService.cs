using System;
using System.Collections.Generic;
using System.Security.Claims;
using Cafe.Dal.Interfaces;
using Cafe.Domain.Helpers;
using Cafe.Domain.Response;
using Cafe.Domain.ViewModel.Account;
using Cafe.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Cafe.Servises.Interfaces;
using Cafe.Domain.Entities;
using System.Linq;
using Cafe.Dal;
using Microsoft.Extensions.Options;

namespace Cafe.Service.Implementations
{
    public class AccountService : IAccountService
    {

        private readonly IBaseRepository<users> _userRepository;
        private readonly IBaseRepository<profile> _proFileRepository;
        private readonly IBaseRepository<basket> _basketRepository;
        private readonly IBaseRepository<orderbasket> _orderbasketRepository;
        private readonly ApplicationDBContext _db;
        public AccountService(IBaseRepository<users> userRepository,
            IBaseRepository<profile> proFileRepository,
            IBaseRepository<basket> basketRepository, IBaseRepository<orderbasket> orderbacketRepository, ApplicationDBContext db)
        {
            _userRepository = userRepository;
            _proFileRepository = proFileRepository;
            _basketRepository = basketRepository;
            _orderbasketRepository = orderbacketRepository;
            _db = db;
        }

        public BaseResponse<ClaimsIdentity> Register(RegisterViewModel model)
        {
            using (_db)
            {
                using(var transact = _db.Database.BeginTransaction())
                {
                    try
                    {
                        var user = _userRepository.GetAll().FirstOrDefault(x => x.Phone == model.Phone);
                        if (user != null)
                        {
                            return new BaseResponse<ClaimsIdentity>()
                            {
                                Description = "Номер телефона уже используется",
                            };
                        }

                        user = new users()
                        {
                            Name = model.Name,
                            Phone = model.Phone,
                            idR = Role.User,
                            Password = HashPasswordHelper.HashPassowrd(model.Password),
                        };
                        _userRepository.Create(user);

                        var profile = new profile()
                        {
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
                        var OrderBasket = new orderbasket()
                        {
                            UserId = user.IdU,
                            orderlists = new List<order>(),
                            User = user,
                        };
                        _orderbasketRepository.Create(OrderBasket);

                        var result = Authenticate(user);

                        transact.Commit();

                        return new BaseResponse<ClaimsIdentity>()
                        {
                            Data = result,
                            Description = "Объект добавился",
                            StatusCode = StatusCode.OK
                        };
                    }
                    catch (Exception ex)
                    {
                        transact.Rollback();
                        return new BaseResponse<ClaimsIdentity>()
                        {
                            Description = ex.Message,
                            StatusCode = StatusCode.InternalServerError
                        };
                        
                    }
                }
            }
                
        }

        public BaseResponse<ClaimsIdentity> Login(LoginViewModel model)
        {
            try
            {
                var user = _userRepository.GetAll().FirstOrDefault(x => x.Phone == model.Phone);
                if (user == null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Пользователь не найден"
                    };
                }

                if (user.Password != HashPasswordHelper.HashPassowrd(model.Password))
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Неверный пароль или логин"
                    };
                }
                var result = Authenticate(user);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {

                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public BaseResponse<bool> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                var user = _userRepository.GetAll().FirstOrDefault(x => x.Name == model.UserName);
                if (user == null)
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = StatusCode.NotFoundUserError,
                        Description = "Пользователь не найден"
                    };
                }

                user.Password = HashPasswordHelper.HashPassowrd(model.NewPassword);
                _userRepository.Update(user);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK,
                    Description = "Пароль обновлен"
                };

            }
            catch (Exception ex)
            {

                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        private ClaimsIdentity Authenticate(users user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.idR.ToString())
            };
            return new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
    }
}