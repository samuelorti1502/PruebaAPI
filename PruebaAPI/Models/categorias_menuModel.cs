using static System.Net.Mime.MediaTypeNames;

namespace RestauranteAPI.Models
{
    public class categorias_menuModel
    {
       public int? id_categoria { get; set; }
       public string? nombre { get; set; }
       public string? imagen { get; set; }
       public int? id_estatus { get; set; }
       public int? usuario_creacion { get; set; }
    }
}
