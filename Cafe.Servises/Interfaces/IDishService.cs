using Cafe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe.Domain.Response;
using Cafe.Domain.ViewModel.Dish;
using Cafe.Domain.ViewModel.User;

namespace Cafe.Servises.Interfaces
{
    public interface IDishService
    {
        IBaseResponse<IEnumerable<dishs>> GetDishs();
        BaseResponse<Dictionary<int, string>> GetCategores();
        IBaseResponse<IEnumerable<dishs>> GetByCategory(int id);
        IBaseResponse<DishViewModel> GetDish(int id);
        BaseResponse<Dictionary<int, string>> GetDish(string term);
        IBaseResponse<bool> DeleteDish(int id);
        IBaseResponse<dishs> CreateDish(DishViewModel model, byte[] b);
        IBaseResponse<dishs> Edit(int id, DishViewModel model, byte[] b);
    }
}
