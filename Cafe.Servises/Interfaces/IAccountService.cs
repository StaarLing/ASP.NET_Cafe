using Cafe.Domain.ViewModel.Account;
using Cafe.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Servises.Interfaces
{
    public interface IAccountService
    {
        BaseResponse<ClaimsIdentity> Register(RegisterViewModel model);

        BaseResponse<ClaimsIdentity> Login(LoginViewModel model);

        BaseResponse<bool> ChangePassword(ChangePasswordViewModel model);
    }
}
