using Microsoft.AspNetCore.Mvc;
using RestauranteAPI.Metodos;
using RestauranteAPI.Models;

namespace RestauranteAPI.Controllers
{
    [ApiController]
    [Route("api/tipoQueso")]
    public class TipoQuesoController: Controller
    {
        [HttpGet]
        [Route("listar")]
        public async Task<ActionResult<List<TipoQueso_CatalogoModel>>> Get()
        {
            try
            {
                var datos = new TipoQueso_Catalogo();
                var lista = await datos.MostrarQuesos();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }

        }

        [HttpPost]
        public async Task Post([FromBody] TipoQueso_CatalogoModel parametros)
        {
            try
            {
                var funcion = new TipoQueso_Catalogo();
                await funcion.InsertarQueso(parametros);

            }
            catch (Exception ex)
            {
                StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPut("id")]
        public async Task Put(int id, [FromBody] TipoQueso_CatalogoModel parametros)
        {
            try
            {
                var funcion = new TipoQueso_Catalogo();
                parametros.id = id;
                await funcion.modificarQueso(parametros);

            }
            catch (Exception ex)
            {
                StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpDelete("id")]
        public async Task Delete(int id)
        {
            try
            {
                var funcion = new TipoQueso_Catalogo();
                await funcion.eliminarQueso(id);

            }
            catch (Exception ex)
            {
                StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

    }
}
