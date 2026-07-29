using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X_Libra_Catering.Server.Data;
using X_Libra_Catering.Shared;
using X_Libra_Catering.Shared.Enums;

namespace X_Libra_Catering.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly BdXLibraCateringContext _context;

        public DashboardController(BdXLibraCateringContext context)
        {
            _context = context;
        }

        [HttpGet("Datos")]
        public async Task<IActionResult> Datos()
        {
            var RespuestaApi = new ResponseAPI<DashboardCompletoDTO>();
            try
            {
                var ahora = DateTime.Now;
                var inicioMes = new DateTime(ahora.Year, ahora.Month, 1);

                var totalEventos = await _context.Eventos.CountAsync();
                var eventosPendientes = await _context.Eventos.CountAsync(e => e.Estado == EstadoEvento.Pendiente);
                var eventosCompletados = await _context.Eventos.CountAsync(e => e.Estado == EstadoEvento.Completado);
                var totalPedidos = await _context.PedidoCabeceras.CountAsync();
                var pedidosEntregados = await _context.PedidoCabeceras.CountAsync(p => p.Estado == EstadoPedido.Entregado);
                var vehiculosDisponibles = await _context.Vehiculos.CountAsync(v => v.Activo && v.Disponible);
                var vehiculosTotales = await _context.Vehiculos.CountAsync(v => v.Activo);
                var ingresosMes = await _context.PedidoCabeceras
                    .Where(p => p.Estado == EstadoPedido.Entregado && p.FechaPedido >= inicioMes)
                    .SumAsync(p => (decimal?)p.Total) ?? 0;

                var eventosPorMes = await _context.Eventos
                    .GroupBy(e => new { e.FechaEvento.Year, e.FechaEvento.Month })
                    .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                    .Select(g => new DatoGrafico
                    {
                        Label = $"{g.Key.Year}-{g.Key.Month:D2}",
                        Valor = g.Count()
                    })
                    .ToListAsync();

                var pedidosPorEstado = await _context.PedidoCabeceras
                    .GroupBy(p => p.Estado)
                    .Select(g => new DatoGrafico
                    {
                        Label = g.Key.ToString(),
                        Valor = g.Count()
                    })
                    .ToListAsync();

                var ingresosPorMes = await _context.PedidoCabeceras
                    .Where(p => p.Estado == EstadoPedido.Entregado)
                    .GroupBy(p => new { p.FechaPedido.Year, p.FechaPedido.Month })
                    .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                    .Select(g => new DatoGrafico
                    {
                        Label = $"{g.Key.Year}-{g.Key.Month:D2}",
                        Valor = g.Sum(p => p.Total)
                    })
                    .ToListAsync();

                RespuestaApi.EsCorrecto = true;
                RespuestaApi.Valor = new DashboardCompletoDTO
                {
                    TotalEventos = totalEventos,
                    EventosPendientes = eventosPendientes,
                    EventosCompletados = eventosCompletados,
                    TotalPedidos = totalPedidos,
                    PedidosEntregados = pedidosEntregados,
                    VehiculosDisponibles = vehiculosDisponibles,
                    VehiculosTotales = vehiculosTotales,
                    IngresosMes = ingresosMes,
                    EventosPorMes = eventosPorMes,
                    PedidosPorEstado = pedidosPorEstado,
                    IngresosPorMes = ingresosPorMes
                };
                RespuestaApi.Mensaje = "Dashboard cargado";
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
