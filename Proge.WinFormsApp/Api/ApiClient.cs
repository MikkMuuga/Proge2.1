using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace Proge.WinFormsApp.Api
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7123/api/");
        }

        public async Task<Result<List<Budget>>> List()
        {
            var result = new Result<List<Budget>>();
            try
            {
                result.Value = await _httpClient.GetFromJsonAsync<List<Budget>>("Budgets");
            }
            catch (HttpRequestException ex)
            {
                result.Error = ex.HttpRequestError == HttpRequestError.ConnectionError
                    ? "Ei saa serveriga ühendust. Palun proovi hiljem uuesti."
                    : ex.Message;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task Save(Budget budget)
        {
            if (budget.Id == 0)
                await _httpClient.PostAsJsonAsync("Budgets", budget);
            else
                await _httpClient.PutAsJsonAsync("Budgets/" + budget.Id, budget);
        }

        public async Task Delete(int id)
        {
            await _httpClient.DeleteAsync("Budgets/" + id);
        }
    }
}
