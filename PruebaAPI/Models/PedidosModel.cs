namespace PruebaAPI.Models
{
    public class PedidosModel
    {
        //public int opcion { get; set; }
        public int? id { get; set; }
        public int? id_mesa { get; set; }
        public string? fecha_pedido { get; set; }
        public int? usuario_creacion { get; set; }
        public int? estatus { get; set; }
    }
}
