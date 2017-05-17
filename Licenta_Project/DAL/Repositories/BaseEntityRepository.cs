using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licenta_Project.DAL;

namespace Licenta_Project.DAL
{
    public class BaseEntityRepository<T> : IBaseEntityRepository<T> where T : class
    {
        private readonly DdsmContext _context;
        private IDbSet<T> _entities;

        public BaseEntityRepository(DdsmContext context)
        {
            this._context = context;
        }

        protected IDbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = _context.Set<T>();
                }
                return _entities;
            }
        }

        protected DdsmContext DbContext
        {
            get
            {
                return _context;
            }
        }

        public void Delete(T entity)
        {
            var entry = this._context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.Entities.Attach(entity);
            }
            this.Entities.Remove(entity);
            _context.SaveChanges();
        }

        public IEnumerable<T> GetAll()
        {
            return this.Entities;
        }

        public T GetById(int id)
        {
            return this.Entities.Find(id);
        }

        public void Add(T entity)
        {
            this.Entities.Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            this._context.Entry(entity).State = EntityState.Modified;
            this._context.SaveChanges();
        }

        public IEnumerable<T> FindBy(Func<T, bool> predicate)
        {
            return this.Entities.Where(predicate);
        }
    }
}
