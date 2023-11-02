namespace RestauranteAPI.Models
{
    public class CategoriasModel
    {
        public int? @id_categoria { get; set; }
        public string? nombre { get; set; }
        public string? imagen { get; set; }
        public int? id_status { get; set; }
        public string? usuario_creacion { get; set; }
    }
}
