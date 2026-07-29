using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using X_Libra_Catering.Server.Data;
using X_Libra_Catering.Server.Models;
using X_Libra_Catering.Shared;
using X_Libra_Catering.Shared.Enums;

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
                    FechaCreacion = p.FechaCreacion,
                    FechaModificacion = p.FechaModificacion,
                    Detalles = p.Detalles.Select(d => new PedidoDetalleDTO
                    {
                        Id = d.Id,
                        PedidoId = d.PedidoId,
                        MenuId = d.MenuId,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario,
                        Subtotal = d.Subtotal,
                        MenuNombre = d.Menu?.Nombre,
                        FechaCreacion = d.FechaCreacion,
                        FechaModificacion = d.FechaModificacion
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
                        FechaCreacion = entidad.FechaCreacion,
                        FechaModificacion = entidad.FechaModificacion,
                        Detalles = entidad.Detalles.Select(d => new PedidoDetalleDTO
                        {
                            Id = d.Id,
                            PedidoId = d.PedidoId,
                            MenuId = d.MenuId,
                            Cantidad = d.Cantidad,
                            PrecioUnitario = d.PrecioUnitario,
                            Subtotal = d.Subtotal,
                            MenuNombre = d.Menu?.Nombre,
                            FechaCreacion = d.FechaCreacion,
                            FechaModificacion = d.FechaModificacion
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

        [HttpPatch("CambiarEstado/{Cod}")]
        public async Task<IActionResult> CambiarEstado(int Cod, [FromBody] string nuevoEstado)
        {
            var RespuestaApi = new ResponseAPI<int>();
            try
            {
                var entidad = await _context.PedidoCabeceras.FindAsync(Cod);
                if (entidad == null)
                {
                    RespuestaApi.EsCorrecto = false;
                    RespuestaApi.Mensaje = "Pedido no encontrado";
                }
                else
                {
                    if (Enum.TryParse<EstadoPedido>(nuevoEstado, out var estado))
                    {
                        entidad.Estado = estado;
                        await _context.SaveChangesAsync();
                        RespuestaApi.EsCorrecto = true;
                        RespuestaApi.Valor = entidad.Id;
                        RespuestaApi.Mensaje = "Estado actualizado";
                    }
                    else
                    {
                        RespuestaApi.EsCorrecto = false;
                        RespuestaApi.Mensaje = "Estado invalido";
                    }
                }
            }
            catch (Exception ex)
            {
                RespuestaApi.EsCorrecto = false;
                RespuestaApi.Mensaje = ex.Message;
            }
            return Ok(RespuestaApi);
        }

        [HttpGet("Qr/{Cod}")]
        public async Task<IActionResult> Qr(int Cod)
        {
            try
            {
                var entidad = await _context.PedidoCabeceras
                    .Include(p => p.Evento)
                    .FirstOrDefaultAsync(p => p.Id == Cod);
                if (entidad == null)
                    return NotFound();

                var contenido = $"Pedido #{entidad.Id} | Evento: {entidad.Evento?.NombreEvento} | Fecha: {entidad.FechaPedido:dd/MM/yyyy} | Total: Bs {entidad.Total:N2}";

                using var qr = new QRCodeGenerator();
                var datos = qr.CreateQrCode(contenido, QRCodeGenerator.ECCLevel.Q);
                using var grafico = new QRCode(datos);
                using var bitmap = grafico.GetGraphic(20);
                using var ms = new MemoryStream();
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;
                return File(ms.ToArray(), "image/png");
            }
            catch (Exception ex)
            {
                return Ok(new ResponseAPI<int> { EsCorrecto = false, Mensaje = ex.Message });
            }
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
