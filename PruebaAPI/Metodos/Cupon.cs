using RestauranteAPI.Models;
using RestauranteAPI.Conn;
using RestauranteAPI.Models;
using System.Data.SqlClient;

namespace RestauranteAPI.Metodos
{
    public class Metodo_Cupon
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

        private async Task<List<CuponModel>> EjecutarSP(int accion, int? id, string? nombre, string? descripcion, float? descuento, int? id_estatus, int? usuario_creacion)
        {
            var lista = new List<CuponModel>();

            using (var sql = new SqlConnection(conexion.GetConexion()))
            using (var cmd = new SqlCommand("sp_CRUDCategoriaMenu", sql))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                AgregarParametro(cmd, "@operacion", accion);
                AgregarParametro(cmd, "@id_cupon", id);
                AgregarParametro(cmd, "@nombre", nombre);
                AgregarParametro(cmd, "@descripcion", descripcion);
                AgregarParametro(cmd, "@descuento", descuento);
                AgregarParametro(cmd, "@id_estatus", id_estatus);
                AgregarParametro(cmd, "@usuario_creacion", usuario_creacion);

                await sql.OpenAsync();

                using (var leer = await cmd.ExecuteReaderAsync())
                {
                    while (await leer.ReadAsync())
                    {
                        var M_Cupon = new CuponModel
                        {
                            id = (int)leer["id"],
                            nombre = (string)leer["nombre"],
                            descripcion = (string)leer["descripcion"],
                            id_status = (int)leer["id_estatus"],
                            descuento = (float)leer["descuento"],
                            usuario_creacion = (int)leer["usuario_creacion"]
                        };

                        lista.Add(M_Cupon);
                    }
                }
            }

            return lista;
        }

        public async Task<List<CuponModel>> MostrarCupon()
        {
            return await EjecutarSP(4, 0, "", "", 0, 0, 0);
        }
        public async Task<List<CuponModel>> MostrarCupon_id(int id)
        {
            return await EjecutarSP(5, id, "", "", 0, 0, 0);
        }
        public async Task InsertarCupon(CuponModel parametros)
        {
            await EjecutarSP(1, 0, parametros.nombre, parametros.descripcion, parametros.descuento, parametros.id_status, parametros.usuario_creacion);
        }
        public async Task ModificarCupon(CuponModel parametros)
        {
            await EjecutarSP(2, parametros.id, parametros.nombre, parametros.descripcion, parametros.descuento, parametros.id_status, parametros.usuario_creacion);
        }

        public async Task EliminarCupon(CuponModel parametros)
        {
            await EjecutarSP(3, parametros.id, "", "", 0, 0, 0);
        }
    }
}
