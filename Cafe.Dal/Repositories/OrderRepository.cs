using Cafe.Dal.Interfaces;
using Cafe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Dal.Repositories
{
    public class OrderRepository : IBaseRepository<order>
    {
        private readonly ApplicationDBContext _db;
        public OrderRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public bool Create(order entity)
        {
            _db.order.Add(entity);
            _db.SaveChanges();
            return true;
        }

        public bool Delete(order entity)
        {
            _db.order.Remove(entity);
            _db.SaveChanges();
            return true;
        }

        public IQueryable<order> GetAll()
        {
            return _db.order;
        }

        public order Update(order entity)
        {
            _db.order.Update(entity);
            _db.SaveChanges();
            return entity;
        }
    }
}
