using RestauranteAPI.Conn;
using RestauranteAPI.Models;
using System.Data.SqlClient;

namespace RestauranteAPI.Metodos
{
    public class Metodo_Categorias
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

        private async Task<List<CategoriasModel>> EjecutarSP(int accion, int? id, string? nombre, int? id_estatus, int? usuario_creacion)
        {
            var lista = new List<CategoriasModel>();

            using (var sql = new SqlConnection(conexion.GetConexion()))
            using (var cmd = new SqlCommand("sp_CRUDCategoriaMenu", sql))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                AgregarParametro(cmd, "@operacion", accion);
                AgregarParametro(cmd, "@id_categoria", id);
                AgregarParametro(cmd, "@nombre", nombre);
                AgregarParametro(cmd, "@id_estatus", id_estatus);
                AgregarParametro(cmd, "@usuario_creacion", usuario_creacion);

                await sql.OpenAsync();

                using (var leer = await cmd.ExecuteReaderAsync())
                {
                    while (await leer.ReadAsync())
                    {
                        var M_Modulos = new CategoriasModel
                        {
                            id_categoria = (int)leer["id_categoria"],
                            nombre = (string)leer["nombre"],
                            id_status = (int)leer["id_estatus"],
                            usuario_creacion = (int)leer["usuario_creacion"]
                        };

                        lista.Add(M_Modulos);
                    }
                }
            }

            return lista;
        }

        public async Task<List<CategoriasModel>> MostrarModulos()
        {
            return await EjecutarSP(4, 0, "", 0, 0);
        }
        public async Task<List<CategoriasModel>> MostrarModulos_id(int id)
        {
            return await EjecutarSP(5, id, "", 0, 0);
        }
        public async Task InsertarModulos(CategoriasModel parametros)
        {
            await EjecutarSP(1, 0, parametros.nombre, parametros.id_status, parametros.usuario_creacion);
        }
        public async Task ModificarModulos(CategoriasModel parametros)
        {
            await EjecutarSP(2, parametros.id_categoria, parametros.nombre, parametros.id_status, parametros.usuario_creacion);
        }

        public async Task EliminarModulos(CategoriasModel parametros)
        {
            await EjecutarSP(3, parametros.id_categoria, "", 0, 0);
        }
    }
}
