using Microsoft.AspNetCore.Mvc;
using RestauranteAPI.Models;

namespace RestauranteAPI.Controllers
{
    [Route("api/facturas")]
    [ApiController]
    public class FacturasController : Controller
    {
        [HttpGet]
        [Route("listar")]
        public async Task<ActionResult<List<FacturasModel>>> Get()
        {
            try
            {
                var datos = new Metodos_Facturas();
                var lista = await datos.MostrarRoles();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<FacturasModel>>> MostrarRol_id(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("El ID de rol debe ser un número entero positivo.");
                }

                // Obtener el rol
                var datos = new Metodos_Facturas();
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
        public async Task Post([FromBody] FacturasModel parametros)
        {
            try
            {
                var funcion = new Metodos_Facturas();
                await funcion.InsertarRol(parametros);
            }
            catch (Exception ex)
            {
                StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] FacturasModel parametros)
        {
            var funcion = new Metodos_Facturas();
            parametros.id_factura = id;
            await funcion.ModificarRol(parametros);
            return new OkResult();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromBody] FacturasModel parametros)
        {
            var funcion = new Metodos_Facturas();
            parametros.id_factura = id;
            await funcion.EliminarRol(parametros);
            return new OkResult();
        }

    }
}
