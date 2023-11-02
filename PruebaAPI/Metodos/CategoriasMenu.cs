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

        private async Task<List<CategoriasModel>> EjecutarSP(int accion, int? id, string? nombre, string? imagen, int? id_estatus, string? usuario_creacion)
        {
            var lista = new List<CategoriasModel>();

            using (var sql = new SqlConnection(conexion.GetConexion()))
            using (var cmd = new SqlCommand("sp_CRUDCategoriaMenu", sql))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                AgregarParametro(cmd, "@operacion", accion);
                AgregarParametro(cmd, "@id_categoria", id);
                AgregarParametro(cmd, "@nombre", nombre);
                AgregarParametro(cmd, "@imagen", imagen);
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
                            imagen = (string)leer["imagen"],
                            id_status = (int)leer["id_estatus"],
                            usuario_creacion = (string)leer["usuario_creacion"]
                        };

                        lista.Add(M_Modulos);
                    }
                }
            }

            return lista;
        }

        public async Task<List<CategoriasModel>> MostrarModulos()
        {
            return await EjecutarSP(4, null, null, null, null, null);
        }
        public async Task<List<CategoriasModel>> MostrarModulos_id(int id)
        {
            return await EjecutarSP(5, id, null, null, null, null);
        }
        public async Task<List<CategoriasModel>> MostrarModulos_nombres(string nombre)
        {
            return await EjecutarSP(6, null, nombre, null, null, null);
        }
        public class ValidacionResultado
        {
            public bool? success { get; set; }
            public string? mensaje { get; set; }
        }
        public async Task<ValidacionResultado> InsertarModulos(CategoriasModel parametros)
        {
            var resultado = new ValidacionResultado();

            try
            {
                await EjecutarSP(1, null, parametros.nombre, parametros.imagen, parametros.id_status, parametros.usuario_creacion);

                resultado.success = true;
                resultado.mensaje = "El producto del menú ha sido creado con éxito.";

            }
            catch (Exception ex)
            {
                resultado.success = false;
                resultado.mensaje = "Ha ocurrido un error al intentar insertar el producto del menú: " + ex.Message;

                throw;
            }
            return resultado;

        }
        public async Task<ValidacionResultado> ModificarModulos(CategoriasModel parametros)
        {
            var resultado = new ValidacionResultado();

            try
            {
                await EjecutarSP(2, parametros.id_categoria, parametros.nombre, parametros.imagen, parametros.id_status, parametros.usuario_creacion);
                resultado.success = true;
                resultado.mensaje = "El producto del menú ha sido modificado con éxito.";
            }
            catch (Exception ex )
            {
                resultado.success = false;
                resultado.mensaje = "Ha ocurrido un error al intentar modificar el producto del menú: " + ex.Message;

                throw;
            }
            return resultado;
        }

        public async Task EliminarModulos(CategoriasModel parametros)
        {
            await EjecutarSP(3, parametros.id_categoria, null, null, null, null);
        }
    }
}
