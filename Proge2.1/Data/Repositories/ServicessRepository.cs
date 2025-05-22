namespace Proge2._1.Data.Repositories
{
    public class ServicessRepository : BaseRepository<Servicess>
        , IServicessRepository
    {
        public ServicessRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }

}
