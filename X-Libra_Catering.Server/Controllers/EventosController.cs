using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
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
        public async Task<IActionResult> Lista([FromQuery] int pagina = 1, [FromQuery] int tamano = 20, [FromQuery] string? busqueda = null)
        {
            var RespuestaApi = new ResponseAPI<ResultadoPaginado<EventoDTO>>();
            try
            {
                var query = _context.Eventos.Where(e => e.Activo).Include(e => e.Cliente).AsQueryable();

                if (!string.IsNullOrWhiteSpace(busqueda))
                    query = query.Where(e =>
                        e.NombreEvento.Contains(busqueda) ||
                        e.Ubicacion.Contains(busqueda) ||
                        (e.Cliente != null && e.Cliente.Nombre.Contains(busqueda)));

                var total = await query.CountAsync();

                var items = await query
                    .OrderBy(e => e.FechaEvento)
                    .Skip((pagina - 1) * tamano)
                    .Take(tamano)
                    .Select(e => new EventoDTO
                    {
                        Id = e.Id,
                        ClienteId = e.ClienteId,
                        NombreEvento = e.NombreEvento,
                        TipoEvento = e.TipoEvento,
                        Estado = e.Estado,
                        FechaEvento = e.FechaEvento,
                        Ubicacion = e.Ubicacion,
                        NumInvitados = e.NumInvitados,
                        ClienteNombre = e.Cliente!.Nombre,
                        FechaCreacion = e.FechaCreacion,
                        FechaModificacion = e.FechaModificacion
                    })
                    .ToListAsync();

                RespuestaApi.EsCorrecto = true;
                RespuestaApi.Valor = new ResultadoPaginado<EventoDTO>
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
                        ClienteNombre = entidad.Cliente?.Nombre,
                        FechaCreacion = entidad.FechaCreacion,
                        FechaModificacion = entidad.FechaModificacion
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
                RespuestaApi.Mensaje = $"{ex.Message} | Inner: {ex.InnerException?.Message}";
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
                RespuestaApi.Mensaje = $"{ex.Message} | Inner: {ex.InnerException?.Message}";
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
                RespuestaApi.Mensaje = $"{ex.Message} | Inner: {ex.InnerException?.Message}";
            }
            return Ok(RespuestaApi);
        }

        [HttpGet("ExportarPdf/{Cod}")]
        public async Task<IActionResult> ExportarPdf(int Cod)
        {
            try
            {
                var entidad = await _context.Eventos.Include(e => e.Cliente).FirstOrDefaultAsync(e => e.Id == Cod);
                if (entidad == null)
                    return NotFound(new ResponseAPI<int> { EsCorrecto = false, Mensaje = "Evento no encontrado" });

                QuestPDF.Settings.License = LicenseType.Community;

                var pdf = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(40);
                        page.DefaultTextStyle(x => x.FontSize(12));

                        page.Header().Column(col =>
                        {
                            col.Item().Text("X-Libra Catering")
                                .SemiBold().FontSize(24).FontColor(Colors.Blue.Darken3);
                            col.Item().Text("Resumen de Evento")
                                .FontSize(14).FontColor(Colors.Grey.Darken2);
                            col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                        });

                        page.Content().PaddingVertical(20).Column(col =>
                        {
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(c =>
                                {
                                    c.ConstantColumn(120);
                                    c.RelativeColumn();
                                });

                                void Fila(string label, string? value)
                                {
                                    table.Cell().Text(label).SemiBold().FontColor(Colors.Grey.Darken2);
                                    table.Cell().Text(value ?? "-");
                                }

                                Fila("Evento:", entidad.NombreEvento);
                                Fila("Cliente:", entidad.Cliente?.Nombre);
                                Fila("Tipo:", entidad.TipoEvento.ToString());
                                Fila("Estado:", entidad.Estado.ToString());
                                Fila("Fecha:", entidad.FechaEvento.ToString("dd/MM/yyyy HH:mm"));
                                Fila("Ubicacion:", entidad.Ubicacion);
                                Fila("Invitados:", entidad.NumInvitados.ToString());
                            });
                        });

                        page.Footer().AlignCenter().Text(t =>
                        {
                            t.Span("Generado el ");
                            t.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm")).SemiBold();
                            t.Span(" | X-Libra Catering");
                        });
                    });
                }).GeneratePdf();

                return File(pdf, "application/pdf", $"Evento_{entidad.NombreEvento}_{entidad.Id}.pdf");
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
                var entidad = await _context.Eventos.FindAsync(Cod);
                if (entidad == null)
                {
                    RespuestaApi.EsCorrecto = false;
                    RespuestaApi.Mensaje = "Evento no encontrado";
                }
                else
                {
                    entidad.Activo = false;
                    entidad.FechaModificacion = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    RespuestaApi.EsCorrecto = true;
                    RespuestaApi.Valor = entidad.Id;
                    RespuestaApi.Mensaje = "Evento eliminado";
                }
            }
            catch (Exception ex)
            {
                RespuestaApi.EsCorrecto = false;
                RespuestaApi.Mensaje = $"{ex.Message} | Inner: {ex.InnerException?.Message}";
            }
            return Ok(RespuestaApi);
        }
    }
}
