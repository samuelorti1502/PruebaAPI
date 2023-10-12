using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestauranteAPI.Models;
using System.Security.Claims;

namespace RestauranteAPI.Controllers
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
        public async Task<IActionResult> Post([FromBody] UsuarioModel parametros)
        {
            try
            {
                var funcion = new Metodo_Usuario();
                await funcion.InsertarUsuario(parametros);

                // Agregar un mensaje de éxito a la respuesta
                var mensaje = "El usuario ha sido creado con éxito, revisa tu correo para confirmar tu cuenta."; // Puedes personalizar el mensaje según tus necesidades.

                // Devolver una respuesta con el código 201 Created y el mensaje de éxito
                return CreatedAtAction(nameof(Get), new { parametros.id }, new { Mensaje = mensaje });
                //return Ok(mensaje);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPut("{user}")]
        public async Task<ActionResult> Put([FromBody] UsuarioModel parametros)
        {
            try
            {
                var funcion = new Metodo_Usuario();
                //parametros.usuario = user;
                await funcion.ModificarUsuario(parametros);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }
        [HttpDelete("{id}")]
        //[Route("listar")]
        public async Task<ActionResult> Delete(int id, [FromBody] UsuarioModel parametros)
        {
            var funcion = new Metodo_Usuario();

            //var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                string userId = userIdClaim.Value;
                return Ok($"ID de usuario: {userId}");
            }
            else
            {
                return BadRequest("Usuario no autenticado o no se encontró el ID.");
            }

            /*parametros.id = id;
            await funcion.EliminarUsuario(parametros);
            return new OkResult();*/
        }
    }
}
