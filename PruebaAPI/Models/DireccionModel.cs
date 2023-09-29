namespace PruebaAPI.Models
{
    public class DireccionModel
    {
        //public int opcion { get; set; }
        public int? id_direccion { get; set; }
        public int? id_persona { get; set; }
        public string? direccion { get; set; }
        public int? id_departamento { get; set; }
        public int? id_municipio { get; set; }
        public int? id_zona { get; set; }
        public int? id_estatus { get; set; }
        public int? usuario_creacion { get; set; }
    }
}
