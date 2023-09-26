using PruebaAPI.Conexion;
using PruebaAPI.Models;
using System.Data.SqlClient;

public class Metodos_Rol
    {
        private ConexionDB conexion = new ConexionDB();

        private async Task<List<Rol>> EjecutarSP(int accion, int id, string rol, string descripcion, int usuarioCreacion)
        {
            var lista = new List<Rol>();

            using (var sql = new SqlConnection(conexion.GetConexion()))
            using (var cmd = new SqlCommand("sp_roles_crud", sql))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@accion", accion);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@rol", rol);
                cmd.Parameters.AddWithValue("@descripcion", descripcion);
                cmd.Parameters.AddWithValue("@usuario_creacion", usuarioCreacion);

                await sql.OpenAsync();

                using (var leer = await cmd.ExecuteReaderAsync())
                {
                    while (await leer.ReadAsync())
                    {
                        var M_Rol = new Rol
                        {
                            opcion = accion,
                            id = (int)leer["id"],
                            rol = (string)leer["rol"],
                            descripcion = (string)leer["descripcion"],
                            usuario_creacion = (int)leer["usuario_creacion"]
                        };

                        lista.Add(M_Rol);
                    }
                }
            }

            return lista;
        }

        public async Task<List<Rol>> MostrarRoles()
        {
            return await EjecutarSP(4, 0, "", "", 3);
        }

        /*public async Task<List<Rol>> MostrarRol_id(Rol parametros)
        {
            return await EjecutarSP(parametros.opcion, parametros.id, "", "", parametros.usuario_creacion);
        }

        /*public async Task InsertarRol(Rol parametros)
        {
            await EjecutarSP(parametros.opcion, 0, parametros.rol, parametros.descripcion, parametros.usuario_creacion);
        }

        public async Task ModificarRol(Rol parametros)
        {
            await EjecutarSP(parametros.opcion, parametros.id, parametros.rol, parametros.descripcion, parametros.usuario_creacion);
        }

        public async Task EliminarRol(Rol parametros)
        {
            await EjecutarSP(parametros.opcion, parametros.id, parametros.rol, parametros.descripcion, parametros.usuario_creacion);
        }*/
    }

