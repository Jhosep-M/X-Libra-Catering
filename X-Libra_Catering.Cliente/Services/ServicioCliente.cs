using System.Net.Http.Json;
using X_Libra_Catering.Shared;

namespace X_Libra_Catering.Cliente.Services
{
    public class ServicioCliente
    {
        private readonly HttpClient Http;

        public ServicioCliente(HttpClient http)
        {
            Http = http;
        }

        public async Task<List<ClienteDTO>> Lista()
        {
            var Resultado = await Http.GetFromJsonAsync<ResponseAPI<List<ClienteDTO>>>("api/Clientes/Lista");
            if (Resultado!.EsCorrecto)
                return Resultado.Valor!;
            else
                throw new Exception(Resultado.Mensaje);
        }

        public async Task<ClienteDTO> Buscar(int Cod)
        {
            var Resultado = await Http.GetFromJsonAsync<ResponseAPI<ClienteDTO>>($"api/Clientes/Buscar/{Cod}");
            if (Resultado!.EsCorrecto)
                return Resultado.Valor!;
            else
                throw new Exception(Resultado.Mensaje);
        }

        public async Task<int> Guardar(ClienteDTO dto)
        {
            var Resultado = await Http.PostAsJsonAsync("api/Clientes/Guardar", dto);
            var Respuesta = await Resultado.Content.ReadFromJsonAsync<ResponseAPI<int>>();
            if (Respuesta!.EsCorrecto)
                return Respuesta.Valor;
            else
                throw new Exception(Respuesta.Mensaje);
        }

        public async Task<int> Modificar(int Cod, ClienteDTO dto)
        {
            var Resultado = await Http.PutAsJsonAsync($"api/Clientes/Modificar/{Cod}", dto);
            var Respuesta = await Resultado.Content.ReadFromJsonAsync<ResponseAPI<int>>();
            if (Respuesta!.EsCorrecto)
                return Respuesta.Valor;
            else
                throw new Exception(Respuesta.Mensaje);
        }

        public async Task<int> Eliminar(int Cod)
        {
            var Resultado = await Http.DeleteAsync($"api/Clientes/Eliminar/{Cod}");
            var Respuesta = await Resultado.Content.ReadFromJsonAsync<ResponseAPI<int>>();
            if (Respuesta!.EsCorrecto)
                return Respuesta.Valor;
            else
                throw new Exception(Respuesta.Mensaje);
        }
    }
}
