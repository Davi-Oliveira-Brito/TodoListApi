using TodoListApi.Models;

namespace TodoListApi.Repositories
{
    public interface ITarefaRepository
    {
        Task<IEnumerable<Tarefa>> GetAllAsync(int usuarioId);
        Task<Tarefa?> GetByIdAsync(int id, int usuarioId);
        Task<Tarefa> CreateAsync(Tarefa tarefa);
        Task<Tarefa?> UpdateAsync(int id, Tarefa tarefa, int usuarioId);
        Task<bool> DeleteAsync(int id, int usuarioId);
    }
}