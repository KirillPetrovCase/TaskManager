using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManager.Data
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();

        Task<T> GetByIdAsync(string id);

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(string id);
    }
}