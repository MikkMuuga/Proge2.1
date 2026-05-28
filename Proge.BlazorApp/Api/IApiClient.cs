
namespace Proge.BlazorApp
{
    public interface IApiClient
    {
        Task<Result<List<Budget>>> List();
        Task<Result<Budget>> Get(int id);
        Task<Result> Save(Budget budget);
        Task<Result> Delete(int id);
    }
}
