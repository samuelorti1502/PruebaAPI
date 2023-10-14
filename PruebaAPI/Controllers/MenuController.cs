﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MenuModel parametros)
        {
            try
            {
                var funcion = new Metodo_Menu();
                await funcion.InsertarModulos(parametros);
                var mensaje = "El producto del menu ha sido creado con éxito.";

                return CreatedAtAction(nameof(Get), new { parametros.id_prod_menu }, new { Mensaje = mensaje });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] MenuModel parametros)
        {
            var funcion = new Metodo_Menu();
            parametros.id_prod_menu = id;
            await funcion.ModificarModulos(parametros);
            return new OkResult();
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