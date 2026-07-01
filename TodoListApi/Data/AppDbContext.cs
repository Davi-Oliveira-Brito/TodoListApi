using Microsoft.EntityFrameworkCore;
using TodoListApi.Models;

namespace TodoListApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<TipoTarefa> TipoTarefas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipoTarefa>().HasData(
                new TipoTarefa { Id = 1, Descricao = "Normal", DataInclusao = new DateTime(2026, 1, 1) },
                new TipoTarefa { Id = 2, Descricao = "Urgente", DataInclusao = new DateTime(2026, 1, 1) }
            );
        }
    }
}