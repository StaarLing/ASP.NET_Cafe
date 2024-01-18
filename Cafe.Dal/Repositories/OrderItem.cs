using Cafe.Dal.Interfaces;
using Cafe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Dal.Repositories
{
    public class OrderItemRepository : IBaseRepository<orderitem>
    {
        private readonly ApplicationDBContext _db;
        public OrderItemRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public bool Create(orderitem entity)
        {
            _db.orderitem.Add(entity);
            _db.SaveChanges();
            return true;
        }

        public bool Delete(orderitem entity)
        {
            _db.orderitem.Remove(entity);
            _db.SaveChanges();
            return true;
        }

        public IQueryable<orderitem> GetAll()
        {
            return _db.orderitem;
        }

        public orderitem Update(orderitem entity)
        {
            _db.orderitem.Update(entity);
            _db.SaveChanges();
            return entity;
        }
    }
}
