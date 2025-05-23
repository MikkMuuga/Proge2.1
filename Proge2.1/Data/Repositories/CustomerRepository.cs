using Microsoft.EntityFrameworkCore;

namespace Proge2._1.Data.Repositories
{
        public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
        {
            public CustomerRepository(ApplicationDbContext context)
                : base(context)
            {
            }
        }

    }

