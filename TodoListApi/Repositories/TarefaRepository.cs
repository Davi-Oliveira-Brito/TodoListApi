using Microsoft.EntityFrameworkCore;
using TodoListApi.Data;
using TodoListApi.Models;

namespace TodoListApi.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly AppDbContext _context;

        public TarefaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tarefa>> GetAllAsync(int usuarioId)
        {
            return await _context.Tarefas
                .Include(t => t.TipoTarefa)
                .Where(t => t.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<Tarefa?> GetByIdAsync(int id, int usuarioId)
        {
            return await _context.Tarefas
                .Include(t => t.TipoTarefa)
                .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == usuarioId);
        }

        public async Task<Tarefa> CreateAsync(Tarefa tarefa)
        {
            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();
            return tarefa;
        }

        public async Task<Tarefa?> UpdateAsync(int id, Tarefa tarefa, int usuarioId)
        {
            var existing = await _context.Tarefas
                .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == usuarioId);
            if (existing == null) return null;

            existing.Titulo = tarefa.Titulo;
            existing.Descricao = tarefa.Descricao;
            existing.Concluida = tarefa.Concluida;
            existing.TipoTarefaId = tarefa.TipoTarefaId;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id, int usuarioId)
        {
            var existing = await _context.Tarefas
                .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == usuarioId);
            if (existing == null) return false;

            _context.Tarefas.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}