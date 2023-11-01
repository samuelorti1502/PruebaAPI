using RestauranteAPI.Conn;
using RestauranteAPI.Models;
using System.Data.SqlClient;

namespace RestauranteAPI.Metodos
{
    public class pedirMetodos
    {
        private ConexionDB conexion = new ConexionDB();

        private void AgregarParametro(SqlCommand cmd, string nombre, object valor)
        {
            if (valor == null)
            {
                cmd.Parameters.AddWithValue(nombre, DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue(nombre, valor);
            }
        }

        private async Task<List<PedirModel>> EjecutarSP(int accion, string? listaArmapizza, string? listaPizzaMitades,int ? total,int ? idMesa, string tipoOrden, int? usuarioCreacion)
        {
            var lista = new List<PedirModel>();

            using (var sql = new SqlConnection(conexion.GetConexion()))
            using (var cmd = new SqlCommand("sp_pedido_producto", sql))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                AgregarParametro(cmd, "@accion", accion);
                AgregarParametro(cmd, "@listaArmapizza", listaArmapizza);
                AgregarParametro(cmd, "@listaPizzaMitades", listaPizzaMitades);
                AgregarParametro(cmd, "@total", total);
                AgregarParametro(cmd, "@idMesa", idMesa);
                AgregarParametro(cmd, "@tipoOrden", tipoOrden);
                AgregarParametro(cmd, "@id_usuario", usuarioCreacion);

                await sql.OpenAsync();

                using (var leer = await cmd.ExecuteReaderAsync())
                {
                    while (await leer.ReadAsync())
                    {
                        var M_PedirModel = new PedirModel
                        {
                            id = (int)leer["id"],
                            listaArmapizza = (string)leer["listaArmapizza"],
                            listaPizzaMitades = (string)leer["listaPizzaMitades"],
                            total = (int)leer["total"],
                            idMesa = (int)leer["idMesa"],
                            tipoOrden = (string)leer["tipoOrden"],
                            id_usuario = (int)leer["id_usuario"]
                        };

                        lista.Add(M_PedirModel);
                    }
                }
            }

            return lista;
        }

        public async Task<List<PedirModel>> MostrarPedido()
        {
            return await EjecutarSP(4, null, null, null, null, null, null);
        }

        public async Task InsertarPedido(PedirModel parametros)
        {
            await EjecutarSP(1, parametros.listaArmapizza,parametros.listaPizzaMitades, parametros.total,parametros.idMesa, parametros.tipoOrden, parametros.id_usuario);
        }

    }
}
