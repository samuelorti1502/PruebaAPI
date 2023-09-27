using Microsoft.AspNetCore.Mvc;
using PruebaAPI.Conexion;
using PruebaAPI.Models;
using System.Data.SqlClient;

public class Metodos_Rol
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

    private async Task<List<Rol>> EjecutarSP(int accion, int? id, string rol, string descripcion, int usuarioCreacion)
    {
        var lista = new List<Rol>();

        using (var sql = new SqlConnection(conexion.GetConexion()))
        using (var cmd = new SqlCommand("sp_roles_crud", sql))
        {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            AgregarParametro(cmd, "@accion", accion);
            AgregarParametro(cmd, "@id", id);
            AgregarParametro(cmd, "@rol", rol);
            AgregarParametro(cmd, "@descripcion", descripcion);
            AgregarParametro(cmd, "@usuario_creacion", usuarioCreacion);

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
                        //estatus = (string)leer["descripcion"],
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
    public async Task<List<Rol>> MostrarRol_id(int id)
    {
        
        return await EjecutarSP(5, id, "", "", 3);
    }
    public async Task InsertarRol(Rol parametros)
    {
        await EjecutarSP(1, 0, parametros.rol, parametros.descripcion, parametros.usuario_creacion);
    }
    public async Task ModificarRol(Rol parametros)
    {
        await EjecutarSP(2, parametros.id, parametros.rol, parametros.descripcion, parametros.usuario_creacion);
    }

    /*public async Task EliminarRol(Rol parametros)
    {
        await EjecutarSP(parametros.opcion, parametros.id, parametros.rol, parametros.descripcion, parametros.usuario_creacion);
    }*/
}

