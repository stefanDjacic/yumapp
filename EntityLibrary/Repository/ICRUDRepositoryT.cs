using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary.Repository
{
    public interface ICRUDRepository<T> : ICRDRepository<T>
    {
        Task<T> GetSingle(int id);
        Task<T> Update(T instance);
    }
}
