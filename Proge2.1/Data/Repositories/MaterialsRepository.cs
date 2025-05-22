namespace Proge2._1.Data.Repositories
{
    public class MaterialsRepository : BaseRepository<Materials>, IMaterialsRepository
    {
        public MaterialsRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }

}
