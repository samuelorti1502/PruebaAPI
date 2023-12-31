using Microsoft.AspNetCore.Mvc;
using RestauranteAPI.Metodos;
using RestauranteAPI.Models;
using System.Text;

namespace RestauranteAPI.Controllers.Administracion
{
    [ApiController]
    [Route("api/correo")]
    public class CorreoController: ControllerBase
    {
     
        [HttpGet]
        [Route("listar")]
        public async Task<ActionResult<List<CorreoModel>>> Get()
        {
            try
            {
                var datos = new Metodo_Correo();
                var lista = await datos.MostrarCorreo();


                if ((lista == null) || (lista.Count <= 0))
                {
                    return BadRequest("No se encontró correo proporcionado.");
                }

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

    
        [HttpGet("{email}")]

        public async Task<ActionResult<List<CorreoModel>>> Get(string email)
        {
            try
            {
                var datos = new Metodo_Correo();
                email = email.Replace("%40", "@");
                var lista = await datos.MostrarUsuario_correo(email);

                if ((lista == null) || (lista.Count<=0))
                {
                    return BadRequest("No se encontró correo proporcionado.");
                }
                
               if(lista.Count >0)
                {
                    const string caracteresPermitidos = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                    Random random = new Random();
                    StringBuilder token = new StringBuilder();

                    for (int i = 0; i < 10; i++)
                    {
                        int indice = random.Next(caracteresPermitidos.Length);
                        token.Append(caracteresPermitidos[indice]);
                    }
                    await datos.actualizarContraseña(email, token.ToString());
                    await datos.EnviarCorreo(email, token.ToString());
                }

                return Ok(lista);
              
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("confirmar/{email}")]

        public async Task<ActionResult<List<CorreoModel>>> Gett(string email)
        {
            try
            {
                var datos = new Metodo_Correo();
                email = email.Replace("%40", "@");
                var lista = await datos.MostrarUsuario_correo(email);

                if ((lista == null) || (lista.Count <= 0))
                {
                    return NotFound("No se encontró el Correo proporcionado.");
                }

                if (lista.Count > 0)
                {
                    const string caracteresPermitidos = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                    Random random = new Random();
                    StringBuilder token = new StringBuilder();

                    for (int i = 0; i < 10; i++)
                    {
                        int indice = random.Next(caracteresPermitidos.Length);
                        token.Append(caracteresPermitidos[indice]);
                    }
                    await datos.agregarTokenCuenta(email, token.ToString());
                    await datos.EnviarCorreoConfirmacion(email, token.ToString());
                }

                return Ok(lista);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("confirmar-password")]
        public async Task<ActionResult> Put(CorreoModel parametros)
        {

            try
            {
                var datos = new Metodo_Correo();
                //var lista = await datos.MostrarUsuario_correo(parametros.correoUsuario);
                var lista = await datos.MostrarUsuario_token(parametros.token);
                var entrada = "";

                foreach (var item in lista)
                {
                    entrada = item.token;
                }
                if ((lista == null) || (lista.Count <= 0))
                {
                    return NotFound("No se encontró el correo proporcionado.");
                    
                }
                
                if (lista.Count > 0)
                {
                    if (entrada== parametros.token)
                    {
                        await datos.ConfirmarContraseña(parametros.contraseña, parametros.token);
                       return Ok("Contraseña actualizada con éxito!!");
                    }
                 
                }
                //await datos.ConfirmarContraseña(parametros.correoUsuario, parametros.contraseña);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro el token no es correcto: " );
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("confirmar-cuenta")]
        public async Task<ActionResult> Putt(CorreoModel parametros)
        {
            try
            {
                var datos = new Metodo_Correo();
                
                var lista = await datos.MostrarUsuario_token(parametros.token);
                var entrada = "";

                foreach (var item in lista)
                {
                    entrada = item.token;
                }
                if (lista == null)
                {
                    return NotFound("No se encontró el correo proporcionado.");
                }

                if (lista.Count > 0)
                {
                    if (entrada == parametros.token)
                    {
                        await datos.ConfirmarCuenta(parametros.token);
                        return Ok("Cuenta confirmada con éxito!!");
                    }

                }
               
                return StatusCode(StatusCodes.Status500InternalServerError, "Error el token no es correcto: ");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
            }
        }
    }
}
