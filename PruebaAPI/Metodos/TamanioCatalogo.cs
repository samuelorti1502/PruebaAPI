using RestauranteAPI.Conn;
using RestauranteAPI.Models;
using System.Data.SqlClient;

namespace RestauranteAPI.Metodos
{
    public class TamanioCatalogo
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

        private async Task<List<TamanioCatalogoModel>> EjecutarSP(int accion, int? id, string? nombre, int? precio)
        {
            var lista = new List<TamanioCatalogoModel>();

            using (var sql = new SqlConnection(conexion.GetConexion()))
            using (var cmd = new SqlCommand("sp_tamanio_Catalogo", sql))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                AgregarParametro(cmd, "@accion", accion);
                AgregarParametro(cmd, "@id", id);
                AgregarParametro(cmd, "@nombre", nombre);
                AgregarParametro(cmd, "@precio", precio);
               // AgregarParametro(cmd, "@id_usuario", usuarioCreacion);

                await sql.OpenAsync();

                using (var leer = await cmd.ExecuteReaderAsync())
                {
                    while (await leer.ReadAsync())
                    {
                        var M_TamanioCatalogoModel = new TamanioCatalogoModel
                        {
                            id = (int)leer["id"],
                            nombre = (string)leer["nombre"],
                            precio = (int)leer["precio"],
                           // id_usuario = (int)leer["id_usuario"]
                        };

                        lista.Add(M_TamanioCatalogoModel);
                    }
                }
            }

            return lista;
        }

        public async Task<List<TamanioCatalogoModel>> MostrarTamanios()
        {
            return await EjecutarSP(4, null, null, null);
        }

        public async Task InsertarTamanio(TamanioCatalogoModel parametros)
        {
            await EjecutarSP(1, null,parametros.nombre, parametros.precio);
        }

        public async Task modificarTamanio(TamanioCatalogoModel parametros)
        {
            await EjecutarSP(2,parametros.id, parametros.nombre, parametros.precio);
        }

        public async Task eliminarTamanio(int id)
        {
            await EjecutarSP(3, null, null, null); ;
        }
    }
}
