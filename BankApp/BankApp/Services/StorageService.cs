using BankApp.Services;
using Microsoft.JSInterop;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BankApp.Interfaces
{
    public class StorageServices : IStorageService
    {
        private readonly IJSRuntime _jsRuntime;

        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() }
            
        };

        public StorageServices(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SetItemAsync<T>(string key, T value)
        {
            var json = JsonSerializer.Serialize(value, _jsonSerializerOptions);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, json);
        }

        public async Task<T?> GetItemAsync<T>(string key)
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);

            if (string.IsNullOrWhiteSpace(json))
                return default;

            return JsonSerializer.Deserialize<T>(json, _jsonSerializerOptions);
        }
    }
}
