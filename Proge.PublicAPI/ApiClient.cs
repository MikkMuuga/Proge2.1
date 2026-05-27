using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Proge.PublicAPI
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7123/api/");
        }

        public async Task<List<Budget>> List()
        {
            var response = await _httpClient.GetAsync("Budgets");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<Budget>>(content, _jsonOptions);
            return result ?? new List<Budget>();
        }

        public async Task Save(Budget budget)
        {
            HttpResponseMessage response;
            if (budget.Id == 0)
                response = await _httpClient.PostAsJsonAsync("Budgets", budget);
            else
                response = await _httpClient.PutAsJsonAsync("Budgets/" + budget.Id, budget);

            response.EnsureSuccessStatusCode();
        }

        public async Task Delete(int id)
        {
            await _httpClient.DeleteAsync("Budgets/" + id);
        }
    }
}
