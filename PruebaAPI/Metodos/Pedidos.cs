using PruebaAPI.Conexion;
using PruebaAPI.Models;
using System.Data.SqlClient;

namespace PruebaAPI.Metodos
{
    public class Metodo_Pedidos
    {
        ConexionDB conexion = new ConexionDB();
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

        private async Task<List<PedidosModel>> EjecutarSP(int accion, int? id, int? id_mesa, string? fecha_pedido, int? usuario_creacion, int? estatus)
        {
            var lista = new List<PedidosModel>();

            using (var sql = new SqlConnection(conexion.GetConexion()))
            using (var cmd = new SqlCommand("sp_CRUDPedido", sql))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                AgregarParametro(cmd, "@operacion", accion);
                AgregarParametro(cmd, "@id", id);
                AgregarParametro(cmd, "@id_mesa", id_mesa);
                AgregarParametro(cmd, "@fecha_pedido", fecha_pedido);
                AgregarParametro(cmd, "@usuario_creacion", usuario_creacion);
                AgregarParametro(cmd, "@estatus", estatus);

                await sql.OpenAsync();

                using (var leer = await cmd.ExecuteReaderAsync())
                {
                    while (await leer.ReadAsync())
                    {
                        var M_Pedidos = new PedidosModel
                        {
                            id_mesa = (int)leer["id_mesa"],
                            fecha_pedido = (String)leer["fecha_pedido"],
                            estatus = (int)leer["estatus"],
                            usuario_creacion = (int)leer["usuario_creacion"]
                        };

                        lista.Add(M_Pedidos);
                    }
                }
            }

            return lista;
        }

        public async Task<List<PedidosModel>> MostrarPedidos()
        {
            return await EjecutarSP(4, 0, 0, "", 0, 0);
        }

        public async Task<List<PedidosModel>> MostrarPedidosId(int id)
        {

            return await EjecutarSP(5, id, 0, "", 0, 0);
        }

        public async Task<List<PedidosModel>> InsertarPedidos(PedidosModel parametros)
        {
            return await EjecutarSP(1, 0, parametros.id_mesa, parametros.fecha_pedido, parametros.estatus, parametros.usuario_creacion);
        }

        public async Task<List<PedidosModel>> ActualizarPedidos(PedidosModel parametros)
        {
            return await EjecutarSP(2, parametros.id, parametros.id_mesa, parametros.fecha_pedido, parametros.estatus, parametros.usuario_creacion);
        }

        public async Task<List<PedidosModel>> EliminarTipoPerona(PedidosModel parametros)
        {
            return await EjecutarSP(3, parametros.id, 0, "", 0, 0);
        }
    }
}
