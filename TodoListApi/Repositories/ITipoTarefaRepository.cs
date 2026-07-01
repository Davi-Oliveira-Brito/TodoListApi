using TodoListApi.Models;

namespace TodoListApi.Repositories
{
    public interface ITipoTarefaRepository
    {
        Task<IEnumerable<TipoTarefa>> GetAllAsync();
        Task<TipoTarefa?> GetByIdAsync(int id);
    }
}