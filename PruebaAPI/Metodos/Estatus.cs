using Microsoft.AspNetCore.Mvc;
using RestauranteAPI.Conn;
using RestauranteAPI.Models;
using System.Data.SqlClient;

public class Metodos_Estatus
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

    private async Task<List<EstatusModel>> EjecutarSP(int accion, int? id, string? nombre_estatus, int? usuarioCreacion)
    {
        var lista = new List<EstatusModel>();

        using (var sql = new SqlConnection(conexion.GetConexion()))
        using (var cmd = new SqlCommand("sp_CRUDEstatus", sql))
        {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            AgregarParametro(cmd, "@operacion", accion);
            AgregarParametro(cmd, "@id", id);
            AgregarParametro(cmd, "@nombre_estatus", nombre_estatus);
            AgregarParametro(cmd, "@usuario_creacion", usuarioCreacion);

            await sql.OpenAsync();

            using (var leer = await cmd.ExecuteReaderAsync())
            {
                while (await leer.ReadAsync())
                {
                    var M_Rol = new EstatusModel
                    {
                        id = (int)leer["id"],
                        nombre_estatus = (string)leer["nombre_estatus"],
                        usuario_creacion = (int)leer["usuario_creacion"]
                    };

                    lista.Add(M_Rol);
                }
            }
        }

        return lista;
    }

    public async Task<List<EstatusModel>> MostrarRoles()
    {
        return await EjecutarSP(4, 0, "", 0);
    }
    public async Task<List<EstatusModel>> MostrarRol_id(int id)
    {

        return await EjecutarSP(5, id, "", 0);
    }
    public async Task InsertarRol(EstatusModel parametros)
    {
        await EjecutarSP(1, 0, parametros.nombre_estatus, parametros.usuario_creacion);
    }
    public async Task ModificarRol(EstatusModel parametros)
    {
        await EjecutarSP(2, parametros.id, parametros.nombre_estatus, parametros.usuario_creacion);
    }

    public async Task EliminarRol(EstatusModel parametros)
    {
        await EjecutarSP(4, parametros.id, "", 0);
    }
}

