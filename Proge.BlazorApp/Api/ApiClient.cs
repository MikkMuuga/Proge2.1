using System.Net.Http.Json;

namespace Proge.BlazorApp
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result<List<Budget>>> List()
        {
            var result = new Result<List<Budget>>();
            try
            {
                result.Value = await _httpClient.GetFromJsonAsync<List<Budget>>("Budgets");
            }
            catch (Exception ex)
            {
                result.AddError("_", ex.Message);
            }
            return result;
        }

        public async Task<Result<Budget>> Get(int id)
        {
            var result = new Result<Budget>();
            try
            {
                result.Value = await _httpClient.GetFromJsonAsync<Budget>("Budgets/" + id);
            }
            catch (Exception ex)
            {
                result.AddError("_", ex.Message);
            }
            return result;
        }

        public async Task<Result> Save(Budget budget)
        {
            HttpResponseMessage response;
            if (budget.Id == 0)
                response = await _httpClient.PostAsJsonAsync("Budgets", budget);
            else
                response = await _httpClient.PutAsJsonAsync("Budgets/" + budget.Id, budget);

            if (!response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Result>();
                return result ?? new Result();
            }
            return new Result();
        }

        public async Task<Result> Delete(int id)
        {
            var result = new Result();
            try
            {
                await _httpClient.DeleteAsync("Budgets/" + id);
            }
            catch (Exception ex)
            {
                result.AddError("_", ex.Message);
            }
            return result;
        }
    }
}