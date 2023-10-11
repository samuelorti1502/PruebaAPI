using Microsoft.AspNetCore.Mvc;
using RestauranteAPI.Metodos;
using RestauranteAPI.Models;

namespace RestauranteAPI.Controllers
{
    [ApiController]
    [Route("api/departamento")]
    public class DepartamentoController : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<DepartamentosModel>>> Get()
        {
            try
            {
                var datos = new Metodo_Departamento();
                var lista = await datos.MostrarDepartamentos();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<DepartamentosModel>>> MostrarDepartamento_id(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("El ID de departamento debe ser un número entero positivo.");
                }
                // Obtener el departamento
                var datos = new Metodo_Departamento();
                var departamentos = await datos.MostrarDepartamento_id(id);

                if (departamentos == null)
                {
                    return NotFound("No se encontró un departamento con el ID proporcionado.");
                }

                // Devolver la respuesta
                return Ok(departamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }

        }
        [HttpPost]
        public async Task Post([FromBody] DepartamentosModel parametros)
        {
            try
            {
                if (parametros.nombre == null)
                {
                    throw new Exception("El nombre del departamento es requerido.");
                }
                var datos = new Metodo_Departamento();
                await datos.insertarDepartamento(parametros);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] DepartamentosModel parametros)
        {
            var funcion = new Metodo_Departamento();
            parametros.id_departamento = id;
            await funcion.actualizarDepartamento(parametros);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromBody] DepartamentosModel parametros)
        {
            var funcion = new Metodo_Departamento();
            parametros.id_departamento = id;
            await funcion.eliminarDepartamento(parametros);
            return Ok();
        }
    }

}
