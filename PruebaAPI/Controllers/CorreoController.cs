using Microsoft.AspNetCore.Mvc;
using RestauranteAPI.Metodos;
using RestauranteAPI.Models;


namespace RestauranteAPI.Controllers.Administracion
{
    [ApiController]
    [Route("api/correo")]
    public class CorreoController: ControllerBase
    {
        [HttpPost]

        public async Task Post([FromBody] CorreoModel parametros)
        {
            try
            {
                var funcion = new Metodo_Correo();
                Console.WriteLine(parametros.correoUsuario);
                Console.WriteLine("--------------------");

                 await funcion.EnviarCorreo(parametros.correoUsuario);
            }
            catch (Exception ex)
            {
                StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }
    }
}
