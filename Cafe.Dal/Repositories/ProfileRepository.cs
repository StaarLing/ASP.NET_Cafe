using Cafe.Dal.Interfaces;
using Cafe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Dal.Repositories
{
    public class ProfileRepository : IBaseRepository<profile>
    {
        private readonly ApplicationDBContext _db;
        public ProfileRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public bool Create(profile entity)
        {
            _db.profile.Add(entity);
            _db.SaveChanges();
            return true;
        }

        public bool Delete(profile entity)
        {
            _db.profile.Remove(entity);
            _db.SaveChanges();
            return true;
        }

        public IQueryable<profile> GetAll()
        {
            return _db.profile;
        }

        public profile Update(profile entity)
        {
            _db.profile.Update(entity);
            _db.SaveChanges();
            return entity;
        }
    }
}
