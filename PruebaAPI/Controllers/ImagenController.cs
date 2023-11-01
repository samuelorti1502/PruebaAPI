using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RestauranteAPI.Conn;
using RestauranteAPI.Helpers;
using RestauranteAPI.Models;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Transactions;

namespace RestauranteAPI.Controllers
{
    [Route("api/imagen")]
    [ApiController]
    public class ImagenController : ControllerBase
    {
        private ConexionDB conexion = new ConexionDB();

        private readonly IConfiguration Configuration;
        private readonly string _connectionString;
        private readonly string _nameProcedure;
        private readonly string pathImagenesMenu;
        private readonly string _root;
        private readonly long _FileSizeLimit;
        private readonly string _replaceFile;
        public ImagenController(IConfiguration _configuration)
        {
            Configuration = _configuration;
            //_connectionString = Configuration.GetConnectionString("MainConnection");
            _nameProcedure = "bod.crudAdjunto";
            pathImagenesMenu = Configuration.GetValue<string>("Files:pathImagenesMenu");
            _root = Configuration.GetValue<string>("Root");
            _FileSizeLimit = Configuration.GetValue<long>("FileSizeLimit");
            //_replaceFile = Configuration.GetValue<string>("ReplaceFiles:pathImage");
        }

        [HttpPost("subirimagen")]
        //[Produces("application/json")]
        //[Route("store")]
        public async Task<ActionResult>Store()
        {
            dynamic resultado;
            Responses result;

            bool Results = false;
            try
            {
                var _uploadfiles = Request.Form.Files;
                foreach (IFormFile source in _uploadfiles)
                {
                    string docName = source.FileName;
                    long size = source.Length;
                    string extension = Path.GetExtension(docName);
                    long sizeImage = source.Length;

                    string thisTime = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
                    string nameUnique = Guid.NewGuid().ToString();
                    string newNameDB = thisTime + "-" + nameUnique + "-" + docName;
                    string Filepath = _root + "/" + newNameDB;

                    Regex reg = new Regex(@"^.*\.(pdf) || \.(pdf)$ || \.(jpg|png|jpeg)$");

                    if (!reg.IsMatch(extension))
                    {
                        resultado = new JObject();
                        resultado.message = "Formato de archivo no soportado.";
                        resultado.response = 2;
                        resultado.value = 2;
                        return BadRequest(resultado);
                    }

                    if (size > _FileSizeLimit)
                    {
                        resultado = new JObject();
                        resultado.value = docName;
                        resultado.response = 0;
                        resultado.message = "Tamaño de archivo sobrepasa el limite soportado.";
                        resultado.Add(resultado);
                        return BadRequest(resultado);
                    }

                    if(!System.IO.Directory.Exists(Filepath))
                    {
                        System.IO.Directory.CreateDirectory(Filepath);
                    }

                    string imagePath = Filepath + "\\iamge.png";

                    if(!System.IO.File.Exists(imagePath))
                    {
                        System.IO.Directory.Delete(imagePath);
                    }
                    using(FileStream stream = System.IO.File.Create(imagePath))
                    {
                        await source.CopyToAsync(stream);
                        Results = true;
                    }
                }

            }
            catch (Exception ex)
            {
                /*result = new Responses(1003, ex.ToString());
                return BadRequest(result.Payback());*/

            }
            //return Ok("Image saved successfully");
            return Ok(Results);
        }

        [HttpPost]
        [Route("UploadFile")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var result = await WriteFile(file);
            return Ok(result);
        }

        private async Task<string> WriteFile(IFormFile file)
        {
            string filename = "";
            dynamic resultado;

            long size = file.Length;
            string docName = file.FileName;
            string extension = Path.GetExtension(docName);
            long sizeImage = file.Length;


            try
            {
                Regex reg = new Regex(@"^.*\.(pdf) || \.(pdf)$ || \.(jpg|png|jpeg)$");

                if (!reg.IsMatch(extension))
                {
                    resultado = new JObject();
                    resultado.message = "Formato de archivo no soportado.";
                    resultado.response = 2;
                    resultado.value = 2;
                    return BadRequest(resultado);
                }

                if (size > _FileSizeLimit)
                {
                    resultado = new JObject();
                    resultado.value = docName;
                    resultado.response = 0;
                    resultado.message = "Tamaño de archivo sobrepasa el limite soportado.";
                    resultado.Add(resultado);
                    return BadRequest(resultado);
                }

                string thisTime = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
                string nameUnique = Guid.NewGuid().ToString();
                string newNameDB = thisTime + "-" + nameUnique + "-" + docName;

                string docDB = Path.Combine(pathImagenesMenu, newNameDB);
                string docServ = Path.Combine(_root, docDB);

                //var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                filename = docServ + extension;

                var filepath = Path.Combine(docServ);

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(docServ);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    try
                    {
                        await file.CopyToAsync(stream);
                        resultado = new JObject();
                        resultado.message = "Documento almacenado con éxito.";
                        resultado.response = 1;
                        resultado.value = 1;
                        //transaction.Commit();

                        return Ok(resultado);
                    }
                    catch (Exception ex)
                    {
                        resultado = new JObject();
                        resultado.message = "Problema encontrado al guardar el registro.";
                        resultado.response = 7001;
                        resultado.value = ex.ToString();
                        //transaction.Rollback();
                        return BadRequest(resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                return ("Error interno del servidor: " + ex.Message);
            }

            return resultado;
        }

        

       

        private const string RutaBase = "/usr/share/nginx/html/media/menu/";

        [HttpPost]
        [Route("guardar-imagen")]
        public IActionResult SubirImagen([FromForm] ImagenModel archivo)
        {
            var file = archivo.nombre;
            if (archivo != null && file.Length > 0)
            {
                var rutaCompleta = Path.Combine(RutaBase, file.Name);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return Ok();
            }
            else
            {
                return BadRequest("No se ha enviado ningún archivo o el archivo es inválido.");
            }
        }

    }
}
