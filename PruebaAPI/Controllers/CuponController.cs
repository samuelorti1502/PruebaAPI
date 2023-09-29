using Microsoft.AspNetCore.Mvc;
using PruebaAPI.Metodos;
using PruebaAPI.Models;

namespace PruebaAPI.Controllers
{
    [Route("api/Cupones")]
    [ApiController]
    public class CuponController : ControllerBase
    {
        [HttpGet]
        [Route("listar")]
        public async Task<ActionResult<List<CuponModel>>> Get()
        {
            try
            {
                var datos = new Metodo_Cupon();
                var lista = await datos.MostrarCupon();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<CuponModel>>> MostrarCupon_id(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("El ID de rol debe ser un número entero positivo.");
                }

                // Obtener el rol
                var datos = new Metodo_Cupon();
                var roles = await datos.MostrarCupon_id(id);

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
        public async Task Post([FromBody] CuponModel parametros)
        {
            try
            {
                var funcion = new Metodo_Cupon();
                await funcion.InsertarCupon(parametros);
            }
            catch (Exception ex)
            {
                StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CuponModel parametros)
        {
            var funcion = new Metodo_Cupon();
            parametros.id = id;
            await funcion.ModificarCupon(parametros);
            return new OkResult();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromBody] CuponModel parametros)
        {
            var funcion = new Metodo_Cupon();
            parametros.id = id;
            await funcion.EliminarCupon(parametros);
            return new OkResult();
        }
    }
}