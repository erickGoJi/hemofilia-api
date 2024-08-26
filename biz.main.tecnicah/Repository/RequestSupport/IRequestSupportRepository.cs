using biz.main.tecnicah.Entities;
using biz.main.tecnicah.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biz.main.tecnicah.Repository.RequestSupport
{
    public interface IRequestSupportRepository : IGenericRepository<Entities.RequestSupport>
    {
        public Task<int> GetRequestSupportByBetweenDate(DateTime startDate, DateTime endDate);

    }
}
