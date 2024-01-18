using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Dal.Interfaces
{
    public interface IBaseRepository<T>
    {
        IQueryable<T> GetAll();
        bool Create(T entity);
        bool Delete(T entity);
        T Update(T entity);
    }
}
