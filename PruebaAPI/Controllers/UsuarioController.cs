using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestauranteAPI.Controllers.Administracion;
using RestauranteAPI.Models;

namespace RestauranteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        [HttpGet]
        [Route("listar")]
        //[Authorize(Roles = ("Administrador"))]
        public async Task<ActionResult<List<UsuarioModel>>> Get()
        {
            try
            {
                var datos = new Metodo_Usuario2();
                var lista = await datos.MostrarUsuario();

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpGet("{usuario}")]
        public async Task<ActionResult<List<UsuarioModel>>> MostrarUsuario_id(string usuario)
        {
            try
            {
                if (string.IsNullOrEmpty(usuario))
                {
                    return BadRequest("El nombre de usuario es requerido.");
                }

                // Obtener el rol
                var datos = new Metodo_Usuario2();
                var roles = await datos.MostrarUsuario_id(usuario);

                if (roles == null || roles.Count<=0)
                {
                    return BadRequest("El usuario proporcionado no esta registrado.");
                }

                // Devolver la respuesta
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> InsertarUsuario([FromBody] UsuarioModel parametros)
        {
            try
            {
                var funcion = new Metodo_Usuario2();
                parametros.password = BCrypt.Net.BCrypt.HashPassword("$$**--");
                await funcion.InsertarUsuario(parametros);

                var mensaje = "El usuario ha sido creado con éxito, revisa tu correo para confirmar tu cuenta.";
                var ConfirmarCuenta = new CorreoController();
                await ConfirmarCuenta.Gett(parametros.email);

                return CreatedAtAction(nameof(Get), new { parametros.id }, new { Mensaje = mensaje });
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPut]
        [Route("modificar/{user}")]
        public async Task<ActionResult> ModificarUsuario(string user,[FromBody] UsuarioModel parametros)
        {
            try
            {
                var funcion = new Metodo_Usuario2();
                
                await funcion.ModificarUsuario(parametros, user);

                var mensaje = "El usuario " + parametros.usuario + " ha sido modificado con éxito.";

                return CreatedAtAction(nameof(Get), new { parametros.id }, new { Mensaje = mensaje });
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpDelete("{usuario}")]
        public async Task<ActionResult> Delete(string usuario)
        {
            try
            {
                var funcion = new Metodo_Usuario2();

                await funcion.EliminarUsuario(usuario);

                var mensaje = "El usuario " + usuario + " ha sido modificado con éxito.";

                return CreatedAtAction(nameof(Get), new { usuario }, new { Mensaje = mensaje });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
            
        }

        public class ValidacionResultado
        {
            public string Mensaje { get; set; }
            public string Usuario { get; set; }
        }

        [HttpPost("validar2")]
        public async Task<IActionResult> ValidarUsuario2(string user, String pass)
        {
            var datos = new Metodo_Usuario2();
            var roles = await datos.Validar(user, pass);

            return Ok(roles);
        }
    }
}
