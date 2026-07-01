using Microsoft.AspNetCore.Mvc;
using TodoListApi.Repositories;

namespace TodoListApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoTarefasController : ControllerBase
    {
        private readonly ITipoTarefaRepository _repository;

        public TipoTarefasController(ITipoTarefaRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tipos = await _repository.GetAllAsync();
            return Ok(tipos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tipo = await _repository.GetByIdAsync(id);
            if (tipo == null) return NotFound();
            return Ok(tipo);
        }
    }
}