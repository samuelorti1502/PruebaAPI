namespace RestauranteAPI.Models
{
    public class ProductosModel
    {
        public int id_prod_menu { get; set; }
        public string? categoria { get; set; }
        public string? nombre { get; set; }
        public string? descripcion { get; set; }
        public decimal? precio { get; set; }
        public string? estatus { get; set; }
        public string? imagen { get; set; }
        public string? usuario_creacion { get; set; }
    }
}
