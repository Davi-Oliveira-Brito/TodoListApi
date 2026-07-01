namespace TodoListApi.DTOs
{
    public class TarefaResponseDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public bool Concluida { get; set; }
        public DateTime DataInclusao { get; set; }
        public int TipoTarefaId { get; set; }
        public string TipoTarefaDescricao { get; set; } = string.Empty;
    }
}