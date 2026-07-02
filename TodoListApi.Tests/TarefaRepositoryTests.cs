using Microsoft.EntityFrameworkCore;
using TodoListApi.Data;
using TodoListApi.Models;
using TodoListApi.Repositories;

namespace TodoListApi.Tests
{
    public class TarefaRepositoryTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);

            context.TipoTarefas.AddRange(
                new TipoTarefa { Id = 1, Descricao = "Normal", DataInclusao = new DateTime(2026, 1, 1) },
                new TipoTarefa { Id = 2, Descricao = "Urgente", DataInclusao = new DateTime(2026, 1, 1) }
            );

            context.Usuarios.Add(new Usuario
            {
                Id = 1,
                Nome = "Davi",
                Email = "davi@email.com",
                SenhaHash = "hash"
            });

            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task CreateAsync_DeveSalvarTarefa()
        {
            var context = GetDbContext();
            var repo = new TarefaRepository(context);

            var tarefa = new Tarefa
            {
                Titulo = "Teste",
                Descricao = "Descricao teste",
                TipoTarefaId = 1,
                UsuarioId = 1
            };

            var result = await repo.CreateAsync(tarefa);

            Assert.NotNull(result);
            Assert.Equal("Teste", result.Titulo);
            Assert.Equal(1, result.UsuarioId);
        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarSomenteTarefasDoUsuario()
        {
            var context = GetDbContext();
            var repo = new TarefaRepository(context);

            await repo.CreateAsync(new Tarefa { Titulo = "Tarefa 1", Descricao = "Desc 1", TipoTarefaId = 1, UsuarioId = 1 });
            await repo.CreateAsync(new Tarefa { Titulo = "Tarefa 2", Descricao = "Desc 2", TipoTarefaId = 2, UsuarioId = 1 });
            await repo.CreateAsync(new Tarefa { Titulo = "Tarefa Outro", Descricao = "Desc", TipoTarefaId = 1, UsuarioId = 99 });

            var result = await repo.GetAllAsync(1);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarTarefaCorreta()
        {
            var context = GetDbContext();
            var repo = new TarefaRepository(context);

            var created = await repo.CreateAsync(new Tarefa { Titulo = "Tarefa X", Descricao = "Desc X", TipoTarefaId = 1, UsuarioId = 1 });

            var result = await repo.GetByIdAsync(created.Id, 1);

            Assert.NotNull(result);
            Assert.Equal("Tarefa X", result.Titulo);
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarNullSeNaoExistir()
        {
            var context = GetDbContext();
            var repo = new TarefaRepository(context);

            var result = await repo.GetByIdAsync(999, 1);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarNullSeTarefaDeOutroUsuario()
        {
            var context = GetDbContext();
            var repo = new TarefaRepository(context);

            var created = await repo.CreateAsync(new Tarefa { Titulo = "Tarefa X", Descricao = "Desc X", TipoTarefaId = 1, UsuarioId = 1 });

            var result = await repo.GetByIdAsync(created.Id, 99);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_DeveAtualizarTarefa()
        {
            var context = GetDbContext();
            var repo = new TarefaRepository(context);

            var created = await repo.CreateAsync(new Tarefa { Titulo = "Original", Descricao = "Desc", TipoTarefaId = 1, UsuarioId = 1 });

            var updated = await repo.UpdateAsync(created.Id, new Tarefa
            {
                Titulo = "Atualizado",
                Descricao = "Nova desc",
                TipoTarefaId = 2,
                Concluida = true
            }, 1);

            Assert.NotNull(updated);
            Assert.Equal("Atualizado", updated.Titulo);
            Assert.True(updated.Concluida);
        }

        [Fact]
        public async Task DeleteAsync_DeveRemoverTarefa()
        {
            var context = GetDbContext();
            var repo = new TarefaRepository(context);

            var created = await repo.CreateAsync(new Tarefa { Titulo = "Deletar", Descricao = "Desc", TipoTarefaId = 1, UsuarioId = 1 });

            var deleted = await repo.DeleteAsync(created.Id, 1);
            var result = await repo.GetByIdAsync(created.Id, 1);

            Assert.True(deleted);
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_DeveRetornarFalseSeNaoExistir()
        {
            var context = GetDbContext();
            var repo = new TarefaRepository(context);

            var result = await repo.DeleteAsync(999, 1);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_DeveRetornarFalseSeTarefaDeOutroUsuario()
        {
            var context = GetDbContext();
            var repo = new TarefaRepository(context);

            var created = await repo.CreateAsync(new Tarefa { Titulo = "Tarefa", Descricao = "Desc", TipoTarefaId = 1, UsuarioId = 1 });

            var result = await repo.DeleteAsync(created.Id, 99);

            Assert.False(result);
        }
    }
}