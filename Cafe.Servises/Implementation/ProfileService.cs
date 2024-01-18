using Cafe.Dal;
using Cafe.Dal.Interfaces;
using Cafe.Domain.Entities;
using Cafe.Domain.Enums;
using Cafe.Domain.Response;
using Cafe.Domain.ViewModel.Profile;
using Cafe.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Services.Implementation
{
    public class ProfileService : IProfileService
    {
        private readonly IBaseRepository<profile> _profileRepository;
        private readonly ApplicationDBContext _db;
        public ProfileService(IBaseRepository<profile> profileRepository, ApplicationDBContext db)
        {
            _profileRepository = profileRepository;
            _db= db;
        }

        public BaseResponse<ProfileViewModel> GetProfile(string userName)
        {
            using (_db)
            {
                using(var transact = _db.Database.BeginTransaction())
                {
                    try
                    {
                        var profile = _profileRepository.GetAll()
                            .Select(x => new ProfileViewModel()
                            {
                                IdP = x.IdP,
                                Address = x.Address,
                                Age = x.Age,
                                UserName = x.User.Name
                            })
                            .FirstOrDefault(x => x.UserName == userName);

                        transact.Commit();
                        return new BaseResponse<ProfileViewModel>()
                        {
                            Data = profile,
                            StatusCode = StatusCode.OK
                        };
                    }
                    catch (Exception ex)
                    {
                        transact.Rollback();
                        return new BaseResponse<ProfileViewModel>()
                        {
                            StatusCode = StatusCode.InternalServerError,
                            Description = $"Внутренняя ошибка: {ex.Message}"
                        };
                    }
                }
            }
                
        }

        public BaseResponse<profile> Save(ProfileViewModel model)
        {
            try
            {
                var profile = _profileRepository.GetAll()
                    .FirstOrDefault(x => x.IdP == model.IdP);

                profile.Address = model.Address;
                profile.Age = model.Age;

                _profileRepository.Update(profile);

                return new BaseResponse<profile>()
                {
                    Data = profile,
                    Description = "Данные обновлены",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<profile>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }
    }
}
