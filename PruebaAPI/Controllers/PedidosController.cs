using Microsoft.AspNetCore.Mvc;
using RestauranteAPI.Metodos;
using RestauranteAPI.Models;

namespace RestauranteAPI.Controllers
{
    [ApiController]
    [Route("api/pedidos")]
    public class PedidosController : ControllerBase
    {

        [HttpGet]
        // [Route("listar")]
        public async Task<ActionResult<List<PedidosModel>>> Get()
        {
            try
            {
                var datos = new Metodo_Pedidos();
                var lista = await datos.MostrarPedidos();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<PedidosModel>>> MostrarPedidos_id(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("El ID de tipo de persona debe ser un número entero positivo.");
                }
                // Obtener el tipo de persona
                var datos = new Metodo_Pedidos();
                var Pedidos = await datos.MostrarPedidosId(id);

                if (Pedidos == null)
                {
                    return NotFound("No se encontró un tipo de persona con el ID proporcionado.");
                }

                // Devolver la respuesta
                return Ok(Pedidos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }

        }

        [HttpPost]
        public async Task Post([FromBody] PedidosModel parametros)
        {
            try
            {
                if (parametros == null | parametros.id_mesa == null | parametros.estatus <= 0 | parametros.usuario_creacion == null)
                {
                    BadRequest("No se proporcionaron todos los parametros para el tipo de persona.");
                }

                var datos = new Metodo_Pedidos();
                await datos.InsertarPedidos(parametros);
            }
            catch (Exception ex)
            {
                StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] PedidosModel parametros)
        {
            var funcion = new Metodo_Pedidos();
            await funcion.ActualizarPedidos(parametros);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromBody] PedidosModel parametros)
        {
            var funcion = new Metodo_Pedidos();
            await funcion.EliminarTipoPerona(parametros);
            return Ok();
        }
    }
}
