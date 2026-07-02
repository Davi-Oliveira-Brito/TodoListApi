using System.ComponentModel;

namespace TodoListApi.DTOs
{
    public class TarefaCreateDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int TipoTarefaId { get; set; }

        [DefaultValue(false)]
        public bool Concluida { get; set; } = false;
    }
}