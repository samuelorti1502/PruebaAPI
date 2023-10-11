using RestauranteAPI.Conn;
using RestauranteAPI.Models;
using System.Data.SqlClient;

namespace RestauranteAPI.Metodos
{
    public class Metodo_Mesas
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

        private async Task<List<MesasModel>> EjecutarSP(int accion, int? id, string? No_mesa, int? id_estatus, int? usuario_creacion)
        {
            var lista = new List<MesasModel>();

            using (var sql = new SqlConnection(conexion.GetConexion()))
            using (var cmd = new SqlCommand("sp_CRUDMesa", sql))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                AgregarParametro(cmd, "@operacion", accion);
                AgregarParametro(cmd, "@id_mesa", id);
                AgregarParametro(cmd, "@No_mesa", No_mesa);
                AgregarParametro(cmd, "@id_estatus", id_estatus);
                AgregarParametro(cmd, "@usuario_creacion", usuario_creacion);

                await sql.OpenAsync();

                using (var leer = await cmd.ExecuteReaderAsync())
                {
                    while (await leer.ReadAsync())
                    {
                        var M_Mesas = new MesasModel
                        {
                            id_mesa = (int)leer["id_mesa"],
                            No_mesa = (string)leer["No_mesa"],
                            id_estatus = (int)leer["id_estatus"],
                            usuario_creacion = (int)leer["usuario_creacion"]
                        };

                        lista.Add(M_Mesas);
                    }
                }
            }

            return lista;
        }

        public async Task<List<MesasModel>> MostrarModulos()
        {
            return await EjecutarSP(4, 0, "", 0, 0);
        }
        public async Task<List<MesasModel>> MostrarModulos_id(int id)
        {
            return await EjecutarSP(5, id, "", 0, 0);
        }
        public async Task InsertarModulos(MesasModel parametros)
        {
            await EjecutarSP(1, 0, parametros.No_mesa, parametros.id_estatus, parametros.usuario_creacion);
        }
        public async Task ModificarModulos(MesasModel parametros)
        {
            await EjecutarSP(2, parametros.id_mesa, parametros.No_mesa, parametros.id_estatus, parametros.usuario_creacion);
        }

        public async Task EliminarModulos(MesasModel parametros)
        {
            await EjecutarSP(3, parametros.id_mesa, "", 0, 0);
        }
    }
}
