using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary.Repository
{
    public interface ICRUDRepository<T>
    {
        IQueryable<T> GetAll();
        Task<T> GetSingle(int id);
        Task<T> Add(T instance);
        Task<T> Update(T instance);
        Task Remove(T instance);
    }
}
