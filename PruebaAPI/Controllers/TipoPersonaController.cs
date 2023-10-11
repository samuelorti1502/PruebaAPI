using Microsoft.AspNetCore.Mvc;
using RestauranteAPI.Metodos;
using RestauranteAPI.Models;

namespace RestauranteAPI.Controllers
{
    [ApiController]
    [Route("api/tipo_persona")]
    public class TipoPersonaController : ControllerBase
    {

        [HttpGet]
        // [Route("listar")]
        public async Task<ActionResult<List<ModeloTipoPersona>>> Get()
        {
            try
            {
                var datos = new MetodosTipoPersona();
                var lista = await datos.MostrarTipoPersona();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<ModeloTipoPersona>>> MostrarTipoPersona_id(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("El ID de tipo de persona debe ser un número entero positivo.");
                }
                // Obtener el tipo de persona
                var datos = new MetodosTipoPersona();
                var tipoPersona = await datos.MostrarTipoPersonaId(id);

                if (tipoPersona == null)
                {
                    return NotFound("No se encontró un tipo de persona con el ID proporcionado.");
                }

                // Devolver la respuesta
                return Ok(tipoPersona);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }

        }

        [HttpPost]
        public async Task Post([FromBody] ModeloTipoPersona parametros)
        {
            try
            {
                if (parametros == null | parametros.tipo_persona == null | parametros.id_estatus <= 0 | parametros.usuario_creacion == null)
                {
                    BadRequest("No se proporcionaron todos los parametros para el tipo de persona.");
                }



                var datos = new MetodosTipoPersona();
                await datos.InsertarTipoPersona(parametros);
            }
            catch (Exception ex)
            {
                StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ModeloTipoPersona parametros)
        {
            var funcion = new MetodosTipoPersona();
            await funcion.ActualizarTipoPersona(parametros);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromBody] ModeloTipoPersona parametros)
        {
            var funcion = new MetodosTipoPersona();
            await funcion.EliminarTipoPerona(parametros);
            return Ok();
        }
    }
}
