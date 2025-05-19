using Proge2._1.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proge2._1.Services.Interfaces
{
    public interface IMaterialService
    {
        Task<PagedResult<Materials>> GetPagedMaterials(int page, int pageSize);
        Task<Materials> GetMaterialById(int id);
        Task AddMaterial(Materials material);
        Task UpdateMaterial(Materials material);
        Task DeleteMaterial(int id);
        Task<bool> MaterialExists(int id);
    }
}
