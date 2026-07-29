using System.Net.Http.Json;
using X_Libra_Catering.Shared;
using X_Libra_Catering.Shared.Enums;

namespace X_Libra_Catering.Cliente.Services
{
    public class ServicioPedido
    {
        private readonly HttpClient Http;

        public ServicioPedido(HttpClient http)
        {
            Http = http;
        }

        public async Task<ResultadoPaginado<PedidoCabeceraDTO>> Lista(int pagina = 1, int tamano = 20, string? busqueda = null)
        {
            var url = $"api/Pedidos/Lista?pagina={pagina}&tamano={tamano}&busqueda={Uri.EscapeDataString(busqueda ?? "")}";
            var Resultado = await Http.GetFromJsonAsync<ResponseAPI<ResultadoPaginado<PedidoCabeceraDTO>>>(url);
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

        public async Task CambiarEstado(int id, EstadoPedido estado)
        {
            var response = await Http.PatchAsJsonAsync($"api/Pedidos/CambiarEstado/{id}", estado.ToString());
            var Respuesta = await response.Content.ReadFromJsonAsync<ResponseAPI<int>>();
            if (Respuesta == null || !Respuesta.EsCorrecto)
                throw new Exception(Respuesta?.Mensaje ?? "Error al cambiar estado");
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
