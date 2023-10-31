using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using RestauranteAPI.Conn;
using RestauranteAPI.Models;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace RestauranteAPI.Metodos
{
    public class Metodo_Correo
    {
        private ConexionDB conexion = new ConexionDB();
        private string direccionToken = "http://localhost:5173/auth/forgot-password/";
        private string direccionTokenConfirmacion = "http://localhost:5173/auth/";

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

        private async Task<List<CorreoModel>> EjecutarSP(int accion, string? idUsuario, string? correoUsuario, string? token, int? activo,string? contraseña)
        {
            var lista = new List<CorreoModel>();

            using (var sql = new SqlConnection(conexion.GetConexion()))
            using (var cmd = new SqlCommand("SP_correo", sql))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                AgregarParametro(cmd, "@accion", accion);
                AgregarParametro(cmd, "@id", idUsuario);
                AgregarParametro(cmd, "@email", correoUsuario);
                AgregarParametro(cmd, "@token", token);
                AgregarParametro(cmd, "@confirmado", activo);
                AgregarParametro(cmd, "@password", contraseña);

                await sql.OpenAsync();

                using (var leer = await cmd.ExecuteReaderAsync())
                {
                    while (await leer.ReadAsync())
                    {
                        var M_correo = new CorreoModel
                        //var estado=0;
                        {
                            idUsuario = (int)leer["id"],
                            correoUsuario = (string)leer["email"],
                            token = (string)leer["token"],
                           // activo = (int)leer["confirmado"],
                            dato = ((int)leer["confirmado"] == 0) ? "No confirmado" : "Confirmado"

                        };

                        lista.Add(M_correo);
                    }
                }
            }

            return lista;
        }


        public async Task<List<CorreoModel>> MostrarCorreo()
        {
            return await EjecutarSP(4, null, null, null, null,null);
        }
        public async Task<List<CorreoModel>> MostrarUsuario_correo(string correo)
        {
            return await EjecutarSP(5,null,correo,null,null,null);
        }

        public async Task<List<CorreoModel>> MostrarUsuario_token(string token)
        {
            return await EjecutarSP(9, null, null,token, null, null);
        }

        public async Task actualizarContraseña(string correo, string token)
        {
            await EjecutarSP(2, null, correo,token,0,null);
        }

        public async Task ConfirmarContraseña(string contraseña,string token)
        {
            string hashPassword = BCrypt.Net.BCrypt.HashPassword(contraseña);
            await EjecutarSP(6, null,null, token, 1, hashPassword);
        }

        public async Task ConfirmarCuenta(string token)
        {
            await EjecutarSP(7, null,null, token, 1, null);
        }

        public async Task agregarTokenCuenta(string correo, string token)
        {
            await EjecutarSP(8, null, correo, token, 0, null);
        }

        public async Task EnviarCorreo(string correo,string token)
        {
            try
            {
                var fromAddress = new MailAddress("pizzafresh35@gmail.com");

                using (MailMessage mensaje = new MailMessage())
                {
                    mensaje.To.Add(correo);
                    direccionToken += token;
                    mensaje.Subject = "Reincio de Contraseña";
                    mensaje.Body = "<H1> Solicitaste un reincicio de Contraseña </H1> <P> Sigue el siguiente enlace <a href= "+direccionToken+">Reiniciar Contraseña</a> </p> ";
                    mensaje.IsBodyHtml = true;

                    // Remitente
                    mensaje.From = new MailAddress("pizzafresh35@gmail.com", "Reinicio de Contraseña");

                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential("pizzafresh35@gmail.com", "oeyv pnoy ratp hylc");
                        smtp.Port = 587;
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        smtp.Send(mensaje);
                    }
                }

                Console.WriteLine("Correo electrónico enviado con éxito a " + correo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar el correo electrónico: " + ex.Message);
            }

        }

        public async Task EnviarCorreoConfirmacion(string correo, string token)
        {
            try
            {
                var fromAddress = new MailAddress("pizzafresh35@gmail.com");

                using (MailMessage mensaje = new MailMessage())
                {
                    mensaje.To.Add(correo);
                    direccionTokenConfirmacion += token; 
                    mensaje.Subject = "Confirmación de Cuenta";
                    mensaje.Body = "<H1> Hola, gracias por registrarte con nosotros </H1> <P> Sigue el siguiente enlace para <a href= "+ direccionTokenConfirmacion + ">confirmar tu cuenta</a> </p> ";
                    mensaje.IsBodyHtml = true;

                    // Remitente
                    mensaje.From = new MailAddress("pizzafresh35@gmail.com", "Confirma tu Cuenta");

                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential("pizzafresh35@gmail.com", "oeyv pnoy ratp hylc");
                        smtp.Port = 587;
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        smtp.Send(mensaje);
                    }
                }

                Console.WriteLine("Correo electrónico enviado con éxito a " + correo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar el correo electrónico: " + ex.Message);
            }

        }
    }
}
