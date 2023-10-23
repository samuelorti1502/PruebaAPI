using Microsoft.AspNetCore.Mvc;
using RestauranteAPI.Metodos;
using RestauranteAPI.Models;

namespace RestauranteAPI.Controllers
{
    [ApiController]
    [Route("api/tamanioCatalogo")]
    public class TamanioCatalogoController : Controller
    {

        [HttpGet]
        [Route("listar")]
        public async Task<ActionResult<List<TamanioCatalogoModel>>> Get()
        {
            try
            {
                var datos = new TamanioCatalogo();
                var lista = await datos.MostrarTamanios();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }

        }

        [HttpPost]
        public async Task Post([FromBody] TamanioCatalogoModel parametros)
        {
            try
            {
                var funcion = new TamanioCatalogo();
                await funcion.InsertarTamanio(parametros);

            }
            catch (Exception ex)
            {
                StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPut("id")]
    public async Task Put(int id, [FromBody] TamanioCatalogoModel parametros)
    {
            try
        {
                var funcion = new TamanioCatalogo();
                parametros.id = id;
                await funcion.modificarTamanio(parametros);

            }
            catch (Exception ex)
        {
                StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpDelete("id")]
        public async Task<ActionResult> Delete(int id)
        {
            var funcion = new TamanioCatalogo();

            await funcion.eliminarTamanio(id);
            return new OkResult();
        }
    }
}
