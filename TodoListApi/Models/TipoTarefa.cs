namespace TodoListApi.Models
{
    public class TipoTarefa
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataInclusao { get; set; } = DateTime.Now;
    }
}