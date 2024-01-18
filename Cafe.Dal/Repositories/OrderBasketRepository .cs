using Cafe.Dal.Interfaces;
using Cafe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Dal.Repositories
{
    public class OrderBasketRepository : IBaseRepository<orderbasket>
    {
        private readonly ApplicationDBContext _db;
        public OrderBasketRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public bool Create(orderbasket entity)
        {
            _db.orderbasket.Add(entity);
            _db.SaveChanges();
            return true;
        }

        public bool Delete(orderbasket entity)
        {
            _db.orderbasket.Remove(entity);
            _db.SaveChanges();
            return true;
        }

        public IQueryable<orderbasket> GetAll()
        {
            return _db.orderbasket;
        }

        public orderbasket Update(orderbasket entity)
        {
            _db.orderbasket.Update(entity);
            _db.SaveChanges();
            return entity;
        }
    }
}
