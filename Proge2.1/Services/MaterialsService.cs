using Proge2._1.Data.Repositories;
using Proge2._1.Data;
using Proge2._1.Services.Interfaces;

public class MaterialService : IMaterialService
{
    private readonly IUnitOfWork _unitOfWork;

    public MaterialService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<Materials>> GetPagedMaterials(int page, int pageSize)
    {
        return await _unitOfWork.MaterialsRepository.GetPagedAsync(page, pageSize);
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