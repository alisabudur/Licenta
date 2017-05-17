using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta_Project.DAL
{
    public interface IBaseEntityRepository<T> where T : class
    {
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        IEnumerable<T> GetAll();
        IEnumerable<T> FindBy(Func<T, bool> predicate);
    }
}
