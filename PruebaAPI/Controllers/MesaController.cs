using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestauranteAPI.Metodos;
using RestauranteAPI.Models;

namespace RestauranteAPI.Controllers
{
    [Route("api/mesas")]
    [ApiController]
    public class MesaController : ControllerBase
    {
        [HttpGet]
        [Route("listar")]
        public async Task<ActionResult<List<MesasModel>>> Get()
        {
            try
            {
                var datos = new Metodo_Mesas();
                var lista = await datos.MostrarModulos();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<MesasModel>>> MostrarModulos_id(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("El ID de rol debe ser un número entero positivo.");
                }

                // Obtener el rol
                var datos = new Metodo_Mesas();
                var roles = await datos.MostrarModulos_id(id);

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
        public async Task Post([FromBody] MesasModel parametros)
        {
            try
            {
                var funcion = new Metodo_Mesas();
                await funcion.InsertarModulos(parametros);
            }
            catch (Exception ex)
            {
                StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] MesasModel parametros)
        {
            var funcion = new Metodo_Mesas();
            parametros.id_mesa = id;
            await funcion.ModificarModulos(parametros);
            return new OkResult();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromBody] MesasModel parametros)
        {
            var funcion = new Metodo_Mesas();
            parametros.id_mesa = id;
            await funcion.EliminarModulos(parametros);
            return new OkResult();
        }
    }
}
