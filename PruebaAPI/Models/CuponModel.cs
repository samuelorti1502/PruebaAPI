namespace RestauranteAPI.Models
{
    public class CuponModel
    {
        public int opcion { get; set; }
        public int? id { get; set; }
        public string? nombre { get; set; }
        public string? descripcion { get; set; }
        public float? descuento { get; set; }
        public int? id_status { get; set; }
        public int? usuario_creacion { get; set; }
    }
}
