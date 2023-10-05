using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaAPI.Models;

namespace PruebaAPI.Controllers
{
    [Route("api/usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        [HttpGet]
        [Route("listar")]
        public async Task<ActionResult<List<UsuarioModel>>> Get()
        {
            try
            {
                var datos = new Metodo_Usuario();
                var lista = await datos.MostrarUsuario();
                
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<UsuarioModel>>> MostrarUsuario_id(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("El ID de rol debe ser un número entero positivo.");
                }

                // Obtener el rol
                var datos = new Metodo_Usuario();
                var roles = await datos.MostrarUsuario_id(id);

                if (roles == null)
                {
                    return NotFound("No se encontró un rol con el ID proporcionado.");
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
        /*public async Task Post([FromBody] UsuarioModel parametros)
        {
            try
            {
                var funcion = new Metodo_Usuario();
                await funcion.InsertarUsuario(parametros);
            }
            catch (Exception ex)
            {
                StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }*/

        public async Task<IActionResult> Post([FromBody] UsuarioModel parametros)
        {
            try
            {
                var funcion = new Metodo_Usuario();
                await funcion.InsertarUsuario(parametros);

                // Agregar un mensaje de éxito a la respuesta
                var mensaje = "El usuario ha sido creado con éxito, revisa tu correo para confirmar tu cuenta."; // Puedes personalizar el mensaje según tus necesidades.

                // Devolver una respuesta con el código 201 Created y el mensaje de éxito
                return CreatedAtAction(nameof(Get), new { id = parametros.id }, new { Mensaje = mensaje });
                //return Ok(mensaje);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPost("validar")]
        public async Task<IActionResult> ValidarUsuario([FromBody] UsuarioModel parametros)
        {
            //return Ok(parametros.password);
            // Validar que se proporcionó el ID de usuario y la contraseña
            if (string.IsNullOrEmpty(parametros.usuario) || string.IsNullOrEmpty(parametros.password))
            {
                return BadRequest("ID de usuario y contraseña son requeridos.");
            }
            var funcion = new Metodo_Usuario();
            // Llamar a la función ValidarUsuario para verificar la contraseña
            bool contraseñaValida = await funcion.ValidarUsuario(parametros.id, parametros.password);
            //var contraseña = await funcion.Validar(parametros.id, parametros.password);

            // Verificar si la contraseña es válida
            if (contraseñaValida)
            {
                return Ok("Contraseña válida.");
            }
            else
            {
                return Unauthorized("La contraseña no es válida.");
            }

            //return Ok(contraseña);
        }


        [HttpPost("validar2")]
        public async Task<IActionResult> ValidarUsuario2(string user, String pass)
        {
            var datos = new Metodo_Usuario();
            var roles = await datos.Validar(user, pass);

            return Ok(roles);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UsuarioModel parametros)
        {
            var funcion = new Metodo_Usuario();
            parametros.id = id;
            await funcion.ModificarUsuario(parametros);
            return new OkResult();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromBody] UsuarioModel parametros)
        {
            var funcion = new Metodo_Usuario();
            parametros.id = id;
            await funcion.EliminarUsuario(parametros);
            return new OkResult();
        }
    }
}
