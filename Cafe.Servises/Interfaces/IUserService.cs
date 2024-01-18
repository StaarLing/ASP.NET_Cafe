using Cafe.Domain.Entities;
using Cafe.Domain.Response;
using Cafe.Domain.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Servises.Interfaces
{
    public interface IUserService
    {
        IBaseResponse<users> Create(UserViewModel model);

        BaseResponse<Dictionary<int, string>> GetRoles();

        BaseResponse<IEnumerable<UserViewModel>> GetUsers();

        IBaseResponse<bool> DeleteUser(long id);
    }
}
