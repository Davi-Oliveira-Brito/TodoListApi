namespace TodoListApi.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public bool Concluida { get; set; } = false;
        public DateTime DataInclusao { get; set; } = DateTime.UtcNow;

        public int TipoTarefaId { get; set; }
        public TipoTarefa? TipoTarefa { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
    }
}