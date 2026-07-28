using System.Net.Http.Json;
using X_Libra_Catering.Shared;

namespace X_Libra_Catering.Cliente.Services
{
    public class ServicioVehiculo
    {
        private readonly HttpClient Http;

        public ServicioVehiculo(HttpClient http)
        {
            Http = http;
        }

        public async Task<List<VehiculoDTO>> Lista()
        {
            var Resultado = await Http.GetFromJsonAsync<ResponseAPI<List<VehiculoDTO>>>("api/Vehiculos/Lista");
            if (Resultado!.EsCorrecto)
                return Resultado.Valor!;
            else
                throw new Exception(Resultado.Mensaje);
        }

        public async Task<VehiculoDTO> Buscar(int Cod)
        {
            var Resultado = await Http.GetFromJsonAsync<ResponseAPI<VehiculoDTO>>($"api/Vehiculos/Buscar/{Cod}");
            if (Resultado!.EsCorrecto)
                return Resultado.Valor!;
            else
                throw new Exception(Resultado.Mensaje);
        }

        public async Task<int> Guardar(VehiculoDTO dto)
        {
            var Resultado = await Http.PostAsJsonAsync("api/Vehiculos/Guardar", dto);
            var Respuesta = await Resultado.Content.ReadFromJsonAsync<ResponseAPI<int>>();
            if (Respuesta!.EsCorrecto)
                return Respuesta.Valor;
            else
                throw new Exception(Respuesta.Mensaje);
        }

        public async Task<int> Modificar(int Cod, VehiculoDTO dto)
        {
            var Resultado = await Http.PutAsJsonAsync($"api/Vehiculos/Modificar/{Cod}", dto);
            var Respuesta = await Resultado.Content.ReadFromJsonAsync<ResponseAPI<int>>();
            if (Respuesta!.EsCorrecto)
                return Respuesta.Valor;
            else
                throw new Exception(Respuesta.Mensaje);
        }

        public async Task<int> Eliminar(int Cod)
        {
            var Resultado = await Http.DeleteAsync($"api/Vehiculos/Eliminar/{Cod}");
            var Respuesta = await Resultado.Content.ReadFromJsonAsync<ResponseAPI<int>>();
            if (Respuesta!.EsCorrecto)
                return Respuesta.Valor;
            else
                throw new Exception(Respuesta.Mensaje);
        }
    }
}
