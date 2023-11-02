namespace RestauranteAPI.Models
{
    public class PedirModel
    {
        public int? id{ get; set; }
        public string? listaArmapizza { get; set; }
        public string? listaPizzaMitades { get; set; }
        public int? total { get; set; }
        public int? idMesa { get; set; }
        public string? tipoOrden { get; set; }
        public int? id_usuario { get; set; }


    }
}
