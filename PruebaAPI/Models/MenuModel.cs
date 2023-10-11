namespace RestauranteAPI.Models
{
    public class MenuModel
    {
        public int id_prod_menu { get; set; }
        public int? id_producto { get; set; }
        public string? descripcion { get; set; }
        public int? id_menu { get; set; }
        public int? precio_venta { get; set; }
        public int? id_estatus { get; set; }
        public int? fecha_creacion { get; set; }
        public int? usuario_creacion { get; set; }
        public int? imagen { get; set; }

    }
}
