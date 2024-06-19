using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using pagina_personal.DTOs;
using pagina_personal.Models;

namespace pagina_personal.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class HeaderController : ControllerBase
    {
        private readonly PersonalContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IConfiguration _configuracion;
        public HeaderController(PersonalContext context, IWebHostEnvironment hostEnvironment, IConfiguration configuracion)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _configuracion = configuracion;
        }


        // TITULARES
        #region Titulares
        [HttpGet("header_obtener")]
        public async Task<IActionResult> HeaderObtener()
        {
            Header h = await _context.Headers.FirstOrDefaultAsync();
            if(h == null)
            {
                return BadRequest();
            }
            try
            {
                // Rutas de servidor 
                string servidor = _configuracion.GetValue<string>("RutaServer:Servidor");
                string publico = _configuracion.GetValue<string>("RutaServer:Publico");

                HeaderDTO header = new HeaderDTO()
                {
                    IdHeader = h.IdHeader,
                    Titulo = h.Titulo,
                    Subtitulo = h.Subtitulo,
                    DocumentoCv = h.DocumentoCv,
                    Carrusel = await _context.HeaderFotoCarrusels.Select(h => new HeaderFotoCarruselDTO
                    {
                        IdHeaderFotoCarrusel = h.IdHeaderFotoCarrusel,
                        Foto = (h.Foto != null) ? h.Foto.ToString().Replace(servidor, publico).Replace(servidor, publico) : ""
                    }).ToListAsync(),
                    FotoFondo = h.FotoFondo
                };

                return Ok(header);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("header_editar")]
        public async Task<IActionResult> HeaderAgregar([FromBody] HeaderTitularDTO header)
        {
            Header h = await _context.Headers.FirstOrDefaultAsync();
            if(header == null && h == null || h == null || header == null)
            {
                return BadRequest();
            }
            try
            {
                h.Titulo = header.Titulo ?? h.Titulo;
                h.Subtitulo = header.Subtitulo ?? h.Subtitulo;

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("header_documento_cargar")]
        public async Task<IActionResult> HeaderDocumentoCargar([FromForm] HeaderCvDTO doc)
        {
            Header header = await _context.Headers.FirstOrDefaultAsync();
            if (doc == null || header == null)
            {
                return BadRequest();
            }
            try
            {

                // Verficamos is hay datos en la bd y si apunta a un archivo
                if(header.DocumentoCv != "" && Directory.Exists(header.DocumentoCv))
                {
                    System.IO.File.Delete(header.DocumentoCv);
                }

                // Tomamos la ruta y si no existe la creamos
                string rutaUploads = Path.Combine(_hostEnvironment.WebRootPath, "uploads");

                if (!Directory.Exists(rutaUploads))
                {
                    Directory.CreateDirectory(rutaUploads);
                }

                string nombreFoto = Path.GetFileName(doc.Documento.FileName) + "_headerDocCv_" + Guid.NewGuid().ToString();

                // LA RUTA QUE SE GUARDA EN BD
                string rutaFoto = Path.Combine(rutaUploads, nombreFoto);

                // GUARDA LA FOTO EN SERVIDOR
                using( var stream = new FileStream(rutaFoto, FileMode.Create))
                {
                    await doc.Documento.CopyToAsync(stream);
                }

                // Guardamos en la base de datos la ruta
                header.DocumentoCv = rutaFoto;
                await _context.SaveChangesAsync();

                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("header_documento_eliminar")]
        public async Task<IActionResult> HeaderDocoumentoEliminar()
        {
            Header header = await _context.Headers.FirstOrDefaultAsync();

            if(header == null)
            {
                return BadRequest();
            }
            try
            {
                if(header.DocumentoCv != "" && System.IO.File.Exists(header.DocumentoCv))
                {
                    System.IO.File.Delete(header.DocumentoCv);
                    header.DocumentoCv = "";
                }
                else if(header.DocumentoCv != "" && System.IO.File.Exists(header.DocumentoCv))
                {
                    header.DocumentoCv = "";
                }

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        #endregion


        #region Carrusel
        [HttpGet("header_foto_carrusel_optener/{idFotoCarrusel}")]
        public async Task<IActionResult> FotoCarruselOptener(int idFotoCarrusel)
        {
            HeaderFotoCarrusel hC = await _context.HeaderFotoCarrusels.Where(h => h.IdHeaderFotoCarrusel == idFotoCarrusel).FirstOrDefaultAsync();
            if (hC == null)
            {
                return BadRequest();
            }
            try
            {

                // Rutas de servidor 
                string servidor = _configuracion.GetValue<string>("RutaServer:Servidor");
                string publico = _configuracion.GetValue<string>("RutaServer:Publico");

                HeaderFotoCarruselDTO hDTO = new HeaderFotoCarruselDTO()
                {
                    IdHeaderFotoCarrusel = hC.IdHeaderFotoCarrusel,
                    Foto = (hC.Foto != null) ? hC.Foto.ToString().Replace(servidor, publico).Replace(servidor, publico) : ""
                };

                return Ok(hDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("header_foto_carrusel_agregar")]
        public async Task<IActionResult> FotoCarruselAgregar([FromForm] HeaderFotoCarruselPostDTO foto)
        {
            //if(foto == null)
            //{
            //    return BadRequest();
            //}
            try
            {

                // Verificar si se envió algún archivo
                //if (file == null || file.Length == 0)
                //{
                //    return BadRequest("No se proporcionó ningún archivo.");
                //}

                // Ruta donde se guardará la foto
                var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads");

                // Crear el directorio si no existe
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generar un nombre único para la foto
                var uniqueFileName = Path.GetFileName(foto.Foto.FileName) + "_headerFotoCarrusel_" + Guid.NewGuid().ToString();
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Guardar la foto en el servidor
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await foto.Foto.CopyToAsync(stream);
                }

                HeaderFotoCarrusel header = new HeaderFotoCarrusel()
                {
                    Foto = filePath
                };

                await _context.HeaderFotoCarrusels.AddAsync(header);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("header_foto_carrusel_editar/{idFotoCarrusel}")]
        public async Task<IActionResult> FotoCarruselEditar(int idFotoCarrusel, [FromForm] HeaderFotoCarruselPostDTO foto)
        {
            HeaderFotoCarrusel hC = await _context.HeaderFotoCarrusels.Where(h => h.IdHeaderFotoCarrusel == idFotoCarrusel).FirstOrDefaultAsync();
            if (hC == null)
            {
                return BadRequest();
            }
            try
            {

                if (hC.Foto != "" && System.IO.File.Exists(hC.Foto))
                {

                    System.IO.File.Delete(hC.Foto);

                    using (var stream = new FileStream(hC.Foto, FileMode.Create))
                    {
                        await foto.Foto.CopyToAsync(stream);
                    }

                }
                else if (hC.Foto == "" || !System.IO.File.Exists(hC.Foto))
                {
                    string rutaCarpeta = Path.Combine(_hostEnvironment.WebRootPath, "uploads");

                    string nombreFotoUnico = idFotoCarrusel.ToString() + "_fotoCarrusel_" + Guid.NewGuid().ToString() + "_" + Path.GetFileName(foto.Foto.FileName);
                    string rutaFoto = Path.Combine(rutaCarpeta, nombreFotoUnico);

                    using (var stream = new FileStream(rutaFoto, FileMode.Create))
                    {
                        await foto.Foto.CopyToAsync(stream);
                    }

                    hC.Foto = rutaFoto;
                }

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("header_foto_carrusel_eliminar/{idFotoCarrusel}")]
        public async Task<IActionResult> FotoCarruselEliminar(int idFotoCarrusel)
        {
            HeaderFotoCarrusel header = await _context.HeaderFotoCarrusels.Where(h => h.IdHeaderFotoCarrusel == idFotoCarrusel).FirstOrDefaultAsync();
            if (header == null)
            {
                return BadRequest();
            }
            try
            {
                if (System.IO.File.Exists(header.Foto))
                {
                    System.IO.File.Delete(header.Foto);
                }

                _context.HeaderFotoCarrusels.Remove(header);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        #endregion


        #region FondoHeader
        [HttpPut("header_fondo_editar/{idHeader}")]
        public async Task<IActionResult> FondoEditar(int idHeader, [FromForm] HeaderFotoFondo fondo)
        {

            Header header = await _context.Headers.Where(h => h.IdHeader == idHeader).FirstOrDefaultAsync();
            if (header == null)
            {
                return BadRequest();
            }

            try
            {

                if (header.FotoFondo == "")
                {
                    string rutaCarpeta = Path.Combine(_hostEnvironment.WebRootPath, "uploads");

                    // Creamos el directorio para guardar la foto
                    if (!Directory.Exists(rutaCarpeta))
                    {
                        Directory.CreateDirectory(rutaCarpeta);
                    }

                    // Generamos un nombre unico para el nombre de la foto
                    string nombreFondoUnico = Path.GetFileName(fondo.FotoFondo.FileName) + "_headerFotoFondo_" + Guid.NewGuid().ToString();
                    string fondoRuta = Path.Combine(rutaCarpeta, nombreFondoUnico);

                    // Guardamos en el servidor
                    using (var stream = new FileStream(fondoRuta, FileMode.Create))
                    {
                        await fondo.FotoFondo.CopyToAsync(stream);
                    }

                    header.FotoFondo = fondoRuta;
                }
                else if (header.FotoFondo != "")
                {
                    if (!System.IO.File.Exists(header.FotoFondo))
                    {
                        header.FotoFondo = "";
                    }
                    else if (System.IO.File.Exists(header.FotoFondo))
                    {
                        System.IO.File.Delete(header.FotoFondo);

                        using (var stream = new FileStream(header.FotoFondo, FileMode.Create))
                        {
                            await fondo.FotoFondo.CopyToAsync(stream);
                        }
                    }
                }



                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("header_fondo_eliminar/{idHeader}")]
        public async Task<IActionResult> FondoEliminar(int idHeader)
        {
            Header header = await _context.Headers.Where(h => h.IdHeader == idHeader).FirstOrDefaultAsync();
            if (header == null)
            {
                return BadRequest();
            }
            try
            {

                if (Directory.Exists(header.FotoFondo))
                {
                    System.IO.File.Delete(header.FotoFondo);
                }

                header.FotoFondo = "";

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        #endregion


    }
}
