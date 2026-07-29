using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using X_Libra_Catering.Server.Data;
using X_Libra_Catering.Server.Models;
using X_Libra_Catering.Shared;

namespace X_Libra_Catering.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly BdXLibraCateringContext _context;

        public MenusController(BdXLibraCateringContext context)
        {
            _context = context;
        }

        [HttpGet("Lista")]
        public async Task<IActionResult> Lista([FromQuery] int pagina = 1, [FromQuery] int tamano = 20, [FromQuery] string? busqueda = null)
        {
            var RespuestaApi = new ResponseAPI<ResultadoPaginado<MenuDTO>>();
            try
            {
                var query = _context.Menus.Where(m => m.Activo).AsQueryable();

                if (!string.IsNullOrWhiteSpace(busqueda))
                    query = query.Where(m =>
                        m.Nombre.Contains(busqueda) ||
                        (m.Descripcion != null && m.Descripcion.Contains(busqueda)));

                var total = await query.CountAsync();

                var items = await query
                    .OrderBy(m => m.Nombre)
                    .Skip((pagina - 1) * tamano)
                    .Take(tamano)
                    .Select(m => new MenuDTO
                    {
                        Id = m.Id,
                        Nombre = m.Nombre,
                        Descripcion = m.Descripcion,
                        Categoria = m.Categoria,
                        Precio = m.Precio,
                        RequiereRefrigeracion = m.RequiereRefrigeracion,
                        ImagenRuta = m.ImagenRuta,
                        FechaCreacion = m.FechaCreacion,
                        FechaModificacion = m.FechaModificacion
                    })
                    .ToListAsync();

                RespuestaApi.EsCorrecto = true;
                RespuestaApi.Valor = new ResultadoPaginado<MenuDTO>
                {
                    Items = items,
                    Total = total,
                    Pagina = pagina,
                    Tamano = tamano
                };
                RespuestaApi.Mensaje = "Lista preparada";
            }
            catch (Exception ex)
            {
                RespuestaApi.EsCorrecto = false;
                RespuestaApi.Mensaje = ex.Message;
            }
            return Ok(RespuestaApi);
        }

        [HttpGet("Buscar/{Cod}")]
        public async Task<IActionResult> Buscar(int Cod)
        {
            var RespuestaApi = new ResponseAPI<MenuDTO>();
            try
            {
                var entidad = await _context.Menus.FindAsync(Cod);
                if (entidad == null)
                {
                    RespuestaApi.EsCorrecto = false;
                    RespuestaApi.Mensaje = "Menu no encontrado";
                }
                else
                {
                    RespuestaApi.EsCorrecto = true;
                    RespuestaApi.Valor = new MenuDTO
                    {
                        Id = entidad.Id,
                        Nombre = entidad.Nombre,
                        Descripcion = entidad.Descripcion,
                        Categoria = entidad.Categoria,
                        Precio = entidad.Precio,
                        RequiereRefrigeracion = entidad.RequiereRefrigeracion,
                        ImagenRuta = entidad.ImagenRuta,
                        FechaCreacion = entidad.FechaCreacion,
                        FechaModificacion = entidad.FechaModificacion
                    };
                    RespuestaApi.Mensaje = "Menu encontrado";
                }
            }
            catch (Exception ex)
            {
                RespuestaApi.EsCorrecto = false;
                RespuestaApi.Mensaje = ex.Message;
            }
            return Ok(RespuestaApi);
        }

        [HttpPost("Guardar")]
        public async Task<IActionResult> Guardar(MenuDTO dto)
        {
            var RespuestaApi = new ResponseAPI<int>();
            try
            {
                var entidad = new Menu
                {
                    Nombre = dto.Nombre,
                    Descripcion = dto.Descripcion,
                    Categoria = dto.Categoria,
                    Precio = dto.Precio,
                    RequiereRefrigeracion = dto.RequiereRefrigeracion,
                    ImagenRuta = dto.ImagenRuta
                };
                _context.Menus.Add(entidad);
                await _context.SaveChangesAsync();
                RespuestaApi.EsCorrecto = true;
                RespuestaApi.Valor = entidad.Id;
                RespuestaApi.Mensaje = "Menu guardado";
            }
            catch (Exception ex)
            {
                RespuestaApi.EsCorrecto = false;
                RespuestaApi.Mensaje = ex.Message;
            }
            return Ok(RespuestaApi);
        }

        [HttpPost("SubirImagen")]
        public async Task<IActionResult> SubirImagen(IFormFile archivo)
        {
            var RespuestaApi = new ResponseAPI<string>();
            try
            {
                if (archivo == null || archivo.Length == 0)
                {
                    RespuestaApi.EsCorrecto = false;
                    RespuestaApi.Mensaje = "No se selecciono un archivo";
                    return Ok(RespuestaApi);
                }

                var extension = Path.GetExtension(archivo.FileName).ToLower();
                var permitidas = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                if (!permitidas.Contains(extension))
                {
                    RespuestaApi.EsCorrecto = false;
                    RespuestaApi.Mensaje = "Formato no permitido. Use jpg, png o webp";
                    return Ok(RespuestaApi);
                }

                var nombreArchivo = $"{Guid.NewGuid()}{extension}";
                var rutaCompleta = Path.Combine("wwwroot", "uploads", "menus", nombreArchivo);

                using var inputStream = archivo.OpenReadStream();
                using var original = SKBitmap.Decode(inputStream);
                var maxW = 400;
                var maxH = 300;
                var escala = Math.Min((float)maxW / original.Width, (float)maxH / original.Height);
                using var resized = escala < 1
                    ? original.Resize(new SKImageInfo((int)(original.Width * escala), (int)(original.Height * escala)), new SKSamplingOptions(SKFilterMode.Linear))
                    : original;
                using var image = SKImage.FromBitmap(resized);
                using var data = image.Encode(SKEncodedImageFormat.Jpeg, 85);
                using var fileStream = new FileStream(rutaCompleta, FileMode.Create);
                data.SaveTo(fileStream);

                RespuestaApi.EsCorrecto = true;
                RespuestaApi.Valor = $"/uploads/menus/{nombreArchivo}";
                RespuestaApi.Mensaje = "Imagen subida";
            }
            catch (Exception ex)
            {
                RespuestaApi.EsCorrecto = false;
                RespuestaApi.Mensaje = ex.Message;
            }
            return Ok(RespuestaApi);
        }

        [HttpPut("Modificar/{Cod}")]
        public async Task<IActionResult> Modificar(int Cod, MenuDTO dto)
        {
            var RespuestaApi = new ResponseAPI<int>();
            try
            {
                var entidad = await _context.Menus.FindAsync(Cod);
                if (entidad == null)
                {
                    RespuestaApi.EsCorrecto = false;
                    RespuestaApi.Mensaje = "Menu no encontrado";
                }
                else
                {
                    entidad.Nombre = dto.Nombre;
                    entidad.Descripcion = dto.Descripcion;
                    entidad.Categoria = dto.Categoria;
                    entidad.Precio = dto.Precio;
                    entidad.RequiereRefrigeracion = dto.RequiereRefrigeracion;
                    entidad.ImagenRuta = dto.ImagenRuta;
                    await _context.SaveChangesAsync();
                    RespuestaApi.EsCorrecto = true;
                    RespuestaApi.Valor = entidad.Id;
                    RespuestaApi.Mensaje = "Menu modificado";
                }
            }
            catch (Exception ex)
            {
                RespuestaApi.EsCorrecto = false;
                RespuestaApi.Mensaje = ex.Message;
            }
            return Ok(RespuestaApi);
        }

        [HttpDelete("Eliminar/{Cod}")]
        public async Task<IActionResult> Eliminar(int Cod)
        {
            var RespuestaApi = new ResponseAPI<int>();
            try
            {
                var entidad = await _context.Menus.FindAsync(Cod);
                if (entidad == null)
                {
                    RespuestaApi.EsCorrecto = false;
                    RespuestaApi.Mensaje = "Menu no encontrado";
                }
                else
                {
                    entidad.Activo = false;
                    entidad.FechaModificacion = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    RespuestaApi.EsCorrecto = true;
                    RespuestaApi.Valor = entidad.Id;
                    RespuestaApi.Mensaje = "Menu eliminado";
                }
            }
            catch (Exception ex)
            {
                RespuestaApi.EsCorrecto = false;
                RespuestaApi.Mensaje = ex.Message;
            }
            return Ok(RespuestaApi);
        }
    }
}
