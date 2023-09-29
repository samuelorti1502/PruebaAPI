namespace PruebaAPI.Models
{
    public class PersonasModel
    {
        public int? @id_persona { get; set; }
        public string? nombres { get; set; }
        public string? apellidos { get; set; }
        public string? @nit { get; set; }
        public string? @email { get; set; }
        public int? id_tipo_persona { get; set; }
        public int? id_direccion { get; set; }
        public int? id_telefono { get; set; }
        public int? id_estatus { get; set; }
        public int? usuario_creacion { get; set; }
    }
}
