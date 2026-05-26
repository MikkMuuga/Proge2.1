using Proge2._1.Data;
using Proge2._1.Search;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proge2._1.Services.Interfaces
{
    public interface IMaterialService
    {
        Task<Materials> GetMaterialById(int id);
        Task AddMaterial(Materials material);
        Task UpdateMaterial(Materials material);
        Task DeleteMaterial(int id);
        Task<bool> MaterialExists(int id);
        Task Save(Materials material);
        Task<PagedResult<Materials>> GetPagedMaterials(int page, int pageSize);
        Task<PagedResult<Materials>> GetPagedMaterials(int page, int pageSize, MaterialSearch search);
    }
}
