using biz.main.tecnicah.Repository.Generic;
using biz.main.tecnicah.Repository.RequestSupport;
using dal.main.tecnicah.DBContext;
using dal.main.tecnicah.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.main.tecnicah.Repository.RequestSupport
{
    public class RequestSupportRepository : GenericRepository<biz.main.tecnicah.Entities.RequestSupport>, IRequestSupportRepository
    {
        public RequestSupportRepository(db_HemofiliaContext context) : base(context)
        {
        }

        public async Task<int> GetRequestSupportByBetweenDate(DateTime startDate, DateTime endDate)
        {
            var i = _context.RequestSupports.Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate).GroupBy(g=>g.UserId);
            return i.Count();
        }
    }
}
