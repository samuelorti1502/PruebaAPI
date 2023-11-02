namespace RestauranteAPI.Models
{
    public class UsuarioModel
    {
        public int? id { get; set; }
        public string? nombres { get; set; }
        public string? apellidos { get; set; }
        public string? email { get; set; }
        public string?   usuario { get; set; }
        public string? password { get; set; }
        public string? rol { get; set; }
        public string? estatus { get; set; }
        public string? token { get; set; }
        public int confirmado { get; set; }
        public string? usuario_creacion { get; set; }
    }
}
