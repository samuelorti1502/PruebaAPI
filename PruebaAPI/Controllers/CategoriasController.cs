using Microsoft.AspNetCore.Mvc;
using RestauranteAPI.Metodos;
using RestauranteAPI.Models;

namespace RestauranteAPI.Controllers
{
    [Route("api/Categorias")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        [HttpGet]
        [Route("listar")]
        public async Task<ActionResult<List<CategoriasModel>>> Get()
        {
            try
            {
                var datos = new Metodo_Categorias();
                var lista = await datos.MostrarModulos();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<CategoriasModel>>> MostrarModulos_id(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("El ID de rol debe ser un número entero positivo.");
                }

                // Obtener el rol
                var datos = new Metodo_Categorias();
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
        [HttpGet]
        [Route("nombre/{nombre}")]

        public async Task<ActionResult<List<CategoriasModel>>> MostrarModulos_nombre(string nombre)
        {
            try
            {
                if (nombre == null)
                {
                    return BadRequest("El nombre es necesario");
                }

                // Obtener el rol
                var datos = new Metodo_Categorias();
                var roles = await datos.MostrarModulos_nombres(nombre);

                if (roles == null)
                {
                    return NotFound("No se encontró un rol con el nombre proporcionado.");
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
        public async Task<IActionResult> Post([FromBody] CategoriasModel parametros)
        {
            try
            {
                var funcion = new Metodo_Categorias();
                var _respuesta = await funcion.InsertarModulos(parametros);
                return Ok(_respuesta);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPut]
        [Route("modificar/{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CategoriasModel parametros)
        {
            try
            {
                var funcion = new Metodo_Categorias();
                parametros.id_categoria = id;
                var _respuseta = await funcion.ModificarModulos(parametros);
                return Ok(_respuseta);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
                throw;
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromBody] CategoriasModel parametros)
        {
            var funcion = new Metodo_Categorias();
            parametros.id_categoria = id;
            await funcion.EliminarModulos(parametros);
            return new OkResult();
        }
    }
}