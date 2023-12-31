﻿using Microsoft.Extensions.Configuration;
using RestauranteAPI.Conn;
using RestauranteAPI.Models;
using System.Data.SqlClient;
using System.Security.Claims;

public class Metodo_Usuario2
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

    public async Task<List<UsuarioModel>> EjecutarSP(int accion, int? id, string? nombres, string? apellidos, string? email, string? usuario, string? rol,
        string? estatus, string? usuario_creacion, string? password, string? temp)
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
            AgregarParametro(cmd, "@rol", rol);
            AgregarParametro(cmd, "@estatus", estatus);
            AgregarParametro(cmd, "@usuario_creacion", usuario_creacion);
            AgregarParametro(cmd, "@password", password);
            AgregarParametro(cmd, "@temp", temp);

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
                        rol = (string)leer["rol"],
                        estatus = (string)leer["estatus"],
                        token = (string)leer["token"],
                        confirmado = (int)leer["confirmado"],
                        usuario_creacion = (string)leer["usuario_creacion"],
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
        return await EjecutarSP(4, null, null, null, null, null, null, null, null, null, null);
    }

    public async Task<List<UsuarioModel>> MostrarUsuario_id(string usr)
    {
        return await EjecutarSP(5, null, null, null, null, usr, null, null, null, null, null);
    }

    public async Task InsertarUsuario(UsuarioModel parametros)
    {
        string hashPassword = BCrypt.Net.BCrypt.HashPassword(parametros.password);

        await EjecutarSP(1, 0, parametros.nombres, parametros.apellidos, parametros.email, parametros.usuario, parametros.rol, null,
            parametros.usuario_creacion, hashPassword, null);
    }

    public async Task ModificarUsuario(UsuarioModel parametros,string temporal)
    {
        string hashPassword = BCrypt.Net.BCrypt.HashPassword(parametros.password);

        await EjecutarSP(2, 0,parametros.nombres ,parametros.apellidos ,parametros.email, parametros.usuario,parametros.rol, parametros.estatus,parametros.usuario_creacion,hashPassword, temporal);
    }

    public async Task EliminarUsuario(string usuario)
    {
        await EjecutarSP(3, null, null, null, null, usuario, null, null, null, null, null);
    }

    public class ValidacionResultado
    {
        public bool success { get; set; }
        public string Mensaje { get; set; }
        public string Usuario { get; set; }
    }
    public async Task<ValidacionResultado> Validar(string? usr, string contraseñaUsuario)
    {
        var resultado = new ValidacionResultado();

        var usuarios = await EjecutarSP(6, null, null, null, null, usr, null, null, null, null, null);

        // Tomar el primer usuario de la lista (asumiendo que es el único)
        var usuario = usuarios[0];

        // Verificar si la lista de usuarios está vacía o si no se encontró el usuario
        if (usuarios.Count == 0)
        {
            resultado.success = false;
            resultado.Mensaje = "Usuario no encontrado";
            resultado.Usuario = "";
        }
        else
        {
            // Verificar la contraseña
            bool contraseñaValida = BCrypt.Net.BCrypt.Verify(contraseñaUsuario, usuario.password);

            if (contraseñaValida)
            {
                // Si la contraseña es válida, establecer el mensaje y el nombre de usuario
                resultado.success = true;
                resultado.Mensaje = "Contraseña válida";
                resultado.Usuario = usuario.usuario;
            }
            else
            {
                resultado.Mensaje = "Contraseña no válida";
            }
        }

        return resultado;
    }
}
