using Microsoft.AspNetCore.Mvc;
using RestauranteAPI.Metodos;
using RestauranteAPI.Models;

namespace RestauranteAPI.Controllers
{
    [ApiController]
    [Route("api/tipoMasa")]
    public class TipoMasaController: Controller
    {
        [HttpGet]
        [Route("listar")]
        public async Task<ActionResult<List<tipoMasa_CatalogoModel>>> Get()
        {
            try
            {
                var datos = new tipoMasa_Catalogo();
                var lista = await datos.MostrarMasas();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }

        }

        [HttpPost]
        public async Task Post([FromBody] tipoMasa_CatalogoModel parametros)
        {
            try
            {
                var funcion = new tipoMasa_Catalogo();
                await funcion.InsertarMasa(parametros);

            }
            catch (Exception ex)
            {
                StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPut("id")]
        public async Task Put(int id, [FromBody] tipoMasa_CatalogoModel parametros)
        {
            try
            {
                var funcion = new tipoMasa_Catalogo();
                parametros.id = id;
                await funcion.modificarMasa(parametros);

            }
            catch (Exception ex)
            {
                StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpDelete("id")]
        public async Task<ActionResult> Delete(int id)
        {
            var funcion = new tipoMasa_Catalogo();

            await funcion.eliminarMasa(id);
            return new OkResult();
        }

    }
}
