using Microsoft.AspNetCore.Mvc;
using RestauranteAPI.Metodos;
using RestauranteAPI.Models;

namespace RestauranteAPI.Controllers
{
    [ApiController]
    [Route("api/pedir")]
    public class pedirController : Controller
    {

        [HttpGet]
        [Route("listar")]
        public async Task<ActionResult<List<PedirModel>>> Get()
        {
            try
            {
                var datos = new pedirMetodos();
                var lista = await datos.MostrarPedido();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }

        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PedirModel parametros)
        {
            try
            {
                var funcion = new pedirMetodos();
                await funcion.InsertarPedido(parametros);
                return Ok("Pedido Agregado con éxito!!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

    }
}
