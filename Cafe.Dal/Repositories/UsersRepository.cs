using Cafe.Dal.Interfaces;
using Cafe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Dal.Repositories
{
    public class UserRepository : IBaseRepository<users>
    {
        private readonly ApplicationDBContext _db;

        public UserRepository(ApplicationDBContext db)
        {
            _db = db;
        }

        public IQueryable<users> GetAll()
        {
            return _db.users;
        }

        public bool Delete(users entity)
        {
            _db.users.Remove(entity);
            _db.SaveChanges();
            return true;
        }

        public bool Create(users entity)
        {
            _db.users.Add(entity);
            _db.SaveChanges();
            return true;
        }

        public users Update(users entity)
        {
            _db.users.Update(entity);
            _db.SaveChanges();

            return entity;
        }
    }
}
