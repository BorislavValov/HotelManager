using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManager.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        ICollection<T> FindAll();
        T FindId(string id);
        bool Create(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        bool Save();
    }
}
