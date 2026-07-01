namespace TodoListApi.DTOs
{
    public class TarefaCreateDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int TipoTarefaId { get; set; }
    }
}