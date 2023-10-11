using Microsoft.AspNetCore.Mvc;
using RestauranteAPI.Metodos;
using RestauranteAPI.Models;

namespace RestauranteAPI.Controllers
{
    [ApiController]
    [Route("api/municipio")]
    public class MunicipioController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<MunicipiosModel>>> Get()
        {
            try
            {
                var datos = new Metodo_Municipios();
                var lista = await datos.MostrarMunicipios();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<List<MunicipiosModel>>> MostrarMunicipio_id(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("El ID de municipio debe ser un número entero positivo.");
                }
                // Obtener el municipio
                var datos = new Metodo_Municipios();
                var municipios = await datos.MostrarMunicipio_id(id);

                if (municipios == null)
                {
                    return NotFound("No se encontró un municipio con el ID proporcionado.");
                }

                // Devolver la respuesta
                return Ok(municipios);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }

        }

        [HttpPost]
        public async Task Post([FromBody] MunicipiosModel parametros)
        {
            try
            {
                if (parametros == null)
                {
                    BadRequest("Datos inválidos.");
                }

                var datos = new Metodo_Municipios();
                await datos.InsertarMunicipio(parametros);
            }
            catch (Exception ex)
            {
                StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("El ID de municipio debe ser un número entero positivo.");
                }

                var datos = new Metodo_Municipios();
                await datos.EliminarMunicipio(id);

                return Ok("Municipio eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] MunicipiosModel parametros)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("El ID de municipio debe ser un número entero positivo.");
                }

                var datos = new Metodo_Municipios();
                parametros.id_municipio = id;
                await datos.ModificarMunicipio(parametros);

                return Ok("Municipio modificado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }
    }
}
