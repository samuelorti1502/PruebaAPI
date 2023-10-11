namespace RestauranteAPI.Models
{
    public class FacturasModel
    {
        public int? id_factura { get; set; }
        public int? id_restaurante { get; set; }
        public string serie { get; set; }
        public int numero { get; set; }
        public string fecha_fact { get; set; }
        public string nit { get; set; }
        public int direccion { get; set; }
        public int id_cupon { get; set; }
        public int id_estatus { get; set; }
        public int usuario_creacion { get; set; }

    }
}
