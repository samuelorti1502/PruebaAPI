using PruebaAPI.Conexion;
using PruebaAPI.Models;
using System.Data.SqlClient;
using BCrypt.Net;

public class Metodo_Personas
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

    private async Task<List<PersonasModel>> EjecutarSP(int accion, int? id, string? nombres, string? apellidos, string? email, string? nit, int? id_tipo_persona, int? id_direccion, int? id_estatus, int? id_telefono, int? usuarioCreacion)
    {
        var lista = new List<PersonasModel>();

        using (var sql = new SqlConnection(conexion.GetConexion()))
        using (var cmd = new SqlCommand("sp_CRUDPersona", sql))
        {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            AgregarParametro(cmd, "@operacion", accion);
            AgregarParametro(cmd, "@id_persona", id);
            AgregarParametro(cmd, "@nombres", nombres);
            AgregarParametro(cmd, "@apellidos", apellidos);
            AgregarParametro(cmd, "@nit", nit);
            AgregarParametro(cmd, "@email", email);
            AgregarParametro(cmd, "@id_tipo_persona", id_tipo_persona);
            AgregarParametro(cmd, "@id_direccion", id_direccion);
            AgregarParametro(cmd, "@id_telefono", id_telefono);
            AgregarParametro(cmd, "@id_estatus", id_estatus);
            AgregarParametro(cmd, "@usuario_creacion", usuarioCreacion);

            await sql.OpenAsync();

            using (var leer = await cmd.ExecuteReaderAsync())
            {
                while (await leer.ReadAsync())
                {
                    var M_Personas = new PersonasModel
                    {
                        id_persona = (int)leer["id_persona"],
                        nombres = (string)leer["nombres"],
                        apellidos = (string)leer["apellidos"],
                        nit = (string)leer["nit"],
                        email = (string)leer["email"],
                        id_tipo_persona = (int)leer["id_tipo_persona"],
                        id_direccion = (int)leer["id_direccion"],
                        id_telefono = (int)leer["id_telefono"],
                        id_estatus = (int)leer["id_estatus"],
                        usuario_creacion = (int)leer["usuario_creacion"],
                    };

                    lista.Add(M_Personas);
                }
            }
        }

        return lista;
    }

    public async Task<List<PersonasModel>> MostrarPersonas()
    {
        return await EjecutarSP(4, 0, "", "", "", "", 0, 0, 0, 0, 0);
    }
    public async Task<List<PersonasModel>> MostrarPersonas_id(int id)
    {
        return await EjecutarSP(5, id, "", "", "", "", 0, 0, 0, 0, 0);
    }
    public async Task InsertarPersonas(PersonasModel parametros)
    {
        await EjecutarSP(1, 0, parametros.nombres, parametros.apellidos, parametros.nit, parametros.email, parametros.id_tipo_persona, parametros.id_direccion, parametros.id_telefono, parametros.id_estatus, parametros.usuario_creacion);
    }
    public async Task ModificarPersonas(PersonasModel parametros)
    {
        await EjecutarSP(1, parametros.id_persona, parametros.nombres, parametros.apellidos, parametros.nit, parametros.email, parametros.id_tipo_persona, parametros.id_direccion, parametros.id_telefono, parametros.id_estatus, parametros.usuario_creacion);
    }

    public async Task EliminarPersonas(PersonasModel parametros)
    {
        await EjecutarSP(5, parametros.id_persona, "", "", "", "", 0, 0, 0, 0, 0);
    }
}
