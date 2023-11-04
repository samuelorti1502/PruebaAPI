using RestauranteAPI.Conn;
using RestauranteAPI.Models;
using System.Data.SqlClient;

namespace RestauranteAPI.Metodos
{

    public class Metodo_Menu
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

        private async Task<List<MenuModel>> EjecutarSP(int accion, int? id_prod_menu, string? producto, string? descripcion, int? id_menu, decimal? precio_venta, int? id_estatus,
                int? usuario_creacion, string? imagen)
        {
            var lista = new List<MenuModel>();

            using (var sql = new SqlConnection(conexion.GetConexion()))
            using (var cmd = new SqlCommand("sp_CRUDProductoMenu", sql))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                AgregarParametro(cmd, "@operacion", accion);
                AgregarParametro(cmd, "@id_prod_menu", id_prod_menu);
                AgregarParametro(cmd, "@producto", producto);
                AgregarParametro(cmd, "@descripcion", descripcion);
                AgregarParametro(cmd, "@id_menu", id_menu);
                AgregarParametro(cmd, "@precio_venta", precio_venta);
                AgregarParametro(cmd, "@id_estatus", id_estatus);
                AgregarParametro(cmd, "@usuario_creacion", usuario_creacion);
                AgregarParametro(cmd, "@imagen", imagen);

                await sql.OpenAsync();

                using (var leer = await cmd.ExecuteReaderAsync())
                {
                    while (await leer.ReadAsync())
                    {
                        var M_Menu = new MenuModel
                        {
                            id_prod_menu = (int)leer["id_prod_menu"],
                            nombre = (string)leer["producto"],
                            descripcion = (string)leer["descripcion"],
                            id_menu = (int)leer["id_menu"],
                            precio = (decimal)leer["precio_venta"],
                            id_estatus = (int)leer["id_estatus"],
                            usuario_creacion = (int)leer["usuario_creacion"],
                            imagen = (string)leer["imagen"]
                        };

                        lista.Add(M_Menu);
                    }
                }
            }

            return lista;
        }

        private async Task<List<ProductosModel>> EjecutarSP2(int accion, int? id_prod_menu, string? producto, string? descripcion, string? id_menu, decimal? precio_venta, string? id_estatus,
                string? usuario_creacion, string? imagen)
        {
            var lista = new List<ProductosModel>();

            using (var sql = new SqlConnection(conexion.GetConexion()))
            using (var cmd = new SqlCommand("sp_CRUDProductoMenu", sql))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                AgregarParametro(cmd, "@operacion", accion);
                AgregarParametro(cmd, "@id_prod_menu", id_prod_menu);
                AgregarParametro(cmd, "@producto", producto);
                AgregarParametro(cmd, "@descripcion", descripcion);
                AgregarParametro(cmd, "@id_menu", id_menu);
                AgregarParametro(cmd, "@precio_venta", precio_venta);
                AgregarParametro(cmd, "@id_estatus", id_estatus);
                AgregarParametro(cmd, "@usuario_creacion", usuario_creacion);
                AgregarParametro(cmd, "@imagen", imagen);

                await sql.OpenAsync();

                using (var leer = await cmd.ExecuteReaderAsync())
                {
                    while (await leer.ReadAsync())
                    {
                        var M_Menu = new ProductosModel
                        {
                            id_prod_menu = (int)leer["id_prod_menu"],
                            categoria = (string)leer["categoria"],
                            nombre = (string)leer["producto"],
                            descripcion = (string)leer["descripcion"],
                            precio = (decimal)leer["precio"],
                            estatus = (string)leer["estatus"],
                            imagen = (string)leer["imagen"]
                        };

                        lista.Add(M_Menu);
                    }
                }
            }

            return lista;
        }

        public async Task<List<MenuModel>> MostrarModulos()
        {
            return await EjecutarSP(4, null, null, null, null, null, null, null, null);
        }
        public async Task<List<ProductosModel>> MostrarProductos()
        {
            return await EjecutarSP2(6, null, null, null, null, null, null, null, null);
        }
        public async Task<List<MenuModel>> MostrarModulos_id(int id)
        {
            return await EjecutarSP(5, id, null, null, null, null, null, null, null);
        }
        public async Task<List<MenuModel>> MostrarUltimoIngresado(string nombre)
        {
            return await EjecutarSP(9,0,nombre, null, null, null, null, null, null);
        }

        public async Task<List<MenuModel>> MostrarModulos_nombre(string nombre)
        {
            return await EjecutarSP(8, 0, nombre, null, null, 0, null, null, null);
        }

        public class ValidacionResultado
        {
            public bool? success { get; set; }
            public string? mensaje { get; set; }
        }

        public async Task<ValidacionResultado> InsertarModulos(MenuModel parametros)
        {
            var resultado = new ValidacionResultado();

            try
            {
                var _prod = await EjecutarSP(1, parametros.id_prod_menu, parametros.nombre, parametros.descripcion, parametros.id_menu, parametros.precio, parametros.id_estatus, parametros.usuario_creacion, parametros.imagen);

                //var _producto = _prod[0];

                resultado.success = true;
                resultado.mensaje = "El producto del menú ha sido creado con éxito.";
            }
            catch (Exception ex)
            {
                // Aquí capturas la excepción y devuelves un mensaje de error significativo
                resultado.success = false;
                resultado.mensaje = "Ha ocurrido un error al intentar insertar el producto del menú: " + ex.Message;
            }

            return resultado;
        }

        public async Task ModificarModulos(MenuModel parametros)
        {
            await EjecutarSP(2, parametros.id_prod_menu, parametros.nombre, parametros.descripcion, parametros.id_menu, parametros.precio, parametros.id_estatus, parametros.usuario_creacion, parametros.imagen);
        }

        public async Task ModificarRutaImagen(string ruta, int id)
        {
            await EjecutarSP(10,id, null, null, null, null, null, null,ruta);
        }

        public async Task EliminarModulos(MenuModel parametros)
        {
            await EjecutarSP(5, parametros.id_prod_menu, null, null, null, null, null, null, null);
        }

        public async Task InactivarProducto(int id)
        {
            await EjecutarSP(11, id, null, null, null, null, null, null, null);
        }

        public async Task<bool> VerificarExistenciaProducto(int id)
        {
            var _existe = await EjecutarSP(5, id, null, null, null, null, null, null, null);

            if(_existe.Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

    }
}