using Microsoft.AspNetCore.Identity.Data;
using System.Net;
using System.Net.Http.Json;

namespace OrderManagementSystem.Web.Mvc.Services
{
    public class ApiClient
    {
        private readonly HttpClient _client;

        public ApiClient(IHttpClientFactory factory)
        {
            _client= factory.CreateClient("ApiClient");
        }

        public async Task<T?> GetAsync<T>(string url)
        {
            var resp = await _client.GetAsync(url);
            if (resp.StatusCode == HttpStatusCode.Unauthorized) throw new UnauthorizedAccessException();
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<T>();
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse> (string url, TRequest body)
        {
            var resp = await _client.PostAsJsonAsync(url, body);
            if(resp.StatusCode == HttpStatusCode.Unauthorized) throw new UnauthorizedAccessException();
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task PutAsync<TRequest> (string url, TRequest body) 
        {
            var resp = await _client.PutAsJsonAsync(url, body);
            if (resp.StatusCode == HttpStatusCode.Unauthorized) throw new UnauthorizedAccessException();

            resp.EnsureSuccessStatusCode();
        }
    }
}
