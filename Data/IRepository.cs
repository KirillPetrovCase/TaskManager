using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManager.Data
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();

        Task<T> Get(string id);

        Task Add(T entity);

        Task Update(T entity);

        Task Delete(string id);
    }
}