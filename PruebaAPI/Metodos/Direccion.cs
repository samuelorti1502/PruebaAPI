using PruebaAPI.Conexion;
using PruebaAPI.Models;
using System.Data.SqlClient;
using BCrypt.Net;

public class Metodo_Direccion
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

    private async Task<List<DireccionModel>> EjecutarSP(int accion, int? id, int? id_persona, string? direccion, int? id_departamento, int? id_municipio, int? id_zona, int? id_estatus, int? usuarioCreacion)
    {
        var lista = new List<DireccionModel>();

        using (var sql = new SqlConnection(conexion.GetConexion()))
        using (var cmd = new SqlCommand("sp_CRUDDireccion", sql))
        {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            AgregarParametro(cmd, "@operacion", accion);
            AgregarParametro(cmd, "@id_direccion", id);
            AgregarParametro(cmd, "@id_persona", id_persona);
            AgregarParametro(cmd, "@direccion", direccion);
            AgregarParametro(cmd, "@id_departamento", id_departamento);
            AgregarParametro(cmd, "@id_municipio", id_municipio);
            AgregarParametro(cmd, "@id_zona", id_zona);
            AgregarParametro(cmd, "@id_estatus", id_estatus);
            AgregarParametro(cmd, "@usuario_creacion", usuarioCreacion);

            await sql.OpenAsync();

            using (var leer = await cmd.ExecuteReaderAsync())
            {
                while (await leer.ReadAsync())
                {
                    var M_Usuario = new DireccionModel
                    {
                        id_direccion = (int)leer["id_direccion"],
                        id_persona = (int)leer["id_persona"],
                        direccion = (string)leer["direccion"],
                        id_departamento = (int)leer["id_departamento"],
                        id_municipio = (int)leer["id_municipio"],
                        id_zona = (int)leer["id_zona"],
                        id_estatus = (int)leer["id_estatus"],
                        usuario_creacion = (int)leer["usuario_creacion"]
                    };

                    lista.Add(M_Usuario);
                }
            }
        }

        return lista;
    }

    public async Task<List<DireccionModel>> MostrarUsuario()
    {
        return await EjecutarSP(4, 0, 0, "", 0, 0, 0, 0, 0);
    }
    public async Task<List<DireccionModel>> MostrarUsuario_id(int id)
    {
        return await EjecutarSP(5, id, 0, "", 0, 0, 0, 0, 0);
    }
    public async Task InsertarUsuario(DireccionModel parametros)
    {
        await EjecutarSP(1, 0, parametros.id_persona, parametros.direccion, parametros.id_departamento, parametros.id_municipio, parametros.id_zona, parametros.id_estatus, parametros.usuario_creacion);
    }
    public async Task ModificarUsuario(DireccionModel parametros)
    {
        await EjecutarSP(2, parametros.id_direccion, parametros.id_persona, parametros.direccion, parametros.id_departamento, parametros.id_municipio, parametros.id_zona, parametros.id_estatus, parametros.usuario_creacion);
    }

    public async Task EliminarUsuario(DireccionModel parametros)
    {
        await EjecutarSP(5, parametros.id_direccion, 0, "", 0, 0, 0, 0, 0);
    }
}
