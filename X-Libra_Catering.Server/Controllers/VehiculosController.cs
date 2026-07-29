using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X_Libra_Catering.Server.Data;
using X_Libra_Catering.Server.Models;
using X_Libra_Catering.Shared;

namespace X_Libra_Catering.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculosController : ControllerBase
    {
        private readonly BdXLibraCateringContext _context;

        public VehiculosController(BdXLibraCateringContext context)
        {
            _context = context;
        }

        [HttpGet("Lista")]
        public async Task<IActionResult> Lista()
        {
            var RespuestaApi = new ResponseAPI<List<VehiculoDTO>>();
            try
            {
                var lista = await _context.Vehiculos.Where(v => v.Activo).ToListAsync();
                    var listaDTO = lista.Select(v => new VehiculoDTO
                    {
                        Id = v.Id,
                        Marca = v.Marca,
                        Modelo = v.Modelo,
                        Placa = v.Placa,
                        CapacidadKg = v.CapacidadKg,
                        TieneRefrigeracion = v.TieneRefrigeracion,
                        Disponible = v.Disponible,
                        Direccion = v.Direccion,
                        Latitud = v.Latitud,
                        Longitud = v.Longitud,
                        FechaCreacion = v.FechaCreacion,
                        FechaModificacion = v.FechaModificacion
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
            var RespuestaApi = new ResponseAPI<VehiculoDTO>();
            try
            {
                var entidad = await _context.Vehiculos.FindAsync(Cod);
                if (entidad == null)
                {
                    RespuestaApi.EsCorrecto = false;
                    RespuestaApi.Mensaje = "Vehiculo no encontrado";
                }
                else
                {
                    RespuestaApi.EsCorrecto = true;
                    RespuestaApi.Valor = new VehiculoDTO
                    {
                        Id = entidad.Id,
                        Marca = entidad.Marca,
                        Modelo = entidad.Modelo,
                        Placa = entidad.Placa,
                        CapacidadKg = entidad.CapacidadKg,
                        TieneRefrigeracion = entidad.TieneRefrigeracion,
                        Disponible = entidad.Disponible,
                        Direccion = entidad.Direccion,
                        Latitud = entidad.Latitud,
                        Longitud = entidad.Longitud,
                        FechaCreacion = entidad.FechaCreacion,
                        FechaModificacion = entidad.FechaModificacion
                    };
                    RespuestaApi.Mensaje = "Vehiculo encontrado";
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
        public async Task<IActionResult> Guardar(VehiculoDTO dto)
        {
            var RespuestaApi = new ResponseAPI<int>();
            try
            {
                var entidad = new Vehiculo
                {
                    Marca = dto.Marca,
                    Modelo = dto.Modelo,
                    Placa = dto.Placa,
                    CapacidadKg = dto.CapacidadKg,
                    TieneRefrigeracion = dto.TieneRefrigeracion,
                    Disponible = dto.Disponible,
                    Direccion = dto.Direccion,
                    Latitud = dto.Latitud,
                    Longitud = dto.Longitud
                };
                _context.Vehiculos.Add(entidad);
                await _context.SaveChangesAsync();
                RespuestaApi.EsCorrecto = true;
                RespuestaApi.Valor = entidad.Id;
                RespuestaApi.Mensaje = "Vehiculo guardado";
            }
            catch (Exception ex)
            {
                RespuestaApi.EsCorrecto = false;
                RespuestaApi.Mensaje = ex.Message;
            }
            return Ok(RespuestaApi);
        }

        [HttpPut("Modificar/{Cod}")]
        public async Task<IActionResult> Modificar(int Cod, VehiculoDTO dto)
        {
            var RespuestaApi = new ResponseAPI<int>();
            try
            {
                var entidad = await _context.Vehiculos.FindAsync(Cod);
                if (entidad == null)
                {
                    RespuestaApi.EsCorrecto = false;
                    RespuestaApi.Mensaje = "Vehiculo no encontrado";
                }
                else
                {
                    entidad.Marca = dto.Marca;
                    entidad.Modelo = dto.Modelo;
                    entidad.Placa = dto.Placa;
                    entidad.CapacidadKg = dto.CapacidadKg;
                    entidad.TieneRefrigeracion = dto.TieneRefrigeracion;
                    entidad.Disponible = dto.Disponible;
                    entidad.Direccion = dto.Direccion;
                    entidad.Latitud = dto.Latitud;
                    entidad.Longitud = dto.Longitud;
                    await _context.SaveChangesAsync();
                    RespuestaApi.EsCorrecto = true;
                    RespuestaApi.Valor = entidad.Id;
                    RespuestaApi.Mensaje = "Vehiculo modificado";
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
                var entidad = await _context.Vehiculos.FindAsync(Cod);
                if (entidad == null)
                {
                    RespuestaApi.EsCorrecto = false;
                    RespuestaApi.Mensaje = "Vehiculo no encontrado";
                }
                else
                {
                    entidad.Activo = false;
                    entidad.FechaModificacion = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    RespuestaApi.EsCorrecto = true;
                    RespuestaApi.Valor = entidad.Id;
                    RespuestaApi.Mensaje = "Vehiculo eliminado";
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
