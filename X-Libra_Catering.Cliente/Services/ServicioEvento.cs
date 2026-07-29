using System.Net.Http.Json;
using X_Libra_Catering.Shared;
using X_Libra_Catering.Shared.Enums;

namespace X_Libra_Catering.Cliente.Services
{
    public class ServicioEvento
    {
        private readonly HttpClient Http;

        public ServicioEvento(HttpClient http)
        {
            Http = http;
        }

        public async Task<List<EventoDTO>> Lista()
        {
            var Resultado = await Http.GetFromJsonAsync<ResponseAPI<List<EventoDTO>>>("api/Eventos/Lista");
            if (Resultado!.EsCorrecto)
                return Resultado.Valor!;
            else
                throw new Exception(Resultado.Mensaje);
        }

        public async Task<DashboardKpiDTO> ObtenerKpi()
        {
            var Resultado = await Http.GetFromJsonAsync<ResponseAPI<DashboardKpiDTO>>("api/Eventos/Kpi");
            if (Resultado!.EsCorrecto)
                return Resultado.Valor!;
            else
                throw new Exception(Resultado.Mensaje);
        }

        public async Task<EventoDTO> Buscar(int Cod)
        {
            var Resultado = await Http.GetFromJsonAsync<ResponseAPI<EventoDTO>>($"api/Eventos/Buscar/{Cod}");
            if (Resultado!.EsCorrecto)
                return Resultado.Valor!;
            else
                throw new Exception(Resultado.Mensaje);
        }

        public async Task<int> Guardar(EventoDTO dto)
        {
            var Resultado = await Http.PostAsJsonAsync("api/Eventos/Guardar", dto);
            var Respuesta = await Resultado.Content.ReadFromJsonAsync<ResponseAPI<int>>();
            if (Respuesta!.EsCorrecto)
                return Respuesta.Valor;
            else
                throw new Exception(Respuesta.Mensaje);
        }

        public async Task<int> Modificar(int Cod, EventoDTO dto)
        {
            var Resultado = await Http.PutAsJsonAsync($"api/Eventos/Modificar/{Cod}", dto);
            var Respuesta = await Resultado.Content.ReadFromJsonAsync<ResponseAPI<int>>();
            if (Respuesta!.EsCorrecto)
                return Respuesta.Valor;
            else
                throw new Exception(Respuesta.Mensaje);
        }

        public async Task<int> CambiarEstado(int Cod, EstadoEvento nuevoEstado)
        {
            var Resultado = await Http.PutAsJsonAsync($"api/Eventos/CambiarEstado/{Cod}", nuevoEstado);
            var Respuesta = await Resultado.Content.ReadFromJsonAsync<ResponseAPI<int>>();
            if (Respuesta!.EsCorrecto)
                return Respuesta.Valor;
            else
                throw new Exception(Respuesta.Mensaje);
        }

        public async Task<byte[]> ExportarPdf(int Cod)
        {
            var Resultado = await Http.GetAsync($"api/Eventos/ExportarPdf/{Cod}");
            if (Resultado.IsSuccessStatusCode)
                return await Resultado.Content.ReadAsByteArrayAsync();
            else
                throw new Exception("Error al exportar PDF");
        }

        public async Task<int> Eliminar(int Cod)
        {
            var Resultado = await Http.DeleteAsync($"api/Eventos/Eliminar/{Cod}");
            var Respuesta = await Resultado.Content.ReadFromJsonAsync<ResponseAPI<int>>();
            if (Respuesta!.EsCorrecto)
                return Respuesta.Valor;
            else
                throw new Exception(Respuesta.Mensaje);
        }
    }
}
