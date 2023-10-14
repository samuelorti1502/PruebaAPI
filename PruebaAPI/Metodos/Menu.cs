﻿using RestauranteAPI.Conn;
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
                            producto = (string)leer["producto"],
                            descripcion = (string)leer["descripcion"],
                            id_menu = (int)leer["id_menu"],
                            precio_venta = (decimal)leer["precio_venta"],
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

        public async Task<List<MenuModel>> MostrarModulos()
        {
            return await EjecutarSP(4, null, null, null, null, null, null, null, null);
        }
        public async Task<List<MenuModel>> MostrarModulos_id(int id)
        {
            return await EjecutarSP(5, id, null, null, null, null, null, null, null);
        }
        public async Task InsertarModulos(MenuModel parametros)
        {
            await EjecutarSP(1, parametros.id_prod_menu, parametros.producto, parametros.descripcion, parametros.id_menu, parametros.precio_venta, parametros.id_estatus, parametros.usuario_creacion, parametros.imagen);
        }
        public async Task ModificarModulos(MenuModel parametros)
        {
            await EjecutarSP(2, parametros.id_prod_menu, parametros.producto, parametros.descripcion, parametros.id_menu, parametros.precio_venta, parametros.id_estatus, parametros.usuario_creacion, parametros.imagen);
        }

        public async Task EliminarModulos(MenuModel parametros)
        {
            await EjecutarSP(5, parametros.id_prod_menu, null, null, null, null, null, null, null);
        }
    }
}