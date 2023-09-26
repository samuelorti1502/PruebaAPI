using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

using PruebaAPI.Models;

namespace PruebaAPI.Controllers
{
    [ApiController]
    [Route("api/rol")]
    public class RolController : Controller
    {
        [HttpGet]
        [Route("listar")]
        public async Task<ActionResult<List<Rol>>> Get()
        {
            var datos = new Metodos_Rol();
            var lista = await datos.MostrarRoles();
            return lista;
        }

    }
}
