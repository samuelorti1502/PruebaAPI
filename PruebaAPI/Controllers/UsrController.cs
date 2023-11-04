using Microsoft.AspNetCore.Mvc;
using RestauranteAPI.Metodos;
using RestauranteAPI.Models;

namespace RestauranteAPI.Controllers
{
    [Route("api/usr")]
    [ApiController]
    public class UsrController : ControllerBase
    {
        

        public IConfiguration _config;

        public UsrController(IConfiguration config)
        {
            _config = config;
        }

        public class ValidacionResultado
        {
            public bool? success { get; set; }
            public string? Mensaje { get; set; }
            public int? id { get; set; }
            public string? Usuario { get; set; }
            public string? rol { get; set; }
            public string? _token { get; set; }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<List<UsrModel>>> LoginUsr([FromBody] UsrModel parametros)
        {
            var resultado = new ValidacionResultado();

            try
            {
                var datos = new Metodo_Usr(_config);
                var _existe = await datos.VerificarExistencia(parametros.usuario);
                if(_existe)
                {
                    var _usr = await datos.MostrarUsuario_usr(parametros);

                    if (_usr.Mensaje == "Contraseña no válida"){
                        return StatusCode(StatusCodes.Status400BadRequest, _usr);
                    }
                    else
                    {
                        return Ok(_usr);
                    }
                    
                }
                else
                {
                    resultado.success = false;
                    resultado.Mensaje = "El usuario especificado no existe.";
                    resultado.Usuario = null;
                    resultado.rol = null;
                    resultado.id = null;

                    return StatusCode(StatusCodes.Status400BadRequest, resultado);
                }
                
            }
            catch (Exception ex)
            {
                resultado.success = false;
                resultado.Mensaje = "Error interno del servidor: " + ex.Message;
                resultado.Usuario = null;
                resultado.rol = null;
                resultado.id = null;
                return StatusCode(StatusCodes.Status500InternalServerError, resultado);
            }

        }

    }
}
