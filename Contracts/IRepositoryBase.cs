using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeLeave.Contracts
{
    //interface is a template
    //for what class should've
    public interface IRepositoryBase<T> where T: class //generic interface that you can pass in any class
    {
        Task<ICollection<T>> FindAll();
        Task<T> FindById(int id);
        Task<bool> IsExist(int id);
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
        Task<bool> Save();
    }
}
