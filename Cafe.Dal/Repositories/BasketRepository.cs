using Cafe.Dal.Interfaces;
using Cafe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Dal.Repositories
{
    public class BasketRepository : IBaseRepository<basket>
    {
        private readonly ApplicationDBContext _db;

        public BasketRepository(ApplicationDBContext dbContext)
        {
            _db = dbContext;
        }

        public bool Create(basket entity)
        {
            _db.basket.Add(entity);
            _db.SaveChanges();
            return true;
        }

        public IQueryable<basket> GetAll()
        {
            return _db.basket;
        }

        public bool Delete(basket entity)
        {
            _db.basket.Remove(entity);
            _db.SaveChanges();
            return true;
        }

        public basket Update(basket entity)
        {
            _db.basket.Update(entity);
            _db.SaveChanges();

            return entity;
        }

    }
}
