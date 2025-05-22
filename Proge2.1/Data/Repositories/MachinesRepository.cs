namespace Proge2._1.Data.Repositories
{
    public class MachinesRepository : BaseRepository<Machines>, IMachinesRepository
    {
        public MachinesRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }

}
