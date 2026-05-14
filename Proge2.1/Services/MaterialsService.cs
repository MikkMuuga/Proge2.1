using Proge2._1.Data.Repositories;
using Proge2._1.Data;
using Proge2._1.Services.Interfaces;
using Proge2._1.Search;
using Proge2._1.Extensions;

public class MaterialService : IMaterialService
{
    private readonly IUnitOfWork _unitOfWork;

    public MaterialService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<Materials>> List(int page, int pageSize, MaterialSearch search = null)
    {
        search ??= new MaterialSearch();

        var query = _unitOfWork.MaterialsRepository.GetQueryable();

        if (!string.IsNullOrWhiteSpace(search.Unit))
            query = query.Where(m => m.Unit.Contains(search.Unit));

        if (!string.IsNullOrWhiteSpace(search.Seller))
            query = query.Where(m => m.Seller.Contains(search.Seller));

        if (search.MinPrice.HasValue)
            query = query.Where(m => m.Price >= search.MinPrice.Value);

        if (search.MaxPrice.HasValue)
            query = query.Where(m => m.Price <= search.MaxPrice.Value);

        return await query.OrderBy(m => m.Id).GetPagedAsync(page, pageSize);
    }

    public async Task<PagedResult<Materials>> GetPagedMaterials(int page, int pageSize)
    {
        return await List(page, pageSize);
    }

    public async Task<PagedResult<Materials>> GetPagedMaterials(int page, int pageSize, MaterialSearch search)
    {
        return await List(page, pageSize, search);
    }

    public async Task<Materials> GetMaterialById(int id)
    {
        return await _unitOfWork.MaterialsRepository.GetByIdAsync(id);
    }

    public async Task AddMaterial(Materials material)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.MaterialsRepository.AddAsync(material);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateMaterial(Materials material)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.MaterialsRepository.UpdateAsync(material);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteMaterial(int id)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.MaterialsRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> MaterialExists(int id)
    {
        return await _unitOfWork.MaterialsRepository.ExistsAsync(id);
    }
}