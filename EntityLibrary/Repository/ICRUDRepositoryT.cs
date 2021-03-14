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
        T GetSingle(int id);
        T Add(T entry);
        T Update(T entry);
        void Remove(T entry);
    }
}
