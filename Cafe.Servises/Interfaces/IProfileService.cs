using Cafe.Domain.Entities;
using Cafe.Domain.Response;
using Cafe.Domain.ViewModel.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Services.Interfaces
{
    public interface IProfileService
    {
        BaseResponse<ProfileViewModel> GetProfile(string userName);

        BaseResponse<profile> Save(ProfileViewModel model);
    }
}
