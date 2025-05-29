
using System.Threading.Tasks;

namespace Proge2._1.Data.Repositories
{
    public interface IBaseRepository<T> where T : Entity
    {
        Task<T> Get(int id);
        Task<PagedResult<T>> List(int page, int pageSize);
        Task Save(T item);
        Task Delete(int id);
    }
}

