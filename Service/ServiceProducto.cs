using AppFirebase.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AppFirebase.Service
{
    public class ServiceProducto
    {
        private readonly HttpClient _httpClient;

        public ServiceProducto()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://netmauiapp-e2ab9-default-rtdb.firebaseio.com/");
        }

        public async Task<List<Producto>> GetProductos()
        {
            try
            {
                var result = await _httpClient.GetStringAsync("productos.json");
                var productos = JsonConvert.DeserializeObject<Dictionary<string, Producto>>(result);

                if (productos == null)
                {
                    return new List<Producto>();
                }

                foreach (var item in productos)
                {
                    item.Value.Id = item.Key;
                }

                return productos.Values.ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al obtener productos: {ex.Message}");
                return new List<Producto>();
            }
        }

        public async Task CreateProducto(Producto producto)
        {
            if (producto == null)
            {
                Debug.WriteLine("Producto no válido.");
                throw new ArgumentNullException(nameof(producto));
            }

            var json = JsonConvert.SerializeObject(producto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            Debug.WriteLine("Crear producto, antes de enviar la solicitud POST.");
            var result = await _httpClient.PostAsync("productos.json", content);
            if (!result.IsSuccessStatusCode)
            {

            }
            Debug.WriteLine("Producto creado con éxito");
        }

        public async Task UpdateProducto(string id, object producto)
        {
            var json = JsonConvert.SerializeObject(producto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PutAsync($"productos/{id}.json", content);
        }

        public async Task DeleteProducto(string id)
        {
            await _httpClient.DeleteAsync($"productos/{id}.json");
        }
    }
}
