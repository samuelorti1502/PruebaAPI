using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaAPI.Metodos;
using PruebaAPI.Models;

namespace PruebaAPI.Controllers
{
    [Route("api/Permisos")]
    [ApiController]
    public class PermisosController : ControllerBase
    {
        [HttpGet]
        [Route("listar")]
        public async Task<ActionResult<List<PermisosModel>>> Get()
        {
            try
            {
                var datos = new Metodo_Permisos();
                var lista = await datos.MostrarPermisos();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<PermisosModel>>> MostrarPermisos_id(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("El ID de rol debe ser un número entero positivo.");
                }

                // Obtener el rol
                var datos = new Metodo_Permisos();
                var roles = await datos.MostrarPermisos_id(id);

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
        public async Task Post([FromBody] PermisosModel parametros)
        {
            try
            {
                var funcion = new Metodo_Permisos();
                await funcion.InsertarPermisos(parametros);
            }
            catch (Exception ex)
            {
                StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] PermisosModel parametros)
        {
            var funcion = new Metodo_Permisos();
            parametros.id = id;
            await funcion.ModificarPermisos(parametros);
            return new OkResult();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromBody] PermisosModel parametros)
        {
            var funcion = new Metodo_Permisos();
            parametros.id = id;
            await funcion.EliminarPermisos(parametros);
            return new OkResult();
        }
        
    }
}
