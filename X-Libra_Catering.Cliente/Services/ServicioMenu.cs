using System.Net.Http.Headers;
using System.Net.Http.Json;
using X_Libra_Catering.Shared;

namespace X_Libra_Catering.Cliente.Services
{
    public class ServicioMenu
    {
        private readonly HttpClient Http;

        public ServicioMenu(HttpClient http)
        {
            Http = http;
        }

        public async Task<List<MenuDTO>> Lista()
        {
            var Resultado = await Http.GetFromJsonAsync<ResponseAPI<List<MenuDTO>>>("api/Menus/Lista");
            if (Resultado!.EsCorrecto)
                return Resultado.Valor!;
            else
                throw new Exception(Resultado.Mensaje);
        }

        public async Task<MenuDTO> Buscar(int Cod)
        {
            var Resultado = await Http.GetFromJsonAsync<ResponseAPI<MenuDTO>>($"api/Menus/Buscar/{Cod}");
            if (Resultado!.EsCorrecto)
                return Resultado.Valor!;
            else
                throw new Exception(Resultado.Mensaje);
        }

        public async Task<int> Guardar(MenuDTO dto)
        {
            var Resultado = await Http.PostAsJsonAsync("api/Menus/Guardar", dto);
            var Respuesta = await Resultado.Content.ReadFromJsonAsync<ResponseAPI<int>>();
            if (Respuesta!.EsCorrecto)
                return Respuesta.Valor;
            else
                throw new Exception(Respuesta.Mensaje);
        }

        public async Task<int> Modificar(int Cod, MenuDTO dto)
        {
            var Resultado = await Http.PutAsJsonAsync($"api/Menus/Modificar/{Cod}", dto);
            var Respuesta = await Resultado.Content.ReadFromJsonAsync<ResponseAPI<int>>();
            if (Respuesta!.EsCorrecto)
                return Respuesta.Valor;
            else
                throw new Exception(Respuesta.Mensaje);
        }

        public async Task<string> SubirImagen(StreamContent archivo, string nombreArchivo)
        {
            using var form = new MultipartFormDataContent();
            archivo.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "archivo",
                FileName = nombreArchivo
            };
            form.Add(archivo, "archivo", nombreArchivo);
            var Resultado = await Http.PostAsync("api/Menus/SubirImagen", form);
            var Respuesta = await Resultado.Content.ReadFromJsonAsync<ResponseAPI<string>>();
            if (Respuesta!.EsCorrecto)
                return Respuesta.Valor!;
            else
                throw new Exception(Respuesta.Mensaje);
        }

        public async Task<int> Eliminar(int Cod)
        {
            var Resultado = await Http.DeleteAsync($"api/Menus/Eliminar/{Cod}");
            var Respuesta = await Resultado.Content.ReadFromJsonAsync<ResponseAPI<int>>();
            if (Respuesta!.EsCorrecto)
                return Respuesta.Valor;
            else
                throw new Exception(Respuesta.Mensaje);
        }
    }
}
