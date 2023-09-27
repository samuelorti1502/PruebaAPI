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
        public async Task Post([FromBody] UsuarioModel parametros)
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
