using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaAPI.Models;

namespace PruebaAPI.Controllers
{
    [Route("api/personas")]
    [ApiController]
    public class PersonasController : ControllerBase
    {
        [HttpGet]
        [Route("listar")]
        public async Task<ActionResult<List<PersonasModel>>> Get()
        {
            try
            {
                var datos = new Metodo_Personas();
                var lista = await datos.MostrarPersonas();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<PersonasModel>>> MostrarPersonas_id(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("El ID de rol debe ser un número entero positivo.");
                }

                // Obtener el rol
                var datos = new Metodo_Personas();
                var roles = await datos.MostrarPersonas_id(id);

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
        public async Task Post([FromBody] PersonasModel parametros)
        {
            try
            {
                var funcion = new Metodo_Personas();
                await funcion.InsertarPersonas(parametros);
            }
            catch (Exception ex)
            {
                StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] PersonasModel parametros)
        {
            var funcion = new Metodo_Personas();
            parametros.id_persona = id;
            await funcion.ModificarPersonas(parametros);
            return new OkResult();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromBody] PersonasModel parametros)
        {
            var funcion = new Metodo_Personas();
            parametros.id_persona = id;
            await funcion.EliminarPersonas(parametros);
            return new OkResult();
        }
    }
}
