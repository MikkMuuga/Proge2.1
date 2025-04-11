
using Proge2._1.Data;

namespace KooliProjekt.Services
{
    public interface ITodoListService
    {
        Task<PagedResult<TodoList>> List(int page, int pageSize);
        Task<TodoList> Get(int id);
        Task Save(TodoList list);
        Task Delete(int id);
    }
}