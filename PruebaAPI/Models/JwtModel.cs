using System.Security.Claims;

namespace RestauranteAPI.Models
{
    public class JwtModel
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set;}
        public string Subject { get; set; }

        /*public static dynamic validarToken(ClaimsIdentity identity)
        {
            try
            {
                if(identity.Claims.Count() == 0)
                {
                    return new
                    {
                        success = false,
                        message = "Verificar si el token es valido",
                        result = ""
                    };
                }
                var id = identity.Claims.FirstOrDefault(x => x.Type == "id").Value;

                UsrModel usuario = usuario.id;
            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = "Catch: " + ex.Message,
                    result = ""
                };
            }
        }*/
    }
}
