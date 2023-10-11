using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using RestauranteAPI.Conn;
using RestauranteAPI.Models;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RestauranteAPI.Metodos
{
    public class Metodo_Usr
    {
        public IConfiguration _config;

        private ConexionDB conexion = new ConexionDB();

        public Metodo_Usr(IConfiguration configuration)
        {
            _config = configuration;
        }

        /*public Metodo_Usr()
        {

        }*/

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

        private async Task<List<UsrModel>> EjecutarSP(int accion, int? id, string? usuario)
        {
            var lista = new List<UsrModel>();

            using (var sql = new SqlConnection(conexion.GetConexion()))
            using (var cmd = new SqlCommand("sp_login", sql))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                AgregarParametro(cmd, "@accion", accion);
                AgregarParametro(cmd, "@id", id);
                AgregarParametro(cmd, "@usuario", usuario);

                await sql.OpenAsync();

                using (var leer = await cmd.ExecuteReaderAsync())
                {
                    while (await leer.ReadAsync())
                    {
                        var M_Usuario = new UsrModel
                        {
                            id = (int)leer["id"],
                            usuario = (string)leer["usuario"],
                            password = (string)leer["password"]
                            //email = (string)leer["email"]
                        };

                        lista.Add(M_Usuario);
                    }
                }
            }

            return lista;
        }

        public async Task<List<UsrModel>> MostrarUsuario()
        {
            return await EjecutarSP(1, null, null);
        }

        public class ValidacionResultado
        {
            public bool? success { get; set; }
            public string? Mensaje { get; set; }
            public int? id { get; set; }
            public string? Usuario { get; set; }
            //public string? email { get; set; }
            public string? _token { get; set; }
        }
        public async Task<ValidacionResultado> MostrarUsuario_usr([FromBody] UsrModel parametros)
        {
            var resultado = new ValidacionResultado();
            var _usuarios = await EjecutarSP(2, null, parametros.usuario);

            var jwt = _config.GetSection("Jwt").Get<JwtModel>();

            var _usuario = _usuarios[0];

            if (_usuarios.Count == 0)
            {
                resultado.success = false;
                resultado.Mensaje = "Usuario no encontrado";
                resultado.Usuario = null;
                //resultado.email = null;
                resultado.id = null;
            }
            else
            {
                // Verificar la contraseña
                bool contraseñaValida = BCrypt.Net.BCrypt.Verify(parametros.password, _usuario.password);

                if (contraseñaValida)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("id", _usuario.id.ToString()),
                        new Claim("usuario", _usuario.usuario),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        jwt.Issuer,
                        jwt.Audience,
                        claims,
                        expires: DateTime.Now.AddHours(1), // Define el tiempo de expiración
                        signingCredentials: creds
                    );

                    resultado.success = true;
                    resultado.Mensaje = "Ingreso exitoso";
                    resultado.id = _usuario.id;
                    resultado.Usuario = _usuario.usuario;
                    //resultado.email= _usuario.email;
                    resultado._token = new JwtSecurityTokenHandler().WriteToken(token);
                }
                else
                {
                    resultado.success = false;
                    resultado.Mensaje = "Contraseña no válida";
                    resultado.Usuario = null;
                    //resultado.email = null;
                    resultado.id = null;
                }
            }

            return resultado;
        }
    }
}
