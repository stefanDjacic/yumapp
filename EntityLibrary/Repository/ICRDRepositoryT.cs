using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary.Repository
{
    public interface ICRDRepository<T> /*: IReadRepository<T>*/
    {
        IQueryable<T> GetAll();
        Task<T> Add(T instance);
        Task Remove(T instance);
    }
}
