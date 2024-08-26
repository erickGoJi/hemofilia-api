using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using biz.main.tecnicah.Repository.Diagnostico;
using dal.main.tecnicah.DBContext;
using dal.main.tecnicah.Repository.Generic;
using System.Globalization;

namespace dal.main.tecnicah.Repository.Diagnostico
{
    public class ResultRepository : GenericRepository<biz.main.tecnicah.Entities.Result>, IResultRepository
    {
        public ResultRepository(db_HemofiliaContext context) : base(context)
        {

        }
    }
}
