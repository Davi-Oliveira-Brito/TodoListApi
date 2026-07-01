using Microsoft.AspNetCore.Mvc;
using TodoListApi.DTOs;
using TodoListApi.Models;
using TodoListApi.Repositories;

namespace TodoListApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefasController : ControllerBase
    {
        private readonly ITarefaRepository _repository;

        public TarefasController(ITarefaRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tarefas = await _repository.GetAllAsync();
            var response = tarefas.Select(t => new TarefaResponseDto
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descricao = t.Descricao,
                Concluida = t.Concluida,
                DataInclusao = t.DataInclusao,
                TipoTarefaId = t.TipoTarefaId,
                TipoTarefaDescricao = t.TipoTarefa?.Descricao ?? string.Empty
            });
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tarefa = await _repository.GetByIdAsync(id);
            if (tarefa == null) return NotFound();

            var response = new TarefaResponseDto
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Concluida = tarefa.Concluida,
                DataInclusao = tarefa.DataInclusao,
                TipoTarefaId = tarefa.TipoTarefaId,
                TipoTarefaDescricao = tarefa.TipoTarefa?.Descricao ?? string.Empty
            };
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TarefaCreateDto dto)
        {
            var tarefa = new Tarefa
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                TipoTarefaId = dto.TipoTarefaId
            };

            var created = await _repository.CreateAsync(tarefa);
            var completa = await _repository.GetByIdAsync(created.Id);

            var response = new TarefaResponseDto
            {
                Id = completa!.Id,
                Titulo = completa.Titulo,
                Descricao = completa.Descricao,
                Concluida = completa.Concluida,
                DataInclusao = completa.DataInclusao,
                TipoTarefaId = completa.TipoTarefaId,
                TipoTarefaDescricao = completa.TipoTarefa?.Descricao ?? string.Empty
            };
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TarefaCreateDto dto)
        {
            var tarefa = new Tarefa
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                TipoTarefaId = dto.TipoTarefaId
            };

            var updated = await _repository.UpdateAsync(id, tarefa);
            if (updated == null) return NotFound();

            var completa = await _repository.GetByIdAsync(updated.Id);

            var response = new TarefaResponseDto
            {
                Id = completa!.Id,
                Titulo = completa.Titulo,
                Descricao = completa.Descricao,
                Concluida = completa.Concluida,
                DataInclusao = completa.DataInclusao,
                TipoTarefaId = completa.TipoTarefaId,
                TipoTarefaDescricao = completa.TipoTarefa?.Descricao ?? string.Empty
            };
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repository.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}