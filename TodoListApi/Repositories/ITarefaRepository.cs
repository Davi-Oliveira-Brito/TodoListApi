using TodoListApi.Models;

namespace TodoListApi.Repositories
{
    public interface ITarefaRepository
    {
        Task<IEnumerable<Tarefa>> GetAllAsync();
        Task<Tarefa?> GetByIdAsync(int id);
        Task<Tarefa> CreateAsync(Tarefa tarefa);
        Task<Tarefa?> UpdateAsync(int id, Tarefa tarefa);
        Task<bool> DeleteAsync(int id);
    }
}