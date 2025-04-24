
using Microsoft.EntityFrameworkCore;
using Proge2._1.Data;

namespace KooliProjekt.Services
{
    public class TodoListService : ITodoListService
    {
        private readonly ApplicationDbContext _context;

        public TodoListService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<TodoList>> List(int page, int pageSize)
        {
            return await _context.TodoLists.GetPagedAsync(page, 5);
        }

        public async Task<TodoList> Get(int id)
        {
            return await _context.TodoLists.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Save(TodoList list)
        {
            if (list.Id == 0)
            {
                _context.Add(list);
            }
            else
            {
                _context.Update(list);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var todoList = await _context.TodoLists.FindAsync(id);
            if (todoList != null)
            {
                _context.TodoLists.Remove(todoList);
                await _context.SaveChangesAsync();
            }
        }
    }
}
