using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Text;


namespace RestauranteAPI.Metodos
{
    public class Metodo_Correo
    {

        public async Task EnviarCorreo(string correo)
        {
            try
            {
                var fromAddress = new MailAddress("pizzafresh35@gmail.com");

                using (MailMessage mensaje = new MailMessage())
                {
                    mensaje.To.Add(correo);
                    mensaje.Subject = "Reincio de Contraseña";
                    mensaje.Body = "<H1> Solicitaste un reincicio de Contraseña </H1> <P> Sigue el siguiente enlace <a href=\"http://localhost:5173/Olvide-Password/20\">Reiniciar Contraseña</a> </p> ";
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
    }
}
