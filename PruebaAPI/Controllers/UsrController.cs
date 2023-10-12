using Microsoft.AspNetCore.Mvc;
using RestauranteAPI.Metodos;
using RestauranteAPI.Models;

namespace RestauranteAPI.Controllers
{
    [Route("api/usr")]
    [ApiController]
    public class UsrController : ControllerBase
    {
        

        private readonly IConfiguration _config;

        public UsrController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<List<UsrModel>>> LoginUsr([FromBody] UsrModel parametros)
        {
            try
            {
                var datos = new Metodo_Usr(_config);
                var _usr = await datos.MostrarUsuario_usr(parametros);

                // Devolver la respuesta
                return Ok(_usr);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }

        }

        [HttpPost]
        [Route("login2")]
        public IActionResult Login(UsrModel parametros)
        {
            var datos = new Metodo_Usr(_config);
            var user = datos.Authenticate(parametros);

            if(user != null)
            {
                //Crear token
                return Ok("Usuario logueado");
            }

            return NotFound("Usuario no encontrado");
        }

        

    }
}
