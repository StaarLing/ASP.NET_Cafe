using Cafe.Dal.Interfaces;
using Cafe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Dal.Repositories
{
    public class DishsRepository:IBaseRepository<dishs>
    {
        private readonly ApplicationDBContext _db;
        public DishsRepository (ApplicationDBContext db)
        {
            _db = db;
        }
        public bool Create(dishs entity)
        {
            _db.dishs.Add(entity);
            _db.SaveChanges();
            return true;
        }

        public bool Delete(dishs entity)
        {
            _db.dishs.Remove(entity);
            _db.SaveChanges();
            return true;
        }

        public IQueryable<dishs> GetAll()
        {
            return _db.dishs;
        }

        public dishs Update(dishs entity)
        {
            _db.dishs.Update(entity);
            _db.SaveChanges();
            return entity;
        }
    }
}
