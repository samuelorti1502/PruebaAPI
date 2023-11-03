using Microsoft.AspNetCore.Mvc;
using RestauranteAPI.Metodos;
using RestauranteAPI.Models;

namespace RestauranteAPI.Controllers
{
    [Route("api/Menu")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        [HttpGet]
        [Route("listar")]
        public async Task<ActionResult<List<MenuModel>>> Get()
        {
            try
            {
                var datos = new Metodo_Menu();
                var lista = await datos.MostrarModulos();

                if(lista == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, "El servidor no tiene la información solicitada.");
                }
                else
                {
                    return Ok(lista);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<List<MenuModel>>> GetProductos()
        {
            try
            {
                var datos = new Metodo_Menu();
                var lista = await datos.MostrarProductos();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<MenuModel>>> MostrarModulos_id(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("El ID de rol debe ser un número entero positivo.");
                }

                // Obtener el rol
                var datos = new Metodo_Menu();
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
        public async Task<ActionResult<List<MenuModel>>> MostrarModulos_Nombre(string nombre)
        {
            try
            {
                if (nombre == null)
                {
                    return BadRequest("El nombre es necesario.");
                }

                // Obtener el rol
                var datos = new Metodo_Menu();
                var roles = await datos.MostrarModulos_nombre(nombre);

                if (roles == null)
                {
                    return NotFound("No se encontraron productos con ese nombre.");
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
        public async Task<IActionResult> Post([FromBody] MenuModel parametros)
        {
            try
            {
                var funcion = new Metodo_Menu();
                var _respuesta = await funcion.InsertarModulos(parametros);
                //var mensaje = "El producto del menu ha sido creado con éxito.";

                //return CreatedAtAction(nameof(Get), new { parametros.id_prod_menu }, new { Mensaje = mensaje });
                return Ok(_respuesta);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("productos")]
        public async Task<IActionResult> NewProducto([FromBody] ProductosModel parametros)
        {
            try
            {
                var funcion = new Metodo_Menu();
               // await funcion.NewProducto(parametros);
                var mensaje = "El producto del menu ha sido creado con éxito.";

                return CreatedAtAction(nameof(Get), new { parametros.id_prod_menu }, new { Mensaje = mensaje });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPut]
        [Route("modificar/{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] MenuModel parametros)
        {
            try
            {
                var funcion = new Metodo_Menu();
                parametros.id_prod_menu = id;
                await funcion.ModificarModulos(parametros);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
                throw;
            }
            
        }

        [HttpPut]
        [Route("inactivar/{id}")]
        public async Task<ActionResult> Inactivar(int id)
        {
            try
            {
                var funcion = new Metodo_Menu();
                var productoExistente = await funcion.VerificarExistenciaProducto(id);

                if (productoExistente)
                {
                    await funcion.InactivarProducto(id);
                    return Ok("El producto ha sido inactivado exitosamente"); 
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "El producto especificado no existe.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromBody] MenuModel parametros)
        {
            var funcion = new Metodo_Menu();
            parametros.id_prod_menu = id;
            await funcion.EliminarModulos(parametros);
            return new OkResult();
        }
    }

}
