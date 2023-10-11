using RestauranteAPI.Conn;
using RestauranteAPI.Models;
using System.Data.SqlClient;

namespace RestauranteAPI.Metodos
{
    public class Metodo_Permisos
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

        private async Task<List<PermisosModel>> EjecutarSP(int accion, int? id, int? id_usuario, int? id_modulo, int? id_estatus, int? usuario_creacion)
        {
            var lista = new List<PermisosModel>();

            using (var sql = new SqlConnection(conexion.GetConexion()))
            using (var cmd = new SqlCommand("sp_CRUDPermiso", sql))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                AgregarParametro(cmd, "@operacion", accion);
                AgregarParametro(cmd, "@id", id);
                AgregarParametro(cmd, "@id_usuario", id_usuario);
                AgregarParametro(cmd, "@id_modulo", id_modulo);
                AgregarParametro(cmd, "@id_estatus", id_estatus);
                AgregarParametro(cmd, "@usuario_creacion", usuario_creacion);

                await sql.OpenAsync();

                using (var leer = await cmd.ExecuteReaderAsync())
                {
                    while (await leer.ReadAsync())
                    {
                        var M_Permisos = new PermisosModel
                        {
                            id = (int)leer["id"],
                            id_usuario = (int)leer["id_usuario"],
                            id_modulo = (int)leer["id_modulo"],
                            id_estatus = (int)leer["id_estatus"],
                            usuario_creacion = (int)leer["usuario_creacion"]
                        };

                        lista.Add(M_Permisos);
                    }
                }
            }

            return lista;
        }

        public async Task<List<PermisosModel>> MostrarPermisos()
        {
            return await EjecutarSP(4, 0, 0, 0, 0, 0);
        }
        public async Task<List<PermisosModel>> MostrarPermisos_id(int id)
        {
            return await EjecutarSP(5, id, 0, 0, 0, 0);
        }
        public async Task InsertarPermisos(PermisosModel parametros)
        {
            await EjecutarSP(1, 0, parametros.id_usuario, parametros.id_modulo, parametros.id_estatus, parametros.usuario_creacion);
        }
        public async Task ModificarPermisos(PermisosModel parametros)
        {
            await EjecutarSP(2, 0, parametros.id_usuario, parametros.id_modulo, parametros.id_estatus, parametros.usuario_creacion);
        }

        public async Task EliminarPermisos(PermisosModel parametros)
        {
            await EjecutarSP(3, parametros.id, 0, 0, 0, 0);
        }
    }
}
