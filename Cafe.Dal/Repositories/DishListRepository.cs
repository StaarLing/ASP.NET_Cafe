using Cafe.Dal.Interfaces;
using Cafe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Dal.Repositories
{
    public class DishListRepository : IBaseRepository<dishlist>
    {
        private readonly ApplicationDBContext _db;

        public DishListRepository(ApplicationDBContext dbContext)
        {
            _db = dbContext;
        }

        public bool Create(dishlist entity)
        {
            _db.dishlist.Add(entity);
            _db.SaveChanges();
            return true;
        }

        public IQueryable<dishlist> GetAll()
        {
            return _db.dishlist;
        }

        public bool Delete(dishlist entity)
        {
            _db.dishlist.Remove(entity);
            _db.SaveChanges();
            return true;
        }

        public dishlist Update(dishlist entity)
        {
            _db.dishlist.Update(entity);
            _db.SaveChanges();

            return entity;
        }

    }
}

