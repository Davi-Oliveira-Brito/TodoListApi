using Microsoft.EntityFrameworkCore;
using TodoListApi.Data;
using TodoListApi.Models;

namespace TodoListApi.Repositories
{
    public class TipoTarefaRepository : ITipoTarefaRepository
    {
        private readonly AppDbContext _context;

        public TipoTarefaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TipoTarefa>> GetAllAsync()
        {
            return await _context.TipoTarefas.ToListAsync();
        }

        public async Task<TipoTarefa?> GetByIdAsync(int id)
        {
            return await _context.TipoTarefas.FindAsync(id);
        }
    }
}