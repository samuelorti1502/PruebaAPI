using Microsoft.AspNetCore.Mvc;
using PruebaAPI.Models;

namespace PruebaAPI.Controllers
{
    [Route("api/estatus")]
    [ApiController]
    public class EstatusController : Controller
    {
        [HttpGet]
        [Route("listar")]
        public async Task<ActionResult<List<EstatusModel>>> Get()
        {
            try
            {
                var datos = new Metodos_Estatus();
                var lista = await datos.MostrarRoles();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<EstatusModel>>> MostrarRol_id(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("El ID de rol debe ser un número entero positivo.");
                }

                // Obtener el rol
                var datos = new Metodos_Estatus();
                var roles = await datos.MostrarRol_id(id);

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
        public async Task Post([FromBody] EstatusModel parametros)
        {
            try
            {
                var funcion = new Metodos_Estatus();
                await funcion.InsertarRol(parametros);
            }
            catch (Exception ex)
            {
                StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] EstatusModel parametros)
        {
            var funcion = new Metodos_Estatus();
            parametros.id = id;
            await funcion.ModificarRol(parametros);
            return new OkResult();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromBody] EstatusModel parametros)
        {
            var funcion = new Metodos_Estatus();
            parametros.id = id;
            await funcion.EliminarRol(parametros);
            return new OkResult();
        }

    }
}
