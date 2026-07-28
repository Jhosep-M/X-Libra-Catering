using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Lista()
        {
            var RespuestaApi = new ResponseAPI<List<MenuDTO>>();
            try
            {
                var lista = await _context.Menus.ToListAsync();
                var listaDTO = lista.Select(m => new MenuDTO
                {
                    Id = m.Id,
                    Nombre = m.Nombre,
                    Descripcion = m.Descripcion,
                    Categoria = m.Categoria,
                    Precio = m.Precio,
                    RequiereRefrigeracion = m.RequiereRefrigeracion
                }).ToList();
                RespuestaApi.EsCorrecto = true;
                RespuestaApi.Valor = listaDTO;
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
                        RequiereRefrigeracion = entidad.RequiereRefrigeracion
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
                    RequiereRefrigeracion = dto.RequiereRefrigeracion
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
                    _context.Menus.Remove(entidad);
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
