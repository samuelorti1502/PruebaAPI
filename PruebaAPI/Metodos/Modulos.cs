using RestauranteAPI.Conn;
using RestauranteAPI.Models;
using System.Data.SqlClient;

namespace RestauranteAPI.Metodos
{
    public class Metodo_Modulos
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

        private async Task<List<ModulosModel>> EjecutarSP(int accion, int? id, string? nombre, int? id_estatus, int? usuario_creacion)
        {
            var lista = new List<ModulosModel>();

            using (var sql = new SqlConnection(conexion.GetConexion()))
            using (var cmd = new SqlCommand("sp_CRUDModulo", sql))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                AgregarParametro(cmd, "@operacion", accion);
                AgregarParametro(cmd, "@id", id);
                AgregarParametro(cmd, "@nombre", nombre);
                AgregarParametro(cmd, "@id_estatus", id_estatus);
                AgregarParametro(cmd, "@usuario_creacion", usuario_creacion);

                await sql.OpenAsync();

                using (var leer = await cmd.ExecuteReaderAsync())
                {
                    while (await leer.ReadAsync())
                    {
                        var M_Modulos = new ModulosModel
                        {
                            id = (int)leer["id"],
                            nombre = (string)leer["nombre"],
                            id_estatus = (int)leer["id_estatus"],
                            usuario_creacion = (int)leer["usuario_creacion"]
                        };

                        lista.Add(M_Modulos);
                    }
                }
            }

            return lista;
        }

        public async Task<List<ModulosModel>> MostrarModulos()
        {
            return await EjecutarSP(4, 0, "", 0, 0);
        }
        public async Task<List<ModulosModel>> MostrarModulos_id(int id)
        {
            return await EjecutarSP(5, id, "", 0, 0);
        }
        public async Task InsertarModulos(ModulosModel parametros)
        {
            await EjecutarSP(1, 0, parametros.nombre, parametros.id_estatus, parametros.usuario_creacion);
        }
        public async Task ModificarModulos(ModulosModel parametros)
        {
            await EjecutarSP(2, parametros.id, parametros.nombre, parametros.id_estatus, parametros.usuario_creacion);
        }

        public async Task EliminarModulos(ModulosModel parametros)
        {
            await EjecutarSP(3, parametros.id, "", 0, 0);
        }
    }
}
