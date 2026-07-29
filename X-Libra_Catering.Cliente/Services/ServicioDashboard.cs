using System.Net.Http.Json;
using X_Libra_Catering.Shared;

namespace X_Libra_Catering.Cliente.Services
{
    public class ServicioDashboard
    {
        private readonly HttpClient Http;

        public ServicioDashboard(HttpClient http)
        {
            Http = http;
        }

        public async Task<DashboardCompletoDTO> ObtenerDatos()
        {
            var Resultado = await Http.GetFromJsonAsync<ResponseAPI<DashboardCompletoDTO>>("api/Dashboard/Datos");
            if (Resultado!.EsCorrecto)
                return Resultado.Valor!;
            else
                throw new Exception(Resultado.Mensaje);
        }
    }
}
