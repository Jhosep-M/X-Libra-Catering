using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X_Libra_Catering.Server.Data;
using X_Libra_Catering.Server.Models;
using X_Libra_Catering.Shared;
using X_Libra_Catering.Shared.Enums;

namespace X_Libra_Catering.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly BdXLibraCateringContext _context;

        public EventosController(BdXLibraCateringContext context)
        {
            _context = context;
        }

        [HttpGet("Kpi")]
        public async Task<IActionResult> Kpi()
        {
            var RespuestaApi = new ResponseAPI<DashboardKpiDTO>();
            try
            {
                var ahora = DateTime.Now;
                var inicioMes = new DateTime(ahora.Year, ahora.Month, 1);

                var total = await _context.Eventos.CountAsync();
                var pendientes = await _context.Eventos.CountAsync(e => e.Estado == EstadoEvento.Pendiente);
                var enPreparacion = await _context.Eventos.CountAsync(e => e.Estado == EstadoEvento.EnPreparacion);
                var completadosMes = await _context.Eventos.CountAsync(e => e.Estado == EstadoEvento.Completado && e.FechaEvento >= inicioMes);

                RespuestaApi.EsCorrecto = true;
                RespuestaApi.Valor = new DashboardKpiDTO
                {
                    TotalEventos = total,
                    Pendientes = pendientes,
                    EnPreparacion = enPreparacion,
                    CompletadosMes = completadosMes
                };
                RespuestaApi.Mensaje = "KPI calculados";
            }
            catch (Exception ex)
            {
                RespuestaApi.EsCorrecto = false;
                RespuestaApi.Mensaje = ex.Message;
            }
            return Ok(RespuestaApi);
        }

        [HttpGet("Lista")]
        public async Task<IActionResult> Lista()
        {
            var RespuestaApi = new ResponseAPI<List<EventoDTO>>();
            try
            {
                var lista = await _context.Eventos.Include(e => e.Cliente).ToListAsync();
                var listaDTO = lista.Select(e => new EventoDTO
                {
                    Id = e.Id,
                    ClienteId = e.ClienteId,
                    NombreEvento = e.NombreEvento,
                    TipoEvento = e.TipoEvento,
                    Estado = e.Estado,
                    FechaEvento = e.FechaEvento,
                    Ubicacion = e.Ubicacion,
                    NumInvitados = e.NumInvitados,
                    ClienteNombre = e.Cliente?.Nombre
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
            var RespuestaApi = new ResponseAPI<EventoDTO>();
            try
            {
                var entidad = await _context.Eventos.Include(e => e.Cliente).FirstOrDefaultAsync(e => e.Id == Cod);
                if (entidad == null)
                {
                    RespuestaApi.EsCorrecto = false;
                    RespuestaApi.Mensaje = "Evento no encontrado";
                }
                else
                {
                    RespuestaApi.EsCorrecto = true;
                    RespuestaApi.Valor = new EventoDTO
                    {
                        Id = entidad.Id,
                        ClienteId = entidad.ClienteId,
                        NombreEvento = entidad.NombreEvento,
                        TipoEvento = entidad.TipoEvento,
                        Estado = entidad.Estado,
                        FechaEvento = entidad.FechaEvento,
                        Ubicacion = entidad.Ubicacion,
                        NumInvitados = entidad.NumInvitados,
                        ClienteNombre = entidad.Cliente?.Nombre
                    };
                    RespuestaApi.Mensaje = "Evento encontrado";
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
        public async Task<IActionResult> Guardar(EventoDTO dto)
        {
            var RespuestaApi = new ResponseAPI<int>();
            try
            {
                var entidad = new Evento
                {
                    ClienteId = dto.ClienteId,
                    NombreEvento = dto.NombreEvento,
                    TipoEvento = dto.TipoEvento,
                    Estado = dto.Estado,
                    FechaEvento = dto.FechaEvento,
                    Ubicacion = dto.Ubicacion,
                    NumInvitados = dto.NumInvitados
                };
                _context.Eventos.Add(entidad);
                await _context.SaveChangesAsync();
                RespuestaApi.EsCorrecto = true;
                RespuestaApi.Valor = entidad.Id;
                RespuestaApi.Mensaje = "Evento guardado";
            }
            catch (Exception ex)
            {
                RespuestaApi.EsCorrecto = false;
                RespuestaApi.Mensaje = ex.Message;
            }
            return Ok(RespuestaApi);
        }

        [HttpPut("Modificar/{Cod}")]
        public async Task<IActionResult> Modificar(int Cod, EventoDTO dto)
        {
            var RespuestaApi = new ResponseAPI<int>();
            try
            {
                var entidad = await _context.Eventos.FindAsync(Cod);
                if (entidad == null)
                {
                    RespuestaApi.EsCorrecto = false;
                    RespuestaApi.Mensaje = "Evento no encontrado";
                }
                else
                {
                    entidad.ClienteId = dto.ClienteId;
                    entidad.NombreEvento = dto.NombreEvento;
                    entidad.TipoEvento = dto.TipoEvento;
                    entidad.Estado = dto.Estado;
                    entidad.FechaEvento = dto.FechaEvento;
                    entidad.Ubicacion = dto.Ubicacion;
                    entidad.NumInvitados = dto.NumInvitados;
                    await _context.SaveChangesAsync();
                    RespuestaApi.EsCorrecto = true;
                    RespuestaApi.Valor = entidad.Id;
                    RespuestaApi.Mensaje = "Evento modificado";
                }
            }
            catch (Exception ex)
            {
                RespuestaApi.EsCorrecto = false;
                RespuestaApi.Mensaje = ex.Message;
            }
            return Ok(RespuestaApi);
        }

        [HttpPut("CambiarEstado/{Cod}")]
        public async Task<IActionResult> CambiarEstado(int Cod, [FromBody] EstadoEvento nuevoEstado)
        {
            var RespuestaApi = new ResponseAPI<int>();
            try
            {
                var entidad = await _context.Eventos.FindAsync(Cod);
                if (entidad == null)
                {
                    RespuestaApi.EsCorrecto = false;
                    RespuestaApi.Mensaje = "Evento no encontrado";
                }
                else
                {
                    entidad.Estado = nuevoEstado;
                    await _context.SaveChangesAsync();
                    RespuestaApi.EsCorrecto = true;
                    RespuestaApi.Valor = entidad.Id;
                    RespuestaApi.Mensaje = $"Estado cambiado a {nuevoEstado}";
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
                var entidad = await _context.Eventos.FindAsync(Cod);
                if (entidad == null)
                {
                    RespuestaApi.EsCorrecto = false;
                    RespuestaApi.Mensaje = "Evento no encontrado";
                }
                else
                {
                    _context.Eventos.Remove(entidad);
                    await _context.SaveChangesAsync();
                    RespuestaApi.EsCorrecto = true;
                    RespuestaApi.Valor = entidad.Id;
                    RespuestaApi.Mensaje = "Evento eliminado";
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
