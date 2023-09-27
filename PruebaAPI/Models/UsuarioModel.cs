namespace PruebaAPI.Models
{
    public class UsuarioModel
    {
        //public int opcion { get; set; }
        public int? id { get; set; }
        public string? nombres { get; set; }
        public string? apellidos { get; set; }
        public string? email{ get; set; }
        public string? usuario { get; set; }
        public int? id_rol{ get; set; }
        public int? id_estatus { get; set; }
        public int? usuario_creacion { get; set; }
        public string? password { get; set; }
    }
}
