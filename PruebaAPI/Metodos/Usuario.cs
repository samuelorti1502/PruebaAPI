﻿using PruebaAPI.Conexion;
using PruebaAPI.Models;
using System.Data.SqlClient;
using BCrypt.Net;

public class Metodo_Usuario
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

    private async Task<List<UsuarioModel>> EjecutarSP(int accion, int? id, string? nombres, string? apellidos, string? email, string? usuario, int? id_rol, int? id_estatus, int? usuarioCreacion, string? password)
    {
        var lista = new List<UsuarioModel>();

        using (var sql = new SqlConnection(conexion.GetConexion()))
        using (var cmd = new SqlCommand("sp_usuario_crud", sql))
        {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            AgregarParametro(cmd, "@accion", accion);
            AgregarParametro(cmd, "@id", id);
            AgregarParametro(cmd, "@nombres", nombres);
            AgregarParametro(cmd, "@apellidos", apellidos);
            AgregarParametro(cmd, "@email", email);
            AgregarParametro(cmd, "@usuario", usuario);
            AgregarParametro(cmd, "@id_rol", id_rol);
            AgregarParametro(cmd, "@id_estatus", id_estatus);
            AgregarParametro(cmd, "@usuario_creacion", usuarioCreacion);
            AgregarParametro(cmd, "@password", password);

            await sql.OpenAsync();

            using (var leer = await cmd.ExecuteReaderAsync())
            {
                while (await leer.ReadAsync())
                {
                    var M_Usuario = new UsuarioModel
                    {
                        id = (int)leer["id"],
                        nombres = (string)leer["nombres"],
                        apellidos = (string)leer["apellidos"],
                        email = (string)leer["email"],
                        usuario = (string)leer["usuario"],
                        id_rol = (int)leer["id_rol"],
                        id_estatus = (int)leer["id_estatus"],
                        usuario_creacion = (int)leer["usuario_creacion"],
                        password = (string)leer["password"]
                    };

                    lista.Add(M_Usuario);
                }
            }
        }

        return lista;
    }

    public async Task<List<UsuarioModel>> MostrarUsuario()
    {
        return await EjecutarSP(4, 0, "", "", "", "", 0, 0, 0, "");
    }
    public async Task<List<UsuarioModel>> MostrarUsuario_id(int id)
    {
        return await EjecutarSP(5, id, "", "", "", "", 0, 0, 0, "");
    }
    public async Task InsertarUsuario(UsuarioModel parametros)
    {
        string hashPassword = BCrypt.Net.BCrypt.HashPassword(parametros.password);

        await EjecutarSP(1, 0, parametros.nombres, parametros.apellidos, parametros.email, parametros.usuario, parametros.id_rol, parametros.id_estatus, parametros.usuario_creacion, hashPassword);
    }
    public async Task ModificarUsuario(UsuarioModel parametros)
    {
        string hashPassword = BCrypt.Net.BCrypt.HashPassword(parametros.password);

        await EjecutarSP(2, parametros.id, parametros.nombres, parametros.apellidos, parametros.email, parametros.usuario, parametros.id_rol, parametros.id_estatus, parametros.usuario_creacion, hashPassword);
    }

    public async Task EliminarUsuario(UsuarioModel parametros)
    {
        await EjecutarSP(3, parametros.id, "", "", "", "", 0, 0, 0, "");
    }
    public async Task<bool> ValidarUsuario(int? id, string? contraseñaUsuario)
    {
        var usuarios = await EjecutarSP(5, id, "", "", "", "", 0, 0, 0, "");

        if (usuarios.Count == 1)
        {
            var usuario = usuarios[0];

            bool contraseñasCoinciden = BCrypt.Net.BCrypt.Verify(contraseñaUsuario, usuario.password);

            return contraseñasCoinciden;
        }

        return false;
    }

    public async Task<string?> Validar(int? id, string? contraseñaUsuario)
{
    var usuarios = await EjecutarSP(5, id, "", "", "", "", 0, 0, 0, "");

    // Verificar si la lista de usuarios está vacía o si no se encontró el usuario
    if (usuarios.Count == 0)
    {
        return "Usuario no encontrado";
    }

    // Tomar el primer usuario de la lista (asumiendo que es el único)
    var usuario = usuarios[0];

    // Verificar la contraseña
    bool contraseñaValida = BCrypt.Net.BCrypt.Verify(contraseñaUsuario, usuario.password);

    if (contraseñaValida)
    {
        // Si la contraseña es válida, retornar la contraseña
        return usuario.password;
    }
    else
    {
        return "Contraseña no válida";
    }
}


}
