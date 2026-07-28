using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X_Libra_Catering.Server.Data;
using X_Libra_Catering.Server.Models;
using X_Libra_Catering.Shared;

namespace X_Libra_Catering.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly BdXLibraCateringContext _context;

        public PedidosController(BdXLibraCateringContext context)
        {
            _context = context;
        }

        [HttpGet("Lista")]
        public async Task<IActionResult> Lista()
        {
            var RespuestaApi = new ResponseAPI<List<PedidoCabeceraDTO>>();
            try
            {
                var lista = await _context.PedidoCabeceras
                    .Include(p => p.Evento)
                    .Include(p => p.Vehiculo)
                    .Include(p => p.Detalles)
                    .ThenInclude(d => d.Menu)
                    .ToListAsync();
                var listaDTO = lista.Select(p => new PedidoCabeceraDTO
                {
                    Id = p.Id,
                    EventoId = p.EventoId,
                    VehiculoId = p.VehiculoId,
                    FechaPedido = p.FechaPedido,
                    Estado = p.Estado,
                    Total = p.Total,
                    EventoNombre = p.Evento?.NombreEvento,
                    VehiculoPlaca = p.Vehiculo?.Placa,
                    Detalles = p.Detalles.Select(d => new PedidoDetalleDTO
                    {
                        Id = d.Id,
                        PedidoId = d.PedidoId,
                        MenuId = d.MenuId,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario,
                        Subtotal = d.Subtotal,
                        MenuNombre = d.Menu?.Nombre
                    }).ToList()
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
            var RespuestaApi = new ResponseAPI<PedidoCabeceraDTO>();
            try
            {
                var entidad = await _context.PedidoCabeceras
                    .Include(p => p.Evento)
                    .Include(p => p.Vehiculo)
                    .Include(p => p.Detalles)
                    .ThenInclude(d => d.Menu)
                    .FirstOrDefaultAsync(p => p.Id == Cod);
                if (entidad == null)
                {
                    RespuestaApi.EsCorrecto = false;
                    RespuestaApi.Mensaje = "Pedido no encontrado";
                }
                else
                {
                    RespuestaApi.EsCorrecto = true;
                    RespuestaApi.Valor = new PedidoCabeceraDTO
                    {
                        Id = entidad.Id,
                        EventoId = entidad.EventoId,
                        VehiculoId = entidad.VehiculoId,
                        FechaPedido = entidad.FechaPedido,
                        Estado = entidad.Estado,
                        Total = entidad.Total,
                        EventoNombre = entidad.Evento?.NombreEvento,
                        VehiculoPlaca = entidad.Vehiculo?.Placa,
                        Detalles = entidad.Detalles.Select(d => new PedidoDetalleDTO
                        {
                            Id = d.Id,
                            PedidoId = d.PedidoId,
                            MenuId = d.MenuId,
                            Cantidad = d.Cantidad,
                            PrecioUnitario = d.PrecioUnitario,
                            Subtotal = d.Subtotal,
                            MenuNombre = d.Menu?.Nombre
                        }).ToList()
                    };
                    RespuestaApi.Mensaje = "Pedido encontrado";
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
        public async Task<IActionResult> Guardar(PedidoCabeceraDTO dto)
        {
            var RespuestaApi = new ResponseAPI<int>();
            try
            {
                var entidad = new PedidoCabecera
                {
                    EventoId = dto.EventoId,
                    VehiculoId = dto.VehiculoId,
                    FechaPedido = dto.FechaPedido,
                    Estado = dto.Estado,
                    Total = 0
                };

                foreach (var det in dto.Detalles)
                {
                    var menu = await _context.Menus.FindAsync(det.MenuId);
                    var precio = menu?.Precio ?? 0;
                    var subtotal = det.Cantidad * precio;
                    entidad.Detalles.Add(new PedidoDetalle
                    {
                        MenuId = det.MenuId,
                        Cantidad = det.Cantidad,
                        PrecioUnitario = precio,
                        Subtotal = subtotal
                    });
                    entidad.Total += subtotal;
                }

                _context.PedidoCabeceras.Add(entidad);
                await _context.SaveChangesAsync();
                RespuestaApi.EsCorrecto = true;
                RespuestaApi.Valor = entidad.Id;
                RespuestaApi.Mensaje = "Pedido guardado";
            }
            catch (Exception ex)
            {
                RespuestaApi.EsCorrecto = false;
                RespuestaApi.Mensaje = ex.Message;
            }
            return Ok(RespuestaApi);
        }

        [HttpPut("Modificar/{Cod}")]
        public async Task<IActionResult> Modificar(int Cod, PedidoCabeceraDTO dto)
        {
            var RespuestaApi = new ResponseAPI<int>();
            try
            {
                var entidad = await _context.PedidoCabeceras
                    .Include(p => p.Detalles)
                    .FirstOrDefaultAsync(p => p.Id == Cod);
                if (entidad == null)
                {
                    RespuestaApi.EsCorrecto = false;
                    RespuestaApi.Mensaje = "Pedido no encontrado";
                }
                else
                {
                    entidad.EventoId = dto.EventoId;
                    entidad.VehiculoId = dto.VehiculoId;
                    entidad.FechaPedido = dto.FechaPedido;
                    entidad.Estado = dto.Estado;

                    _context.PedidoDetalles.RemoveRange(entidad.Detalles);
                    entidad.Total = 0;

                    foreach (var det in dto.Detalles)
                    {
                        var menu = await _context.Menus.FindAsync(det.MenuId);
                        var precio = menu?.Precio ?? 0;
                        var subtotal = det.Cantidad * precio;
                        entidad.Detalles.Add(new PedidoDetalle
                        {
                            MenuId = det.MenuId,
                            Cantidad = det.Cantidad,
                            PrecioUnitario = precio,
                            Subtotal = subtotal
                        });
                        entidad.Total += subtotal;
                    }

                    await _context.SaveChangesAsync();
                    RespuestaApi.EsCorrecto = true;
                    RespuestaApi.Valor = entidad.Id;
                    RespuestaApi.Mensaje = "Pedido modificado";
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
                var entidad = await _context.PedidoCabeceras
                    .Include(p => p.Detalles)
                    .FirstOrDefaultAsync(p => p.Id == Cod);
                if (entidad == null)
                {
                    RespuestaApi.EsCorrecto = false;
                    RespuestaApi.Mensaje = "Pedido no encontrado";
                }
                else
                {
                    _context.PedidoDetalles.RemoveRange(entidad.Detalles);
                    _context.PedidoCabeceras.Remove(entidad);
                    await _context.SaveChangesAsync();
                    RespuestaApi.EsCorrecto = true;
                    RespuestaApi.Valor = entidad.Id;
                    RespuestaApi.Mensaje = "Pedido eliminado";
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
