﻿namespace RestauranteAPI.Models
{
    public class MenuModel
    {
        public int id_prod_menu { get; set; }
        public string? nombre { get; set; }
        public string? descripcion { get; set; }
        public int? id_menu { get; set; }
        public decimal? precio { get; set; }
        public int? id_estatus { get; set; }
        //public string? fecha_creacion { get; set; }
        public int? usuario_creacion { get; set; }
        public string? imagen { get; set; }

    }
}
