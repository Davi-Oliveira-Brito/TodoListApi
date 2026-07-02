using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoListApi.DTOs;
using TodoListApi.Models;
using TodoListApi.Repositories;

namespace TodoListApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TarefasController : ControllerBase
    {
        private readonly ITarefaRepository _repository;

        public TarefasController(ITarefaRepository repository)
        {
            _repository = repository;
        }

        private int GetUsuarioId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(claim!);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tarefas = await _repository.GetAllAsync(GetUsuarioId());
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
            var tarefa = await _repository.GetByIdAsync(id, GetUsuarioId());
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
                TipoTarefaId = dto.TipoTarefaId,
                Concluida = dto.Concluida,
                UsuarioId = GetUsuarioId()
            };

            var created = await _repository.CreateAsync(tarefa);
            var response = new TarefaResponseDto
            {
                Id = created.Id,
                Titulo = created.Titulo,
                Descricao = created.Descricao,
                Concluida = created.Concluida,
                DataInclusao = created.DataInclusao,
                TipoTarefaId = created.TipoTarefaId,
                TipoTarefaDescricao = created.TipoTarefa?.Descricao ?? string.Empty
            };
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TarefaCreateDto dto)
        {
            var tarefa = new Tarefa
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                TipoTarefaId = dto.TipoTarefaId,
                Concluida = dto.Concluida
            };

            var updated = await _repository.UpdateAsync(id, tarefa, GetUsuarioId());
            if (updated == null) return NotFound();

            var response = new TarefaResponseDto
            {
                Id = updated.Id,
                Titulo = updated.Titulo,
                Descricao = updated.Descricao,
                Concluida = updated.Concluida,
                DataInclusao = updated.DataInclusao,
                TipoTarefaId = updated.TipoTarefaId,
                TipoTarefaDescricao = updated.TipoTarefa?.Descricao ?? string.Empty
            };
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repository.DeleteAsync(id, GetUsuarioId());
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}