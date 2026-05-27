using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proge.PublicAPI
{
    public interface IApiClient
    {
        Task<List<Budget>> List();
        Task Save(Budget budget);
        Task Delete(int id);
    }
}
