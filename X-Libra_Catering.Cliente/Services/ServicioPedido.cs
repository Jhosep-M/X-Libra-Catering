using System.Net.Http.Json;
using X_Libra_Catering.Shared;

namespace X_Libra_Catering.Cliente.Services
{
    public class ServicioPedido
    {
        private readonly HttpClient Http;

        public ServicioPedido(HttpClient http)
        {
            Http = http;
        }

        public async Task<List<PedidoCabeceraDTO>> Lista()
        {
            var Resultado = await Http.GetFromJsonAsync<ResponseAPI<List<PedidoCabeceraDTO>>>("api/Pedidos/Lista");
            if (Resultado!.EsCorrecto)
                return Resultado.Valor!;
            else
                throw new Exception(Resultado.Mensaje);
        }

        public async Task<PedidoCabeceraDTO> Buscar(int Cod)
        {
            var Resultado = await Http.GetFromJsonAsync<ResponseAPI<PedidoCabeceraDTO>>($"api/Pedidos/Buscar/{Cod}");
            if (Resultado!.EsCorrecto)
                return Resultado.Valor!;
            else
                throw new Exception(Resultado.Mensaje);
        }

        public async Task<int> Guardar(PedidoCabeceraDTO dto)
        {
            var Resultado = await Http.PostAsJsonAsync("api/Pedidos/Guardar", dto);
            var Respuesta = await Resultado.Content.ReadFromJsonAsync<ResponseAPI<int>>();
            if (Respuesta!.EsCorrecto)
                return Respuesta.Valor;
            else
                throw new Exception(Respuesta.Mensaje);
        }

        public async Task<int> Modificar(int Cod, PedidoCabeceraDTO dto)
        {
            var Resultado = await Http.PutAsJsonAsync($"api/Pedidos/Modificar/{Cod}", dto);
            var Respuesta = await Resultado.Content.ReadFromJsonAsync<ResponseAPI<int>>();
            if (Respuesta!.EsCorrecto)
                return Respuesta.Valor;
            else
                throw new Exception(Respuesta.Mensaje);
        }

        public async Task<int> Eliminar(int Cod)
        {
            var Resultado = await Http.DeleteAsync($"api/Pedidos/Eliminar/{Cod}");
            var Respuesta = await Resultado.Content.ReadFromJsonAsync<ResponseAPI<int>>();
            if (Respuesta!.EsCorrecto)
                return Respuesta.Valor;
            else
                throw new Exception(Respuesta.Mensaje);
        }
    }
}
