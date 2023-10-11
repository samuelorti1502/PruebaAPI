using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using RestauranteAPI.Models;

namespace RestauranteAPI.Controllers
{
    [ApiController]
    [Route("api/rol")]
    public class RolController : Controller
    {
        [HttpGet]
        [Route("listar")]
        public async Task<ActionResult<List<RolModel>>> Get()
        {
            try
            {
                var datos = new Metodos_Rol();
                var lista = await datos.MostrarRoles();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<RolModel>>> MostrarRol_id(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("El ID de rol debe ser un número entero positivo.");
                }

                // Obtener el rol
                var datos = new Metodos_Rol();
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
        public async Task Post([FromBody] RolModel parametros)
        {
            try
            {
                var funcion = new Metodos_Rol();
                await funcion.InsertarRol(parametros);
            }
            catch (Exception ex)
            {
                StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] RolModel parametros)
        {
            var funcion = new Metodos_Rol();
            parametros.id = id;
            await funcion.ModificarRol(parametros);
            return new OkResult();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromBody] RolModel parametros)
        {
            var funcion = new Metodos_Rol();
            parametros.id = id;
            await funcion.EliminarRol(parametros);
            return new OkResult();
        }
    }
}
